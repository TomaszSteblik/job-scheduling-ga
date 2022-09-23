using System;
using ReactiveUI;
using Serilog;

namespace Scheduling.Helpers;

public static class ReactiveCommandExtensions
{
    public static void LogExceptions(this IHandleObservableErrors command)
    {
        command.ThrownExceptions.Subscribe(x=> 
            Log.Error("Message: {Message}. On: {StackTrace}",
                x.Message, x.StackTrace));
    }
}