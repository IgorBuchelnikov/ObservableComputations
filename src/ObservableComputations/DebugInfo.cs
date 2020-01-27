using System.Collections.Generic;
using System.Threading;
using ObservableComputations;

namespace ObservableComputations
{
	public static class DebugInfo
	{
		internal static Dictionary<Thread, IComputing> _computingsExecutingUserCode = new Dictionary<Thread, IComputing>();
		public static IReadOnlyDictionary<Thread, IComputing> ComputingsExecutingUserCode => _computingsExecutingUserCode;
	}
}
