using System.Data;
using Autofac;
using Autofac.Core.Activators.Reflection;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Infrastructure.Operators.Crossover;
using GeneticAlgorithm.Infrastructure.Operators.Elimination;
using GeneticAlgorithm.Infrastructure.Operators.Mutation;
using GeneticAlgorithm.Infrastructure.Operators.Selection;
using GeneticAlgorithm.Models;
using GeneticAlgorithm.Models.Enums;
using Serilog;

namespace GeneticAlgorithm.Infrastructure.DependencyInjection;

public class GeneticAlgorithmModule : Module
{
    public GeneticAlgorithmModule(Person[] people, Machine[] machines, int populationSize)
    {
        People = people;
        Machines = machines;
        PopulationSize = populationSize;
    }

    public Machine[] Machines { get; init; }
    public Person[] People { get; init; }
    public int PopulationSize { get; init; }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(Random.Shared);

        builder.RegisterType<Population>().WithParameters(new []
        {
            new PositionalParameter(0,Machines),
            new PositionalParameter(1, People),
            new PositionalParameter(2,PopulationSize)
        }).AsImplementedInterfaces().SingleInstance();
        
        builder.Register<ICrossover>(x => x.Resolve<Parameters>().Crossover
            switch
            { 
                Crossover.CrossPointMachine => new CrossPointMachineCrossover(x.Resolve<Random>()),
                Crossover.CrossPointDay =>  new CrossPointDayCrossover(x.Resolve<Random>()),
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