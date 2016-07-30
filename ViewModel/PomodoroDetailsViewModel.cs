using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ViewModel
{
    public class PomodoroDetailsViewModel : INotifyPropertyChanged
    {
        private ICollection<Model.Task> _tasks = new List<Model.Task>();
        private ICollection<View.PomodoroView> _pomodoros = new List<View.PomodoroView>();
        private object _selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICollection<Model.Task> Tasks
        {
            get { return _tasks; }
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

        public ICollection<View.PomodoroView> Pomodoros
        {
            get { return _pomodoros; }
        }

        public async Task Initialize()
        {
            _tasks.Clear();
            foreach (var item in Controller.Instance.UnitOfWork.Tasks.GetAll())
            {
                var task = await Controller.Instance.UnitOfWork.TasksAsync.GetAsync(item.TaskId);
                _tasks.Add(task);
            }

            SelectedItem = _tasks.FirstOrDefault(t => t.Equals(Controller.Instance.CurrentTask));
            OnPropertyChanged("Tasks");

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

            _pomodoros.Clear();
            int i = 0;
            foreach (var pomodoro in task.Pomodoroes)
            {
                _pomodoros.Add(new View.PomodoroView(pomodoro) { No = ++i });
            }

            OnPropertyChanged("Pomodoros");
        }
    }
}
