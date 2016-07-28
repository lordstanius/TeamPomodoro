using System;
using ViewModel.Globalization;

namespace ViewModel.View
{
    public class PomodoroView
    {
        public PomodoroView(Model.Pomodoro pomodoro)
        {
            Date = pomodoro.StartTime.Value.Date;
            Start = pomodoro.StartTime.Value.TimeOfDay.ToString("hh\\:mm\\:ss");
            Duration = TimeSpan.FromMinutes(pomodoro.DurationInMin).ToString("mm\\:hh");
            IsSuccessful = pomodoro.IsSuccessfull == true ? Strings.TxtYes : Strings.TxtNo;
        }

        public int No { get; set; }
        public DateTime Date { get; private set; }
        public string Start { get; private set; }
        public string Duration { get; private set; }
        public string IsSuccessful { get; private set; }
    }
}
