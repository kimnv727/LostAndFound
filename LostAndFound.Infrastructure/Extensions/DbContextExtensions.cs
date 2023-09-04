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
                    Id = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857720"),
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

                User user = new User()
                {
                    Id = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),
                    Email = "abc123@fpt.edu.vn",
                    Password = "",
                    IsActive = true,
                    Avatar = "Avatar.png",
                    FirstName = "Test",
                    LastName = "User",
                    Gender = Core.Enums.Gender.Male,
                    Phone = "0909090909",
                    Role = new Role
                    {
                        Name = "User",
                        Description = "User of the system. For normal usage of the system like posting post and items.",
                        CreatedDate = DateTime.Now.ToVNTime()

                    },
                    FirebaseUID = "IwZdGow330VpjeBFnrIm7T5H0262",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Users.Add(user);
                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}
