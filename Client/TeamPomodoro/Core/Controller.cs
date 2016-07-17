using System;
using System.Linq;
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
	internal sealed class Controller
	{
		internal UnitOfWork UnitOfWork { get; private set; }

		internal MainWindow Main { get; private set; }

		static Controller _Controller;
		static object Sync = new object();

		public Model.User User { get; private set; }

		Controller(MainWindow main)
		{
			Main = main;
			UnitOfWork = new UnitOfWork(Settings.Default.Uri);
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

		internal void StartPomodoro()
		{

		}

		internal void StopPomodoro()
		{

		}
	}
}
