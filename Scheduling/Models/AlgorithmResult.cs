using System.Collections.Generic;

namespace Scheduling.Models;

public class AlgorithmResult
{
    public double Fitness { get; set; }
    public IEnumerable<object>? Schedule { get; set; }
}