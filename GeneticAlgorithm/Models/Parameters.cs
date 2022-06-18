using System.Text.Json.Serialization;
using GeneticAlgorithm.Models.Enums;

namespace GeneticAlgorithm.Models;

public class Parameters
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
    public string DataPathMachines { get; set; }
    public string DataPathPersonel { get; set; }
}