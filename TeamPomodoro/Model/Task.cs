using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Task : IEquatable<Task>, IEntity
	{
		public Guid TaskId { get; set; }

		[MaxLength(50)]
		public string Name { get; set; }
		public int PomodoroCount { get; set; }
		public Guid UserId { get; set; }
		public Guid ProjectId { get; set; }
		public Guid? TeamId { get; set; }

		public User User { get; set; }
		public Project Project { get; set; }
		public Team Team { get; set; }
		public ICollection<Pomodoro> Pomodoroes { get; set; }

		public override bool Equals(object obj)
		{
			return obj is Task ? ((Task)obj).TaskId == TaskId : false;
		}

		public bool Equals(Task task)
		{
			return task.TaskId == TaskId;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(TaskId.ToByteArray(), 0);
		}

		public override string ToString()
		{
			return Name;
		}

		public Guid GetId() { return TaskId; }
	}
}
