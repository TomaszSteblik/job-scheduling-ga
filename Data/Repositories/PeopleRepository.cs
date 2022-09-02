using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class PeopleRepository : IPeopleRepository
{
    private readonly ScheduleContext _context;

    public PeopleRepository(ScheduleContext context)
    {
        _context = context;
    }

    public async Task<bool> AddPerson(Person person)
    {
        await _context.People.AddAsync(person);
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<Person> GetPerson(int id)
    {
        return await _context.People.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Not found");
    }

    public async Task<ICollection<Person>> GetPeople()
    {
        return await _context.People.ToListAsync();
    }

    public async Task<bool> UpdatePerson(Person person)
    {
        var current = await GetPerson(person.Id);
        current.Qualifications = person.Qualifications;
        current.FirstName = person.FirstName;
        current.LastName = person.LastName;
        current.PreferredMachines = person.PreferredMachines;
        return await _context.SaveChangesAsync() == 1;
    }
}