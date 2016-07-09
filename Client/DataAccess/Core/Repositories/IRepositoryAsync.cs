using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Core.Repositories
{
	public interface IRepositoryAsync<TEntity> where TEntity : class
	{
		Task<TEntity> GetAsync(Guid id);

		Task<IEnumerable<TEntity>> GetAllAsync();

		Task<TEntity> FindAsync(Func<TEntity, bool> predicate);

		Task AddAsync(TEntity entity);

		Task RemoveAsync(Guid id);
	}
}
