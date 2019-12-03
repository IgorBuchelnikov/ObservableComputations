using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using ObservableComputations.Common;
using ObservableComputations.Common.Interface;

namespace ObservableComputations
{
	public class IndicesComputing<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, int>, IHasSources
	{
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpression;

		// ReSharper disable once MemberCanBePrivate.Global
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _source;

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		[ObservableComputationsCall]
		public IndicesComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int capacity = 0) : base(getSource(sourceScalar, predicateExpression, capacity), pair => pair.ItemLeft) 
		{
			_predicateExpression = predicateExpression;
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public IndicesComputing(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int capacity = 0) : base(getSource(source, predicateExpression, capacity), pair => pair.ItemLeft) 
		{
			_predicateExpression = predicateExpression;
			_source = source;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int capacity)
		{
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairPredicateExpression = getZipPairPredicateExpression(predicateExpression);

			return Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Computing().SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar)
				.Filtering(zipPairPredicateExpression, capacity);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int capacity)
		{
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairPredicateExpression = getZipPairPredicateExpression(predicateExpression);

			return Expr.Is(() => ((IList) source).Count).Computing().SequenceComputing()
				.Zipping<int, TSourceItem>(source)
				.Filtering(zipPairPredicateExpression, capacity);
		}

		private static Expression<Func<ZipPair<int, TSourceItem>, bool>> getZipPairPredicateExpression(Expression<Func<TSourceItem, bool>> predicateExpression)
		{
			ParameterExpression zipPairParameterExpression
				= Expression.Parameter(typeof(ZipPair<int, TSourceItem>), "zipPair");
			Expression zipPairItem2Expression
				= Expression.PropertyOrField(
					zipPairParameterExpression,
					nameof(ZipPair<int, TSourceItem>.ItemRight));
			ReplaceParameterVisitor replaceParameterVisitor
				= new ReplaceParameterVisitor(
					predicateExpression.Parameters[0],
					zipPairItem2Expression);
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairPredicateExpression
				= Expression.Lambda<Func<ZipPair<int, TSourceItem>, bool>>(
					replaceParameterVisitor.Visit(predicateExpression.Body),
					zipPairParameterExpression);
			return zipPairPredicateExpression;
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			Func<TSourceItem, bool> predicate = _predicateExpression.Compile();

			List<int> result = new List<int>();

			// ReSharper disable once PossibleNullReferenceException
			for (int i = 0; i < source.Count; i++)
			{
				if (predicate(source[i]))
				{
					result.Add(i);
				}
			}

			if (!this.SequenceEqual(result)) throw new ObservableComputationsException("Consistency violation: IndicesComputing.1");
		}

		//private class FindExpressionVisitor : ExpressionVisitor
		//{
		//	private List<Expression> _result = new List<Expression>();
		//	public IEnumerable<Expression> Result => _result;

		//	Func<Expression, (bool, bool)> _predicate;
		//	public FindExpressionVisitor(Func<Expression, (bool, bool)> predicate)
		//	{
		//		_predicate = predicate;
		//	}

		//	public override Expression Visit(Expression node)
		//	{
		//		(bool includeInResult, bool stopFind) = _predicate(node);

		//		if (includeInResult) _result.Add(node);
		//		if (stopFind) return null;

		//		return base.Visit(node);
		//	}
		//}
	}
}
