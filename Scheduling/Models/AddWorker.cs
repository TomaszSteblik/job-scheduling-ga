using System.Collections.Generic;
using Data.Dtos.Read;

namespace Scheduling.Models;

public class AddWorker
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<int>? PreferredDays { get; set; }
    public ICollection<MachineRead>? PreferredMachines { get; set; }
    public ICollection<QualificationRead> Qualifications { get; set; }
}