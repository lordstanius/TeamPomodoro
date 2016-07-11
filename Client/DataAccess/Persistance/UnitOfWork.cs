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
		public IRepository<Task> Tasks { get; private set; }

		public IRepository<User> Users { get; private set; }

		public IRepository<Team> Teams { get; private set; }

		public IRepository<Project> Projects { get; private set; }

		public IRepository<Pomodoro> Pomodoroes { get; private set; }

		public IRepository<UserTeam> UserTeams { get; private set; }

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

			Tasks = new Repository<Task>(_client, uri + "/tasks/");
			Users = new Repository<User>(_client, uri + "/users/");
			Teams = new Repository<Team>(_client, uri + "/teams/");
			Projects = new Repository<Project>(_client, uri + "/projects/");
			Pomodoroes = new Repository<Pomodoro>(_client, uri + "/pomodoroes/");
			UserTeams = new Repository<UserTeam>(_client, uri + "/userteams/");
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
