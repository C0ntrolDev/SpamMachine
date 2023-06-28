using System;
using System.Globalization;
using System.Windows.Data;

namespace SpamBotRemaster.Infrastructure.Converters
{
    internal class ToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse((string)value, out int res))
            {
                return res;
            }
            return -1;
        }
    }
}

