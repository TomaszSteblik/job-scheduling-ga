namespace Data.Entities;

internal class Machine
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int QualificationId { get; set; }
    public Qualification? RequiredQualification { get; set; }
    public ICollection<Person> People { get; set; }
}