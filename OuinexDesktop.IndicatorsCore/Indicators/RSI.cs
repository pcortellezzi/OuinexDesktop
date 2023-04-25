using System.Drawing;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class RSI : IndicatorBase
    {
        [IndicatorParameter]
        public int Period { get; set; } = 20;

        public RSI()
        {
            IsExternal = true;

            CreateLevel(50, Color.White);
            CreateLevel(30, Color.Gray);
            CreateLevel(70, Color.Gray);
        }

        public override void Init()
        {
            Name = $"RSI [{Period}]";
        }

        public XYSerie Values { get; private set; } = new XYSerie("RSI") { DefaultColor = Color.OrangeRed };

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            var rsi = new double[total];

            double gain = 0.0;
            double loss = 0.0;

            // first RSI value
            rsi[0] = 0.0;
            for (int i = 1; i <= Period; ++i)
            {
                var diff = close[i] - close[i - 1];
                if (diff >= 0)
                {
                    gain += diff;
                }
                else
                {
                    loss -= diff;
                }
            }

            double avrg = gain / Period;
            double avrl = loss / Period;
            double rs = gain / loss;
            rsi[Period] = 100 - (100 / (1 + rs));

            for (int i = Period + 1; i < close.Length; ++i)
            {
                var diff = close[i] - close[i - 1];

                if (diff >= 0)
                {
                    avrg = ((avrg * (Period - 1)) + diff) / Period;
                    avrl = (avrl * (Period - 1)) / Period;
                }
                else
                {
                    avrl = ((avrl * (Period - 1)) - diff) / Period;
                    avrg = (avrg * (Period - 1)) / Period;
                }

                rs = avrg / avrl;

                rsi[i] = 100 - (100 / (1 + rs));
            }

            for(int i = 0; i < rsi.Length; i++)
            {
                if (i < Period)
                    Values.Append(double.NaN);
                else
                    Values.Append(rsi[i]);
            }
        }
    }
}

