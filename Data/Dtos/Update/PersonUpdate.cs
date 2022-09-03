namespace Data.Dtos.Update;

public class PersonUpdate
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<MachineUpdate>? PreferredMachines { get; set; }
    public ICollection<QualificationUpdate>? Qualifications { get; set; }
}