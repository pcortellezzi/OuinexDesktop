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
        int[] _levels = { 5, 10, 20 };
        int _selectedLevel = 1;
        private string _ticker = string.Empty;
        private TickerViewModel _tickerViewModel;

        private BinanceSocketClient socket = new BinanceSocketClient();

        public async Task Init(TickerViewModel ticker)
        {
            _tickerViewModel = ticker;

            await Dispatcher.UIThread.InvokeAsync(new Action(async () =>
            {
                IsEmptyOfData = false;
                IsBusy = true;                

                await socket.UnsubscribeAllAsync();
                Levels.Clear();
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

                for (int i = 0; i < _levels[_selectedLevel] *2; i++)
                {
                    Levels.Add(new MarketDepthItem());
                }

                await socket.SpotStreams.SubscribeToPartialOrderBookUpdatesAsync(ticker.Symbol.Name, _levels[_selectedLevel], 100, (data) =>
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
                    for (int i = 0; i < _levels[_selectedLevel]; i++)
                    {
                        Levels[i].Price = (double)asks[i].Price;
                        Levels[i].Ask = (double)asks[i].Quantity;
                        Levels[i].PercentAsk = (asks[i].Quantity /totalAsks ) * 100;
                        Levels[i].Bid = double.NaN;
                        Levels[i].IsBestAsk = i == _levels[_selectedLevel] - 1;

                        Levels[i + _levels[_selectedLevel]].Price = (double)bids[i].Price;
                        Levels[i + _levels[_selectedLevel]].Bid = (double)bids[i].Quantity;
                        Levels[i + _levels[_selectedLevel]].PercentBid = (bids[i].Quantity/totalBids) * 100;
                        Levels[i + _levels[_selectedLevel]].Ask = double.NaN;
                        Levels[i + _levels[_selectedLevel]].IsBestBid = i + _levels[_selectedLevel] == _levels[_selectedLevel];
                    }

                    // ici c'est la barre horizontale des volumes aux ticks
                    _bids = totalBids;
                    _total = total;
                    TotalBids = (int)((_bids / _total) * 100);

                    // ici c'est la barre horizontale des volumes totauw
                    _cumuledBids+= totalBids;
                    _cumuledBids1+= total;
                    CumuledTotal = (int)((100 / _cumuledBids1) * _cumuledBids);

                    IsBusy = false;
                });                
            }));           
        }

        public ObservableCollection<MarketDepthItem> Levels { get; private set; } = new ObservableCollection<MarketDepthItem>();

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

        public int SelectedLevel
        {
            get => _selectedLevel;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedLevel, value, nameof(SelectedLevel));

                Task.Run(async () => await Init(_tickerViewModel));
            }
        }
    }
}