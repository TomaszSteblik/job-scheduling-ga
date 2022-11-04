// See https://aka.ms/new-console-template for more information

using System.Data;
using System.Text.Json;
using Autofac;
using GeneticAlgorithm.Infrastructure;
using GeneticAlgorithm.Infrastructure.DependencyInjection;
using GeneticAlgorithm.Models;
using Serilog;
using static ConsoleRunner.DataReaderCsvHelper;

namespace ConsoleRunner;

internal static class Program
{
    private static IContainer? Container { get; set; }

    public static async Task Main()
    {
        var log = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .CreateLogger();
        Log.Logger = log;
        var stream = new FileStream("Data/parameters.json", FileMode.Open);
        var parameters = await JsonSerializer.DeserializeAsync<Parameters>(stream);
        if (parameters is null)
            throw new DataException("Invalid parameters format");
        var builder = new ContainerBuilder();
        builder.Register<Parameters>(x => parameters);
        var machines = GetMachinesFromCsv(parameters.DataPathMachines);
        var people = GetPeopleFromCsv(parameters.DataPathPersonel);
        builder.RegisterModule(new GeneticAlgorithmModule());
        Container = builder.Build();

        var z = Container.Resolve<Algorithm>();
        z.Run(machines, people, parameters.PopulationSize);
    }
}