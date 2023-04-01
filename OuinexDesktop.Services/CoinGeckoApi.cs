using Newtonsoft.Json;

public class CoinGeckoAPI
{
    private const string BASE_URL = "https://api.coingecko.com/api/v3";

    public async Task<BitcoinData> GetCoinInfo(string coinId="bitcoin")
    {
        var coin = await GetCoinName(coinId);

        using var client = new HttpClient();
        var url = $"{BASE_URL}/coins/{coin}?include_image=true";

        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var coinInfo = JsonConvert.DeserializeObject<BitcoinData>(json);
            return coinInfo;
        }
        else
        {
            throw new Exception($"Failed to retrieve coin info for {coinId}. HTTP Status Code: {response.StatusCode}");
        }
    }
    public async Task<string> GetCoinName(string symbol)
    {
        using var httpClient = new HttpClient();
        var url = $"https://api.coingecko.com/api/v3/coins?symbol={symbol}";
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve coin information for symbol {symbol}.");
        }

        var json = await response.Content.ReadAsStringAsync();
        var coin = JsonConvert.DeserializeObject<Coin>(json);
        return coin.Name;
    }
}

public class BitcoinData
{
    public string Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    public ImageData Image { get; set; }

    [JsonProperty("market_data")]
    public MarketData MarketData { get; set; }
}

public class ImageData
{
    public string Thumb { get; set; }
    public string Small { get; set; }
    public string Large { get; set; }
}

public class MarketData
{
    [JsonProperty("current_price")]
    public Dictionary<string, decimal> CurrentPrice { get; set; }
    [JsonProperty("market_cap")]
    public Dictionary<string, decimal> MarketCap { get; set; }
    [JsonProperty("total_volume")]
    public Dictionary<string, decimal> TotalVolume { get; set; }
    // ... other properties
}

public class Coin
{
    public string Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    // ... other properties
}
