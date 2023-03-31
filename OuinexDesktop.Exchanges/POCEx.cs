using OuinexDesktop.Models;

namespace OuinexDesktop.Exchanges
{
    public class POCSymbol : Symbol
    {
    } 
    public class POCEx : Exchange
    {
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