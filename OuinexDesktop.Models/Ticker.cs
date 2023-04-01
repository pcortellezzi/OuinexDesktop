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
}
