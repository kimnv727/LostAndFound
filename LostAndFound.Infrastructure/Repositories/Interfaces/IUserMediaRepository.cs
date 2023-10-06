using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IUserMediaRepository :
        IAddAsync<UserMedia>,
        IGetAllAsync<UserMedia>
    {
        Task<UserMedia> FindUserMediaWithOnlyAvatarAsync(string userId);
        Task<ICollection<UserMedia>> FindUserMediaWithMediasExceptAvatarAsync(string userId);
        Task<ICollection<UserMedia>> FindUserMediaWithMediasAsync(string userId);
    }
}
