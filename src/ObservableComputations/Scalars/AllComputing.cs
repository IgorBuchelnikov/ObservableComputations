// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class AllComputing<TSourceItem> : Computing<bool>, IHasSourceCollections
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpression;

		public virtual ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public virtual ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

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

		[ExcludeFromCodeCoverage]
		internal void ValidateConsistency()
		{
			IList<TSourceItem> source = (IList<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>());

			Func<TSourceItem, bool> predicate = _predicateExpression.Compile();

			if (_value != source.All(predicate))
				throw new ObservableComputationsException(this, "Consistency violation: AllComputing.1");
		}
	}
}
