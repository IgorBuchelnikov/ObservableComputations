using System;
using System.Collections.Generic;

namespace ObservableComputations.Common
{
	internal class Processable : IProcessable
	{
		private object _eventSender;
		private EventArgs _eventArgs;
		private Action _action;

		public Processable(object eventSender, EventArgs eventArgs, Action action)
		{
			_eventSender = eventSender;
			_eventArgs = eventArgs;
			_action = action;
		}

		#region Implementation of IProcessable

		public void Process(Queue<IProcessable>[] deferredProcessings)
		{
			_action();
		}

		public object EventSender => _eventSender;
		public EventArgs EventArgs => _eventArgs;

		#endregion
	}
}
