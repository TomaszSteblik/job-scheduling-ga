using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;
using Serilog;

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
        foreach (var offspring in offsprings)
        {
            offspring.RecalculateFitness();
        }
        offsprings = offsprings.Where(x => x.IsValid(_population.GetMachines())).OrderBy(x=>x.Fitness).ToArray();
        Log.Debug("Valid chromosomes for elimination: {Count}",offsprings.Length);
        for (var i = 0; i < offsprings.Length; i++)
        {
            offsprings[i].RecalculateFitness();
            if(offsprings[i].Fitness >= lowestFitnessCurrent)
                continue;
            Log.Debug("ALMOST ELIMINATED");
            if(offsprings[i].Fitness >= _population.GetAll()[i].Fitness)
                continue;
            _population.Replace(i,offsprings[i]);
            Log.Debug("REPLACED");
        }
    }
}