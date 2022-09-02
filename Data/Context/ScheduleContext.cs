using System.Reflection;
using Data.Extensions;
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
        optionsBuilder.UseUserSqlite();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ScheduleContext)) ?? throw new Exception("TBD"));
    }
}