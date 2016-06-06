using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
	public class Pomodoro
	{
		[Key]
		public Guid PomodoroId { get; set; }
		public Guid TaskId { get; set; }
		public DateTime StartTime { get; set; }
		public int DurationInMin { get; set; }
		public bool IsSuccessfull { get; set; }

		[ForeignKey("TaskId")]
		public virtual Task Task { get; set; }
	}
}
