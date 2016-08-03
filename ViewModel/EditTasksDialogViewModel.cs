using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModel
{
    public class EditTasksDialogViewModel : INotifyPropertyChanged
    {
        private ICollection<Model.Task> _tasks;
        private object _selectedItem;
        private bool _canSelect;

        public event PropertyChangedEventHandler PropertyChanged;

        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                CanSelect = _selectedItem != null;
                Controller.Instance.CurrentTask = SelectedTask;
                OnPropertyChanged();
            }
        }

        public bool CanSelect
        {
            get
            {
                return _canSelect;
            }
            set
            {
                _canSelect = value;
                OnPropertyChanged();
            }
        }

        public ICollection<Model.Task> Tasks
        {
            get { return _tasks; }
        }

        public Model.Task SelectedTask
        {
            get { return (Model.Task)_selectedItem; }
        }

        public async Task DeleteTask()
        {
            var task = (Model.Task)SelectedItem;
            await Controller.Instance.UnitOfWork.TasksAsync.RemoveAsync(task);

            _tasks.Remove(task);
            OnPropertyChanged("Tasks");
        }

        public async Task LoadTasks()
        {
            var user = await Controller.Instance.UnitOfWork.UsersAsync.GetAsync(Controller.Instance.User.UserId);
            _tasks = new List<Model.Task>(user.Tasks);
            
            if (Controller.Instance.User.Tasks == null)
            {
                Controller.Instance.User.Tasks = new List<Model.Task>(user.Tasks);
            }
            else
            {
                Controller.Instance.User.Tasks.Clear();
                foreach (var task in _tasks)
                {
                    Controller.Instance.User.Tasks.Add(task);
                }
            }

            OnPropertyChanged("Tasks");
            if (Controller.Instance.CurrentTask != null)
            {
                SelectedItem = Controller.Instance.CurrentTask;
            }
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
