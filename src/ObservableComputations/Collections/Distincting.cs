using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ObservableComputations
{
	public class Distincting<TSourceItem> : Selecting<Group<TSourceItem, TSourceItem>, TSourceItem>, IHasSourceCollections
	{
		// ReSharper disable once UnusedMember.Local
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		public virtual IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _source;

		public virtual IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		protected IReadScalar<IEqualityComparer<TSourceItem>> _equalityComparerScalar;

		protected IEqualityComparer<TSourceItem> _equalityComparer;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;

		public override int InitialCapacity => ((IHasInitialCapacity)base._source).InitialCapacity;

		[ObservableComputationsCall]
		public Distincting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) : base(getSource(sourceScalar, equalityComparerScalar, initialCapacity), g => g.Key)
		{
			_sourceScalar = sourceScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Distincting(
			INotifyCollectionChanged source,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int initialCapacity = 0) : base(getSource(source, equalityComparerScalar, initialCapacity), g => g.Key)
		{
			_source = source;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Distincting(
			INotifyCollectionChanged source,
			IEqualityComparer<TSourceItem> equalityComparer,
			int initialCapacity = 0) : base(getSource(source, equalityComparer, initialCapacity), g => g.Key)
		{
			_source = source;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Distincting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int initialCapacity = 0) : base(getSource(sourceScalar, equalityComparer, initialCapacity), g => g.Key)
		{
			_sourceScalar = sourceScalar;
			_equalityComparer = equalityComparer;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar,
			int initialCapacity) =>
			sourceScalar.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparerScalar, initialCapacity);

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar,
			int initialCapacity) =>
			source.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparerScalar, initialCapacity);

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source,
			IEqualityComparer<TSourceItem> equalityComparer,
			int initialCapacity) =>
			source.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparer, initialCapacity);

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IEqualityComparer<TSourceItem> equalityComparer,
			int initialCapacity) =>
			sourceScalar.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparer, initialCapacity);

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());
			IEqualityComparer<TSourceItem> equalityComparer = _equalityComparerScalar.getValue(_equalityComparer);
			if (_equalityComparerScalar == null)
				equalityComparer = EqualityComparer<TSourceItem>.Default;

			if (!this.SequenceEqual(source.Distinct(equalityComparer)))
				throw new ObservableComputationsException(this, "Consistency violation: Distincting.1");
		}
	}
}
