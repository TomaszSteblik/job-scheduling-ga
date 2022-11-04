using System.Collections.Generic;
using Scheduling.Models;

namespace Scheduling.Repositories;

class SelectedDataRepository : ISelectedDataRepository
{
    private IEnumerable<Worker> _workers;
    private IEnumerable<Machine> _machines;
    public SelectedDataRepository()
    {
        _workers = new List<Worker>();
        _machines = new List<Machine>();
    }
    
    public IEnumerable<Worker> GetWorkers()
    {
        return _workers;
    }

    public void SetWorkers(IEnumerable<Worker> workers)
    {
        _workers = workers;
    }

    public IEnumerable<Machine> GetMachines()
    {
        return _machines;
    }

    public void SetMachines(IEnumerable<Machine> machines)
    {
        _machines = machines;
    }
}