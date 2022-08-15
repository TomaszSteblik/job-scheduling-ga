using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Scheduling.ViewModels;
using Splat;

namespace Scheduling.Views;

public partial class AlgorithmParametersUserControl : ReactiveUserControl<AlgorithmParametersViewModel>
{
    public AlgorithmParametersUserControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
        ViewModel = Locator.Current.GetService<AlgorithmParametersViewModel>();
    }
}