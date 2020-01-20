using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ObservableComputations;

namespace ObservableComputations
{
	public class Reversing<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, TSourceItem>, IHasSources
	{
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _source;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		[ObservableComputationsCall]
		public Reversing(
			IReadScalar<INotifyCollectionChanged> sourceScalar)
			: base(getSource(sourceScalar), zipPair => zipPair.ItemRight)
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Reversing(
			INotifyCollectionChanged source)
			: base(getSource(source), zipPair => zipPair.ItemRight)
		{
			_source = source;
		}

		private static INotifyCollectionChanged getSource(
			 IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			return 		
				Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Computing().SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar)
				.Ordering(zipPair => zipPair.ItemLeft, ListSortDirection.Descending);
		}

		private static INotifyCollectionChanged getSource(
			 INotifyCollectionChanged source)
		{
			return 		
				Expr.Is(() => ((IList) source).Count).Computing().SequenceComputing()
				.Zipping<int, TSourceItem>(source)
				.Ordering(zipPair => zipPair.ItemLeft, ListSortDirection.Descending);
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Reverse()))
				throw new ObservableComputationsException(this, "Consistency violation: Reversing.1");
		}
	}
}
