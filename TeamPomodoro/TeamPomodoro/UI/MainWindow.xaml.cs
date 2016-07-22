using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using TeamPomodoro.Core;

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
            Util.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Controller.Create(this);

            AppDomain.CurrentDomain.UnhandledException += (o, a) => { MessageDialog.ShowError((Exception)a.ExceptionObject, "AppDomain unhandeled exception"); Close(); };
            Application.Current.DispatcherUnhandledException += (o, a) => { MessageDialog.ShowError((Exception)a.Exception, "Dipatcher unhandeled exception"); Close(); };
        }

        private void OnEditTeamsClick(object sender, RoutedEventArgs e)
        {
            Controller.Instance.ShowEditTeams();
        }

        private void OnEditProjectsClick(object sender, RoutedEventArgs e)
        {
            Controller.Instance.ShowEditProjets();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnSignOut(object sender, RoutedEventArgs e)
        {
            Controller.Instance.SignOut();
        }

        private void OnSignInClick(object sender, RoutedEventArgs e)
        {
            Controller.Instance.SignIn();
        }

        private async void OnUserSettingsClick(object sender, RoutedEventArgs e)
        {
            await Controller.Instance.ShowUserDetails();
        }

        private void OnEditTasksClick(object sender, RoutedEventArgs e)
        {
            Controller.Instance.ShowEditTasks();
        }

        private void OnSwitchChecked(object sender, RoutedEventArgs e)
        {
            Controller.Instance.StartPomodoro();
        }

        private void OnSwitchUnhecked(object sender, RoutedEventArgs e)
        {
            Controller.Instance.StopPomodoro();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Controller.Instance.UpdateGuiOnTaskChanged();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            Controller.Instance.Dispose();
        }

        private void OnPomodorosClick(object sender, RoutedEventArgs e)
        {
            Controller.Instance.ShowPomodoros();
        }
    }
}
