using System;
using System.Runtime.Serialization;

namespace IBCode.ObservableCalculations.Common
{
	[Serializable]
	public class ObservableCalculationsException : Exception
	{
		public ObservableCalculationsException()
		{
		}

		public ObservableCalculationsException(string message) : base(message)
		{
		}

		public ObservableCalculationsException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ObservableCalculationsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
