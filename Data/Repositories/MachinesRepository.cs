using Data.Context;

namespace Data.Repositories;

internal class MachinesRepository : IMachinesRepository
{
    private readonly ScheduleContext _context;

    public MachinesRepository(ScheduleContext context)
    {
        _context = context;
    }
}