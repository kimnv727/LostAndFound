using System;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository :
        IGetAllAsync<User>,
        IQueryAsync<User, UserQuery>,
        IAddAsync<User>,
        IDelete<User>
    {
        Task<User> FindUserByID(Guid id);
        Task<User> FindUserByEmail(string email);
        Task<bool> IsDuplicatedEmail(string email);
        Task<bool> IsDuplicatedPhoneNumber(string phoneNumber);
        
    }
}
