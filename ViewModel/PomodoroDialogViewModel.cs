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
        private List<Model.User> _users;
        private List<Model.Team> _teams;
        private List<View.TaskView> _tasks;
        private object _selectedUserItem;
        private object _selectedTeamItem;
        private object _selectedTask;
        private object _selectedDate;
        private bool _isTeamsEnabled;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Model.User> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public List<Model.Team> Teams
        {
            get
            {
                return _teams;
            }
            set
            {
                _teams = value;
                OnPropertyChanged();
            }
        }

        public List<View.TaskView> Tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                _tasks = value;
                OnPropertyChanged();
            }
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
                    value == null ?
                    ((View.TaskView)SelectedTask).Task :
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
            Users = new List<Model.User>(await Controller.Instance.UnitOfWork.UsersAsync.GetAllAsync());
            Teams = new List<Model.Team>(await Controller.Instance.UnitOfWork.TeamsAsync.GetAllAsync());

            // pre-load tasks data to speed up filtering
            await Controller.Instance.UnitOfWork.TasksAsync.GetAllAsync();

            SelectedUserItem = Controller.Instance.User;
            SelectedTeamItem = Controller.Instance.User.TeamId.HasValue ?
                                  Controller.Instance.UnitOfWork.Teams.GetById(Controller.Instance.User.TeamId.Value) :
                                  null;

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
            var user = (Model.User)SelectedUserItem;
            var team = (Model.Team)SelectedTeamItem;

            if (team == null)
            {
                return;
            }

            var tasks = from task in Controller.Instance.UnitOfWork.Tasks.GetAll()
                        where task.UserId == user.UserId && task.TeamId == team.TeamId
                        select task;

            var items = new List<View.TaskView>(tasks.Count());

            // TODO: What about exception handling?
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

                items.Add(new View.TaskView(t));
            }

            Tasks = items;
        }
    }
}
