// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ObservableComputations
{
	public class Reversing<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, TSourceItem>, IHasSourceCollections
	{
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _source;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		[ObservableComputationsCall]
		public Reversing(
			IReadScalar<INotifyCollectionChanged> sourceScalar)
			: base(getSource(sourceScalar), zipPair => zipPair.RightItem)
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Reversing(
			INotifyCollectionChanged source)
			: base(getSource(source), zipPair => zipPair.RightItem)
		{
			_source = source;
		}

		private static INotifyCollectionChanged getSource(
			 IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			return 		
				new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar)
				.Ordering(zipPair => zipPair.LeftItem, ListSortDirection.Descending);
		}

		private static INotifyCollectionChanged getSource(
			 INotifyCollectionChanged source)
		{
			return 		
				new Computing<int>(() => ((IList) source).Count).SequenceComputing()
				.Zipping<int, TSourceItem>(source)
				.Ordering(zipPair => zipPair.LeftItem, ListSortDirection.Descending);
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Reverse()))
				throw new ObservableComputationsException(this, "Consistency violation: Reversing.1");
		}
	}
}
