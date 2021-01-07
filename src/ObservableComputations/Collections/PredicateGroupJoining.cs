using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class PredicateGroupJoining<TOuterSourceItem, TInnerSourceItem> : Selecting<TOuterSourceItem, PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>>, IHasSourceCollections
	{
		public IReadScalar<INotifyCollectionChanged> OuterSourceScalar => _outerSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged OuterSource => _outerSource;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> InnerSourceScalar => _innerSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged InnerSource => _innerSource;

		public override ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{OuterSource, InnerSource});
		public override ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{OuterSourceScalar, InnerSourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> JoinPredicateExpression => _joinPredicateExpression;
		private readonly IReadScalar<INotifyCollectionChanged> _outerSourceScalar;
		private readonly INotifyCollectionChanged _outerSource;
		private readonly IReadScalar<INotifyCollectionChanged> _innerSourceScalar;
		private readonly INotifyCollectionChanged _innerSource;
		private readonly Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> _joinPredicateExpression;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public PredicateGroupJoining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression) : base(outerSourceScalar, getSelectorExpression(innerSourceScalar, joinPredicateExpression))
		{
			_outerSourceScalar = outerSourceScalar;
			_innerSourceScalar = innerSourceScalar;
			_joinPredicateExpression = joinPredicateExpression;
		}

		[ObservableComputationsCall]
		public PredicateGroupJoining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			INotifyCollectionChanged innerSource,
			Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression) : base(outerSourceScalar, getSelectorExpression(innerSource, joinPredicateExpression))
		{
			_outerSourceScalar = outerSourceScalar;
			_innerSource = innerSource;
			_joinPredicateExpression = joinPredicateExpression;
		}


		[ObservableComputationsCall]
		public PredicateGroupJoining(
			INotifyCollectionChanged outerSource,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression) : base(outerSource, getSelectorExpression(innerSourceScalar, joinPredicateExpression))
		{
			_outerSource = outerSource;
			_innerSourceScalar = innerSourceScalar;
			_joinPredicateExpression = joinPredicateExpression;
		}

		[ObservableComputationsCall]
		public PredicateGroupJoining(
			INotifyCollectionChanged outerSource,
			INotifyCollectionChanged innerSource,
			Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression) : base(outerSource, getSelectorExpression(innerSource, joinPredicateExpression))
		{
			_outerSource = outerSource;
			_innerSource = innerSource;
			_joinPredicateExpression = joinPredicateExpression;
		}

		private static Expression<Func<TOuterSourceItem, PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>>> getSelectorExpression(IReadScalar<INotifyCollectionChanged> innerSourceScalar, Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			Expression<Func<TInnerSourceItem, bool>> predicateExpression = null;

			Expression<Func<TOuterSourceItem, PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>>> result =
				outerItem => new PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>(innerSourceScalar, predicateExpression, outerItem);

			// ReSharper disable once ExpressionIsAlwaysNull
			return getSelectorExpression(joinPredicateExpression, result, predicateExpression);
		}

		private static Expression<Func<TOuterSourceItem, PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>>> getSelectorExpression(INotifyCollectionChanged innerSource, Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			Expression<Func<TInnerSourceItem, bool>> predicateExpression = null;

			Expression<Func<TOuterSourceItem, PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>>> result =
				outerItem => new PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>(innerSource, predicateExpression, outerItem);

			// ReSharper disable once ExpressionIsAlwaysNull
			return getSelectorExpression(joinPredicateExpression, result, predicateExpression);
		}

		private static Expression<Func<TOuterSourceItem, PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>>> getSelectorExpression(Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression, Expression<Func<TOuterSourceItem, PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>>> result,
			Expression<Func<TInnerSourceItem, bool>> predicateExpression)
		{
			predicateExpression = Expression.Lambda<Func<TInnerSourceItem, bool>>(
				((LambdaExpression)
					new ReplaceParameterVisitor(
							new Dictionary<ParameterExpression, Expression>
							{
								{joinPredicateExpression.Parameters[0], result.Parameters[0]}
							})
						.Visit(joinPredicateExpression)
				).Body,
				joinPredicateExpression.Parameters[1]);

			return (Expression<Func<TOuterSourceItem, PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem>>>)
				new ReplaceMemberVisitor(
						me => me.Member.Name == "predicateExpression" ? predicateExpression : null)
					.Visit(result);
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateConsistency()
		{
			IList<TOuterSourceItem> outerSource = (IList<TOuterSourceItem>) _outerSourceScalar.getValue(_outerSource, new ObservableCollection<TOuterSourceItem>());
			IList<TInnerSourceItem> innerSource = (IList<TInnerSourceItem>) _innerSourceScalar.getValue(_innerSource, new ObservableCollection<TInnerSourceItem>());
			Func<TOuterSourceItem, TInnerSourceItem, bool> joinPredicate = _joinPredicateExpression.Compile();

			var result = outerSource.Select(outerItem => 
				new
				{
					Key = outerItem,
					InnerItems = innerSource.Where(innerItem => joinPredicate(outerItem, innerItem)).ToArray()
				}).ToList();

			if (Count !=  result.Count)
				throw new ObservableComputationsException(this, "Consistency violation: PredicateGroupJoining.1");

			for (int index = 0; index < result.Count; index++)
			{
				var resultItem = result[index];
				PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem> thisItem = this[index];

				int length = resultItem.InnerItems.Length;
				if (length !=  thisItem.Count)
					throw new ObservableComputationsException($"Consistency violation: PredicateGroupJoining.3 {length}");

				EqualityComparer<TInnerSourceItem> equalityComparer = EqualityComparer<TInnerSourceItem>.Default;

				IEnumerator enumerator1 = resultItem.InnerItems.GetEnumerator();
				{
					using (IEnumerator<TInnerSourceItem> enumerator2 = thisItem.GetEnumerator())
					{
						while (enumerator1.MoveNext())
						{
							enumerator2.MoveNext();
							if (!equalityComparer.Equals((TInnerSourceItem)enumerator1.Current, enumerator2.Current))
								throw new ObservableComputationsException(this, "Consistency violation: PredicateGroupJoining.4");
						}
					}
				}
			}
		}
	}

	public class PredicateJoinGroup<TOuterSourceItem, TInnerSourceItem> : Filtering<TInnerSourceItem>
	{
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		public TOuterSourceItem Key { get; }

		[ObservableComputationsCall]
		internal PredicateJoinGroup(IReadScalar<INotifyCollectionChanged> sourceScalar, Expression<Func<TInnerSourceItem, bool>> predicateExpression, TOuterSourceItem key) : base(sourceScalar, predicateExpression)
		{
			Key = key;
		}

		[ObservableComputationsCall]
		internal PredicateJoinGroup(INotifyCollectionChanged source, Expression<Func<TInnerSourceItem, bool>> predicateExpression, TOuterSourceItem key) : base(source, predicateExpression)
		{
			Key = key;
		}
	}

}
