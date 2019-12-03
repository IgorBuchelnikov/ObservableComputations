using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq.Expressions;
using ObservableComputations.Common;
using ObservableComputations.Common.Interface;

namespace ObservableComputations
{
	public class Summarizing<TSourceItem> : Aggregating<TSourceItem, TSourceItem>, IHasSources
	{
		public new IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarSummarizing;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public new INotifyCollectionChanged Source => _sourceSummarizing;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarSummarizing;
		private readonly INotifyCollectionChanged _sourceSummarizing;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public Summarizing(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : base(sourceScalar, getSummarizingFuncs())
		{
			_sourceScalarSummarizing = sourceScalar;
		}

		[ObservableComputationsCall]
		public Summarizing(
			INotifyCollectionChanged source) : base(source, getSummarizingFuncs())
		{
			_sourceSummarizing = source;
		}

		private static (Func<TSourceItem, TSourceItem, TSourceItem> aggregateFunc, Func<TSourceItem, TSourceItem, TSourceItem> deaggregateFunc) getSummarizingFuncs()
		{
			ParameterExpression sourceItemParameterExpression = Expression.Parameter(typeof(TSourceItem), "sourceItem");
			ParameterExpression aggregateParameterExpression = Expression.Parameter(typeof(TSourceItem), "aggregate");

			Func<TSourceItem, TSourceItem, TSourceItem> aggregateFunc =
				(Func<TSourceItem, TSourceItem, TSourceItem>) Expression
					.Lambda(Expression.Add(aggregateParameterExpression, sourceItemParameterExpression),
						new[]{sourceItemParameterExpression, aggregateParameterExpression}).Compile();

			Func<TSourceItem, TSourceItem, TSourceItem> deaggregateFunc =
				(Func<TSourceItem, TSourceItem, TSourceItem>) Expression
					.Lambda(Expression.Subtract(aggregateParameterExpression, sourceItemParameterExpression),
						new[] {sourceItemParameterExpression, aggregateParameterExpression}).Compile();

			return (aggregateFunc, deaggregateFunc);
		}

		public new void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalarSummarizing.getValue(_sourceSummarizing, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			Func<TSourceItem, TSourceItem, TSourceItem> aggregateFunc = getSummarizingFuncs().aggregateFunc;

			TSourceItem result = default(TSourceItem);
			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			for (int index = 0; index < sourceCount; index++)
			{
				TSourceItem sourceItem = source[index];
				result = aggregateFunc(result, sourceItem);
			}

			if (!Value.Equals(result))
				throw new ObservableComputationsException("Consistency violation: Summarizing.1");
		}

	}
}
