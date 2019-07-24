using System.Collections.Generic;
using System.Collections.Specialized;

namespace IBCode.ObservableCalculations.Common.Interface
{
	public interface IOrdering<TSourceItem> : IList<TSourceItem>, INotifyCollectionChanged
	{
		bool Compare(int resultIndex1, int resultIndex2);
		IOrdering<TSourceItem> Parent { get; }
	}
}
