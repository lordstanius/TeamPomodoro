using System;
using System.Linq;

namespace ViewModel.View
{
    public class TaskView
    {
        public TaskView(Model.Task task)
        {
            if (task != null)
            {
                TaskName = task.Name;
                Successful = task.Pomodoroes.Count(p => p.IsSuccessfull == true);
                Failed = task.Pomodoroes.Count(p => p.IsSuccessfull == false);
                Total = task.PomodoroCount - task.Pomodoroes.Count;
                Duration = TimeSpan.FromMinutes(task.Pomodoroes.Sum(p => p.DurationInMin)).ToString("mm\\:ss");
                Task = task;
            }
        }

        public string TaskName { get; private set; }
        public int Successful { get; private set; }
        public int Failed { get; private set; }
        public int Total { get; private set; }
        public string Duration { get; private set; }

        public Model.Task Task { get; private set; }
    }
}
