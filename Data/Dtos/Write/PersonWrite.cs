namespace Data.Dtos.Write;

public class PersonWrite
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int PreferenceDaysCount { get; set; }
    public ICollection<PropertyMachineWrite>? PreferredMachines { get; set; }
    public ICollection<int>? PreferredDays { get; set; }
    public ICollection<PropertyQualificationWrite>? Qualifications { get; set; }
}