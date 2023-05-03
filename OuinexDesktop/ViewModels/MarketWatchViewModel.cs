using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using OuinexDesktop.Models;
using OuinexDesktop.Views;
using OuinexDesktop.Views.Controls.Charting;
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
        private MarketDepthViewModel _orderBook = new MarketDepthViewModel();

        private CoinGeckoAPI _cmcAPI = new CoinGeckoAPI();
        private Symbol _selectedSymbol;

        public WindowNotificationManager? _manager;
        public MarketWatchViewModel()
        {
            this.OpenChartCommand = ReactiveCommand.Create(async () =>
            {
                await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    var context = new ChartViewModel();

                    var wnd = new ContainerWindow();
                    var chartControl = new ChartControl();
                    wnd.mainContainer.Children.Add(chartControl);
                    chartControl.DataContext = context;

                    wnd.Show();

                    await context.Populate(this.SelectedTicker.Symbol.Name);
                });
         
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

                if(value != null)
                {
                    Task.Run(async () => await MarketDepth.Init(value));
                    Task.Run(async () => await OrderBook.Init(value));
                    Task.Run(async()=> await Dispatcher.UIThread.InvokeAsync(async()=> await ChartMVVM.Populate(value.Symbol.Name)));
                }
            }
        }

        public OrderBookViewModel OrderBook { get; } = new OrderBookViewModel();

        public MarketDepthViewModel MarketDepth
        {
            get => _orderBook;
            set => this.RaiseAndSetIfChanged(ref _orderBook, value, nameof(MarketDepth));
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

        public ChartViewModel ChartMVVM { get; } = new ChartViewModel();
    }
}