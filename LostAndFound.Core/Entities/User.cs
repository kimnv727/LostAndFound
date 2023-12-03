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
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName +" " + LastName;
        public Gender? Gender { get; set; }
        public string? Avatar { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? SchoolId { get; set; }
        [ForeignKey("Campus")]
        public int? CampusId { get; set; }
        public bool IsActive { get; set; }
        public UserVerifyStatus VerifyStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public virtual Role Role { get; set; }
        public virtual Campus Campus { get; set; }
        public ICollection<UserMedia> UserMedias { get; set; }
        public ICollection<PostBookmark> PostBookmarks { get; set; }
        public ICollection<PostFlag> PostFlags { get; set; }
        public ICollection<CommentFlag> CommentFlags { get; set; }
        public ICollection<UserDevice> UserDevices { get; set; }
        public ICollection<GiveawayParticipant> GiveawayParticipants { get; set; }
    }
}
