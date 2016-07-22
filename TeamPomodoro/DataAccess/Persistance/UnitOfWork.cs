﻿using System;
using System.Net;
using System.Text;
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
        private Repository<Task> _tasksRepo;
        private Repository<User> _usersRepo;
        private Repository<Team> _teamsRepo;
        private Repository<Project> _projectsRepo;
        private Repository<Pomodoro> _pomodoroesRepo;
        private Repository<UserTeam> _userTeamsRepo;

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

        public IRepository<Task> Tasks
        {
            get { return _tasksRepo; }
        }

        public IRepository<User> Users
        {
            get { return _usersRepo; }
        }

        public IRepository<Team> Teams
        {
            get { return _teamsRepo; }
        }

        public IRepository<Project> Projects
        {
            get { return _projectsRepo; }
        }

        public IRepository<Pomodoro> Pomodoroes
        {
            get { return _pomodoroesRepo; }
        }

        public IRepository<UserTeam> UserTeams
        {
            get { return _userTeamsRepo; }
        }

        public IRepositoryAsync<Task> TasksAsync
        {
            get { return _tasksRepo; }
        }

        public IRepositoryAsync<User> UsersAsync
        {
            get { return _usersRepo; }
        }

        public IRepositoryAsync<Team> TeamsAsync
        {
            get { return _teamsRepo; }
        }

        public IRepositoryAsync<Project> ProjectsAsync
        {
            get { return _projectsRepo; }
        }

        public IRepositoryAsync<Pomodoro> PomodoroesAsync
        {
            get { return _pomodoroesRepo; }
        }

        public IRepositoryAsync<UserTeam> UserTeamsAsync
        {
            get { return _userTeamsRepo; }
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
            // following is not tested
            ////int count = 0;
            ////count += SaveChanges(_tasksRepo);
            ////count += SaveChanges(_usersRepo);
            ////count += SaveChanges(_teamsRepo);
            ////count += SaveChanges(_projectsRepo);
            ////count += SaveChanges(_pomodoroesRepo);
            ////count += SaveChanges(_userTeamsRepo);

            ////return count;
        }

        HttpClient _client;

        public async System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            int count = 0;
            count += await SaveChangesAsync(_tasksRepo);
            count += await SaveChangesAsync(_usersRepo);
            count += await SaveChangesAsync(_teamsRepo);
            count += await SaveChangesAsync(_projectsRepo);
            count += await SaveChangesAsync(_pomodoroesRepo);
            count += await SaveChangesAsync(_userTeamsRepo);

            return count;
        }

        int SaveChanges<TEntity>(Repository<TEntity> repo) where TEntity : IEntity
        {
            throw new NotImplementedException();
            //// following is not tested
            ////var task = SaveChangesAsync(repo);
            ////task.Wait();
            ////return task.Result;
        }

        async System.Threading.Tasks.Task<int> SaveChangesAsync<TEntity>(Repository<TEntity> repo) where TEntity : IEntity
        {
            foreach (var entity in repo.Entities)
            {
                string content = JsonConvert.SerializeObject(entity);
                using (HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json"))
                using (HttpResponseMessage message = await _client.PostAsync(repo.Uri, httpContent))
                    if (message.StatusCode != HttpStatusCode.NoContent)
                        throw new ArgumentException(string.Format("Failed to save entity with status {0} ({1})", message.StatusCode, (int)message.StatusCode));
            }

            int changes = repo.Entities.Count;
            repo.Entities.Clear();
            return changes;
        }

        #region DISPOSABLE
        ~UnitOfWork()
        {
            Dispose(false);
        }

        private bool _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
