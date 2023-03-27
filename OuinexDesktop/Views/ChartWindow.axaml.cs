using Avalonia.Controls;
namespace OuinexDesktop.Views;

public partial class ChartWindow : Window
{
    public ChartWindow()
    {
        InitializeComponent();
        Loaded += ChartWindow_Loaded;
    }

    private void ChartWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}