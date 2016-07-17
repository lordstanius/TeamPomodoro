using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace TeamPomodoro.Util
{
	class IndexToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value > -1);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
