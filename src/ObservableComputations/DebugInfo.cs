using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ObservableComputations
{
	public static class DebugInfo
	{
		internal static ConcurrentDictionary<Thread, IComputing> _computingsExecutingUserCode = new ConcurrentDictionary<Thread, IComputing>();
		public static IReadOnlyDictionary<Thread, IComputing> ComputingsExecutingUserCode => _computingsExecutingUserCode;

		internal static ConcurrentDictionary<Thread, Invocation> _executingDispatcherInvocations = new ConcurrentDictionary<Thread, Invocation>();
		public static IReadOnlyDictionary<Thread, Invocation> ExecutingDispatcherInvocations => _executingDispatcherInvocations;
	}
}
