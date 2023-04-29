using CNergyTrader.Indicator;
using ScottPlot.Avalonia;
using ScottPlot.Plottable;
using System.Threading.Tasks.Dataflow;
using System.Drawing;
using Avalonia.Controls.Shapes;
using DynamicData;

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

            displayXYSeries();
            displayXYYSeries();
            displayLevels();
        }

        private void displayXYSeries()
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

        private void displayXYYSeries()
        {
            foreach(var xyy in _indicator.XyySeries)
            {
                _indicator.OnCalculated += () =>
                {
                    var xs = xyy.Select(x => (double)xyy.IndexOf(x)).ToArray();
                    var y1 = xyy.Values.Select(x => x.Item1).ToArray();
                    var y2 = xyy.Values.Select(x => x.Item2).ToArray();
                    //  line.Update(serie.Select(x => (double)serie.IndexOf(x)).ToArray(), serie.ToArray());
                    var fill = _priceArea.Plot.AddFill(xs,y2,y1);         
                };
            }
        }

        private void displayLevels()
        {
            if (_indicator.Levels.Count <= 0)
                return;

            foreach(var level in _indicator.Levels)
            {
                var line = _priceArea.Plot.AddHorizontalLine(level.Y, level.LevelColor, label: level.Y.ToString());
            }
        }


        private IPlottable test(double[] bothX, double[] bothY)
        {
           var plottable = new ScottPlot.Plottable.Polygon(bothX, bothY)
            {
                Fill = true,
                FillColor = System.Drawing.Color.Red,
                LineWidth = 1,
                LineColor = System.Drawing.Color.Black
            };

            return plottable;
        }
    }
}