using CsvHelper.Configuration.Attributes;

namespace GeneticAlgorithm.Models;

public class Machine
{
    [Name("qualification")]
    public Qualification RequiredQualification{ get; set; }
    [Name("name")]
    public string? Name { get; set; }
}