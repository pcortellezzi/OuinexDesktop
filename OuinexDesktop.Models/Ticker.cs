namespace OuinexDesktop.Models
{
    public partial class Ticker : BaseRequest
    {
        public Symbol Symbol { get; protected set; }

        public event OnTickHandler OnTick;
        public Ticker() { }
        public Ticker(Symbol symbol)
        {
            Symbol = symbol;
        }
        
        public decimal BidPrice { get; protected set; }

        public decimal AskPrice { get; protected set; }

        public decimal High { get; protected set; }

        public decimal Low { get; protected set; }

        public decimal Change { get; protected set; }

        protected void RaiseTick()
        {
            this.OnTick?.Invoke(this);
        }
    }

    public partial class OHLC
    {
        public OHLC(decimal open, decimal high, decimal low, decimal close, decimal volumes, DateTime time)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volumes = volumes;
            Time = time;
        }

        public decimal Open { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Close { get; }
        public decimal Volumes { get; }
        public DateTime Time { get; }
    }
}
