using System;
using System.Collections.Generic;

namespace ObservableComputations
{
	internal class ExpressionItemInfo : Position
	{
		public ExpressionWatcher ExpressionWatcher;
		public List<IComputingInternal> NestedComputings;
	}

	internal class KeyValueExpressionItemInfo<TKey, TValue> : Position
	{
		public Func<TKey> _keySelectorFunc;
		public Func<TValue> _valueSelectorFunc;
		public TKey Key;
		public TValue Value;

		public ExpressionWatcher KeyExpressionWatcher;
		public List<IComputingInternal> KeyNestedComputings;
		public ExpressionWatcher ValueExpressionWatcher;
		public List<IComputingInternal> ValueNestedComputings;
	}

	internal sealed class OrderingItemInfo<TOrderingValue> : ExpressionItemInfo
	{
		public Func<TOrderingValue> GetOrderingValueFunc;
		public OrderedItemInfo<TOrderingValue> OrderedItemInfo;
	}

	internal sealed class OrderedItemInfo<TOrderingValue> : Position
	{
		public OrderingItemInfo<TOrderingValue> ItemInfo;
		public RangePosition RangePosition;
	}
}
