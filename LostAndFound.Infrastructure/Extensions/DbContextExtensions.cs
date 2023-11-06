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

                List<UserMedia> userMedias1 = new List<UserMedia>();
                UserMedia userMedia1 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfUser1.png",
                        Description = "Avatar of User1 Account",
                        URL = "https://pbs.twimg.com/media/FuaR5ktaIAEYzQy?format=jpg&name=medium",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                userMedias1.Add(userMedia1);
                User user = new User()
                {
                    Id = "NOEOH77CAtd5VgEniFBLGGZz8sM2",
                    Email = "abc456@fpt.edu.vn",
                    Password = "",
                    Avatar= "https://pbs.twimg.com/media/FuaR5ktaIAEYzQy?format=jpg&name=medium",
                    IsActive = true,
                    FirstName = "Test",
                    LastName = "User",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.NOT_VERIFIED,
                    Phone = "0808080808",
                    SchoolId = "SE111111",
                    Campus = CampusName.HO_CHI_MINH_CAMPUS,
                    RoleId = 4,
                    CreatedDate = DateTime.Now.ToVNTime(),
                    UserMedias = userMedias1
                };
                context.Users.Add(user);
                context.SaveChanges();

                List<UserMedia> userMedias2 = new List<UserMedia>();
                UserMedia userMedia2 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfUser2.png",
                        Description = "Avatar of User2 Account",
                        URL = "https://pbs.twimg.com/media/F9nsg-2a0AA0WCc?format=jpg&name=large",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                UserMedia userMediaCCID2 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "CCIDOfUser2.png",
                        Description = "CCID of User2 Account",
                        URL = "https://pbs.twimg.com/media/F9FHQzNagAAy6NU?format=jpg&name=large",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD
                };
                UserMedia userMediaStudentCard2 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "StudentCardOfUser2.png",
                        Description = "Student Card of User2 Account",
                        URL = "https://pbs.twimg.com/media/F9FHQzObcAAgyyK?format=jpg&name=large",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.STUDENT_CARD
                };
                userMedias2.Add(userMedia2);
                userMedias2.Add(userMediaCCID2);
                userMedias2.Add(userMediaStudentCard2);
                User user2 = new User()
                {
                    Id = "eK9abWwxluYzbScdsQ8dx2zWnxD3",
                    Email = "aruurara@gmail.com",
                    Password = "",
                    Avatar = "https://pbs.twimg.com/media/F9nsg-2a0AA0WCc?format=jpg&name=large",
                    IsActive = true,
                    FirstName = "aruurara",
                    LastName = "あるうらら",
                    Gender = Gender.Female,
                    VerifyStatus = UserVerifyStatus.WAITING_VERIFIED,
                    Phone = "0909090909",
                    SchoolId = "SE121212",
                    Campus = CampusName.HO_CHI_MINH_CAMPUS,
                    RoleId = 4,
                    CreatedDate = DateTime.Now.ToVNTime(),
                    UserMedias = userMedias2
                };
                context.Users.Add(user2);
                context.SaveChanges();


                List<UserMedia> managerMedias = new List<UserMedia>();
                UserMedia managerMedia = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfManager.png",
                        Description = "Avatar of Manager Account",
                        URL = "https://pbs.twimg.com/media/F94XNNBWcAAnOqX?format=jpg&name=medium",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                managerMedias.Add(managerMedia);
                User managerUser = new User()
                {
                    Id = "FLtIEJvuMgfg58u4sXhzxPn9qr73",
                    Email = "abc123@fpt.edu.vn",
                    Password = "",
                    Avatar = "https://pbs.twimg.com/media/F94XNNBWcAAnOqX?format=jpg&name=medium",
                    IsActive = true,
                    FirstName = "Test",
                    LastName = "Manager",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0909090909",
                    SchoolId = "MANAGER",
                    Campus = CampusName.HO_CHI_MINH_CAMPUS,
                    RoleId = 2,
                    CreatedDate = DateTime.Now.ToVNTime(),
                    UserMedias = managerMedias,
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
                context.Users.Add(storageManagerUser);
                context.SaveChanges();

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
                CategoryGroup electronicsDevices = new CategoryGroup()
                {
                    Name = "Electronics Devices",
                    Description = "High value electrionic devices such as phones, tablets, laptops,...",
                };
                context.CategoryGroups.Add(electronicsDevices);
                context.SaveChanges();

                CategoryGroup electronicsAccessories = new CategoryGroup()
                {
                    Name = "Electronics Accessories",
                    Description = "Accessories to use with electronic devices like phone case, mousepad,...",
                };
                context.CategoryGroups.Add(electronicsAccessories);
                context.SaveChanges();

                CategoryGroup key = new CategoryGroup()
                {
                    Name = "Keys",
                    Description = "Car keys, house keys, and so on",
                };
                context.CategoryGroups.Add(key);
                context.SaveChanges();

                CategoryGroup peronsalAccessories = new CategoryGroup()
                {
                    Name = "Personal Accessories",
                    Description = "Accessories people wear eveeryday like chain, necklace, bracelet,...",
                };
                context.CategoryGroups.Add(peronsalAccessories);
                context.SaveChanges();

                CategoryGroup idCard = new CategoryGroup()
                {
                    Name = "ID Card",
                    Description = "Contain Identify information could be Student Card, CCID, Driver Liscense,...",
                };
                context.CategoryGroups.Add(idCard);
                context.SaveChanges();

                CategoryGroup clothing = new CategoryGroup()
                {
                    Name = "Clothes",
                    Description = "Clothes, belts and the likes",
                };
                context.CategoryGroups.Add(clothing);
                context.SaveChanges();
                #endregion

                #region Add Categories
                Category laptop = new Category()
                {
                    Name = "Laptops",
                    Description = "On top of your lap",
                    IsSensitive = true,
                    Value = ItemValue.High,
                    CategoryGroupId = 1,
                };
                context.Categories.Add(laptop);
                context.SaveChanges();

                Category camera = new Category()
                {
                    Name = "Camera",
                    Description = "Records and take pictures",
                    IsSensitive = true,
                    Value = ItemValue.High,
                    CategoryGroupId = 1,
                };
                context.Categories.Add(camera);
                context.SaveChanges();

                Category phone = new Category()
                {
                    Name = "Phones",
                    Description = "Smartphone, feature phones alike",
                    IsSensitive = true,
                    Value = ItemValue.High,
                    CategoryGroupId = 1,
                };
                context.Categories.Add(phone);
                context.SaveChanges();

                Category wallets = new Category()
                {
                    Name = "Wallets",
                    Description = "Can contain money, and various identifier items",
                    IsSensitive = true,
                    Value = ItemValue.High,
                    CategoryGroupId = 4,
                };
                context.Categories.Add(wallets);
                context.SaveChanges();

                Category glasses = new Category()
                {
                    Name = "Glasses",
                    Description = "Glasses that you wear init?",
                    IsSensitive = false,
                    Value = ItemValue.Low,
                    CategoryGroupId = 4,
                };
                context.Categories.Add(glasses);
                context.SaveChanges();

                Category charger = new Category()
                {
                    Name = "Charger",
                    Description = "Charger for various things like laptop, phone, smartwatch",
                    IsSensitive = false,
                    Value = ItemValue.Low,
                    CategoryGroupId = 2,
                };
                context.Categories.Add(charger);
                context.SaveChanges();

                Category headphone = new Category()
                {
                    Name = "Headphones & Earphones",
                    Description = "Headphones & Earphones (Wired & Wireless)",
                    IsSensitive = false,
                    Value = ItemValue.High,
                    CategoryGroupId = 2,
                };
                context.Categories.Add(headphone);
                context.SaveChanges();

                Category keyboards = new Category()
                {
                    Name = "Keyboards",
                    Description = "Membrance keyboard, mechanical keyboards and more",
                    IsSensitive = false,
                    Value = ItemValue.High,
                    CategoryGroupId = 2,
                };
                context.Categories.Add(keyboards);
                context.SaveChanges();

                Category mouses = new Category()
                {
                    Name = "Mouses",
                    Description = "Mouses for PC and that",
                    IsSensitive = false,
                    Value = ItemValue.High,
                    CategoryGroupId = 2,
                };
                context.Categories.Add(mouses);
                context.SaveChanges();

                Category driverLiscense = new Category()
                {
                    Name = "Driver License",
                    Description = "License for motorbike, car, helicopter, planes, aircraft carriers,...",
                    IsSensitive = true,
                    Value = ItemValue.Low,
                    CategoryGroupId = 5,
                };
                context.Categories.Add(driverLiscense);
                context.SaveChanges();

                Category ccid = new Category()
                {
                    Name = "Ciziten Identity Card",
                    Description = "Citizen Identity Card (CCID)",
                    IsSensitive = true,
                    Value = ItemValue.Low,
                    CategoryGroupId = 5,
                };
                context.Categories.Add(ccid);
                context.SaveChanges();

                Category studentCard = new Category()
                {
                    Name = "Student Card",
                    Description = "FPTU Student Card",
                    IsSensitive = true,
                    Value = ItemValue.Low,
                    CategoryGroupId = 5,
                };
                context.Categories.Add(studentCard);
                context.SaveChanges();

                Category underwear = new Category()
                {
                    Name = "Underwear",
                    Description = "Okay bro, but how?",
                    IsSensitive = true,
                    Value = ItemValue.Low,
                    CategoryGroupId = 6,
                };
                context.Categories.Add(underwear);
                context.SaveChanges();

                Category swimsuit = new Category()
                {
                    Name = "Swimsuit",
                    Description = "You swim in lotus lake or something? Why is this here?",
                    IsSensitive = true,
                    Value = ItemValue.Low,
                    CategoryGroupId = 6,
                };
                context.Categories.Add(swimsuit);
                context.SaveChanges();
                #endregion

                #region Add Items

                    #region Item 1
                    List<ItemMedia> medias1 = new List<ItemMedia>();
                    ItemMedia itemMedia1 = new ItemMedia()
                    {
                        Media = new Media()
                        {
                            Name = "Item1.png",
                            Description="Item 1 Image",
                            URL= "https://pbs.twimg.com/media/F2Gk22ZbMAAL3qf?format=jpg&name=large",
                            CreatedDate = DateTime.Now.ToVNTime(),
                        }
                    };
                    ItemMedia itemMedia2nd1 = new ItemMedia()
                    {
                        Media = new Media()
                        {
                            Name = "Item1.png",
                            Description = "Item 1 2nd Image",
                            URL = "https://pbs.twimg.com/media/F63opCAaMAA1RCN?format=jpg&name=large",
                            CreatedDate = DateTime.Now.ToVNTime(),
                        }
                    };
                    medias1.Add(itemMedia2nd1);

                    Item item1 = new Item()
                    {
                        Name = "Macbook Air",
                        Description = "I lost my macbook air please help me find it",
                        LocationId = 1,
                        CategoryId = 1,
                        CreatedDate = DateTime.Now.ToVNTime(),
                        FoundUserId = "NOEOH77CAtd5VgEniFBLGGZz8sM2",
                        ItemStatus = ItemStatus.PENDING,
                        IsInStorage = false,
                        FoundDate = DateTime.Now.ToVNTime(),
                        ItemMedias = medias1,
                    };
                    context.Items.Add(item1);
                    context.SaveChanges();
                    #endregion

                    #region Item 2
                    List<ItemMedia> medias2 = new List<ItemMedia>();
                    ItemMedia itemMedia2 = new ItemMedia()
                    {
                        Media = new Media()
                        {
                            Name = "Item2.png",
                            Description = "Item 2 Image",
                            URL = "https://pbs.twimg.com/media/F0Hyt75aAAAm6IC?format=jpg&name=large",
                            CreatedDate = DateTime.Now.ToVNTime(),
                        }
                    };
                    medias2.Add(itemMedia2);

                    Item item2 = new Item()
                    {
                        Name = "Canon DSLR",
                        Description = "Help me find my DSLR, some roadman stole it from me and yeeted it somewhere",
                        LocationId = 26,
                        CategoryId = 2,
                        CreatedDate = DateTime.Now.ToVNTime(),
                        FoundUserId = "NOEOH77CAtd5VgEniFBLGGZz8sM2",
                        ItemStatus = ItemStatus.ACTIVE,
                        IsInStorage = false,
                        FoundDate = DateTime.Now.ToVNTime(),
                        ItemMedias = medias2,
                    };
                    context.Items.Add(item2);
                    context.SaveChanges();
                #endregion

                #endregion

                #region Add Storages
                Storage storage1 = new Storage
                {
                    CampusId = 1,
                    Location = "Copper Drum Lobby",
                    MainStorageManagerId = "UXFjz04VvING1SxKxcfIZQQhVCo1"
                };
                context.Storages.Add(storage1);
                context.SaveChanges();
                Storage storage2 = new Storage
                {
                    CampusId = 1,
                    Location = "Back Gate",
                    MainStorageManagerId = "UXFjz04VvING1SxKxcfIZQQhVCo1"
                };
                context.Storages.Add(storage2);
                context.SaveChanges();
                #endregion

                #region Add Cabinets
                List<Cabinet> cabinets1 = new List<Cabinet>();
                cabinets1.Add(new Cabinet() { Name = "A1", StorageId = 1});
                cabinets1.Add(new Cabinet() { Name = "A2", StorageId = 1 });
                cabinets1.Add(new Cabinet() { Name = "A3", StorageId = 1 });
                cabinets1.Add(new Cabinet() { Name = "A4", StorageId = 1 });
                cabinets1.Add(new Cabinet() { Name = "A5", StorageId = 1 });
                cabinets1.Add(new Cabinet() { Name = "A6", StorageId = 1 });
                cabinets1.Add(new Cabinet() { Name = "A7", StorageId = 1 });
                cabinets1.Add(new Cabinet() { Name = "A8", StorageId = 1 });
                cabinets1.Add(new Cabinet() { Name = "A9", StorageId = 1 });
                cabinets1.Add(new Cabinet() { Name = "A10", StorageId = 1 });
                foreach (var c in cabinets1)
                {
                    context.Cabinets.Add(c);
                    context.SaveChanges();
                }

                List<Cabinet> cabinets2 = new List<Cabinet>();
                cabinets2.Add(new Cabinet() { Name = "B1", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B2", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B3", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B4", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B5", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B6", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B7", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B8", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B9", StorageId = 2 });
                cabinets2.Add(new Cabinet() { Name = "B10", StorageId = 2 });
                foreach (var c in cabinets2)
                {
                    context.Cabinets.Add(c);
                    context.SaveChanges();
                }
                #endregion

                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}
