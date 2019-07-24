using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class Prepending<TSourceItem> : Concatenating<TSourceItem>, IHasSources
	{
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<TSourceItem> ItemScalar => _itemScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public TSourceItem Item => _item;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly IReadScalar<TSourceItem> _itemScalar;
		private readonly INotifyCollectionChanged _source;
		private readonly TSourceItem _item;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableCalculationsCall]
		public Prepending(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar) : base(getSources(sourceScalar, itemScalar))
		{
			_sourceScalar = sourceScalar;
			_itemScalar = itemScalar;
		}

		[ObservableCalculationsCall]
		public Prepending(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar) : base(getSources(source, itemScalar))
		{
			_source = source;
			_itemScalar = itemScalar;
		}

		[ObservableCalculationsCall]
		public Prepending(
			INotifyCollectionChanged source,
			TSourceItem item) : base(getSources(source, item))
		{
			_source = source;
			_item = item;
		}

		[ObservableCalculationsCall]
		public Prepending(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item) : base(getSources(sourceScalar, item))
		{
			_sourceScalar = sourceScalar;
			_item = item;
		}

		private static IReadScalar<INotifyCollectionChanged> getSources(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar)
		{
			return Expr.Is(() => new Common.ReadOnlyObservableCollection<object>(
				new object[]{new Common.ReadOnlyObservableCollection<TSourceItem>(itemScalar.Value), sourceScalar.Value})).Calculating();
		}

		private static IReadScalar<INotifyCollectionChanged> getSources(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar)
		{
			return Expr.Is(() => new Common.ReadOnlyObservableCollection<object>(
				new object[]{new Common.ReadOnlyObservableCollection<TSourceItem>(itemScalar.Value), source})).Calculating();
		}

		private static Common.ReadOnlyObservableCollection<object> getSources(
			INotifyCollectionChanged source,
			TSourceItem item)
		{
			return new Common.ReadOnlyObservableCollection<object>(
				new object[]{new Common.ReadOnlyObservableCollection<TSourceItem>(item), source});
		}

		private static IReadScalar<INotifyCollectionChanged> getSources(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item)
		{
			return Expr.Is(() => new Common.ReadOnlyObservableCollection<object>(
				new object[]{new Common.ReadOnlyObservableCollection<TSourceItem>(item), sourceScalar.Value})).Calculating();
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			TSourceItem item = _itemScalar.getValue(_item);

			List<TSourceItem> result = new List<TSourceItem>(source);
			result.Insert(0, item);

			if (!this.SequenceEqual(result))
				throw new ObservableCalculationsException("Consistency violation: Prepending.1");
		}
	}
}
