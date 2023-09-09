using System;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserDetailsReadDTO
    {
        public string Id { get; set; }
        
        public string FirstName{ get; set; }
        
        public string LastName{ get; set; }
        
        public string FullName{ get; set; }

        public Gender Gender { get; set; }
        
        public string Email{ get; set; }

        public string Phone{ get; set; }

        public string Avatar{ get; set; }
        
        //TODO: show Role name here
        public int RoleId { get; set; } 
        
        public DateTime CreatedDate{ get; set; }
    }
}