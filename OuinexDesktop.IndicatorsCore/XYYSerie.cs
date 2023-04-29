using System.ComponentModel;
using System.Drawing;

namespace CNergyTrader.Indicator
{
    [Browsable(false)]
    public class XYYSerie : Dictionary<int, (double,double)>
    {
        public string SerieName { get; set; }
        public bool IsEnabled { get; set; } = true;
        public Color UpColor { get; set; } = Color.Green;
        public Color DownColor { get; set; } = Color.Crimson;

        public XYYSerie(string serieName)
        {
            SerieName = serieName;
        }

        public void Append(int index, (double, double) values)
        {
            Add(index, values);
        }
    }
}
