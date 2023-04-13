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
}
