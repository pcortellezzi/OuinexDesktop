namespace OuinexDesktop.Models
{
    public partial class Symbol
    {
        public string BaseCurrency { get; protected set; } = string.Empty;

        public string QuoteCurrency { get; protected set; } = string.Empty;

        public string FullName => BaseCurrency + "/" + QuoteCurrency;

        public string ExchangeDenomination { get; protected set; } = string.Empty;
    }

    public partial class Ticker
    {
        public Symbol Symbol { get; protected set; }

        public Ticker() { }
        public Ticker(Symbol symbol)
        {
            Symbol = symbol;
        }
    }
}
