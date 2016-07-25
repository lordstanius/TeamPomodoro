using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using ViewModel;
using ViewModel.Globalization;

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
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private async void OnSignInClick(object sender, RoutedEventArgs e)
        {
            var viewModel = (SignInViewModel)FindResource("SignInViewModel");
            viewModel.Password = passwordBox.SecurePassword;

            try
            {
                Cursor = Cursors.Wait;
                signInButton.IsEnabled = false;

                if (!viewModel.ValidateUser(userName.Text))
                {
                    return;
                }

                bool? result = await viewModel.GetUser();
                if (result == true)
                {
                    DialogResult = true;
                }
                else if (result == false)
                {
                    MessageDialog.Show(this, Strings.MsgWrongPass);
                }
                else
                {
                    // result is null, which means user is not found
                    if (MessageDialog.ShowYesNo(this, Strings.MsgUserCannotBeFound))
                    {
                        if (await UserDetails.ShowDialog(this, userName.Text) == true)
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
                signInButton.IsEnabled = true;
            }
        }
    }
}
