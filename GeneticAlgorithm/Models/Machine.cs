using CsvHelper.Configuration.Attributes;

namespace GeneticAlgorithm.Models;

public class Machine
{
    [Name("qualification")]
    public Qualification RequiredQualification{ get; set; }
    [Name("personelCount")]
    public int PersonelCount { get; set; }
    [Name("name")]
    public string Name { get; set; }
}