using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ViewModel.Globalization;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private TimeSpan _timeRemaining;
        private bool _isGridInEnabled = false;
        private bool _isSignInEnabled = true;
        private bool _isSignOutEnabled = false;
        private bool _isEditTasksEnabled = false;
        private bool _isAdminVisible = false;
        private bool _isTasksEnabled = false;
        private bool _isSwitchEnabled = false;
        private bool _isSwitchChecked = false;
        private List<Model.Task> _tasks;
        private object _selectedItem;
        private string _title = Strings.TxtTeamPomodoro;
        private string _pomodoroXofY;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsGridEnabled
        {
            get
            {
                return _isGridInEnabled;
            }
            set
            {
                _isGridInEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsSignInEnabled
        {
            get
            {
                return _isSignInEnabled;
            }
            set
            {
                _isSignInEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsSignOutEnabled
        {
            get
            {
                return _isSignOutEnabled;
            }
            set
            {
                _isSignOutEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditTasksEnabled
        {
            get
            {
                return _isEditTasksEnabled;
            }
            set
            {
                _isEditTasksEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsTasksEnabled
        {
            get
            {
                return _isTasksEnabled;
            }
            set
            {
                _isTasksEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdminVisible
        {
            get
            {
                return _isAdminVisible;
            }
            set
            {
                _isAdminVisible = value;
                OnPropertyChanged();
            }
        }

        public string TimeRemaining
        {
            get { return _timeRemaining.ToString("mm\\:ss"); }
        }

        public List<Model.Task> Tasks
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

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string PomodoroXofY
        {
            get
            {
                return _pomodoroXofY;
            }
            set
            {
                _pomodoroXofY = value;
                OnPropertyChanged();
            }
        }

        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                Controller.Instance.CurrentTask = (Model.Task)value;
                PomodoroXofY = Controller.Instance.PomodoroXofY;
                IsSwitchEnabled = true;
                SetTimeRemaining(Controller.Instance.User.PomodoroDurationInMin);
                OnPropertyChanged();
            }
        }

        public bool IsSwitchEnabled
        {
            get
            {
                return _isSwitchEnabled;
            }
            set
            {
                _isSwitchEnabled = value && !Controller.Instance.IsTaskCompleted;
                OnPropertyChanged();
            }
        }

        public bool IsSwitchChecked
        {
            get
            {
                return _isSwitchChecked;
            }
            set
            {
                _isSwitchChecked = value;

                if (value)
                {
                    SetTimeRemaining(Controller.Instance.User.PomodoroDurationInMin);
                    Controller.Instance.CreatePomodoro();
                    PomodoroXofY = Controller.Instance.PomodoroXofY;
                    IsTasksEnabled = false;
                }
                else
                {
                    Controller.Instance.CompletePomodoro(_timeRemaining);
                    IsTasksEnabled = true;

                    if (IsTaskCompleted)
                    {
                        SetTimeRemaining(Controller.Instance.User.PomodoroDurationInMin);
                        IsSwitchEnabled = false;
                    }
                }
            }
        }

        public bool IsTimeExpired
        {
            get { return _timeRemaining.TotalSeconds == 0.0; }
        }

        public bool IsTaskCompleted
        {
            get { return Controller.Instance.IsTaskCompleted; }
        }

        public bool ShouldShowWarning
        {
            get { return Controller.Instance.User.ShowWarningAfterPomodoroExpires; }
        }

        public void Dispose()
        {
            Controller.Instance.Dispose();
        }

        public async Task SignIn()
        {
            IsGridEnabled = true;
            IsAdminVisible = Controller.Instance.User.UserName.Equals("admin");
            IsSignInEnabled = false;
            IsSignOutEnabled = true;
            IsEditTasksEnabled = true;
            Tasks = new List<Model.Task>();

            foreach (var task in Controller.Instance.User.Tasks)
            {
                Tasks.Add(await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(task.TaskId));
            }

            IsTasksEnabled = Tasks.Count > 0;

            Title = string.Format("{0}: {1}", Strings.TxtTeamPomodoro, Controller.Instance.User.UserName);
            SetTimeRemaining(Controller.Instance.User.PomodoroDurationInMin);
        }

        public async Task GetTasks()
        {
            var tasks = new List<Model.Task>();
            foreach (var task in Controller.Instance.User.Tasks)
            {
                tasks.Add(await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(task.TaskId));
            }

            if (Controller.Instance.CurrentTask != null)
            {
                var task = tasks.FirstOrDefault(t => t.Equals(Controller.Instance.CurrentTask));
                if (task != null)
                {
                    Controller.Instance.CurrentTask = task;
                    SelectedItem = task;
                    IsSwitchEnabled = true;
                }
            }

            Tasks = tasks;
        }

        public void SignOut()
        {
            SetTimeRemaining(0);

            IsGridEnabled = false;
            Title = Strings.TxtTeamPomodoro;
            SelectedItem = null;
            IsSignInEnabled = true;
            IsSignOutEnabled = false;
            IsEditTasksEnabled = false;
            IsAdminVisible = false;
            IsTasksEnabled = false;
            Controller.Instance.User = null;
        }

        public void Initialize(string uri)
        {
            Controller.Instance.Initialize(uri);
        }

        public void OnTimer()
        {
            _timeRemaining -= TimeSpan.FromSeconds(1.0);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("TimeRemaining"));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SetTimeRemaining(int durationInMin)
        {
            _timeRemaining = TimeSpan.FromMinutes(durationInMin);
            OnPropertyChanged("TimeRemaining");
        }
    }
}