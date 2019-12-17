using System;

namespace ObservableComputations.Common.Interface
{
	public interface IConsistent
	{
		bool IsConsistent {get;}
		event EventHandler ConsistencyRestored;

	}
}
