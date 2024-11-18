namespace SchedulingAlgorithmModels.Models;

public class Result
{
    public Chromosome Chromosome { get; }
    public Parameters Parameters { get; }
    public Person[] People { get; }
    public Machine[] Machines { get; }
    public Chromosome Worst { get; set; }

    public Result(Chromosome chromosome, Parameters parameters, Person[] people, Machine[] machines, Chromosome worst)
    {
        Chromosome = chromosome;
        Parameters = parameters;
        People = people;
        Machines = machines;
        Worst = worst;
    }
}