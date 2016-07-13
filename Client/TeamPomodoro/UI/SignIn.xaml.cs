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

		async void OnSignOnClick(object sender, RoutedEventArgs e)
		{
			DialogResult = Controller.Instance.ValidateUser(txtUserName.Text, txtPassword.SecurePassword.ToString());
			if (DialogResult == true)
				return;

			if (MessageDialog.ShowYesNo(Strings.MsgUserCannotBeFound))
				DialogResult = await Controller.Instance.ShowUserDetails(txtUserName.Text);
		}
	}
}
