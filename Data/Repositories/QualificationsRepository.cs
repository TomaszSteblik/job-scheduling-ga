using System.Collections;
using AutoMapper;
using Data.Context;
using Data.Dtos.Read;
using Data.Dtos.Update;
using Data.Dtos.Write;
using Data.Entities;
using Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class QualificationsRepository : IQualificationsRepository
{
    private readonly ScheduleContext _context;
    private readonly IMapper _mapper;

    public QualificationsRepository(ScheduleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> AddQualification(QualificationWrite qualification)
    {
        await _context.Qualifications.AddAsync(_mapper.Map<Qualification>(qualification));
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<QualificationRead> GetQualification(int id)
    {
        var qualification = await _context.Qualifications.FirstOrDefaultAsync(x => x.Id == id) ??
                                  throw new ItemNotFoundException(nameof(Qualification), id);
        return _mapper.Map<QualificationRead>(qualification);
    }

    public async Task<IEnumerable<QualificationRead>> GetQualifications()
    {
        var qualifications = await _context.Qualifications.ToListAsync();
        return _mapper.Map<ICollection<QualificationRead>>(qualifications);
    }

    public async Task<bool> UpdateQualification(QualificationUpdate qualification)
    {
        var current = await _context.Qualifications.FindAsync(qualification.Id) ??
                      throw new ItemNotFoundException(nameof(Qualification), qualification.Id);
        current.Name = qualification.Name;
        return await _context.SaveChangesAsync() == 1;
    }
}