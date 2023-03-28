using OuinexPro.Views.Controls;
using ReactiveUI;
using System.Windows.Input;

namespace OuinexPro.ViewModels
{
    public class TickerViewModel : ReactiveObject
    {
        private decimal _bid, _ask, _spread, _high, _low, _volume, _percentChange;
        private TickState _askColor, _bidColor, _changeColor = TickState.NEUTRAL;
        private decimal _percentRange;

        public TickerViewModel(string tickerName)
        {
            this.TickerName = tickerName;

            
        }
        public decimal Bid
        {
            get => _bid;
            set
            {
                BidColor = value > _bid ? TickState.UP : value < _bid ? TickState.DOWN : TickState.NEUTRAL;
                this.RaiseAndSetIfChanged(ref _bid, value, nameof(Bid));
            }
        }

        public decimal Ask
        {
            get => _ask;
            set
            {
                AskColor = value > _ask ? TickState.UP : value < _ask ? TickState.DOWN : TickState.NEUTRAL;
                this.RaiseAndSetIfChanged(ref _ask, value, nameof(Ask));
            }
        }

        public decimal PercentChange
        {
            get => _percentChange;
            set
            {
                ChangeColor = value >= 0 ? TickState.UP : TickState.DOWN;
                this.RaiseAndSetIfChanged(ref _percentChange, value, nameof(PercentChange));
            }
        }
        public decimal High
        {
            get => _high;
            set => this.RaiseAndSetIfChanged(ref _high, value, nameof(High));
        }
        public decimal Low
        {
            get => _low;
            set => this.RaiseAndSetIfChanged(ref _low, value, nameof(Low));
        }

        public decimal Spread
        {
            get => _spread;
            set => this.RaiseAndSetIfChanged(ref _spread, value, nameof(Spread));
        }

        public string TickerName { get; }

        public TickState AskColor
        {
            get => _askColor;
            set => this.RaiseAndSetIfChanged(ref _askColor, value, nameof(AskColor));
        }

        public TickState BidColor
        {
            get => _bidColor;
            set => this.RaiseAndSetIfChanged(ref _bidColor, value, nameof(BidColor));
        }

        public TickState ChangeColor
        {
            get => _changeColor;
            set => this.RaiseAndSetIfChanged(ref _changeColor, value, nameof(ChangeColor));
        }

        public decimal PercentRange
        {
            get => _percentRange;
            set => this.RaiseAndSetIfChanged(ref _percentRange, value, nameof(PercentRange));
        }

        internal void CalculateRange()
        {
            PercentRange =  ((Ask - High) / (Low-High)) * 100;
        }

        public ICommand OpenChartCommand { get; }
    }
}