namespace OuinexDesktop.Models
{
    public partial class Symbol
    {
        public string BaseCurrency { get;  set; } = string.Empty;

        public string QuoteCurrency { get;  set; } = string.Empty;

        public string FullName => BaseCurrency + "/" + QuoteCurrency;

        public string Name { get;  set; } = string.Empty;

        public decimal TickSize { get; set;} = decimal.Zero;

    }
}
