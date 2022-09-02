using System.Reflection;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

internal class ScheduleContext : DbContext
{
    public DbSet<Machine> Machines { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Qualification> Qualifications { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Scheduling");
        if (!Directory.Exists(dbFilePath))
            Directory.CreateDirectory(dbFilePath);
        
        var dataSource = Path.Combine(dbFilePath, "ScheduleDB.db");

        optionsBuilder.UseSqlite($"Data Source={dataSource};");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ScheduleContext)) ?? throw new Exception("TBD"));
    }

}