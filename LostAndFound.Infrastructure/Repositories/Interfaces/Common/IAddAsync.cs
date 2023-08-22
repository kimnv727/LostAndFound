using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces.Common
{
    public interface IAddAsync<T> where T : class
    {
        Task AddAsync(T obj);
    }
}
