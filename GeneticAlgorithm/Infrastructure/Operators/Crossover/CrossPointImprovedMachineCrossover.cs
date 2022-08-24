using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Models;

namespace GeneticAlgorithm.Infrastructure.Operators.Crossover;

public class CrossPointImprovedMachineCrossover : ICrossover
{
    private readonly Random _random;
    private readonly IPopulation _population;
    private readonly IDictionary<Qualification, ICollection<Person>> _peopleByQualification;

    public CrossPointImprovedMachineCrossover(Random random, IPopulation population)
    {
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
        var offsprings = new List<Chromosome>();
        for (var parentNumber = 0; parentNumber < selected.Count; parentNumber+=2)
        {

            var (offspringOne, offspringTwo) = 
                GenerateChromosomePair(selected.ElementAt(parentNumber), 
                    selected.ElementAt(parentNumber + 1));
            
            FixChromosome(offspringOne);
            FixChromosome(offspringTwo);
            
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
        var offspringOne = new Chromosome(outerLength,innerLength);
        var offspringTwo = new Chromosome(outerLength,innerLength);

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

    private void FixChromosome(Chromosome offspring)
    {
        for (var i = 0; i < offspring.Value.Length; i++)
        {
            for (var j = 0; j < offspring.Value[i].Length; j++)
            {
                var workerQualifications = offspring.Value[i][j].Qualifications;
                var machineRequiredQualification = _population.GetMachines()[j].RequiredQualification;
                if (workerQualifications != null && workerQualifications.Contains(machineRequiredQualification))
                    continue;

                var qualifiedPeople = _peopleByQualification[machineRequiredQualification];
                var unusedQualifiedPeople = qualifiedPeople
                    .Where(x => !offspring.Value[i].Contains(x))
                    .ToArray();
                
                offspring.Value[i][j] = unusedQualifiedPeople.Any() ? 
                    unusedQualifiedPeople.ElementAt(_random.Next(qualifiedPeople.Count)) : 
                    qualifiedPeople.ElementAt(_random.Next(qualifiedPeople.Count));
            }
        }
    }
}