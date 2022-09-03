using Autofac;
using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Extensions;

public static class BootstrapExtensions
{
    public static async Task ApplyMigrations(this IContainer container)
    {
        await using var context = container.Resolve<ScheduleContext>();
        await context.Database.MigrateAsync();
    }
}