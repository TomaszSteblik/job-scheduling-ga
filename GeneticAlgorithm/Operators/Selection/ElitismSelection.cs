using GeneticAlgorithm.Abstraction;
using SchedulingAlgorithmModels.Models;

namespace GeneticAlgorithm.Infrastructure.Operators.Selection;

public class ElitismSelection : ISelection
{
    public IEnumerable<Chromosome> Select(IPopulation population, int count)
    {
        population.OrderByFitnessDesc();
        return population.GetAll().Take(count);
    }
}