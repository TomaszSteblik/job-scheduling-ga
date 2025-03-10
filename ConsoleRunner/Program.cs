// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Autofac;
using CsvHelper;
using GeneticAlgorithm.Infrastructure;
using GeneticAlgorithm.Infrastructure.DependencyInjection;
using QLearning;
using Sat;
using SchedulingAlgorithmModels.Models;
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
            .MinimumLevel.Information()
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

        

         // var z = Container.Resolve<Algorithm>();
         // var resultGa = z.Run(machines, people);
         // resultGa.Chromosome.RecalculateFitness();
         // Log.Information("Resulting fitness GA BEST: {fitness}", resultGa.Chromosome.Fitness);
         // Log.Information("Resulting fitness GA WORST : {fitness}", resultGa.Worst.Fitness);

         // var sat = new SchedulingSatSolver();
         // var resultSat = sat.Run(people, machines);
         // resultSat.RecalculateFitness();
         //
         // Log.Information("Resulting fitness SAT: {fitness} {valid}", resultSat.Fitness, resultSat.IsValid(machines));
         
         //
         // var c = new QNetworkTrainer(machines, people);
         //  var b = new DeepQLearning();
         //  c.TrainQNetwork(b,1000,0,0);
         //  var s = c.GenerateSolution(b);
         //  s.RecalculateFitness();
         //  Log.Information("Resulting fitness Deep RL : {fitness} and {valid}", s.Fitness, s.IsValid(machines));


         await RunResults();

         //
         // var b = new DeepQLearning();
         // foreach (var i in Enumerable.Range(1,6))
         // {
         //     var peopleZ = GetPeopleFromCsv($"Data\\personel{i}.csv");
         //     var machinesZ = GetMachinesFromCsv($"Data\\machines{i}.csv");
         //     var c = new QNetworkTrainer(machinesZ, peopleZ);
         //     c.TrainQNetwork(b,1000, i, 7);
         //     var s = c.GenerateSolution(b);
         //     s.RecalculateFitness();
         //     Log.Information("Resulting fitness Deep RL : {fitness} and {valid}", s.Fitness, s.IsValid(machinesZ));
         // }
         //
         // b.model.save("C:\\Users\\tomasz.steblik\\RiderProjects\\job-scheduling-ga\\QLearning\\MODEL_VALID.model");



         // var q = new QLearning.SimpleQLearning(machines, people);
         //  q.TrainAgent(100);
         // var r = q.Run();
         // r.RecalculateFitness();
         // Log.Information("Resulting fitness RL : {fitness} and {valid}", r.Fitness, r.IsValid(machines));
         // r = q.Run();
         // r.RecalculateFitness();
         // Log.Information("Resulting fitness RL : {fitness} and {valid}", r.Fitness, r.IsValid(machines));
         // r = q.Run();
         // r.RecalculateFitness();
         // Log.Information("Resulting fitness RL : {fitness} and {valid}", r.Fitness, r.IsValid(machines));
         // r = q.Run();
         // r.RecalculateFitness();
         // Log.Information("Resulting fitness RL : {fitness} and {valid}", r.Fitness, r.IsValid(machines));

    }
    
    async static Task RunResults()
    {
         var results = new ConcurrentBag<Result>();
         var z = Container.Resolve<Algorithm>();

         for (int abc = 0; abc < 10; abc++)
         {
             var b = new DeepQLearning();
             var sat = new SchedulingSatSolver();

             b.model.load("C:\\Users\\tomasz.steblik\\RiderProjects\\job-scheduling-ga\\QLearning\\MODEL_VALID.model");

             foreach (var i in Enumerable.Range(3, 5))
             {
                 Person[] peopleZ;
                 Machine[] machinesZ;
                 Stopwatch stopwatch = new Stopwatch();

                     peopleZ = GetPeopleFromCsv($"Data\\personel_validate{i}.csv");
                     machinesZ = GetMachinesFromCsv($"Data\\machines_validate{i}.csv");
                 
                 var c = new QNetworkTrainer(machinesZ, peopleZ);
                 stopwatch.Start();
                 var s = c.GenerateSolution(b);
                 stopwatch.Stop();
                 var elapsedRL = stopwatch.Elapsed.Milliseconds;
                 stopwatch.Reset();
                 s.RecalculateFitness();
                 Log.Information("Resulting fitness Deep RL {2} : {0} and {1}", i, s.Fitness, s.IsValid(machinesZ));

                 stopwatch.Start();
                 var resultSat = sat.Run(peopleZ, machinesZ);
                 stopwatch.Stop();
                 var elapsedSAT = stopwatch.Elapsed.Milliseconds;
                 stopwatch.Reset();
                 resultSat.RecalculateFitness();

                 Log.Information("Resulting fitness SAT {2}: {fitness} {valid}", i, resultSat.Fitness,
                     resultSat.IsValid(machinesZ));

                 stopwatch.Start();
                 var resultGa = z.Run(machinesZ, peopleZ);
                 stopwatch.Stop();
                 var elapsedGA = stopwatch.Elapsed.Milliseconds;
                 stopwatch.Reset();
                 resultGa.Chromosome.RecalculateFitness();
                 Log.Information("Resulting fitness GA BEST {2}: {fitness}", i, resultGa.Chromosome.Fitness);
                 Log.Information("Resulting fitness GA WORST {2}: {fitness}", i, resultGa.Worst.Fitness);
                 results.Add(new Result()
                 {
                     SAT_Fitness = resultSat.Fitness,
                     DeepRL_Fitness = s.Fitness,
                     GA_Fitness = resultGa.Chromosome.Fitness,
                     Name = $"validate{i}",
                     MachineSize = machinesZ.Length,
                     WorkerSize = peopleZ.Length,
                     GA_Ms = elapsedGA,
                     SAT_Ms = elapsedSAT,
                     DeepRL_Ms = elapsedRL
                 });
             }

             foreach (var i in Enumerable.Range(1, 7))
             {
                 Stopwatch stopwatch = new Stopwatch();
                 var peopleZ = GetPeopleFromCsv($"Data\\personel{i}.csv");
                 var machinesZ = GetMachinesFromCsv($"Data\\machines{i}.csv");
                 var c = new QNetworkTrainer(machinesZ, peopleZ);
                 stopwatch.Start();
                 var s = c.GenerateSolution(b);
                 stopwatch.Stop();
                 var elapsedRL = stopwatch.Elapsed.Milliseconds;
                 stopwatch.Reset();
                 s.RecalculateFitness();
                 Log.Information("Resulting fitness Deep RL {2} : {0} and {1}", i, s.Fitness, s.IsValid(machinesZ));

                 stopwatch.Start();
                 var resultSat = sat.Run(peopleZ, machinesZ);
                 stopwatch.Stop();
                 var elapsedSAT = stopwatch.Elapsed.Milliseconds;
                 stopwatch.Reset();
                 resultSat.RecalculateFitness();

                 Log.Information("Resulting fitness SAT {2}: {fitness} {valid}", i, resultSat.Fitness,
                     resultSat.IsValid(machinesZ));

                 stopwatch.Start();
                 var resultGa = z.Run(machinesZ, peopleZ);
                 stopwatch.Stop();
                 var elapsedGA = stopwatch.Elapsed.Milliseconds;
                 stopwatch.Reset();
                 resultGa.Chromosome.RecalculateFitness();
                 Log.Information("Resulting fitness GA BEST {2}: {fitness}", i, resultGa.Chromosome.Fitness);
                 Log.Information("Resulting fitness GA WORST {2}: {fitness}", i, resultGa.Worst.Fitness);
                 results.Add(new Result()
                 {
                     SAT_Fitness = resultSat.Fitness,
                     DeepRL_Fitness = s.Fitness,
                     GA_Fitness = resultGa.Chromosome.Fitness,
                     Name = $"training{i}",
                     MachineSize = machinesZ.Length,
                     WorkerSize = peopleZ.Length,
                     GA_Ms = elapsedGA,
                     SAT_Ms = elapsedSAT,
                     DeepRL_Ms = elapsedRL
                 });
             }
         }

         var str = new StreamWriter("Data/results.csv");
         await using var csv = new CsvWriter(str, CultureInfo.InvariantCulture);
         //csv.WriteHeader(typeof(Result));
         await csv.WriteRecordsAsync(results);
    }
    
    class Result 
    {
        public string Name { get; set; }
        public int MachineSize { get; set; }
        public int WorkerSize { get; set; }
        public double DeepRL_Fitness { get; set; }
        public double SAT_Fitness { get; set; }
        public double GA_Fitness { get; set; }
        public int DeepRL_Ms { get; set; }
        public int SAT_Ms { get; set; }
        public int GA_Ms { get; set; }
    }
}