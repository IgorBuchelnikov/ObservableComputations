using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObservableComputations
{
	internal class CollectionChangedEventRaise : IProcessable
	{
		private object _eventSender;
		private NotifyCollectionChangedEventArgs _eventArgs;
		internal ISourceCollectionChangeProcessor SourceCollectionChangeProcessor;

		public CollectionChangedEventRaise(object eventSender, NotifyCollectionChangedEventArgs eventArgs, ISourceCollectionChangeProcessor sourceCollectionChangeProcessor)
		{
			_eventSender = eventSender;
			_eventArgs = eventArgs;
			SourceCollectionChangeProcessor = sourceCollectionChangeProcessor;

		}

		#region Implementation of IProcessable

		public void Process(Queue<IProcessable>[] deferredProcessings)
		{
			SourceCollectionChangeProcessor.processSourceCollectionChanged(_eventSender, _eventArgs);
		}

		public object EventSender => _eventSender;
		public EventArgs EventArgs => _eventArgs;

		#endregion
	}
}
