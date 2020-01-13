using System.Collections.Generic;
using System.Collections.Specialized;
using ObservableComputations.Common;

namespace ObservableComputations.Interface
{
	public interface IOrdering<TSourceItem> : IList<TSourceItem>, INotifyCollectionChanged
	{
		bool Compare(int resultIndex1, int resultIndex2);
	}

	internal interface IOrderingInternal<TSourceItem> : IOrdering<TSourceItem>
	{
		RangePosition GetRangePosition(int orderedIndex);
		RangePositions<RangePosition> GetRangePositions();
		void RemoveThenOrdering();
		void AddThenOrdering(IThenOrdering<TSourceItem> thenOrdering);
		void ValidateConsistency();
	}

	public interface IThenOrdering<TSourceItem> : IList<TSourceItem>, INotifyCollectionChanged
	{
	}

	internal interface IThenOrderingInternal<TSourceItem> : IThenOrdering<TSourceItem>
	{
		void ProcessSourceItemChange(int sourceIndex);
	}
}
