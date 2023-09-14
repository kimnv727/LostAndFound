using System;
using LostAndFound.Core.Enums;
using Newtonsoft.Json;

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
        
        [JsonProperty("role")]
        public string RoleName {get; set; }
        
        public DateTime CreatedDate{ get; set; }
    }
}