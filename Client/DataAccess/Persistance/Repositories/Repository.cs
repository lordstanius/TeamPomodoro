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
		string _uri;
		HttpClient _client;
		List<TEntity> _entities;

		internal Repository(HttpClient client, string uri)
		{
			_client = client;
			_uri = uri;
			_entities = new List<TEntity>(GetAll());
		}

		public void Add(TEntity entity)
		{
			AddAsync(entity).Wait();
		}

		public TEntity Find(Func<TEntity, bool> predicate)
		{
			throw new NotImplementedException();
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
			using (HttpResponseMessage message = await _client.PutAsync(_uri, httpContent))
				if (message.StatusCode != HttpStatusCode.Created)
					throw new ArgumentException(string.Format("Failed to add entity with status {0} ({1}): ", message.StatusCode, (int)message.StatusCode));
		}

		public Task<TEntity> FindAsync(Func<TEntity, bool> predicate)
		{
			throw new NotImplementedException();
		}

		public async Task<TEntity> GetAsync(Guid id)
		{
			using (HttpResponseMessage message = await _client.GetAsync(_uri + id))
			{
				string content = await message.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<TEntity>(content);
			}
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			using (HttpResponseMessage message = await _client.GetAsync(_uri))
			{
				string content = await message.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<List<TEntity>>(content);
			}
		}

		public async Task RemoveAsync(Guid id)
		{
			using (HttpResponseMessage message = await _client.DeleteAsync(_uri + id))
				if (message.StatusCode != HttpStatusCode.OK)
					throw new ArgumentException(string.Format("Failed to delete entity with status {0} ({1}): ", message.StatusCode, (int)message.StatusCode));
		}
	}
}
