using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using DataAccess.Persistence;
using NLog;
using NLog.Config;
using NLog.Targets;
using ViewModel.Globalization;
using ViewModel.Util;

namespace ViewModel
{
    internal sealed class Controller : IDisposable
    {
        private static Controller _controller = new Controller();
        private Model.Pomodoro _currentPomodoro;

        private Controller()
        {
            // configure log
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget
            {
                FileName = Path.Combine(Path.GetTempPath(), "TeamPomodoro\\TeamPomodoro.log"),
                Layout = @"[${date:format=HH\:mm\:ss}] ${logger} ${message} ${exception}"
            };

            config.AddTarget("file", fileTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = config;
        }

        public static Controller Instance
        {
            get { return _controller; }
        }

        public UnitOfWork UnitOfWork { get; private set; }

        public Model.User User { get; set; }

        public Model.Task CurrentTask { get; set; }

        public Model.Team CurrentTeam { get; set; }

        public Model.Project CurrentProject { get; set; }

        public Model.Pomodoro CurrentPomodoro
        {
            get { return _currentPomodoro; }
        }

        public NetworkCredential UserCredential { get; private set; }

        public bool IsTaskCompleted
        {
            get
            {
                if (CurrentTask == null || CurrentTask.Pomodoroes == null)
                {
                    return false;
                }

                return CurrentTask.PomodoroCount == CurrentTask.Pomodoroes.Count;
            }
        }

        public string PomodoroXofY
        {
            get
            {
                if (CurrentTask == null || CurrentTask.Pomodoroes == null)
                {
                    return string.Empty;
                }

                return string.Format(
                                 CultureInfo.CurrentCulture,
                                 Strings.TxtPomodoroXofY,
                                 CurrentTask.Pomodoroes.Count,
                                 CurrentTask.PomodoroCount);
            }
        }

        public void Initialize(string uri)
        {
            UnitOfWork = new UnitOfWork(uri);
        }

        /// <summary>
        /// Get user and return dialog result to calling dialog.
        /// </summary>
        public async Task<bool?> LoadUser(string userName, SecureString password)
        {
            foreach (var user in await UnitOfWork.UsersAsync.GetAllAsync())
            {
                if (user.UserName.Equals(userName))
                {
                    if (!password.GetHashString().Equals(user.Password))
                    {
                        return false;
                    }

                    User = await UnitOfWork.UsersAsync.GetAsync(user.UserId);
                    UserCredential = new NetworkCredential(userName, password);
                    return true;
                }
            }

            return null;
        }

        public async Task UpdateUser(string userName, string password, int durationInMin, bool showWarning, Guid? teamId)
        {
            bool newUser = User == null;
            if (newUser)
            {
                // add new user
                User = new Model.User { UserId = Guid.NewGuid() };
            }
            else
            {
                // load user for updating
                User = await UnitOfWork.UsersAsync.GetAsync(User.UserId);
            }

            UserCredential = new NetworkCredential(userName, password);

            User.UserName = userName;
            User.Password = password.GetHashString();
            User.ShowWarningAfterPomodoroExpires = showWarning;
            User.PomodoroDurationInMin = durationInMin;
            User.TeamId = teamId;

            if (newUser)
            {
                await UnitOfWork.UsersAsync.AddAsync(User);
            }
        }

        public void CompletePomodoro(TimeSpan timeRemaining)
        {
            CurrentPomodoro.IsSuccessfull = timeRemaining.TotalSeconds < 10.0;
            CurrentPomodoro.DurationInMin = (int)TimeSpan.FromMinutes(User.PomodoroDurationInMin).Subtract(timeRemaining).TotalMinutes;

            UnitOfWork.PomodoroesAsync.AddAsync(CurrentPomodoro);
        }

        public void CreatePomodoro()
        {
            _currentPomodoro = new Model.Pomodoro
            {
                PomodoroId = Guid.NewGuid(),
                StartTime = DateTime.Now,
                TaskId = CurrentTask.TaskId,
            };

            CurrentTask.Pomodoroes.Add(_currentPomodoro);
        }

        public bool ValidateUserName(string userName)
        {
            return !string.IsNullOrEmpty(userName);
        }

        public bool ValidatePassword(string password)
        {
            return password.Length > 2;
        }

        public void Dispose()
        {
            if (UnitOfWork != null)
            {
                UnitOfWork.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
