using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using OxyPlot;
using OxyPlot.Series;

namespace OuinexDesktop.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public ChartViewModel() 
        {
            InitTest();
        }

        public async Task Populate(string symbol)
        {
            

           await  Dispatcher.UIThread.InvokeAsync(new Action(async () =>
            {
                var client = new Binance.Net.Clients.BinanceClient();

                var request = await client.SpotApi.ExchangeData.GetUiKlinesAsync(symbol, Binance.Net.Enums.KlineInterval.OneHour, limit:200);

                if (request.Success)
                {
                    var test = new CandleStickSeries();
                   
                    int i = 0;

                    foreach(var data in request.Data)
                    {
                        test.Items.Add(new HighLowItem()
                        {
                             Close=(double)data.ClosePrice,
                             High=(double)data.HighPrice,
                             Low=(double)data.LowPrice,
                             Open=(double)data.OpenPrice,
                             X=i
                        });

                        i++;
                    }

                    this.Model.Series.Add(test);

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Model.InvalidatePlot(true);
                        Test();
                    }, DispatcherPriority.Background);
                }
            }));
        }

        public void Test()
        {
            Model.DefaultYAxis.AxislineStyle = LineStyle.Solid;
            Model.DefaultYAxis.MajorGridlineStyle = LineStyle.Solid;
            Model.DefaultXAxis.AxislineStyle = LineStyle.Solid;
            Model.DefaultXAxis.MajorGridlineStyle = LineStyle.Solid;

            Model.DefaultYAxis.Position = OxyPlot.Axes.AxisPosition.Right;
        }
        public void InitTest()
        {
            // Create the plot model
            var tmp = new PlotModel {   };

            // Create two line series (markers are hidden by default)
           /* var series1 = new LineSeries { Title = "Series 1", MarkerType = MarkerType.Circle };
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(10, 18));
            series1.Points.Add(new DataPoint(20, 12));
            series1.Points.Add(new DataPoint(30, 8));
            series1.Points.Add(new DataPoint(40, 15));

            var series2 = new LineSeries { Title = "Series 2", MarkerType = MarkerType.Square };
            series2.Points.Add(new DataPoint(0, 4));
            series2.Points.Add(new DataPoint(10, 12));
            series2.Points.Add(new DataPoint(20, 16));
            series2.Points.Add(new DataPoint(30, 25));
            series2.Points.Add(new DataPoint(40, 5));

            // Add the series to the plot model
            tmp.Series.Add(series1);
            tmp.Series.Add(series2);*/

            // Axes are created automatically if they are not defined

            // Set the Model property, the INotifyPropertyChanged event will make the WPF Plot control update its content
            this.Model = tmp;
        }

        public PlotModel Model { get; private set; }
    }
}
