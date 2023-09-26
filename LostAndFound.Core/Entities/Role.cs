using LostAndFound.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LostAndFound.Core.Entities
{
    public class Role : IAuditedEntity
    {
        public const string ADMIN_ROLE_NAME = "System Admin";
        public bool IsAdminRole()
        {
            return Name.ToLower() == ADMIN_ROLE_NAME.ToLower();
        }

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(512)] 
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<User> Users { get; set; }
        //TODO: Add 1 new role Storage Manager
    }
}