using SchedulingAlgorithmModels.Models;

namespace GeneticAlgorithm.Abstraction;

public interface ISelection
{
    IEnumerable<Chromosome> Select(IPopulation population, int count);
}