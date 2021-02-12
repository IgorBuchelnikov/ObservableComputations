// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ObservableComputations
{
	public static class DebugInfo
	{
		internal static readonly ConcurrentDictionary<int, IComputing> _computingsExecutingUserCode = new ConcurrentDictionary<int, IComputing>();
		public static ReadOnlyDictionary<int, IComputing> ComputingsExecutingUserCode => 
			new ReadOnlyDictionary<int, IComputing>(_computingsExecutingUserCode);

		internal static readonly ConcurrentDictionary<int, Stack<Invocation>> _executingOcDispatcherInvocations = new ConcurrentDictionary<int, Stack<Invocation>>();
		public static IReadOnlyDictionary<int, ReadOnlyCollection<Invocation>> ExecutingOcDispatcherInvocationStacks => 
			_executingOcDispatcherInvocations.ToDictionary(
				kvp => kvp.Key, 
				kvp => new ReadOnlyCollection<Invocation>(kvp.Value.ToArray()));
	}
}
