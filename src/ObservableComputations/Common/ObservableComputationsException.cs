// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Diagnostics.CodeAnalysis;

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

	[ExcludeFromCodeCoverage]
	public class ValidateInternalConsistencyException : Exception
	{
		public ValidateInternalConsistencyException(string message) : base(message)
		{
		}
	}

	public class ObservableComputationsInconsistencyException : ObservableComputationsException
	{
		private readonly object _eventSender;
		private readonly EventArgs _eventArgs;

		public object EventSender => _eventSender;
		public EventArgs EventArgs => _eventArgs;

		public ObservableComputationsInconsistencyException(IComputing computing, string message, object eventSender, EventArgs eventArgs) : base(computing, message)
		{
			_eventSender = eventSender;
			_eventArgs = eventArgs;
		}
	}

}
