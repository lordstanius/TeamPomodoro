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

		int SaveChanges();
	}
}
