using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Project
	{
		public Guid ProjectId { get; set; }

		[MaxLength(50)]
		public string Name { get; set; }

		public ICollection<Task> Tasks { get; set; }
	}
}
