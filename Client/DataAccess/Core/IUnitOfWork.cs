using System;
using Model;
using DataAccess.Core.Repositories;

namespace DataAccess.Core
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<User> Users { get; }
		IRepository<Task> Tasks { get; }
		IRepository<Team> Teams { get; }
		IRepository<Project> Projects { get; }
		IRepository<Pomodoro> Pomodoroes { get; }
		IRepository<UserTeam> UserTeams { get; }

		IRepositoryAsync<User> UsersAsync { get; }
		IRepositoryAsync<Task> TasksAsync { get; }
		IRepositoryAsync<Team> TeamsAsync { get; }
		IRepositoryAsync<Project> ProjectsAsync { get; }
		IRepositoryAsync<Pomodoro> PomodoroesAsync { get; }
		IRepositoryAsync<UserTeam> UserTeamsAsync { get; }

		int SaveChanges();
		System.Threading.Tasks.Task<int> SaveChangesAsync();
	}
}
