using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IUserMediaRepository :
        IAddAsync<UserMedia>,
        IGetAllAsync<UserMedia>
    {
        Task<UserMedia> FindUserMediaIncludeMediaAsync(string userId);
    }
}
