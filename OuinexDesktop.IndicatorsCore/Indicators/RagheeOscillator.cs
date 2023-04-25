namespace CNergyTrader.Indicator.Indicators
{
    public sealed class RagheeOscillator : IndicatorBase
    {
        public RagheeOscillator()
        {
            IsExternal = true;
            Name = "Raghee Oscillator";
        }

        public XYSerie Low { get; private set; } = new XYSerie("Raghee oscillator");

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            var highValues = new double[total];
            var lowValues = new double[total];
            var middleValues = new double[total];

            for (int i = 0; i < total; i++)
            {
                highValues[i] = high.GetEMA(i, 34, i == 0 ? high[i] : highValues[i - 1]);
                lowValues[i] = low.GetEMA(i, 34, i == 0 ? low[i] : lowValues[i - 1]);
                middleValues[i] = close.GetEMA(i, 34, i == 0 ? close[i] : middleValues[i - 1]);

                Low.Append((highValues[i] - lowValues[i]) - (middleValues[i] - lowValues[i])); 
            }
        }
    }
}
