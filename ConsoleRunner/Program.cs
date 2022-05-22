// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Autofac;
using GeneticAlgorithm.Abstraction;
using GeneticAlgorithm.Infrastructure;
using GeneticAlgorithm.Models;
using GeneticAlgorithm.Operators.Crossover;
using GeneticAlgorithm.Operators.Elimination;
using GeneticAlgorithm.Operators.Mutation;
using GeneticAlgorithm.Operators.Selection;

class Program
{
    private static IContainer Container { get; set; }

    public async static Task Main()
    {
        var stream = new FileStream("/Users/tsteblik/RiderProjects/Scheduling/ConsoleRunner/Data/parameters.json", FileMode.Open);
        var parameters = await JsonSerializer.DeserializeAsync<Parameters>(stream);
        var builder = new ContainerBuilder();
        builder.Register<Parameters>(x=> parameters);
        builder.RegisterType<CrossPointCrossover>().As<ICrossover>();
        builder.RegisterType<ElitismElimination>().As<IElimination>();
        builder.RegisterType<RandomSwitchMutation>().As<IMutation>();
        builder.RegisterType<ElitismSelection>().As<ISelection>();
        builder.RegisterType<Population>().As<IPopulation>().SingleInstance();
        builder.RegisterType<Algorithm>();
        Container = builder.Build();
        var z = Container.Resolve<Algorithm>();
        var result = z.Run();
        Console.WriteLine("Offspring:");
        
        foreach (var days in result.Value)
        {
            string val = "";
            foreach (var person in days)
            {
                val += " " + person.Id + " ";
            }

            Console.WriteLine(val);
        }
    }
    
}


