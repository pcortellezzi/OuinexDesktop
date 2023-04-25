using System.Drawing;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class ParabolicSAR : IndicatorBase
    {
        public int Period { get; set; } = 20;
        double[] ExtSARBuffer;
        double []ExtEPBuffer;
        double []ExtAFBuffer;
        //--- global variables
        int ExtLastRevPos;
        bool ExtDirectionLong;
        double ExtSarStep = 0.02;
        double ExtSarMaximum = 0.2;
        //+----------------------------------

        public ParabolicSAR()
        {
            Name = "Parabolic SAR";
            IsExternal = false;
        }

        public XYSerie Value { get; private set; } = new XYSerie("Parabolic") { DefaultColor = Color.CadetBlue };

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            ExtEPBuffer = new double[total];
            ExtAFBuffer = new double[total];
            ExtSARBuffer = new double[total];


            for (int i = total -1; i>=0; i--)
            {
                //--- check for reverse
                if (ExtDirectionLong)
                {
                    if (ExtSARBuffer[i] > low[i])
                    {
                        //--- switch to SHORT
                        ExtDirectionLong = false;
                        ExtSARBuffer[i] = high.GetHighest(i, ExtLastRevPos);
                        ExtEPBuffer[i] = low[i];
                        ExtLastRevPos = i;
                        ExtAFBuffer[i] = ExtSarStep;
                    }
                }
                else
                {
                    if (ExtSARBuffer[i] < high[i])
                    {
                        //--- switch to LONG
                        ExtDirectionLong = true;
                        ExtSARBuffer[i] = low.GetLowest(i, ExtLastRevPos);//GetLow(i, ExtLastRevPos, low);
                        ExtEPBuffer[i] = high[i];
                        ExtLastRevPos = i;
                        ExtAFBuffer[i] = ExtSarStep;
                    }
                }
                //--- continue calculations
                if (ExtDirectionLong)
                {
                    //--- check for new High
                    if (high[i] > ExtEPBuffer[i - 1] && i != ExtLastRevPos)
                    {
                        ExtEPBuffer[i] = high[i];
                        ExtAFBuffer[i] = ExtAFBuffer[i - 1] + ExtSarStep;
                        if (ExtAFBuffer[i] > ExtSarMaximum)
                            ExtAFBuffer[i] = ExtSarMaximum;
                    }
                    else
                    {
                        //--- when we haven't reversed
                        if (i != ExtLastRevPos)
                        {
                            ExtAFBuffer[i] = ExtAFBuffer[i - 1];
                            ExtEPBuffer[i] = ExtEPBuffer[i - 1];
                        }
                    }
                    //--- calculate SAR for tomorrow
                    ExtSARBuffer[i + 1] = ExtSARBuffer[i] + ExtAFBuffer[i] * (ExtEPBuffer[i] - ExtSARBuffer[i]);
                    //--- check for SAR
                    if (ExtSARBuffer[i + 1] > low[i] || ExtSARBuffer[i + 1] > low[i - 1])
                        ExtSARBuffer[i + 1] = Math.Min(low[i], low[i - 1]);
                }
                else
                {
                    //--- check for new Low
                    if (low[i] < ExtEPBuffer[i - 1] && i != ExtLastRevPos)
                    {
                        ExtEPBuffer[i] = low[i];
                        ExtAFBuffer[i] = ExtAFBuffer[i - 1] + ExtSarStep;
                        if (ExtAFBuffer[i] > ExtSarMaximum)
                            ExtAFBuffer[i] = ExtSarMaximum;
                    }
                    else
                    {
                        //--- when we haven't reversed
                        if (i != ExtLastRevPos)
                        {
                            ExtAFBuffer[i] = ExtAFBuffer[i - 1];
                            ExtEPBuffer[i] = ExtEPBuffer[i - 1];
                        }
                    }
                    //--- calculate SAR for tomorrow
                    ExtSARBuffer[i + 1] = ExtSARBuffer[i] + ExtAFBuffer[i] * (ExtEPBuffer[i] - ExtSARBuffer[i]);
                    //--- check for SAR
                    if (ExtSARBuffer[i + 1] < high[i] || ExtSARBuffer[i + 1] < high[i - 1])
                        ExtSARBuffer[i + 1] = Math.Max(high[i], high[i - 1]);
                }

                Value.Append(ExtSARBuffer[i]);
            }
        }
    }
}

