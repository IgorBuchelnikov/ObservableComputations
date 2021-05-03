// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Summarizing<TSourceItem> : Aggregating<TSourceItem, TSourceItem>, IHasSources
	{
		public override IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalarSummarizing;

		public override ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public override INotifyCollectionChanged Source => _sourceSummarizing;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalarSummarizing;
		private readonly INotifyCollectionChanged _sourceSummarizing;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public Summarizing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			TSourceItem defaultValue = default) : base(sourceScalar, aggregateFunc, deaggregateFunc, defaultValue)
		{
			_sourceScalarSummarizing = sourceScalar;
		}

		[ObservableComputationsCall]
		public Summarizing(
			INotifyCollectionChanged source,
			TSourceItem defaultValue = default) : base(source, aggregateFunc, deaggregateFunc, defaultValue)
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
		internal new void ValidateInternalConsistency()
		{
			IList<TSourceItem> source = _sourceScalarSummarizing.getValue(_sourceSummarizing, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			TSourceItem result = default(TSourceItem);
			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			for (int index = 0; index < sourceCount; index++)
				result = aggregateFunc(result, source[index]);

			if (!Value.Equals(result))
				throw new ValidateInternalConsistencyException("Consistency violation: Summarizing.1");
		}

	}
}
