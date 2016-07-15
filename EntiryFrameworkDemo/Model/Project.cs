using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Project : IEquatable<Project>, IEntity
	{
		public Guid ProjectId { get; set; }

		[MaxLength(50)]
		public string Name { get; set; }

		public ICollection<Task> Tasks { get; set; }

		public override bool Equals(object obj)
		{
			return ((Project)obj).ProjectId == ProjectId;
		}

		public bool Equals(Project project)
		{
			return project.ProjectId == ProjectId;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(ProjectId.ToByteArray(), 0);
		}

		public Guid Id() { return ProjectId; }
	}
}
