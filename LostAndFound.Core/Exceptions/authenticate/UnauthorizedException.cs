using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.Authenticate
{
    public class UnauthorizedException : HandledException
    {
        public UnauthorizedException() : base(401, "You are not authorized to use this")
        {
        }
    }
}
