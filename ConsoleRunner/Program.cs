// See https://aka.ms/new-console-template for more information

using System.Data;
using System.Text.Json;
using Autofac;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Infrastructure;
using GeneticAlgorithm.Infrastructure.DependencyInjection;
using GeneticAlgorithm.Models;

namespace ConsoleRunner;

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
        builder.RegisterModule<GeneticAlgorithmModule>();
        Container = builder.Build();
        var z = Container.Resolve<Algorithm>();
        var result = z.Run();
        result.RecalculateFitness(Container.Resolve<IPopulation>().GetMachines());
        
        Console.Write($"\n\n\nResult fitness: {result.Fitness}");
    }
}