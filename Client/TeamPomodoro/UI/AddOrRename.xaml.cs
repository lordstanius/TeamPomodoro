using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;

namespace TeamPomodoro.UI
{
	public partial class AddOrRename : Window
	{
		public AddOrRename()
		{
			InitializeComponent();
		}

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}

		void OnSaveClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}

