using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModel
{
    public class PomodoroDetailsViewModel : INotifyPropertyChanged
    {
        private List<Model.Task> _tasks;
        private List<View.PomodoroView> _pomodoros;
        private object _selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;
        
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

        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }

            set
            {
                _selectedItem = value;
                UpdatePomodoroList();
                OnPropertyChanged();
            }
        }

        public List<View.PomodoroView> Pomodoros
        {
            get
            {
                return _pomodoros;
            }

            set
            {
                _pomodoros = value;
                OnPropertyChanged();
            }
        }

        public async Task Initialize()
        {
            var items = new List<Model.Task>();
            foreach (var item in Controller.Instance.UnitOfWork.Tasks.GetAll())
            {
                var task = await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(item.TaskId);
                items.Add(task);
            }

            Tasks = items;
            SelectedItem = items.FirstOrDefault(t => t.Equals(Controller.Instance.CurrentTask));

            if (SelectedItem != null)
            {
                UpdatePomodoroList();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void UpdatePomodoroList()
        {
            var task = (Model.Task)SelectedItem;
            Controller.Instance.CurrentTask = task;

            var items = new List<View.PomodoroView>(task.Pomodoroes.Count);
            int i = 0;
            foreach (var pomodoro in task.Pomodoroes)
            {
                items.Add(new View.PomodoroView(pomodoro) { No = ++i });
            }

            Pomodoros = items;
        }
    }
}
