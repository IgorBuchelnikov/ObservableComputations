using System;

namespace ObservableComputations
{
	public interface IConsistent
	{
		bool IsConsistent {get;}
		event EventHandler ConsistencyRestored;

	}
}
