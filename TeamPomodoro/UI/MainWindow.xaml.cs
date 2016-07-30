using System;
using System.Media;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ViewModel;
using ViewModel.Globalization;

namespace TeamPomodoro.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _timer;
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _timer = new Timer { Interval = 1000 };
            _timer.Elapsed += OnTimerElapsed;

            _viewModel = (MainWindowViewModel)FindResource("MainWindowViewModel");
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _viewModel.OnTimer();
            if (_viewModel.IsTimeExpired)
            {
                Dispatcher.Invoke(OnPomodoroCompleted);
            }
        }

        private void OnPomodoroCompleted()
        {
            toggle.IsChecked = false;

            if (!_viewModel.ShouldShowWarning)
            {
                return;
            }

            var sp = new SoundPlayer(Properties.Resources.martian_code_ding);
            sp.Play();

            MessageDialog.Show(this, Strings.MsgPomodoroDone);

            if (_viewModel.IsTaskCompleted)
            {
                MessageDialog.Show(this, Strings.MsgTaskCompleted);
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Helper.WindowHelper.Move(new WindowInteropHelper(this).Handle);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Initialize(Properties.Settings.Default.Uri);
            AppDomain.CurrentDomain.UnhandledException += (o, a) => { MessageDialog.ShowError((Exception)a.ExceptionObject, "AppDomain unhandled exception"); Close(); };
            Application.Current.DispatcherUnhandledException += (o, a) => { MessageDialog.ShowError((Exception)a.Exception, "Dispatcher unhandled exception"); Close(); };
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
            _timer.Stop();
            toggle.IsChecked = false;
            _viewModel.SignOut();
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
                Cursor = Cursors.Wait;
                try
                {
                    await _viewModel.SignIn();
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
                await _viewModel.LoadTasks();
            }
        }

        private void OnSwitchChecked(object sender, RoutedEventArgs e)
        {
            _viewModel.IsSwitchChecked = true;
            _timer.Start();
        }

        private void OnSwitchUnhecked(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _viewModel.IsSwitchChecked = false;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            _viewModel.Dispose();

            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        private async void OnPomodorosClick(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            var dlg = new PomodoroDialog
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                DataContext = new PomodoroDialogViewModel()
            };
            
            try
            {
                await dlg.Initialize();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowError(ex, "MainWindow.OnPomodorosClick()");
            }

            dlg.ShowDialog();

            Cursor = Cursors.Arrow;
        }
    }
}