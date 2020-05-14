using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ObservableComputations
{
	public static class DebugInfo
	{
		internal static ConcurrentDictionary<Thread, IComputing> _computingsExecutingUserCode = new ConcurrentDictionary<Thread, IComputing>();
		public static IReadOnlyDictionary<Thread, IComputing> ComputingsExecutingUserCode => _computingsExecutingUserCode;

		internal static ConcurrentDictionary<Thread, Stack<Invocation>> _executingDispatcherInvocations = new ConcurrentDictionary<Thread, Stack<Invocation>>();
		public static IReadOnlyDictionary<Thread, Stack<Invocation>> ExecutingDispatcherInvocations => _executingDispatcherInvocations;
	}
}
