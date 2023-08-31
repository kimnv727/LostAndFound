using LostAndFound.Core.Exceptions.common;

namespace LostAndFound.Core.Exceptions.User
{
    public class EmailAlreadyUsedException : HandledException
    {
        public EmailAlreadyUsedException() : base(400, "Email already in use.")
        {
        }
    }
}