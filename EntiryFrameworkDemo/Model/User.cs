﻿using System;
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

		public virtual Team Team { get; set; }
		public virtual ICollection<Task> Tasks { get; set; }
		public virtual ICollection<UserTeam> UserTeams { get; set; }

	}
}
