using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
	public class User
	{
		public Guid UserId {	get; set; }

		[MaxLength(20)]
		public string UserName { get; set; }

		[MaxLength(20)]
		public string Password { get; set; }
		public bool ShowWarningAfterPomodoroExpires { get; set; }
		public int PomodoroDurationInMin { get; set; }
		public Guid? TeamId { get; set; }
		public Guid? CurrentUserTeamId { get; set; }

		public Team Team { get; set; }
		public ICollection<User> Users { get; set; }
		public ICollection<UserTeam> UserTeams { get; set; }

		public override bool Equals(object obj)
		{
			return ((User)obj).UserId == UserId;
		}

		public bool Equals(User user)
		{
			return user.UserId == UserId;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(UserId.ToByteArray(), 0);
		}
	}
}
