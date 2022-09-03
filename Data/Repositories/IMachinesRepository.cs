using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;

namespace Data.Repositories;

public interface IMachinesRepository
{
    Task<bool> AddMachine(MachineWrite machineDto);
    Task<MachineRead> GetMachine(int id);
    Task<ICollection<MachineRead>> GetMachines();
    Task<bool> UpdateMachine(MachineUpdate machine);
}