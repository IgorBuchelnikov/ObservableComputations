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

	public class ObservableComputationsInconsistencyException : ObservableComputationsException
	{
		private object _eventSender;
		private EventArgs _eventArgs;

		public object EventSender => _eventSender;
		public EventArgs EventArgs => _eventArgs;

		public ObservableComputationsInconsistencyException(string message, object eventSender, EventArgs eventArgs) : base(message)
		{
			_eventSender = eventSender;
			_eventArgs = eventArgs;
		}

		public ObservableComputationsInconsistencyException(IComputing computing, string message, object eventSender, EventArgs eventArgs) : base(computing, message)
		{
			_eventSender = eventSender;
			_eventArgs = eventArgs;
		}
	}

}
