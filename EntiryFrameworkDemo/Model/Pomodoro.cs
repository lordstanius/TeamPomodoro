using System;

namespace Model
{
	public class Pomodoro
	{
		public Guid PomodoroId { get; set; }
		public Guid TaskId { get; set; }
		public DateTime StartTime { get; set; }
		public int DurationInMin { get; set; }
		public bool? IsSuccessfull { get; set; }
		public virtual Task Task { get; set; }
	}
}
