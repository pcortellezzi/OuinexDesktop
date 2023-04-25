namespace CNergyTrader.Indicator.Indicators
{
    public sealed class Volume : IndicatorBase
    {
        public XYSerie Volumes { get; } = new XYSerie("Volume") { PlotType = PlotType.Histogram };
        public XYSerie Sma { get; } = new XYSerie("Average");

        public Volume()
        {
            Name = "Volumes";
            IsExternal = true;
        }

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            for(int i = 0; i<total; i++)
            {
                Volumes.Append(volume[i]);
                Sma.Append(volume.GetSMA(i, 50));
            }
        }
    }
}
