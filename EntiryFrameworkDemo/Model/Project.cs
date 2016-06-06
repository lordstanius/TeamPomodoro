using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Project
	{
		[Key]
		public Guid ProjectId { get; set; }
		public string Name { get; set; }

		public ICollection<Task> Tasks { get; set; }
	}
}
