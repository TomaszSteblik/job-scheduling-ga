using System.Data;
using Autofac;
using Autofac.Core.Activators.Reflection;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Infrastructure.Operators.Crossover;
using GeneticAlgorithm.Infrastructure.Operators.Elimination;
using GeneticAlgorithm.Infrastructure.Operators.Mutation;
using GeneticAlgorithm.Infrastructure.Operators.Selection;
using GeneticAlgorithm.Models;
using SchedulingAlgorithmModels.Models;
using SchedulingAlgorithmModels.Models.Enums;
using Serilog;

namespace GeneticAlgorithm.Infrastructure.DependencyInjection;

public class GeneticAlgorithmModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(Random.Shared);

        builder.RegisterType<Population>().AsImplementedInterfaces().SingleInstance();

        builder.Register<ICrossover>(x => x.Resolve<Parameters>().Crossover
            switch
            {
                Crossover.CrossPointMachine => new CrossPointMachineCrossover(x.Resolve<Random>()),
                Crossover.CrossPointDay => new CrossPointDayCrossover(x.Resolve<Random>()),
                Crossover.CrossPointMixed => new CrossPointMixedCrossover(x.Resolve<Random>()),
                Crossover.CrossPointImprovedMachine => new CrossPointImprovedMachineCrossover(x.Resolve<Random>(),
                    x.Resolve<IPopulation>()),
                Crossover.CrossPointImprovedMixed => new CrossPointImprovedMixedCrossover(x.Resolve<Random>(),
                    x.Resolve<IPopulation>()),
                _ => throw new DataException($"Unknown parameter: {nameof(Crossover)}")
            });

        builder.Register<IElimination>(x => x.Resolve<Parameters>().Elimination
            switch
            {
                Elimination.Elitism => new ElitismElimination(x.Resolve<IPopulation>()),
                _ => throw new DataException($"Unknown parameter: {nameof(Elimination)}")
            });

        builder.Register<ISelection>(x => x.Resolve<Parameters>().Selection
            switch
            {
                Selection.Elitism => new ElitismSelection(),
                _ => throw new DataException($"Unknown parameter: {nameof(Selection)}")
            });

        builder.Register<IMutation>(x => x.Resolve<Parameters>().Mutation
            switch
            {
                Mutation.Random => new RandomSwitchMutation(x.Resolve<IPopulation>(), x.Resolve<Random>()),
                _ => throw new DataException($"Unknown parameter: {nameof(Mutation)}")
            });

        builder.RegisterType<Algorithm>();
    }
}