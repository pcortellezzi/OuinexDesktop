using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using Microsoft.VisualBasic;
using OuinexDesktop.Models;
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
        private Symbol _selectedSymbol;

        public WindowNotificationManager? _manager;
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
            Symbols = new ObservableCollection<Symbol>(ExchangesConnector.Instances.First().Value.Spot.Symbols);
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

        public ObservableCollection<Symbol> Symbols { get; set; } = new ObservableCollection<Symbol>();

        public ICommand OpenChartCommand { get; }

        public Symbol SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSymbol, value, nameof(SelectedSymbol));

                if(value != null)
                {
                    Task.Run(async () =>
                    {
                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            var newTicker = new TickerViewModel(_selectedSymbol, ExchangesConnector.Instances.First().Value);
                            this.Tickers.Add(newTicker);

                            _manager?.Show(new Notification("Information", string.Format("{0} added to the list", _selectedSymbol.FullName), NotificationType.Information));
                        });
                    });
                }
            }
        }
    }
}