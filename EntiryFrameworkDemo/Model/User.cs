using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
	public class User
	{
		[Key]
		public Guid UserId {	get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }

		public bool ShowWarningAfterPomodoroExpires { get; set; }
		public int PomodoroDurationInMin { get; set; }

		public Guid TeamId { get; set; }
		public Guid CurrentUserTeamId { get; set; }

		//[ForeignKey("TeamId")]
		public virtual Team Team { get; set; }
		public virtual ICollection<Task> Tasks { get; set; }
		public virtual ICollection<UserTeam> UserTeams { get; set; }

	}
}
