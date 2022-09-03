using Autofac;
using Data.Context;
using Data.Repositories;

namespace Data.DependencyInjection;

public class DataModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ScheduleContext>().AsSelf();
        builder.RegisterType<MachinesRepository>().AsImplementedInterfaces();
        builder.RegisterType<PeopleRepository>().AsImplementedInterfaces();
        builder.RegisterType<QualificationsRepository>().AsImplementedInterfaces();
    }
}