using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using ObservableComputations.Common;
using ObservableComputations.Common.Interface;

namespace ObservableComputations
{
	public class AllComputing<TSourceItem> : Computing<bool>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpression;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;

		[ObservableComputationsCall]
		public AllComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression) : base(getValueExpression(sourceScalar, predicateExpression))
		{
			_sourceScalar = sourceScalar;
			_predicateExpression = predicateExpression;
		}

		[ObservableComputationsCall]
		public AllComputing(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, bool>> predicateExpression) : base(getValueExpression(source, predicateExpression))
		{
			_source = source;
			_predicateExpression = predicateExpression;
		}

		private static Expression<Func<bool>> getValueExpression(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, bool>> predicateExpression)
		{
			Expression<Func<TSourceItem, bool>> negativePredicateExpression = getNegativePredicateExpression(predicateExpression);

			return () => !sourceScalar.AnyComputing(negativePredicateExpression).Value;
		}

		private static Expression<Func<bool>> getValueExpression(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, bool>> predicateExpression)
		{
			Expression<Func<TSourceItem, bool>> negativePredicateExpression = getNegativePredicateExpression(predicateExpression);

			return () => !source.AnyComputing(negativePredicateExpression).Value;
		}

		private static Expression<Func<TSourceItem, bool>> getNegativePredicateExpression(Expression<Func<TSourceItem, bool>> predicateExpression)
		{
			Expression<Func<TSourceItem, bool>> negativePredicateExpression =
				Expression.Lambda<Func<TSourceItem, bool>>(
					Expression.Not(predicateExpression.Body), predicateExpression.Parameters);
			return negativePredicateExpression;
		}

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());

			Func<TSourceItem, bool> predicate = _predicateExpression.Compile();

			if (_value != source.All(predicate))
				throw new ObservableComputationsException(this, "Consistency violation: AllComputing.1");
		}
	}
}
