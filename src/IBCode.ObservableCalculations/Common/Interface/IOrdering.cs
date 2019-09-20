using System.Collections.Generic;
using System.Collections.Specialized;

namespace IBCode.ObservableCalculations.Common.Interface
{
	public interface IOrdering<TSourceItem> : IList<TSourceItem>, INotifyCollectionChanged
	{
	}

	internal interface IOrderingInternal<TSourceItem> : IOrdering<TSourceItem>
	{
		RangePosition GetRangePosition(int sourceIndex);
	}
}
