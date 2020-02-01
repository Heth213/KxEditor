using System;
using System.Runtime.Serialization;

namespace KxSharpLib.Exceptions
{
    [Serializable]
    public class DecodingException : BaseException
    {
		private const string GenericMessage = "Input could not be decoded!";

		public DecodingException() 
			: base(GenericMessage) 
		{ 
		}

		public DecodingException(string message) 
			: base(message) 
		{ 
		}

		public DecodingException(string message, Exception innerException) 
			: base(message, innerException) 
		{ 
		}

		protected DecodingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
