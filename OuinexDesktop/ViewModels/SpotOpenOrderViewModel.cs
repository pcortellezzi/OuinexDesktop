using Avalonia.Threading;
using ReactiveUI;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using ScottPlot;
using ScottPlot.Plottables;

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
                var price = _plot != null ? (_plot.Axes.YAxis.Max + _plot.Axes.YAxis.Min) / 2 : 0;

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

            //TODO: Chart.Plot.YAxis.Layout(padding: 0);
            //TODO: Chart.Plot.XAxis.DateTimeFormat(true);

            crosshair = Chart.Plot.Add.Crosshair(0, 0);
            crosshair.EnableAutoscale = false;
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

        public ICommand AddExitLevelCommand { get; }

        private CandlestickPlot _plot;
        private async Task Populate()
        {
            await Dispatcher.UIThread.InvokeAsync(new Action(async () =>
            {
                /*TODO: if (_plot != null)
                    _plot.Clear();*/

                var client = new Binance.Net.Clients.BinanceRestClient();

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
                            new TimeSpan(0, 60, 0)));
                    }

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                       _plot = Chart.Plot.Add.Candlestick(prices.ToArray());
                        Chart.Plot.Axes.AutoScale();
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
        private HorizontalLine _line;

        public SpotOrderExitLevel(bool isStopLoss, AvaPlot plot, double price)
        {
            _isSl = isStopLoss;
            _plot = plot;
            
            _line = _plot.Plot.Add.HorizontalLine(price, 2, _isSl ? Colors.OrangeRed : Colors.Green);
            _line.Text = "Stop Loss";
            _line.IsDraggable = true;
            _line.IsVisible = true;
            //_line.PositionLabel = true;

            //_line.Dragged += (s, e) =>
            PriceString = _line.Y.ToString();
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

        public ICommand RemoveLevelCommand { get; set; }

        public HorizontalLine Hline
        {
            get => _line;
        }
    }
}
