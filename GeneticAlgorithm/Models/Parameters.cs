namespace GeneticAlgorithm.Models;

public class Parameters
{
    public int PopulationSize { get; set; }
    public int EpochsCount { get; set; }
    public int InstancesToRun { get; set; }
    public double MutationProbability { get; set; }
    public int ParentsPerChild { get; set; }
    public int ChildrenCount { get; set; }
    public string Selection { get; set; }
    public string Elimination { get; set; }
    public string Mutation { get; set; }
    public string Crossover { get; set; }
    public string DataPathMachines { get; set; }
    public string DataPathPersonel { get; set; }
}