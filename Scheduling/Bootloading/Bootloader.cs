using Autofac;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace Scheduling.Bootloading;

internal static class Bootloader
{
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
        var container = builder.Build();
        autofacResolver.SetLifetimeScope(container);
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