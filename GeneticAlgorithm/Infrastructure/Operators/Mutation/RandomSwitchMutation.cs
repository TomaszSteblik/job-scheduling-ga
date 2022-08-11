using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;
using Serilog;

namespace GeneticAlgorithm.Infrastructure.Operators.Mutation;

public class RandomSwitchMutation : IMutation
{
    private readonly IPopulation _population;
    private readonly Random _random;

    public RandomSwitchMutation(IPopulation population, Random random)
    {
        _population = population;
        _random = random;
    }

    public void MutatePopulation(double probability)
    {
        foreach (var chromosome in _population.GetAll())
        {
            if (probability > 0 && probability >= _random.NextDouble())
                Mutate(chromosome);
        }
    }

    private void Mutate(Chromosome chromosome)
    {
        var dayIndexOne = _random.Next(0, chromosome.Value.Length);
        var dayIndexTwo = _random.Next(0, chromosome.Value.Length);
        var personIndexOne = _random.Next(0, chromosome.Value.First().Length);
        var personIndexTwo = _random.Next(0, chromosome.Value.First().Length);
        chromosome.RecalculateFitness(_population.GetMachines());
        var fitnessBefore = chromosome.Fitness;
        //swap here through deconstruction
        (chromosome.Value[dayIndexOne][personIndexOne], chromosome.Value[dayIndexTwo][personIndexTwo]) = 
            (chromosome.Value[dayIndexTwo][personIndexTwo], chromosome.Value[dayIndexOne][personIndexOne]);
        chromosome.RecalculateFitness(_population.GetMachines());
        var fitnessAfter = chromosome.Fitness;
        if (!chromosome.IsValid(_population.GetMachines()))
        {
            (chromosome.Value[dayIndexOne][personIndexOne], chromosome.Value[dayIndexTwo][personIndexTwo]) = 
                (chromosome.Value[dayIndexTwo][personIndexTwo], chromosome.Value[dayIndexOne][personIndexOne]);
            chromosome.RecalculateFitness(_population.GetMachines());
            return;
        }

        if (fitnessBefore > fitnessAfter)
            return;
        
        (chromosome.Value[dayIndexOne][personIndexOne], chromosome.Value[dayIndexTwo][personIndexTwo]) = 
            (chromosome.Value[dayIndexTwo][personIndexTwo], chromosome.Value[dayIndexOne][personIndexOne]);
        chromosome.RecalculateFitness(_population.GetMachines());
        var fitnessCurrent = chromosome.Fitness;
        Log.Debug("Rolled back mutation fitness before: {FitnessBefore}, fitnessAfter: " +
                  "{FitnessAfter}, fitnessCurrent: {FitnessCurrent}",fitnessBefore,
            fitnessAfter,fitnessCurrent);

    }
}