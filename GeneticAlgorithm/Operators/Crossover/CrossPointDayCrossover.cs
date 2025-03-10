using GeneticAlgorithm.Abstraction;
using SchedulingAlgorithmModels.Models;
using Serilog;

namespace GeneticAlgorithm.Infrastructure.Operators.Crossover;

public class CrossPointDayCrossover : ICrossover
{
    private readonly Random _random;

    public CrossPointDayCrossover(Random random)
    {
        _random = random;
    }
    public Chromosome[] GenerateOffsprings(ICollection<Chromosome> selected)
    {
        var innerLength = selected.First().Value.First().Length;
        var outerLength = selected.First().Value.Length;
        var offsprings = new List<Chromosome>();
        for (var parentNumber = 0; parentNumber < selected.Count; parentNumber += 2)
        {
            var crossPoint = _random.Next(1, outerLength);
            var offspringOne = new Chromosome(outerLength, innerLength);
            var offspringTwo = new Chromosome(outerLength, innerLength);
            for (var i = 0; i < outerLength; i++)
            {
                offspringOne.Value[i] = selected.ElementAt(parentNumber + 1).Value[i];
                offspringTwo.Value[i] = selected.ElementAt(parentNumber).Value[i];
            }
            for (var i = crossPoint; i < outerLength; i++)
            {
                offspringOne.Value[i] = selected.ElementAt(parentNumber).Value[i];
                offspringTwo.Value[i] = selected.ElementAt(parentNumber + 1).Value[i];
            }
            offsprings.Add(offspringOne);
            offsprings.Add(offspringTwo);
        }

        return offsprings.ToArray();
    }
}