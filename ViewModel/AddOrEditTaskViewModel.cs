using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class AddOrEditTaskViewModel : INotifyPropertyChanged
    {
        private List<Model.Project> _projects;
        private object _selectedItem;
        private bool _isProjectsEnabled;
        private string _taskName;
        private int _pomodoroCount = 6;

        public List<Model.Project> Projects
        {
            get
            {
                return _projects;
            }
            set
            {
                _projects = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task GetProjects()
        {
            var projects = await Controller.Instance.UnitOfWork.ProjectsAsync.GetAllAsync();
            Projects = new List<Model.Project>(projects);
            IsProjectsEnabled = Projects.Count > 0;
        }

        public int GetMinPomodoroCount()
        {
            return Controller.Instance.CurrentTask.PomodoroCount;
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
    }
}