using ReactiveUI;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;

namespace OuinexDesktop.Charting
{
    public enum ChartType
    {
        Candlestick,
        OHLC,
        Line
    } 

    public class StockChart : ReactiveObject
    {
        #region private fields
        private ChartType _chartType = ChartType.Candlestick;
        private AvaPlot _plot;
        private Crosshair _crossHair;





        #endregion

        #region Constructor
        public StockChart()
        {
            // init
            setupBases();
        }
        #endregion

        public ChartType ChartType
        {
            get => _chartType;
            set=> this.RaiseAndSetIfChanged(ref _chartType, value);
        }

        public AvaPlot MainPlotArea
        {
            get => _plot;
            private set => _plot = value;
        }

        private void setupBases()
        {
            MainPlotArea = new AvaPlot();

            _crossHair = MainPlotArea.Plot.AddCrosshair(0, 0);
            _crossHair.IgnoreAxisAuto = true;
            _crossHair.LineStyle = ScottPlot.LineStyle.Solid;
            _crossHair.LineWidth = 1;
            _crossHair.Color = System.Drawing.Color.Black;

            MainPlotArea.PointerMoved += (o, e) =>
            {
                (double coordinateX, double coordinateY) = MainPlotArea.GetMouseCoordinates();

                _crossHair.X = coordinateX;
                _crossHair.Y = coordinateY;                 

                MainPlotArea.Refresh();
            };
        }

        public void GenerateTestDatas()
        {
            var datas = DataGen.RandomStockPrices(new Random(), 500);

            foreach(var data in datas )
            {

            }
        }
    }
}