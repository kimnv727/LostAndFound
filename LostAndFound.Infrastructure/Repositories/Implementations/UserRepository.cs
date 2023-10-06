using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Implementations.Common;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(LostAndFoundDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> QueryUserAsync(UserQuery query, bool trackChanges = false)
        {
            IQueryable<User> users = _context.Users
                .Include(u => u.UserMedias.Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null && um.MediaType != UserMediaType.AVATAR))
                .ThenInclude(um => um.Media)
                .Include(u => u.Role)
                .Where(u => u.IsActive == true && u.RoleId != 1)
                .AsSplitQuery();

            if (!trackChanges)
            {
                users = users.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(query.FirstName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.LastName))
            {
                users = users.Where(u => u.LastName.ToLower().Contains(query.LastName.ToLower()));
            }

            if (Enum.IsDefined(query.Gender))
            {
                if (query.Gender == UserQuery.GenderSearch.Male)
                {
                    users = users.Where(u => u.Gender == Gender.Male);
                }
                else if (query.Gender == UserQuery.GenderSearch.Female)
                {
                    users = users.Where(u => u.Gender == Gender.Female);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                users = users.Where(u => u.Email.ToLower().Contains(query.Email.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Phone))
            {
                users = users.Where(u => u.Phone.ToLower().Contains(query.Phone.ToLower()));
            }

            /*if (!string.IsNullOrWhiteSpace(query.Avatar))
            {
                users = users.Where(u => u.Avatar.ToLower().Contains(query.Avatar.ToLower()));
            }*/
            
            if (!string.IsNullOrWhiteSpace(query.SchoolId))
            {
                users = users.Where(u => u.SchoolId.ToLower().Contains(query.SchoolId.ToLower()));
            }

            if (Enum.IsDefined(query.Campus))
            {
                if (query.Campus == UserQuery.CampusSearch.HO_CHI_MINH_CAMPUS)
                {
                    users = users.Where(u => u.Campus == Campus.HO_CHI_MINH_CAMPUS);
                }
                else if (query.Campus == UserQuery.CampusSearch.DA_NANG_CAMPUS)
                {
                    users = users.Where(u => u.Campus == Campus.DA_NANG_CAMPUS);
                }
                else if (query.Campus == UserQuery.CampusSearch.HA_NOI_CAMPUS)
                {
                    users = users.Where(u => u.Campus == Campus.HA_NOI_CAMPUS);
                }
            }
            
            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                users = users.Where(p => p.FullName.ToLower().Contains(query.SearchText.ToLower()));
            }
            else if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                users = users.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(users.ToList());
        }

        public async Task<IEnumerable<User>> QueryUserIgnoreStatusAsync(UserQueryIgnoreStatus query, bool trackChanges = false)
        {
            IQueryable<User> users = _context.Users
                .Include(u => u.UserMedias.Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null && um.MediaType != UserMediaType.AVATAR))
                .ThenInclude(um => um.Media)
                .Include(u => u.Role)
                .Where(u => u.RoleId != 1)
                .AsSplitQuery();

            if (!trackChanges)
            {
                users = users.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(query.FirstName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.LastName))
            {
                users = users.Where(u => u.LastName.ToLower().Contains(query.LastName.ToLower()));
            }

            if (Enum.IsDefined(query.Gender))
            {
                if (query.Gender == UserQueryIgnoreStatus.GenderSearch.Male)
                {
                    users = users.Where(u => u.Gender == Gender.Male);
                }
                else if (query.Gender == UserQueryIgnoreStatus.GenderSearch.Female)
                {
                    users = users.Where(u => u.Gender == Gender.Female);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                users = users.Where(u => u.Email.ToLower().Contains(query.Email.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Phone))
            {
                users = users.Where(u => u.Phone.ToLower().Contains(query.Phone.ToLower()));
            }

            /*if (!string.IsNullOrWhiteSpace(query.Avatar))
            {
                users = users.Where(u => u.Avatar.ToLower().Contains(query.Avatar.ToLower()));
            }*/
            
            if (!string.IsNullOrWhiteSpace(query.SchoolId))
            {
                users = users.Where(u => u.SchoolId.ToLower().Contains(query.SchoolId.ToLower()));
            }

            if (Enum.IsDefined(query.Campus))
            {
                if (query.Campus == UserQueryIgnoreStatus.CampusSearch.HO_CHI_MINH_CAMPUS)
                {
                    users = users.Where(u => u.Campus == Campus.HO_CHI_MINH_CAMPUS);
                }
                else if (query.Campus == UserQueryIgnoreStatus.CampusSearch.DA_NANG_CAMPUS)
                {
                    users = users.Where(u => u.Campus == Campus.DA_NANG_CAMPUS);
                }
                else if (query.Campus == UserQueryIgnoreStatus.CampusSearch.HA_NOI_CAMPUS)
                {
                    users = users.Where(u => u.Campus == Campus.HA_NOI_CAMPUS);
                }
            }
            
            if (Enum.IsDefined(query.UserVerifyStatus))
            {
                if (query.UserVerifyStatus == UserQueryIgnoreStatus.UserVerifyStatusSearch.VERIFIED)
                {
                    users = users.Where(u => u.VerifyStatus == UserVerifyStatus.VERIFIED);
                }
                else if (query.UserVerifyStatus == UserQueryIgnoreStatus.UserVerifyStatusSearch.WAITING_VERIFIED)
                {
                    users = users.Where(u => u.VerifyStatus == UserVerifyStatus.WAITING_VERIFIED);
                }
                else if (query.UserVerifyStatus == UserQueryIgnoreStatus.UserVerifyStatusSearch.NOT_VERIFIED)
                {
                    users = users.Where(u => u.VerifyStatus == UserVerifyStatus.NOT_VERIFIED);
                }
            }
            
            if (Enum.IsDefined(query.UserStatus))
            {
                if (query.UserStatus == UserQueryIgnoreStatus.UserStatusSearch.ACTIVE)
                {
                    users = users.Where(u => u.IsActive == true);
                }
                else if (query.UserStatus == UserQueryIgnoreStatus.UserStatusSearch.INACTIVE)
                {
                    users = users.Where(u => u.IsActive == false);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                users = users.Where(p => p.FullName.ToLower().Contains(query.SearchText.ToLower()));
            }
            else if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                users = users.OrderBy(query.OrderBy);
            }

            return await Task.FromResult(users.ToList());
        }

        public Task<User> FindUserByID(string id)
        {
            return _context.Users
                .Include(u => u.UserMedias.Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null && um.MediaType != UserMediaType.AVATAR))
                .ThenInclude(um => um.Media)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<User> FindUserByEmail(string email)
        {
            return _context.Users
                .Include(u => u.UserMedias.Where(um => um.Media.IsActive == true && um.Media.DeletedDate == null && um.MediaType != UserMediaType.AVATAR))
                .ThenInclude(um => um.Media)
                .Where(u => u.IsActive == true )
                .Where(u => u.DeletedDate == null)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsDuplicatedEmail(string email)
        {

            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim()) != null;
        }

        public async Task<bool> IsDuplicatedPhoneNumber(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
                    u.Phone.ToLower().Trim() == phoneNumber.ToLower().Trim()) != null;
        }
    }
}
