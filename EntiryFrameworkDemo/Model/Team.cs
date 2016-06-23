using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Team
	{
		public Guid TeamId { get; set; }

		[MaxLength(20)]
		public string Name { get; set; }

		public ICollection<User> Users { get; set; }
		public ICollection<UserTeam> UserTeams { get; set; }
	}
}
