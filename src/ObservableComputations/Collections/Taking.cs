using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace ObservableComputations
{
	public class Taking<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, TSourceItem>, IHasSourceCollections
	{
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarTaking;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _sourceTaking;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> CountScalar => _countScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public int CountTaking => _countTaking;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> StartIndexScalar => _startIndexScalar;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public override int InitialCapacity => ((CollectionComputing<TSourceItem>)_source)._initialCapacity;

		// ReSharper disable once MemberCanBePrivate.Global
		public int StartIndex => _startIndex;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarTaking;
		private readonly INotifyCollectionChanged _sourceTaking;
		private readonly IReadScalar<int> _countScalar;
		private readonly int _countTaking;
		private readonly IReadScalar<int> _startIndexScalar;
		private readonly int _startIndex;

		// ReSharper disable once MemberCanBePrivate.Global


		[ObservableComputationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countScalar,
			int initialCapacity = 0)
			: base(
				getSource(sourceScalar, startIndexScalar, countScalar, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTaking = sourceScalar;
			_countScalar = countScalar;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableComputationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			int count)
			: base(
				getSource(sourceScalar, startIndexScalar, count),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTaking = sourceScalar;
			_countTaking = count;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableComputationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			IReadScalar<int> countScalar,
			int initialCapacity = 0)
			: base(
				getSource(sourceScalar, startIndex, countScalar, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTaking = sourceScalar;
			_countScalar = countScalar;
			_startIndex = startIndex;
		}

		[ObservableComputationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			int count)
			: base(
				getSource(sourceScalar, startIndex, count),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTaking = sourceScalar;
			_countTaking = count;
			_startIndex = startIndex;
		}

		[ObservableComputationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countScalar,
			int initialCapacity = 0)
			: base(
				getSource(source, startIndexScalar, countScalar, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceTaking = source;
			_countScalar = countScalar;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableComputationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			int count)
			: base(
				getSource(source, startIndexScalar, count),
				zipPair => zipPair.RightItem)
		{
			_sourceTaking = source;
			_countTaking = count;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableComputationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			int startIndex,
			IReadScalar<int> countScalar,
			int initialCapacity = 0)
			: base(
				getSource(source, startIndex, countScalar, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceTaking = source;
			_countScalar = countScalar;
			_startIndex = startIndex;
		}

		[ObservableComputationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			int startIndex,
			int count)
			: base(
				getSource(source, startIndex, count),
				zipPair => zipPair.RightItem)
		{
			_sourceTaking = source;
			_countTaking = count;
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

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarTaking.getValue(_sourceTaking, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int startIndex = _startIndexScalar.getValue(_startIndex);
			int count = _countScalar.getValue(_countTaking);

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(startIndex).Take(count)))
			{
				throw new ObservableComputationsException(this, "Consistency violation: Taking.1");
			}
		}
	}
}
