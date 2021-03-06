using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Infrastructure.Operators.Crossover;

public class CrossPointMachineCrossover : ICrossover
{
    private readonly Random _random;

    public CrossPointMachineCrossover(Random random)
    {
        _random = random;
    }
    public Chromosome[] GenerateOffsprings(ICollection<Chromosome> selected)
    {
        var innerLength = selected.First().Value.First().Length;
        var outerLength = selected.First().Value.Length;
        var offsprings = new List<Chromosome>();
        for (var parentNumber = 0; parentNumber < selected.Count; parentNumber+=2)
        {
            var crossPoint = _random.Next(1, innerLength);
            var offspringOne = new Chromosome(outerLength,innerLength);
            var offspringTwo = new Chromosome(outerLength,innerLength);
            for (var i = 0; i < outerLength; i++)
            {
                for (var j = 0; j < crossPoint; j++)
                {
                    offspringOne.Value[i][j] = selected.ElementAt(parentNumber).Value[i][j];
                    offspringTwo.Value[i][j] = selected.ElementAt(parentNumber+1).Value[i][j];
                }
                for (var j = crossPoint; j < innerLength; j++)
                {
                    offspringOne.Value[i][j] = selected.ElementAt(parentNumber+1).Value[i][j];
                    offspringTwo.Value[i][j] = selected.ElementAt(parentNumber).Value[i][j];
                }
            }

            offsprings.Add(offspringOne);
            offsprings.Add(offspringTwo);
        }
        
        return offsprings.ToArray();

    }
}