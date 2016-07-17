using System;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;
using Model;

namespace TeamPomodoro.UI
{
	/// <summary>
	/// Interaction logic for UserDetails.xaml
	/// </summary>
	public partial class UserDetails : Window
	{
		public UserDetails()
		{
			InitializeComponent();
		}

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}

		async void OnSaveClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(userName.Text) || string.IsNullOrEmpty(password.SecurePassword.ToString()))
				return;

			try
			{
				Cursor = Cursors.Wait;

				Guid? teamId = null;
				if (teams.SelectedItem != null)
					teamId = ((Team)teams.SelectedItem).TeamId;

				// TODO: Take care of password handling
				await Controller.Instance.UpdateUser(userName.Text, "Pass123", numUpDown.Value,
					(bool)chkShowWarning.IsChecked,
					teamId);

				DialogResult = true;
			}
			catch (Exception ex)
			{
				MessageDialog.Show(ex.Message);
			}
			finally
			{
				Cursor = Cursors.Arrow;
			}
		}
	}
}
