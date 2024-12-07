using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using Binance.Net.Clients;
using Binance.Net.Enums;
using ReactiveUI;
using ScottPlot;
using StockPlot.Charts.Controls;
using StockPlot.Charts.Models;

namespace OuinexDesktop.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        BinanceSocketClient socket = new BinanceSocketClient();
        private string _symbol;
        private KlineInterval _selectedInterval = KlineInterval.OneMinute;

        public ChartViewModel() 
        {
            CreateTrendLineCommand = ReactiveCommand.Create(() => { Chart.DrawingManager.EnableDrawingMode(StockPlot.Charts.DrawType.TrendLine); });

        }

        public async Task Populate(string symbol)
        {
            _symbol = symbol;
            IsEmptyOfData = false;
            IsBusy = true;

            await socket.UnsubscribeAllAsync();

            var client = new BinanceClient();

            var request = await client.SpotApi.ExchangeData.GetUiKlinesAsync(_symbol, _selectedInterval, limit: 500);

            if (request.Success)
            {
                var bars = request.Data.Select(x => new OHLC((double)x.OpenPrice, (double)x.HighPrice, (double)x.LowPrice, (double)x.ClosePrice, x.OpenTime, TimeSpan.FromSeconds((int)_selectedInterval))).ToArray();

                var _chartModel = new StockPricesModel(false);
                _chartModel.Append(bars);
                Chart.PricesModel = _chartModel;
                
                 
                IsBusy = false;
                await socket.SpotStreams.SubscribeToKlineUpdatesAsync(_symbol, _selectedInterval, async (data) =>
                {
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        var candle = data.Data.Data;

                        var toUpdate = _chartModel.Prices.FirstOrDefault(x => x.DateTime == candle.OpenTime);

                        if (toUpdate != null)
                        {
                            toUpdate.Volume = (double)candle.Volume;
                            toUpdate.High = (double)candle.HighPrice;
                            toUpdate.Close = (double)candle.ClosePrice;
                            toUpdate.Low = (double)candle.LowPrice;

                            _chartModel.UpdateBar(toUpdate);
                        }
                        else
                        {
                            var newBar = new OHLC((double)candle.OpenPrice, (double)candle.HighPrice, (double)candle.LowPrice, (double)candle.ClosePrice, candle.OpenTime, TimeSpan.FromSeconds((int)_selectedInterval));
                            _chartModel.Append(newBar);
                        }
                    }, DispatcherPriority.Background);
                });               
            }
        }

        public StockChart Chart { get; } = new StockChart() 
        { 
            DisplayPrice = StockPlot.Charts.DisplayPrice.Candlestick,
            CandleUpColor = Avalonia.Media.Brush.Parse("#2ABD8B"),
            CandleWickColor= Avalonia.Media.Brushes.Black
        };

        public List<KlineInterval> Intervals { get; } = Enum.GetValues(typeof(KlineInterval)).Cast<KlineInterval>().ToList();

        public KlineInterval SelectedInterval
        {
            get => _selectedInterval;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedInterval, value);

                Task.Run(async () => await Dispatcher.UIThread.InvokeAsync(async () => await Populate(_symbol)));
            }
        }

        public ICommand CreateTrendLineCommand { get; }
    }
}
