using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Summarizing<TSourceItem> : Aggregating<TSourceItem, TSourceItem>, IHasSourceCollections
	{
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarSummarizing;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _sourceSummarizing;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarSummarizing;
		private readonly INotifyCollectionChanged _sourceSummarizing;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public Summarizing(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : base(sourceScalar, aggregateFunc, deaggregateFunc)
		{
			_sourceScalarSummarizing = sourceScalar;
		}

		[ObservableComputationsCall]
		public Summarizing(
			INotifyCollectionChanged source) : base(source, aggregateFunc, deaggregateFunc)
		{
			_sourceSummarizing = source;
		}

		private static Func<TSourceItem, TSourceItem, TSourceItem> _aggregateFunc;
		private static Func<TSourceItem, TSourceItem, TSourceItem> aggregateFunc
		{
			get
			{
				if (_aggregateFunc != null) return _aggregateFunc;

				ParameterExpression sourceItemParameterExpression1 = Summarizing<TSourceItem>.sourceItemParameterExpression;
				ParameterExpression aggregateParameterExpression1 = Summarizing<TSourceItem>.aggregateParameterExpression;

				_aggregateFunc =
					(Func<TSourceItem, TSourceItem, TSourceItem>) Expression
						.Lambda(Expression.Add(aggregateParameterExpression1, sourceItemParameterExpression1),
							new[] {sourceItemParameterExpression1, aggregateParameterExpression1}).Compile();


				return _aggregateFunc;
			}
		}

		private static ParameterExpression _aggregateParameterExpression;
		private static ParameterExpression aggregateParameterExpression => 
			_aggregateParameterExpression = _aggregateParameterExpression ?? Expression.Parameter(typeof(TSourceItem), "aggregate");

		private static ParameterExpression _sourceItemParameterExpression;
		private static ParameterExpression sourceItemParameterExpression => 
			_sourceItemParameterExpression = _sourceItemParameterExpression ?? Expression.Parameter(typeof(TSourceItem), "sourceItem");


		private static Func<TSourceItem, TSourceItem, TSourceItem> _deaggregateFunc;
		private static Func<TSourceItem, TSourceItem, TSourceItem> deaggregateFunc
		{
			get
			{
				if (_deaggregateFunc != null) return _deaggregateFunc;

				ParameterExpression sourceItemParameterExpression1 = Summarizing<TSourceItem>.sourceItemParameterExpression;
				ParameterExpression aggregateParameterExpression1 = Summarizing<TSourceItem>.aggregateParameterExpression;

				_deaggregateFunc =
					(Func<TSourceItem, TSourceItem, TSourceItem>) Expression
						.Lambda(Expression.Subtract(aggregateParameterExpression1, sourceItemParameterExpression1),
							new[] {sourceItemParameterExpression1, aggregateParameterExpression1}).Compile();

				return _deaggregateFunc;
			}

		}

		[ExcludeFromCodeCoverage]
		internal void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarSummarizing.getValue(_sourceSummarizing, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			TSourceItem result = default(TSourceItem);
			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			for (int index = 0; index < sourceCount; index++)
				result = aggregateFunc(result, source[index]);

			if (!Value.Equals(result))
				throw new ObservableComputationsException(this, "Consistency violation: Summarizing.1");
		}

	}
}
