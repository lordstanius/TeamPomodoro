using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Core.Repositories
{
	public interface IRepositoryAsync<TEntity> where TEntity : Model.IEntity
	{
		/// <summary>
		/// Returns entity with a specified Id after the asynchronous task is completed. 
		/// Changes to this object will be tracked and saved after unit of work is completed.
		/// </summary>
		Task<TEntity> GetAsync(Guid id);

		/// <summary>
		/// Returns read-only collection of entities after the asynchronous task is completed.
		/// </summary>
		Task<IEnumerable<TEntity>> GetAllAsync();

		/// <summary>
		/// Runs task to add an entity to the collecton 
		/// and returns immediatelly to the caller.
		/// </summary>
		Task AddAsync(TEntity entity);

		/// <summary>
		/// Run task to remove an entity with a specified ID from collection,
		/// and returns immediatelly to the caller.
		/// </summary>
		Task RemoveAsync(TEntity entity);
	}
}
