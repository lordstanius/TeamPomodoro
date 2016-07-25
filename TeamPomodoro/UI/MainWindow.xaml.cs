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

        private void OnEditTeamsClick(object sender, RoutedEventArgs e)
        {
            //Controller.Instance.ShowEditTeams();
        }

        private void OnEditProjectsClick(object sender, RoutedEventArgs e)
        {
            //Controller.Instance.ShowEditProjets();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnSignOut(object sender, RoutedEventArgs e)
        {
            //Controller.Instance.SignOut();
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
                    tasks.IsEnabled = tasks.Items.Count > 0;
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

        private void OnEditTasksClick(object sender, RoutedEventArgs e)
        {
            //Controller.Instance.ShowEditTasks();
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
            //Controller.Instance.Dispose();
        }

        private void OnPomodorosClick(object sender, RoutedEventArgs e)
        {
            //Controller.Instance.ShowPomodoros();
        }
    }
}