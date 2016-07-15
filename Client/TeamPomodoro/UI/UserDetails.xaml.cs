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
			if (string.IsNullOrEmpty(txtUserName.Text) || string.IsNullOrEmpty(txtPassword.SecurePassword.ToString()))
				return;

			try
			{
				Cursor = Cursors.Wait;

				Guid? teamId = null;
				if (cbTeam.SelectedItem != null)
					teamId = ((Team)cbTeam.SelectedItem).TeamId;

				// TODO: Take care of password handling
				DialogResult = await Controller.Instance.AddUser(
					txtUserName.Text,
					"Pass123",
					numUpDown.Value,
					(bool)chkShowWarning.IsChecked,
					teamId
				);

				if (DialogResult == true)
					return;

				MessageDialog.Show(Strings.MsgUserExists);
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
