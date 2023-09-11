using System;

namespace LostAndFound.Core.Entities.Common
{
    public interface IPostSoftDeleteEntity
    {
        public DateTime? DeletedDate { get; set; }
    }

    public abstract class PostSoftDeleteEntity : IPostSoftDeleteEntity
    {
        public DateTime? DeletedDate { get; set; }
    }
}