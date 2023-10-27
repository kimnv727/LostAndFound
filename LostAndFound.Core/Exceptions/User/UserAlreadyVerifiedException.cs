using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.User
{
    public class UserAlreadyVerifiedException : HandledException
    {
        public UserAlreadyVerifiedException() : base(400, "User is already verified.")
        {
        }
    }
}