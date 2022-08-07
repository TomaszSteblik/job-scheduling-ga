using System.Data;
using System.Globalization;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using GeneticAlgorithm.Abstraction;

namespace GeneticAlgorithm.Models;

public class Population : IPopulation
{
    private Chromosome[] Chromosomes { get; set; }
    private Machine[] Machines { get; set; }
    
    public Population(Machine[] machines, Person[] people, int populationSize, Random random)
    {
        Machines = machines;
        Chromosomes = new Chromosome[populationSize];
        for (int i = 0; i < populationSize; i++)
        {
            Chromosomes[i] = new Chromosome(20, Machines.Length);
            for (int j = 0; j < 20; j++)
            {
                for (int k = 0; k < Machines.Length; k++)
                {

                    var s = people.Where(x =>
                        x.Qualifications != null && x.Qualifications.Contains(Machines[k].RequiredQualification) && !Chromosomes[i].Value[j].Contains(x)).ToArray();
                    Chromosomes[i].Value[j][k] = s[random.Next(s.Length)];
                }
            }
        }
    }
    
    public Chromosome[] GetAll()
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

    public Machine[] GetMachines()
    {
        return Machines;
    }
}