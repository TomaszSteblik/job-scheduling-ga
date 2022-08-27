using System;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Autofac;
using AutoMapper;
using GeneticAlgorithm.Infrastructure;
using GeneticAlgorithm.Infrastructure.DependencyInjection;
using GeneticAlgorithm.Models;
using GeneticAlgorithm.Models.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Scheduling.Models;

namespace Scheduling.ViewModels;

public class AlgorithmParametersViewModel : ViewModelBase, IActivatableViewModel
{
    public AlgorithmSettings Settings { get; }
    public static IEnumerable<Crossover> CrossoverValues => Enum.GetValues<Crossover>();
    public static IEnumerable<Selection> SelectionValues => Enum.GetValues<Selection>();
    public static IEnumerable<Elimination> EliminationValues => Enum.GetValues<Elimination>();
    public static IEnumerable<Mutation> MutationValues => Enum.GetValues<Mutation>();
    public ReactiveCommand<Unit, Unit> RunGaCommand { get; }
    
    [Reactive] 
    public Chromosome Result { get; set; }

    [Reactive] 
    public IEnumerable<object> Values { get; set; }

    public AlgorithmParametersViewModel(AlgorithmSettings settings, IMapper mapper)
    {
        Settings = settings;
        Activator = new ViewModelActivator();
        settings.PopulationSize = 100;
        settings.ChildrenCount = 10;
        settings.ParentsPerChild = 2;
        settings.EpochsCount = 100;
        settings.MutationProbability = 0.01;
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            /* handle activation */
            Disposable
                .Create(() => { /* handle deactivation */ })
                .DisposeWith(disposables);
        });
        RunGaCommand = ReactiveCommand.Create(() =>
        {
            var builder = new ContainerBuilder();
            var param = mapper.Map<Parameters>(Settings);
            builder.RegisterInstance(param).As<Parameters>();
            builder.RegisterModule(new GeneticAlgorithmModule());
            var container = builder.Build();
            var machines = new[]
            {
                new Machine() {Name = "Machine1", RequiredQualification = Qualification.Milling},
                new Machine() {Name = "Machine2", RequiredQualification = Qualification.Sawing}
            };
            var result = container.Resolve<Algorithm>().Run(machines,
                new[]
                {
                    new Person()
                    {
                        Id = 0,
                        Name = "Joe",
                        Surname = "Doe",
                        Qualifications = new List<Qualification> {Qualification.Milling}
                    },
                    new Person()
                    {
                        Id = 1,
                        Name = "Janusz",
                        Surname = "Kowalski",
                        Qualifications = new List<Qualification> {Qualification.Sawing}
                    }
                },
                Settings.PopulationSize);
            Result = result;
            
            
            var assemblyName = new AssemblyName
            {
                Name = "DynamicObject"
            };
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            
            var modBuilder = asmBuilder.DefineDynamicModule("DynamicModule");
            
            var typeBuilder = modBuilder.DefineType("ScheduledDay",TypeAttributes.Public);
            
            foreach (var machine in machines)
            {
                var propertyBuilder = typeBuilder.DefineProperty(machine.Name,PropertyAttributes.None,CallingConventions.HasThis,
                    typeof(string), Type.EmptyTypes);
                
                //define backing field
                
                var backingFieldBuilder = typeBuilder.DefineField($"_machine.Name",
                    typeof(string),
                    FieldAttributes.Private);

                
                //define getter
                var getterBuilder = typeBuilder.DefineMethod($"{machine.Name}_getter",
                    MethodAttributes.Public, typeof(string), Type.EmptyTypes);

        
                var getterIl = getterBuilder.GetILGenerator();
                getterIl.Emit(OpCodes.Ldarg_0);
                getterIl.Emit(OpCodes.Ldfld, backingFieldBuilder);
                getterIl.Emit(OpCodes.Ret);
                
                propertyBuilder.SetGetMethod(getterBuilder);
                
                // Define the "set" accessor method for CustomerName.
                var setterBuilder =
                    typeBuilder.DefineMethod($"{machine.Name}_getter",
                        MethodAttributes.Public,
                        null,
                        new Type[] { typeof(string) });

                var setterIl = setterBuilder.GetILGenerator();

                setterIl.Emit(OpCodes.Ldarg_0);
                setterIl.Emit(OpCodes.Ldarg_1);
                setterIl.Emit(OpCodes.Stfld, backingFieldBuilder);
                setterIl.Emit(OpCodes.Ret);
                
                propertyBuilder.SetSetMethod(setterBuilder);
            }

            var type = typeBuilder.CreateType();
            
            var scheduledDaysList = new List<object>();
            foreach (var day in result.Value)
            {
                var obj = asmBuilder.CreateInstance(type.FullName);
                for (var i = 0; i < machines.Length; i++)
                {
                    var propInfo = type.GetProperty(machines[i].Name);
                    propInfo.SetValue(obj, day[i].Id.ToString());
                }
                

               
                scheduledDaysList.Add(obj);
            }

            Values = scheduledDaysList;

        });
    }

    public ViewModelActivator Activator { get; }

}