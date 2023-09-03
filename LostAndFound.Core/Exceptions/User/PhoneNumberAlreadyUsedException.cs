using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.User
{
    public class PhoneNumberAlreadyUsedException : HandledException
    {
        public PhoneNumberAlreadyUsedException() : base(400, "Phone number already in use.")
        {
        }
    }
}