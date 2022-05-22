using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Abstraction;

public interface IElimination
{
    void ReplaceWeakestWithOffsprings(Chromosome[] offsprings);
}