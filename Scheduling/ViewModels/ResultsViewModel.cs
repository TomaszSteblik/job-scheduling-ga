using ReactiveUI.Fody.Helpers;
using Scheduling.Models;
using Splat;

namespace Scheduling.ViewModels;

public class ResultsViewModel : ViewModelBase
{
    public ScheduleViewModel ScheduleViewModel { get; set; }

    public ResultsViewModel(AlgorithmResult algorithmResult)
    {
        ScheduleViewModel = Locator.Current.GetService<ScheduleViewModel>();
        ScheduleViewModel.ScheduleResult = algorithmResult;
    }
}