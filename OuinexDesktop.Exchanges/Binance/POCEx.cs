using Binance.Net.Clients;
using OuinexDesktop.Models;

namespace OuinexDesktop.Exchanges
{
    public class POCSymbol : Symbol
    {
    }

    public class POCTicker : Ticker
    {
        internal void Update(decimal bid, decimal ask, decimal high, decimal low, decimal change)
        {
            BidPrice = bid;
            AskPrice = ask;
            High = high;
            Low = low;
            Change = change;

            RaiseTick();
        }
    }

    public class POCEx : Exchange
    {
        private BinanceSocketClient socketClient = new BinanceSocketClient();
        public override async Task<Ticker> GetTickerAsync(Symbol symbol)
        {
            var result = new POCTicker();

            var client = new BinanceClient();

            var t = await client.SpotApi.ExchangeData.GetTickerAsync(symbol.Name);

            result.Update(t.Data.BestBidPrice, t.Data.BestAskPrice, t.Data.HighPrice, t.Data.LowPrice, t.Data.PriceChangePercent);

            Thread.Sleep(100);

            var subscribeResult = await socketClient.SpotStreams.SubscribeToTickerUpdatesAsync(symbol.Name, (t) =>
            {
                result.Update(t.Data.BestBidPrice, t.Data.BestAskPrice, t.Data.HighPrice, t.Data.LowPrice, t.Data.PriceChangePercent);
            });            

            return result;
        }

        public override async Task InitAsync()
        {
            var client = new BinanceClient();
            var request = await client.SpotApi.ExchangeData.GetProductsAsync();

            if (request.Success)
            {
                Symbols = request.Data.Select(x => new POCSymbol()
                {
                    BaseCurrency = x.BaseAsset,
                    QuoteCurrency = x.QuoteAsset,
                    Name = x.Symbol
                });

                Console.WriteLine(Symbols.ToString());

                IsInitialized = true;
            }
            else
            {
                //todo : handle error
            }
        }
    }
}