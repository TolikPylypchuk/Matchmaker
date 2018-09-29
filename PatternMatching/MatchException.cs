using System;
using System.Runtime.Serialization;

namespace PatternMatching
{
	public class MatchException : Exception
	{
		public MatchException() { }

		public MatchException(string message) : base(message) { }

		public MatchException(string message, Exception innerException) : base(message, innerException) { }

		protected MatchException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
