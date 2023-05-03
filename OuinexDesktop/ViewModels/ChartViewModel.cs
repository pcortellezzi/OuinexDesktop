using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using Binance.Net.Clients;
using Microsoft.CodeAnalysis.FlowAnalysis;
using ScottPlot;
using StockPlot.Charts.Controls;
using StockPlot.Charts.Models;

namespace OuinexDesktop.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public ChartViewModel() 
        {
            
            
        }

        public async Task Populate(string symbol)
        {
            IsEmptyOfData = false;
            IsBusy = true;

            var client = new BinanceClient();

            var request = await client.SpotApi.ExchangeData.GetUiKlinesAsync(symbol, Binance.Net.Enums.KlineInterval.OneHour, limit: 500);

            if (request.Success)
            {
                var bars = request.Data.Select(x => new OHLC((double)x.OpenPrice, (double)x.HighPrice, (double)x.LowPrice, (double)x.ClosePrice, x.OpenTime, TimeSpan.FromMinutes(60))).ToArray();

                var _chartModel = new StockPricesModel(false);
                _chartModel.Append(bars);
                Chart.PricesModel = _chartModel;
                var socket = new BinanceSocketClient();


                IsBusy = false;
                await socket.SpotStreams.SubscribeToKlineUpdatesAsync(symbol, Binance.Net.Enums.KlineInterval.OneHour, async (data) =>
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
                            var newBar = new OHLC((double)candle.OpenPrice, (double)candle.HighPrice, (double)candle.LowPrice, (double)candle.ClosePrice, candle.OpenTime, TimeSpan.FromMinutes(60));
                            _chartModel.Append(newBar);
                        }
                    }, DispatcherPriority.Background);
                });               
            }
        }

        public StockChart Chart { get; } = new StockChart() { DisplayPrice = StockPlot.Charts.DisplayPrice.OHLC };
    }
}
