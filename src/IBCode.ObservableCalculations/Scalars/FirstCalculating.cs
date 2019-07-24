using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class FirstCalculating<TSourceItem> : ItemCalculating<TSourceItem>
	{
		[ObservableCalculationsCall]
		public FirstCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> defaultValueScalar = null) : base(sourceScalar, 0, defaultValueScalar)
		{
		}

		[ObservableCalculationsCall]
		public FirstCalculating(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> defaultValueScalar = null) : base(source, 0, defaultValueScalar)
		{
		}

		[ObservableCalculationsCall]
		public FirstCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem defaultValue = default(TSourceItem)) : base(sourceScalar, 0, defaultValue)
		{
		}

		[ObservableCalculationsCall]
		public FirstCalculating(
			INotifyCollectionChanged source,
			TSourceItem defaultValue = default(TSourceItem)) : base(source, 0, defaultValue)
		{
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());
			TSourceItem defaultValue = _defaultValueScalar.getValue(_defaultValue);

			if (!EqualityComparer<TSourceItem>.Default.Equals(_value, source.Count > 0 ? source.First() : defaultValue))
				throw new ObservableCalculationsException("Consistency violation: FirstCalculating.1");
		}

	}
}
