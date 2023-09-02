using System;
using LostAndFound.Core.Enums;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserReadDTO
    {
        public Guid ID { get; set; }
        
        public string FirstName{ get; set; }
        
        public string LastName{ get; set; }
        
        public string FullName{ get; set; }

        public Gender Gender { get; set; }
        
        public string Email{ get; set; }

        public string Avatar{ get; set; }
        
        public string FirebaseUID { get; set; }
    }
}