namespace GeneticAlgorithm.Models;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public ICollection<Qualification>? Qualifications { get; set; }
    public override string ToString()
    {
        if (Name is null)
            throw new ArgumentNullException(nameof(Name));
        return Name;
    }
}