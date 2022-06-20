namespace GeneticAlgorithm.Models;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public ICollection<Qualification>? Qualifications { get; set; }
    public override string ToString()
    {
        return Name ?? "NO_NAME";
    }
}