using Data.Context;

namespace Data.Repositories;

internal class QualificationsRepository : IQualificationsRepository
{
    private readonly ScheduleContext _context;

    public QualificationsRepository(ScheduleContext context)
    {
        _context = context;
    }
}