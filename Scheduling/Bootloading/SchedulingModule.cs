using Autofac;
using Scheduling.Models;

namespace Scheduling.Bootloading;

public class SchedulingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<AlgorithmSettings>().AsSelf().SingleInstance();
    }
}