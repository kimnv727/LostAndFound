using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LostAndFound.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static void MapInitialData(this LostAndFoundDbContext context)
        {
            //TEAMMATE_NOTE: please nuke the db and run the project again because I only check the very first acc
            //              if that acc exist the rest of the function will not run
            //              Besides, we use int on a few ids, so if you only nuke the data, index of those pk might get 
            //              messed up
            var testAcc = context.Users.FirstOrDefault(u => u.Email.Equals("abc123@fpt.edu.vn"));
            if(testAcc == null)
            {
                #region Add Roles
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
                
                Role storageManager = new Role
                {
                    Name = "Storage Manager",
                    Description = "Storage Manager of the system. Managing data related to posts, items, users and also managing system's storage in the system.",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Roles.Add(storageManager);

                Role userRole = new Role
                {
                    Name = "User",
                    Description = "User of the system. For normal usage of the system like posting post and items.",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Roles.Add(userRole);
                #endregion

                #region Add Properties
                Campus property = new Campus
                {
                    Name = "FPT HCM Campus",
                    Address = "FPT HCM Campus",
                };
                context.Campuses.Add(property);
                #endregion

                #region Add Users
                User adminUser = new User()
                {
                    Id = "n8pJOw1SeoXexNsGwGCDq9GQ8SV2",
                    Email = "admin@fpt.edu.vn",
                    Password = "",
                    IsActive = true,
                    FirstName = "Test",
                    LastName = "Admin",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0101010101",
                    SchoolId = "ADMIN",
                    Campus = CampusName.HO_CHI_MINH_CAMPUS,
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
                    FirstName = "Test",
                    LastName = "User",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.NOT_VERIFIED,
                    Phone = "0808080808",
                    SchoolId = "SE111111",
                    Campus = CampusName.HO_CHI_MINH_CAMPUS,
                    RoleId = 4,
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
                    FirstName = "Test",
                    LastName = "Manager",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0909090909",
                    SchoolId = "MANAGER",
                    Campus = CampusName.HO_CHI_MINH_CAMPUS,
                    RoleId = 2,
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Users.Add(managerUser);
                context.SaveChanges();

                User storageManagerUser = new User()
                {
                    Id = "UXFjz04VvING1SxKxcfIZQQhVCo1",
                    Email = "def123@fpt.edu.vn",
                    Password = "",
                    IsActive = true,
                    FirstName = "Test",
                    LastName = "Storage Manager",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0909090909",
                    SchoolId = "MANAGER",
                    Campus = CampusName.HO_CHI_MINH_CAMPUS,
                    RoleId = 3,
                    CreatedDate = DateTime.Now.ToVNTime()
                };

                #endregion

                #region Add Locations
                List<Location> locations = new List<Location>();

                    #region Floor Ground
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Back Gate", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Passio", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 004", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 005", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 006", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 007", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 008", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 009", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 010", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 011", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 012", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 013", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 014", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 015", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 016", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 017", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Copper Drum Lobby", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Main Gate", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 020", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 021", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 022", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 023", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 024", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Room 025", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "Eating Area", Floor = 0, IsActive = true });
                    locations.Add(new Location() { PropertyId = 1, LocationName = "7Eleven", Floor = 0, IsActive = true });
                    #endregion

                foreach (var l in locations)
                {
                    context.Locations.Add(l);
                    context.SaveChanges();
                }
                #endregion

                #region Add Category Groups
                //CategoryGroup electronics = new CategoryGroup()
                //{
                //    Name = "Electronics",
                //    Description="Laptop, Chargers, Phone and any technology related devices",
                //    Value = ItemValue.High, //TODO: This and one below should be inside the category instead of the group
                //                            // Please remove or remigrate this later
                //    IsSensitive = true,
                //};
                //context.CategoryGroups.Add(electronics);
                //context.SaveChanges();

                //CategoryGroup personalItems = new CategoryGroup()
                //{
                //    Name = "Personal Items",
                //    Description = "Wallet, Keys, Parking Ticket and so on that a person carry but not electronic devices",
                //    Value = ItemValue.High,
                //    IsSensitive = true,
                //};
                //context.CategoryGroups.Add(personalItems);
                //context.SaveChanges();
                #endregion

                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}
