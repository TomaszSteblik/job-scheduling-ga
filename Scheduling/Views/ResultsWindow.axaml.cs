using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Scheduling.ViewModels;

namespace Scheduling.Views;

public partial class ResultsWindow : ReactiveWindow<ResultsViewModel>
{
    public ResultsWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}