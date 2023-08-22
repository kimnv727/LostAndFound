using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository :
                IGetAllAsync<User>,
                IAddAsync<User>,
                IDelete<User>
    {
    }
}
