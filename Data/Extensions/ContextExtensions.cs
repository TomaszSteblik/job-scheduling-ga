using Microsoft.EntityFrameworkCore;

namespace Data.Extensions;

internal static class ContextExtensions
{
    internal static void UseUserSqlite(this DbContextOptionsBuilder optionsBuilder)
    {
        var dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Scheduling");
        if (!Directory.Exists(dbFilePath))
            Directory.CreateDirectory(dbFilePath);
        
        var dataSource = Path.Combine(dbFilePath, "ScheduleDB.db");

        optionsBuilder.UseSqlite($"Data Source={dataSource};");
    }
}