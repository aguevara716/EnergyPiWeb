using System;
using System.Linq;
using System.Threading.Tasks;
using EnergyPi.Web.Entities;

namespace EnergyPi.Web.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        // Create

        // Read
        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Func<TEntity, Boolean> predicate);

        // Update

        // Delete
    }
}
