using System;
using System.Collections.Generic;

namespace ObservableComputations
{
	internal class CollectionReset : IProcessable
	{
		private readonly object _eventSender;
		private readonly EventArgs _eventArgs;
		internal Action Action;
		internal ICanInitializeFromSource CanInitializeFromSource;

		public CollectionReset(object eventSender, EventArgs eventArgs, ICanInitializeFromSource canInitializeFromSource, Action action)
		{
			_eventSender = eventSender;
			_eventArgs = eventArgs;
			CanInitializeFromSource = canInitializeFromSource;
			Action = action;
		}

		#region Implementation of IProcessable
		public void Process(Queue<IProcessable>[] deferredProcessings)
		{
			Utils.clearDeferredProcessings(deferredProcessings);

			Action?.Invoke();
			CanInitializeFromSource.InitializeFromSource();
		}

		#endregion

		public object EventSender => _eventSender;
		public EventArgs EventArgs => _eventArgs;
	}
}

