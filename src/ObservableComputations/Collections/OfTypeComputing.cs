using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ObservableComputations.Common;
using ObservableComputations.Common.Interface;

namespace ObservableComputations
{
	public class OfTypeComputing<TResultItem> : Casting<TResultItem>, IHasSources
	{
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarOfTypeComputing;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _sourceOfTypeComputing;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarOfTypeComputing;
		private readonly INotifyCollectionChanged _sourceOfTypeComputing;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public OfTypeComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : base(getSource(sourceScalar))
		{
			_sourceScalarOfTypeComputing = sourceScalar;
		}

		[ObservableComputationsCall]
		public OfTypeComputing(
			INotifyCollectionChanged source) : base(getSource(source))
		{
			_sourceOfTypeComputing = source;
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
			IList source = _sourceScalarOfTypeComputing.getValue(_sourceOfTypeComputing, new ObservableCollection<object>()) as IList;

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.OfType<TResultItem>()))
				 throw new ObservableComputationsException(this, "Consistency violation: OfTypeComputing.1");
		}
	}
}
