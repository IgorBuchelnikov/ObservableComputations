using System;
using System.Runtime.Serialization;

namespace ObservableComputations.Common
{
	[Serializable]
	public class ObservableComputationsException : Exception
	{
		public ObservableComputationsException()
		{
		}

		public ObservableComputationsException(string message) : base(message)
		{
		}

		public ObservableComputationsException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ObservableComputationsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
