using System.Drawing;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class BullBearPower : IndicatorBase
    {
        [IndicatorParameter]
        public int Period { get; set; } = 20;

        public BullBearPower()
        {
            
            IsExternal = true;
        }

        public XYSerie Bull { get; private set; } = new XYSerie("Bull") { DefaultColor = Color.CadetBlue };
        public XYSerie Bear { get; private set; } = new XYSerie("Bear") { DefaultColor = Color.Orange };

        private ExponentialMovingAverage ema;

        public override void Init()
        {
            ema = new ExponentialMovingAverage() { Period = Period };
            Name = $"Bull and Bear [{Period}]";
        }

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            ema.Calculate(total, time, open, high, low, close, volume);

            for (int i = 0; i < total; i++)
            {                
                Bull.Append(i < Period ? double.NaN : low[i] - ema.Ma[i]);
                Bear.Append(i < Period ? double.NaN : high[i] - ema.Ma[i]);
            }
        }
    }
}

