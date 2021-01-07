using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ObservableComputations
{
	public static class DebugInfo
	{
		internal static readonly ConcurrentDictionary<int, IComputing> _computingsExecutingUserCode = new ConcurrentDictionary<int, IComputing>();
		public static IReadOnlyDictionary<int, IComputing> ComputingsExecutingUserCode => _computingsExecutingUserCode;

		internal static readonly ConcurrentDictionary<int, Stack<Invocation>> _executingOcDispatcherInvocations = new ConcurrentDictionary<int, Stack<Invocation>>();
		public static IReadOnlyDictionary<int, Stack<Invocation>> ExecutingOcDispatcherInvocations => _executingOcDispatcherInvocations;
	}
}
