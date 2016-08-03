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
