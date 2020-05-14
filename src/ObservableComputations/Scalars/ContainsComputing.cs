using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class ContainsComputing<TSourceItem> : AnyComputing<TSourceItem>, IHasSourceCollections
	{
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarContainsComputing;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _sourceContainsComputing;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<TSourceItem> ItemScalar => _itemScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public TSourceItem Item => _item;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarContainsComputing;
		private readonly INotifyCollectionChanged _sourceContainsComputing;
		private readonly IReadScalar<TSourceItem> _itemScalar;
		private readonly TSourceItem _item;
		private readonly IReadScalar<IEqualityComparer<TSourceItem>> _equalityComparerScalar;
		private readonly IEqualityComparer<TSourceItem> _equalityComparer;

		// ReSharper disable once MemberCanBePrivate.Global


		[ObservableComputationsCall]
		public ContainsComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null) 
			: base(sourceScalar, getPredicateExpression(itemScalar, equalityComparerScalar))
		{
			_sourceScalarContainsComputing = sourceScalar;
			_itemScalar = itemScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public ContainsComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null) 
			: base(sourceScalar, getPredicateExpression(item, equalityComparerScalar))
		{
			_sourceScalarContainsComputing = sourceScalar;
			_item = item;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public ContainsComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null) 
			: base(sourceScalar, getPredicateExpression(itemScalar, equalityComparer))
		{
			_sourceScalarContainsComputing = sourceScalar;
			_itemScalar = itemScalar;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public ContainsComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item,
			IEqualityComparer<TSourceItem> equalityComparer = null) 
			: base(sourceScalar, getPredicateExpression(item, equalityComparer))
		{
			_sourceScalarContainsComputing = sourceScalar;
			_item = item;
			_equalityComparer = equalityComparer;
		}



		[ObservableComputationsCall]
		public ContainsComputing(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null) 
			: base(source, getPredicateExpression(itemScalar, equalityComparerScalar))
		{
			_sourceContainsComputing = source;
			_itemScalar = itemScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public ContainsComputing(
			INotifyCollectionChanged source,
			TSourceItem item,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null) 
			: base(source, getPredicateExpression(item, equalityComparerScalar))
		{
			_sourceContainsComputing = source;
			_item = item;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public ContainsComputing(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null) 
			: base(source, getPredicateExpression(itemScalar, equalityComparer))
		{
			_sourceContainsComputing = source;
			_itemScalar = itemScalar;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public ContainsComputing(
			INotifyCollectionChanged source,
			TSourceItem item,
			IEqualityComparer<TSourceItem> equalityComparer = null) 
			: base(source, getPredicateExpression(item, equalityComparer))
		{
			_sourceContainsComputing = source;
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
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalarContainsComputing.getValue(_sourceContainsComputing, new ObservableCollection<TSourceItem>());
			TSourceItem sourceItem = _itemScalar.getValue(_item);
			IEqualityComparer<TSourceItem> equalityComparer =  _equalityComparerScalar.getValue(_equalityComparer);

			if (_value != source.Contains(sourceItem, equalityComparer))
				throw new ObservableComputationsException(this, "Consistency violation: ContainsComputing.1");
		}
	}
}
