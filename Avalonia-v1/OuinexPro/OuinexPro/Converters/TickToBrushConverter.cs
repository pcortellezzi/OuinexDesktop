using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace OuinexPro.Converters
{
    public class TickToBrushConverter : IValueConverter
    {
        IBrush _down = Brush.Parse("#EC5656");
        IBrush _up = Brush.Parse("#34a560");
        IBrush _neutral = Brush.Parse("#00000000");
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return _neutral;

            var state = (TickState)value;

            switch(state)
            {
                case TickState.UP:
                    return _up;
                    
                case TickState.NEUTRAL:
                    return _neutral;

                case TickState.DOWN:
                    return _down;

            }

            return _neutral;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
