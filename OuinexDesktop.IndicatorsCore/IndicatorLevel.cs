using System.Drawing;

namespace CNergyTrader.Indicator
{
    public class IndicatorLevel
    {
        public IndicatorLevel(double y, Color levelColor)
        {
            this.Y = y;
            this.LevelColor = levelColor;
        }

        public double Y { get; }
        public Color LevelColor { get; set; } = Color.Red;
        public bool IsEnabled { get; set; } = true;
    }
}
