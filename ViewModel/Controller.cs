using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Timers;
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
        private Timer _timer;
        private TimeSpan _timeRemaining;
        private Model.Pomodoro _currentPomodoro;
        private Model.Task _currentTask;

        private Controller()
        {
            _timer = new Timer { Interval = 1000 };
            _timer.Elapsed += OnTimerElapsed;

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

        public static Controller Instance { get { return _controller; } }

        public UnitOfWork UnitOfWork { get; private set; }

        public Model.User User { get; private set; }

        public NetworkCredential UserCredential { get; private set; }

        //public MainWindow Main { get; set; }

        private bool IsTaskCompleted
        {
            get { return _currentTask.PomodoroCount == _currentTask.Pomodoroes.Count; }
        }

        public void Initialize(string uri)
        {
            UnitOfWork = new UnitOfWork(uri);
        }

        public void SignOut()
        {
            //_timer.Stop();
            //Main.counter.Text = "00:00";
            //Main.tasks.IsEnabled = false;
            //Main.tasks.SelectedItem = null;
            //Main.pomodoro.Visibility = Visibility.Hidden;
            //Main.grid.IsEnabled = false;
            //Main.toggle.IsChecked = false;
            //Main.Title = Strings.TxtTeamPomodoro;

            //var signIn = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "miSignIn");
            //var signOut = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "miSignOut");
            //var editTasks = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "miEditTasks");
            //var admin = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "miAdmin");

            //admin.Visibility = Visibility.Hidden;
            //signIn.IsEnabled = true;
            //signOut.IsEnabled = false;
            //editTasks.IsEnabled = false;

            //User = null;
        }

        public async void SignIn()
        {
            //if (ShowSignIn())
            //{
            //    Main.grid.IsEnabled = true;
            //    var signIn = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "miSignIn");
            //    var signOut = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "miSignOut");
            //    var editTasks = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "miEditTasks");
            //    var admin = (MenuItem)LogicalTreeHelper.FindLogicalNode(Main.menu, "miAdmin");

            //    admin.Visibility = User.UserName.Equals("admin") ? Visibility.Visible : Visibility.Collapsed;
            //    signIn.IsEnabled = false;
            //    signOut.IsEnabled = true;
            //    editTasks.IsEnabled = true;

            //    Main.tasks.Items.Clear();
            //    Main.Cursor = Cursors.Wait;
            //    try
            //    {
            //        foreach (var task in User.Tasks)
            //        {
            //            Main.tasks.Items.Add(await UnitOfWork.TasksAsync.GetAsync(task.TaskId));
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageDialog.ShowError(ex, "Controller.SignIn() failed to get tasks for user");
            //    }

            //    Main.Cursor = Cursors.Arrow;
            //    Main.tasks.IsEnabled = Main.tasks.Items.Count > 0;
            //    Main.Title = string.Format("{0}: {1}", Strings.TxtTeamPomodoro, User);
            //}
        }

        public bool ShowSignIn()
        {
            throw new NotImplementedException();
            //try
            //{
            //    Main.Cursor = Cursors.Wait;
            //    var signIn = new SignIn
            //    {
            //        Owner = Main,
            //        WindowStartupLocation = WindowStartupLocation.CenterOwner
            //    };

            //    signIn.userName.Focus();

            //    return (bool)signIn.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    MessageDialog.ShowError(ex, "Controller.ShowSignIn()");
            //    return false;
            //}
            //finally
            //{
            //    Main.Cursor = Cursors.Arrow;
            //}
        }

        /// <summary>
        /// Get user and return dialog result to calling dialog.
        /// </summary>
        public async Task<bool?> GetUser(string userName, SecureString password)
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

        public async void ShowEditTasks()
        {
            throw new NotImplementedException();
            //int i = Main.tasks.SelectedIndex;
            //var editHelper = new EditHelper(EditHelper.EditType.Task);
            //await editHelper.ShowEditDialog();
            //Main.tasks.SelectedIndex = i;
        }

        //public bool? ValidateTask(AddOrEditTask dialog)
        //{
        //    if (dialog.projects.SelectedItem == null && !dialog.IsOfEditType)
        //    {
        //        MessageDialog.Show(Strings.MsgAddProjects);
        //        return null;
        //    }

        //    return true;
        //}

        public async void ShowPomodoros()
        {
            //Main.Cursor = Cursors.Wait;
            //var dlg = new PomodoroDialog
            //{
            //    Owner = Main,
            //    WindowStartupLocation = WindowStartupLocation.CenterOwner
            //};

            //try
            //{
            //    dlg.users.ItemsSource = await UnitOfWork.UsersAsync.GetAllAsync();
            //    dlg.teams.ItemsSource = await UnitOfWork.TeamsAsync.GetAllAsync();

            //    // load tasks data to speed up filtering
            //    await UnitOfWork.TasksAsync.GetAllAsync();
            //}
            //catch (Exception ex)
            //{
            //    MessageDialog.ShowError(ex, "Controller.ShowPomodoroes()");
            //}

            //dlg.users.SelectedItem = User;
            //dlg.teams.IsEnabled = dlg.teams.Items.Count > 0;
            //dlg.teams.SelectedItem = User.TeamId.HasValue ? UnitOfWork.Teams.GetById(User.TeamId.Value) : null;

            //dlg.users.SelectionChanged += (o, e) => UpdateTaskList(dlg);
            //dlg.teams.SelectionChanged += (o, e) => UpdateTaskList(dlg);
            //dlg.date.SelectedDateChanged += (o, e) => UpdateTaskList(dlg);

            //UpdateTaskList(dlg);
            //dlg.ShowDialog();

            //Main.Cursor = Cursors.Arrow;
        }

        //public void ShowPomodoroDetails(PomodoroDialog dlg)
        //{
        //var details = new PomodoroDetails
        //{
        //    Owner = dlg,
        //    WindowStartupLocation = WindowStartupLocation.CenterOwner
        //};

        //var task = ((UI.View.TaskView)dlg.list.SelectedItem).Task;

        //foreach (var item in UnitOfWork.Tasks.GetAll())
        //{
        //    details.tasks.Items.Add(item);
        //}

        //details.tasks.SelectedItem = task;

        //UpdatePomodoroList(details);
        //details.ShowDialog();
        //}

        //public async void UpdatePomodoroList(PomodoroDetails details)
        //{
        //if (details.tasks.SelectedItem == null)
        //{
        //    return;
        //}

        //Model.Task task = (Model.Task)details.tasks.SelectedItem;
        //details.Cursor = Cursors.Wait;
        //try
        //{
        //    if (task.Pomodoroes == null)
        //    {
        //        task = await UnitOfWork.TasksAsync.GetAsync(task.TaskId);
        //        details.tasks.Items[details.tasks.SelectedIndex] = task;
        //        details.tasks.SelectedItem = task;
        //    }

        //    details.Cursor = Cursors.Arrow;
        //}
        //catch (Exception ex)
        //{
        //    MessageDialog.ShowError(ex, "Controller.UpdatePomodoroList()");
        //    return;
        //}

        //details.list.Items.Clear();
        //int i = 0;
        //foreach (var pomodoro in task.Pomodoroes)
        //{
        //    details.list.Items.Add(new
        //    {
        //        No = ++i,
        //        Date = pomodoro.StartTime.Value.Date,
        //        Start = pomodoro.StartTime.Value.TimeOfDay.ToString("hh\\:mm\\:ss"),
        //        Duration = TimeSpan.FromMinutes(pomodoro.DurationInMin).ToString("mm\\:hh"),
        //        IsSuccessful = pomodoro.IsSuccessfull == true ? Strings.TxtYes : Strings.TxtNo
        //    });
        //}
        //}

        //public void StartPomodoro()
        //{
        //    SetTimeRemaining();

        //    if (IsTaskCompleted)
        //    {
        //        MessageDialog.Show(Strings.TxtTaskCompletedWarning);
        //        return;
        //    }

        //    Main.tasks.IsEnabled = false;
        //    var currentTask = (Model.Task)Main.tasks.SelectedItem;
        //    _currentPomodoro = new Model.Pomodoro
        //    {
        //        PomodoroId = Guid.NewGuid(),
        //        StartTime = DateTime.Now,
        //        TaskId = currentTask.TaskId,
        //    };

        //    _currentTask.Pomodoroes.Add(_currentPomodoro);
        //    SetPomodorosXofY();

        //    _timer.Start();
        //}

        //public void StopPomodoro()
        //{
        //    _timer.Stop();
        //    Main.tasks.IsEnabled = true;
        //    _currentPomodoro.IsSuccessfull = _timeRemaining.TotalSeconds < 10.0;
        //    _currentPomodoro.DurationInMin = (int)TimeSpan.FromMinutes(User.PomodoroDurationInMin).Subtract(_timeRemaining).TotalMinutes;

        //    UnitOfWork.PomodoroesAsync.AddAsync(_currentPomodoro);

        //    Main.toggle.IsEnabled = !IsTaskCompleted;
        //}

        //public void UpdateGuiOnTaskChanged()
        //{
        //    if (Main.tasks.SelectedItem == null)
        //    {
        //        return;
        //    }

        //    _currentTask = (Model.Task)Main.tasks.SelectedItem;

        //    SetPomodorosXofY();
        //    SetTimeRemaining();

        //    Main.pomodoro.Visibility = Visibility.Visible;

        //    Main.toggle.IsEnabled = !IsTaskCompleted;
        //    if (IsTaskCompleted)
        //    {
        //        MessageDialog.Show(Strings.TxtTaskCompletedWarning);
        //    }
        //}

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
            if (_timer != null)
            {
                _timer.Dispose();
            }

            if (UnitOfWork != null)
            {
                UnitOfWork.Dispose();
            }
        }

        //private async void UpdateTaskList(PomodoroDialog dlg)
        //{
        //    var user = (Model.User)dlg.users.SelectedItem;
        //    var team = (Model.Team)dlg.teams.SelectedItem;

        //    dlg.list.Items.Clear();

        //    if (team == null)
        //    {
        //        return;
        //    }

        //    var tasks = from task in UnitOfWork.Tasks.GetAll()
        //                where task.UserId == user.UserId && task.TeamId == team.TeamId
        //                select task;

        //    try
        //    {
        //        foreach (var task in tasks)
        //        {
        //            var t = await UnitOfWork.TasksAsync.GetAsync(task.TaskId);

        //            if (t.Pomodoroes == null || t.Pomodoroes.Count == 0)
        //            {
        //                continue;
        //            }

        //            if (dlg.date.SelectedDate != null &&
        //                t.Pomodoroes.First().StartTime.Value.Date != dlg.date.SelectedDate)
        //            {
        //                continue;
        //            }

        //            dlg.list.Items.Add(new UI.View.TaskView(t));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageDialog.ShowError(ex, "Controller.UpdateList()");
        //    }
        //}

        private void SetPomodorosXofY()
        {
            //Main.pomodoro.Text = string.Format(
            //                        Strings.TxtPomodoroXofY,
            //                        _currentTask.Pomodoroes != null ? _currentTask.Pomodoroes.Count : 0, 
            //                        _currentTask.PomodoroCount);
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //_timeRemaining -= TimeSpan.FromSeconds(1.0);
            //Main.Dispatcher.Invoke(() => Main.counter.Text = _timeRemaining.ToString("mm\\:ss"));
            //if (_timeRemaining.TotalSeconds == 0)
            //{
            //    Main.Dispatcher.Invoke(() => OnPomodoroCompleted());
            //}
        }

        private void OnPomodoroCompleted()
        {
            //StopPomodoro();

            //if (!User.ShowWarningAfterPomodoroExpires)
            //{
            //    return;
            //}

            //var sp = new SoundPlayer(Resources.martian_code_ding);
            //sp.Play();

            //MessageDialog.Show(Strings.MsgPomodoroDone);
            //if (IsTaskCompleted)
            //{
            //    MessageDialog.Show(Strings.TxtTaskCompletedInfo);
            //}

            //Main.toggle.IsChecked = false;
            //SetTimeRemaining();
        }
    }
}
