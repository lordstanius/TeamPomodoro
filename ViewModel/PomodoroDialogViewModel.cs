using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModel
{
    public class PomodoroDialogViewModel : INotifyPropertyChanged
    {
        private ICollection<Model.User> _users;
        private ICollection<Model.Team> _teams;
        private ICollection<View.TaskView> _taskViews = new List<View.TaskView>();
        private object _selectedUserItem;
        private object _selectedTeamItem;
        private object _selectedTask;
        private object _selectedDate;
        private bool _isTeamsEnabled;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler UpdateStarted;
        public event EventHandler UpdateFinished;
        public event EventHandler<System.IO.ErrorEventArgs> ExceptionThrown;

        public ICollection<Model.User> Users
        {
            get { return _users; }
        }

        public ICollection<Model.Team> Teams
        {
            get { return _teams; }
        }

        public ICollection<View.TaskView> Tasks
        {
            get { return _taskViews; }
        }

        public object SelectedUserItem
        {
            get
            {
                return _selectedUserItem;
            }
            set
            {
                _selectedUserItem = value;
                UpdateTasks();
                OnPropertyChanged();
            }
        }

        public object SelectedTeamItem
        {
            get
            {
                return _selectedTeamItem;
            }
            set
            {
                _selectedTeamItem = value;
                UpdateTasks();
                OnPropertyChanged();
            }
        }

        public object SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                Controller.Instance.CurrentTask =
                    value != null ?
                    ((View.TaskView)value).Task :
                    null;

                OnPropertyChanged();
            }
        }

        public bool IsTeamsEnabled
        {
            get
            {
                return _isTeamsEnabled;
            }
            set
            {
                _isTeamsEnabled = value;
                OnPropertyChanged();
            }
        }

        public object SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                UpdateTasks();
                OnPropertyChanged();
            }
        }

        public async Task Initialize()
        {
            _users = new List<Model.User>(await Controller.Instance.UnitOfWork.UsersAsync.GetAllAsync());
            _teams = new List<Model.Team>(await Controller.Instance.UnitOfWork.TeamsAsync.GetAllAsync());

            OnPropertyChanged("Users");
            OnPropertyChanged("Teams");

            // pre-load tasks data to speed up filtering
            await Controller.Instance.UnitOfWork.TasksAsync.GetAllAsync();

            _selectedUserItem = Controller.Instance.User;
            _selectedTeamItem = Controller.Instance.User.TeamId.HasValue ?
                                  Controller.Instance.UnitOfWork.Teams.GetById(Controller.Instance.User.TeamId.Value) :
                                  null;
            OnPropertyChanged("SelectedUserItem");
            OnPropertyChanged("SelectedTeamItem");

            IsTeamsEnabled = Teams.Count > 0;

            UpdateTasks();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void UpdateTasks()
        {
            if (UpdateStarted != null)
            {
                UpdateStarted(this, EventArgs.Empty);
            }

            var user = (Model.User)SelectedUserItem;
            var team = (Model.Team)SelectedTeamItem;

            if (team == null)
            {
                return;
            }

            var tasks = from task in Controller.Instance.UnitOfWork.Tasks.GetAll()
                        where task.UserId == user.UserId && task.TeamId == team.TeamId
                        select task;

            _taskViews.Clear();

            try
            {
                foreach (var task in tasks)
                {
                    var t = await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(task.TaskId);

                    if (t.Pomodoroes == null || t.Pomodoroes.Count == 0)
                    {
                        continue;
                    }

                    if (SelectedDate != null &&
                        t.Pomodoroes.First().StartTime.Value.Date != (DateTime)SelectedDate)
                    {
                        continue;
                    }

                    _taskViews.Add(new View.TaskView(t));
                }
            }
            catch (Exception ex)
            {
                if (ExceptionThrown != null)
                {
                    ExceptionThrown(this, new System.IO.ErrorEventArgs(ex));
                }
            }

            OnPropertyChanged("Tasks");
            if (UpdateFinished != null)
            {
                UpdateFinished(this, EventArgs.Empty);
            }
        }
    }
}
