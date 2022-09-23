using Microsoft.EntityFrameworkCore;

namespace Data.Extensions;

internal static class ContextExtensions
{
    internal static void UseUserSqlite(this DbContextOptionsBuilder optionsBuilder)
    {
        var dbFilePath = GetSchedulingPath();
        if (!Directory.Exists(dbFilePath))
            Directory.CreateDirectory(dbFilePath);
        
        var dataSource = Path.Combine(dbFilePath, "ScheduleDB.db");

        optionsBuilder.UseSqlite($"Data Source={dataSource};");
    }

    private static string GetSchedulingPath() =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Scheduling");

}