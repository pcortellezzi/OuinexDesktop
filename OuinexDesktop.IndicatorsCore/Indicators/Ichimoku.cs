using System.Drawing;

namespace CNergyTrader.Indicator.Indicators
{
    public sealed class Ichimoku : IndicatorBase
    {
        [IndicatorParameter]
        public int InpTenkan { get; set; } = 9;
        [IndicatorParameter]
        public int InpKijun { get; set; } = 26;
        [IndicatorParameter]
        public int InpSenkou { get; set; } = 52;

        private XYYSerie _Cloud { get; } = new XYYSerie("_cloud");


        public XYSerie Tenkan { get; } = new XYSerie("Tenkan");
        public XYSerie Kijun { get; } = new XYSerie("Kijun") { DefaultColor = Color.Red };
        public XYSerie Chikou { get; } = new XYSerie("Chikou") { DefaultColor = Color.Blue };
        public XYYSerie Cloud { get; } = new XYYSerie("Cloud");
        

        public override void Init()
        {
            Name = $"Ichimoku [Tenkan ({InpTenkan}), Kijun ({InpKijun}), Senkou ({InpSenkou})]";
        }

        protected override void Calculate_(int total, DateTime[] time, double[] open, double[] high, double[] low, double[] close, double[] volume)
        {
            _Cloud.Clear();

            for(int i = 0; i<total; i++)
            {
                var highest = high.GetHighest(i, InpTenkan);
                var lowest = low.GetLowest(i, InpTenkan);
                Tenkan.Append((highest + lowest) /2 );

                highest = high.GetHighest(i, InpKijun);
                lowest = low.GetLowest(i, InpKijun);
                Kijun.Append((highest + lowest) / 2);

                highest = high.GetHighest(i, InpSenkou);
                lowest = low.GetLowest(i, InpSenkou);

               // _Cloud.Append(time[i], ((Tenkan[i] + Kijun[i]) / 2, (highest + lowest) / 2));

                Chikou.Append(i < total - InpKijun ? close[i + InpKijun] : double.NaN);
            }

      
            //var span = (time[1] - time[0]).TotalMinutes;

            for(int i =0; i<total+ InpKijun; i++)
            {
                if (i < InpKijun)
                {
                 //   Cloud.Append(time[i], (double.NaN, double.NaN));
                }
                else
                {
                    //var newTime = time[i - InpKijun].AddMinutes(InpKijun * span);

                   // Cloud.Append(newTime, (_Cloud.Values.ToArray()[i- InpKijun].Item1, _Cloud.Values.ToArray()[i - InpKijun].Item2));
                }
            }
        }
    }
}
