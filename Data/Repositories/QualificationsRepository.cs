using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class QualificationsRepository : IQualificationsRepository
{
    private readonly ScheduleContext _context;

    public QualificationsRepository(ScheduleContext context)
    {
        _context = context;
    }

    public async Task<bool> AddQualification(Qualification qualification)
    {
        await _context.Qualifications.AddAsync(qualification);
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<Qualification> GetQualification(int id)
    {
        return await _context.Qualifications.FirstOrDefaultAsync(x => x.Id == id) ?? 
               throw new Exception("Not found exception");
    }

    public async Task<IEnumerable<Qualification>> GetQualifications()
    {
        return await _context.Qualifications.ToListAsync();
    }

    public async Task<bool> UpdateQualification(Qualification qualification)
    {
        var current = await GetQualification(qualification.Id);
        current.Name = qualification.Name;
        return await _context.SaveChangesAsync() == 1;
    }
}