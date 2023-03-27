using Avalonia.Controls;
using ScottPlot.Avalonia;
using Steema.TeeChart.Avalonia;
using Steema.TeeChart.Styles;
using System.Threading.Tasks;

namespace OuinexDesktop.Views.Controls
{
    public partial class ChartControl : UserControl
    {
        public ChartControl()
        {
            InitializeComponent();
        }

        public async Task CreateCandles(string symbol)
        {

            //  this.priceChart.Series.Add(test);
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            AvaPlot avaPlot1 = this.Find<AvaPlot>("AvaPlot1");
            avaPlot1.Plot.AddScatter(dataX, dataY);
            avaPlot1.Refresh();

            var request = new Binance.Net.Clients.BinanceClient();
            var datas =  await request.SpotApi.ExchangeData.GetUiKlinesAsync(symbol, Binance.Net.Enums.KlineInterval.OneMinute);
        }
    }
}
