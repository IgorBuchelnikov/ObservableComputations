using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Averaging<TSourceItem, TResult> : Computing<TResult>, IHasSourceCollections
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		public virtual ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public virtual ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;

		[ObservableComputationsCall]
		public Averaging(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : base(getValueExpression(sourceScalar))
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Averaging(
			INotifyCollectionChanged source) : base(getValueExpression(source))
		{
			_source = source;
		}

		private static Expression<Func<TResult>> getValueExpression(
			IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			Expression<Func<TSourceItem>> summarizingExpression = () => sourceScalar.Summarizing<TSourceItem>().Value;
			Expression<Func<int>> countExpression = () => sourceScalar.Value != null ? ((IList)sourceScalar.Value).Count : 0;	
			
			return getExpression(summarizingExpression, countExpression);
		}

		private static Expression<Func<TResult>> getValueExpression(
			INotifyCollectionChanged source)
		{
			Expression<Func<TSourceItem>> summarizingExpression = () => source.Summarizing<TSourceItem>().Value;
			Expression<Func<int>> countExpression = () => ((IList)source).Count;	
			
			return getExpression(summarizingExpression, countExpression);
		}

		private static Expression<Func<TResult>> getExpression(Expression<Func<TSourceItem>> summarizingExpression, Expression<Func<int>> countExpression)
		{
			return Expression.Lambda<Func<TResult>>(
				Expression.Divide(
					Expression.Convert(summarizingExpression.Body, typeof(TResult)),
					Expression.Convert(countExpression.Body, typeof(TResult))));
		}

		public void ValidateConsistency()
		{
			IList<int> source = _sourceScalar.getValue(_source, new ObservableCollection<int>()) as IList<int>;
			Averaging<int, double> @this = this as Averaging<int, double>;

			// ReSharper disable once PossibleNullReferenceException
			if (source.Count > 0)
			{
				// ReSharper disable once PossibleNullReferenceException
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (source.Average() != @this.Value) throw new ObservableComputationsException(this, "Consistency violation: Averaging.1");
			}
			else
			{
				// ReSharper disable once PossibleNullReferenceException
				if (!double.IsNaN(@this.Value)) throw new ObservableComputationsException(this, "Consistency violation: Averaging.2");
			}
		}
	}
}
