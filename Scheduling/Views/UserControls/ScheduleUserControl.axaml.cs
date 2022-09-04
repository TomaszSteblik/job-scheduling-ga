using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Scheduling.ViewModels;

namespace Scheduling.Views.UserControls;

public partial class ScheduleUserControl : ReactiveUserControl<ScheduleViewModel>
{
    public ScheduleUserControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}