using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using DynamicData;
using GeneticAlgorithm.Infrastructure;
using GeneticAlgorithm.Infrastructure.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Scheduling.Exceptions;
using Scheduling.Helpers;
using Scheduling.Models;
using Scheduling.Repositories;
using Scheduling.Views;
using Scheduling.Views.Windows;
using SchedulingAlgorithmModels.Models;
using SchedulingAlgorithmModels.Models.Enums;
using Machine = SchedulingAlgorithmModels.Models.Machine;

namespace Scheduling.ViewModels;

public class AlgorithmParametersViewModel : ViewModelBase, IActivatableViewModel
{
    private readonly IMapper _mapper;
    private readonly ISelectedDataRepository _selectedDataRepository;
    public AlgorithmSettings Settings { get; }
    public ViewModelActivator Activator { get; }
    public static IEnumerable<Crossover> CrossoverValues => Enum.GetValues<Crossover>();
    public static IEnumerable<Selection> SelectionValues => Enum.GetValues<Selection>();
    public static IEnumerable<Elimination> EliminationValues => Enum.GetValues<Elimination>();
    public static IEnumerable<Mutation> MutationValues => Enum.GetValues<Mutation>();
    public ReactiveCommand<Unit, Unit> RunGaCommand { get; }

    public AlgorithmParametersViewModel(AlgorithmSettings settings, IMapper mapper, 
        ISelectedDataRepository selectedDataRepository)
    {
        _mapper = mapper;
        _selectedDataRepository = selectedDataRepository;
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
        RunGaCommand.LogExceptions();
    }

    private Task ExecuteGeneticAlgorithm()
    {
        var builder = new ContainerBuilder();
        var param = _mapper.Map<Parameters>(Settings);
        builder.RegisterInstance(param).As<Parameters>();
        builder.RegisterModule(new GeneticAlgorithmModule());
        var container = builder.Build();
        var machines = _selectedDataRepository.GetMachines();
        var ma = _mapper.Map<Machine[]>(machines);
        var workers = _selectedDataRepository.GetWorkers().ToList();
        var mw = _mapper.Map<Person[]>(workers);
        for (var i = 0; i < mw.Length; i++)
        {
            mw[i].Id = i;
            mw[i].PreferredMachineIds = workers[i].PreferredMachines.Select(x => machines.IndexOf(x)).ToList();
        }

        var result = container.Resolve<Algorithm>().Run(ma,mw);

        var algorithmResult = _mapper.Map<AlgorithmResult>(result);

        if (algorithmResult == null)
            throw new EmptyResultException();

        var context = new ResultsViewModel(algorithmResult);

        var window = new ResultsWindow()
        {
            DataContext = context
        };
        window.Show();

        return Task.CompletedTask;
    }

    #region Mock
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