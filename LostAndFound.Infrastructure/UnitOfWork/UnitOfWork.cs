using System.Threading.Tasks;
using LostAndFound.Infrastructure.Data;

namespace LostAndFound.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly LostAndFoundDbContext _context;

        public UnitOfWork(LostAndFoundDbContext context)
        {
            _context = context;
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}