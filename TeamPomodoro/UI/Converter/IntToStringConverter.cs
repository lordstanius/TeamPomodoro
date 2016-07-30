using System;
using System.Globalization;
using System.Windows.Data;

namespace TeamPomodoro.UI.Converter
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? System.Convert.ToString(value, CultureInfo.InvariantCulture) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? int.Parse(value.ToString(), CultureInfo.InvariantCulture) : 0;
        }
    }
}