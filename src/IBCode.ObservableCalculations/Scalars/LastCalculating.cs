using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class LastCalculating<TSourceItem> : ItemCalculating<TSourceItem>
	{
		[ObservableCalculationsCall]
		public LastCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<TSourceItem> defaultValueScalar = null) : base(sourceScalar, getIndex(sourceScalar), defaultValueScalar)
		{
		}

		[ObservableCalculationsCall]
		public LastCalculating(
			INotifyCollectionChanged source,
			IReadScalar<TSourceItem> defaultValueScalar = null) : base(source, getIndex(source), defaultValueScalar)
		{
		}

		[ObservableCalculationsCall]
		public LastCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem defaultValue = default(TSourceItem)) : base(sourceScalar, getIndex(sourceScalar), defaultValue)
		{
		}

		[ObservableCalculationsCall]
		public LastCalculating(
			INotifyCollectionChanged source,
			TSourceItem defaultValue = default(TSourceItem)) : base(source, getIndex(source), defaultValue)
		{
		}

		private static IReadScalar<int> getIndex(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			IReadScalar<IList> listCalculating = sourceScalar.Using(s => (IList)s.Value);
			Expression<Func<int>> indexExpression = () => listCalculating.Value != null && listCalculating.Value.Count > 0 ? listCalculating.Value.Count - 1 : 0;
			return indexExpression.Calculating();
		}

		private static IReadScalar<int> getIndex(INotifyCollectionChanged source)
		{
			IList list = (IList)source;
			Expression<Func<int>> indexExpression = () => list.Count > 0 ? list.Count - 1 : 0;
			return indexExpression.Calculating();
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());
			TSourceItem defaultValue = _defaultValueScalar.getValue(_defaultValue);

			if (!EqualityComparer<TSourceItem>.Default.Equals(_value, source.Count > 0 ? source.Last() : defaultValue))
				throw new ObservableCalculationsException("Consistency violation: LastCalculating.1");
		}

	}
}
