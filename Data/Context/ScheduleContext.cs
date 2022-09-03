using System.Reflection;
using Data.Entities;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

internal class ScheduleContext : DbContext
{
    public DbSet<Machine> Machines { get; set; } = null!;
    public DbSet<Person> People { get; set; } = null!;
    public DbSet<Qualification> Qualifications { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseUserSqlite();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ScheduleContext)) ?? throw new Exception("TBD"));
    }
}