using System;
using System.Globalization;
using System.Windows.Data;

namespace TeamPomodoro.UI.Converter
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).Length > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}