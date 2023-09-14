using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.User
{
    public class FailToCreateUserException : HandledException
    {
        public FailToCreateUserException() : base(400, "Fail To Create User.")
        {
        }
    }
}