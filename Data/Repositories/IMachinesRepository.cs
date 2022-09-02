using Data.Models;

namespace Data.Repositories;

public interface IMachinesRepository
{
    Task<bool> AddMachine(Machine machine);
    Task<Machine> GetMachine(int id);
    Task<ICollection<Machine>> GetMachines();
    Task<bool> UpdateMachine(Machine machine);
}