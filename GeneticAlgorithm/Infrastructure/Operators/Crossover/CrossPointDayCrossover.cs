using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Operators.Crossover;

public class CrossPointDayCrossover : ICrossover
{
    public Chromosome[] GenerateOffsprings(ICollection<Chromosome> selected)
    {
        var innerLength = selected.First().Value.First().Length;
        var outerLength = selected.First().Value.Length;
        var offsprings = new List<Chromosome>();
        for (var parentNumber = 0; parentNumber < selected.Count; parentNumber+=2)
        {
            var crossPoint = Random.Shared.Next(1, outerLength);
            var offspringOne = new Chromosome(outerLength,innerLength);
            var offspringTwo = new Chromosome(outerLength,innerLength);
            for (var i = 0; i < outerLength; i++)
            {
                offspringOne.Value[i] = selected.ElementAt(parentNumber+1).Value[i];
                offspringTwo.Value[i] = selected.ElementAt(parentNumber).Value[i];
            }
            for (var i = crossPoint; i < outerLength; i++)
            {
                offspringOne.Value[i] = selected.ElementAt(parentNumber).Value[i];
                offspringTwo.Value[i] = selected.ElementAt(parentNumber+1).Value[i];
            }

            offsprings.Add(offspringOne);
            offsprings.Add(offspringTwo);
        }
        
        return offsprings.ToArray();
    }
}