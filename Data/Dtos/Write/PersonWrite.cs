using Data.Dtos.Read;

namespace Data.Dtos.Write;

public class PersonWrite
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<MachineWrite>? PreferredMachines { get; set; }
    public ICollection<PropertyQualificationWrite>? Qualifications { get; set; }
}