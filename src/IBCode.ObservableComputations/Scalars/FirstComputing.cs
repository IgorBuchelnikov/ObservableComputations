using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using ObservableComputations.Common;
using ObservableComputations.Common.Interface;

namespace ObservableComputations
{
	public class FirstComputing<TSourceItem> : ItemComputing<TSourceItem>
	{
		[ObservableComputationsCall]
		public FirstComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> defaultValueScalar = null) : base(sourceScalar, 0, defaultValueScalar)
		{
		}

		[ObservableComputationsCall]
		public FirstComputing(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> defaultValueScalar = null) : base(source, 0, defaultValueScalar)
		{
		}

		[ObservableComputationsCall]
		public FirstComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem defaultValue = default(TSourceItem)) : base(sourceScalar, 0, defaultValue)
		{
		}

		[ObservableComputationsCall]
		public FirstComputing(
			INotifyCollectionChanged source,
			TSourceItem defaultValue = default(TSourceItem)) : base(source, 0, defaultValue)
		{
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());
			TSourceItem defaultValue = _defaultValueScalar.getValue(_defaultValue);

			if (!EqualityComparer<TSourceItem>.Default.Equals(_value, source.Count > 0 ? source.First() : defaultValue))
				throw new ObservableComputationsException("Consistency violation: FirstComputing.1");
		}

	}
}
