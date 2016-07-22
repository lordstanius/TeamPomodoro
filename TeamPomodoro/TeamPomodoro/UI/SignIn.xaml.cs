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

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        async void OnSignInClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Cursor = Cursors.Wait;
                btnSignIn.IsEnabled = false;

                if (!Controller.Instance.ValidateUser(userName.Text))
                {
                    return;
                }

                bool? result = await Controller.Instance.GetUser(userName.Text, password.SecurePassword);
                if (result == true)
                {
                    DialogResult = true;
                    return;
                }
                else if (result == false)
                {
                    if (MessageDialog.ShowYesNo(Strings.MsgUserCannotBeFound))
                    {
                        if (await Controller.Instance.ShowUserDetails(userName.Text))
                        {
                            DialogResult = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "SignIn.OnSignInClick()");
            }
            finally
            {
                Cursor = Cursors.Arrow;
                btnSignIn.IsEnabled = true;
            }
        }
    }
}
