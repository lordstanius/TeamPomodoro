using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class EditTasksDialogViewModel : INotifyPropertyChanged
    {
        private List<Model.Task> _tasks;
        private object _selectedItem;
        private bool _canSelect;

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

        public Model.Task SelectedTask { get { return (Model.Task)_selectedItem; } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async Task DeleteTask()
        {
            var task = (Model.Task)SelectedItem;
            await Controller.Instance.UnitOfWork.TasksAsync.RemoveAsync(task);

            Tasks.Remove(task);
        }

        public async Task GetTasks()
        {
            var user = await Controller.Instance.UnitOfWork.UsersAsync.GetAsync(Controller.Instance.User.UserId);
            Tasks = new List<Model.Task>(user.Tasks);
            if (Controller.Instance.CurrentTask != null)
                SelectedItem = Controller.Instance.CurrentTask;
        }
    }
}
