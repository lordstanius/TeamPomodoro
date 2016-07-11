using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Model;
using DataAccess.Core;
using DataAccess.Core.Repositories;
using DataAccess.Persistance.Repositories;


namespace DataAccess.Persistance
{
	public class UnitOfWork : IUnitOfWork
	{
		Repository<Task> _tasksRepo;
		Repository<User> _usersRepo;
		Repository<Team> _teamsRepo;
		Repository<Project> _projectsRepo;
		Repository<Pomodoro> _pomodoroesRepo;
		Repository<UserTeam> _userTeamsRepo;

		public IRepository<Task> Tasks { get { return _tasksRepo; } }
		public IRepository<User> Users { get { return _usersRepo; } }
		public IRepository<Team> Teams { get { return _teamsRepo; } }
		public IRepository<Project> Projects { get { return _projectsRepo; } }
		public IRepository<Pomodoro> Pomodoroes { get { return _pomodoroesRepo; } }
		public IRepository<UserTeam> UserTeams { get { return _userTeamsRepo; } }
		public IRepositoryAsync<Task> AsyncTasks { get { return _tasksRepo; } }
		public IRepositoryAsync<User> AsyncUsers { get { return _usersRepo; } }
		public IRepositoryAsync<Team> AsyncTeams { get { return _teamsRepo; } }
		public IRepositoryAsync<Project> AsyncProjects { get { return _projectsRepo; } }
		public IRepositoryAsync<Pomodoro> AsyncPomodoroes { get { return _pomodoroesRepo; } }
		public IRepositoryAsync<UserTeam> AsyncUserTeams { get { return _userTeamsRepo; } }

		public int SaveChanges()
		{
			int count = 0;
			count += SaveChanges(Tasks);
			count += SaveChanges(Users);
			count += SaveChanges(Teams);
			count += SaveChanges(Projects);
			count += SaveChanges(Pomodoroes);
			count += SaveChanges(UserTeams);

			return count;
		}

		public UnitOfWork(string uri)
		{
			_client = new HttpClient();

			_tasksRepo = new Repository<Task>(_client, uri + "/tasks/");
			_usersRepo = new Repository<User>(_client, uri + "/users/");
			_teamsRepo = new Repository<Team>(_client, uri + "/teams/");
			_projectsRepo = new Repository<Project>(_client, uri + "/projects/");
			_pomodoroesRepo = new Repository<Pomodoro>(_client, uri + "/pomodoroes/");
			_userTeamsRepo = new Repository<UserTeam>(_client, uri + "/userteams/");
		}

		HttpClient _client;

		public async System.Threading.Tasks.Task<int> SaveChangesAsync()
		{
			int count = 0;
			count += await SaveChangesAsync(Tasks);
			count += await SaveChangesAsync(Users);
			count += await SaveChangesAsync(Teams);
			count += await SaveChangesAsync(Projects);
			count += await SaveChangesAsync(Pomodoroes);
			count += await SaveChangesAsync(UserTeams);

			return count;
		}

		int SaveChanges<TEntity>(IRepository<TEntity> repo) where TEntity : class
		{
			var task = SaveChangesAsync(repo);
			task.Wait();
			return task.Result;
		}

		async System.Threading.Tasks.Task<int> SaveChangesAsync<TEntity>(IRepository<TEntity> repo) where TEntity : class
		{
			var entities = ((Repository<TEntity>)repo).Entities;
			foreach (var entity in entities)
			{
				string content = JsonConvert.SerializeObject(entity);
				using (HttpContent httpContent = new StringContent(content))
				using (HttpResponseMessage message = await _client.PostAsync(((Repository<TEntity>)repo).Uri, httpContent))
					if (message.StatusCode != HttpStatusCode.NoContent)
						throw new ArgumentException(string.Format("Failed to save entity with status {0} ({1})", message.StatusCode, (int)message.StatusCode));
			}

			int changes = entities.Count;
			entities.Clear();
			return changes;
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
