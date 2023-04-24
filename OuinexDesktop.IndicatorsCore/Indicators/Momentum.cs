using System.Drawing;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class Momentum : IndicatorBase
    {
        [IndicatorParameter]
        public int Period { get; set; } = 20; 

        public Momentum()
        { 
            IsExternal = true; 
        }

        public override void Init()
        {
            Name = $"Momentum [{Period}]";
        }

        public XYSerie Values { get; private set; } = new XYSerie("Momentum") { DefaultColor = Color.CadetBlue };

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            for(int i = 0; i<total; i++)
            {
                Values.Append(time[i], i < Period ? double.NaN : close[i] * 100 / close[i - Period]);
            }
        }
    }
}

