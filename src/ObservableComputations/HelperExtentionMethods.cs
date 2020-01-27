using System;
using ObservableComputations;

namespace ObservableComputations
{
	public static class HelperExtentionMethods
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
