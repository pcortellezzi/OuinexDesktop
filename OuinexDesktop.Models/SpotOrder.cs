namespace OuinexDesktop.Models
{
    public class SpotOrder
    {
        public string OrderSymbol { get; set; }

        public decimal Amount { get; set; }
        public decimal FilledAmount { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
    }
}
