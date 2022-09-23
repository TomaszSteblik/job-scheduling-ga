using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Serilog;

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
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(assemblies));
        var mapper = configuration.CreateMapper();
        builder.RegisterInstance(mapper).As<IMapper>();
        return builder;
    }

    internal static ContainerBuilder AddSerilog(this ContainerBuilder builder)
    {
        var log = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(GetSchedulingPath())
            .MinimumLevel.Debug()
            .CreateLogger();
        Log.Logger = log;
        builder.RegisterInstance<ILogger>(log);
        return builder;
    }

    private static string GetSchedulingPath() =>  
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "Scheduling",$"log_{DateTime.Now.ToString(CultureInfo.CurrentCulture)}.txt");
    }