using System.Globalization;
using GeneticAlgorithm.Abstraction;
using SchedulingAlgorithmModels.Models;
using Serilog;

namespace GeneticAlgorithm.Infrastructure;

public class Algorithm
{
    private readonly IPopulation _population;
    private readonly ISelection _selection;
    private readonly ICrossover _crossover;
    private readonly IElimination _elimination;
    private readonly IMutation _mutation;
    private readonly Parameters _parameters;

    public Algorithm(Parameters parameters, IPopulation population, ISelection selection,
        ICrossover crossover, IElimination elimination, IMutation mutation)
    {
        _population = population;
        _selection = selection;
        _crossover = crossover;
        _elimination = elimination;
        _mutation = mutation;
        _parameters = parameters;
    }

    public Result Run(Machine[] machines, Person[] people)
    {
        //initialiaze
        _population.InitializePopulation(machines, people, _parameters.PopulationSize);

        var worst = _population.GetAll().MaxBy(x => x.Fitness);

        for (int i = 0; i < _parameters.EpochsCount; i++)
        {
            //calculate fitness
            _population.RecalculateAll();
            var temp = _population.GetAll().MaxBy(x => x.Fitness);
            worst = temp.Fitness > worst.Fitness ? temp : worst;
            //selection
            var selected = _selection.Select(_population, _parameters.ChildrenCount * _parameters.ParentsPerChild);
            //crossover
            var offsprings = _crossover.GenerateOffsprings(selected.ToList());
            //elimination
            _elimination.ReplaceWeakestWithOffsprings(offsprings);
            //mutation
            _mutation.MutatePopulation(_parameters.MutationProbability);

        }

        //calculate fitness
        _population.RecalculateAll();
        var result = _population.GetAll().MinBy(x => x.Fitness) ?? 
                     throw new InvalidOperationException("Empty population");
        //return best 
        return new Result(result, _parameters, people, _population.GetMachines(), worst);
    }
}