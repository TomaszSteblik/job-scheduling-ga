using System.Collections.Generic;
using Scheduling.Models;

namespace Scheduling.Repositories;

class SelectedDataRepository : ISelectedDataRepository
{
    public IEnumerable<Worker> GetWorkers()
    {
        throw new System.NotImplementedException();
    }

    public void SetWorkers(IEnumerable<Worker> workers)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Machine> GetMachines()
    {
        throw new System.NotImplementedException();
    }

    public void SetMachines(IEnumerable<Machine> machines)
    {
        throw new System.NotImplementedException();
    }
}