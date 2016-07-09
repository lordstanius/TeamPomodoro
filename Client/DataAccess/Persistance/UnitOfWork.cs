using System;
using System.Net.Http;
using Model;
using DataAccess.Core;
using DataAccess.Core.Repositories;
using DataAccess.Persistance.Repositories;


namespace DataAccess.Persistance
{
	public class UnitOfWork : IUnitOfWork
	{
		public IRepository<Task> Tasks { get; private set; }

		public IRepository<User> Users { get; private set; }

		public IRepository<Team> Teams { get; private set; }

		public IRepository<Project> Projects { get; private set; }

		public IRepository<Pomodoro> Pomodoroes { get; private set; }

		public IRepository<UserTeam> UserTeams { get; private set; }

		public int SaveChanges()
		{
			throw new NotImplementedException();
		}

		HttpClient _client;

		public UnitOfWork(string uri)
		{
			_client = new HttpClient();

			Tasks = new Repository<Task>(_client, uri + "/tasks/");
			Users = new Repository<User>(_client, uri + "/users/");
			Teams = new Repository<Team>(_client, uri + "/teams/");
			Projects = new Repository<Project>(_client, uri + "/projects/");
			Pomodoroes = new Repository<Pomodoro>(_client, uri + "/pomodoroes/");
			UserTeams = new Repository<UserTeam>(_client, uri + "/userteams/");
		}

		public void Dispose()
		{
			if (_client != null)
			{
				_client.Dispose();
				_client = null;
			}
		}
	}
}
