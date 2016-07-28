﻿using System;
using System.IO;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public static Controller Instance { get { return _controller; } }

        public UnitOfWork UnitOfWork { get; private set; }

        public Model.User User { get; set; }

        public Model.Task CurrentTask { get; set; }

        public Model.Team CurrentTeam { get; set; }

        public Model.Project CurrentProject { get; set; }

        public Model.Pomodoro CurrentPomodoro
        {
            get { return _currentPomodoro; }
            set { _currentPomodoro = value; }
        }

        public NetworkCredential UserCredential { get; private set; }

        public bool IsTaskCompleted
        {
            get { return CurrentTask.PomodoroCount == CurrentTask.Pomodoroes.Count; }
        }

        public void Initialize(string uri)
        {
            UnitOfWork = new UnitOfWork(uri);
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

        public string PomodoroXofY
        {
            get
            {
                return string.Format(
                                 Strings.TxtPomodoroXofY,
                                 CurrentTask != null ? CurrentTask.Pomodoroes.Count : 0,
                                 CurrentTask.PomodoroCount);
            }
        }

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
    }
}