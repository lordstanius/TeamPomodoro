using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Team
	{
		[Key]
		public Guid TeamId { get; set; }
		public string Name { get; set; }

		public virtual ICollection<User> Users { get; set; }
		public virtual ICollection<UserTeam> UserTeams { get; set; }
	}
}
