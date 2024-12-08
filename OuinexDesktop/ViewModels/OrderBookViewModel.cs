using Avalonia.Threading;
using Binance.Net.Clients;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OuinexDesktop.ViewModels
{
    public class OrderBookViewModel : ViewModelBase
    {
        int levels = 10;
        private string _ticker = string.Empty;

        private BinanceSocketClient socket = new BinanceSocketClient();

        public async Task Init(TickerViewModel ticker)
        {
            await Dispatcher.UIThread.InvokeAsync(new Action(async () =>
            {
                IsEmptyOfData = false;
                IsBusy = true;

                await socket.UnsubscribeAllAsync();
                Asks.Clear();
                Bids.Clear();

                if (ticker == null)
                {
                    this.IsEmptyOfData = true;
                    return;
                }

                for (int i = 0; i < levels; i++)
                {
                    Asks.Add(new MarketDepthItem());
                    Bids.Add(new MarketDepthItem());
                }

                await socket.SpotApi.ExchangeData.SubscribeToBookTickerUpdatesAsync(ticker.Symbol.Name, (data) =>
                {
                  
                   
                    IsBusy = false;
                });
            }));
        }

        public ObservableCollection<MarketDepthItem> Asks { get; private set; } = new ObservableCollection<MarketDepthItem>();

        public ObservableCollection<MarketDepthItem> Bids { get; private set; } = new ObservableCollection<MarketDepthItem>();

        public string TickerName
        {
            get => _ticker;
            set => this.RaiseAndSetIfChanged(ref _ticker, value, nameof(TickerName));
        }
    }
}