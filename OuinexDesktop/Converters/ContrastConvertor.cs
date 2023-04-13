using Avalonia.Data.Converters;
using Avalonia.Media;
using System;

namespace OuinexDesktop.Converters
{
    public class ContrastConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return Brushes.Gray;
                }
                var brush = (SolidColorBrush)value;
                // Extract RGB and apply formula
                var foreGround = Brightness(brush.Color) > 150 ? Brushes.Black : Brushes.White;
                return foreGround;
            }
            catch (Exception)
            {
                throw; // return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        private int Brightness(Color c)
        {
            return (int)Math.Sqrt(c.R * c.R * .241 + c.G * c.G * .691 + c.B * c.B * .068);
        }
    }
}
