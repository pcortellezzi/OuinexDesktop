using System.Drawing;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class PsychoLine : IndicatorBase
    {
        public XYSerie Result { get; private set; } = new XYSerie("");

        public int Period { get; set; } = 12;

        public PsychoLine()
        {
            IsExternal = true;
            Name = "Psycho line";

            CreateLevel(75, Color.Red);
            CreateLevel(25, Color.Green);
        }

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            for (int i = 0; i < total; i++)
            {
                double UpBarCount = 0;

                for (int j = 0; j < Period; j++)
                {
                    if (close[i + j] > close[i + j + 1]) UpBarCount++;
                }

                Result.Append(100 * UpBarCount / Period);
            }
        }
    }
}
