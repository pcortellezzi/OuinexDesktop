using Newtonsoft.Json;

namespace OuinexDesktop.Services
{
    public class CoinMarketCapAPI
    {
        private const string ApiKey = "YOUR_API_KEY_HERE"; // replace with your actual API key
        private const string BaseUrl = "https://pro-api.coinmarketcap.com/v1/cryptocurrency";
        private readonly HttpClient _client;

        public CoinMarketCapAPI()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", ApiKey);
        }

        public async Task<Coin> GetCoin(string symbol)
        {
            var url = $"{BaseUrl}/info?symbol={symbol}";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve coin information: {response.ReasonPhrase}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponse>(content);

            return result.Data[symbol].ConvertToCoin();
        }
    }

    public class ApiResponse
    {
        [JsonProperty("data")]
        public Dictionary<string, CoinInfo> Data { get; set; }
    }

    public class CoinInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("logo")]
        public Uri LogoUrl { get; set; }

        [JsonProperty("quote")]
        public Quote Quote { get; set; }

        public Coin ConvertToCoin()
        {
            return new Coin
            {
                Id = Id,
                Name = Name,
                Symbol = Symbol,
                Price = Quote.USD.Price,
                MarketCap = Quote.USD.MarketCap,
                Volume24h = Quote.USD.Volume24h,
                PercentChange1h = Quote.USD.PercentChange1h,
                PercentChange24h = Quote.USD.PercentChange24h,
                PercentChange7d = Quote.USD.PercentChange7d,
                LogoUrl = LogoUrl
            };
        }
    }

    public class Quote
    {
        [JsonProperty("USD")]
        public Usd USD { get; set; }
    }

    public class Usd
    {
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("market_cap")]
        public double MarketCap { get; set; }

        [JsonProperty("volume_24h")]
        public double Volume24h { get; set; }

        [JsonProperty("percent_change_1h")]
        public double PercentChange1h { get; set; }

        [JsonProperty("percent_change_24h")]
        public double PercentChange24h { get; set; }

        [JsonProperty("percent_change_7d")]
        public double PercentChange7d { get; set; }
    }

    public class Coin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public double MarketCap { get; set; }
        public double Volume24h { get; set; }
        public double PercentChange1h { get; set; }
        public double PercentChange24h { get; set; }
        public double PercentChange7d { get; set; }
        public Uri LogoUrl { get; set; }
    }

}