// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObservableComputations
{
	internal class CollectionChangedEventRaise : IProcessable
	{
		private readonly object _eventSender;
		private readonly NotifyCollectionChangedEventArgs _eventArgs;
		private readonly ISourceCollectionChangeProcessor SourceCollectionChangeProcessor;

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
