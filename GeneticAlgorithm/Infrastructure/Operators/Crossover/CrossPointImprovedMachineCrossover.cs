using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Infrastructure.Operators.Crossover;

public class CrossPointImprovedMachineCrossover : ICrossover
{
    private readonly Random _random;
    private readonly IPopulation _population;
    private readonly IDictionary<Qualification, ICollection<Person>> _peopleByQualification;
    private readonly ICrossover _machineCrossover;

    public CrossPointImprovedMachineCrossover(Random random, IPopulation population)
    {
        _machineCrossover = new CrossPointMachineCrossover(random);
        _random = random;
        _population = population;
        _peopleByQualification = new Dictionary<Qualification, ICollection<Person>>();
        var people = population.GetPeople();
        foreach (var qualification in Enum.GetValues<Qualification>())
        {
            var qualifiedPeople = people.Where(x =>
                x.Qualifications != null && x.Qualifications.Contains(qualification)).ToArray();
            _peopleByQualification.Add(qualification, qualifiedPeople);
        }

    }

    public Chromosome[] GenerateOffsprings(ICollection<Chromosome> selected)
    {
        var offsprings = _machineCrossover.GenerateOffsprings(selected);
        
        foreach (var offspring in offsprings)
            FixChromosome(offspring);
        
        return offsprings;
    }

    private void FixChromosome(Chromosome offspring)
    {
        for (var i = 0; i < offspring.Value.Length; i++)
        {
            for (var j = 0; j < offspring.Value[i].Length; j++)
            {
                var count = offspring.Value[i].Count(x => x.Id == offspring.Value[i][j].Id);
                if(count <= 1)
                    continue;
                
                var machineRequiredQualification = _population.GetMachines()[j].RequiredQualification;
                
                var qualifiedPeople = _peopleByQualification[machineRequiredQualification];
                var unusedQualifiedPeople = qualifiedPeople
                    .Where(x => !offspring.Value[i].Contains(x))
                    .ToArray();
                
                offspring.Value[i][j] = unusedQualifiedPeople.Any() ? 
                    unusedQualifiedPeople.ElementAt(_random.Next(unusedQualifiedPeople.Length)) : 
                    throw new IndexOutOfRangeException(
                        $"Not enough of qualified workers for {machineRequiredQualification}");
            }
        }
    }
}