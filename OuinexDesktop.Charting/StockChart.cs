using Avalonia.Controls;
using CNergyTrader.Indicator;
using CNergyTrader.Indicator.Indicators;
using ReactiveUI;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;
using System.Collections.ObjectModel;

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
        private ChartType _chartType = ChartType.OHLC;
        private AvaPlot _plot;
        private Crosshair _crossHair;
        private OHLC[] _price;
        private FinancePlot _candlesPlot, _ohlcsPlot;



        #endregion

        #region Constructor
        public StockChart()
        {
            // init
            setupBases();
        }
        #endregion

        public ChartType SelectedChartType
        {
            get => _chartType;
            set
            {
                this.RaiseAndSetIfChanged(ref _chartType, value);
                
                switch (_chartType)
                {
                    case ChartType.Candlestick:
                        _candlesPlot.IsVisible = true;
                        _ohlcsPlot.IsVisible = false;
                        break;
                    case ChartType.OHLC:
                        _candlesPlot.IsVisible = false;
                        _ohlcsPlot.IsVisible = true;
                        break;
                }

                MainPlotArea.Refresh();
            }
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
            _crossHair.LineStyle = LineStyle.Solid;
            _crossHair.LineWidth = 1;
            _crossHair.Color = System.Drawing.Color.Black;

            MainPlotArea.PointerMoved += (o, e) =>
            {
                (double coordinateX, double coordinateY) = MainPlotArea.GetMouseCoordinates();

                _crossHair.X = coordinateX;
                _crossHair.Y = coordinateY;                 

                MainPlotArea.Refresh();
            };

            this.ChartsArea.RowDefinitions.Add(new RowDefinition());

            this.ChartsArea.Children.Add(MainPlotArea);

            GenerateTestDatas();
        }

        public void GenerateTestDatas()
        {
            _price = DataGen.RandomStockPrices(new Random(), 500);

            _candlesPlot = MainPlotArea.Plot.AddCandlesticks(_price);
            _candlesPlot.IsVisible = false;
            _candlesPlot.ColorUp = System.Drawing.Color.Black;

            _ohlcsPlot = MainPlotArea.Plot.AddOHLCs(_price);

            MainPlotArea.Refresh();

            AddDatasTestCommand = ReactiveCommand.Create(() =>
            {
                _price = DataGen.RandomStockPrices(new Random(), 1500);

                _candlesPlot.Clear();
                _ohlcsPlot.Clear();

                _candlesPlot.AddRange(_price);
                _ohlcsPlot.AddRange(_price);               
        

                foreach(var indicator in OnPriceIndicators)
                {
                    var opens = _price.Select(x => x.Open).ToArray();
                    var closes = _price.Select(x => x.Close).ToArray();
                    var highs = _price.Select(x => x.High).ToArray();
                    var lows = _price.Select(x => x.Low).ToArray();

                    indicator.Calculate(_price.Count(), null, opens, highs, lows, closes, null);
                }

                MainPlotArea.Plot.AxisAuto();
                MainPlotArea.Refresh();
            });


            AddIndicator();
            test();
            test2();
        }


        public void AddIndicator()
        {
            var Indicator = new BollingerBands();

            var manager = new OnPriceIndicatorItem(Indicator, MainPlotArea);

            OnPriceIndicators.Add(Indicator);

            Indicator.Calculate(_price.Count(), null, _price.Select(x => x.Open).ToArray(), _price.Select(x => x.High).ToArray(), _price.Select(x => x.Low).ToArray(), _price.Select(x => x.Close).ToArray(), null);

            MainPlotArea.Refresh();
        }

        public IEnumerable<ChartType> ChartTypes
        {
            get => Enum.GetValues(typeof(ChartType)).Cast<ChartType>();
        }

        public IReactiveCommand AddDatasTestCommand { get; private set; }

        public ObservableCollection<IndicatorBase> OnPriceIndicators { get; set; } = new ObservableCollection<IndicatorBase>();

        public void test()
        {
            var external = new AvaPlot();
            external.Configuration.AddLinkedControl(MainPlotArea, vertical: false);

            MainPlotArea.Configuration.AddLinkedControl(external, vertical: false);

            _AddSubChart(external);

            var Indicator = new RSI();

            var manager = new OnPriceIndicatorItem(Indicator, external);

            OnPriceIndicators.Add(Indicator);

            Indicator.Calculate(_price.Count(), null, _price.Select(x => x.Open).ToArray(), _price.Select(x => x.High).ToArray(), _price.Select(x => x.Low).ToArray(), _price.Select(x => x.Close).ToArray(), null);

            MainPlotArea.Plot.XAxis.IsVisible = false;
            MainPlotArea.Refresh();

            external.Plot.AxisAuto();
            external.Refresh();
        }

        public void test2()
        {
            var external = new AvaPlot();
            external.Configuration.AddLinkedControl(MainPlotArea, vertical: false);

            MainPlotArea.Configuration.AddLinkedControl(external, vertical: false);

            _AddSubChart(external);

            var Indicator = new MACD();

            var manager = new OnPriceIndicatorItem(Indicator, external);

            OnPriceIndicators.Add(Indicator);

            Indicator.Calculate(_price.Count(), null, _price.Select(x => x.Open).ToArray(), _price.Select(x => x.High).ToArray(), _price.Select(x => x.Low).ToArray(), _price.Select(x => x.Close).ToArray(), null);

            external.Plot.AxisAuto();
            external.Refresh();
        }

        private void _AddSubChart(UserControl chart)
        {
            ChartsArea.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(5, GridUnitType.Auto) });
            ChartsArea.RowDefinitions.Add(new RowDefinition());

            var splitter = new GridSplitter()
            {
                Height = 5,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
            };

            ChartsArea.Children.Add(splitter);
            Grid.SetRow(splitter, ChartsArea.RowDefinitions.Count - 2);

            ChartsArea.Children.Add(chart);
            Grid.SetRow(chart, ChartsArea.RowDefinitions.Count - 1);
        }

        public Grid ChartsArea { get; private set;} = new Grid();
    }
}