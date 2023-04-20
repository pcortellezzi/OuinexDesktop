using Avalonia.Threading;
using ReactiveUI;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OuinexDesktop.ViewModels
{
    public class SpotOpenOrderViewModel : ViewModelBase
    {
        private bool _isBuy, _isMarketOrder, _canBuy, _canSell;
        private Models.Symbol _selectedSymbol;
        private Crosshair crosshair;
        private double crossY;

        public SpotOpenOrderViewModel()
        {
            setupTheChart();

            AddExitLevelCommand = ReactiveCommand.Create(() =>
            {
                var price = _plot != null ? (_plot.GetAxisLimits().YMax + _plot.GetAxisLimits().YMin) / 2 : 0;

                var level = new SpotOrderExitLevel(false, Chart, price);
                ExitLevels.Add(level);

                level.RemoveLevelCommand = ReactiveCommand.Create(() =>
                {
                    ExitLevels.Remove(level);
                    Chart.Plot.Remove(level.Hline);

                    Chart.Refresh();
                });
            });
        }

        public ObservableCollection<SpotOrderExitLevel> ExitLevels { get; set; } = new ObservableCollection<SpotOrderExitLevel>();

        public IEnumerable<Models.Symbol> Symbols { get; } = ExchangesConnector.Instances.Values.First().Spot.Symbols;

        public Models.Symbol SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSymbol, value, nameof(SelectedSymbol));

                if(value != null)
                {
                    Task.Run(async () => await Populate());
                }
            }
        }

        public AvaPlot Chart
        {
            get;private set;
        }

        public bool IsBuy
        {
            get => _isBuy;
            set
            {
                this.RaiseAndSetIfChanged(ref _isBuy, value, nameof(IsBuy));
            }
        }

        private void setupTheChart()
        {
            Chart = new AvaPlot();

            Chart.Plot.YAxis.Layout(padding: 0);

            crosshair = Chart.Plot.AddCrosshair(0, 0);
            crosshair.IgnoreAxisAuto = true;

            Chart.PointerMoved += (o, e) =>
            {
                (double coordinateX, double coordinateY) = Chart.GetMouseCoordinates();

                crosshair.X = coordinateX;
                crosshair.Y = coordinateY;

                crossY = coordinateY;

                Chart.Refresh();
            };

            Chart.Refresh();
        }

        public IReactiveCommand AddExitLevelCommand { get; }

        private FinancePlot _plot;
        private async Task Populate()
        {
            await Dispatcher.UIThread.InvokeAsync(new Action(async () =>
            {
                if (_plot != null)
                    _plot.Clear();

                var client = new Binance.Net.Clients.BinanceClient();

                var request = await client.SpotApi.ExchangeData.GetUiKlinesAsync(this.SelectedSymbol.Name, Binance.Net.Enums.KlineInterval.OneHour, limit: 100);

                if (request.Success)
                {
                    List<ScottPlot.OHLC> prices = new List<ScottPlot.OHLC>();

                    foreach (var data in request.Data)
                    {
                        prices.Add(new ScottPlot.OHLC((double)data.OpenPrice,
                            (double)data.HighPrice,
                            (double)data.LowPrice,
                            (double)data.ClosePrice,
                            data.OpenTime,
                            //TODO : coonvert time frime to timespan
                            new TimeSpan(1, 0, 0)));
                    }

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                       _plot = Chart.Plot.AddCandlesticks(prices.ToArray());
                        Chart.Plot.AxisAuto();
                        Chart.Refresh();
                        
                    }, DispatcherPriority.Background);
                }
            }));
        }
    }

    public class SpotOrderExitLevel : ViewModelBase
    {
        private double _priceDouble;
        private bool _isSl = true;
        private string _priceString, _sizeString;
        private AvaPlot _plot;
        private HLine _line;

        public SpotOrderExitLevel(bool isStopLoss, AvaPlot plot, double price)
        {
            _isSl = isStopLoss;
            _plot = plot;

            _line = _plot.Plot.AddHorizontalLine(price, _isSl ? System.Drawing.Color.OrangeRed : System.Drawing.Color.Green, 2, label: "Stop Loss");

            _line.DragEnabled = true;
            _line.IsVisible = true;
            _line.PositionLabel = true;

            _line.Dragged += (s, e) => PriceString = _line.Y.ToString();
            _priceString = price.ToString();

            plot.Refresh();
        }

        public string PriceString
        {
            get => _priceString;
            set
            {
                this.RaiseAndSetIfChanged(ref _priceString, value, nameof(PriceString));

                if(double.TryParse(value, out _priceDouble))
                {
                    _line.Y = _priceDouble;
                    _plot.Refresh();
                }
            }
        }

        public string SizeString
        {
            get => _sizeString;
            set
            {
                this.RaiseAndSetIfChanged(ref _sizeString, value, nameof(SizeString));
            }
        }

        public IReactiveCommand RemoveLevelCommand { get; set; }

        public HLine Hline
        {
            get => _line;
        }
    }
}
