using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IItemMediaRepository :
        IAddAsync<ItemMedia>,
        IGetAllAsync<ItemMedia>
    {
        Task<ItemMedia> FindItemMediaIncludeMediaAsync(int itemId);
    }
}
