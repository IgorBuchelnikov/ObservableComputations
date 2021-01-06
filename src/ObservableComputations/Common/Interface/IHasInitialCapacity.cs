using System.Collections;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface IHasInitialCapacity : IList, INotifyCollectionChanged
	{
		int InitialCapacity {get;}
	}
}
