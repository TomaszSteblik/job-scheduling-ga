using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Infrastructure.Operators.Elimination;

public class ElitismElimination : IElimination
{
    private readonly IPopulation _population;

    public ElitismElimination(IPopulation population)
    {
        _population = population;
    }
    public void ReplaceWeakestWithOffsprings(Chromosome[] offsprings)
    {
        _population.OrderByFitnessDesc();
        var lowestFitnessCurrent = _population.GetAll().Max(x=>x.Fitness);
        for (var i = 0; i < offsprings.Length; i++)
        {
            offsprings[i].RecalculateFitness(_population.GetMachines());
            if(offsprings[i].Fitness >= lowestFitnessCurrent)
                continue;
            if(offsprings[i].Fitness >= _population.GetAll()[i].Fitness)
                continue;
            _population.Replace(i,offsprings[i]);
        }
    }
}