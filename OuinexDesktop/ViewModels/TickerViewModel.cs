using OuinexDesktop.Models;
using ReactiveUI;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OuinexDesktop.ViewModels
{
    public class TickerViewModel : ReactiveObject
    {
        private decimal _bid, _ask, _spread, _high, _low, _volume, _percentChange;
        private TickState _askColor, _bidColor, _changeColor = TickState.NEUTRAL;
        private decimal _percentRange;
        private Exchange _exchange;
        private Symbol _symbol;
        public TickerViewModel(Symbol symbol, Exchange exchange)
        {
            _symbol = symbol; 
            _exchange = exchange;

            var ticker = Task.Run(async () =>
            {
                var ticker = await _exchange.GetTickerAsync(symbol);

                Update(ticker);

                ticker.OnTick += (t) =>
                {
                    Update(t);
                };
            });
        }

        private void Update(Ticker t)
        {
            Ask = t.AskPrice;
            Bid = t.BidPrice;
            High = t.High;
            Low = t.Low;
            PercentChange = t.Change;
            Spread = Ask - Bid;

            CalculateRange();
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

        public string TickerName
        {
            get => _symbol?.FullName ?? string.Empty;
        }

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

        public Symbol Symbol
        {
            get => _symbol;
        }
    }
}