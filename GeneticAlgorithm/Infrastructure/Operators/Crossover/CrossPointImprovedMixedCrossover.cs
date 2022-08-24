using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Infrastructure.Operators.Crossover;

public class CrossPointImprovedMixedCrossover : ICrossover
{
    private readonly Random _random;
    private readonly ICrossover _dayCrossover;
    private readonly ICrossover _machineCrossover;
    private bool _flag;

    public CrossPointImprovedMixedCrossover(Random random, IPopulation population)
    {
        _random = random;
        _machineCrossover = new CrossPointImprovedMachineCrossover(random, population);
        _dayCrossover = new CrossPointDayCrossover(random);
        _flag = false;
    }
    public Chromosome[] GenerateOffsprings(ICollection<Chromosome> selected)
    {
        _flag = !_flag;
        return _flag ? _machineCrossover.GenerateOffsprings(selected) : _dayCrossover.GenerateOffsprings(selected);
    }
}