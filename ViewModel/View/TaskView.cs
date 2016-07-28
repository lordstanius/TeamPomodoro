using System;
using System.Linq;

namespace ViewModel.View
{
    public class TaskView
    {
        public TaskView(Model.Task t)
        {
            TaskName = t.Name;
            Successful = t.Pomodoroes.Count(p => p.IsSuccessfull == true);
            Failed = t.Pomodoroes.Count(p => p.IsSuccessfull == false);
            Total = t.PomodoroCount - t.Pomodoroes.Count;
            Duration = TimeSpan.FromMinutes(t.Pomodoroes.Sum(p => p.DurationInMin)).ToString("mm\\:ss");
            Task = t;
        }

        public string TaskName { get; private set; }
        public int Successful { get; private set; }
        public int Failed { get; private set; }
        public int Total { get; private set; }
        public string Duration { get; private set; }

        public Model.Task Task { get; private set; }
    }
}
