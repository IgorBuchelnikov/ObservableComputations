using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class Taking<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, TSourceItem>, IHasSources
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

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public int StartIndex => _startIndex;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarTaking;
		private readonly INotifyCollectionChanged _sourceTaking;
		private readonly IReadScalar<int> _countScalar;
		private readonly int _countTaking;
		private readonly IReadScalar<int> _startIndexScalar;
		private readonly int _startIndex;

		// ReSharper disable once MemberCanBePrivate.Global


		[ObservableCalculationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countScalar,
			int capacity = 0)
			: base(
				getSource(sourceScalar, startIndexScalar, countScalar, capacity),
				zipPair => zipPair.ItemRight)
		{
			_sourceScalarTaking = sourceScalar;
			_countScalar = countScalar;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableCalculationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			int count)
			: base(
				getSource(sourceScalar, startIndexScalar, count),
				zipPair => zipPair.ItemRight)
		{
			_sourceScalarTaking = sourceScalar;
			_countTaking = count;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableCalculationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			IReadScalar<int> countScalar,
			int capacity = 0)
			: base(
				getSource(sourceScalar, startIndex, countScalar, capacity),
				zipPair => zipPair.ItemRight)
		{
			_sourceScalarTaking = sourceScalar;
			_countScalar = countScalar;
			_startIndex = startIndex;
		}

		[ObservableCalculationsCall]
		public Taking(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			int count)
			: base(
				getSource(sourceScalar, startIndex, count),
				zipPair => zipPair.ItemRight)
		{
			_sourceScalarTaking = sourceScalar;
			_countTaking = count;
			_startIndex = startIndex;
		}

		[ObservableCalculationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countScalar,
			int capacity = 0)
			: base(
				getSource(source, startIndexScalar, countScalar, capacity),
				zipPair => zipPair.ItemRight)
		{
			_sourceTaking = source;
			_countScalar = countScalar;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableCalculationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			int count)
			: base(
				getSource(source, startIndexScalar, count),
				zipPair => zipPair.ItemRight)
		{
			_sourceTaking = source;
			_countTaking = count;
			_startIndexScalar = startIndexScalar;
		}

		[ObservableCalculationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			int startIndex,
			IReadScalar<int> countScalar,
			int capacity = 0)
			: base(
				getSource(source, startIndex, countScalar, capacity),
				zipPair => zipPair.ItemRight)
		{
			_sourceTaking = source;
			_countScalar = countScalar;
			_startIndex = startIndex;
		}

		[ObservableCalculationsCall]
		public Taking(			
			INotifyCollectionChanged source, 
			int startIndex,
			int count)
			: base(
				getSource(source, startIndex, count),
				zipPair => zipPair.ItemRight)
		{
			_sourceTaking = source;
			_countTaking = count;
			_startIndex = startIndex;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countScalar,
			int capacity)
		{
			return 
				Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Calculating().SequenceCalculating()
					.Zipping<int, TSourceItem>(sourceScalar).Using(zipping => 
					zipping.Filtering(zp => zp.ItemLeft >= startIndexScalar.Value && zp.ItemLeft < startIndexScalar.Value + countScalar.Value, capacity)).Value;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> startIndexScalar,
			int count)
		{
			return 
				Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Calculating().SequenceCalculating()
					.Zipping<int, TSourceItem>(sourceScalar).Using(zipping => 
					zipping.Filtering(zp => zp.ItemLeft >= startIndexScalar.Value && zp.ItemLeft < startIndexScalar.Value + count, count)).Value;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			IReadScalar<int> countScalar,
			int capacity)
		{
			return 
				Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Calculating().SequenceCalculating()
					.Zipping<int, TSourceItem>(sourceScalar).Using(zipping => 
					zipping.Filtering(zp => zp.ItemLeft >= startIndex && zp.ItemLeft < startIndex + countScalar.Value, capacity)).Value;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int startIndex,
			int count)
		{
			return 
				Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Calculating().SequenceCalculating()
					.Zipping<int, TSourceItem>(sourceScalar).Using(zipping => 
					zipping.Filtering(zp => zp.ItemLeft >= startIndex && zp.ItemLeft < startIndex + count, count)).Value;
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			IReadScalar<int> countScalar,
			int capacity)
		{
			return 
				Expr.Is(() => ((IList) source).Count).Calculating().SequenceCalculating()
					.Zipping<int, TSourceItem>(source).Using(zipping => 
					zipping.Filtering(zp => zp.ItemLeft >= startIndexScalar.Value && zp.ItemLeft < startIndexScalar.Value + countScalar.Value, capacity)).Value;
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			IReadScalar<int> startIndexScalar,
			int count)
		{
			return 
				Expr.Is(() => ((IList) source).Count).Calculating().SequenceCalculating()
					.Zipping<int, TSourceItem>(source).Using(zipping => 
					zipping.Filtering(zp => zp.ItemLeft >= startIndexScalar.Value && zp.ItemLeft < startIndexScalar.Value + count, count)).Value;
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			int startIndex,
			IReadScalar<int> countScalar,
			int capacity)
		{
			return 
				Expr.Is(() => ((IList) source).Count).Calculating().SequenceCalculating()
					.Zipping<int, TSourceItem>(source).Using(zipping => 
					zipping.Filtering(zp => zp.ItemLeft >= startIndex && zp.ItemLeft < startIndex + countScalar.Value, capacity)).Value;
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			int startIndex,
			int count)
		{
			return 
				Expr.Is(() => ((IList) source).Count).Calculating().SequenceCalculating()
					.Zipping<int, TSourceItem>(source).Using(zipping => 
					zipping.Filtering(zp => zp.ItemLeft >= startIndex && zp.ItemLeft < startIndex + count, count)).Value;
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarTaking.getValue(_sourceTaking, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int startIndex = _startIndexScalar.getValue(_startIndex);
			int count = _countScalar.getValue(_countTaking);

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(startIndex).Take(count)))
			{
				throw new ObservableCalculationsException("Consistency violation: Taking.1");
			}
		}
	}
}
