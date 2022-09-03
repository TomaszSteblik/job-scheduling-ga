using Data.Dtos.Read;

namespace Data.Dtos.Write;

public class MachineWrite
{
    public string? Name { get; set; }
    public PropertyQualificationWrite? RequiredQualification { get; set; }
}