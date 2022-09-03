using AutoMapper;
using Data.Context;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class MachinesRepository : IMachinesRepository
{
    private readonly ScheduleContext _context;
    private readonly IMapper _mapper;

    public MachinesRepository(ScheduleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> AddMachine(MachineWrite machineDto)
    {
        var machine = _mapper.Map<Machine>(machineDto);
        await _context.Machines.AddAsync(machine);
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<MachineRead> GetMachine(int id)
    {
        var machine = await _context.Machines.FindAsync(id) ?? throw new Exception("Not found");
        return _mapper.Map<MachineRead>(machine);
    }

    public async Task<ICollection<MachineRead>> GetMachines()
    {
        var machines = await _context.Machines.ToListAsync();
        return _mapper.Map<ICollection<MachineRead>>(machines);
    }

    public async Task<bool> UpdateMachine(MachineUpdate machine)
    {
        var current = await _context.Machines.FindAsync(machine.Id);
        if (current is null)
            throw new Exception("Not found");

        current.Name = machine.Name;
        current.RequiredQualification = _mapper.Map<Qualification>(machine.RequiredQualification);
        return await _context.SaveChangesAsync() == 1;
    }
}