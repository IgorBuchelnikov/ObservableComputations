// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ObservableComputations
{
	public class Taking<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, TSourceItem>, IHasSourceCollections
	{
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarTaking;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _sourceTaking;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> CountTakingScalar => _countTakingScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public int CountTaking => _countTaking;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> StartIndexScalar => _startIndexScalar;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public override int InitialCapacity => ((IHasInitialCapacity)base._source).InitialCapacity;

		// ReSharper disable once MemberCanBePrivate.Global
		public int StartIndex => _startIndex;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarTaking;
		private readonly INotifyCollectionChanged _sourceTaking;
		private readonly IReadScalar<int> _countTakingScalar;
		private readonly int _countTaking;
		private readonly IReadScalar<int> _startIndexScalar;
		private readonly int _startIndex;

		// ReSharper disable once MemberCanBePrivate.Global


		[ObservableComputationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countTakingScalar,
			int initialCapacity = 0)
			: base(
				getSource(sourceScalar, startIndexScalar, countTakingScalar, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTaking = sourceScalar;
			_countTakingScalar = countTakingScalar;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableComputationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			int countTaking)
			: base(
				getSource(sourceScalar, startIndexScalar, countTaking),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTaking = sourceScalar;
			_countTaking = countTaking;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableComputationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			IReadScalar<int> countTakingScalar,
			int initialCapacity = 0)
			: base(
				getSource(sourceScalar, startIndex, countTakingScalar, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTaking = sourceScalar;
			_countTakingScalar = countTakingScalar;
			_startIndex = startIndex;
		}

		[ObservableComputationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			int countTaking)
			: base(
				getSource(sourceScalar, startIndex, countTaking),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTaking = sourceScalar;
			_countTaking = countTaking;
			_startIndex = startIndex;
		}

		[ObservableComputationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countTakingScalar,
			int initialCapacity = 0)
			: base(
				getSource(source, startIndexScalar, countTakingScalar, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceTaking = source;
			_countTakingScalar = countTakingScalar;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableComputationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			int countTaking)
			: base(
				getSource(source, startIndexScalar, countTaking),
				zipPair => zipPair.RightItem)
		{
			_sourceTaking = source;
			_countTaking = countTaking;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableComputationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			int startIndex,
			IReadScalar<int> countTakingScalar,
			int initialCapacity = 0)
			: base(
				getSource(source, startIndex, countTakingScalar, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceTaking = source;
			_countTakingScalar = countTakingScalar;
			_startIndex = startIndex;
		}

		[ObservableComputationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			int startIndex,
			int countTaking)
			: base(
				getSource(source, startIndex, countTaking),
				zipPair => zipPair.RightItem)
		{
			_sourceTaking = source;
			_countTaking = countTaking;
			_startIndex = startIndex;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countScalar,
			int initialCapacity)
		{
			Zipping<int, TSourceItem> zipping = 
				new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0)
				.SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar);

			return zipping.Filtering(zp => zp.LeftItem >= startIndexScalar.Value && zp.LeftItem < startIndexScalar.Value + countScalar.Value, initialCapacity);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			int count)
		{
			Zipping<int, TSourceItem> zipping = 
				new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0)
				.SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar);

			return zipping.Filtering(zp => zp.LeftItem >= startIndexScalar.Value && zp.LeftItem < startIndexScalar.Value + count, count);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			IReadScalar<int> countScalar,
			int initialCapacity)
		{
			Zipping<int, TSourceItem> zipping = 
				new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0)
				.SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar);
			return zipping.Filtering(zp => zp.LeftItem >= startIndex && zp.LeftItem < startIndex + countScalar.Value, initialCapacity);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			int count)
		{
			Zipping<int, TSourceItem> zipping = 
				new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0)
				.SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar);
			return zipping.Filtering(zp => zp.LeftItem >= startIndex && zp.LeftItem < startIndex + count, count);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countScalar,
			int initialCapacity)
		{
			Zipping<int, TSourceItem> zipping = new Computing<int>(() => ((IList) source).Count).SequenceComputing()
				.Zipping<int, TSourceItem>(source);
			return zipping.Filtering(zp => zp.LeftItem >= startIndexScalar.Value && zp.LeftItem < startIndexScalar.Value + countScalar.Value, initialCapacity);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			int count)
		{
			Zipping<int, TSourceItem> zipping = new Computing<int>(() => ((IList) source).Count).SequenceComputing()
				.Zipping<int, TSourceItem>(source);
			return zipping.Filtering(zp => zp.LeftItem >= startIndexScalar.Value && zp.LeftItem < startIndexScalar.Value + count, count);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			int startIndex,
			IReadScalar<int> countScalar,
			int initialCapacity)
		{
			Zipping<int, TSourceItem> zipping = new Computing<int>(() => ((IList) source).Count).SequenceComputing()
				.Zipping<int, TSourceItem>(source);
			return zipping.Filtering(zp => zp.LeftItem >= startIndex && zp.LeftItem < startIndex + countScalar.Value, initialCapacity);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			int startIndex,
			int count)
		{
			Zipping<int, TSourceItem> zipping = new Computing<int>(() => ((IList) source).Count).SequenceComputing()
				.Zipping<int, TSourceItem>(source);
			return zipping.Filtering(zp => zp.LeftItem >= startIndex && zp.LeftItem < startIndex + count, count);
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			IList<TSourceItem> source = _sourceScalarTaking.getValue(_sourceTaking, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int startIndex = _startIndexScalar.getValue(_startIndex);
			int count = _countTakingScalar.getValue(_countTaking);

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(startIndex).Take(count)))
			{
				throw new ValidateInternalConsistencyException("Consistency violation: Taking.1");
			}
		}
	}
}
