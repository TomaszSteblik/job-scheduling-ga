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

    class PersonelHelper
    {
        [JsonInclude]
        [Name("name")]
        public string? Name { get; set; }
        
        [JsonInclude]
        [Name("qualifications")]
        public string? Qualifications { get; set; }
    }

    public Population(Parameters parameters)
    {
        if (parameters.DataPathMachines is null)
            throw new ArgumentNullException(nameof(parameters) ,nameof(parameters.DataPathMachines));
        if (parameters.DataPathPersonel is null)
            throw new ArgumentNullException(nameof(parameters) ,nameof(parameters.DataPathPersonel));
        using (var reader = new StreamReader(parameters.DataPathMachines))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Machine>();
            Machines = records.ToArray();
        }

        var people = new List<Person>();
        using (var reader = new StreamReader(parameters.DataPathPersonel))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<PersonelHelper>();
            var enumerable = records.ToList();
            for (var index = 0; index < enumerable.Count; index++)
            {
                var record = enumerable[index];
                if (record.Qualifications is null)
                    throw new DataException(nameof(record));
                var person = new Person
                {
                    Id = index,
                    Name = record.Name,
                    Qualifications = new List<Qualification>()
                };

                var qualifications = record.Qualifications.Split('-');
                foreach (var qualification in qualifications)
                {
                    person.Qualifications.Add(Enum.Parse<Qualification>(qualification));
                }
                people.Add(person);
            }
        }

        Chromosomes = new Chromosome[parameters.PopulationSize];
        for (int i = 0; i < parameters.PopulationSize; i++)
        {
            Chromosomes[i] = new Chromosome(20, Machines.Length);
            for (int j = 0; j < 20; j++)
            {
                for (int k = 0; k < Machines.Length; k++)
                {

                    var s = people.Where(x =>
                        x.Qualifications.Contains(Machines[k].RequiredQualification) &&
                        !Chromosomes[i].Value[j].Contains(x)).ToArray();
                    Chromosomes[i].Value[j][k] = s[Random.Shared.Next(s.Length)];
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