namespace Data.Dtos.Update;

public class MachineUpdate
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public QualificationUpdate RequiredQualification { get; set; }
}