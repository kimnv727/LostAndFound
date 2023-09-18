using Microsoft.EntityFrameworkCore;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Services.Implementations;
using LostAndFound.Infrastructure.Services.Interfaces;
using System;
using System.Linq;

namespace LostAndFound.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static void MapInitialData(this LostAndFoundDbContext context)
        {
            var testAcc = context.Users.FirstOrDefault(u => u.Email.Equals("abc123@fpt.edu.vn"));
            if(testAcc == null)
            {
                Role admin = new Role
                {
                    Name = "Admin",
                    Description = "Admin of the system. Capable of config system as well as manage Users.",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Roles.Add(admin);

                Role manager = new Role
                {
                    Name = "Manager",
                    Description = "Manager of the system. Managing data related to posts, items and users in the system.",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Roles.Add(manager);
                
                Role userRole = new Role
                {
                    Name = "User",
                    Description = "User of the system. For normal usage of the system like posting post and items.",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Roles.Add(userRole);

                User adminUser = new User()
                {
                    Id = "n8pJOw1SeoXexNsGwGCDq9GQ8SV2",
                    Email = "admin@fpt.edu.vn",
                    Password = "",
                    IsActive = true,
                    Avatar = "Avatar.png",
                    FirstName = "Test",
                    LastName = "Admin",
                    Gender = Core.Enums.Gender.Male,
                    Phone = "0101010101",
                    RoleId = 1,
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
                
                User user = new User()
                {
                    Id = "NOEOH77CAtd5VgEniFBLGGZz8sM2",
                    Email = "abc456@fpt.edu.vn",
                    Password = "",
                    IsActive = true,
                    Avatar = "Avatar.png",
                    FirstName = "Test",
                    LastName = "User",
                    Gender = Core.Enums.Gender.Male,
                    Phone = "0808080808",
                    RoleId = 3,
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Users.Add(user);
                context.SaveChanges();
                
                User managerUser = new User()
                {
                    Id = "FLtIEJvuMgfg58u4sXhzxPn9qr73",
                    Email = "abc123@fpt.edu.vn",
                    Password = "",
                    IsActive = true,
                    Avatar = "Avatar.png",
                    FirstName = "Test",
                    LastName = "Manager",
                    Gender = Core.Enums.Gender.Male,
                    Phone = "0909090909",
                    RoleId = 2,
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Users.Add(managerUser);
                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}
