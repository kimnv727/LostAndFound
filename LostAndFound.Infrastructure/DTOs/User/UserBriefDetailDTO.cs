using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.DTOs.User
{
    public class UserBriefDetailDTO
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public Gender Gender { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }
        
        public string SchoolId { get; set; }
        
        public Campus Campus { get; set; }
        
        public bool IsActive { get; set; }
        
        public UserVerifyStatus VerifyStatus { get; set; }
    }
}
