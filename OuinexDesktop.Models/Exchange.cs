namespace OuinexDesktop.Models
{
    public abstract class Exchange
    {
        public event ExchangeInitializedHandler OnInit;

        private bool _initialized = false;
        public IEnumerable<Symbol> Symbols { get; protected set; } = new List<Symbol>();

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

        public abstract Task<Ticker> GetTickerAsync(Symbol symbol);

        public abstract Task InitAsync();
    }
}
