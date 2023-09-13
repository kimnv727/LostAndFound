using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.Authenticate
{
    public class NotPermittedException : HandledException
    {
        public NotPermittedException(string message) : base(403, message)
        {
        }
    }
    public class UserNotPermittedToResourceException<T> : NotPermittedException
    {
        public UserNotPermittedToResourceException() : base(
            $"{typeof(T).Name} is not available to current user")
        {
        }
    }
}
