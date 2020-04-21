using System.Collections.Generic;
using System.Threading;

namespace ObservableComputations
{
	public static class DebugInfo
	{
		internal static Dictionary<Thread, IComputing> _computingsExecutingUserCode = new Dictionary<Thread, IComputing>();
		public static IReadOnlyDictionary<Thread, IComputing> ComputingsExecutingUserCode => _computingsExecutingUserCode;
	}
}
