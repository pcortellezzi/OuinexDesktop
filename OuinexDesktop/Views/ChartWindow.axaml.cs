using Avalonia.Controls;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;

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

public class ViewModel : ViewModels.ViewModelBase
{
    public ISeries[] Series { get; set; }
        = new ISeries[]
        {
                new LineSeries<double>
                {
                    Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                    Fill = null
                }
        };
}