using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Operators.Mutation;

public class RandomSwitchMutation : IMutation
{
    private readonly IPopulation _population;

    public RandomSwitchMutation(IPopulation population)
    {
        _population = population;
    }

    public void MutatePopulation(double probability)
    {
        foreach (var chromosome in _population.GetAll())
        {
            if (probability > 0 && probability <= Random.Shared.NextDouble())
                Mutate(chromosome);
        }
    }

    private void Mutate(Chromosome chromosome)
    {
        var dayIndexOne = Random.Shared.Next(0, chromosome.Value.Length);
        var dayIndexTwo = Random.Shared.Next(0, chromosome.Value.Length);
        var personIndexOne = Random.Shared.Next(0, chromosome.Value.First().Length);
        var personIndexTwo = Random.Shared.Next(0, chromosome.Value.First().Length);
        //swap here through deconstruction
        (chromosome.Value[dayIndexOne][personIndexOne], chromosome.Value[dayIndexTwo][personIndexTwo]) = (chromosome.Value[dayIndexTwo][personIndexTwo], chromosome.Value[dayIndexOne][personIndexOne]);
    }
}