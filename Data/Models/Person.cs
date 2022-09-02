namespace Data.Models;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Machine>? PreferredMachines { get; set; }
    public ICollection<Qualification>? Qualifications { get; set; }
}