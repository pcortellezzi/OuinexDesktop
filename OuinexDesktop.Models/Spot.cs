namespace OuinexDesktop.Models
{
    public abstract class Spot
    {
        public IEnumerable<Symbol> Symbols { get; set; } = new List<Symbol>();

        public IEnumerable<SpotOrder> Orders { get; set; } = new List<SpotOrder>();

        public abstract Task<Ticker> GetTickerAsync(Symbol symbol);
    }
}
