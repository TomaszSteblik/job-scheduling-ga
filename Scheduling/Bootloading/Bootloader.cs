using Autofac;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace Scheduling.Bootloading;

internal static class Bootloader
{
    private static IContainer? _container;
    internal static void Setup()
    {
        var builder = new ContainerBuilder();
        var autofacResolver = builder.UseAutofacDependencyResolver();
        builder.RegisterInstance(autofacResolver);
        autofacResolver.InitializeReactiveUI();
        Locator.SetLocator(autofacResolver);
        InitializeLocator();
        builder.RegisterModule(new SchedulingModule());
        builder.RegisterViewModels();
        builder.AddAutoMapper();
        RegisterAvalonia();
        _container = builder.Build();
        autofacResolver.SetLifetimeScope(_container);
    }

    private static void InitializeLocator()
    {
        Locator.CurrentMutable.InitializeSplat();
        Locator.CurrentMutable.InitializeReactiveUI();
    }

    private static void RegisterAvalonia()
    {
        Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(), typeof(IActivationForViewFetcher));
        Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
    }
}