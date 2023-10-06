using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Core.Entities
{
    public class User : IAuditedEntity, ISoftDeleteEntity
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FullName => FirstName +" " + LastName;

        public Gender? Gender { get; set; }

        public string? Avatar { get; set; }
        
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [Required]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Password { get; set; }
        public string? SchoolId { get; set; }
        public Campus? Campus { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public UserVerifyStatus VerifyStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public IReadOnlyDictionary<Func<string>, double> SearchTextsWithWeights => new Dictionary<Func<string>, double>()
        {
            {() => FirstName, 8 },
            {() => Email, 2 }
        };
        
        public virtual Role Role { get; set; }
        public ICollection<Token> Tokens { get; set; }
        public ICollection<UserMedia> UserMedias { get; set; }
        public ICollection<PostBookmark> PostBookmarks { get; set; }
        public ICollection<PostFlag> PostFlags { get; set; }
        public ICollection<CommentFlag> CommentFlags { get; set; }
        public ICollection<UserDevice> UserDevices { get; set; }
        public ICollection<GiveawayParticipant> GiveawayParticipants { get; set; }
    }
}
