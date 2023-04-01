using Binance.Net.Clients;
using CryptoExchange.Net.Interfaces;
using OuinexDesktop.Models;

namespace OuinexDesktop.Exchanges
{
    public class POCSymbol : Symbol
    {
    }

    public class POCTicker : Ticker
    {

    }

    public class POCEx : Exchange
    {
        private BinanceSocketClient socketClient = new BinanceSocketClient();
        public override async Task<Ticker> GetTickerAsync(Symbol symbol)
        {
            var result = new Ticker();

            var subscribeResult = await socketClient.SpotStreams.SubscribeToTickerUpdatesAsync(symbol.ExchangeDenomination, (t) => 
            {
                
            });

            return result;
        }

        public override async Task InitAsync()
        {
            var client = new Binance.Net.Clients.BinanceClient();
            var request = await client.SpotApi.ExchangeData.GetProductsAsync();

            if (request.Success)
            {
                this.Symbols = request.Data.Select(x => new POCSymbol()
                {
                    BaseCurrency = x.BaseAsset,
                    QuoteCurrency = x.QuoteAsset,
                    ExchangeDenomination = "POC-Binance"
                });

                Console.WriteLine(Symbols.ToString());
            }
            else
            {
                //todo : handle error
            }
        }
    }
}