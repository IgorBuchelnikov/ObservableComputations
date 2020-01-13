using System;

namespace ObservableComputations.Interface
{
	public interface IConsistent
	{
		bool IsConsistent {get;}
		event EventHandler ConsistencyRestored;

	}
}
