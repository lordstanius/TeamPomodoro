using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Timers;
using System.Threading.Tasks;
using ViewModel.Globalization;
using System.Runtime.CompilerServices;

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
        private List<Model.Task> _tasks;
        private object _selectedItem;
        private string _title = Strings.TxtTeamPomodoro;
        private Timer _timer;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            _timer = new Timer { Interval = 1000 };
            _timer.Elapsed += OnTimerElapsed;
        }

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

        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _timeRemaining -= TimeSpan.FromSeconds(1.0);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("TimeElapsed"));

            if (_timeRemaining.TotalSeconds == 0)
            {
                OnPomodoroCompleted();
            }
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

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

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
            Tasks = new List<Model.Task>();

            foreach (var task in Controller.Instance.User.Tasks)
            {
                Tasks.Add(await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(task.TaskId));
            }
        }

        public void SignOut()
        {
            _timer.Stop();
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

        internal void SetTimeRemaining(int durationInMin)
        {
            _timeRemaining = TimeSpan.FromMinutes(durationInMin);
            OnPropertyChanged("TimeRemaining");
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
