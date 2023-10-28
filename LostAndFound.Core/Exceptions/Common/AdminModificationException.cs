namespace LostAndFound.Core.Exceptions.Common
{
    public class AdminModificationException : HandledException
    {
        public AdminModificationException() : base(400, "You can't modify any status of Admin") {
        
        }
    }
}
