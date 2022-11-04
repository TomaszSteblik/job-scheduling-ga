using Data.Dtos.Read;

namespace Scheduling.Models;

public class AddMachine
{
    public string Name { get; set; }
    public QualificationRead RequiredQualification { get; set; }
}