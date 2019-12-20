using System;
using System.Runtime.Serialization;
using ObservableComputations.Common.Interface;

namespace ObservableComputations.Common
{
	[Serializable]
	public class ObservableComputationsException : Exception
	{
		private IComputing _computing;
		private IComputing Computing => _computing;

		public ObservableComputationsException(string message) : base(message)
		{
		}

		public ObservableComputationsException(IComputing computing, string message) : base(message)
		{
			_computing = computing;
		}
	}
}
