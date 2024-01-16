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
                    Name = "Member",
                    Description = "Member of the system. For normal usage of the system like posting post and items.",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Roles.Add(userRole);
                #endregion

                #region Add Campuses
                Campus campus = new Campus
                {
                    Name = "FPT Ho Chi Minh Campus",
                    Address = "FPT Ho Chi Minh Campus",
                    //CampusLocation = CampusLocation.HO_CHI_MINH
                };
                context.Campuses.Add(campus);

                Campus campus2 = new Campus
                {
                    Name = "FPT Da Nang Campus",
                    Address = "FPT Da Nang Campus",
                    //CampusLocation = CampusLocation.DA_NANG
                };
                context.Campuses.Add(campus2);

                Campus campus3 = new Campus
                {
                    Name = "FPT Ha Noi Campus",
                    Address = "FPT Ha Noi Campus",
                    //CampusLocation = CampusLocation.HA_NOI
                };
                context.Campuses.Add(campus3);

                Campus campus4 = new Campus
                {
                    Name = "FPT Can tho Campus",
                    Address = "FPT Can tho Campus",
                    //CampusLocation = CampusLocation.CAN_THO
                };
                context.Campuses.Add(campus4);

                /*Campus campus5 = new Campus
                {
                    Name = "FPT HCM Nha Van Hoa",
                    Address = "FPT HCM Nha Van Hoa",
                    CampusLocation = CampusLocation.HO_CHI_MINH
                };
                context.Campuses.Add(campus5);*/
                #endregion

                #region Add Users

                    #region Admin
                List<UserMedia> userMedias1 = new List<UserMedia>();
                UserMedia userMedia1 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfAdmin.png",
                        Description = "Avatar of Admin Account",
                        URL = "https://files.catbox.moe/05jiq7.png?fbclid=IwAR0d2x-q19sCGIvjAoZVqwt5xzEtHe72ONjqWTs-RkLdTx4fEI2ERIL7oOM",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                userMedias1.Add(userMedia1);

                User adminUser = new User()
                {
                    Id = "G3KbI9nWz2gRoWRwsJc9r8jErbT2",
                    Email = "abc123@fpt.edu.vn",
                    Password = "",
                    Avatar= "https://files.catbox.moe/05jiq7.png?fbclid=IwAR0d2x-q19sCGIvjAoZVqwt5xzEtHe72ONjqWTs-RkLdTx4fEI2ERIL7oOM",
                    IsActive = true,
                    FirstName = "Admin",
                    LastName = "Account",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0101010101",
                    SchoolId = "ADMIN",
                    CampusId = 1,
                    RoleId = 1,
                    CreatedDate = new DateTime(2023, 1, 1),
                    UserMedias = userMedias1
                };
                context.Users.Add(adminUser);
                context.SaveChanges();
                #endregion

                    #region Manager
                List<UserMedia> userMedias2 = new List<UserMedia>();
                UserMedia userMedia2 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfManager1.png",
                        Description = "Avatar of Manager1 Account",
                        URL = "https://pbs.twimg.com/media/FuaR5ktaIAEYzQy?format=jpg&name=medium",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                UserMedia managerMediaCCID1 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "FrontCCIDOfManager1.png",
                        Description = "Front CCID of Manager Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD_FRONT
                };
                UserMedia managerMediaCCID2 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "BackCCIDOfManager2.png",
                        Description = "Back CCID of Manager Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD_BACK
                };
                UserMedia managerMediaStudentCard = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "StudentCardOfManager2.png",
                        Description = "Student Card of Manager Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.STUDENT_CARD
                };
                userMedias2.Add(userMedia2);
                userMedias2.Add(managerMediaCCID1);
                userMedias2.Add(managerMediaCCID2);
                userMedias2.Add(managerMediaStudentCard);

                User user = new User()
                {
                    Id = "iTOIMxgSC5Vq74EZMMDJrLtncoH2",
                    Email = "gglcolab002@gmail.com",
                    Password = "",
                    Avatar= "https://pbs.twimg.com/media/FuaR5ktaIAEYzQy?format=jpg&name=medium",
                    IsActive = true,
                    FirstName = "Manager",
                    LastName = "Account",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0808080808",
                    SchoolId = "Manager1",
                    CampusId = 1,
                    RoleId = 2,
                    CreatedDate = new DateTime(2023, 1, 1),
                    UserMedias = userMedias2
                };
                context.Users.Add(user);
                context.SaveChanges();
                #endregion

                    #region Storage Manager
                List<UserMedia> userMedias3 = new List<UserMedia>();
                UserMedia userMedia3 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfStorageManager1.png",
                        Description = "Avatar of Storage Manager1 Account",
                        URL = "https://pbs.twimg.com/profile_images/1091963888021602305/9440xgqT_400x400.jpg",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                UserMedia managerMediaCCID3 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "FrontCCIDOfStorageManager1.png",
                        Description = "Front CCID of Storage Manager Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD_FRONT
                };
                UserMedia managerMediaCCID4 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "BackCCIDOfStorageManager2.png",
                        Description = "Back CCID of Storage Manager Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD_BACK
                };
                UserMedia managerMediaStudentCard2 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "StudentCardOfManager2.png",
                        Description = "Student Card of Manager Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = new DateTime(2023, 1, 1),
                    },
                    MediaType = UserMediaType.STUDENT_CARD
                };
                userMedias3.Add(userMedia3);
                userMedias3.Add(managerMediaCCID3);
                userMedias3.Add(managerMediaCCID4);
                userMedias3.Add(managerMediaStudentCard2);

                User user2 = new User()
                {
                    Id = "jDlpStSQWiQsG2F7yDEqOTGka0x2",
                    Email = "gglcolab001@gmail.com",
                    Password = "",
                    Avatar = "https://pbs.twimg.com/profile_images/1091963888021602305/9440xgqT_400x400.jpg",
                    IsActive = true,
                    FirstName = "Storage Manager",
                    LastName = "Account",
                    Gender = Gender.Female,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0909090909",
                    SchoolId = "StorageManager",
                    CampusId = 1,
                    RoleId = 3,
                    CreatedDate = new DateTime(2023, 1, 1),
                    UserMedias = userMedias3
                };
                context.Users.Add(user2);
                context.SaveChanges();
                #endregion

                    #region Member - Kim fpt
                List<UserMedia> userMedias4 = new List<UserMedia>();
                UserMedia userMedia4 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfUser1.png",
                        Description = "Avatar of User 1 Account",
                        URL = "https://pbs.twimg.com/media/F_-LOp5bAAAaR_O?format=jpg&name=large",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                UserMedia userMediaCCID5 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "FrontCCIDOfUser1.png",
                        Description = "Front CCID of User Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD_FRONT
                };
                UserMedia userMediaCCID6 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "BackCCIDOfUser2.png",
                        Description = "Back CCID of User Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD_BACK
                };
                UserMedia userMediaStudent7 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "StudentCardOfUser.png",
                        Description = "Back CCID of User Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.STUDENT_CARD
                };
                userMedias4.Add(userMedia4);
                userMedias4.Add(userMediaCCID5);
                userMedias4.Add(userMediaCCID6);
                userMedias4.Add(userMediaStudent7);

                User user3 = new User()
                {
                    Id = "LHFJkI0EzeN1pnfkfFuScgNvixj1",
                    Email = "kimnvse150529@fpt.edu.vn",
                    Password = "",
                    Avatar = "https://pbs.twimg.com/media/F_-LOp5bAAAaR_O?format=jpg&name=large",
                    IsActive = true,
                    FirstName = "Nguyen Van Kim",
                    LastName = "(K15 HCM)",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0909090909",
                    SchoolId = "USER",
                    CampusId = 1,
                    RoleId = 4,
                    CreatedDate = DateTime.Now.ToVNTime(),
                    UserMedias = userMedias4
                };
                context.Users.Add(user3);
                context.SaveChanges();
                #endregion

                    #region Member - CongThanh (for deny verified)
                List<UserMedia> userMediasCT = new List<UserMedia>();
                UserMedia userMediaCT = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfCT.png",
                        Description = "Avatar of CT Account",
                        URL = "https://lh3.googleusercontent.com/a/ACg8ocKPaqlljRt6bLPk-9rwcyVAoKAPc7znmNKt6ErIpM5w=s96-c",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                UserMedia userMediaCCIDCT = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "FrontCCIDOfCT.png",
                        Description = "Front CCID of CT Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD_FRONT
                };
                UserMedia userMediaCCIDCT2 = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "BackCCIDOfCT.png",
                        Description = "Back CCID of CT Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.IDENTIFICATION_CARD_BACK
                };
                UserMedia userMediaStudentCT = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "StudentCardOfCT.png",
                        Description = "Back CCID of CT Account",
                        URL = "https://images2.thanhnien.vn/528068263637045248/2023/11/27/doi-ten-the-can-cuoc-1701048226664995346108.jpg?fbclid=IwAR0r3WvuGg5qP_xfg4syETEinAGryrvWyxeRXGIYE4WUajJQoOeMOGMFfm4",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    },
                    MediaType = UserMediaType.STUDENT_CARD
                };
                userMediasCT.Add(userMediaCT);
                userMediasCT.Add(userMediaCCIDCT);
                userMediasCT.Add(userMediaCCIDCT2);
                userMediasCT.Add(userMediaStudentCT);

                User userCT = new User()
                {
                    Id = "EHJGxqTLgVaaFW3BTsE3BI0lIgm2",
                    Email = "ct4862101@gmail.com",
                    Password = "",
                    Avatar = "https://lh3.googleusercontent.com/a/ACg8ocKPaqlljRt6bLPk-9rwcyVAoKAPc7znmNKt6ErIpM5w=s96-c",
                    IsActive = true,
                    FirstName = "Cong",
                    LastName = "Thanh",
                    Gender = Gender.Female,
                    VerifyStatus = UserVerifyStatus.WAITING_VERIFIED,
                    Phone = "0101010101",
                    SchoolId = "CT123456",
                    CampusId = 1,
                    RoleId = 4,
                    CreatedDate = new DateTime(2024, 1, 10),
                    UserMedias = userMediasCT
                };
                context.Users.Add(userCT);
                context.SaveChanges();
                #endregion

                    #region Member - Bao fpt
                List<UserMedia> userMediasBao = new List<UserMedia>();
                UserMedia userMediaBao = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfBao.png",
                        Description = "Avatar of Bao Account",
                        URL = "https://lh3.googleusercontent.com/a/ACg8ocK19tliPY9j0kh7IRX4FJ_D-FgOGgUIE2Z-Vl_ihK4S=s96-c",
                        CreatedDate = new DateTime(2023, 12, 15),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                userMediasBao.Add(userMediaBao);

                User userBao = new User()
                {
                    Id = "2CPF2ND0KGXalizaooAJLhY8CPl2",
                    Email = "baongse150657@fpt.edu.vn",
                    Password = "",
                    Avatar = "https://lh3.googleusercontent.com/a/ACg8ocK19tliPY9j0kh7IRX4FJ_D-FgOGgUIE2Z-Vl_ihK4S=s96-c",
                    IsActive = true,
                    FirstName = "Nguyen Gia Bao",
                    LastName = "(K15 HCM)",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0934979633",
                    SchoolId = "SE150657",
                    CampusId = 1,
                    RoleId = 4,
                    CreatedDate = new DateTime(2023, 12, 15),
                    UserMedias = userMediasBao
                };
                context.Users.Add(userBao);
                context.SaveChanges();
                #endregion

                    #region Member - Duc fpt
                List<UserMedia> userMediasDuc = new List<UserMedia>();
                UserMedia userMediaDuc = new UserMedia()
                {
                    Media = new Media()
                    {
                        Name = "AvatarOfDuc.png",
                        Description = "Avatar of Duc Account",
                        URL = "https://lh3.googleusercontent.com/a/ACg8ocLx7B9iEfEgeNY_bU-SLnsxQXS620nJoR_T90OSeraA=s96-c",
                        CreatedDate = new DateTime(2023, 10, 17),
                    },
                    MediaType = UserMediaType.AVATAR
                };
                userMediasDuc.Add(userMediaDuc);

                User userDuc = new User()
                {
                    Id = "nY5n19jbQIX5ncSA1UiwpNFiMXh1",
                    Email = "duchmde150269@fpt.edu.vn",
                    Password = "",
                    Avatar = "https://lh3.googleusercontent.com/a/ACg8ocLx7B9iEfEgeNY_bU-SLnsxQXS620nJoR_T90OSeraA=s96-c",
                    IsActive = true,
                    FirstName = "Huynh Minh Duc",
                    LastName = "(K15 DN)",
                    Gender = Gender.Male,
                    VerifyStatus = UserVerifyStatus.VERIFIED,
                    Phone = "0908777888",
                    SchoolId = "DE150269",
                    CampusId = 1,
                    RoleId = 4,
                    CreatedDate = new DateTime(2023, 10, 17),
                    UserMedias = userMediasDuc
                };
                context.Users.Add(userDuc);
                context.SaveChanges();
                #endregion

                #endregion

                #region Add Locations
                List<Location> locations = new List<Location>();

                    #region Floor Ground
                    locations.Add(new Location() { CampusId = 1, LocationName = "Back Gate", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Passio", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 004", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 005", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 006", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 007", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 008", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 009", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 010", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 011", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 012", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 013", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 014", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 015", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 016", Floor = 0, IsActive = true }); 
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 017", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Copper Drum Lobby", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Main Gate", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 020", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 021", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 022", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 023", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 024", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 025", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Eating Area", Floor = 0, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "7Eleven", Floor = 0, IsActive = true });
                    #endregion

                    #region Floor 1
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 101", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 102", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 103", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 104", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 105", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 106", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 107", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 108", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 109", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 110", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 111", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 112", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 113", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 114", Floor = 1, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 115", Floor = 1, IsActive = true });
                    #endregion

                    #region Floor 2
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 201", Floor = 2, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 202", Floor = 2, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 203", Floor = 2, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 204", Floor = 2, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 205", Floor = 2, IsActive = true });
                    #endregion

                    #region Floor 3
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 301", Floor = 3, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 302", Floor = 3, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 303", Floor = 3, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 304", Floor = 3, IsActive = true });
                    locations.Add(new Location() { CampusId = 1, LocationName = "Room 305", Floor = 3, IsActive = true });
                    #endregion

                    #region NVH
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 506 (NVH)", Floor = 5, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 507 (NVH)", Floor = 5, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 508 (NVH)", Floor = 5, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 509 (NVH)", Floor = 5, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 510 (NVH)", Floor = 5, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 511 (NVH)", Floor = 5, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 512 (NVH)", Floor = 5, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 606 (NVH)", Floor = 6, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 607 (NVH)", Floor = 6, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 608 (NVH)", Floor = 6, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 609 (NVH)", Floor = 6, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 610 (NVH)", Floor = 6, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 611 (NVH)", Floor = 6, IsActive = true });
                locations.Add(new Location() { CampusId = 1, LocationName = "Room 612 (NVH)", Floor = 6, IsActive = true });
                #endregion

                foreach (var l in locations)
                {
                    context.Locations.Add(l);
                    context.SaveChanges();
                }
                #endregion

                #region Add Locations 2
                List<Location> locations2 = new List<Location>();

                #region Floor Ground
                locations2.Add(new Location() { CampusId = 2, LocationName = "Back Gate", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Passio", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 004", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 005", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 006", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 007", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 008", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 009", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 010", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 011", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 012", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 013", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 014", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 015", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 016", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 017", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Copper Drum Lobby", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Main Gate", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 020", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 021", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 022", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 023", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 024", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 025", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Eating Area", Floor = 0, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "7Eleven", Floor = 0, IsActive = true });
                #endregion

                #region Floor 1
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 101", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 102", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 103", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 104", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 105", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 106", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 107", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 108", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 109", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 110", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 111", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 112", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 113", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 114", Floor = 1, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 115", Floor = 1, IsActive = true });
                #endregion

                #region Floor 2
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 201", Floor = 2, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 202", Floor = 2, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 203", Floor = 2, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 204", Floor = 2, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 205", Floor = 2, IsActive = true });
                #endregion

                #region Floor 3
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 301", Floor = 3, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 302", Floor = 3, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 303", Floor = 3, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 304", Floor = 3, IsActive = true });
                locations2.Add(new Location() { CampusId = 2, LocationName = "Room 305", Floor = 3, IsActive = true });
                #endregion

                foreach (var l in locations2)
                {
                    context.Locations.Add(l);
                    context.SaveChanges();
                }
                #endregion

                #region Add Locations 3
                List<Location> locations3 = new List<Location>();

                #region Floor Ground
                locations3.Add(new Location() { CampusId = 3, LocationName = "Back Gate", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Passio", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 004", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 005", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 006", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 007", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 008", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 009", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 010", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 011", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 012", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 013", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 014", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 015", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 016", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 017", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Copper Drum Lobby", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Main Gate", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 020", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 021", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 022", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 023", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 024", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 025", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Eating Area", Floor = 0, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "7Eleven", Floor = 0, IsActive = true });
                #endregion

                #region Floor 1
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 101", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 102", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 103", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 104", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 105", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 106", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 107", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 108", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 109", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 110", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 111", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 112", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 113", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 114", Floor = 1, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 115", Floor = 1, IsActive = true });
                #endregion

                #region Floor 2
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 201", Floor = 2, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 202", Floor = 2, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 203", Floor = 2, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 204", Floor = 2, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 205", Floor = 2, IsActive = true });
                #endregion

                #region Floor 3
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 301", Floor = 3, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 302", Floor = 3, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 303", Floor = 3, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 304", Floor = 3, IsActive = true });
                locations3.Add(new Location() { CampusId = 3, LocationName = "Room 305", Floor = 3, IsActive = true });
                #endregion

                foreach (var l in locations3)
                {
                    context.Locations.Add(l);
                    context.SaveChanges();
                }
                #endregion

                #region Add Locations 4
                List<Location> locations4 = new List<Location>();

                #region Floor Ground
                locations4.Add(new Location() { CampusId = 4, LocationName = "Back Gate", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Passio", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 004", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 005", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 006", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 007", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 008", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 009", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 010", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 011", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 012", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 013", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 014", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 015", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 016", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 017", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Copper Drum Lobby", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Main Gate", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 020", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 021", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 022", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 023", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 024", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 025", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Eating Area", Floor = 0, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "7Eleven", Floor = 0, IsActive = true });
                #endregion

                #region Floor 1
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 101", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 102", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 103", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 104", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 105", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 106", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 107", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 108", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 109", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 110", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 111", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 112", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 113", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 114", Floor = 1, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 115", Floor = 1, IsActive = true });
                #endregion

                #region Floor 2
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 201", Floor = 2, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 202", Floor = 2, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 203", Floor = 2, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 204", Floor = 2, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 205", Floor = 2, IsActive = true });
                #endregion

                #region Floor 3
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 301", Floor = 3, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 302", Floor = 3, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 303", Floor = 3, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 304", Floor = 3, IsActive = true });
                locations4.Add(new Location() { CampusId = 4, LocationName = "Room 305", Floor = 3, IsActive = true });
                #endregion

                foreach (var l in locations4)
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

                CategoryGroup personalBelonging = new CategoryGroup()
                {
                    Name = "Personal Belonging",
                    Description = "Bottle of Water, Meal and the likes",
                };
                context.CategoryGroups.Add(personalBelonging);
                context.SaveChanges();
                #endregion

                #region Add Categories
                Category laptop = new Category()
                {
                    Name = "Laptop",
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
                    Name = "Phone",
                    Description = "Smartphone, feature phones alike",
                    IsSensitive = true,
                    Value = ItemValue.High,
                    CategoryGroupId = 1,
                };
                context.Categories.Add(phone);
                context.SaveChanges();

                Category wallets = new Category()
                {
                    Name = "Wallet",
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
                    Name = "Keyboard",
                    Description = "Membrance keyboard, mechanical keyboards and more",
                    IsSensitive = false,
                    Value = ItemValue.High,
                    CategoryGroupId = 2,
                };
                context.Categories.Add(keyboards);
                context.SaveChanges();

                Category mouses = new Category()
                {
                    Name = "Mouse",
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

                Category bottleOfWater = new Category()
                {
                    Name = "Bottle of Water",
                    Description = "Bottle of Water",
                    IsSensitive = false,
                    Value = ItemValue.Low,
                    CategoryGroupId = 7,
                };
                context.Categories.Add(bottleOfWater);
                context.SaveChanges();

                Category helmet = new Category()
                {
                    Name = "Helmet",
                    Description = "Helmet",
                    IsSensitive = false,
                    Value = ItemValue.Low,
                    CategoryGroupId = 7,
                };
                context.Categories.Add(helmet);
                context.SaveChanges();

                Category keychain = new Category()
                {
                    Name = "Key Chain",
                    Description = "Key Chain",
                    IsSensitive = false,
                    Value = ItemValue.Low,
                    CategoryGroupId = 4,
                };
                context.Categories.Add(keychain);
                context.SaveChanges();
                #endregion

                #region Add Storages
                Storage storage1 = new Storage
                {
                    CampusId = 1,
                    Location = "Copper Drum Lobby",
                    MainStorageManagerId = "jDlpStSQWiQsG2F7yDEqOTGka0x2",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Storages.Add(storage1);
                context.SaveChanges();
                Storage storage2 = new Storage
                {
                    CampusId = 1,
                    Location = "Back Gate",
                    MainStorageManagerId = "jDlpStSQWiQsG2F7yDEqOTGka0x2",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Storages.Add(storage2);
                context.SaveChanges();
                #endregion

                #region Add Cabinets
                List<Cabinet> cabinets1 = new List<Cabinet>();
                cabinets1.Add(new Cabinet() { Name = "A1", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A2", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A3", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A4", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A5", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A6", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A7", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A8", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A9", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets1.Add(new Cabinet() { Name = "A10", StorageId = 1, CreatedDate = DateTime.Now.ToVNTime() });
                foreach (var c in cabinets1)
                {
                    context.Cabinets.Add(c);
                    context.SaveChanges();
                }

                List<Cabinet> cabinets2 = new List<Cabinet>();
                cabinets2.Add(new Cabinet() { Name = "B1", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B2", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B3", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B4", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B5", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B6", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B7", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B8", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B9", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                cabinets2.Add(new Cabinet() { Name = "B10", StorageId = 2, CreatedDate = DateTime.Now.ToVNTime() });
                foreach (var c in cabinets2)
                {
                    context.Cabinets.Add(c);
                    context.SaveChanges();
                }
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
                            URL= "https://haloshop.vn/image/cache/catalog/products/apple/macbook/macbook-pro-2020-13-inch-chip-m1-gray-00-700x700.jpg",
                            CreatedDate = new DateTime(2023, 10, 11),
                        }
                    };
                    ItemMedia itemMedia2nd1 = new ItemMedia()
                    {
                        Media = new Media()
                        {
                            Name = "Item1.png",
                            Description = "Item 1 2nd Image",
                            URL = "https://cdn.tgdd.vn/Products/Images/44/231244/grey-1-org.jpg",
                            CreatedDate = new DateTime(2023, 10, 11),
                        }
                    };
                    medias1.Add(itemMedia1);
                    medias1.Add(itemMedia2nd1);

                    Item item1 = new Item()
                    {
                        Name = "Macbook Air",
                        Description = "Minh co tim thay mot macbook air mau trang sau gio hoc slot 1",
                        LocationId = 8,
                        CategoryId = 1,
                        CreatedDate = new DateTime(2023, 10, 11),
                        FoundUserId = "LHFJkI0EzeN1pnfkfFuScgNvixj1",
                        ItemStatus = ItemStatus.PENDING,
                        IsInStorage = false,
                        //TODO: fix
                        FoundDate = "2023-10-10|Slot 1",
                        //FoundDate = DateTime.Now.ToVNTime(),
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
                            URL = "https://zshop.vn/blogs/wp-content/uploads/2022/07/Canon_EOS_R7_hands-on_angled_hands-768x576.jpeg",
                            CreatedDate = new DateTime(2023, 11, 11),
                        }
                    };
                    medias2.Add(itemMedia2);

                    Item item2 = new Item()
                    {
                        Name = "Canon DSLR",
                        Description = "Minh co lum duoc mot chiec DSLR sau hoat dong cau lac bo.",
                        LocationId = 26,
                        CategoryId = 2,
                        CreatedDate = new DateTime(2023, 11, 11),
                        FoundUserId = "jDlpStSQWiQsG2F7yDEqOTGka0x2",
                        ItemStatus = ItemStatus.ACTIVE,
                        IsInStorage = true,
                        //TODO: fix
                        FoundDate = "2023-11-10|Slot 3",
                        //FoundDate = DateTime.Now.ToVNTime(),
                        ItemMedias = medias2,
                    };
                    context.Items.Add(item2);
                    context.SaveChanges();
                #endregion

                #region Item 3 - Expired for Ongoing Giveaway
                List<ItemMedia> medias3 = new List<ItemMedia>();
                ItemMedia itemMedia3 = new ItemMedia()
                {
                    Media = new Media()
                    {
                        Name = "Item3.png",
                        Description = "Item 3 Image",
                        URL = "https://quaviet365.vn/huc/fitting-570-425-100/20230810/moc-khoa-2-20230810150640183.jpg",
                        CreatedDate = new DateTime(2023, 6, 12),
                    }
                };
                medias3.Add(itemMedia3);

                Item item3 = new Item()
                {
                    Name = "Moc khoa hinh chibi",
                    Description = "Minh co lum duoc mot moc khoa hinh chibi.",
                    LocationId = 10,
                    CategoryId = 15,
                    CreatedDate = new DateTime(2023, 6, 12),
                    FoundUserId = "jDlpStSQWiQsG2F7yDEqOTGka0x2",
                    ItemStatus = ItemStatus.EXPIRED,
                    IsInStorage = true,
                    CabinetId = 3,
                    FoundDate = "2023-6-11|Slot 3",
                    ItemMedias = medias3,
                };
                context.Items.Add(item3);
                context.SaveChanges();
                #endregion

                #region Item 4 - Expired for Reward Giveaway
                List<ItemMedia> medias4 = new List<ItemMedia>();
                ItemMedia itemMedia4 = new ItemMedia()
                {
                    Media = new Media()
                    {
                        Name = "Item4.png",
                        Description = "Item 4 Image",
                        URL = "https://down-vn.img.susercontent.com/file/vn-11134201-7r98o-ln72ji12v848ee",
                        CreatedDate = new DateTime(2023, 7, 16),
                    }
                };
                medias4.Add(itemMedia4);

                Item item4 = new Item()
                {
                    Name = "Binh nuoc giu nhiet Lock & Lock",
                    Description = "Minh co lum duoc mot Binh nuoc giu nhiet Lock & Lock o Passio.",
                    LocationId = 2,
                    CategoryId = 13,
                    CreatedDate = new DateTime(2023, 7, 16),
                    FoundUserId = "jDlpStSQWiQsG2F7yDEqOTGka0x2",
                    ItemStatus = ItemStatus.EXPIRED,
                    IsInStorage = true,
                    CabinetId = 4,
                    FoundDate = "2023-7-15|Slot 2",
                    ItemMedias = medias4,
                };
                context.Items.Add(item4);
                context.SaveChanges();
                #endregion

                #region Item 5 - Expired for Not Started Giveaway
                List<ItemMedia> medias5 = new List<ItemMedia>();
                ItemMedia itemMedia5 = new ItemMedia()
                {
                    Media = new Media()
                    {
                        Name = "Item5.png",
                        Description = "Item 5 Image",
                        URL = "https://quatangthuonghieu.vn/wp-content/uploads/2019/09/non-bao-hiem-80-1.jpg",
                        CreatedDate = new DateTime(2023, 7, 22),
                    }
                };
                medias5.Add(itemMedia5);

                Item item5 = new Item()
                {
                    Name = "Non bao hiem mau hong",
                    Description = "Minh co lum duoc mot non bao hiem mau hong vao slot 4.",
                    LocationId = 5,
                    CategoryId = 14,
                    CreatedDate = new DateTime(2023, 7, 22),
                    FoundUserId = "jDlpStSQWiQsG2F7yDEqOTGka0x2",
                    ItemStatus = ItemStatus.EXPIRED,
                    IsInStorage = true,
                    CabinetId = 5,
                    FoundDate = "2023-7-21|Slot 4",
                    ItemMedias = medias5,
                };
                context.Items.Add(item5);
                context.SaveChanges();
                #endregion

                #region Item 6 - Recently Returned for Report
                List<ItemMedia> medias6 = new List<ItemMedia>();
                ItemMedia itemMedia6 = new ItemMedia()
                {
                    Media = new Media()
                    {
                        Name = "Item6.png",
                        Description = "Item 6 Image",
                        URL = "https://hanoicomputercdn.com/media/product/37667_mouse_dell_ms116_usb_black_1.jpg",
                        CreatedDate = new DateTime(2024, 1, 15),
                    }
                };
                medias6.Add(itemMedia6);

                Item item6 = new Item()
                {
                    Name = "Chuot may tinh Dell mau den",
                    Description = "Minh co lum duoc mot con chuot may tinh Dell mau den.",
                    LocationId = 6,
                    CategoryId = 9,
                    CreatedDate = new DateTime(2024, 1, 15),
                    FoundUserId = "2CPF2ND0KGXalizaooAJLhY8CPl2",
                    ItemStatus = ItemStatus.RETURNED,
                    IsInStorage = true,
                    CabinetId = 5,
                    FoundDate = "2024-1-14|Slot 1",
                    ItemMedias = medias6,
                };
                context.Items.Add(item6);
                context.SaveChanges();
                #endregion

                #region Claim for Item 6 and already Accepted - acc Duc
                ItemClaim itemClaim1 = new ItemClaim()
                {
                    ItemId = 6,
                    UserId = "nY5n19jbQIX5ncSA1UiwpNFiMXh1",
                    IsActive = true,
                    ClaimStatus = ClaimStatus.ACCEPTED,
                    ClaimDate = DateTime.Now.ToVNTime()
                };
                context.ItemClaims.Add(itemClaim1);
                context.SaveChanges();
                #endregion

                #region Receipt for return Item 6 - Placeholder pic atm
                Media mediaForRecord1 = new Media()
                {
                    Name = "Record1.png",
                    Description = "Record 1 Image",
                    URL = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTK9LWLIlSp4HJH4Xkm6VJYQGys7e70FSr5vWl8GL9YcuS_6JNn9J6RBJeZbpdnwHbJpAY&usqp=CAU",
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.Medias.Add(mediaForRecord1);
                context.SaveChanges();
                TransferRecord record1 = new TransferRecord()
                {
                    ItemId = 6,
                    ReceiverId = "nY5n19jbQIX5ncSA1UiwpNFiMXh1",
                    SenderId = "2CPF2ND0KGXalizaooAJLhY8CPl2",
                    IsActive = true,
                    Media = mediaForRecord1,
                    ReceiptImage = mediaForRecord1.Id,
                    ReceiptType = ReceiptType.RETURN_USER_TO_USER,
                    CreatedDate = DateTime.Now.ToVNTime()
                };
                context.TransferRecords.Add(record1);
                context.SaveChanges();
                #endregion

                #endregion

                #region Add Posts

                #region Post 1
                List<PostMedia> postMedias1 = new List<PostMedia>();
                PostMedia postMedia1 = new PostMedia()
                {
                    Media = new Media()
                    {
                        Name = "Post1.png",
                        Description = "Post 1 Image",
                        URL = "https://fungift.vn/wp-content/uploads/2020/07/nha-cung-cap-binh-nuoc-uy-tin-1.jpg",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    }
                };
                postMedias1.Add(postMedia1);

                ICollection<Location> postLoca1 = new List<Location> {
                    context.Locations.FirstOrDefault(l => l.Id == 5),
                    context.Locations.FirstOrDefault(l => l.Id == 6)
                };
                ICollection<Category> postCate1 = new List<Category> {
                    context.Categories.FirstOrDefault(c => c.Id == 13)
                };

                Post post1 = new Post()
                {
                    Title = "Mat 1 binh nuoc the thao mau xam",
                    PostContent = "Em co lam rot mot binh nuoc the thao mau xam. Moi nguoi ai nhat duoc xin hay lien he em a.",
                    PostUserId = "LHFJkI0EzeN1pnfkfFuScgNvixj1",
                    LostDateFrom = new DateTime(2023, 12, 17),
                    LostDateTo = null,
                    //PostLocation = "|5|6|",
                    //PostCategory = "|13|",
                    CreatedDate = DateTime.Now.ToVNTime(),
                    PostStatus = PostStatus.ACTIVE,
                    PostMedias = postMedias1,
                    Locations = postLoca1,
                    Categories = postCate1
                };
                context.Posts.Add(post1);
                context.SaveChanges();
                #endregion

                    #region Post 2
                List<PostMedia> postMedias2 = new List<PostMedia>();
                PostMedia postMedia2 = new PostMedia()
                {
                    Media = new Media()
                    {
                        Name = "Post2.png",
                        Description = "Post 2 Image",
                        URL = "https://cdn.tgdd.vn/Files/2022/01/02/1408651/tu-van-chon-mua-chuot-may-tinh-phu-hop-cho-ban-13.jpg",
                        CreatedDate = DateTime.Now.ToVNTime(),
                    }
                };
                postMedias2.Add(postMedia2);

                ICollection<Location> postLoca2 = new List<Location> {
                    context.Locations.FirstOrDefault(l => l.Id == 10)
                };
                ICollection<Category> postCate2 = new List<Category> {
                    context.Categories.FirstOrDefault(c => c.Id == 9)
                };
                Post post2 = new Post()
                {
                    Title = "Mat 1 con chuot logitach mau do",
                    PostContent = "Sang nay em co lam mat con chuot logitach mau do. Moi nguoi ai nhat duoc xin hay lien he em a.",
                    PostUserId = "LHFJkI0EzeN1pnfkfFuScgNvixj1",
                    LostDateFrom = new DateTime(2023, 12, 16),
                    LostDateTo = null,
                    //PostLocation = "|10|",
                    //PostCategory = "|9|",
                    CreatedDate = DateTime.Now.ToVNTime(),
                    PostStatus = PostStatus.PENDING,
                    PostMedias = postMedias2,
                    Locations = postLoca2,
                    Categories = postCate2
                };

                context.Posts.Add(post2);
                context.SaveChanges();
                #endregion

                #endregion

                #region Giveaway

                #region Giveaway 1 - Ongoing
                Giveaway giveaway1 = new Giveaway()
                {
                    ItemId = 3,
                    StartAt = new DateTime(2024, 1, 15),
                    EndAt = new DateTime(2024, 1, 22),
                    GiveawayStatus = GiveawayStatus.ONGOING,
                    CreatedDate = new DateTime(2024, 1, 14)
                };
                context.Giveaways.Add(giveaway1);
                context.SaveChanges();
                #endregion

                #region Giveaway 2 - Reward
                Giveaway giveaway2 = new Giveaway()
                {
                    ItemId = 4,
                    StartAt = new DateTime(2024, 1, 18),
                    EndAt = new DateTime(2024, 1, 20),
                    GiveawayStatus = GiveawayStatus.REWARD_DISTRIBUTION_IN_PROGRESS,
                    CreatedDate = new DateTime(2024, 1, 12)
                };
                context.Giveaways.Add(giveaway2);
                context.SaveChanges();
                #endregion

                #region Participant for Giveaway 2
                GiveawayParticipant gp1 = new GiveawayParticipant()
                {
                    GiveawayId = 2,
                    UserId = "nY5n19jbQIX5ncSA1UiwpNFiMXh1",
                    IsActive = true,
                    IsChosenAsWinner = true,
                    IsWinner = true,
                    CreatedDate = new DateTime(2024, 1, 19)
                };
                context.GiveawayParticipants.Add(gp1);
                context.SaveChanges();
                #endregion

                #endregion

                #region Misc
                item2.CabinetId = 2;
                #endregion

                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}
