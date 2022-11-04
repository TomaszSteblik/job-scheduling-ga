using ReactiveUI.Fody.Helpers;
using Scheduling.Models;

namespace Scheduling.ViewModels;

public class ScheduleViewModel : ViewModelBase
{
    [Reactive]
    public AlgorithmResult? ScheduleResult { get; set; }

    public ScheduleViewModel()
    {

    }

}