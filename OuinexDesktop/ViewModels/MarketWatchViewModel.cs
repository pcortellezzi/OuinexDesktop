using Avalonia.Threading;
using OuinexDesktop.Views;
using ReactiveUI;
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
        private CoinGeckoAPI _cmcAPI = new CoinGeckoAPI();
        public MarketWatchViewModel()
        {
            this.OpenChartCommand = ReactiveCommand.Create(async () =>
            {
                var context = new ChartViewModel();

                var chart = new ChartWindow()
                {
                    DataContext = context,
                };

                chart.Show();

               await context.Populate(this.SelectedTicker.Symbol.Name);
            });
        }

        public ObservableCollection<TickerViewModel> Tickers { get; set; } = new ObservableCollection<TickerViewModel>();

        public async Task InitStream()
        {
            var symbols = ExchangesConnector.Instances.First().Value.Symbols.Take(20);

            foreach (var symbol in symbols)
            {
                var newTicker = new TickerViewModel(symbol, ExchangesConnector.Instances.First().Value);

                await Dispatcher.UIThread.InvokeAsync(() => { this.Tickers.Add(newTicker); });
            }

            ShowLoading = false;
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