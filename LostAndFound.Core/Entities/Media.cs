using LostAndFound.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LostAndFound.Core.Entities
{
    public class Media : IAuditedEntity, ISoftDeleteEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string URL { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string DeletedBy { get; set; }

        public ICollection<UserMedia> UserMedias { get; set; }
        public ICollection<PostMedia> PostMedias { get; set; }
        public ICollection<ItemMedia> ItemMedias { get; set; }
    }
}
