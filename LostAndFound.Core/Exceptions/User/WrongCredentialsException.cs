using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.User
{
    public class WrongCredentialsException : HandledException
    {
        public WrongCredentialsException() : base(401, "Email or Password is incorrect")
        {
        }
    }
}