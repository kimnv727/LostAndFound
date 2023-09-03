
using LostAndFound.Core.Exceptions.Common;

namespace LostAndFound.Core.Exceptions.Common
{
    public class InvalidFileFormatException : HandledException
    {
        public InvalidFileFormatException(string format) : base(400,
            $"The submitted file is corrupted or is not of {format} format")
        {
        }
    }
}
