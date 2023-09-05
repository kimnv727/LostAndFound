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
        
        public async Task<IEnumerable<User>> QueryAsync(UserQuery query, bool trackChanges = false)
        {
            IQueryable<User> users = _context.Users.AsSplitQuery();

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
            
            if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
            {
                users = users.Where(u => u.Phone.ToLower().Contains(query.PhoneNumber.ToLower()));
            }
            
            if (!string.IsNullOrWhiteSpace(query.Avatar))
            {
                users = users.Where(u => u.Avatar.ToLower().Contains(query.Avatar.ToLower()));
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
            return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public Task<User> FindUserByEmail(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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
