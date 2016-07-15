using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Controls;

namespace TeamPomodoro.UI
{
	/// <summary>
	/// Interaction logic for SignIn.xaml
	/// </summary>
	public partial class EditDialog : Window
	{
		public EditDialog()
		{
			InitializeComponent();
		}

		void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			btnEdit.IsEnabled = list.SelectedItem != null;
		}

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}
	}
}
