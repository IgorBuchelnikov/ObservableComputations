// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;

namespace ObservableComputations
{
	internal class Processable : IProcessable
	{
		private readonly object _eventSender;
		private readonly EventArgs _eventArgs;
		private readonly Action _action;

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
