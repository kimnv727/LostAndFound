using System;

namespace LostAndFound.Core.Exceptions.common
{
    public class ModelStateException : Exception
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public ModelStateException(string propName, string errMessage) 
        {
            PropertyName = propName;
            ErrorMessage = errMessage;
        }
    }
}
