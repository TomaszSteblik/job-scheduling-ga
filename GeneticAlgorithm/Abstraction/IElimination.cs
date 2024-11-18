using SchedulingAlgorithmModels.Models;

namespace GeneticAlgorithm.Abstraction;

public interface IElimination
{
    void ReplaceWeakestWithOffsprings(Chromosome[] offsprings);
}