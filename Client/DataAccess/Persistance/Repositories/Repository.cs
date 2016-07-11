using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using DataAccess.Core.Repositories;

namespace DataAccess.Persistance.Repositories
{
	public class Repository<TEntity> :
		IRepository<TEntity>, IRepositoryAsync<TEntity> where TEntity : class
	{ 
		internal string Uri { get; private set; }
		internal List<TEntity> Entities { get; private set; }

		HttpClient _client;

		internal Repository(HttpClient client, string uri)
		{
			_client = client;
			Uri = uri;
			Entities = new List<TEntity>();
		}

		public void Add(TEntity entity)
		{
			AddAsync(entity).Wait();
		}

		public TEntity Get(Guid id)
		{
			Task<TEntity> task = GetAsync(id);
			task.Wait();
			return task.Result;
		}

		public IEnumerable<TEntity> GetAll()
		{
			Task<IEnumerable<TEntity>> task = GetAllAsync();
			task.Wait();
			foreach (var entity in task.Result)
				UpdateEntities(entity);

			return task.Result;
		}

		public void Remove(Guid id)
		{
			RemoveAsync(id).Wait();
		}

		public async Task AddAsync(TEntity entity)
		{
			string content = JsonConvert.SerializeObject(entity);
			using (HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json"))
			using (HttpResponseMessage message = await _client.PutAsync(Uri, httpContent))
				if (message.StatusCode != HttpStatusCode.Created)
					throw new ArgumentException(string.Format("Failed to add entity with status {0} ({1}): ", message.StatusCode, (int)message.StatusCode));
		}

		public async Task<TEntity> GetAsync(Guid id)
		{
			using (HttpResponseMessage message = await _client.GetAsync(Uri + id))
			{
				string content = await message.Content.ReadAsStringAsync();
				TEntity entity = JsonConvert.DeserializeObject<TEntity>(content);
				if (entity != null)
					UpdateEntities(entity);
				return entity;
			}
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			using (HttpResponseMessage message = await _client.GetAsync(Uri))
			{
				string content = await message.Content.ReadAsStringAsync();
				var entities = JsonConvert.DeserializeObject<List<TEntity>>(content);
				foreach (var entity in entities)
					UpdateEntities(entity);

				return entities;
			}
		}

		public async Task RemoveAsync(Guid id)
		{
			using (HttpResponseMessage message = await _client.DeleteAsync(Uri + id))
				if (message.StatusCode != HttpStatusCode.OK)
					throw new ArgumentException(string.Format("Failed to delete entity with status {0} ({1})", message.StatusCode, (int)message.StatusCode));
		}

		void UpdateEntities(TEntity entity)
		{
			bool updated = false;
			for (int i = 0; i < Entities.Count; i++)
			{
				if (Entities[i].Equals(entity))
				{
					Entities[i] = entity;
					updated = true;
				}
			}

			if (!updated)
				Entities.Add(entity);
		}
	}
}
