using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SukiUI.Controls;

namespace OuinexDesktop.Views.Controls;

public partial class DialogContent : UserControl
{
    public DialogContent()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
       InteractiveContainer.CloseDialog();
    }
}