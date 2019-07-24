using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class ContainsCalculating<TSourceItem> : AnyCalculating<TSourceItem>, IHasSources
	{
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarContainsCalculating;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _sourceContainsCalculating;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<TSourceItem> ItemScalar => _itemScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public TSourceItem Item => _item;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarContainsCalculating;
		private readonly INotifyCollectionChanged _sourceContainsCalculating;
		private readonly IReadScalar<TSourceItem> _itemScalar;
		private readonly TSourceItem _item;
		private readonly IReadScalar<IEqualityComparer<TSourceItem>> _equalityComparerScalar;
		private readonly IEqualityComparer<TSourceItem> _equalityComparer;

		// ReSharper disable once MemberCanBePrivate.Global


		[ObservableCalculationsCall]
		public ContainsCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null) 
			: base(sourceScalar, getPredicateExpression(itemScalar, equalityComparerScalar))
		{
			_sourceScalarContainsCalculating = sourceScalar;
			_itemScalar = itemScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableCalculationsCall]
		public ContainsCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null) 
			: base(sourceScalar, getPredicateExpression(item, equalityComparerScalar))
		{
			_sourceScalarContainsCalculating = sourceScalar;
			_item = item;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableCalculationsCall]
		public ContainsCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null) 
			: base(sourceScalar, getPredicateExpression(itemScalar, equalityComparer))
		{
			_sourceScalarContainsCalculating = sourceScalar;
			_itemScalar = itemScalar;
			_equalityComparer = equalityComparer;
		}

		[ObservableCalculationsCall]
		public ContainsCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item,
			IEqualityComparer<TSourceItem> equalityComparer = null) 
			: base(sourceScalar, getPredicateExpression(item, equalityComparer))
		{
			_sourceScalarContainsCalculating = sourceScalar;
			_item = item;
			_equalityComparer = equalityComparer;
		}



		[ObservableCalculationsCall]
		public ContainsCalculating(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null) 
			: base(source, getPredicateExpression(itemScalar, equalityComparerScalar))
		{
			_sourceContainsCalculating = source;
			_itemScalar = itemScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableCalculationsCall]
		public ContainsCalculating(
			INotifyCollectionChanged source,
			TSourceItem item,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null) 
			: base(source, getPredicateExpression(item, equalityComparerScalar))
		{
			_sourceContainsCalculating = source;
			_item = item;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableCalculationsCall]
		public ContainsCalculating(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null) 
			: base(source, getPredicateExpression(itemScalar, equalityComparer))
		{
			_sourceContainsCalculating = source;
			_itemScalar = itemScalar;
			_equalityComparer = equalityComparer;
		}

		[ObservableCalculationsCall]
		public ContainsCalculating(
			INotifyCollectionChanged source,
			TSourceItem item,
			IEqualityComparer<TSourceItem> equalityComparer = null) 
			: base(source, getPredicateExpression(item, equalityComparer))
		{
			_sourceContainsCalculating = source;
			_item = item;
			_equalityComparer = equalityComparer;
		}


		private static Expression<Func<TSourceItem, bool>> getPredicateExpression(
			IReadScalar<TSourceItem> itemScalar, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			if (equalityComparerScalar == null) return getPredicateExpression(itemScalar, EqualityComparer<TSourceItem>.Default);

			return sourceItem => 
					equalityComparerScalar.Value.Equals(sourceItem, itemScalar.Value);
		}

		private static Expression<Func<TSourceItem, bool>> getPredicateExpression(
			TSourceItem item, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			if (equalityComparerScalar == null) return getPredicateExpression(item, EqualityComparer<TSourceItem>.Default);

			return sourceItem => 
					equalityComparerScalar.Value.Equals(sourceItem, item);
		}

		private static Expression<Func<TSourceItem, bool>> getPredicateExpression(
			IReadScalar<TSourceItem> itemScalar, 
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			if (equalityComparer == null) equalityComparer = EqualityComparer<TSourceItem>.Default;

			return sourceItem => 
					equalityComparer.Equals(sourceItem, itemScalar.Value);
		}

		private static Expression<Func<TSourceItem, bool>> getPredicateExpression(
			TSourceItem item, 
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			if (equalityComparer == null) equalityComparer = EqualityComparer<TSourceItem>.Default;

			return sourceItem => 
					equalityComparer.Equals(sourceItem, item);
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalarContainsCalculating.getValue(_sourceContainsCalculating, new ObservableCollection<TSourceItem>());
			TSourceItem sourceItem = _itemScalar.getValue(_item);
			IEqualityComparer<TSourceItem> equalityComparer =  _equalityComparerScalar.getValue(_equalityComparer);

			if (_value != source.Contains(sourceItem, equalityComparer))
				throw new ObservableCalculationsException("Consistency violation: ContainsCalculating.1");
		}
	}
}
