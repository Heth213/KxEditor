using System;
using System.Runtime.Serialization;

namespace KxSharpLib.Exceptions
{
    [Serializable]
    public class BaseException : Exception
    {
        BaseException()
        {
        }

        public BaseException(string message)
            : base(message)
        {
        }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
