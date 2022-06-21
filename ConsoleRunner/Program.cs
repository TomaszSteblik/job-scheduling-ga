// See https://aka.ms/new-console-template for more information

using System.Data;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Infrastructure;
using GeneticAlgorithm.Infrastructure.DependencyInjection;
using GeneticAlgorithm.Models;

namespace ConsoleRunner;
class PersonelHelper
{
    [JsonInclude]
    [Name("name")]
    public string? Name { get; set; }
        
    [JsonInclude]
    [Name("qualifications")]
    public string? Qualifications { get; set; }
}
static class Program
{
    private static IContainer? Container { get; set; }
    
    public static async Task Main()
    {
        var stream = new FileStream("/Users/tsteblik/RiderProjects/Scheduling/ConsoleRunner/Data/parameters.json", FileMode.Open);
        var parameters = await JsonSerializer.DeserializeAsync<Parameters>(stream);
        if (parameters is null)
            throw new DataException("Invalid parameters format");
        var builder = new ContainerBuilder();
        builder.Register<Parameters>(x=> parameters);
        var machines = GetMachinesFromCsv(parameters.DataPathMachines);
        var people = GetPeopleFromCsv(parameters.DataPathPersonel);
        builder.RegisterModule(new GeneticAlgorithmModule(people, machines, parameters.PopulationSize));
        Container = builder.Build();
        var z = Container.Resolve<Algorithm>();
        var result = z.Run();
        result.RecalculateFitness(Container.Resolve<IPopulation>().GetMachines());
        
        Console.Write($"\n\n\nResult fitness: {result.Fitness}");
    }

    private static Person[] GetPeopleFromCsv(string? parametersDataPathPersonel)
    {
        var people = new List<Person>();
        if (parametersDataPathPersonel is null)
            throw new ArgumentNullException(nameof(parametersDataPathPersonel),"Null path to personel");
        using (var reader = new StreamReader(parametersDataPathPersonel))
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

        return people.ToArray();
    }

    private static Machine[] GetMachinesFromCsv(string? parametersDataPathMachines)
    {
        if (parametersDataPathMachines is null)
            throw new ArgumentNullException(nameof(parametersDataPathMachines), "Null path to machines");
        using var reader = new StreamReader(parametersDataPathMachines);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Machine>();
        return records.ToArray();
    }
}