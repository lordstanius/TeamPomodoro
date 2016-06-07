﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public class PomodoroContext : DbContext
	{
		public DbSet<Pomodoro> Pomodoros { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Task> Tasks { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserTeam> UserTeams { get; set; }

		public PomodoroContext() :
			base("Data Source=.\\sqlexpress; Initial Catalog = TeamPomodoro; Integrated Security = True; MultipleActiveResultSets = True")
		{
		}
	}
}
