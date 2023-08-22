using System;

namespace LostAndFound.Core.Entities.Common
{
    public interface ICreatedEntity
    {
        public DateTime CreatedDate { get; set; }
    }

    public abstract class CreatedEntity : ICreatedEntity
    {
        public DateTime CreatedDate { get; set; }
    }
}
