using System.Collections.Generic;
using System.Threading.Tasks;
using Scheduling.Models;

namespace Scheduling.Repositories;

public interface IDataRepository
{
    Task<IEnumerable<Worker>> GetWorkers();
    Task<IEnumerable<Machine>> GetMachines();
}