using Avalonia.Threading;
using Binance.Net.Clients;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OuinexDesktop.ViewModels
{
    public class MarketDepthViewModel : ViewModelBase
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
                TotalBids = 0;
                _bids = 0;
                _total = 0;
                CumuledTotal = 0;
                _cumuledBids  = 0;
                _cumuledBids1 = 0;

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

                await socket.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(ticker.Symbol.Name, levels, 100, (data) =>
                {
                    // creation des listes bid & ask 
                    var asks = data.Data.Asks.ToList();
                    asks.Reverse();

                    var bids = data.Data.Bids.ToList();

                    // total des volumes à trader pour les deux listes

                    var totalAsks = asks.Sum(x => x.Quantity);
                    var totalBids = bids.Sum(x => x.Quantity);
                    var total = totalAsks+ totalBids; ;

                   
                    // population des liste visible sur l'ui
                    for (int i = 0; i < levels; i++)
                    {
                        Asks[i].Price = asks[i].Price;
                        Asks[i].Volume = asks[i].Quantity;
                        Asks[i].Percent = (int)((100 / total) * asks[i].Quantity);

                        Bids[i].Price = bids[i].Price;
                        Bids[i].Volume = bids[i].Quantity;
                        Bids[i].Percent = (int)((100 / total) * bids[i].Quantity);
                    }

                    // ici c'est la barre horizontale des volumes aux ticks
                    _bids = totalBids;
                    _total = total;
                    TotalBids = (int)((100 / _total) * _bids);

                    // ici c'est la barre horizontale des volumes totauw
                    _cumuledBids+= totalBids;
                    _cumuledBids1+= total;
                    CumuledTotal = (int)((100 / _cumuledBids1) * _cumuledBids);

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

        private int _totalBids = 0;
        private decimal _bids, _total, _cumuledBids, _cumuledBids1, _cumuledTotal = 0;

        public int TotalBids
        {
            get => _totalBids;
            set => this.RaiseAndSetIfChanged(ref _totalBids, value, nameof(TotalBids));
        }

        public int CumuledTotal
        {
            get => _totalBids;
            set => this.RaiseAndSetIfChanged(ref _cumuledTotal, value, nameof(CumuledTotal));
        }
    }
}