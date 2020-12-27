using System;

namespace ObservableComputations
{
	public static class HelperExtensionMethods
	{
		[ObservableComputationsCall]
		public static TComputation SetStackTraceToDebugTag<TComputation>(this TComputation computation)
			where TComputation : IHasTags
		{
			computation.DebugTag = Environment.StackTrace;
			return computation;
		}
	}
}
