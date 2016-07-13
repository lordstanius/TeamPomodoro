using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using TeamPomodoro.UI;
using TeamPomodoro.Properties;
using TeamPomodoro.Globalization;
using DataAccess.Persistance;
using Model;

namespace TeamPomodoro.Core
{
	internal sealed class Controller
	{
		internal UnitOfWork UnitOfWork { get; private set; }

		internal MainWindow MainWindow { get; private set; }

		static Controller _Controller;
		static object Sync = new object();

		User _user;

		Controller(MainWindow main)
		{
			MainWindow = main;
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

		internal async void ShowSignIn()
		{
			try
			{
				MainWindow.Cursor = Cursors.Wait;
				IEnumerable<User> users = await UnitOfWork.UsersAsync.GetAllAsync();

				var signIn = new SignIn
				{
					Owner = MainWindow,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};

				if (signIn.ShowDialog() == false)
					MainWindow.Close();
			}
			catch (Exception ex)
			{
				MessageDialog.ShowError(ex.Message, Strings.MsgFailedToRespond);
			}

			MainWindow.Cursor = Cursors.Arrow;
		}

		/// <summary>
		/// Returns null if action is not completed to indicate that Sign in dialog should stay opened.
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		internal async Task<bool?> ShowUserDetails(string userName)
		{
			try
			{
				MainWindow.Cursor = Cursors.Arrow;
				var teams = await UnitOfWork.TeamsAsync.GetAllAsync();

				var userDetails = new UserDetails
				{
					Owner = MainWindow,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};

				userDetails.cbTeam.IsEnabled = teams.Count() > 0;
				userDetails.txtUserName.Text = userName;
				userDetails.txtPassword.Focus();

				bool? result = true;
				if (userDetails.ShowDialog() == false)
					result = null;

				return result;
			}
			catch (Exception ex)
			{
				MessageDialog.ShowError(ex.Message, Strings.MsgFailedToRespond);
				return null;
			}
			finally
			{
				MainWindow.Cursor = Cursors.Arrow;
			}
		}

		internal bool? ValidateUser(string userName, string password)
		{
			if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
				return null;

			// TODO: implement password validation
			foreach (User user in UnitOfWork.Users.GetAll())
			{
				if (user.UserName.Equals(userName))
				{
					_user = user;
					return true;
				}
			}

			return null;
		}

		internal async Task<bool?> AddUser(string userName, string password, int durationInMin, bool showWarning, Guid? teamId)
		{
			foreach (User user in UnitOfWork.Users.GetAll())
				if (user.UserName.Equals(userName))
					return null;

			// create new user
			await UnitOfWork.UsersAsync.AddAsync(new User
			{
				UserId = Guid.NewGuid(),
				UserName = userName,
				Password = password,
				ShowWarningAfterPomodoroExpires = showWarning,
				PomodoroDurationInMin = durationInMin,
				TeamId = teamId
			});

			return true;
		}
	}
}
