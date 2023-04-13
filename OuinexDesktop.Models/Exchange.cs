namespace OuinexDesktop.Models
{
    public abstract class Exchange
    {
        public event ExchangeInitializedHandler OnInit;

        private bool _initialized = false;
       

        public bool IsInitialized
        {
            get => _initialized;
            protected set
            {
                if (_initialized == value)
                    return;

                _initialized = value;

                if (_initialized)
                    OnInit?.Invoke();
            }
        }       

        public abstract Task InitAsync();

        public abstract Spot Spot { get; }
    }

    public abstract class Spot
    {
        public IEnumerable<Symbol> Symbols { get; set; } = new List<Symbol>();

        public abstract Task<Ticker> GetTickerAsync(Symbol symbol);
    }
}
