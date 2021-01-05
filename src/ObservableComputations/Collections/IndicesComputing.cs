using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class IndicesComputing<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, int>, IHasSourceCollections
	{
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpression;

		// ReSharper disable once MemberCanBePrivate.Global
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _source;

		public override int InitialCapacity => ((CollectionComputing<TSourceItem>)_source)._initialCapacity;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		[ObservableComputationsCall]
		public IndicesComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int initialCapacity = 0) : base(getSource(sourceScalar, predicateExpression, initialCapacity), pair => pair.LeftItem) 
		{
			_predicateExpression = predicateExpression;
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public IndicesComputing(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int initialCapacity = 0) : base(getSource(source, predicateExpression, initialCapacity), pair => pair.LeftItem) 
		{
			_predicateExpression = predicateExpression;
			_source = source;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int initialCapacity)
		{
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairPredicateExpression = getZipPairPredicateExpression(predicateExpression);

			return new Computing<int>(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar)
				.Filtering(zipPairPredicateExpression, initialCapacity);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int initialCapacity)
		{
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairPredicateExpression = getZipPairPredicateExpression(predicateExpression);

			return new Computing<int>(() => ((IList) source).Count).SequenceComputing()
				.Zipping<int, TSourceItem>(source)
				.Filtering(zipPairPredicateExpression, initialCapacity);
		}

		private static Expression<Func<ZipPair<int, TSourceItem>, bool>> getZipPairPredicateExpression(Expression<Func<TSourceItem, bool>> predicateExpression)
		{
			ParameterExpression zipPairParameterExpression
				= Expression.Parameter(typeof(ZipPair<int, TSourceItem>), "zipPair");
			Expression zipPairItem2Expression
				= Expression.PropertyOrField(
					zipPairParameterExpression,
					nameof(ZipPair<int, TSourceItem>.RightItem));
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
				if (predicate(source[i]))
					result.Add(i);

			if (!this.SequenceEqual(result)) throw new ObservableComputationsException(this, "Consistency violation: IndicesComputing.1");
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
