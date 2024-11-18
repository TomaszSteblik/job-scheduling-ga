using GeneticAlgorithm.Abstraction;
using SchedulingAlgorithmModels.Models;

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
        var offsprings = new List<Chromosome>();
        for (var parentNumber = 0; parentNumber < selected.Count; parentNumber += 2)
        {

            var (offspringOne, offspringTwo) =
                GenerateChromosomePair(selected.ElementAt(parentNumber),
                                        selected.ElementAt(parentNumber + 1));

            offsprings.Add(offspringOne);
            offsprings.Add(offspringTwo);

        }
        return offsprings.ToArray();
    }

    private (Chromosome offspringOne, Chromosome offspringTwo) GenerateChromosomePair(Chromosome parentOne, Chromosome parentTwo)
    {
        var innerLength = parentOne.Value.First().Length;
        var outerLength = parentOne.Value.Length;
        var crossPoint = _random.Next(1, innerLength);
        var offspringOne = new Chromosome(outerLength, innerLength);
        var offspringTwo = new Chromosome(outerLength, innerLength);

        for (var i = 0; i < outerLength; i++)
        {
            for (var j = 0; j < crossPoint; j++)
            {
                offspringOne.Value[i][j] = parentOne.Value[i][j];
                offspringTwo.Value[i][j] = parentTwo.Value[i][j];
            }
            for (var j = crossPoint; j < innerLength; j++)
            {
                offspringOne.Value[i][j] = parentTwo.Value[i][j];
                offspringTwo.Value[i][j] = parentOne.Value[i][j];
            }
        }

        return (offspringOne, offspringTwo);
    }
}