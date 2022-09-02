using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class MachinesRepository : IMachinesRepository
{
    private readonly ScheduleContext _context;

    public MachinesRepository(ScheduleContext context)
    {
        _context = context;
    }

    public async Task<bool> AddMachine(Machine machine)
    {
        await _context.Machines.AddAsync(machine);
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<Machine> GetMachine(int id)
    {
        return await _context.Machines.FindAsync(id) ?? throw new Exception("Not found");
    }

    public async Task<ICollection<Machine>> GetMachines()
    {
        return await _context.Machines.ToListAsync();
    }

    public async Task<bool> UpdateMachine(Machine machine)
    {
        var current = await _context.Machines.FindAsync(machine.Id);
        if (current is null)
            throw new Exception("Not found");

        current.Name = machine.Name;
        current.RequiredQualification = machine.RequiredQualification;
        return await _context.SaveChangesAsync() == 1;
    }
}