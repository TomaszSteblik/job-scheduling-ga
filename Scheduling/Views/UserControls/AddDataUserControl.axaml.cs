using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Scheduling.Views.UserControls;

public partial class AddDataUserControl : UserControl
{
    public AddDataUserControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}