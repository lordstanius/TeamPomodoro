using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using ViewModel.Globalization;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private TimeSpan _timeRemaining;
        private bool _isGridInEnabled = false;
        private bool _isSignInEnabled = true;
        private bool _isSignOutEnabled = false;
        private bool _isEditTasksEnabled = false;
        private bool _isAdminVisible = false;
        private List<Model.Task> _tasks;
        private string _title = Strings.TxtTeamPomodoro;

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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsGridEnabled"));
                }
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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSignInEnabled"));
                }
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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSignOutEnabled"));
                }
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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsEditTasksEnabled"));
                }
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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsAdminVisible"));
                }
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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Tasks"));
                }
            }
        }

        internal void SetTimeRemaining(int durationInMin)
        {
            _timeRemaining = TimeSpan.FromMinutes(durationInMin);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("TimeRemaining"));
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
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                }
            }
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
            
            Title = string.Format("{0}: {1}", Strings.TxtTeamPomodoro, Controller.Instance.User.UserName);
            SetTimeRemaining(Controller.Instance.User.PomodoroDurationInMin);
        }

        public void Initialize(string uri)
        {
            Controller.Instance.Initialize(uri);
        }
    }
}
