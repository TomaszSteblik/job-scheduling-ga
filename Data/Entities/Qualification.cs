namespace Data.Entities;

internal class Qualification
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Person> People { get; set; }
    public ICollection<Machine> Machines { get; set; }
}