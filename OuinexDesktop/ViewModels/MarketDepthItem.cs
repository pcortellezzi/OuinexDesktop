using ReactiveUI;

namespace OuinexDesktop.ViewModels
{
    public class MarketDepthItem : ViewModelBase
    {
        private string _price, _bid, _ask;
        private int _percentAsks, _percentBid;

        public string Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value, nameof(Price));
        }

        public string Bid
        {
            get => _bid;
            set => this.RaiseAndSetIfChanged(ref _bid, value, nameof(Bid));
        }

        public string Ask
        {
            get => _ask;
            set => this.RaiseAndSetIfChanged(ref _ask, value, nameof(Ask));
        }

        public int PercentAsk
        {
            get => _percentAsks * 2;
            set => this.RaiseAndSetIfChanged(ref _percentAsks, value, nameof(PercentAsk));
        }

        public int PercentBid
        {
            get => _percentBid * 2;
            set => this.RaiseAndSetIfChanged(ref _percentBid, value, nameof(PercentBid));
        }
    }
}