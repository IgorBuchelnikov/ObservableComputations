using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using ObservableComputations.ExtentionMethods;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace ObservableComputations
{
	public class Joining<TOuterSourceItem, TInnerSourceItem> : Filtering<JoinPair<TOuterSourceItem, TInnerSourceItem>>, IHasSources
	{
		public IReadScalar<INotifyCollectionChanged> OuterSourceScalar => _outerSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> InnerSourceScalar => _innerSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged OuterSource => _outerSource;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged InnerSource => _innerSource;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> JoinPredicateExpression { get; }

		public new ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{OuterSource, InnerSource});
		public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{OuterSourceScalar, InnerSourceScalar});

		private readonly IReadScalar<INotifyCollectionChanged> _outerSourceScalar;
		private readonly IReadScalar<INotifyCollectionChanged> _innerSourceScalar;
		private readonly INotifyCollectionChanged _outerSource;
		private readonly INotifyCollectionChanged _innerSource;

		// ReSharper disable once MemberCanBePrivate.Global

		[ObservableComputationsCall]
		public Joining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			int capacity = 0) 
			: base(getSource(outerSourceScalar, innerSourceScalar), getJoinPairPredicateExpression(joinPredicateExpression), capacity)
		{
			_outerSourceScalar = outerSourceScalar;
			_innerSourceScalar = innerSourceScalar;
			JoinPredicateExpression = joinPredicateExpression;
		}

		[ObservableComputationsCall]
		public Joining(
			INotifyCollectionChanged outerSource,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			int capacity = 0) 
			: base(getSource(outerSource, innerSourceScalar), getJoinPairPredicateExpression(joinPredicateExpression), capacity)
		{
			_outerSource = outerSource;
			_innerSourceScalar = innerSourceScalar;
			JoinPredicateExpression = joinPredicateExpression;
		}

		[ObservableComputationsCall]
		public Joining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			INotifyCollectionChanged innerSource,
			Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			int capacity = 0) 
			: base(getSource(outerSourceScalar, innerSource), getJoinPairPredicateExpression(joinPredicateExpression), capacity)
		{
			_outerSourceScalar = outerSourceScalar;
			_innerSource = innerSource;
			JoinPredicateExpression = joinPredicateExpression;
		}

		[ObservableComputationsCall]
		public Joining(
			INotifyCollectionChanged outerSource,
			INotifyCollectionChanged innerSource,
			Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression,
			int capacity = 0) 
			: base(getSource(outerSource, innerSource), getJoinPairPredicateExpression(joinPredicateExpression), capacity)
		{
			_outerSource = outerSource;
			_innerSource = innerSource;
			JoinPredicateExpression = joinPredicateExpression;
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1,
			IReadScalar<INotifyCollectionChanged> source2)
		{
			return 
				source1
				.Crossing<TOuterSourceItem, TInnerSourceItem>(source2);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			IReadScalar<INotifyCollectionChanged> source2)
		{
			return 
				source1
				.Crossing<TOuterSourceItem, TInnerSourceItem>(source2);
		}

		private static INotifyCollectionChanged getSource(
			IReadScalar<INotifyCollectionChanged> source1,
			INotifyCollectionChanged source2)
		{
			return 
				source1
				.Crossing<TOuterSourceItem, TInnerSourceItem>(source2);
		}

		private static INotifyCollectionChanged getSource(
			INotifyCollectionChanged source1,
			INotifyCollectionChanged source2)
		{
			return 
				source1
				.Crossing<TOuterSourceItem, TInnerSourceItem>(source2);
		}

		private static Expression<Func<JoinPair<TOuterSourceItem, TInnerSourceItem>, bool>> getJoinPairPredicateExpression(Expression<Func<TOuterSourceItem, TInnerSourceItem, bool>> joinPredicateExpression)
		{
			ParameterExpression joinPairParameterExpression
				= Expression.Parameter(typeof(JoinPair<TOuterSourceItem, TInnerSourceItem>), "joinPair");
			Expression joinPairOuterItemExpression
				= Expression.PropertyOrField(
					joinPairParameterExpression,
					nameof(JoinPair<TOuterSourceItem, TInnerSourceItem>.OuterItem));
			Expression joinPairInnerItemExpression
				= Expression.PropertyOrField(
					joinPairParameterExpression,
					nameof(JoinPair<TOuterSourceItem, TInnerSourceItem>.InnerItem));
			ReplaceParameterVisitor replaceParameterVisitor
				= new ReplaceParameterVisitor(
					joinPredicateExpression.Parameters,
					new[] {joinPairOuterItemExpression, joinPairInnerItemExpression});
			Expression<Func<JoinPair<TOuterSourceItem, TInnerSourceItem>, bool>> joinPairPredicateExpression
				= Expression.Lambda<Func<JoinPair<TOuterSourceItem, TInnerSourceItem>, bool>>(
					// ReSharper disable once AssignNullToNotNullAttribute
					replaceParameterVisitor.Visit(joinPredicateExpression.Body),
					joinPairParameterExpression);
			return joinPairPredicateExpression;
		}

		public new void ValidateConsistency()
		{
			IList<TOuterSourceItem> source1 = (IList<TOuterSourceItem>) _outerSourceScalar.getValue(_outerSource, new ObservableCollection<TOuterSourceItem>());
			IList<TInnerSourceItem> source2 = (IList<TInnerSourceItem>) _innerSourceScalar.getValue(_innerSource, new ObservableCollection<TInnerSourceItem>());
			Func<TOuterSourceItem, TInnerSourceItem, bool> joinPredicate = JoinPredicateExpression.Compile();

			if (!this.SequenceEqual(source1.SelectMany(item1 => source2.Select(item2 => new JoinPair<TOuterSourceItem, TInnerSourceItem>(item1, item2)).Where(jp => joinPredicate(jp.OuterItem, jp.InnerItem)))))
			{
				throw new ObservableComputationsException(this, "Consistency violation: Joining.1");
			}
		}
	}

	public class JoinPair<TOuterSourceItem, TInnerSourceItem> : IEquatable<JoinPair<TOuterSourceItem, TInnerSourceItem>>, INotifyPropertyChanged
	{
		public Action<JoinPair<TOuterSourceItem, TInnerSourceItem>, TOuterSourceItem> JoinPairSetOuterItemAction
		{
			get => _joinPairSetOuterItemAction;
			set
			{
				if (_joinPairSetOuterItemAction != value)
				{
					_joinPairSetOuterItemAction = value;
					PropertyChanged?.Invoke(this, Utils.JoinPairSetOuterItemActionPropertyChangedEventArgs);
				}
			}
		}

		public Action<JoinPair<TOuterSourceItem, TInnerSourceItem>, TInnerSourceItem> JoinPairSetInnerItemAction
		{
			get => _joinPairSetInnerItemAction;
			set
			{
				if (_joinPairSetInnerItemAction != value)
				{
					_joinPairSetInnerItemAction = value;
					PropertyChanged?.Invoke(this, Utils.JoinPairSetInnerItemActionPropertyChangedEventArgs);
				}
			}
		}

		public TOuterSourceItem OuterItem
		{
			get => _outerItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set => _joinPairSetOuterItemAction(this, value);
		}

		public TInnerSourceItem InnerItem
		{
			get => _innerItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set => _joinPairSetInnerItemAction(this, value);
		}

		readonly EqualityComparer<TOuterSourceItem> _outerSourceItemEqualityComparer = EqualityComparer<TOuterSourceItem>.Default;
		readonly EqualityComparer<TInnerSourceItem> _innerSourceItemEqualityComparer = EqualityComparer<TInnerSourceItem>.Default;

		private Action<JoinPair<TOuterSourceItem, TInnerSourceItem>, TOuterSourceItem> _joinPairSetOuterItemAction;

		private Action<JoinPair<TOuterSourceItem, TInnerSourceItem>, TInnerSourceItem> _joinPairSetInnerItemAction;

		private TOuterSourceItem _outerItem;

		private TInnerSourceItem _innerItem;

		public JoinPair(
			TOuterSourceItem outerItem, TInnerSourceItem innerItem)
		{
			_outerItem = outerItem;
			_innerItem = innerItem;

		}

		internal void setInnerItem(TInnerSourceItem innerSourceItem)
		{
			_innerItem = innerSourceItem;
			PropertyChanged?.Invoke(this, Utils.InnerItemPropertyChangedEventArgs);
		}

		internal void setOuterItem(TOuterSourceItem outerSourceItem)
		{
			_outerItem = outerSourceItem;
			PropertyChanged?.Invoke(this, Utils.OuterItemPropertyChangedEventArgs);
		}

		public override int GetHashCode()
		{
			return _outerSourceItemEqualityComparer.GetHashCode(OuterItem) +_innerSourceItemEqualityComparer.GetHashCode(InnerItem);
		}

		public override bool Equals(object obj)
		{
			return obj is JoinPair<TOuterSourceItem, TInnerSourceItem> other && Equals(other);
		}

		public bool Equals(JoinPair<TOuterSourceItem, TInnerSourceItem> other)
		{
			return other != null && (_outerSourceItemEqualityComparer.Equals(OuterItem, other.OuterItem) && _innerSourceItemEqualityComparer.Equals(InnerItem, other.InnerItem));
		}

		public override string ToString()
		{
			return $"JoinPair: OuterItem = {(OuterItem != null ? $"{OuterItem.ToString()}" : "null")}    InnerItem = {(InnerItem != null ? $"{InnerItem.ToString()}" : "null")}";
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
