using Avalonia.Controls;
using ScottPlot.Avalonia;
using ScottPlot;
using System;

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
       /* avaPlot1 = this.Find<AvaPlot>("avaPlot1");


        OHLC[] prices = DataGen.RandomStockPrices(null, 300, TimeSpan.FromMinutes(5));
        var candlePlot = avaPlot1.Plot.AddOHLCs(prices);
        candlePlot.YAxisIndex = 1;
        avaPlot1.Plot.XAxis.DateTimeFormat(true);

        avaPlot1.Plot.YAxis.Ticks(false);
        avaPlot1.Plot.YAxis2.Ticks(true);
        avaPlot1.Plot.YAxis2.Label("Price (USD)");
        avaPlot1.Refresh();*/
    }
}