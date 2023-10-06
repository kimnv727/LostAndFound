using LostAndFound.Core.Enums;
using System;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserDetailAuthenticateReadDTO
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public Gender Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Avatar { get; set; }

        public bool IsActive { get; set; }
        
        public UserVerifyStatus VerifyStatus { get; set; }
        
    }
}