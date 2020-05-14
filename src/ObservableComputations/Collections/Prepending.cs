using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ObservableComputations
{
	public class Prepending<TSourceItem> : Concatenating<TSourceItem>, IHasSourceCollections
	{
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<TSourceItem> ItemScalar => _itemScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public TSourceItem Item => _item;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly IReadScalar<TSourceItem> _itemScalar;
		private readonly INotifyCollectionChanged _source;
		private readonly TSourceItem _item;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public Prepending(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar) : base(getSources(sourceScalar, itemScalar))
		{
			_sourceScalar = sourceScalar;
			_itemScalar = itemScalar;
		}

		[ObservableComputationsCall]
		public Prepending(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar) : base(getSources(source, itemScalar))
		{
			_source = source;
			_itemScalar = itemScalar;
		}

		[ObservableComputationsCall]
		public Prepending(
			INotifyCollectionChanged source,
			TSourceItem item) : base(getSources(source, item))
		{
			_source = source;
			_item = item;
		}

		[ObservableComputationsCall]
		public Prepending(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item) : base(getSources(sourceScalar, item))
		{
			_sourceScalar = sourceScalar;
			_item = item;
		}

		private static FreezedObservableCollection<object> getSources(
			INotifyCollectionChanged source,
			TSourceItem item) =>
				new FreezedObservableCollection<object>(
					new object[]{new FreezedObservableCollection<TSourceItem>(item), source});

		private static INotifyCollectionChanged getSources(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar) =>
			new FreezedObservableCollection<object>(new object[]{new Computing<FreezedObservableCollection<TSourceItem>>(() => new FreezedObservableCollection<TSourceItem>(itemScalar.Value)), source});

		private static INotifyCollectionChanged getSources(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item) =>
			new FreezedObservableCollection<object>(new object[]{new FreezedObservableCollection<TSourceItem>(item), sourceScalar});

		private static INotifyCollectionChanged getSources(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar) =>
			new FreezedObservableCollection<object>(new object[]{new Computing<FreezedObservableCollection<TSourceItem>>(() => new FreezedObservableCollection<TSourceItem>(itemScalar.Value)), sourceScalar});

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			TSourceItem item = _itemScalar.getValue(_item);

			List<TSourceItem> result = new List<TSourceItem>(source);
			result.Insert(0, item);

			if (!this.SequenceEqual(result))
				throw new ObservableComputationsException(this, "Consistency violation: Prepending.1");
		}
	}
}
