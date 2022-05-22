using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Abstraction;

public interface ICrossover
{
    Chromosome[] GenerateOffsprings(ICollection<Chromosome> selected);
}