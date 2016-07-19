using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;

namespace TeamPomodoro.UI
{
	/// <summary>
	/// Interaction logic for SignIn.xaml
	/// </summary>
	public partial class PomodoroDetails : Window
	{
		public PomodoroDetails()
		{
			InitializeComponent();
		}

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}

		private void DateLoaded(object sender, RoutedEventArgs e)
		{
			DatePicker datePicker = sender as DatePicker;
			if (datePicker != null)
			{
				System.Windows.Controls.Primitives.DatePickerTextBox datePickerTextBox = FindVisualChild<System.Windows.Controls.Primitives.DatePickerTextBox>(datePicker);
				if (datePickerTextBox != null)
				{
					ContentControl watermark = datePickerTextBox.Template.FindName("PART_Watermark", datePickerTextBox) as ContentControl;
					if (watermark != null)
					{
						watermark.Content = Strings.TxtChoose;
						watermark.Foreground = Brushes.Silver;
					}
				}
			}
		}

		private T FindVisualChild<T>(DependencyObject depencencyObject) where T : DependencyObject
		{
			if (depencencyObject != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depencencyObject); ++i)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depencencyObject, i);
					T result = (child as T) ?? FindVisualChild<T>(child);
					if (result != null)
						return result;
				}
			}

			return null;
		}

		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			tasks.SelectionChanged -= OnSelectionChanged;
			Controller.Instance.UpdatePomodoroList(this);
			tasks.SelectionChanged += OnSelectionChanged;
		}
	}
}
