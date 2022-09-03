using System.Collections;
using Data.Dtos.Update;
using AutoMapper;
using Data.Context;
using Data.Dtos.Read;
using Data.Dtos.Write;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class PeopleRepository : IPeopleRepository
{
    private readonly ScheduleContext _context;
    private readonly IMapper _mapper;

    public PeopleRepository(ScheduleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> AddPerson(PersonWrite person)
    {
        await _context.People.AddAsync(_mapper.Map<Person>(person));
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<PersonRead> GetPerson(int id)
    {
        var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id) ?? 
                     throw new Exception("Not found");
        return _mapper.Map<PersonRead>(person);
    }

    public async Task<ICollection<PersonRead>> GetPeople()
    {
        var people = await _context.People.ToListAsync();
        return _mapper.Map<ICollection<PersonRead>>(people);
    }

    public async Task<bool> UpdatePerson(PersonUpdate person)
    {
        var current = await _context.People.FindAsync(person.Id) ?? throw new Exception("not found");
        current.Qualifications = _mapper.Map<ICollection<Qualification>>(person.Qualifications);
        current.FirstName = person.FirstName;
        current.LastName = person.LastName;
        current.PreferredMachines = _mapper.Map<ICollection<Machine>>(person.PreferredMachines);
        return await _context.SaveChangesAsync() == 1;
    }
}