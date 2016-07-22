using System.Data.Entity;

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
			base("name=TeamPomodoro")
		{
		}
	}
}
