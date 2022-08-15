using GeneticAlgorithm.Models.Enums;

namespace Scheduling.Models;

public class AlgorithmSettings
{
    public int PopulationSize { get; set; }
    public int EpochsCount { get; set; }
    public int InstancesToRun { get; set; }
    public double MutationProbability { get; set; }
    public int ParentsPerChild { get; set; }
    public int ChildrenCount { get; set; }
    public Selection Selection { get; set; }
    public Elimination Elimination { get; set; }
    public Mutation Mutation { get; set; }
    public Crossover Crossover { get; set; }
}