using System;
using System.Linq;
using System.Threading.Tasks;
using EnergyPi.Web.Data;
using EnergyPi.Web.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnergyPi.Web.Repositories
{
    public class DataRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        // Dependencies
        private readonly DataDbContext _dataDbContext;

        // Constructors
        public DataRepository(DataDbContext dataDbContext)
        {
            _dataDbContext = dataDbContext;
        }

        // Private Methods

        // Create
        /* purposely blank */

        // Read
        public TEntity Get(int id)
        {
            var record = Get().FirstOrDefault(t => t.Id == id);
            return record;
        }

        public Task<TEntity> GetAsync(int id)
        {
            var record = Get().FirstOrDefaultAsync(t => t.Id == id);
            return record;
        }

        public IQueryable<TEntity> Get()
        {
            var table = _dataDbContext.Set<TEntity>();
            return table;
        }

        public IQueryable<TEntity> Get(Func<TEntity, Boolean> predicate)
        {
            var results = Get().Where(predicate).AsQueryable();
            return results;
        }

        // Update
        /* purposely blank */

        // Delete
        /* purposely blank */

    }
}
