// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

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
