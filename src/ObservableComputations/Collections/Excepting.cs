using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ObservableComputations
{
	public class Excepting<TSourceItem> : Distincting<TSourceItem>, IHasSourceCollections
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> Source1Scalar => _source1Scalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> Source2Scalar => _source2Scalar;

		public new IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source1 => _source1;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source2 => _source2;

		public new IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source1, Source2});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{Source1Scalar, Source2Scalar});

		private readonly IReadScalar<INotifyCollectionChanged> _source1Scalar;
		private readonly IReadScalar<INotifyCollectionChanged> _source2Scalar;
		private readonly INotifyCollectionChanged _source1;
		private readonly INotifyCollectionChanged _source2;


		[ObservableComputationsCall]
		public Excepting(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) 
			: base(
				getSource(source1Scalar, source2Scalar, equalityComparerScalar),
				equalityComparerScalar,
				capacity)
		{
			_source1Scalar = source1Scalar;
			_source2Scalar = source2Scalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Excepting(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			INotifyCollectionChanged source2,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) 
			: base(
				getSource(source1Scalar, source2, equalityComparerScalar),
				equalityComparerScalar,
				capacity)
		{
			_source1Scalar = source1Scalar;
			_source2 = source2;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Excepting(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			INotifyCollectionChanged source2,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) 
			: base(
				getSource(source1Scalar, source2, equalityComparer),
				equalityComparer,
				capacity)
		{
			_source1Scalar = source1Scalar;
			_source2 = source2;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Excepting(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) 
			: base(
				getSource(source1Scalar, source2Scalar, equalityComparer),
				equalityComparer,
				capacity)
		{
			_source1Scalar = source1Scalar;
			_source2Scalar = source2Scalar;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Excepting(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) 
			: base(
				getSource(source1, source2Scalar, equalityComparerScalar),
				equalityComparerScalar,
				capacity)
		{
			_source1 = source1;
			_source2Scalar = source2Scalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Excepting(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) 
			: base(
				getSource(source1, source2, equalityComparerScalar),
				equalityComparerScalar,
				capacity)
		{
			_source1 = source1;
			_source2 = source2;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Excepting(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) 
			: base(
				getSource(source1, source2, equalityComparer),
				equalityComparer,
				capacity)
		{
			_source1 = source1;
			_source2 = source2;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Excepting(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) 
			: base(
				getSource(source1, source2Scalar, equalityComparer),
				equalityComparer,
				capacity)
		{
			_source1 = source1;
			_source2Scalar = source2Scalar;
			_equalityComparer = equalityComparer;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return source1Scalar
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2Scalar, item1 => item1, item2 => item2, equalityComparerScalar)
				.Filtering(jg => jg.Count == 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			INotifyCollectionChanged source2,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return source1Scalar
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2, item1 => item1, item2 => item2, equalityComparerScalar)
				.Filtering(jg => jg.Count == 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			INotifyCollectionChanged source2,
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			return source1Scalar
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2, item1 => item1, item2 => item2, equalityComparer)
				.Filtering(jg => jg.Count == 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1Scalar,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			return source1Scalar
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2Scalar, item1 => item1, item2 => item2, equalityComparer)
				.Filtering(jg => jg.Count == 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return source1
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2Scalar, item1 => item1, item2 => item2, equalityComparerScalar)
				.Filtering(jg => jg.Count == 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar)
		{
			return source1
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2, item1 => item1, item2 => item2, equalityComparerScalar)
				.Filtering(jg => jg.Count == 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2,
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			return source1
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2, item1 => item1, item2 => item2, equalityComparer)
				.Filtering(jg => jg.Count == 0)
				.Selecting(jg => jg.OuterItem);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2Scalar,
			IEqualityComparer<TSourceItem> equalityComparer)
		{
			return source1
				.GroupJoining<TSourceItem, TSourceItem, TSourceItem>(
					source2Scalar, item1 => item1, item2 => item2, equalityComparer)
				.Filtering(jg => jg.Count == 0)
				.Selecting(jg => jg.OuterItem);
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source1 = (IList<TSourceItem>) _source1Scalar.getValue(_source1, new ObservableCollection<TSourceItem>());
			IList<TSourceItem> source2 = (IList<TSourceItem>) _source2Scalar.getValue(_source2, new ObservableCollection<TSourceItem>());
			IEqualityComparer<TSourceItem> equalityComparer = _equalityComparerScalar.getValue(_equalityComparer);

			if (!this.SequenceEqual(source1.Except(source2, equalityComparer)))
				throw new ObservableComputationsException(this, "Consistency violation: Excepting.1");
		}
	}
}
