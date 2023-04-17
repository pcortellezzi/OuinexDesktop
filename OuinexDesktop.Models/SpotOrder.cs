namespace OuinexDesktop.Models
{
    public class SpotOrder
    {
        public string OrderSymbol { get; set; }
        public string OrderId { get; set; } = "H76FS_HUF-0";
        public decimal Amount { get; set; }
        public decimal FilledAmount { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Price { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime OrderTime { get; } = DateTime.MinValue;
        public OrderSide OrderSide { get; set; }
    }
}
