using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ObservableComputations.Common;
using ObservableComputations.Common.Interface;

namespace ObservableComputations
{
	public class Skipping<TSourceItem> : Taking<TSourceItem>, IHasSources
	{
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarSkipping;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _sourceSkipping;

		// ReSharper disable once MemberCanBePrivate.Global
		public new IReadScalar<int> CountScalar => _countScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public int CountSkipping => _countSkipping;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarSkipping;
		private readonly INotifyCollectionChanged _sourceSkipping;
		private readonly IReadScalar<int> _countScalar;
		private readonly int _countSkipping;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public Skipping(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> countScalar,
			int capacity = 0) : 
			base(
				sourceScalar,
				countScalar,
				getCount(sourceScalar),
				capacity)
		{
			_sourceScalarSkipping = sourceScalar;
			_countScalar = countScalar;
		}

		[ObservableComputationsCall]
		public Skipping(
			INotifyCollectionChanged source, 
			IReadScalar<int> countScalar,
			int capacity = 0) : 
			base(
				source,
				countScalar,
				getCount(source),
				capacity)
		{
			_sourceSkipping = source;
			_countScalar = countScalar;
		}

		[ObservableComputationsCall]
		public Skipping(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int count,
			int capacity = 0) : 
			base(
				sourceScalar,
				count,
				getCount(sourceScalar),
				capacity)
		{
			_sourceScalarSkipping = sourceScalar;
			_countSkipping = count;
		}

		[ObservableComputationsCall]
		public Skipping(
			INotifyCollectionChanged source, 
			int count,
			int capacity = 0) : 
			base(
				source,
				count,
				getCount(source),
				capacity)
		{
			_sourceSkipping = source;
			_countSkipping = count;
		}

		private static IReadScalar<int> getCount(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			return Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Computing();
		}

		private static IReadScalar<int> getCount(INotifyCollectionChanged source)
		{
			return Expr.Is(() => ((IList)source).Count).Computing();
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarSkipping.getValue(_sourceSkipping, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int count = _countScalar.getValue(_countSkipping);

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(count)))
			{
				throw new ObservableComputationsException(this, "Consistency violation: Skipping.1");
			}
		}

	}
}
