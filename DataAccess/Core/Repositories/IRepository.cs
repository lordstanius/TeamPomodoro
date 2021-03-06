﻿using System;
using System.Collections.Generic;

namespace DataAccess.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : Model.IEntity
    {
        /// <summary>
        /// Returns entity with a specified ID. Changes to this object will be tracked
        /// and saved after unit of work is completed.
        /// </summary>
        TEntity GetById(Guid id);

        /// <summary>
        /// Returns read-only collection of entities.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        //Required by design
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Adds an entiry to collecton
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Removes entity with a specified ID from collection
        /// </summary>
        void Remove(TEntity entity);
    }
}
