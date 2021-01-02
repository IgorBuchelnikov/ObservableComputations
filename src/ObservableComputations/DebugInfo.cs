using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ObservableComputations
{
	public static class DebugInfo
	{
		internal static ConcurrentDictionary<int, IComputing> _computingsExecutingUserCode = new ConcurrentDictionary<int, IComputing>();
		public static IReadOnlyDictionary<int, IComputing> ComputingsExecutingUserCode => _computingsExecutingUserCode;

		internal static ConcurrentDictionary<int, Stack<Invocation>> _executingOcDispatcherInvocations = new ConcurrentDictionary<int, Stack<Invocation>>();
		public static IReadOnlyDictionary<int, Stack<Invocation>> ExecutingOcDispatcherInvocations => _executingOcDispatcherInvocations;
	}
}
