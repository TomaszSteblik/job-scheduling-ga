using System.Data;
using System.Globalization;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using GeneticAlgorithm.Abstraction;

namespace GeneticAlgorithm.Models;

public class Population : IPopulation
{
    private Chromosome[]? Chromosomes { get; set; }
    private Machine[]? Machines { get; set; }
    private Person[]? People { get; set; }
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

    public Chromosome[]? GetAll()
    {
        return Chromosomes;
    }

    public void RecalculateAll()
    {
        foreach (var chromosome in Chromosomes)
        {
            chromosome.RecalculateFitness(Machines);
        }
    }

    public void OrderByFitnessDesc()
    {
        Chromosomes = Chromosomes.OrderByDescending(x => x.Fitness).ToArray();
    }

    public void Replace(int index, Chromosome chromosome)
    {
        Chromosomes[index] = chromosome;
    }

    public Machine[]? GetMachines()
    {
        return Machines;
    }

    public Person[]? GetPeople()
    {
        return People;
    }
}