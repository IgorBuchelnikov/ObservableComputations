// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;

namespace ObservableComputations
{
	internal class CollectionReset : IProcessable
	{
		private readonly object _eventSender;
		private readonly EventArgs _eventArgs;
		private readonly Action Action;
		private readonly ICanInitializeFromSource CanInitializeFromSource;

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
			CanInitializeFromSource.ProcessSource();
		}

		#endregion

		public object EventSender => _eventSender;
		public EventArgs EventArgs => _eventArgs;
	}
}

