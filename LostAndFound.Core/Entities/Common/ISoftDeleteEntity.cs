using System;

namespace LostAndFound.Core.Entities.Common
{
    public interface ISoftDeleteEntity
    {
        public DateTime? DeletedDate { get; set; }
    }

    public abstract class SoftDeleteEntity : ISoftDeleteEntity
    {
        public DateTime? DeletedDate { get; set; }
    }
}
