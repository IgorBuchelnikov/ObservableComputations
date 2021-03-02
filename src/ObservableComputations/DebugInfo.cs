// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;

namespace ObservableComputations
{
	public static class StaticInfo
	{
		internal static readonly ConcurrentDictionary<int, IComputing> _computingsExecutingUserCode = new ConcurrentDictionary<int, IComputing>();

		public static ReadOnlyDictionary<int, IComputing> ComputingsExecutingUserCode => 
			new ReadOnlyDictionary<int, IComputing>(_computingsExecutingUserCode);

		internal static readonly ConcurrentDictionary<Thread, OcDispatcher> _ocDispatchers = new ConcurrentDictionary<Thread, OcDispatcher>();

		public static ReadOnlyDictionary<Thread, OcDispatcher> OcDispatchers => 
			new ReadOnlyDictionary<Thread, OcDispatcher>(_ocDispatchers);
	}
}
