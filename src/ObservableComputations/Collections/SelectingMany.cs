using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class SelectingMany<TSourceItem, TResultItem> : Concatenating<TResultItem>, IHasSourceCollections
	{
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly INotifyCollectionChanged _source;
		private readonly Expression<Func<TSourceItem, INotifyCollectionChanged>> _selectorExpression;
		private readonly Expression<Func<TSourceItem, int, INotifyCollectionChanged>> _selectorWithIndexExpression;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, INotifyCollectionChanged>> SelectorExpression => _selectorExpression;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, int, INotifyCollectionChanged>> SelectorWithIndexExpression => _selectorWithIndexExpression;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		[ObservableComputationsCall]
		public SelectingMany(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, INotifyCollectionChanged>> selectorExpression)
			: base(
				getSource(sourceScalar, selectorExpression))
		{
			_sourceScalar = sourceScalar;
			_selectorExpression = selectorExpression;
		}

		[ObservableComputationsCall]
		public SelectingMany(			
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, INotifyCollectionChanged>> selectorExpression)
			: base(
				getSource(source, selectorExpression))
		{
			_source = source;
			_selectorExpression = selectorExpression;
		}

		[ObservableComputationsCall]
		public SelectingMany(			
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, int, INotifyCollectionChanged>> selectorWithIndexExpression)
			: base(
				getSource(sourceScalar, selectorWithIndexExpression))
		{
			_sourceScalar = sourceScalar;
			_selectorWithIndexExpression = selectorWithIndexExpression;
		}

		[ObservableComputationsCall]
		public SelectingMany(			
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, int, INotifyCollectionChanged>> selectorWithIndexExpression)
			: base(
				getSource(source, selectorWithIndexExpression))
		{
			_source = source;
			_selectorWithIndexExpression = selectorWithIndexExpression;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, INotifyCollectionChanged>> selectorExpression)
		{
			return sourceScalar.Selecting(selectorExpression);
		}


		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, INotifyCollectionChanged>> selectorExpression)
		{
			return source.Selecting(selectorExpression);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, int, INotifyCollectionChanged>> selectorExpression)
		{
			return Expr.Is(() => sourceScalar.Value != null ? ((IList) sourceScalar.Value).Count : 0).Computing().SequenceComputing()
				.Zipping<int, TSourceItem>(sourceScalar)
				.Selecting(getZipPairSelectorExpression(selectorExpression));
		}


		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, int, INotifyCollectionChanged>> selectorExpression)
		{
			return Expr.Is(() => ((IList) source).Count).Computing().SequenceComputing()
				.Zipping<int, TSourceItem>(source)
				.Selecting(getZipPairSelectorExpression(selectorExpression));
		}

		private static Expression<Func<ZipPair<int, TSourceItem>, INotifyCollectionChanged>> getZipPairSelectorExpression(Expression<Func<TSourceItem, int, INotifyCollectionChanged>> selectorExpression)
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
					selectorExpression.Parameters,
					new[] {zipPairItemExpression, zipPairIndexExpression});
			Expression<Func<ZipPair<int, TSourceItem>, INotifyCollectionChanged>> zipPairSelectorExpression
				= Expression.Lambda<Func<ZipPair<int, TSourceItem>, INotifyCollectionChanged>>(
					// ReSharper disable once AssignNullToNotNullAttribute
					replaceParameterVisitor.Visit(selectorExpression.Body),
					zipPairParameterExpression);
			return zipPairSelectorExpression;
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			Func<TSourceItem, INotifyCollectionChanged> selector = _selectorExpression?.Compile();
			Func<TSourceItem, int, INotifyCollectionChanged> selectorWithIndex = _selectorWithIndexExpression?.Compile();

			List<TResultItem> result = new List<TResultItem>();
			// ReSharper disable once PossibleNullReferenceException
			for (int index = 0; index < source.Count; index++)
			{
				TSourceItem sourceItem = source[index];
				// ReSharper disable once PossibleNullReferenceException
				result.AddRange(selector != null ? (IEnumerable<TResultItem>) selector(sourceItem) : (IEnumerable<TResultItem>) selectorWithIndex(sourceItem, index));
			}

			// ReSharper disable once AssignNullToNotNullAttribute
			if (!this.SequenceEqual(result))
			{
				throw new ObservableComputationsException(this, "Consistency violation: SelectingMany.1");
			}
		}
	}
}
