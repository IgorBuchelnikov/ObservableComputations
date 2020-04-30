using System;
using System.Collections.Specialized;
using System.Threading;

namespace ObservableComputations.Test
{
	public static  class ExtensionMethods
	{
		public static void WaitOneAndDispose(
			this WaitHandle waitHandle)
		{
			waitHandle.WaitOne();
			waitHandle.Dispose();
		}
	}
}
