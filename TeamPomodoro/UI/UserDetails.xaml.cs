using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ViewModel;
using ViewModel.Globalization;

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

        public static async Task<bool?> ShowDialog(Window owner, string userName)
        {
            try
            {
                owner.Cursor = Cursors.Arrow;

                var userDetails = new UserDetails
                {
                    Owner = owner,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var viewModel = (UserDetailsViewModel)userDetails.FindResource("UserDetailsViewModel");
                await viewModel.Initialize(userName);
                userDetails.passwordBox.Password = viewModel.Password;
                return userDetails.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "Controller.ShowUserDetails()");
                return false;
            }
            finally
            {
                owner.Cursor = Cursors.Arrow;
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var viewModel = (UserDetailsViewModel)FindResource("UserDetailsViewModel");
            viewModel.Password = passwordBox.Password;

            if (!viewModel.ValidateUserName())
            {
                return;
            }

            if (!viewModel.ValidatePassword())
            {
                MessageDialog.Show(this, Strings.MsgPasswordLenght);
                return;
            }
            
            try
            {
                Cursor = Cursors.Wait;
                viewModel.UpdateUser();
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(this, ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
    }
}
