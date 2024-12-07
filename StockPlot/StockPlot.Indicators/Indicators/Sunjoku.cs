using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace StockPlot.Indicators.Indicators
{
    public sealed class Sunjoku : IndicatorBase
    {
        public XYYSerie CloudUp { get; private set; } = new XYYSerie("CloudUp") { Color = Color.Green };
        public XYYSerie CloudDown { get; private set; } = new XYYSerie("CloudUp") { Color = Color.Red };
        public XYSerie UpFast { get; private set; } = new XYSerie("UpFast") { DefaultColor = Color.Green };

        public XYSerie DownFast { get; private set; } = new XYSerie("DownFast") { DefaultColor = Color.OrangeRed };

        public XYSerie UpSlow { get; private set; } = new XYSerie("UpSlow") { DefaultColor = Color.Green };

        public XYSerie DownSlow { get; private set; } = new XYSerie("DownSlow") { DefaultColor = Color.OrangeRed };

        public XYSerie Fib1 { get; private set; } = new XYSerie("Fib1") { DefaultColor = Color.WhiteSmoke , PlotType = PlotType.Line};

        public XYSerie Fib2 { get; private set; } = new XYSerie("Fib2") { DefaultColor = Color.WhiteSmoke, PlotType = PlotType.Line };

        public XYSerie Fib3 { get; private set; } = new XYSerie("Fib3") { DefaultColor = Color.WhiteSmoke, PlotType = PlotType.Line };


        [IndicatorParameter]
        public int PeriodFast { get; set; } = 20;

        [IndicatorParameter]
        public int PeriodSlow { get; set; } = 50;

        [IndicatorParameter]
        public double ML1Mult = 0.382;

        [IndicatorParameter]
        public double ML2Mult = 0.5;

        [IndicatorParameter]
        public double ML3Mult = 0.618;
        public Sunjoku()
        {
            //AddFill("Up", "Down");
        }

        public override void Init()
        {
            Name = $"Sunjoku [{this.PeriodFast}, {this.PeriodSlow}]";
        }

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            for (int i = 0; i < total; i++)
            {
                var _high = high.GetHighest(i, PeriodFast);
                var _low = low.GetLowest(i, PeriodFast);

                this.UpFast.Append((time[i], _high));
                this.DownFast.Append((time[i], _low));

                this.UpSlow.Append((time[i], high.GetHighest(i, PeriodSlow)));
                this.DownSlow.Append((time[i], low.GetLowest(i, PeriodSlow)));

                this.Fib1.Append((time[i],_low+(_high-_low)*ML1Mult));

                this.Fib2.Append((time[i], _low + (_high - _low) * ML2Mult));
                this.Fib3.Append((time[i], _low + (_high - _low) * ML3Mult));

                CloudUp.Append((time[i], UpSlow[i].Value, UpFast[i].Value));
                CloudDown.Append((time[i], DownSlow[i].Value, DownFast[i].Value));
            }
        }
    }
}
