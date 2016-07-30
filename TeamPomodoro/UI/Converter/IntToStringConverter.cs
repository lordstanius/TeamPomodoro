using System;
using System.Globalization;
using System.Windows.Data;

namespace TeamPomodoro.UI.Converter
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? value.ToString() : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? int.Parse(value.ToString()) : 0;
        }
    }
}