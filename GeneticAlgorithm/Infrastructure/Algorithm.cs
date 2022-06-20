using System.Globalization;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;

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
        ICrossover crossover,  IElimination elimination, IMutation mutation)
    {
        _population = population;
        _selection = selection;
        _crossover = crossover;
        _elimination = elimination;
        _mutation = mutation;
        _parameters = parameters;
    }

    public Chromosome Run()
    {
        //initialiaze
        for (int i = 0; i < _parameters.EpochsCount; i++)
        {
            //calculate fitness
            Console.WriteLine($"EPOCH {i,3} AVG: {_population.GetAll().Average(x=>x.Fitness).ToString(CultureInfo.InvariantCulture)} MACHINES_REPEAT:{_population.GetAll().Max(x=>x.AnalyzeMultipleMachines())}" +
                              $" POS_WRNG: {_population.GetAll().Max(x=>x.AnalyzeWrongPosition(_population.GetMachines()))}");

            _population.RecalculateAll();
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
        //return best 
        return _population.GetAll().MinBy(x=>x.Fitness) ?? throw new InvalidOperationException("Empty population");  
    }
}