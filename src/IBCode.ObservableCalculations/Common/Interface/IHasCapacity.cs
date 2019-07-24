using System.Collections;
using System.Collections.Specialized;

namespace IBCode.ObservableCalculations.Common.Interface
{
	public interface IHasCapacity : IList, INotifyCollectionChanged
	{
		int Capacity {get;}
	}
}
