// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;

namespace ObservableComputations
{
	public interface INotifyMethodChanged
	{
		event EventHandler<MethodChangedEventArgs> MethodChanged;
	}

	public class MethodChangedEventArgs : EventArgs
	{
		public MethodChangedEventArgs(string methodName, Func<object[], bool> argumentsPredicate)
		{
			MethodName = methodName;
			ArgumentsPredicate = argumentsPredicate;
		}

		public string MethodName { get; }

		public Func<object[], bool> ArgumentsPredicate { get; }
	}
}
