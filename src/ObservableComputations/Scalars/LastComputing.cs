using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using ObservableComputations;

namespace ObservableComputations
{
	public class LastComputing<TSourceItem> : ItemComputing<TSourceItem>
	{
		[ObservableComputationsCall]
		public LastComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem defaultValue = default(TSourceItem)) : base(sourceScalar, getIndex(sourceScalar), defaultValue)
		{
		}

		[ObservableComputationsCall]
		public LastComputing(
			INotifyCollectionChanged source,
			TSourceItem defaultValue = default(TSourceItem)) : base(source, getIndex(source), defaultValue)
		{
		}

		private static IReadScalar<int> getIndex(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			IReadScalar<IList> listComputing = sourceScalar.Using(s => (IList)s.Value);
			Expression<Func<int>> indexExpression = () => listComputing.Value != null && listComputing.Value.Count > 0 ? listComputing.Value.Count - 1 : 0;
			return indexExpression.Computing();
		}

		private static IReadScalar<int> getIndex(INotifyCollectionChanged source)
		{
			IList list = (IList)source;
			Expression<Func<int>> indexExpression = () => list.Count > 0 ? list.Count - 1 : 0;
			return indexExpression.Computing();
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());
			TSourceItem defaultValue = _defaultValue;

			if (!EqualityComparer<TSourceItem>.Default.Equals(_value, source.Count > 0 ? source.Last() : defaultValue))
				throw new ObservableComputationsException(this, "Consistency violation: LastComputing.1");
		}

	}
}
