using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using AutoMapper;
using GeneticAlgorithm.Models;
using Scheduling.Models;

namespace Scheduling.MapperProfiles;

public class AlgorithmResultProfile : Profile
{
    public AlgorithmResultProfile()
    {
        CreateMap<Result, AlgorithmResult>()
            .ForMember(x=>x.Schedule, 
                opt => opt.MapFrom(result => GenerateScheduleFromChromosome(result.Chromosome, result.Machines)));
    }

    private static IEnumerable<object> GenerateScheduleFromChromosome(Chromosome chromosome, Machine[] machines)
    {
        var assemblyName = new AssemblyName
            {
                Name = "DynamicObject"
            };
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            
            var modBuilder = asmBuilder.DefineDynamicModule("DynamicModule");
            
            var typeBuilder = modBuilder.DefineType("ScheduledDay",TypeAttributes.Public);

            for (var machineNumber = 0; machineNumber < machines.Length; machineNumber++)
            {
                var machine = machines[machineNumber];
                var propertyBuilder = typeBuilder.DefineProperty(machine.Name ?? $"Unknown_{machineNumber}", PropertyAttributes.None,
                    CallingConventions.HasThis,
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
                        new[] {typeof(string)});

                var setterIl = setterBuilder.GetILGenerator();

                setterIl.Emit(OpCodes.Ldarg_0);
                setterIl.Emit(OpCodes.Ldarg_1);
                setterIl.Emit(OpCodes.Stfld, backingFieldBuilder);
                setterIl.Emit(OpCodes.Ret);

                propertyBuilder.SetSetMethod(setterBuilder);
            }

            var type = typeBuilder.CreateType() ?? throw new Exception("Creating temporary type failed during mapping");

            
            var scheduledDaysList = new List<object>();
            foreach (var day in chromosome.Value)
            {
                var obj = asmBuilder.CreateInstance(type.FullName ?? 
                                                    throw new Exception("Missing namespace during creation ")) 
                          ?? throw new Exception("Failed to create the temporary the object");
                for (var i = 0; i < machines.Length; i++)
                {
                    var propInfo = type.GetProperty(machines[i].Name ?? $"Unknown_{i}");
                    propInfo?.SetValue(obj, day[i].Id.ToString());
                }
                

               
                scheduledDaysList.Add(obj);
            }

            return scheduledDaysList;
    }
}