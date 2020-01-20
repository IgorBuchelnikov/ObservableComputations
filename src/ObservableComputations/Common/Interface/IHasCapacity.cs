using System.Collections;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface IHasCapacity : IList, INotifyCollectionChanged
	{
		int Capacity {get;}
	}
}
