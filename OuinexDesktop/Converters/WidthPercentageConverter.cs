using Avalonia.Controls;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace OuinexDesktop.Converters
{
    public class WidthPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parent = (parameter as Border).Parent as UserControl;

            if (value is double width && parameter is Border border && parent.Width > 0)
            {
                return parent.Width * width / 100;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
