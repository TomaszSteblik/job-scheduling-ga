namespace GeneticAlgorithm.Models;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int PreferenceDays { get; set; }
    public int PreferredMachineId { get; set; }
    public ICollection<Qualification>? Qualifications { get; set; }
    public override string ToString()
    {
        return Name ?? "NO_NAME";
    }
}