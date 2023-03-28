using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Globalization;

namespace OuinexPro.Converters
{
    public class TickToForegroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            /*var theme = Statics.Theme;
                          
            if (value == null)
                return theme == ThemeVariant.Dark ? Brushes.White : Brushes.Black;

            var state = (TickState)value;

            if(state == TickState.NEUTRAL)
            {
                return theme == ThemeVariant.Dark ? Brushes.White : Brushes.Black;
            }*/

            return Brushes.White;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
