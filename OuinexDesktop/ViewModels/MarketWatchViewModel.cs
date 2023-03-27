using Avalonia.Threading;
using Binance.Net.Clients;
using OuinexDesktop.Views;
using OuinexDesktop.Views.Controls;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OuinexDesktop.ViewModels
{
    public class MarketWatchViewModel : ReactiveObject
    {
        private bool _initialized = false;
        private bool _showLoading = true;
        private TickerViewModel _ticker;
        private OrderBookViewModel _orderBook = new OrderBookViewModel();

        public MarketWatchViewModel()
        {
            this.OpenChartCommand = ReactiveCommand.Create(async () =>
            {
                /*var window = new ContainerWindow()
                {

                    Title = "market watch"
                };

                var control = new ChartControl();
                window.mainContainer.Children.Add(control);
                window.Show();

                await control.CreateCandles(this.SelectedTicker.TickerName);*/
                var chart = new ChartWindow();
                chart.Show(); ;
            });
        }

        public ObservableCollection<TickerViewModel> Tickers { get; set; } = new ObservableCollection<TickerViewModel>();

        public async Task InitStream()
        {
            var socketClient = new BinanceSocketClient();

            try
            {
                //// Spot | Spot market and user subscription methods
                var subscribeResult = await socketClient.SpotStreams.SubscribeToAllTickerUpdatesAsync(data =>
                {
                    // Handle data
                    var list = data.Data;

                    if (!_initialized)
                    {
                        int count = 0;
                        _initialized = true;

                        Dispatcher.UIThread.InvokeAsync(new Action(() =>
                        {
                            foreach (var ticker in list)
                            {
                                if (count >= 20)
                                    return;

                                var newTicker = new TickerViewModel(ticker.Symbol)
                                {
                                    Bid = ticker.BestBidPrice,
                                    Ask = ticker.BestAskPrice,
                                    PercentChange = ticker.PriceChangePercent,
                                    High = ticker.HighPrice,
                                    Low = ticker.LowPrice,
                                    Spread = ticker.BestAskPrice - ticker.BestBidPrice
                                };

                                newTicker.CalculateRange();

                                this.Tickers.Add(newTicker);

                                count++;
                            }
                        }));

                        ShowLoading = false;
                    }
                    else
                    {
                        foreach (var ticker in list)
                        {
                            var toUpdate = Tickers.FirstOrDefault((e) => e.TickerName == ticker.Symbol);

                            if (toUpdate != null)
                            {
                                toUpdate.Bid = ticker.BestBidPrice;
                                toUpdate.Ask = ticker.BestAskPrice;
                                toUpdate.PercentChange = ticker.PriceChangePercent;
                                toUpdate.High = ticker.HighPrice;
                                toUpdate.Low = ticker.LowPrice;
                                toUpdate.Spread = ticker.BestAskPrice - ticker.BestBidPrice;

                                toUpdate.CalculateRange();
                            }
                        }
                    }
                });
               
            }
            catch(Exception ex)
            {

            }
        }

        public bool ShowLoading
        {
            get => _showLoading;
            set => this.RaiseAndSetIfChanged(ref _showLoading, value, nameof(ShowLoading));
        }

        public TickerViewModel SelectedTicker
        {
            get => _ticker;
            set
            {
                this.OrderBook.Init(value);
                this.RaiseAndSetIfChanged(ref _ticker, value, nameof(SelectedTicker));
            }
        }

        public OrderBookViewModel OrderBook
        {
            get => _orderBook;
            set => this.RaiseAndSetIfChanged(ref _orderBook, value, nameof(OrderBook));
        }



        public ICommand OpenChartCommand { get; }
    }
}