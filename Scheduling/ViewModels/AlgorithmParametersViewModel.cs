using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
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
    public double ResultFitness { get; set; }

    public AlgorithmParametersViewModel(AlgorithmSettings settings, IMapper mapper)
    {
        Settings = settings;
        Activator = new ViewModelActivator();
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
            builder.RegisterModule(new GeneticAlgorithmModule(new[]
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
                }, new[]
                {
                    new Machine() {Name = "Machine1", PersonelCount = 1, RequiredQualification = Qualification.Milling},
                    new Machine() {Name = "Machine2", PersonelCount = 1, RequiredQualification = Qualification.Sawing}
                },
                Settings.PopulationSize));
            var container = builder.Build();
            var result = container.Resolve<Algorithm>().Run();
            ResultFitness = result.Fitness;
        });
    }

    public ViewModelActivator Activator { get; }
}