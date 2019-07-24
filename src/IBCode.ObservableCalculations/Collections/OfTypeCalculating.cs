using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class OfTypeCalculating<TResultItem> : Casting<TResultItem>, IHasSources
	{
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarOfTypeCalculating;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _sourceOfTypeCalculating;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarOfTypeCalculating;
		private readonly INotifyCollectionChanged _sourceOfTypeCalculating;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableCalculationsCall]
		public OfTypeCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : base(getSource(sourceScalar))
		{
			_sourceScalarOfTypeCalculating = sourceScalar;
		}

		[ObservableCalculationsCall]
		public OfTypeCalculating(
			INotifyCollectionChanged source) : base(getSource(source))
		{
			_sourceOfTypeCalculating = source;
		}

		private static INotifyCollectionChanged getSource(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			return sourceScalar.Casting<object>().Filtering(item => item is TResultItem);
		}

		private static INotifyCollectionChanged getSource(INotifyCollectionChanged source)
		{
			return source.Casting<object>().Filtering(item => item is TResultItem);
		}

		// ReSharper disable once InconsistentNaming
		public new void ValidateConsistency()
		{
			IList source = _sourceScalarOfTypeCalculating.getValue(_sourceOfTypeCalculating, new ObservableCollection<object>()) as IList;

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.OfType<TResultItem>()))
				 throw new ObservableCalculationsException("Consistency violation: OfTypeCalculating.1");
		}
	}
}
