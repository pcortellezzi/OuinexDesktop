using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Avalonia.Threading;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;

namespace OuinexDesktop.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        ScottPlot.Plottable.Crosshair Crosshair;
        public ChartViewModel() 
        {
            MainChart.Plot.XAxis.DateTimeFormat(true);
        }

        public async Task Populate(string symbol)
        {           

           await  Dispatcher.UIThread.InvokeAsync(new Action(async () =>
            {
                var client = new Binance.Net.Clients.BinanceClient();

                var request = await client.SpotApi.ExchangeData.GetUiKlinesAsync(symbol, Binance.Net.Enums.KlineInterval.OneHour);

                if (request.Success)
                {
                    List<OHLC> prices = new List<OHLC>();               
                   

                    foreach (var data in request.Data)
                    {
                        prices.Add(new OHLC((double)data.OpenPrice,
                            (double)data.HighPrice,
                            (double)data.LowPrice, 
                            (double)data.ClosePrice, 
                            data.OpenTime,
                            new TimeSpan(1,0,0)));
                    }
                   

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        var test = MainChart.Plot.AddOHLCs(prices.ToArray());

                       
                        var bol = test.GetBollingerBands(20);
                        
                        MainChart.Plot.AddScatterLines(bol.xs, bol.sma, Color.Blue);
                        MainChart.Plot.AddScatterLines(bol.xs, bol.lower, Color.Blue, lineStyle: LineStyle.Dash);
                        MainChart.Plot.AddScatterLines(bol.xs, bol.upper, Color.Blue, lineStyle: LineStyle.Dash);

                        MainChart.Plot.AddFill(bol.xs, bol.upper, bol.xs, bol.lower);
                        //   candlePlot.YAxisIndex = 1;
                        MainChart.Refresh();

                    }, DispatcherPriority.Background);
                }
            }));
        }
        
        public AvaPlot MainChart { get; private set; } = new AvaPlot();
    }
}
