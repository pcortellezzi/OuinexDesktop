using System.Drawing;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class BollingerBands : IndicatorBase
    {
        public XYSerie Middle { get; } = new XYSerie("Middle") { DefaultColor = Color.Blue };

        public XYSerie Up { get; private set; } = new XYSerie("Up");

        public XYSerie Down { get; private set; } = new XYSerie("Down");

        [IndicatorParameter]
        public int Period { get; set; } = 20;  

        [IndicatorParameter]
        public double Deviation { get; set; } =2.0;

        public override void Init()
        {
            this.Name = $"Bollinger Bands [{Period}]";
        }

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            for (int i = 0; i < total; i++)
            {
                var array = this.Middle.ToArray();
                var std = close.GetStdDev(i, Period, array);
                var ma = close.GetSMA(i, Period);

                this.Middle.Append(ma);
                this.Down.Append(ma - (Deviation * std));
                this.Up.Append(ma + (Deviation * std));
            }
        }
    }
}
