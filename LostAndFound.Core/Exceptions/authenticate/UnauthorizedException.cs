using LostAndFound.Core.Exceptions.common;

namespace LostAndFound.Core.Exceptions.authenticate
{
    public class UnauthorizedException : HandledException
    {
        public UnauthorizedException() : base(401, "You are not authorized to use this")
        {
        }
    }
}
