using Autofac;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models.Enums;
using GeneticAlgorithm.Operators.Crossover;

namespace GeneticAlgorithm.Infrastructure.Factories;

public class OperatorsFactory
{
    public void RegisterCrossover(Crossover crossover, ContainerBuilder builder)
    {
        switch (crossover)
        {
            case Crossover.CrossPointMachine:
                builder.RegisterType<CrossPointMachineCrossover>().AsImplementedInterfaces();
                break;
            case Crossover.CrossPointDay:
                builder.RegisterType<CrossPointDayCrossover>().AsImplementedInterfaces();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(crossover), crossover, "Crossover name invalid");
        }
    }
}