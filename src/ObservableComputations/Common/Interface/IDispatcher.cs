using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public interface IDispatcher
	{
		void Invoke(Action action, IComputing computing);
	}
}
