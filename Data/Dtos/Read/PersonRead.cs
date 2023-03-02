namespace Data.Dtos.Read;

public class PersonRead
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int PreferenceDaysCount { get; set; }
    public ICollection<MachineRead>? PreferredMachines { get; set; }
    public ICollection<int>? PreferredDays { get; set; }
    public ICollection<QualificationRead>? Qualifications { get; set; }
}