using CNergyTrader.Indicator;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;
using System.Threading.Tasks.Dataflow;
using System.Drawing;

namespace OuinexDesktop.Charting
{
    public class OnPriceIndicatorItem
    {
        private IndicatorBase _indicator;
        private AvaPlot _priceArea;

        public OnPriceIndicatorItem(IndicatorBase indicator, AvaPlot priceArea) 
        {
            _indicator = indicator;
            _priceArea = priceArea;

            _indicator.Init();
            setupSeries();
        }

        private void setupSeries()
        {
            foreach(var serie in _indicator.Series)
            {
                switch (serie.PlotType)
                {
                    case PlotType.Line:
                        var line = _priceArea.Plot.AddScatterLines(null, null, serie.DefaultColor);
                        line.OnNaN = ScatterPlot.NanBehavior.Gap;

                        _indicator.OnCalculated += () =>
                        {
                            line.Update(serie.Select(x => (double)serie.IndexOf(x)).ToArray(), serie.ToArray());
                        };
                        break;
                    case PlotType.Histogram:
                        var bars = _priceArea.Plot.AddBarSeries();
                        
                        _indicator.OnCalculated += () =>
                        {
                            bars.Bars.Clear();

                            foreach(var data in serie)
                            {
                                var bar = new Bar();
                                bar.Value = data;                                
                                bar.Position = serie.IndexOf(data);
                                bar.FillColor = data > 0 ? Color.Green : Color.Red;
                                bars.Bars.Add(bar);
                            }
                        };
                        break;
                }               
            }
        }
    }
}