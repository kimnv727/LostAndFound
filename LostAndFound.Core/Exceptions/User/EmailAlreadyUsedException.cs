using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.User
{
    public class EmailAlreadyUsedException : HandledException
    {
        public EmailAlreadyUsedException() : base(400, "Email already in use.")
        {
        }
    }
}