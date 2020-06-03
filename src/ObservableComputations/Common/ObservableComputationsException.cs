using System;

namespace ObservableComputations
{
	[Serializable]
	public class ObservableComputationsException : Exception
	{
		private IComputing _computing;
		public IComputing Computing => _computing;

		public ObservableComputationsException(string message) : base(message)
		{
		}

		public ObservableComputationsException(IComputing computing, string message) : base(message)
		{
			_computing = computing;
		}
	}

}
