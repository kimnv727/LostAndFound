using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces.Common;

namespace LostAndFound.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository :
        IGetAllAsync<User>,
        IAddAsync<User>,
        IUpdate<User>,
        IDelete<User>
    {
        Task<User> FindUserByID(string id);
        Task<User> FindUserByEmail(string email);
        Task<bool> IsDuplicatedEmail(string email);
        Task<bool> IsDuplicatedPhoneNumber(string phoneNumber);
        Task<IEnumerable<User>> QueryUserAsync(UserQuery query, bool trackChanges = false);
        Task<IEnumerable<User>> QueryUserIgnoreStatusAsync(UserQueryIgnoreStatus query, bool trackChanges = false);
        Task<IEnumerable<User>> QueryUserIgnoreStatusWithoutWaitingVerifiedAsync(UserQueryIgnoreStatusWithoutWaitingVerified query, bool trackChanges = false);
    }
}
