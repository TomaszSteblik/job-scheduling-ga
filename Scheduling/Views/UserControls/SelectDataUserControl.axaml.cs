using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Scheduling.ViewModels;
using Splat;

namespace Scheduling.Views.UserControls;

public partial class SelectDataUserControl : ReactiveUserControl<SelectDataViewModel>
{
    public SelectDataUserControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
        ViewModel = Locator.Current.GetService<SelectDataViewModel>();
    }
}