namespace Data.Entities;

internal class Machine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Qualification RequiredQualification { get; set; }
}