using System;

namespace LostAndFound.Core.Entities.Common
{
    public interface IAuditedEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public abstract class AuditedEntity : IAuditedEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
