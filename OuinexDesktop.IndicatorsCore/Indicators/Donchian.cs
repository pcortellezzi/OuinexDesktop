using System.Drawing;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class Donchian : IndicatorBase
    {
        public XYSerie Up { get; private set; } = new XYSerie("Up") { DefaultColor = Color.Green };

        public XYSerie Down { get; private set; } = new XYSerie("Down") { DefaultColor = Color.OrangeRed };

        [IndicatorParameter]
        public int Period { get; set; } = 14;

        public Donchian()
        {
        }

        public override void Init()
        {
            Name = $"Average True Range (ATR) [{this.Period}]";
        }

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            for (int i = 0; i < total; i++)
            {
                this.Up.Append(high.GetHighest(i, Period));
                this.Down.Append(low.GetLowest(i, Period));
            }
        }
    }
}
