using System;

namespace Model
{
	public class Pomodoro
	{
		public Guid PomodoroId { get; set; }
		public Guid TaskId { get; set; }
		public DateTime? StartTime { get; set; }
		public int DurationInMin { get; set; }
		public bool? IsSuccessfull { get; set; }

		public Task Task { get; set; }

		public override bool Equals(object obj)
		{
			return ((Pomodoro)obj).PomodoroId == PomodoroId;
		}

		public bool Equals(Pomodoro pomodoro)
		{
			return pomodoro.PomodoroId == PomodoroId;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(PomodoroId.ToByteArray(), 0);
		}
	}
}
