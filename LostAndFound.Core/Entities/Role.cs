using LostAndFound.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Core.Entities
{
    public class Role : IAuditedEntity, ISoftDeleteEntity
    {
        public const string ADMIN_ROLE_NAME = "System Admin";
        public bool IsAdminRole()
        {
            return Name.ToLower() == ADMIN_ROLE_NAME.ToLower();
        }

        [Key]
        public Guid ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(512)] 
        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }

        //public ICollection<Authority> Authorities { get; set; }
        //public ICollection<Permission> Permissions { get; set; }
    }
}