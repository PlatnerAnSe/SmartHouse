using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace SmartHouse;

public partial class ADDWindow : Window
{
    public ADDWindow()
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