using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoMapper;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using LostAndFound.Core.Entities;
using LostAndFound.Core.Enums;
using LostAndFound.Core.Exceptions.Authenticate;
using LostAndFound.Core.Exceptions.User;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.DTOs.Authenticate;
using LostAndFound.Infrastructure.DTOs.User;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Services.Interfaces;
using LostAndFound.Infrastructure.UnitOfWork;
using Newtonsoft.Json.Linq;
using User = LostAndFound.Core.Entities.User;

namespace LostAndFound.API.Authentication
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly FirebaseAuthClient _firebaseAuth;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserMediaRepository _userMediaRepository;
        private readonly IEmailSendingService _emailSendingService;
        public FirebaseAuthService(FirebaseAuthClient firebaseAuth, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, 
            IUserMediaRepository userMediaRepository, IEmailSendingService emailSendingService)
        {
            _firebaseAuth = firebaseAuth;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userMediaRepository = userMediaRepository;
            _emailSendingService = emailSendingService;
        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            try
            {
                //Check in DB first, if not existed then Deny
                var user = await _userRepository.FindUserByEmail(loginRequest.Email);
                if (user != null)
                {
                    var userCredentials =
                        await _firebaseAuth.SignInWithEmailAndPasswordAsync(loginRequest.Email, loginRequest.Password);
                        
                    if (userCredentials != null)
                    {
                        var loginResponse = new LoginResponseDTO()
                        {
                            AccessToken = userCredentials.User.Credential.IdToken,
                            RefreshToken = userCredentials.User.Credential.RefreshToken
                        };
                        
                        return loginResponse;
                    }
                    else
                    {
                        //Either Wrong Email or Password
                        throw new WrongCredentialsException();
                    }
                }
                else
                {
                    //User not yet existed
                    throw new WrongCredentialsException();
                }
            }
            catch (Exception e)
            {
                //Catch Wrong Email or Password 
                throw new Exception(e.Message);
            }
        }
    
        public async Task Logout(string uid){
            _firebaseAuth.SignOut();
            await FirebaseAuth.DefaultInstance.RevokeRefreshTokensAsync(uid);
        }
        
        public async Task<UserDetailAuthenticateReadDTO> Authenticate(string uid, string email, string name,
            string avatar, string phone
            , int campusId
            )
        {
            //Check if user existed in DB -> If not then create (For GoogleLogin user)
            var user = await _userRepository.FindUserByID(uid);
            if (user != null)
            {
                //check if account disabled or not
                if(user.IsActive == false)
                {
                    throw new AccountBannedException();
                }
                //check if user has same campus
                if(user.CampusId != campusId)
                {
                    throw new WrongCampusException();
                }
                //check if user role
                if(user.RoleId != 4)
                {
                    throw new NotAMemberException();
                }

                return _mapper.Map<UserDetailAuthenticateReadDTO>(user);
            }
            else
            {
                //check email
                if (email.Contains("fpt.edu.vn") || email.Contains("fe.edu.vn"))
                {
                    //create media
                    List<UserMedia> userMedias = new List<UserMedia>();
                    UserMedia userMedia = new UserMedia()
                    {
                        UserId = uid,
                        Media = new Media()
                        {
                            Name = "GoogleAvatar",
                            Description = "Avatar of " + email,
                            URL = avatar,
                            CreatedDate = DateTime.Now.ToVNTime()
                        },
                        MediaType = UserMediaType.AVATAR
                    };
                    userMedias.Add(userMedia);
                    //create verified User
                    var newUser = new User()
                    {
                        Id = uid,
                        Email = email,
                        Password = "",
                        IsActive = true,
                        Avatar = avatar,
                        FirstName = name,
                        LastName = " ",
                        //default male
                        Gender = 0,
                        Phone = phone,
                        //User role
                        RoleId = 4,
                        //User School Id
                        SchoolId = "",
                        //CampusId = 1,
                        CampusId = campusId,
                        VerifyStatus = UserVerifyStatus.VERIFIED,
                        CreatedDate = DateTime.Now.ToVNTime(),
                        UserMedias = userMedias
                    };
                    await _userRepository.AddAsync(newUser);
                    await _unitOfWork.CommitAsync();

                    /*UserMedia userMedia = new UserMedia()
                    {
                        UserId = uid,
                        Media = new Media()
                        {
                            Name = "GoogleAvatar",
                            Description = "Avatar of " + email,
                            URL = avatar,
                        },
                        MediaType = UserMediaType.AVATAR
                    };
                    await _userMediaRepository.AddAsync(userMedia);
                    await _unitOfWork.CommitAsync();*/

                    return _mapper.Map<UserDetailAuthenticateReadDTO>(newUser);
                }
                else
                {
                    //create media
                    List<UserMedia> userMedias = new List<UserMedia>();
                    UserMedia userMedia = new UserMedia()
                    {
                        UserId = uid,
                        Media = new Media()
                        {
                            Name = "GoogleAvatar",
                            Description = "Avatar of " + email,
                            URL = avatar,
                            CreatedDate = DateTime.Now.ToVNTime()
                        },
                        MediaType = UserMediaType.AVATAR
                    };
                    userMedias.Add(userMedia);
                    //create unverified User
                    var newUser = new User()
                    {
                        Id = uid,
                        Email = email,
                        Password = "",
                        IsActive = true,
                        Avatar = avatar,
                        FirstName = name,
                        LastName = " ",
                        //default male
                        Gender = 0,
                        Phone = phone,
                        //User role
                        RoleId = 4,
                        //User School Id
                        SchoolId = "",
                        //CampusId = 1,
                        CampusId = campusId,
                        VerifyStatus = UserVerifyStatus.NOT_VERIFIED,
                        CreatedDate = DateTime.Now.ToVNTime(),
                        UserMedias = userMedias
                    };
                    //_emailSendingService.SendMailToRegister(email);
                    await _userRepository.AddAsync(newUser);
                    await _unitOfWork.CommitAsync();
                    
                    /*UserMedia userMedia = new UserMedia()
                    {
                        UserId = uid,
                        Media = new Media()
                        {
                            Name = "GoogleAvatar",
                            Description = "Avatar of " + email,
                            URL = avatar,
                        },
                        MediaType = UserMediaType.AVATAR
                    };
                    await _userMediaRepository.AddAsync(userMedia);
                    await _unitOfWork.CommitAsync();*/

                    return _mapper.Map<UserDetailAuthenticateReadDTO>(newUser);
                }

                //send email welcome new User
                _emailSendingService.SendMailToRegister(email);
            }
        }

        public async Task<string> GetAccessTokenWithRefreshToken(string refreshToken)
        {
            string baseUrl = "https://securetoken.googleapis.com/v1/token?key=";
            string key = "AIzaSyDj7Wa-uQkY9jO4NQP5s6MwvQJMO_b2PkA";
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                
                var request = new HttpRequestMessage
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri(baseUrl + key),
                };

                request.Content = JsonContent.Create(new { grant_type = "refresh_token", refresh_token = refreshToken });
                
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject jObj = JObject.Parse(result);
                    
                    return jObj["id_token"].ToString();
                }

                return null;
            }
        }

        public async Task CheckUserRoles(string userId, string[] roles)
        {
            bool checker = false;
            var user = await _userRepository.FindUserByID(userId);
            if(user == null)
            {
                throw new UnauthorizedAccessException();
            }

            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i].ToLower().Equals(user.Role.Name.ToLower()))
                {
                    checker = true;
                }
            }
            if (!checker)
            {
                throw new NotPermittedException("You are not permitted to access this function");
            }
        }
    }
}