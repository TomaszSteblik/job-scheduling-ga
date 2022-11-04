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
using Scheduling.Exceptions;
using Scheduling.Models;
using Scheduling.Repositories;
using Scheduling.Views;
using Scheduling.Views.Windows;
using Machine = GeneticAlgorithm.Models.Machine;

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

    public AlgorithmParametersViewModel(AlgorithmSettings settings, IMapper mapper, ISelectedDataRepository selectedDataRepository)
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
    }

    private Task ExecuteGeneticAlgorithm()
    {
        var builder = new ContainerBuilder();
        var param = _mapper.Map<Parameters>(Settings);
        builder.RegisterInstance(param).As<Parameters>();
        builder.RegisterModule(new GeneticAlgorithmModule());
        var container = builder.Build();

        var result = container.Resolve<Algorithm>().Run(_mapper.Map<Machine[]>(_selectedDataRepository.GetMachines()),
            _mapper.Map<Person[]>(_selectedDataRepository.GetWorkers()),
            Settings.PopulationSize);

        var algorithmResult = _mapper.Map<AlgorithmResult>(result);

        if (algorithmResult == null)
            throw new EmptyResultException();

        var window = new ResultsWindow()
        {
            DataContext = new ResultsViewModel(algorithmResult)
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