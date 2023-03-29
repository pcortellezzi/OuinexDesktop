using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using OuinexDesktop.Views.Controls;
using SukiUI.Controls;

namespace OuinexDesktop.Views;

public partial class FreshWindow : Window
{
    public FreshWindow()
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
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        InteractiveContainer.ShowDialog(new DialogContent());
    }
}