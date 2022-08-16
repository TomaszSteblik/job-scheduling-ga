using Avalonia;
using System;
using Scheduling.Bootloading;

namespace Scheduling;

internal static class Program
{
    
    [STAThread]
    public static void Main(string[] args)
    {
        Bootloader.Setup();
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}