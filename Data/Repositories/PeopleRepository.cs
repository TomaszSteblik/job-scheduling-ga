using Data.Context;

namespace Data.Repositories;

internal class PeopleRepository : IPeopleRepository
{
    private readonly ScheduleContext _context;

    public PeopleRepository(ScheduleContext context)
    {
        _context = context;
    }
}