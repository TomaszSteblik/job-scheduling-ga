using System.Collections.Generic;

namespace Scheduling.Models;

public class Worker
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<int>? PreferredDays { get; set; }
    public ICollection<Machine>? PreferredMachines { get; set; }
    public ICollection<Qualification> Qualifications { get; set; }
}