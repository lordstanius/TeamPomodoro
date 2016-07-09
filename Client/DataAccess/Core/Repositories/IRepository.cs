using System;
using System.Collections.Generic;

namespace DataAccess.Core.Repositories
{
	public interface IRepository<TEntity> where TEntity : class
	{
		TEntity Get(Guid id);

		IEnumerable<TEntity> GetAll();

		TEntity Find(Func<TEntity, bool> predicate);

		void Add(TEntity entity);

		void Remove(Guid id);
	}
}
