using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ObservableComputations
{
	public class Uniting<TSourceItem> : Distincting<TSourceItem>, IHasSourceCollections
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		public new IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public new IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;

		public override int InitialCapacity => ((CollectionComputing<TSourceItem>)_source)._initialCapacity;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) : base(getSource(sourceScalar), equalityComparerScalar, initialCapacity)
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) : base(getSource(source), equalityComparerScalar, initialCapacity)
		{
			_source = source;
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source,
			IEqualityComparer<TSourceItem> equalityComparer,
			int initialCapacity = 0) : base(getSource(source), equalityComparer, initialCapacity)
		{
			_source = source;
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) : base(getSource(sourceScalar), equalityComparer, initialCapacity)
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source1, 
			INotifyCollectionChanged source2, 
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) 
			: this(new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1, source2}), equalityComparer, initialCapacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> source1Scalar, 
			INotifyCollectionChanged source2, 
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) 
			: this(Expr.Is(() => new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1Scalar.Value, source2})).Computing(), equalityComparer, initialCapacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> source1Scalar, 
			IReadScalar<INotifyCollectionChanged> source2Scalar, 
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) 
			: this(Expr.Is(() => new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1Scalar.Value, source2Scalar.Value})).Computing(), equalityComparer, initialCapacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source1, 
			IReadScalar<INotifyCollectionChanged> source2Scalar, 
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) 
			: this(Expr.Is(() => new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1, source2Scalar.Value})).Computing(), equalityComparer, initialCapacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source1, 
			INotifyCollectionChanged source2, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) 
			: this(new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1, source2}), equalityComparerScalar, initialCapacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> source1Scalar, 
			INotifyCollectionChanged source2, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) 
			: this(Expr.Is(() => new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1Scalar.Value, source2})).Computing(), equalityComparerScalar, initialCapacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> source1Scalar, 
			IReadScalar<INotifyCollectionChanged> source2Scalar, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) 
			: this(Expr.Is(() => new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1Scalar.Value, source2Scalar.Value})).Computing(), equalityComparerScalar, initialCapacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source1, 
			IReadScalar<INotifyCollectionChanged> source2Scalar, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) 
			: this(Expr.Is(() => new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1, source2Scalar.Value})).Computing(), equalityComparerScalar, initialCapacity)
		{
		}

		private static INotifyCollectionChanged getSource(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			return sourceScalar.Concatenating<TSourceItem>();
		}

		private static INotifyCollectionChanged getSource(INotifyCollectionChanged source)
		{
			return source.Concatenating<TSourceItem>();
		}

		public new void ValidateConsistency()
		{
			IList sources = (IList) _sourceScalar.getValue(_source, new ObservableCollection<ObservableCollection<TSourceItem>>());

			IEnumerable<TSourceItem> result = new List<TSourceItem>();

			int sourcesCount = sources.Count;
			for (int index = 0; index < sourcesCount; index++)
			{
				IEnumerable<TSourceItem> source = (IEnumerable<TSourceItem>) sources[index];
				if (source != null)
					result = result.Union(source);
			}

			if (!this.SequenceEqual(result))
				throw new ObservableComputationsException(this, "Consistency violation: Uniting.1");
		}
	}
}