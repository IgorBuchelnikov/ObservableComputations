// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ObservableComputations
{
	public class Appending<TSourceItem> : Concatenating<TSourceItem>, IHasSourceCollections
	{
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global\
		// ReSharper disable once ConvertToAutoProperty
		public IReadScalar<TSourceItem> ItemScalar => _itemScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once ConvertToAutoProperty
		public override INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once ConvertToAutoProperty
		public TSourceItem Item => _item;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly IReadScalar<TSourceItem> _itemScalar;
		private readonly INotifyCollectionChanged _source;
		private readonly TSourceItem _item;

		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once ConvertToAutoProperty

		[ObservableComputationsCall]
		public Appending(
			INotifyCollectionChanged source,
			TSourceItem item) : base(getSources(source, item))
		{
			_source = source;
			_item = item;
		}

		[ObservableComputationsCall]
		public Appending(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar) : base(getSources(source, itemScalar))
		{
			_source = source;
			_itemScalar = itemScalar;
		}

		[ObservableComputationsCall]
		public Appending(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item) : base(getSources(sourceScalar, item))
		{
			_sourceScalar = sourceScalar;
			_item = item;
		}

		[ObservableComputationsCall]
		public Appending(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar) : base(getSources(sourceScalar, itemScalar))
		{
			_sourceScalar = sourceScalar;
			_itemScalar = itemScalar;
		}

		private static FreezedObservableCollection<object> getSources(
			INotifyCollectionChanged source,
			TSourceItem item) =>
			new FreezedObservableCollection<object>(
				new object[]{source, new FreezedObservableCollection<TSourceItem>(item)});

		private static INotifyCollectionChanged getSources(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> itemScalar) =>
				new FreezedObservableCollection<object>(new object[]{source, new Computing<FreezedObservableCollection<TSourceItem>>(() => new FreezedObservableCollection<TSourceItem>(itemScalar.Value))});

		private static INotifyCollectionChanged getSources(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem item) =>
				new FreezedObservableCollection<object>(new object[]{sourceScalar, new FreezedObservableCollection<TSourceItem>(item)});

		private static INotifyCollectionChanged getSources(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> itemScalar) =>
				new FreezedObservableCollection<object>(new object[]{sourceScalar, new Computing<FreezedObservableCollection<TSourceItem>>(() => new FreezedObservableCollection<TSourceItem>(itemScalar.Value))});

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			TSourceItem item = _itemScalar.getValue(Item);

			List<TSourceItem> result = new List<TSourceItem>(source) {item};

			if (!this.SequenceEqual(result))
				throw new ValidateInternalConsistencyException("Consistency violation: Appending.1");
		}
	}
}
