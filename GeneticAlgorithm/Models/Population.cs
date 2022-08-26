using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm.Models;

public class Population : IPopulation
{
    private Chromosome[]? Chromosomes { get; set; }
    private Machine[]? Machines { get; set; }
    private Person[]? People { get; set; }
    public bool IsInitialized => Chromosomes is not null || Machines is not null || People is not null;
    
    private readonly Random _random;
    
    public Population(Random random)
    {
        _random = random;
    }

    public void InitializePopulation(Machine[] machines, Person[] people, int populationSize)
    {
        People = people;
        Machines = machines;
        Chromosomes = new Chromosome[populationSize];
        for (var i = 0; i < populationSize; i++)
        {
            Chromosomes[i] = new Chromosome(20, Machines.Length);
            for (var j = 0; j < 20; j++)
            {
                for (var k = 0; k < Machines.Length; k++)
                {
                    var qualifiedPeople = people.Where(x =>
                        x.Qualifications != null && x.Qualifications.Contains(Machines[k].RequiredQualification) &&
                        !Chromosomes[i].Value[j].Contains(x)).ToArray();
                    Chromosomes[i].Value[j][k] = qualifiedPeople[_random.Next(qualifiedPeople.Length)];
                }
            }
        }
    }

    public Chromosome[] GetAll()
    {
        if (Chromosomes is null)
            throw new PopulationNotInitializedException(nameof(Chromosomes));
        return Chromosomes;
    }

    public void RecalculateAll()
    {
        if (Chromosomes is null)
            throw new PopulationNotInitializedException(nameof(Chromosomes));
        
        foreach (var chromosome in Chromosomes)
        {
            chromosome.RecalculateFitness(Machines);
        }
    }

    public void OrderByFitnessDesc()
    {
        if (Chromosomes is null)
            throw new PopulationNotInitializedException(nameof(Chromosomes));
        
        Chromosomes = Chromosomes.OrderByDescending(x => x.Fitness).ToArray();
    }

    public void Replace(int index, Chromosome chromosome)
    {
        if (Chromosomes is null)
            throw new PopulationNotInitializedException(nameof(Chromosomes));
        
        Chromosomes[index] = chromosome;
    }

    public Machine[] GetMachines()
    {
        if (Machines is null)
            throw new PopulationNotInitializedException(nameof(Machines));
        
        return Machines;
    }

    public Person[] GetPeople()
    {
        if (People is null)
            throw new PopulationNotInitializedException(nameof(People));

        return People;
    }
}