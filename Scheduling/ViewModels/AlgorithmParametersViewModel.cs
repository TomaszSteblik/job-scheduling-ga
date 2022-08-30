using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
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
    private readonly IMapper _mapper;
    public AlgorithmSettings Settings { get; }
    public ViewModelActivator Activator { get; }
    public static IEnumerable<Crossover> CrossoverValues => Enum.GetValues<Crossover>();
    public static IEnumerable<Selection> SelectionValues => Enum.GetValues<Selection>();
    public static IEnumerable<Elimination> EliminationValues => Enum.GetValues<Elimination>();
    public static IEnumerable<Mutation> MutationValues => Enum.GetValues<Mutation>();
    public ReactiveCommand<Unit, Unit> RunGaCommand { get; }
    [Reactive] 
    public double? Result { get; private set; }
    [Reactive] 
    public IEnumerable<object>? Values { get; set; }

    public AlgorithmParametersViewModel(AlgorithmSettings settings, IMapper mapper)
    {
        _mapper = mapper;
        Settings = settings;
        Activator = new ViewModelActivator();
        MockSettings(settings);
        this.WhenActivated((CompositeDisposable disposables) =>
        {
            /* handle activation */
            Disposable
                .Create(() => { /* handle deactivation */ })
                .DisposeWith(disposables);
        });
        RunGaCommand = ReactiveCommand.CreateFromTask(ExecuteGeneticAlgorithm);
    }

    private Task ExecuteGeneticAlgorithm()
    {
        var builder = new ContainerBuilder();
        var param = _mapper.Map<Parameters>(Settings);
        builder.RegisterInstance(param).As<Parameters>();
        builder.RegisterModule(new GeneticAlgorithmModule());
        var container = builder.Build();
            
        var result = container.Resolve<Algorithm>().Run(MockMachines(),
            MockPeople(),
            Settings.PopulationSize);
        
        Result = result.Chromosome.Fitness;

        var scheduledDays = _mapper.Map<AlgorithmResult>(result).Schedule;

        //TODO: Make application exception
        Values = scheduledDays ?? throw new Exception("Empty result schedule");
        return Task.CompletedTask;
    }

    #region Mock
    //TEMPORARY METHOD
    private static Machine[] MockMachines()
    {
        return new[]
        {
            new Machine() {Name = "Machine1", RequiredQualification = Qualification.Milling},
            new Machine() {Name = "Machine2", RequiredQualification = Qualification.Sawing}
        };
    }

    //TEMPORARY METHOD
    private static Person[] MockPeople()
    {
        return new[]
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
        };
    }
    
    //TEMPORARY METHOD
    private static void MockSettings(AlgorithmSettings settings)
    {
        settings.PopulationSize = 100;
        settings.ChildrenCount = 10;
        settings.ParentsPerChild = 2;
        settings.EpochsCount = 100;
        settings.MutationProbability = 0.01;
    }
    #endregion
    
}