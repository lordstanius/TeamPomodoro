using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;

namespace TeamPomodoro.UI
{
	/// <summary>
	/// Interaction logic for SignIn.xaml
	/// </summary>
	public partial class SignIn : Window
	{
		public SignIn()
		{
			InitializeComponent();
		}

		void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
		}

		async void OnSignInClick(object sender, RoutedEventArgs e)
		{
			Cursor = Cursors.Wait;
			try
			{
				if (await Controller.Instance.GetUser(txtUserName.Text, txtPassword.SecurePassword.ToString()))
				{
					DialogResult = true;
					return;
				}
				if (MessageDialog.ShowYesNo(Strings.MsgUserCannotBeFound))
					if (await Controller.Instance.ShowUserDetails(txtUserName.Text))
						DialogResult = true;
			}
			catch (Exception ex)
			{
				MessageDialog.ShowError(ex.ToString());
			}
			finally
			{
				Cursor = Cursors.Arrow;
			}
		}
	}
}
