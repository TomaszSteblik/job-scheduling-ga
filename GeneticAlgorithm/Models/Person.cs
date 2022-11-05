namespace GeneticAlgorithm.Models;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int PreferenceDaysCount { get; set; }
    public ICollection<int>? PreferredMachineIds { get; set; }
    public ICollection<int>? PreferredDays { get; set; }
    public ICollection<string>? Qualifications { get; set; }
    public override string ToString()
    {
        return Name ?? string.Empty;
    }
}