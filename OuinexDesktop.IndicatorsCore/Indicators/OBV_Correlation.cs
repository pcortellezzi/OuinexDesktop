namespace CNergyTrader.Indicator.Indicators
{
    public sealed class OBV_Correlation : IndicatorBase
    {
        private int Period_ = 14;

        public XYSerie Result { get; private set; } = new XYSerie("Correlation");

        public int Period
        {
            get
            {
                return this.Period_;
            }
            set
            {
                this.Period_ = value;
            }
        }

        public OBV_Correlation()
        {
            this.Name = "On Balance Volume Correlation";
            this.IsExternal = true;
        }

        private OBV OBV_ = new OBV();
        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            this.OBV_.Calculate(total, time, open, high, low, close, volume);
            var diff1 = new double[total];
            var diff2 = new double[total];

            for (int i = 0; i < total; i++)
            {
                diff1[i] = close[i] - close.GetSMA(i, Period);
                diff2[i] = this.OBV_.Result.Values.ToArray()[i] - OBV_.Result.GetSMA(i, Period);


                double sum = 0, sump1 = 0, sump2 = 0;
                for (int k = 0; k < this.Period && (i - k) >= 0; k++)
                {
                    sum += diff1[i - k] * diff2[i - k];
                    sump1 += diff1[i - k] * diff1[i - k];
                    sump2 += diff2[i - k] * diff2[i - k];
                }

                this.Result.Append(time[i], (sump1 * sump2 != 0) ? sum / Math.Sqrt(sump1 * sump2) : double.NaN);
            }
        }
    }
}
