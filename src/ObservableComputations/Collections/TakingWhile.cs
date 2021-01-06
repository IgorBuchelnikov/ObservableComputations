using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class TakingWhile<TSourceItem> : Selecting<ZipPair<int, TSourceItem>, TSourceItem>, IHasSourceCollections
	{
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarTakingWhile;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _sourceTakingWhile;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpression;
		public Expression<Func<TSourceItem, int, bool>> IndexedPredicateExpression => _indexedPredicateExpression;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public override int InitialCapacity => ((IHasInitialCapacity)_source).InitialCapacity;

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarTakingWhile;
		private readonly INotifyCollectionChanged _sourceTakingWhile;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;
		private readonly Expression<Func<TSourceItem, int, bool>> _indexedPredicateExpression;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public TakingWhile(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, int, bool>> indexedPredicateExpression,
			int initialCapacity = 0)
			: base(
				getSource(sourceScalar, indexedPredicateExpression, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceScalarTakingWhile = sourceScalar;
			_indexedPredicateExpression = indexedPredicateExpression;
		}

		[ObservableComputationsCall]
		public TakingWhile(			
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, int, bool>> indexedPredicateExpression,
			int initialCapacity = 0)
			: base(
				getSource(source, indexedPredicateExpression, initialCapacity),
				zipPair => zipPair.RightItem)
		{
			_sourceTakingWhile = source;
			_indexedPredicateExpression = indexedPredicateExpression;
		}

		[ObservableComputationsCall]
		public TakingWhile(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int initialCapacity = 0) : this(sourceScalar, predicateExpression.getIndexedPredicate(), initialCapacity)
		{
			_sourceScalarTakingWhile = sourceScalar;
			_predicateExpression = predicateExpression;
		}

		[ObservableComputationsCall]
		public TakingWhile(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int initialCapacity = 0) : this(source, predicateExpression.getIndexedPredicate(), initialCapacity)
		{
			_sourceTakingWhile = source;
			_predicateExpression = predicateExpression;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, int, bool>> predicateExpression,
			int initialCapacity)
		{
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairNotPredicateExpression = getZipPairNotPredicateExpression(predicateExpression);

			Computing<int> countComputing = Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Computing();

			Zipping<int, TSourceItem> zipping = countComputing.SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar);

			return getFiltering(zipping, zipPairNotPredicateExpression, countComputing, initialCapacity);

			//return () => (INotifyCollectionChanged)Expr.Is(() => (INotifyCollectionChanged)getSource.Computing().Using(sc =>
			//			Expr.Is(() => ((IList)sc.Value).Count).SequenceComputing()
			//				.Zipping<int, TSourceItem>(() => sc.Value)).Value).Computing().Using(zipping => zipping.Value.Filtering<ZipPair<int, TSourceItem>>(zp => zp.ItemLeft < zipping.Value.Filtering(zipPairNotPredicateExpression).Selecting(zp1 => zp1.ItemLeft).Minimazing(() => (((IList)zipping.Value).Count)).Value)).Value;
		}

		private static Filtering<ZipPair<int, TSourceItem>> getFiltering(
			Zipping<int, TSourceItem> zipping, 
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairNotPredicateExpression, 
			Computing<int> countComputing,
			int initialCapacity)
		{
			return zipping.Filtering(zp => 
				zp.LeftItem < 
					zipping
					.Filtering(zipPairNotPredicateExpression, initialCapacity)
					.Selecting(zp1 => zp1.LeftItem)
					.Using(ic => ic.Count > 0 ? ic.Minimazing().Value : countComputing.Value)
					.Value, 
				initialCapacity);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, int, bool>> predicateExpression,
			int initialCapacity)
		{
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairNotPredicateExpression = getZipPairNotPredicateExpression(predicateExpression);

			Computing<int> countComputing = Expr.Is(() => source != null ? ((IList)source).Count : 0).Computing();

			Zipping<int, TSourceItem> zipping = countComputing.SequenceComputing()
				.Zipping<int, TSourceItem>(source);

			return getFiltering(zipping, zipPairNotPredicateExpression, countComputing, initialCapacity);
		}

		private static Expression<Func<ZipPair<int, TSourceItem>, bool>> getZipPairNotPredicateExpression(Expression<Func<TSourceItem, int, bool>> predicateExpression)
		{
			ParameterExpression zipPairParameterExpression
				= Expression.Parameter(typeof(ZipPair<int, TSourceItem>), "zipPair");
			Expression zipPairIndexExpression
				= Expression.PropertyOrField(
					zipPairParameterExpression,
					nameof(ZipPair<int, TSourceItem>.LeftItem));
			Expression zipPairItemExpression
				= Expression.PropertyOrField(
					zipPairParameterExpression,
					nameof(ZipPair<int, TSourceItem>.RightItem));
			ReplaceParameterVisitor replaceParameterVisitor
				= new ReplaceParameterVisitor(
					predicateExpression.Parameters,
					new[] {zipPairItemExpression, zipPairIndexExpression});
			Expression<Func<ZipPair<int, TSourceItem>, bool>> zipPairNotPredicateExpression
				= Expression.Lambda<Func<ZipPair<int, TSourceItem>, bool>>(
					// ReSharper disable once AssignNullToNotNullAttribute
					Expression.Not(replaceParameterVisitor.Visit(predicateExpression.Body)),
					zipPairParameterExpression);
			return zipPairNotPredicateExpression;
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarTakingWhile.getValue(_sourceTakingWhile, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			OcConsumer ocConsumer = new OcConsumer();

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(source.TakeWhile((si, i) => new Computing<bool>(_indexedPredicateExpression.ApplyParameters(si, i)).For(ocConsumer).Value)))
			{
				throw new ObservableComputationsException(this, "Consistency violation: TakingWhile.1");
			}
		}
	}
}
