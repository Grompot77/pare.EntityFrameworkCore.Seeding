using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace pare.EntityFrameworkCore
{
    /// <summary>
    /// Extracted parts of the DbContext for methods and properties that we might want to invoke or use
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Sets this instance.
        /// </summary>
        /// <typeparam name="TDomainObject">The type of the domain object.</typeparam>
        /// <returns></returns>
        DbSet<TDomainObject> Set<TDomainObject>() where TDomainObject : class;
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
        /// <summary>
        /// Attaches the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        EntityEntry<TEntity> Attach<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Gets the change tracker.
        /// </summary>
        /// <value>
        /// The change tracker.
        /// </value>
        ChangeTracker ChangeTracker { get; }

        /// <summary>
        /// Entries the specified entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
