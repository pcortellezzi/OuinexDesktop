using System.ComponentModel;
using System.Drawing;

namespace CNergyTrader.Indicator
{
    [Browsable(false)]
    public class XYSerie : Dictionary<DateTime, double>
    { 
        public PlotType PlotType { get; set; } = PlotType.Line;
        public Color DefaultColor { get; set; } = Color.Red;
        public int Lenght { get; set; } = 1;
        public bool IsEnabled { get; set; } = true;        
        public string SerieName { get; }        

        public XYSerie(string name)
        {
            SerieName = name;
        }

        public void Append(DateTime Time, double value)
        {
            Add(Time, value);
        }
    }
}
