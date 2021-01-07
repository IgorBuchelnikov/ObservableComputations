using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ObservableComputations
{
	public class Skipping<TSourceItem> : Taking<TSourceItem>, IHasSourceCollections
	{
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarSkipping;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _sourceSkipping;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> SkippingCountScalar => _skippingCountScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public int SkippingCount => _skippingCount;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarSkipping;
		private readonly INotifyCollectionChanged _sourceSkipping;
		private readonly IReadScalar<int> _skippingCountScalar;
		private readonly int _skippingCount;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public Skipping(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			IReadScalar<int> skippingCountScalar,
			int initialCapacity = 0) : 
			base(
				sourceScalar,
				skippingCountScalar,
				getCount(sourceScalar),
				initialCapacity)
		{
			_sourceScalarSkipping = sourceScalar;
			_skippingCountScalar = skippingCountScalar;
		}

		[ObservableComputationsCall]
		public Skipping(
			INotifyCollectionChanged source, 
			IReadScalar<int> skippingCountScalar,
			int initialCapacity = 0) : 
			base(
				source,
				skippingCountScalar,
				getCount(source),
				initialCapacity)
		{
			_sourceSkipping = source;
			_skippingCountScalar = skippingCountScalar;
		}

		[ObservableComputationsCall]
		public Skipping(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			int skippingCount,
			int initialCapacity = 0) : 
			base(
				sourceScalar,
				skippingCount,
				getCount(sourceScalar),
				initialCapacity)
		{
			_sourceScalarSkipping = sourceScalar;
			_skippingCount = skippingCount;
		}

		[ObservableComputationsCall]
		public Skipping(
			INotifyCollectionChanged source, 
			int skippingCount,
			int initialCapacity = 0) : 
			base(
				source,
				skippingCount,
				getCount(source),
				initialCapacity)
		{
			_sourceSkipping = source;
			_skippingCount = skippingCount;
		}

		private static IReadScalar<int> getCount(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			return Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Computing();
		}

		private static IReadScalar<int> getCount(INotifyCollectionChanged source)
		{
			return Expr.Is(() => ((IList)source).Count).Computing();
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarSkipping.getValue(_sourceSkipping, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int count = _skippingCountScalar.getValue(_skippingCount);

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.Skip(count)))
			{
				throw new ObservableComputationsException(this, "Consistency violation: Skipping.1");
			}
		}

	}
}
