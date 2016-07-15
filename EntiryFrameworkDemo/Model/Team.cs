using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class Team : IEquatable<Team>, IEntity
	{
		public Guid TeamId { get; set; }

		[MaxLength(20)]
		public string Name { get; set; }

		public ICollection<User> Users { get; set; }
		public ICollection<UserTeam> UserTeams { get; set; }

		public override bool Equals(object obj)
		{
			return ((Team)obj).TeamId == TeamId;
		}

		public bool Equals(Team team)
		{
			return team.TeamId == TeamId;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(TeamId.ToByteArray(), 0);
		}

		public override string ToString()
		{
			return Name;
		}

		public Guid Id() { return TeamId; }
	}
}
