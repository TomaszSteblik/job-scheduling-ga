// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Autofac;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Infrastructure;
using GeneticAlgorithm.Infrastructure.DependencyInjection;
using GeneticAlgorithm.Models;

namespace ConsoleRunner;

class Program
{
    private static IContainer Container { get; set; }

    public static async Task Main()
    {
        var stream = new FileStream("/Users/tsteblik/RiderProjects/Scheduling/ConsoleRunner/Data/parameters.json", FileMode.Open);
        var parameters = await JsonSerializer.DeserializeAsync<Parameters>(stream);
        var builder = new ContainerBuilder();
        builder.Register<Parameters>(x=> parameters);
        builder.RegisterModule<GeneticAlgorithmModule>();
        Container = builder.Build();
        var z = Container.Resolve<Algorithm>();
        var result = z.Run();
        result.RecalculateFitness(Container.Resolve<IPopulation>().GetMachines());
        
        Console.Write($"\n\n\nResult fitness: {result.Fitness}\n\nDays:        ");
        for (int i = 0; i < result.Value.Length; i++)
        {
            Console.Write($"{i+1,3}");
        }

        Console.WriteLine("\n");

        var persons = result.Value.Transpose();
        for (var index = 0; index < persons.Length; index++)
        {
            var machine = persons[index];
            Console.WriteLine(machine.Aggregate($"Machine {index+1}:   ", (current, person) => current + $"{person.Id,3}"));
        }


        Console.WriteLine();
        
        var workers = result.Value.SelectMany(x => x).Select(x => x).ToList();
        var dic = workers.Select(x => new {Person = x, Count = workers.Count(z => z.Id == x.Id)});
        var distinctBy = dic.DistinctBy(x=>x.Person.Id);
        Console.WriteLine($"{"Person:",-20} {"Skills:",-30} Days in work:");
        foreach (var worker in distinctBy)
        {
            Console.WriteLine($"{worker.Person.Name,-20} {worker.Person.Qualifications.Aggregate(" ",(current, q)=>current+=$"{q.ToString()} "),-30} {worker.Count}");
        }

        Console.WriteLine($"\nWrong person at wrong position count: {AnalyzeWrongPosition(result, Container.Resolve<IPopulation>().GetMachines())}");
        var analysis = AnalyzeWorkingDays(result);
        Console.WriteLine($"\nPeople working more than 14 days: {analysis.Item1}\nPeople working less than 10 days: {analysis.Item2}");
        Console.WriteLine($"Days with one person working on multiple machines: {AnalyzeMultipleMachines(result)}");
    }
    static int AnalyzeWrongPosition(Chromosome chromosome, Machine[] machines)
    {
        var fitness = 0;
        for (var day = 0; day < chromosome.Value.Length; day++)
        {
            for (var machineNumber = 0; machineNumber < machines.Length; machineNumber++)
            {
                if (!chromosome.Value[day][machineNumber].Qualifications.Contains(machines[machineNumber].RequiredQualification))
                    fitness++;
                
            }
        }

        return fitness;
    }

    static (int,int) AnalyzeWorkingDays(Chromosome chromosome)
    {
        var tooMuch = 0;
        var tooLittle = 0;
        var workers = chromosome.Value.SelectMany(x => x).Select(x => x).ToList();
        var dic = workers.Select(x => new {Person = x, Count = workers.Count(z => z.Id == x.Id)});
        var distinctBy = dic.DistinctBy(x=>x.Person.Id);
        foreach (var worker in distinctBy)
        {
            if (worker.Count is > 14)
                tooMuch += 1;
            else if (worker.Count is < 10)
                tooLittle += 1;
        }

        return (tooMuch, tooLittle);
    }

    static int AnalyzeMultipleMachines(Chromosome chromosome)
    {
        var fitness = 0;
        foreach (var day in chromosome.Value)
        {
            var count = day.DistinctBy(x => x.Id).Count();
            if(count != day.Count())
                fitness += 1;
        }

        return fitness;
    }
}