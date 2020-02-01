using System;
using System.Runtime.Serialization;

namespace KxSharpLib.Exceptions
{
	[Serializable]
	public class EncodingException : BaseException
	{
		private const string GenericMessage = "Input could not be encoded!";

		public EncodingException()
			: base(GenericMessage)
		{
		}

		public EncodingException(string message)
			: base(message)
		{
		}

		public EncodingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected EncodingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
