﻿using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace OuinexDesktop.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public Axis[] XAxes { get; set; } =
   {
        new Axis
        {
            Labeler = value => new DateTime((long)value).ToString("yyyy MMM dd"),
            // set the unit width of the axis to "days"
            // since our X axis is of type date time and 
            // the interval between our points is in days
            UnitWidth = TimeSpan.FromDays(1).Ticks,
           
        }
    };

        public async Task Populate(string symbol)
        {
           await  Dispatcher.UIThread.InvokeAsync(new Action(async () =>
            {
                var client = new Binance.Net.Clients.BinanceClient();

                var request = await client.SpotApi.ExchangeData.GetUiKlinesAsync(symbol, Binance.Net.Enums.KlineInterval.OneHour);

                if (request.Success)
                {
                    var datas = new ObservableCollection<FinancialPoint>();

                    foreach (var data in request.Data)
                    {
                        datas.Add(new FinancialPoint(data.OpenTime, 
                            (double)data.HighPrice, 
                            (double)data.OpenPrice, 
                            (double)data.ClosePrice,
                            (double)data.LowPrice));
                    }

                    Series[0].Values = datas;
                }
            }));
        }
        public ISeries[] Series { get; set; } =
        {
        new CandlesticksSeries<FinancialPoint>
        {
            Values = new ObservableCollection<FinancialPoint>
            {
                //                      date, high, open, close, low
                new(new DateTime(2021, 1, 1), 523, 500, 450, 400),
                new(new DateTime(2021, 1, 2), 500, 450, 425, 400),
                new(new DateTime(2021, 1, 3), 490, 425, 400, 380),
                new(new DateTime(2021, 1, 4), 420, 400, 420, 380),
                new(new DateTime(2021, 1, 5), 520, 420, 490, 400),
                new(new DateTime(2021, 1, 6), 580, 490, 560, 440),
                new(new DateTime(2021, 1, 7), 570, 560, 350, 340),
                new(new DateTime(2021, 1, 8), 380, 350, 380, 330),
                new(new DateTime(2021, 1, 9), 440, 380, 420, 350),
                new(new DateTime(2021, 1, 10), 490, 420, 460, 400),
                new(new DateTime(2021, 1, 11), 520, 460, 510, 460),
                new(new DateTime(2021, 1, 12), 580, 510, 560, 500),
                new(new DateTime(2021, 1, 13), 600, 560, 540, 510),
                new(new DateTime(2021, 1, 14), 580, 540, 520, 500),
                new(new DateTime(2021, 1, 15), 580, 520, 560, 520),
                new(new DateTime(2021, 1, 16), 590, 560, 580, 520),
                new(new DateTime(2021, 1, 17), 650, 580, 630, 550),
                new(new DateTime(2021, 1, 18), 680, 630, 650, 600),
                new(new DateTime(2021, 1, 19), 670, 650, 600, 570),
                new(new DateTime(2021, 1, 20), 640, 600, 610, 560),
                new(new DateTime(2021, 1, 21), 630, 610, 630, 590),
            }
        }
    };
    }
}