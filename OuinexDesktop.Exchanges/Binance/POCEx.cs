using Binance.Net.Clients;
using OuinexDesktop.Models;
using System.Data;
using System.Globalization;
using System.Text.Json;

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

    public class BPOCSpot : Spot
    {
        private BinanceSocketClient socketClient = new BinanceSocketClient();

        public BPOCSpot()
        {
            var orders = new List<SpotOrder>();
            orders.Add(new SpotOrder
            {
                Amount = (decimal)1.5,
                FilledAmount = (decimal)0.034,
                OrderSymbol = "BTC/USDT",
                OrderType = OrderType.Pending,
                Price = (decimal)31245.89,
                Status = OrderStatus.PartiallyFilled,
                OrderSide = OrderSide.BUY
            });

            Orders = orders;
        }

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
    }

    public class POCEx : Exchange
    {
        public override Spot Spot { get; } = new BPOCSpot();

        public override async Task InitAsync()
        {
            var list = await exceute();

           /* var client = new BinanceClient();

            var request = await client.SpotApi.ExchangeData.GetExchangeInfoAsync();

            if (request.Success)
            {
                var datas = request.Data.Symbols.Where(x => x.Status == Binance.Net.Enums.SymbolStatus.Trading);

                Symbols = datas.OrderBy(x => x.BaseAsset).Select(x => new POCSymbol()
                {
                    BaseCurrency = x.BaseAsset,
                    QuoteCurrency = x.QuoteAsset,
                    Name = x.Name,
                    TickSize = x.PriceFilter.TickSize
                });

                Console.WriteLine(Symbols.ToString());*/
           if(list != null)
            {
                this.Spot.Symbols = list.OrderBy(x => x.BaseCurrency);

                IsInitialized = true;
            }
            else
            {
                //todo : handle error
            }
        }

        async Task<List<Symbol>> exceute()
        {
            string url = "https://api.binance.com/api/v3/exchangeInfo";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = response.Content.ReadAsStringAsync().Result;

                        var symbolList = new List<Symbol>();

                        using (JsonDocument doc = JsonDocument.Parse(responseString))
                        {
                            JsonElement root = doc.RootElement;
                            JsonElement symbols = root.GetProperty("symbols");

                            foreach (JsonElement element in symbols.EnumerateArray())
                            {
                                Symbol symbol = new Symbol();

                                symbol.Name = element.GetProperty("symbol").GetString();
                                symbol.BaseCurrency = element.GetProperty("baseAsset").GetString();
                                symbol.QuoteCurrency = element.GetProperty("quoteAsset").GetString();
                                //  symbol.Status = element.GetProperty("status").GetString();
                                //symbol.MinPrice = decimal.Parse( element.GetProperty("filters")[0].GetProperty("minPrice")!.GetString(), CultureInfo.InvariantCulture);
                                //symbol.MaxPrice = decimal.Parse(element.GetProperty("filters")[0].GetProperty("maxPrice")!.GetString(), CultureInfo.InvariantCulture);
                                symbol.TickSize = decimal.Parse(element.GetProperty("filters")[0].GetProperty("tickSize")!.GetString(), CultureInfo.InvariantCulture);
                                //symbol.MinQty = decimal.Parse(element.GetProperty("filters")[1].GetProperty("minQty")!.GetString(), CultureInfo.InvariantCulture);
                               // symbol.MaxQty = decimal.Parse(element.GetProperty("filters")[1].GetProperty("maxQty")!.GetString(), CultureInfo.InvariantCulture);
                               // symbol.StepSize = decimal.Parse(element.GetProperty("filters")[1].GetProperty("stepSize")!.GetString(), CultureInfo.InvariantCulture);
                               // symbol.MinNotional = decimal.Parse(element.GetProperty("filters")[2].GetProperty("minNotional")!.GetString(), CultureInfo.InvariantCulture);
                               // symbol.IsSpotTradingAllowed = element.GetProperty("isSpotTradingAllowed").GetBoolean();
                               // symbol.IsMarginTradingAllowed = element.GetProperty("isMarginTradingAllowed").GetBoolean();

                                symbolList.Add(symbol);
                            }
                        }

                        return symbolList;
                    }
                    else
                    {
                        Console.WriteLine("Error getting response from API.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                    return null;
                }
            }
        }
    }
}