using Binance.Net.Clients;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;

namespace OuinexPro.ViewModels
{
    public class OrderBookViewModel : ViewModelBase
    {
        private string _ticker = string.Empty;

        private BinanceSocketClient socket = new BinanceSocketClient();
        public async void Init(TickerViewModel ticker)
        {
            if (ticker == null)
                return;

            await socket.UnsubscribeAllAsync();
            Asks.Clear();
            Bids.Clear();

            for(int i =0; i < 10; i++)
            {
                Asks.Add(new OrderBookItem());
                Bids.Add(new OrderBookItem());
            }

            await socket.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(ticker.TickerName, 20, 100, (data)=>
            {
                // creation des listes bid & ask 
                var asks = data.Data.Asks.ToList();
                asks.Reverse();

                var bids = data.Data.Bids.ToList();

                // total des volumes à trader pour les deux listes

                var totalAsks = asks.Sum(x => x.Quantity);
                var totalBids = bids.Sum(x => x.Quantity);

                // population des liste visible sur l'ui
                for (int i = 0; i < 10; i++)
                {
                    Asks[i].Price = asks[i].Price;
                    Asks[i].Volume = asks[i].Quantity;
                    Asks[i].Percent = (int)((100/totalAsks) * asks[i].Quantity);

                    Bids[i].Price = bids[i].Price;
                    Bids[i].Volume = bids[i].Quantity;
                    Bids[i].Percent = (int)((100 / totalBids) * bids[i].Quantity);
                }
            });
        }

        public ObservableCollection<OrderBookItem> Asks { get; private set; } = new ObservableCollection<OrderBookItem>();

        public ObservableCollection<OrderBookItem> Bids { get; private set; } = new ObservableCollection<OrderBookItem>();

        public string TickerName
        {
            get => _ticker;
            set => this.RaiseAndSetIfChanged(ref _ticker, value, nameof(TickerName));
        }
    }
}