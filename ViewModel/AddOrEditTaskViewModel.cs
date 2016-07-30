using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AddOrEditTaskViewModel : INotifyPropertyChanged
    {
        private ICollection<Model.Project> _projects;
        private object _selectedItem;
        private bool _isProjectsEnabled;
        private string _taskName;
        private int _pomodoroCount = 6;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICollection<Model.Project> Projects
        {
            get { return _projects; }
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

        public string TaskName
        {
            get
            {
                return _taskName;
            }
            set
            {
                _taskName = value;
                OnPropertyChanged();
            }
        }

        public int PomodoroCount
        {
            get
            {
                return _pomodoroCount;
            }
            set
            {
                _pomodoroCount = value;
                OnPropertyChanged();
            }
        }

        public int MinPomodoroCount
        {
            get { return Controller.Instance.CurrentTask.PomodoroCount; }
        }

        public bool IsProjectsEnabled
        {
            get
            {
                return _isProjectsEnabled;
            }
            set
            {
                _isProjectsEnabled = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadProjects()
        {
            var projects = await Controller.Instance.UnitOfWork.ProjectsAsync.GetAllAsync();
            _projects = new List<Model.Project>(projects);
            OnPropertyChanged("Projects");
            IsProjectsEnabled = _projects.Count > 0;
        }

        public void InitializeValues()
        {
            PomodoroCount = Controller.Instance.CurrentTask.PomodoroCount;
            TaskName = Controller.Instance.CurrentTask.Name;
            SelectedItem = Controller.Instance.UnitOfWork.Projects.GetById(Controller.Instance.CurrentTask.ProjectId);
        }

        public async Task AddTask(string taskName)
        {
            var task = new Model.Task
            {
                TaskId = Guid.NewGuid(),
                UserId = Controller.Instance.User.UserId,
                TeamId = Controller.Instance.User.TeamId,
                Name = taskName,
                ProjectId = ((Model.Project)SelectedItem).ProjectId,
                PomodoroCount = PomodoroCount
            };

            await Controller.Instance.UnitOfWork.TasksAsync.AddAsync(task);
        }

        public async Task UpdateTask(string taskName)
        {
            Model.Task t = await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(Controller.Instance.CurrentTask.TaskId);
            t.Name = taskName;
            t.PomodoroCount = PomodoroCount;
            t.ProjectId = ((Model.Project)SelectedItem).ProjectId;

            Controller.Instance.CurrentTask = t;
            await Controller.Instance.UnitOfWork.SaveChangesAsync();
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