using System.Security.Cryptography.X509Certificates;

namespace OuinexDesktop.Models
{
    public partial class Symbol
    {
        public string BaseCurrency { get;  set; } = string.Empty;

        public string QuoteCurrency { get;  set; } = string.Empty;

        public string FullName => BaseCurrency + "/" + QuoteCurrency;

        public string ExchangeDenomination { get;  set; } = string.Empty;
    }

    public partial class Ticker : BaseRequest
    {
        public Symbol Symbol { get; protected set; }

        public Ticker() { }
        public Ticker(Symbol symbol)
        {
            Symbol = symbol;
        }
    }

    public abstract class Exchange
    {
        public ExchangeInitializedHandler OnInit;

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

    public class BaseRequest
    {
        public bool Success { get; protected set; }
    }
}
