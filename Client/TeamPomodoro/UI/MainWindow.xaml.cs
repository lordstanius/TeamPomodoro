using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Controls;
using TeamPomodoro.Core;
using TeamPomodoro.Globalization;

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

		private void OnStartTimerClick(object sender, RoutedEventArgs e)
		{

		}

		private void OnStopTimerClick(object sender, RoutedEventArgs e)
		{

		}

		private void OnEditTasksClick(object sender, RoutedEventArgs e)
		{
			Controller.Instance.ShowEditTasks();
		}

		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cbTasks.SelectedIndex == -1)
			{
				btnStart.IsEnabled = false;
				btnStop.IsEnabled = false;
			}
			else
			{
				btnStart.IsEnabled = true;
			}
		}
	}
}
