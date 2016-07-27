using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using ViewModel;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainWindowViewModel)FindResource("MainWindowViewModel");
            viewModel.Initialize(Properties.Settings.Default.Uri);
            AppDomain.CurrentDomain.UnhandledException += (o, a) => { MessageDialog.ShowError((Exception)a.ExceptionObject, "AppDomain unhandeled exception"); Close(); };
            Application.Current.DispatcherUnhandledException += (o, a) => { MessageDialog.ShowError((Exception)a.Exception, "Dipatcher unhandeled exception"); Close(); };
        }

        private async void OnEditTeamsClick(object sender, RoutedEventArgs e)
        {
            await EditTeamsDialog.ShowEditDialog(this);
        }

        private async void OnEditProjectsClick(object sender, RoutedEventArgs e)
        {
            await EditProjectsDialog.ShowEditDialog(this);
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnSignOut(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainWindowViewModel)FindResource("MainWindowViewModel");
            
            // TODO: move these to view model
            pomodoro.Visibility = Visibility.Hidden;
            toggle.IsChecked = false;

            viewModel.SignOut();
        }

        private async void OnSignInClick(object sender, RoutedEventArgs e)
        {
            var signIn = new SignIn
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            signIn.userName.Focus();
            if (signIn.ShowDialog() == true)
            {
                var viewModel = (MainWindowViewModel)FindResource("MainWindowViewModel");
                Cursor = Cursors.Wait;
                try
                {
                    await viewModel.SignIn();
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowError(ex, "SignIn.OnSignInClick(): failed to get tasks for user");
                }

                Cursor = Cursors.Arrow;
            }
        }

        private async void OnUserSettingsClick(object sender, RoutedEventArgs e)
        {
            await UserDetails.ShowDialog(this, null);
        }

        private async void OnEditTasksClick(object sender, RoutedEventArgs e)
        {
            await EditTasksDialog.ShowEditDialog(this);
            {
                var viewModel = (MainWindowViewModel)FindResource("MainWindowViewModel");
                await viewModel.GetTasks();
            }
        }

        private void OnSwitchChecked(object sender, RoutedEventArgs e)
        {
            //Controller.Instance.StartPomodoro();
        }

        private void OnSwitchUnhecked(object sender, RoutedEventArgs e)
        {
            //Controller.Instance.StopPomodoro();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Controller.Instance.UpdateGuiOnTaskChanged();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            var viewModel = (MainWindowViewModel)FindResource("MainWindowViewModel");
            viewModel.Dispose();
        }

        private void OnPomodorosClick(object sender, RoutedEventArgs e)
        {
            //Controller.Instance.ShowPomodoros();
        }
    }
}