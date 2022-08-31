namespace GeneticAlgorithm.Models;

public class Result
{
    public Chromosome Chromosome { get; }
    public Parameters Parameters { get; }
    public Person[] People { get; }
    public Machine[] Machines { get; }
    
    public Result(Chromosome chromosome, Parameters parameters, Person[] people, Machine[] machines)
    {
        Chromosome = chromosome;
        Parameters = parameters;
        People = people;
        Machines = machines;
    }
}