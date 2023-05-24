namespace OuinexDesktop.Models
{
    public enum SymbolType
    {
        Spot,
        Futures,
        Margin,
        Option
    }

    public enum TimeFrime :int
    {
        M1 =1,
        M5=5,
        M15=15,
        M30=30,
        H1=60,
        H2=120,
        H4=240,
        Daily=1440
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
        Pending
    }

    public enum OrderSide
    {
        BUY,
        SELL
    }

    public enum OrderStatus
    {
        Filled,
        Canceled,
        PartiallyFilled,
        Error
    }
}