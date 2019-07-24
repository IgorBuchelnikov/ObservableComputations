using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class Distincting<TSourceItem> : Selecting<Group<TSourceItem, TSourceItem>, TSourceItem>, IHasSources
	{
		// ReSharper disable once UnusedMember.Local
		private new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		public IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _source;

		public IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		protected IReadScalar<IEqualityComparer<TSourceItem>> _equalityComparerScalar;

		protected IEqualityComparer<TSourceItem> _equalityComparer;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;

		[ObservableCalculationsCall]
		public Distincting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) : base(getSource(sourceScalar, equalityComparerScalar, capacity), g => g.Key)
		{
			_sourceScalar = sourceScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableCalculationsCall]
		public Distincting(
			INotifyCollectionChanged source,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) : base(getSource(source, equalityComparerScalar, capacity), g => g.Key)
		{
			_source = source;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableCalculationsCall]
		public Distincting(
			INotifyCollectionChanged source,
			IEqualityComparer<TSourceItem> equalityComparer,
			int capacity = 0) : base(getSource(source, equalityComparer, capacity), g => g.Key)
		{
			_source = source;
			_equalityComparer = equalityComparer;
		}

		[ObservableCalculationsCall]
		public Distincting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) : base(getSource(sourceScalar, equalityComparer, capacity), g => g.Key)
		{
			_sourceScalar = sourceScalar;
			_equalityComparer = equalityComparer;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar,
			int capacity) =>
			sourceScalar.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparerScalar, capacity);

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar,
			int capacity) =>
			source.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparerScalar, capacity);

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source,
			IEqualityComparer<TSourceItem> equalityComparer,
			int capacity) =>
			source.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparer, capacity);

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IEqualityComparer<TSourceItem> equalityComparer,
			int capacity) =>
			sourceScalar.Grouping<TSourceItem, TSourceItem>(sourceItem => sourceItem, equalityComparer, capacity);

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());
			IEqualityComparer<TSourceItem> equalityComparer = _equalityComparerScalar.getValue(_equalityComparer);
			if (_equalityComparerScalar == null)
				equalityComparer = EqualityComparer<TSourceItem>.Default;

			if (!this.SequenceEqual(source.Distinct(equalityComparer)))
				throw new ObservableCalculationsException("Consistency violation: Distincting.1");
		}
	}
}
