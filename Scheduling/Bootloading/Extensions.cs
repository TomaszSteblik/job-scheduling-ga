using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;

namespace Scheduling.Bootloading;

internal static class Extensions
{
    private const string ViewmodelTypeNameFragment = "ViewModel";

    internal static ContainerBuilder RegisterViewModels(this ContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        var viewModelTypes = types.Where(x=>x.Name.Contains(ViewmodelTypeNameFragment));
        foreach (var viewModelType in viewModelTypes)
        {
            builder.RegisterType(viewModelType).AsSelf();
        }
        return builder;
    }

    internal static ContainerBuilder AddAutoMapper(this ContainerBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(assembly));
        var mapper = configuration.CreateMapper();
        builder.RegisterInstance(mapper).As<IMapper>();
        return builder;
    }
}