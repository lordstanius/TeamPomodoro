using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
	public class Task
	{
		[Key]
		public Guid TaskId { get; set; }
		public string Name { get; set; }
		public int PomodoroCount { get; set; }
		public Guid UserId { get; set; }
		public Guid ProjectId { get; set; }

		[ForeignKey("UserId")]
		public virtual User User { get; set; }

		[ForeignKey("ProjectId")]
		public virtual Project Project { get; set; }

		public virtual ICollection<Pomodoro> Pomodoros { get; set; }
	}
}
