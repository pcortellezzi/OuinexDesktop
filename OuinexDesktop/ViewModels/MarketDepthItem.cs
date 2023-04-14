using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;

namespace OuinexDesktop.ViewModels
{
    public class MarketDepthItem : ViewModelBase
    {
        private double _price, _bid = double.NaN, _ask = double.NaN;
        private decimal _percentAsks, _percentBid;
        private bool _displayAsk = false, _displayBid = false;

        public double Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value, nameof(Price));
        }

        public double Bid
        {
            get => _bid;
            set
            {
                this.RaiseAndSetIfChanged(ref _bid, value, nameof(Bid));
                DisplayBid = !double.IsNaN(value);
            }
        }

        public double Ask
        {
            get => _ask;
            set
            {
                this.RaiseAndSetIfChanged(ref _ask, value, nameof(Ask));
                DisplayAsk = !double.IsNaN(value);
            }
        }

        public decimal PercentAsk
        {
            get => _percentAsks;
            set => this.RaiseAndSetIfChanged(ref _percentAsks, value, nameof(PercentAsk));
        }

        public decimal PercentBid
        {
            get => _percentBid;
            set => this.RaiseAndSetIfChanged(ref _percentBid, value, nameof(PercentBid));
        }

        public bool DisplayAsk
        {
            get => _displayAsk;
            set => this.RaiseAndSetIfChanged(ref _displayAsk, value, nameof(DisplayAsk));
        }

        public bool DisplayBid
        {
            get => _displayBid;
            set => this.RaiseAndSetIfChanged(ref _displayBid, value, nameof(DisplayBid));
        }
    }
}