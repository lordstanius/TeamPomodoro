﻿using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using TeamPomodoro.UI;
using TeamPomodoro.Util;
using TeamPomodoro.Properties;
using TeamPomodoro.Globalization;
using DataAccess.Persistance;

namespace TeamPomodoro.Core
{
	internal sealed class Controller : IDisposable
	{
		internal UnitOfWork UnitOfWork { get; private set; }
		internal Model.User User { get; private set; }
		internal MainWindow Main { get; private set; }

		static Controller _Controller;
		static object Sync = new object();

		Timer _timer;
		TimeSpan _timeRemaining;
		Model.Pomodoro _currentPomodoro;
		Model.Task _currentTask;

		Controller(MainWindow main)
		{
			Main = main;
			UnitOfWork = new UnitOfWork(Settings.Default.Uri);

			_timer = new Timer { Interval = 1000 };
			_timer.Elapsed += OnTimerElapsed;
		}

		internal static void Create(MainWindow main)
		{
			lock (Sync)
			{
				if (_Controller == null)
					_Controller = new Controller(main);
			}
		}

		internal static Controller Instance
		{
			get
			{
				if (_Controller == null)
					throw new ArgumentNullException("Controller is not initialized.");

				return _Controller;
			}
		}

		internal async void ShowEditProjets()
		{
			var editHelper = new EditHelper(EditHelper.EditType.Project);
			await editHelper.ShowEditDialog();
		}

		internal async void ShowEditTeams()
		{
			var editHelper = new EditHelper(EditHelper.EditType.Team);
			await editHelper.ShowEditDialog();
		}

		internal void SignOut()
		{
			Main.cbTasks.IsEnabled = false;
			Main.cbTasks.SelectedItem = null;
			Main.lPomodoro.Visibility = Visibility.Hidden;
			Main.grid.IsEnabled = false;
			Main.toggle.IsChecked = false;
			Main.Title = Strings.TxtTeamPomodoro;

			var mniSignIn = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "mniSignIn");
			var mniSignOut = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "mniSignOut");
			var mniEditTasks = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "mniEditTasks");

			mniSignIn.IsEnabled = true;
			mniSignOut.IsEnabled = false;
			mniEditTasks.IsEnabled = false;

			User = null;
		}

		internal void SignIn()
		{
			if (ShowSignIn())
			{
				Main.grid.IsEnabled = true;
				var mniSignIn = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "mniSignIn");
				var mniSignOut = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "mniSignOut");
				var mniEditTasks = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "mniEditTasks");

				mniSignIn.IsEnabled = false;
				mniSignOut.IsEnabled = true;
				mniEditTasks.IsEnabled = true;

				Main.cbTasks.ItemsSource = User.Tasks;
				Main.cbTasks.IsEnabled = Main.cbTasks.Items.Count > 0;
				Main.Title = string.Format("{0}: {1}", Strings.TxtTeamPomodoro, User);
			}
		}

		internal bool ShowSignIn()
		{
			try
			{
				Main.Cursor = Cursors.Wait;
				var signIn = new SignIn
				{
					Owner = Main,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};

				signIn.txtUserName.Focus();

				return (bool)signIn.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageDialog.ShowError(ex.Message);
				return false;
			}
			finally
			{
				Main.Cursor = Cursors.Arrow;
			}
		}

		internal async Task<bool> ShowUserDetails(string userName = null)
		{
			try
			{
				Main.Cursor = Cursors.Arrow;
				var teams = await UnitOfWork.TeamsAsync.GetAllAsync();

				var userDetails = new UserDetails
				{
					Owner = Main,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};

				userDetails.cbTeams.IsEnabled = teams.Count() > 0;
				userDetails.cbTeams.ItemsSource = teams;
				if (User != null)
				{
					if (User.TeamId != null)
						userDetails.cbTeams.SelectedItem = UnitOfWork.Teams.Get((Guid)User.TeamId);

					userDetails.chkShowWarning.IsChecked = User.ShowWarningAfterPomodoroExpires;
					userDetails.numUpDown.Value = User.PomodoroDurationInMin;
				}

				userDetails.txtUserName.Text = userName ?? User.UserName;

				if (userDetails.ShowDialog() == true)
					await UnitOfWork.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				MessageDialog.ShowError(ex.Message);
				return false;
			}
			finally
			{
				Main.Cursor = Cursors.Arrow;
			}
		}

		internal async Task<bool> GetUser(string userName, string password)
		{
			if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
				return false;

			foreach (var user in await UnitOfWork.UsersAsync.GetAllAsync())
				if (user.UserName.Equals(userName))
				{
					User = await UnitOfWork.UsersAsync.GetAsync(user.UserId);
					return true;
				}

			// TODO: implement password validation
			return false;
		}

		internal async Task UpdateUser(string userName, string password, int durationInMin, bool showWarning, Guid? teamId)
		{
			bool newUser = User == null;
			if (newUser) // add new user
				User = new Model.User { UserId = Guid.NewGuid() };
			else // load user for updating
				User = await UnitOfWork.UsersAsync.GetAsync(User.UserId);

			// update date
			User.UserName = userName;
			User.Password = password;
			User.ShowWarningAfterPomodoroExpires = showWarning;
			User.PomodoroDurationInMin = durationInMin;
			User.TeamId = teamId;

			if (newUser)
				await UnitOfWork.UsersAsync.AddAsync(User);

			await UnitOfWork.SaveChangesAsync();
		}

		internal async void ShowEditTasks()
		{
			var editHelper = new EditHelper(EditHelper.EditType.Task);
			await editHelper.ShowEditDialog();
			Main.cbTasks.ItemsSource = (await UnitOfWork.UsersAsync.GetAsync(User.UserId)).Tasks;
			Main.cbTasks.IsEnabled = Main.cbTasks.Items.Count > 0;
		}

		internal bool? ValidateTask(AddOrEditTask dialog)
		{
			if (dialog.cbProjects.SelectedItem == null && !dialog.IsOfEditType)
			{
				MessageDialog.Show(Strings.MsgAddProjects);
				return null;
			}

			return true;
		}

		internal async void ShowPomodoros()
		{
			Main.Cursor = Cursors.Wait;
			try
			{
				var dlg = new PomodoroDialog
				{
					Owner = Main,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};

				dlg.users.ItemsSource = await UnitOfWork.UsersAsync.GetAllAsync();
				dlg.teams.ItemsSource = await UnitOfWork.TeamsAsync.GetAllAsync();

				// load pomodoro data to speed up filtering
				await UnitOfWork.PomodoroesAsync.GetAllAsync();

				dlg.users.SelectedItem = User;
				dlg.teams.IsEnabled = dlg.teams.Items.Count > 0;
				if (User.TeamId.HasValue)
				{
					dlg.teams.SelectedItem = UnitOfWork.Teams.Get(User.TeamId.Value);
					dlg.placeholder.Visibility = Visibility.Collapsed;
				}
				else
				{
					dlg.teams.SelectedItem = null;
					dlg.placeholder.Visibility = Visibility.Visible;
				}

				dlg.users.SelectionChanged += (o, e) => UpdateList(dlg);
				dlg.teams.SelectionChanged += (o, e) => UpdateList(dlg);
				dlg.date.SelectedDateChanged += (o, e) => UpdateList(dlg);

				UpdateList(dlg);

				dlg.ShowDialog();
			}
			catch (Exception ex)
			{
				MessageDialog.ShowError(ex.ToString());
			}

			Main.Cursor = Cursors.Arrow;
		}

		async void UpdateList(PomodoroDialog dlg)
		{

		}

		internal void StartPomodoro()
		{
			SetTimeRemaining();

			if (IsTaskCompleted)
			{
				MessageDialog.Show(Strings.TxtTaskCompletedWarning);
				return;
			}

			Main.cbTasks.IsEnabled = false;
			var currentTask = (Model.Task)Main.cbTasks.SelectedItem;
			_currentPomodoro = new Model.Pomodoro
			{
				PomodoroId = Guid.NewGuid(),
				StartTime = DateTime.Now,
				TaskId = currentTask.TaskId,
			};

			_currentTask.Pomodoroes.Add(_currentPomodoro);
			SetPomodorosXofY();

			_timer.Start();
		}

		internal void StopPomodoro()
		{
			_timer.Stop();
			Main.cbTasks.IsEnabled = true;
			_currentPomodoro.IsSuccessfull = _timeRemaining.TotalSeconds < 10.0;
			_currentPomodoro.DurationInMin = (int)TimeSpan.FromMinutes(User.PomodoroDurationInMin).Subtract(_timeRemaining).TotalMinutes;

			UnitOfWork.PomodoroesAsync.AddAsync(_currentPomodoro);

			Main.toggle.IsEnabled = !IsTaskCompleted;
			if (IsTaskCompleted)
				MessageDialog.Show(Strings.TxtTaskCompletedInfo);
		}

		internal async void UpdateGuiOnTaskChanged()
		{
			if (Main.cbTasks.SelectedItem == null)
				return;

			var task = (Model.Task)Main.cbTasks.SelectedItem;
			_currentTask = await UnitOfWork.TasksAsync.GetAsync(task.TaskId);

			SetPomodorosXofY();
			SetTimeRemaining();

			Main.lPomodoro.Visibility = Visibility.Visible;

			Main.toggle.IsEnabled = !IsTaskCompleted;
			if (IsTaskCompleted)
				MessageDialog.Show(Strings.TxtTaskCompletedWarning);
		}

		bool IsTaskCompleted
		{
			get { return _currentTask.PomodoroCount == _currentTask.Pomodoroes.Count; }
		}

		void SetPomodorosXofY()
		{
			Main.lPomodoro.Text = string.Format(Strings.TxtPomodoroXofY,
				_currentTask.Pomodoroes.Count, _currentTask.PomodoroCount);
		}

		void SetTimeRemaining()
		{
			_timeRemaining = TimeSpan.FromMinutes(User.PomodoroDurationInMin);
			Main.lCounter.Text = _timeRemaining.ToString("mm\\:ss");
		}

		private void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			_timeRemaining -= TimeSpan.FromSeconds(1.0);
			if (_timeRemaining.TotalSeconds == 0)
				StopPomodoro();

			Main.Dispatcher.Invoke(() => Main.lCounter.Text = _timeRemaining.ToString("mm\\:ss"));
		}

		public void Dispose()
		{
			if (_timer != null)
				_timer.Dispose();

			if (UnitOfWork != null)
				UnitOfWork.Dispose();
		}
	}
}
