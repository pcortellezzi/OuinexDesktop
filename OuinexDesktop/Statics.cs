using Avalonia.Controls;
using Avalonia.Styling;
using OuinexDesktop.Exchanges;
using OuinexDesktop.Models;
using System.Collections.Generic;

namespace OuinexDesktop
{
    public static class Statics
    {
        public static ThemeVariant Theme { get; set; }

        public static Window MainWinw { get; set; }
    }

    public static class ExchangesConnector
    {
        public static Dictionary<string, Exchange> Instances { get; } = new Dictionary<string, Exchange>()
        {
            { "POC-Binance", new POCEx() }
        };
    }
}
