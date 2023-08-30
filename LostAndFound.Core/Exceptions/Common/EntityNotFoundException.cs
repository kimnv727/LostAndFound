using LostAndFound.Core.Exceptions.common;

namespace LostAndFound.Core.Exceptions.Common
{
    public class EntityNotFoundException : HandledException
    {
        public EntityNotFoundException(string message) : base(404, message)
        {
        }
    }

    public class EntityWithIDNotFoundException<T> : EntityNotFoundException
    {
        public EntityWithIDNotFoundException(params object[] IDs) : base(
            $"{typeof(T).Name} with ID ({string.Join(',', IDs)}) could not be found")
        {
        }
    }

    public class EntityWithEmailNotFoundException<T> : EntityNotFoundException
    {
        public EntityWithEmailNotFoundException(params object[] Emails) : base(
            $"{typeof(T).Name} with email ({string.Join(',', Emails)}) could not be found")
        {
        }
    }

    public class EntityWithAttributeNotFoundException<T> : EntityNotFoundException
    {
        public EntityWithAttributeNotFoundException(string attributeName, object attributeValue) : base(
            $"{typeof(T).Name} with {attributeName} = {attributeValue} could not be found")
        {
        }
    }
}
