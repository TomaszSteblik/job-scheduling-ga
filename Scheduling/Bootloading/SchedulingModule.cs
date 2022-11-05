using Autofac;
using Scheduling.Models;
using Scheduling.Repositories;

namespace Scheduling.Bootloading;

public class SchedulingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AlgorithmSettings>().AsSelf().SingleInstance();
        builder.RegisterType<SelectedDataRepository>().AsImplementedInterfaces().SingleInstance();
        builder.RegisterType<DataRepository>().AsImplementedInterfaces();
    }
}