using GeneticAlgorithm.Abstraction;
using SchedulingAlgorithmModels.Models;

namespace GeneticAlgorithm.Infrastructure.Operators.Crossover;

public class CrossPointMixedCrossover : ICrossover
{
    private readonly ICrossover _dayCrossover;
    private readonly ICrossover _machineCrossover;
    private bool _flag;

    public CrossPointMixedCrossover(Random random)
    {
        _machineCrossover = new CrossPointMachineCrossover(random);
        _dayCrossover = new CrossPointDayCrossover(random);
        _flag = false;
    }

    public Chromosome[] GenerateOffsprings(ICollection<Chromosome> selected)
    {
        _flag = !_flag;
        return _flag ? _machineCrossover.GenerateOffsprings(selected) : _dayCrossover.GenerateOffsprings(selected);
    }
}