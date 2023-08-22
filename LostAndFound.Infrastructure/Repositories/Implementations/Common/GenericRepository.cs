using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Implementations.Common
{
    public class GenericRepository<T> :
        IGetAllAsync<T>,
        IFindAsync<T>,
        IAddAsync<T>,
        IUpdate<T>,
        IReload<T>,
        IDelete<T> where T : class
    {
        protected readonly LostAndFoundDbContext _context;
        public GenericRepository(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
        }

        public virtual void Delete(T obj)
        {
            _context.Set<T>().Remove(obj);
        }

        public virtual async Task<T> FindAsync(params object[] keys)
        {
            return await _context.Set<T>().FindAsync(keys);
        }

        public virtual Task<IEnumerable<T>> GetAllAsync(bool trackChanges = false)
        {
            IQueryable<T> dbSet = _context.Set<T>();
            if (trackChanges == false)
            {
                dbSet = dbSet.AsNoTracking();
            }

            return Task.FromResult(dbSet.AsEnumerable());
        }

        public void Reload(T obj)
        {
            _context.Entry(obj).Reload();
        }

        public virtual void Update(T obj)
        {
            _context.Set<T>().Update(obj);
        }
    }
}
