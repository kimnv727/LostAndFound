using System.Threading.Tasks;
using LostAndFound.Infrastructure;

namespace LostAndFound.Infrastructure.UnitOfWork
{
    // public interface IUnitOfWork
    // {
    //     Task<int> CommitAsync();
    // }
    //
    // public class UnitOfWork : IUnitOfWork
    // {
    //     private readonly ParkingDbContext _context;
    //
    //     public UnitOfWork(ParkingDbContext context)
    //     {
    //         _context = context;
    //     }
    //
    //     public Task<int> CommitAsync()
    //     {
    //         return _context.SaveChangesAsync();
    //     }
    // }
}