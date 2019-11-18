using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IBCode.ObservableComputations.Common;
using IBCode.ObservableComputations.Common.Interface;

namespace IBCode.ObservableComputations
{
	public class Uniting<TSourceItem> : Distincting<TSourceItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourcesScalar => _sourcesScalar;

		public new IReadScalar<IEqualityComparer<TSourceItem>> EqualityComparerScalar => _equalityComparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Sources => _sources;

		public new IEqualityComparer<TSourceItem> EqualityComparer => _equalityComparer;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Sources});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourcesScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourcesScalar;
		private readonly INotifyCollectionChanged _sources;

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> sourcesScalar,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) : base(getSource(sourcesScalar), equalityComparerScalar, capacity)
		{
			_sourcesScalar = sourcesScalar;
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged sources,
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) : base(getSource(sources), equalityComparerScalar, capacity)
		{
			_sources = sources;
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged sources,
			IEqualityComparer<TSourceItem> equalityComparer,
			int capacity = 0) : base(getSource(sources), equalityComparer, capacity)
		{
			_sources = sources;
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> sourcesScalar,
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) : base(getSource(sourcesScalar), equalityComparer, capacity)
		{
			_sourcesScalar = sourcesScalar;
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source1, 
			INotifyCollectionChanged source2, 
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) 
			: this(new Common.ReadOnlyObservableCollection<INotifyCollectionChanged>(new []{source1, source2}), equalityComparer, capacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> source1Scalar, 
			INotifyCollectionChanged source2, 
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) 
			: this(Expr.Is(() => new Common.ReadOnlyObservableCollection<INotifyCollectionChanged>(new []{source1Scalar.Value, source2})).Computing(), equalityComparer, capacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> source1Scalar, 
			IReadScalar<INotifyCollectionChanged> source2Scalar, 
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) 
			: this(Expr.Is(() => new Common.ReadOnlyObservableCollection<INotifyCollectionChanged>(new []{source1Scalar.Value, source2Scalar.Value})).Computing(), equalityComparer, capacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source1, 
			IReadScalar<INotifyCollectionChanged> source2Scalar, 
			IEqualityComparer<TSourceItem> equalityComparer = null,
			int capacity = 0) 
			: this(Expr.Is(() => new Common.ReadOnlyObservableCollection<INotifyCollectionChanged>(new []{source1, source2Scalar.Value})).Computing(), equalityComparer, capacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source1, 
			INotifyCollectionChanged source2, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) 
			: this(new Common.ReadOnlyObservableCollection<INotifyCollectionChanged>(new []{source1, source2}), equalityComparerScalar, capacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> source1Scalar, 
			INotifyCollectionChanged source2, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) 
			: this(Expr.Is(() => new Common.ReadOnlyObservableCollection<INotifyCollectionChanged>(new []{source1Scalar.Value, source2})).Computing(), equalityComparerScalar, capacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			IReadScalar<INotifyCollectionChanged> source1Scalar, 
			IReadScalar<INotifyCollectionChanged> source2Scalar, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) 
			: this(Expr.Is(() => new Common.ReadOnlyObservableCollection<INotifyCollectionChanged>(new []{source1Scalar.Value, source2Scalar.Value})).Computing(), equalityComparerScalar, capacity)
		{
		}

		[ObservableComputationsCall]
		public Uniting(
			INotifyCollectionChanged source1, 
			IReadScalar<INotifyCollectionChanged> source2Scalar, 
			IReadScalar<IEqualityComparer<TSourceItem>> equalityComparerScalar = null,
			int capacity = 0) 
			: this(Expr.Is(() => new Common.ReadOnlyObservableCollection<INotifyCollectionChanged>(new []{source1, source2Scalar.Value})).Computing(), equalityComparerScalar, capacity)
		{
		}

		private static INotifyCollectionChanged getSource(IReadScalar<INotifyCollectionChanged> sourcesScalar)
		{
			return sourcesScalar.Concatenating<TSourceItem>();
		}

		private static INotifyCollectionChanged getSource(INotifyCollectionChanged sources)
		{
			return sources.Concatenating<TSourceItem>();
		}

		public new void ValidateConsistency()
		{
			IList sources = (IList) _sourcesScalar.getValue(_sources, new ObservableCollection<ObservableCollection<TSourceItem>>());

			IEnumerable<TSourceItem> result = new List<TSourceItem>();

			int sourcesCount = sources.Count;
			for (int index = 0; index < sourcesCount; index++)
			{
				IEnumerable<TSourceItem> source = (IEnumerable<TSourceItem>) sources[index];
				if (source != null)
					result = result.Union(source);
			}

			if (!this.SequenceEqual(result))
				throw new ObservableComputationsException("Consistency violation: Uniting.1");
		}
	}
}