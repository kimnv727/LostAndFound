using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.Storage
{
    public class StorageStillHaveActiveCabinet : HandledException
    {
        public StorageStillHaveActiveCabinet() : base(400, "Storage still have active Cabinet")
        {
        }
    }
}
