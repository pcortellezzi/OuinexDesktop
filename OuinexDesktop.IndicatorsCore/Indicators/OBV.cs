using System;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class OBV : IndicatorBase
    {
        public XYSerie Result { get; private set; } = new XYSerie("OBV");

        public OBV()
        {
            this.Name = "On Balance Volume";
            this.IsExternal = true;
        }

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            for (var i = 0; i < total; i++)
            {
                if (i == 0)
                {
                    this.Result.Append(volume[i]);
                    continue;
                }

                if (close[i] < close[i - 1])
                {
                    this.Result.Append(this.Result[i - 1] - volume[i]);
                }
                else
                {
                    this.Result.Append(close[i] > close[i - 1] ? this.Result[i - 1] + volume[i] : this.Result[i - 1]);
                }
            }
        }
    }
}
