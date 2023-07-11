using SpamBotRemaster.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SpamBotRemaster.Infrastructure.Enums;

namespace SpamBotRemaster.Infrastructure.Converters
{
    class LanguageToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Language)value)
            {
                case Language.eng:
                    return "English";
                    
                case Language.ru:
                    return "Русский";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)value)
            {
                case "English":
                    return Language.eng;

                case "Русский":
                    return Language.ru;
            }

            return AplicationSettings.DeffaultSettings.Language;
        }
    }
}
