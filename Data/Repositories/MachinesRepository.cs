using AutoMapper;
using Data.Context;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using Data.Entities;
using Data.Exceptions;
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
        machine.RequiredQualification = await _context.Qualifications.FindAsync(machine.QualificationId);
        await _context.Machines.AddAsync(machine);
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<MachineRead> GetMachine(int id)
    {
        var machine = await _context.Machines.FindAsync(id) ??
                      throw new ItemNotFoundException(nameof(Machine), id);
        return _mapper.Map<MachineRead>(machine);
    }

    public async Task<ICollection<MachineRead>> GetMachines()
    {
        var machines = await _context.Machines.Include(x=>x.RequiredQualification).ToListAsync();
        return _mapper.Map<ICollection<MachineRead>>(machines);
    }

    public async Task<bool> UpdateMachine(MachineUpdate machine)
    {
        var current = await _context.Machines.FindAsync(machine.Id);
        if (current is null)
            throw new ItemNotFoundException(nameof(Machine), machine.Id);

        current.Name = machine.Name;
        current.RequiredQualification = _mapper.Map<Qualification>(machine.RequiredQualification);
        return await _context.SaveChangesAsync() == 1;
    }
}