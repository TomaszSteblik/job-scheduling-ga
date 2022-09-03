using Avalonia;
using System;
using System.Threading.Tasks;
using Scheduling.Bootloading;

namespace Scheduling;

internal static class Program
{
    
    [STAThread]
    public static async Task Main(string[] args)
    {
        await Bootloader.Setup();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}