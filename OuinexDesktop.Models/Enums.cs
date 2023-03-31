namespace OuinexDesktop.Models
{
    public enum SymbolType
    {
        Spot,
        Futures,
        Margin,
        Option
    }

    public enum TimeFrime
    {
        M1,
        M5,
        M15,
        M30,
        H1,
        H2,
        H4,
        Daily,
        Weekly,
        Monthly,
        Year
    }

    public enum TradeSide
    {
        Long,
        Short
    }

    public enum OrderType
    {
        Market,
        StopLoss,
        TakeProfit,
    }
}