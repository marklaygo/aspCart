using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace aspCart.Infrastructure.EFRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get All Entity
        /// </summary>
        /// <returns>Entity</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Find Entity using linq expression
        /// </summary>
        /// <param name="predicate">linq expression</param>
        /// <returns>Entity</returns>
        TEntity FindByExpression(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find Entities using linq expression
        /// </summary>
        /// <param name="predicate">linq expression</param>
        /// <returns>List of entities</returns>
        IQueryable<TEntity> FindManyByExpression(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find Entity using id
        /// </summary>
        /// <param name="id">Id of entity</param>
        /// <returns>Entity</returns>
        TEntity FindById(Guid id);

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Save Changes
        /// </summary>
        void SaveChanges();
    }
}
