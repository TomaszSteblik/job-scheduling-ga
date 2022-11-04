using System.Collections.Generic;
using Scheduling.Models;

namespace Scheduling.Repositories;

public interface ISelectedDataRepository
{
    IEnumerable<Worker> GetWorkers();
    void SetWorkers(IEnumerable<Worker> workers);

    IEnumerable<Machine> GetMachines();
    void SetMachines(IEnumerable<Machine> machines);
}