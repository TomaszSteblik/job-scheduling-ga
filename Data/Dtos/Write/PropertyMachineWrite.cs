namespace Data.Dtos.Write;

public class PropertyMachineWrite
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public PropertyQualificationWrite? RequiredQualification { get; set; }
}