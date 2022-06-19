using Autofac;
using Autofac.Core;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Infrastructure.Operators.Crossover;
using GeneticAlgorithm.Models;
using GeneticAlgorithm.Models.Enums;
using GeneticAlgorithm.Operators.Crossover;
using GeneticAlgorithm.Operators.Elimination;
using GeneticAlgorithm.Operators.Mutation;
using GeneticAlgorithm.Operators.Selection;

namespace GeneticAlgorithm.Infrastructure.DependencyInjection;

public class GeneticAlgorithmModule : Module
{

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Population>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterInstance(Random.Shared);
        
        builder.Register<ICrossover>(x => x.Resolve<Parameters>().Crossover
            switch
            { 
                Crossover.CrossPointMachine => new CrossPointMachineCrossover(x.Resolve<Random>()),
                Crossover.CrossPointDay =>  new CrossPointDayCrossover(x.Resolve<Random>()),
                _ => throw new ArgumentException("Unknown parameter: ", nameof(Crossover))
            });
        
        builder.Register<IElimination>(x => x.Resolve<Parameters>().Elimination
            switch
            {
                Elimination.Elitism => new ElitismElimination(x.Resolve<IPopulation>()),
                _ => throw new ArgumentException("Unknown parameter: ", nameof(Elimination))
            });
        
        builder.Register<ISelection>(x => x.Resolve<Parameters>().Selection
            switch
            {
                Selection.Elitism => new ElitismSelection(),
                _ => throw new ArgumentException("Unknown parameter: ", nameof(Selection))
            });
        
        builder.Register<IMutation>(x => x.Resolve<Parameters>().Mutation
            switch
            {
                Mutation.Random => new RandomSwitchMutation(x.Resolve<IPopulation>(), x.Resolve<Random>()),
                _ => throw new ArgumentException("Unknown parameter: ", nameof(Mutation))
            });

        builder.RegisterType<Algorithm>();
    }
}