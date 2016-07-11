using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Task
	{
		public Guid TaskId { get; set; }

		[MaxLength(50)]
		public string Name { get; set; }
		public int PomodoroCount { get; set; }
		public Guid UserId { get; set; }
		public Guid ProjectId { get; set; }

		public User User { get; set; }
		public Project Project { get; set; }
		public ICollection<Pomodoro> Pomodoroes { get; set; }

		public override bool Equals(object obj)
		{
			return ((Task)obj).TaskId == TaskId;
		}

		public bool Equals(Task task)
		{
			return task.TaskId == TaskId;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(TaskId.ToByteArray(), 0);
		}
	}
}
