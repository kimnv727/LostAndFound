using System;
using System.Collections.Generic;
using LostAndFound.Core.Enums;
using LostAndFound.Infrastructure.DTOs.Property;
using LostAndFound.Infrastructure.DTOs.UserMedia;
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
        
        public string SchoolId { get; set; }
        
        public string CampusName { get; set; }
        
        public bool IsActive { get; set; }
        
        public UserVerifyStatus VerifyStatus { get; set; }
        
        public DateTime CreatedDate{ get; set; }

        public ICollection<UserMediaLiteReadDTO> UserMedias { get; set; }
        public virtual CampusLiteReadDTO Campus { get; set; }
    }
}