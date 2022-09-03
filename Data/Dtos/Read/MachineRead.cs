namespace Data.Dtos.Read;

public class MachineRead
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public QualificationRead? RequiredQualification { get; set; }
}