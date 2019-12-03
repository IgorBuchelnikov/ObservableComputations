using System.Collections;
using System.Collections.Specialized;

namespace ObservableComputations.Common.Interface
{
	public interface IHasCapacity : IList, INotifyCollectionChanged
	{
		int Capacity {get;}
	}
}
