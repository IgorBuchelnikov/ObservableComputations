// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Generic;
using System.Collections.Specialized;

namespace ObservableComputations
{
	public interface IOrdering<TSourceItem> : IList<TSourceItem>, INotifyCollectionChanged
	{
		bool Compare(int resultIndex1, int resultIndex2);
	}

	internal interface IOrderingInternal<TSourceItem> : IOrdering<TSourceItem>
	{
		RangePosition GetRangePosition(int orderedIndex);
		void RemoveThenOrdering(IThenOrderingInternal<TSourceItem> thenOrdering);
		void AddThenOrdering(IThenOrdering<TSourceItem> thenOrdering);
		void ValidateConsistency();
	}

	public interface IThenOrdering<TSourceItem> : IList<TSourceItem>, INotifyCollectionChanged
	{
	}

	internal interface IThenOrderingInternal<TSourceItem> : IThenOrdering<TSourceItem>
	{
		void ProcessSourceItemChange(int sourceIndex, TSourceItem sourceItem);
	}
}
