using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using IBCode.ObservableComputations.Common;
using IBCode.ObservableComputations.Common.Base;
using IBCode.ObservableComputations.Common.Interface;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace IBCode.ObservableComputations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class ThenOrdering<TSourceItem, TOrderingValue> : CollectionComputing<TSourceItem>, INotifyPropertyChanged, IOrderingInternal<TSourceItem>, IThenOrderingInternal<TSourceItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IOrdering<TSourceItem>> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TOrderingValue>> OrderingValueSelector => _orderingValueSelectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<ListSortDirection> SortDirectionScalar => _sortDirectionScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IComparer<TOrderingValue>> ComparerScalar => _comparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IOrdering<TSourceItem> Source => _source;

		public ListSortDirection SortDirection => _sortDirection;

		public IComparer<TOrderingValue> Comparer => _comparer;

		public Func<TSourceItem, TOrderingValue> OrderingValueSelectorFunc => _orderingValueSelectorFunc;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public int MaxTogetherThenOrderings => _maxTogetherThenOrderings;

		private PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		internal IOrderingInternal<TSourceItem> _source;
		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
		private Queue<ExpressionWatcher> _deferredExpressionWatcherChangedProcessings;

		private Positions<OrderedItemInfo> _orderedPositions;
		private Positions<ItemInfo> _sourcePositions;

		//private readonly List<Expression<Func<TResultElement>>> _selectorExpressions;
		private List<ItemInfo> _itemInfos;
		private List<OrderedItemInfo> _orderedItemInfos;

		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorExpression;

		private readonly ExpressionWatcher.ExpressionInfo _orderingValueSelectorExpressionInfo;

		private readonly bool _orderingValueSelectorContainsParametrizedLiveLinqCalls;

		private PropertyChangedEventHandler _comparerScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _comparerScalarWeakPropertyChangedEventHandler;

		private PropertyChangedEventHandler _sortDirectionScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sortDirectionScalarWeakPropertyChangedEventHandler;

		List<TOrderingValue> _orderingValues;

		internal RangePosition _thenOrderingRangePosition;
		
		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<IOrdering<TSourceItem>> _sourceScalar;
		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorExpressionOriginal;
		private readonly IReadScalar<ListSortDirection> _sortDirectionScalar;
		private readonly IReadScalar<IComparer<TOrderingValue>> _comparerScalar;
		internal readonly Func<TSourceItem, TOrderingValue> _orderingValueSelectorFunc;
		internal IComparer<TOrderingValue> _comparer;
		internal ListSortDirection _sortDirection;

		private RangePositions<RangePosition> _equalOrderingValueRangePositions;
		private int _thenOrderingsCount;
		private WeakReference<IThenOrderingInternal<TSourceItem>>[] _thenOrderings;
		private int _maxTogetherThenOrderings;

		private void initializeSourceScalar()
		{
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
		}

		private void initializeComparerScalar()
		{
			if (_comparerScalar != null)
			{
				_comparerScalarPropertyChangedEventHandler = handleComparerScalarValueChanged;
				_comparerScalarWeakPropertyChangedEventHandler =
					new WeakPropertyChangedEventHandler(_comparerScalarPropertyChangedEventHandler);
				_comparerScalar.PropertyChanged += _comparerScalarWeakPropertyChangedEventHandler.Handle;
				_comparer = _comparerScalar.Value ?? Comparer<TOrderingValue>.Default;
			}
			else
			{
				_comparer = Comparer<TOrderingValue>.Default;
			}
		}

		private void initializeSortDirectionScalar()
		{
			if (_sortDirectionScalar != null)
			{
				_sortDirectionScalarPropertyChangedEventHandler = handleSortDirectionScalarValueChanged;
				_sortDirectionScalarWeakPropertyChangedEventHandler =
					new WeakPropertyChangedEventHandler(_sortDirectionScalarPropertyChangedEventHandler);
				_sortDirectionScalar.PropertyChanged += _sortDirectionScalarWeakPropertyChangedEventHandler.Handle;
				_sortDirection = _sortDirectionScalar.Value;
			}
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null,
			int maxTogetherThenOrderings = 4) : this(orderingValueSelectorExpression, Utils.getCapacity(source), maxTogetherThenOrderings)
		{
			_source = (IOrderingInternal<TSourceItem>) source;
			_source.AddThenOrdering(this);

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null,
			int maxTogetherThenOrderings = 4) : this(orderingValueSelectorExpression, Utils.getCapacity(source), maxTogetherThenOrderings)
		{
			_source = (IOrderingInternal<TSourceItem>) source;
			_source.AddThenOrdering(this);

			_sortDirection = sortDirection;

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}


		[ObservableComputationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null,
			int maxTogetherThenOrderings = 4) : this(orderingValueSelectorExpression, Utils.getCapacity(source), maxTogetherThenOrderings)
		{
			_source = (IOrderingInternal<TSourceItem>) source;
			_source.AddThenOrdering(this);

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null,
			int maxTogetherThenOrderings = 4) : this(orderingValueSelectorExpression, Utils.getCapacity(source), maxTogetherThenOrderings)
		{
			_source = (IOrderingInternal<TSourceItem>) source;
			_source.AddThenOrdering(this);

			_sortDirection = sortDirection;

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null,
			int maxTogetherThenOrderings = 4) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar), maxTogetherThenOrderings)
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null,
			int maxTogetherThenOrderings = 4) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar), maxTogetherThenOrderings)
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirection = sortDirection;

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null,
			int maxTogetherThenOrderings = 4) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar), maxTogetherThenOrderings)
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null,
			int maxTogetherThenOrderings = 4) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar), maxTogetherThenOrderings)
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirection = sortDirection;

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		private ThenOrdering(
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			int sourceCapacity,
			int maxTogetherThenOrderings) : base(sourceCapacity)
		{
			_maxTogetherThenOrderings = maxTogetherThenOrderings;

			_orderedItemInfos = new List<OrderedItemInfo>(sourceCapacity);
			_orderedPositions = new Positions<OrderedItemInfo>(_orderedItemInfos);
			_itemInfos = new List<ItemInfo>(sourceCapacity);
			_sourcePositions = new Positions<ItemInfo>(_itemInfos);
			_orderingValues = new List<TOrderingValue>(sourceCapacity);

			_orderingValueSelectorExpressionOriginal = orderingValueSelectorExpression;
			CallToConstantConverter callToConstantConverter =
				new CallToConstantConverter(_orderingValueSelectorExpressionOriginal.Parameters);
			_orderingValueSelectorExpression =
				(Expression<Func<TSourceItem, TOrderingValue>>)
				callToConstantConverter.Visit(_orderingValueSelectorExpressionOriginal);
			_orderingValueSelectorContainsParametrizedLiveLinqCalls =
				callToConstantConverter.ContainsParametrizedObservableComputationCalls;

			if (!_orderingValueSelectorContainsParametrizedLiveLinqCalls)
			{
				_orderingValueSelectorExpressionInfo =
					ExpressionWatcher.GetExpressionInfo(_orderingValueSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				_orderingValueSelectorFunc = _orderingValueSelectorExpression.Compile();
			}
		}

		private void handleComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent();

			_consistent = false;

			_comparer = _comparerScalar.Value ?? Comparer<TOrderingValue>.Default;
			initializeFromSource();

			_consistent = true;
			raiseConsistencyRestored();
		}


		private void handleSortDirectionScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent();

			_consistent = false;
			_sortDirection = _sortDirectionScalar.Value;
			initializeFromSource();

			_consistent = true;
			raiseConsistencyRestored();


			//if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			//checkConsistent();
		
			//_consistent = false;
			//ListSortDirection newListSortDirection = _sortDirectionScalar.Value;
			//if (_sortDirection != newListSortDirection)
			//{
			//	_sortDirection = newListSortDirection;

			//	RangePositions<RangePosition> sourceRangePositions = _source.GetRangePositions();

			//	List<RangePosition> rangePositions = sourceRangePositions.List;
			//	int count = rangePositions.Count;
			//	for (int rangePositionIndex = 0; rangePositionIndex < count; rangePositionIndex++)
			//	{
			//		RangePosition rangePosition = rangePositions[rangePositionIndex];

			//		if (_thenOrderingsCount > 0)
			//		{
			//			for (int lowerIndex = _orderedItemInfos[rangePosition.PlainIndex].RangePosition.Index, 
			//				upperIndex = _orderedItemInfos[rangePosition.PlainIndex + rangePosition.Length - 1].RangePosition.Index; 
			//				lowerIndex < upperIndex; lowerIndex++, upperIndex--)
			//			{
			//				RangePosition tempRangePosition =  _equalOrderingValueRangePositions.List[lowerIndex];
			//				_equalOrderingValueRangePositions.List[lowerIndex] =  _equalOrderingValueRangePositions.List[upperIndex];
			//				_equalOrderingValueRangePositions.List[upperIndex] = tempRangePosition;
			//				_equalOrderingValueRangePositions.List[lowerIndex].Index = lowerIndex;
			//				tempRangePosition.Index = upperIndex;
			//			}
			//		}

			//		for (
			//			int lowerIndex = rangePosition.PlainIndex, 
			//			upperIndex = rangePosition.PlainIndex + rangePosition.Length - 1; 
			//			lowerIndex < upperIndex; 
			//			lowerIndex++, upperIndex--)
			//		{
			//			TOrderingValue tempOrderingValue = _orderingValues[lowerIndex];
			//			_orderingValues[lowerIndex] = _orderingValues[upperIndex];
			//			_orderingValues[upperIndex] = tempOrderingValue;

			//			OrderedItemInfo tempItemPosition = _orderedItemInfos[lowerIndex];
			//			 _orderedItemInfos[lowerIndex] = _orderedItemInfos[upperIndex];
			//			 _orderedItemInfos[upperIndex] = tempItemPosition;
			//			_orderedItemInfos[lowerIndex].Index = lowerIndex;
			//			tempItemPosition.Index = upperIndex;

			//			baseMoveItem(lowerIndex, upperIndex);
			//			baseMoveItem(upperIndex - 1, lowerIndex);
			//		}	
			//	}
			//}

			//_consistent = true;
			//raiseConsistencyRestored();
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				int itemInfosCount = _itemInfos.Count;
				for (int index = 0; index < itemInfosCount; index++)
				{
					ItemInfo itemInfo = _itemInfos[index];
					ExpressionWatcher expressionWatcher = itemInfo.ExpressionWatcher;
					expressionWatcher.Dispose();
				}

				int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
				_orderedItemInfos = new List<OrderedItemInfo>(_orderedItemInfos);
				_orderedPositions = new Positions<OrderedItemInfo>(_orderedItemInfos);
				_itemInfos = new List<ItemInfo>(capacity);
				_sourcePositions = new Positions<ItemInfo>(_itemInfos);
				_orderingValues = new List<TOrderingValue>(capacity);

				baseClearItems();

				if (_rootSourceWrapper)
				{
					_sourceAsList.CollectionChanged -= _sourceNotifyCollectionChangedEventHandler;
				}
				else
				{
					_sourceAsList.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
					_sourceWeakNotifyCollectionChangedEventHandler = null;					
				}

				_sourceNotifyCollectionChangedEventHandler = null;

				_source.RemoveThenOrdering();
			}

			if (_sourceScalar != null)
			{
				_source = (IOrderingInternal<TSourceItem>) _sourceScalar.Value;

			}

			_sourceAsList = null;

			if (_source != null)
			{
				_source.AddThenOrdering(this);
				if (_source is ObservableCollectionWithChangeMarker<TSourceItem> sourceAsList)
				{
					_sourceAsList = sourceAsList;
					_rootSourceWrapper = false;
				}
				else
				{
					_sourceAsList = new RootSourceWrapper<TSourceItem>(_source);
					_rootSourceWrapper = true;
				}

				_lastProcessedSourceChangeMarker = _sourceAsList.ChangeMarker;

				fillFromSource();

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;

				if (_rootSourceWrapper)
				{
					_sourceAsList.CollectionChanged += _sourceNotifyCollectionChangedEventHandler;
				}
				else
				{
					_sourceWeakNotifyCollectionChangedEventHandler = 
						new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

					_sourceAsList.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				}
			}
		}

		private void fillFromSource()
		{
			int count = _sourceAsList.Count;
			if (count > 0)
			{
				RangePosition sourceRangePosition = _source.GetRangePosition(0);
				int sourceLowerIndex = sourceRangePosition.PlainIndex;
				int sourceUpperIndex = sourceLowerIndex + sourceRangePosition.Length - 2;

				for (int index = 0; index < count; index++)
				{
					if (index == sourceUpperIndex + 2)
					{
						sourceRangePosition = _source.GetRangePosition(index);
						sourceLowerIndex = sourceRangePosition.PlainIndex;
						sourceUpperIndex = sourceLowerIndex + sourceRangePosition.Length - 2;
					}

					TSourceItem sourceItem = _sourceAsList[index];
					registerSourceItem(sourceItem, index, sourceLowerIndex, sourceUpperIndex);
				}
			}
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent();
			_consistent = false;

			initializeFromSource();

			_consistent = true;
			raiseConsistencyRestored();
		}

		private void registerSourceItem(TSourceItem sourceItem, int sourceIndex, int sourceLowerIndex, int sourceUpperIndex)
		{
			ItemInfo itemInfo = _sourcePositions.Insert(sourceIndex);

			getNewExpressionWatcherAndGetOrderingValueFunc(sourceItem, out ExpressionWatcher expressionWatcher, out Func<TOrderingValue> getOrderingValueFunc);

			itemInfo.GetOrderingValueFunc = getOrderingValueFunc;
			itemInfo.ExpressionWatcher = expressionWatcher;

			TOrderingValue orderingValue = getOrderingValue(itemInfo, sourceItem);

			int orderedIndex = sourceLowerIndex <= sourceUpperIndex 
				? getOrderedIndex(orderingValue, sourceLowerIndex, sourceUpperIndex)
				: sourceIndex;

			if (orderedIndex < sourceLowerIndex) orderedIndex = sourceLowerIndex;

			itemInfo.ExpressionWatcher.ValueChanged = expressionWatcher_OnValueChanged;
			itemInfo.ExpressionWatcher._position = itemInfo;
			OrderedItemInfo orderedItemInfo = _orderedPositions.Insert(orderedIndex);
			itemInfo.OrderedItemInfo = orderedItemInfo;
			orderedItemInfo.ItemInfo = itemInfo;
			_orderingValues.Insert(orderedIndex, orderingValue);

			if (_thenOrderingsCount > 0)
			{
				adjustEqualOrderingValueRangePosition(orderingValue, orderedItemInfo, orderedIndex, orderedIndex - 1, orderedIndex);
			}
			 
			baseInsertItem(orderedIndex, sourceItem);
		}

		private void adjustEqualOrderingValueRangePosition(
			TOrderingValue orderingValue, 
			OrderedItemInfo orderedItemInfo,
			int orderedIndex,
			int previousOrderedIndex,
			int nextOrderedIndex)
		{
			bool isIncludedInRange = false;

			void tryIncludeInRange(OrderedItemInfo nearbyOrderedItemInfo, int nearbyOrderedIndex)
			{
				if (_comparer.Compare(orderingValue,
					    getOrderingValue(
						    nearbyOrderedItemInfo.ItemInfo,
						    this[nearbyOrderedIndex])) == 0)
				{
					RangePosition rangePosition = nearbyOrderedItemInfo.RangePosition;
					orderedItemInfo.RangePosition = rangePosition;
					_equalOrderingValueRangePositions.ModifyLength(rangePosition.Index, 1);
					isIncludedInRange = true;
				}
			}

			RangePosition sourceRangePosition = _source.GetRangePosition(orderedIndex);
			OrderedItemInfo previousOrderedItemInfo = null;
			if (orderedIndex > 0)
			{
				previousOrderedItemInfo = _orderedItemInfos[orderedIndex - 1];
				if (sourceRangePosition.PlainIndex < orderedIndex)
				{
					tryIncludeInRange(previousOrderedItemInfo, previousOrderedIndex);
				}

			}

			if (!isIncludedInRange && orderedIndex < Count)
			{
				if (sourceRangePosition.PlainIndex + sourceRangePosition.Length - 1 > orderedIndex)
				{
					tryIncludeInRange(_orderedItemInfos[orderedIndex + 1], nextOrderedIndex);
				}
			}

			if (!isIncludedInRange)
			{
				orderedItemInfo.RangePosition = previousOrderedItemInfo != null 
					? _equalOrderingValueRangePositions.Insert(previousOrderedItemInfo.RangePosition.Index + 1, 1) 
					: _equalOrderingValueRangePositions.Insert(0, 1);
			}
		}

		private void getNewExpressionWatcherAndGetOrderingValueFunc(TSourceItem sourceItem,
			out ExpressionWatcher expressionWatcher, out Func<TOrderingValue> getOrderingValueFunc)
		{
			getOrderingValueFunc = null;

			if (!_orderingValueSelectorContainsParametrizedLiveLinqCalls)
			{
				expressionWatcher = new ExpressionWatcher(_orderingValueSelectorExpressionInfo, sourceItem);
			}
			else
			{
				Expression<Func<TOrderingValue>> deparametrizedOrderingValueSelectorExpression =
					(Expression<Func<TOrderingValue>>) _orderingValueSelectorExpression.ApplyParameters(new object[] {sourceItem});
				Expression<Func<TOrderingValue>> orderingValueSelectorExpression =
					(Expression<Func<TOrderingValue>>)
						new CallToConstantConverter().Visit(deparametrizedOrderingValueSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				getOrderingValueFunc = orderingValueSelectorExpression.Compile();
				expressionWatcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(orderingValueSelectorExpression));
			}
		}

		private void unregisterSourceItem(int sourceIndex)
		{
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			ExpressionWatcher watcher = itemInfo.ExpressionWatcher;
			watcher.Dispose();

			int orderedIndex = itemInfo.OrderedItemInfo.Index;
			_orderedPositions.Remove(orderedIndex);	
			_sourcePositions.Remove(sourceIndex);
			_orderingValues.RemoveAt(orderedIndex);

			if (_thenOrderingsCount > 0)
			{
				RangePosition rangePosition = itemInfo.OrderedItemInfo.RangePosition;
				if (rangePosition.Length == 1)
					_equalOrderingValueRangePositions.Remove(rangePosition.Index);
				else
					_equalOrderingValueRangePositions.ModifyLength(rangePosition.Index, -1);
			}

			baseRemoveItem(orderedIndex);	
		}

		public TOrderingValue GetOrderingValueBySourceIndex(int sourceIndex)
		{ 
			checkConsistent();
			return getOrderingValueBySourceIndex(sourceIndex);
		}

		internal TOrderingValue getOrderingValueBySourceIndex(int sourceIndex)
		{
			return !_orderingValueSelectorContainsParametrizedLiveLinqCalls ? _orderingValueSelectorFunc(_sourceAsList[sourceIndex]) : _itemInfos[sourceIndex].GetOrderingValueFunc();
		}

		public TOrderingValue GetOrderingValueByOrderedIndex(int orderedIndex)
		{
			checkConsistent();
			return getOrderingValueByOrderedIndex(orderedIndex);
		}

		private TOrderingValue getOrderingValueByOrderedIndex(int orderedIndex)
		{
			return !_orderingValueSelectorContainsParametrizedLiveLinqCalls 
				? _orderingValueSelectorFunc(this[orderedIndex]) 
				: _orderedItemInfos[orderedIndex].ItemInfo.GetOrderingValueFunc();
		}

		private TOrderingValue getOrderingValue(ItemInfo itemInfo, TSourceItem sourceItem)
		{
			return !_orderingValueSelectorContainsParametrizedLiveLinqCalls 
				? _orderingValueSelectorFunc(sourceItem) 
				: itemInfo.GetOrderingValueFunc();
		}

		private bool _replacing;

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!_replacing) checkConsistent();
			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newIndex = e.NewStartingIndex;
					TSourceItem addedItem = _sourceAsList[newIndex];
					RangePosition sourceRangePosition = _source.GetRangePosition(newIndex);
					registerSourceItem(addedItem, newIndex, sourceRangePosition.PlainIndex, sourceRangePosition.PlainIndex + sourceRangePosition.Length - 2);
					break;
				case NotifyCollectionChangedAction.Remove:
					unregisterSourceItem(e.OldStartingIndex);
					break;

				case NotifyCollectionChangedAction.Replace:
					_consistent = false;
					_replacing = true;
					int replacingSourceIndex = e.NewStartingIndex;
					TSourceItem replacingSourceItem = _sourceAsList[replacingSourceIndex];
					ItemInfo replacingItemInfo = _itemInfos[replacingSourceIndex];
					ExpressionWatcher oldExpressionWatcher = _itemInfos[replacingSourceIndex].ExpressionWatcher;

					oldExpressionWatcher.Dispose();

					getNewExpressionWatcherAndGetOrderingValueFunc(replacingSourceItem, out ExpressionWatcher newExpressionWatcher, out Func<TOrderingValue> newGetOrderingValueFunc);

					replacingItemInfo.GetOrderingValueFunc = newGetOrderingValueFunc;
					replacingItemInfo.ExpressionWatcher = newExpressionWatcher;
					replacingItemInfo.ExpressionWatcher.ValueChanged = expressionWatcher_OnValueChanged;
					replacingItemInfo.ExpressionWatcher._position = oldExpressionWatcher._position;

					baseSetItem(replacingItemInfo.OrderedItemInfo.Index, replacingSourceItem);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex = e.OldStartingIndex;
					int newStartingIndex = e.NewStartingIndex;
					if (oldStartingIndex == newStartingIndex) return;
					_sourcePositions.Move(oldStartingIndex, newStartingIndex);
					processSourceItemChange(newStartingIndex, false);
					break;
				case NotifyCollectionChangedAction.Reset:
					_consistent = false;

					initializeFromSource();

					_consistent = true;
					raiseConsistencyRestored();
					break;
			}

			if (_deferredExpressionWatcherChangedProcessings != null)
				while (_deferredExpressionWatcherChangedProcessings.Count > 0)
				{
					ExpressionWatcher expressionWatcher = _deferredExpressionWatcherChangedProcessings.Dequeue();
					if (!expressionWatcher._disposed)
					{
						processSourceItemChange(expressionWatcher._position.Index, true);
					}
				} 
		}


		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			checkConsistent();

			if (_sourceAsList == null || _rootSourceWrapper || _sourceAsList.ChangeMarker ==_lastProcessedSourceChangeMarker)
			{
				_consistent = false;
				processSourceItemChange(expressionWatcher._position.Index, true);
			}
			else
			{
				(_deferredExpressionWatcherChangedProcessings = _deferredExpressionWatcherChangedProcessings ??  new Queue<ExpressionWatcher>()).Enqueue(expressionWatcher);
			}
		}


		private void processSourceItemChange(int sourceIndex, bool checkOrderingValues)
		{
			void notifyThenOrderings(int newOrderedIndex)
			{
				int processedThenOrderingsCount = 0;
				for (int thenOrderingIndex = 0; thenOrderingIndex < _maxTogetherThenOrderings; thenOrderingIndex++)
				{
					WeakReference<IThenOrderingInternal<TSourceItem>> thenOrderingЦeakReference = _thenOrderings[thenOrderingIndex];
					if (thenOrderingЦeakReference != null && thenOrderingЦeakReference.TryGetTarget(out IThenOrderingInternal<TSourceItem> thenOrdering))
					{
						thenOrdering.ProcessSourceItemChange(newOrderedIndex);
						processedThenOrderingsCount++;
						if (processedThenOrderingsCount == _thenOrderingsCount) break;
					}
				}
			}

			ItemInfo itemInfo = _itemInfos[sourceIndex];
			OrderedItemInfo orderedItemInfo = itemInfo.OrderedItemInfo;
			int orderedIndex = orderedItemInfo.Index;
			TOrderingValue orderingValue = !_orderingValueSelectorContainsParametrizedLiveLinqCalls 
				? _orderingValueSelectorFunc(_sourceAsList[sourceIndex]) 
				: itemInfo.GetOrderingValueFunc();

			if (!checkOrderingValues || _comparer.Compare(_orderingValues[orderedIndex], orderingValue) != 0)
			{
				RangePosition newRangePosition = _source.GetRangePosition(sourceIndex);
				int lowerIndex = newRangePosition.PlainIndex;
				int upperIndex = newRangePosition.PlainIndex + newRangePosition.Length - 1;

				if (_source.GetRangePosition(_orderedItemInfos[lowerIndex].ItemInfo.Index) != newRangePosition 
					|| lowerIndex == orderedIndex)
					lowerIndex++;
				if (_source.GetRangePosition(_orderedItemInfos[upperIndex].ItemInfo.Index) != newRangePosition 
				    || upperIndex == orderedIndex)
					upperIndex--;

				int newOrderedIndex = lowerIndex <= upperIndex 
					? getOrderedIndex(orderingValue, lowerIndex, upperIndex)
					: sourceIndex;

				if (newOrderedIndex == Count 
				    || (newOrderedIndex > orderedIndex 
				        && newOrderedIndex != newRangePosition.PlainIndex))
					newOrderedIndex--;

				_orderingValues.RemoveAt(orderedIndex);
				_orderingValues.Insert(newOrderedIndex, orderingValue);

				_orderedPositions.Move(orderedIndex, newOrderedIndex);

				if (_thenOrderingsCount > 0)
				{
					RangePosition rangePosition = orderedItemInfo.RangePosition;
					if (rangePosition.Length == 1)
						_equalOrderingValueRangePositions.Remove(rangePosition.Index);
					else
						_equalOrderingValueRangePositions.ModifyLength(rangePosition.Index, -1);

					adjustEqualOrderingValueRangePosition(
						orderingValue, 
						orderedItemInfo, 
						newOrderedIndex,
						orderedIndex < newOrderedIndex ? newOrderedIndex : newOrderedIndex - 1,
						orderedIndex > newOrderedIndex ? newOrderedIndex : newOrderedIndex + 1);
				}

				if (orderedIndex != newOrderedIndex)
				{
					baseMoveItem(orderedIndex, newOrderedIndex);
				}
				else if (_thenOrderingsCount > 0)
				{
					notifyThenOrderings(newOrderedIndex);
				}
			}
			else if (_thenOrderingsCount > 0)
			{
				notifyThenOrderings(orderedIndex);
			}


			if (!_consistent)
			{
				_consistent = true;
				_replacing = false;
				raiseConsistencyRestored();
			}
		}

		private int getOrderedIndex(TOrderingValue orderingValue, int lowerIndex, int upperIndex)
		{
			int maxIndex = Count - 1;
			//lowerIndex = lowerIndex > maxIndex ? maxIndex : lowerIndex;
			upperIndex = upperIndex > maxIndex ? maxIndex : upperIndex;
			int newIndex = 0;

			do
			{
				TOrderingValue middleItemOrderingValue;
				int middleIndex;

				TSourceItem nextAfterMiddleItem;
				TOrderingValue nextAfterMiddleItemOrderingValue;
				int nextAfterMiddleIndex;

				int length = upperIndex - lowerIndex + 1;
				if (length <= 2)
				{
					if (length == 2)
					{
						TOrderingValue upperItemOrderingValue;
						upperItemOrderingValue = _orderingValues[upperIndex];

						TOrderingValue lowerItemOrderingValue;
						lowerItemOrderingValue = _orderingValues[lowerIndex];

						int comparisonWithUpperItem = _comparer.Compare(orderingValue, upperItemOrderingValue);
						int comparisonWithLowerEntity = _comparer.Compare(orderingValue, lowerItemOrderingValue);

						if (comparisonWithUpperItem < 0 && comparisonWithLowerEntity < 0)
						{
							newIndex = _sortDirection == ListSortDirection.Ascending ? lowerIndex : upperIndex + 1;
						}
						else if (comparisonWithUpperItem > 0 && comparisonWithLowerEntity > 0)
						{
							newIndex = _sortDirection == ListSortDirection.Ascending ? upperIndex + 1 : lowerIndex;
						}
						else
						{
							newIndex = upperIndex;
						}
					}
					else if (length == 1) // currentUpperIndex == currentLowerIndex
					{
						TOrderingValue itemOrderingValue;
						itemOrderingValue = _orderingValues[lowerIndex];
						int comparisonWithUpperItem = _comparer.Compare(orderingValue, itemOrderingValue);
						newIndex = getNearIndex(comparisonWithUpperItem, lowerIndex);
					}
					else if (length == 0) // _temporarilyDisorderedStartsWithIndex == 0
					{
						newIndex = 0;
					}

					if (newIndex == -1) newIndex = 0;
					return newIndex;
				}

				middleIndex = lowerIndex + (length >> 1);

				middleItemOrderingValue = _orderingValues[middleIndex];

				int comparisonWithMiddleItem = _comparer.Compare(orderingValue, middleItemOrderingValue);

				if (comparisonWithMiddleItem == 0)
				{
					newIndex = middleIndex;
					return newIndex;
				}

				int nextAfterMiddleIncrement = comparisonWithMiddleItem * (_sortDirection == ListSortDirection.Descending ? -1 : 1);
				nextAfterMiddleIndex = middleIndex;
				do
				{
					nextAfterMiddleIndex = nextAfterMiddleIndex + nextAfterMiddleIncrement;
					nextAfterMiddleItemOrderingValue = _orderingValues[nextAfterMiddleIndex];
					nextAfterMiddleItem = this[nextAfterMiddleIndex];
				} while (
					_comparer.Compare(nextAfterMiddleItemOrderingValue, middleItemOrderingValue) == 0
					&& nextAfterMiddleIndex != lowerIndex
					&& nextAfterMiddleIndex != upperIndex);


				if (nextAfterMiddleItem != null)
				{
					int comparisonWithNextAfterMiddleItem = _comparer.Compare(orderingValue, nextAfterMiddleItemOrderingValue);
					int comparisonsSum = comparisonWithMiddleItem + comparisonWithNextAfterMiddleItem;

					if (comparisonsSum >= -1 && comparisonsSum <= 1)
					{
						newIndex =
							comparisonWithNextAfterMiddleItem == 0
								? nextAfterMiddleIndex
								: comparisonWithNextAfterMiddleItem > 0
									? _sortDirection == ListSortDirection.Ascending ? 
										nextAfterMiddleIndex - nextAfterMiddleIncrement : 
										nextAfterMiddleIndex
									: _sortDirection == ListSortDirection.Ascending ? 
										nextAfterMiddleIndex : 
										nextAfterMiddleIndex - nextAfterMiddleIncrement;

						return newIndex;
					}
					else
					{
						if (nextAfterMiddleIncrement > 0)
						{
							lowerIndex = nextAfterMiddleIndex;
						}
						else
						{
							upperIndex = nextAfterMiddleIndex;
						}
					}
				}
				else
				{
					newIndex = middleIndex + nextAfterMiddleIncrement;
					if (newIndex == -1) newIndex = 0;
					return newIndex;
				}
			} while (true);
		}

		private int getNearIndex(int comparison, int index)
		{
			return comparison > 0
				? _sortDirection == ListSortDirection.Ascending ? index + 1 : index
				: _sortDirection == ListSortDirection.Ascending ? index : index + 1;
		}

		internal int getOrderedIndexBySourceIndex(int sourceIndex)
		{
			return _itemInfos[sourceIndex].OrderedItemInfo.Index;
		}

		internal int getSourceIndexByOrderedIndex(int orderedIndex)
		{
			return  _orderedItemInfos[orderedIndex].ItemInfo.Index;	
		}

		public int GetOrderedIndexBySourceIndex(int sourceIndex)
		{
			checkConsistent();
			return _itemInfos[sourceIndex].OrderedItemInfo.Index;
		}

		public int GetSourceIndexByOrderedIndex(int orderedIndex)
		{
			checkConsistent();
			return _orderedItemInfos[orderedIndex].ItemInfo.Index;	
		}

		private sealed class ItemInfo : Position
		{
			public Func<TOrderingValue> GetOrderingValueFunc;
			public OrderedItemInfo OrderedItemInfo;
			public ExpressionWatcher ExpressionWatcher;
		}

		private sealed class OrderedItemInfo : Position
		{
			public ItemInfo ItemInfo;
			public RangePosition RangePosition;
		}

		//#region Implementation of IOrdering<TSourceItem>

		//public bool Compare(int resultIndex1, int resultIndex2)
		//{
		//	return 
		//		_comparer.Compare(
		//			getOrderingValueBySourceIndex(getSourceIndexByOrderedIndex(resultIndex1)), 
		//			getOrderingValueBySourceIndex(getSourceIndexByOrderedIndex(resultIndex2))) == 0;
		//}

		//public IOrdering<TSourceItem> Parent => null;
		//#endregion

		void IOrderingInternal<TSourceItem>.AddThenOrdering(IThenOrdering<TSourceItem> thenOrdering)
		{
			Interlocked.Increment(ref _thenOrderingsCount);
			_thenOrderings = _thenOrderings ?? new WeakReference<IThenOrderingInternal<TSourceItem>>[_maxTogetherThenOrderings];

			bool placed = false;
			for (int thenOrderingWeakReferenceIndex = 0; thenOrderingWeakReferenceIndex < _maxTogetherThenOrderings; thenOrderingWeakReferenceIndex++)
			{
				if (_thenOrderings[thenOrderingWeakReferenceIndex] == null)
				{
					_thenOrderings[thenOrderingWeakReferenceIndex] = new WeakReference<IThenOrderingInternal<TSourceItem>>((IThenOrderingInternal<TSourceItem>) thenOrdering);
					placed = true;
					break;
				}

			}
			
			if (!placed)
				throw new ObservableComputationsException("The maximum number of join ThenOrderings has been reached. Please increase the value of MaxTogetherThenOrderings parameter. The value of this parameter can be specified in the constructor of this class. The default value is 4.");


			if (_thenOrderingsCount == 1)
			{
				_equalOrderingValueRangePositions = new RangePositions<RangePosition>(
					new List<RangePosition>(
						_sourceScalar != null 
							? Utils.getCapacity(_sourceScalar) 
							: Utils.getCapacity(_source)));

				int equalOrderingValueItemsCount = 0;
				RangePosition rangePosition = null;

				RangePosition sourceRangePosition;
				int nextRangeIndex = 0;

				int count = Count;
				for (int orderedIndex = 0; orderedIndex < count; orderedIndex++)
				{
					if (orderedIndex > 0)
					{
						int previousOrderedIndex = orderedIndex - 1;

						RangePosition registerNewRangePosition()
						{
							rangePosition.Length = equalOrderingValueItemsCount;
							rangePosition = _equalOrderingValueRangePositions.Add(1);
							_orderedItemInfos[orderedIndex].RangePosition = rangePosition;
							equalOrderingValueItemsCount = 1;
							return rangePosition;
						}

						if (orderedIndex < nextRangeIndex)
						{
							if (_comparer.Compare(
								    getOrderingValue(_orderedItemInfos[orderedIndex].ItemInfo, this[orderedIndex]),
								    getOrderingValue(_orderedItemInfos[previousOrderedIndex].ItemInfo, this[previousOrderedIndex])) == 0)
							{
								equalOrderingValueItemsCount++;
								_orderedItemInfos[orderedIndex].RangePosition = rangePosition;
							}
							else
							{
								rangePosition = registerNewRangePosition();
							}
						}
						else
						{
							rangePosition = registerNewRangePosition();
							sourceRangePosition = _source.GetRangePosition(orderedIndex);
							nextRangeIndex = sourceRangePosition.PlainIndex + sourceRangePosition.Length;
						}
					}
					else
					{
						rangePosition = _equalOrderingValueRangePositions.Add(1);
						equalOrderingValueItemsCount = 1;
						_orderedItemInfos[orderedIndex].RangePosition = rangePosition;
						sourceRangePosition = _source.GetRangePosition(0);
						nextRangeIndex = sourceRangePosition.PlainIndex + sourceRangePosition.Length;
					}
				}

				if (count > 0)
					rangePosition.Length = equalOrderingValueItemsCount;
			}
		}

		void IOrderingInternal<TSourceItem>.RemoveThenOrdering()
		{
			Interlocked.Decrement(ref _thenOrderingsCount);

			Monitor.Enter(_itemInfos);

			int? releasingIndex = null;
			int? maxUsedIndex = null;
			for (int i = 0; i < _maxTogetherThenOrderings; i++)
			{
				WeakReference<IThenOrderingInternal<TSourceItem>> thenOrderingWeakReference = _thenOrderings[i];
				if (thenOrderingWeakReference != null)
				{
					if (!thenOrderingWeakReference.TryGetTarget(out _))
					{
						if (releasingIndex == null) releasingIndex = i;
					}
					else
					{
						maxUsedIndex = i;
					}
				}
			}

			//defragmentation
			if (releasingIndex != null && maxUsedIndex != null && maxUsedIndex.Value > releasingIndex.Value)
			{
				_thenOrderings[releasingIndex.Value] = _thenOrderings[maxUsedIndex.Value];
				_thenOrderings[maxUsedIndex.Value] = null;
			}
			else if (releasingIndex != null)
			{
				_thenOrderings[releasingIndex.Value] = null;
			}

			Monitor.Exit(_itemInfos);
		}

		public OrderingIndicesRange? GetIndicesRangeOf(TOrderingValue orderingValue)
		{
			int lowerIndex = 0;
			int upperIndex = Count - 1;
			int length = upperIndex - lowerIndex + 1;

			do
			{
				if (length == 0)
				{
					return null;
				}

				if (length == 1)
				{
					if (_comparer.Compare(orderingValue, getOrderingValueByOrderedIndex(lowerIndex)) == 0)
						return new OrderingIndicesRange(0, 0);

					return null;
				}

				if (length == 2)
				{
					bool lower = _comparer.Compare(orderingValue, getOrderingValueByOrderedIndex(lowerIndex)) == 0;
					bool upper = _comparer.Compare(orderingValue, getOrderingValueByOrderedIndex(upperIndex)) == 0;

					if (!upper && !lower) return null;
					return new OrderingIndicesRange(lower ? 0 : 1, upper ? 0 : 1);
				}

				int middleIndex = lowerIndex + (length >> 1);

				TOrderingValue middleItemOrderingValue = getOrderingValueByOrderedIndex(middleIndex);
				int comparisonWithMiddleItem = _comparer.Compare(orderingValue, middleItemOrderingValue);

				if (comparisonWithMiddleItem == 0)
				{
					int nextAfterMiddleIndex = middleIndex;
					int from = middleIndex;
					do
					{
						nextAfterMiddleIndex = nextAfterMiddleIndex - 1;
						if (_comparer.Compare(getOrderingValueByOrderedIndex(nextAfterMiddleIndex), middleItemOrderingValue) == 0)
							@from = nextAfterMiddleIndex;
						else
							break;	
					} while (
						nextAfterMiddleIndex != lowerIndex);


					nextAfterMiddleIndex = middleIndex;
					int to = middleIndex;
					do
					{
						nextAfterMiddleIndex = nextAfterMiddleIndex + 1;
						if (_comparer.Compare(getOrderingValueByOrderedIndex(nextAfterMiddleIndex), middleItemOrderingValue) == 0)
							to = nextAfterMiddleIndex;
						else
							break;	
					} while (
						nextAfterMiddleIndex != upperIndex);

					return new OrderingIndicesRange(@from, to);
				}
				else
				{
					if (_sortDirection == ListSortDirection.Ascending)
					{
						if (comparisonWithMiddleItem > 0) lowerIndex = middleIndex;
						else upperIndex = middleIndex;
					}
					else
					{
						if (comparisonWithMiddleItem > 0) upperIndex = middleIndex;
						else lowerIndex = middleIndex;							
					}
				}
			} while (true);
		}

		void IThenOrderingInternal<TSourceItem>.ProcessSourceItemChange(int sourceIndex)
		{
			processSourceItemChange(sourceIndex, false);
		}

		~ThenOrdering()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_sourceAsList.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_comparerScalarWeakPropertyChangedEventHandler != null)
			{
				_comparerScalar.PropertyChanged -= _comparerScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_sortDirectionScalarWeakPropertyChangedEventHandler != null)
			{
				_sortDirectionScalar.PropertyChanged -= _sortDirectionScalarWeakPropertyChangedEventHandler.Handle;			
			}

			_source?.RemoveThenOrdering();
		}

		public bool Compare(int resultIndex1, int resultIndex2)
		{
			var sourceIndex1 = getSourceIndexByOrderedIndex(resultIndex1);
			var sourceIndex2 = getSourceIndexByOrderedIndex(resultIndex2);
			return
				_comparer.Compare(
					getOrderingValueBySourceIndex(sourceIndex1),
					getOrderingValueBySourceIndex(sourceIndex2)) == 0
				&& _source.Compare(sourceIndex1, sourceIndex2);
		}

		public void ValidateConsistency()
		{
			IComparer<TOrderingValue> comparer = _comparerScalar.getValue(_comparer) ?? Comparer<TOrderingValue>.Default;
			ListSortDirection listSortDirection = _sortDirectionScalar.getValue(_sortDirection);
			Func<TSourceItem, TOrderingValue> orderingValueSelector = _orderingValueSelectorExpressionOriginal.Compile();
			IOrderingInternal<TSourceItem> source = (IOrderingInternal<TSourceItem>) _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>().Ordering(_orderingValueSelectorExpressionOriginal));

			source.ValidateConsistency();

			IOrdering<TSourceItem > orderingSource = (IOrdering<TSourceItem>)source;

			if (_itemInfos.Count != Count) throw new ObservableComputationsException("Consistency violation: ThenOrdering.1");

			if (_orderingValues.Count != Count) 
				throw new ObservableComputationsException("Consistency violation: ThenOrdering.2");

			if (_sourcePositions.List.Count != source.Count)
				throw new ObservableComputationsException("Consistency violation: Ordering.3");

			if (_thenOrderingsCount > 0)
			{
				_equalOrderingValueRangePositions.ValidateConsistency();
			}

			int orderingRangeIndex = 0;
			List<TSourceItem> copy = this.ToList();	
			List<TSourceItem> buffer = new List<TSourceItem>();	
			for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
			{
				TSourceItem sourceItem = source[sourceIndex];
				if (!copy.Remove(sourceItem)) throw new ObservableComputationsException("Consistency violation: ThenOrdering.4");

				if (sourceIndex > 0)
				{
					bool validateBuffer = true;
					int bufferLastSourceIndex = sourceIndex - 1;
					if (orderingSource.Compare(sourceIndex, sourceIndex - 1))
					{
						buffer.Add(source[sourceIndex]);
						if (sourceIndex != source.Count - 1)
						{
							validateBuffer = false;
						}
						else
						{
							bufferLastSourceIndex = sourceIndex;
						}
					}
					
					if (validateBuffer)
					{
						//OrderingInfo orderingInfo = _orderingInfos[orderingRangeIndex];
						//orderingInfo.Ordering.ValidateConsistency();

						List<TSourceItem> orderedBuffer = new List<TSourceItem>();

						for (int i = bufferLastSourceIndex - buffer.Count + 1 ; i <= bufferLastSourceIndex; i++)
						{
							orderedBuffer.Add(this[i]);
						}

						List<TSourceItem> bufferCopy = buffer.ToList();
						int orderedBufferCount = orderedBuffer.Count;
						for (int index = 0; index < orderedBufferCount; index++)
						{
							TSourceItem item = orderedBuffer[index];
							if (!bufferCopy.Remove(item))
								throw new ObservableComputationsException("Consistency violation: ThenOrdering.5");
						}

						for (int bufferIndex = 0; bufferIndex < orderedBuffer.Count; bufferIndex++)
						{
							if (bufferIndex > 0)
							{
								int compareResult = comparer.Compare(orderingValueSelector(orderedBuffer[bufferIndex - 1]), orderingValueSelector(orderedBuffer[bufferIndex]));
								if ((compareResult < 0 && listSortDirection == ListSortDirection.Descending)
									|| (compareResult > 0 && listSortDirection == ListSortDirection.Ascending)) 
									throw new ObservableComputationsException("Consistency violation: ThenOrdering.6");								
							}
						}


						buffer.Clear();
						orderingRangeIndex++;
						buffer.Add(source[sourceIndex]);
					}
				}
				else
				{
					buffer.Add(source[sourceIndex]);
				}

				ItemInfo itemInfo = _itemInfos[sourceIndex];
				if (itemInfo.ExpressionWatcher._position != _sourcePositions.List[sourceIndex])
					throw new ObservableComputationsException("Consistency violation: Ordering.7");
				if (!EqualityComparer<TOrderingValue>.Default.Equals(_orderingValues[itemInfo.OrderedItemInfo.Index], orderingValueSelector(sourceItem)))
					throw new ObservableComputationsException("Consistency violation: Ordering.8");

				if (!_orderedItemInfos.Contains(_itemInfos[sourceIndex].OrderedItemInfo))
					throw new ObservableComputationsException("Consistency violation: Ordering.9");

				if (!_sourcePositions.List.Contains(_itemInfos[sourceIndex].ExpressionWatcher._position))
					throw new ObservableComputationsException("Consistency violation: Ordering.10");

				if (_itemInfos[sourceIndex].ExpressionWatcher._position.Index != sourceIndex)
					throw new ObservableComputationsException("Consistency violation: Ordering.11");
			}

			List<IComparer<TOrderingValue>> ancestorComparers = new List<IComparer<TOrderingValue>>();
			List<ListSortDirection> ancestorListSortDirections = new List<ListSortDirection>();
			List<Func<TSourceItem, TOrderingValue>> ancestorOrderingValueSelectorFuncs = new List<Func<TSourceItem, TOrderingValue>>();
			IOrdering<TSourceItem> ancestor;
			ancestor = this;

			do
			{
				// ReSharper disable once ConditionIsAlwaysTrueOrFalse
				if (ancestor is Ordering<TSourceItem, int?> || ancestor is Ordering<TSourceItem, int>)
					// ReSharper disable once HeuristicUnreachableCode
				{
					// ReSharper disable once HeuristicUnreachableCode
					Ordering<TSourceItem, TOrderingValue> orderingAncestor = ((Ordering<TSourceItem, TOrderingValue>)ancestor);
					ancestorComparers.Add(orderingAncestor._comparer);
					ancestorListSortDirections.Add(orderingAncestor._sortDirection);
					ancestorOrderingValueSelectorFuncs.Add(orderingAncestor._orderingValueSelectorFunc);
					ancestor = null;
				}
				else
				{
					ThenOrdering<TSourceItem, TOrderingValue> thenOrderingAncestor = ((ThenOrdering<TSourceItem, TOrderingValue>)ancestor);
					ancestorComparers.Add(thenOrderingAncestor._comparer);
					ancestorListSortDirections.Add(thenOrderingAncestor._sortDirection);
					ancestorOrderingValueSelectorFuncs.Add(thenOrderingAncestor._orderingValueSelectorFunc);		
					ancestor = thenOrderingAncestor._source;
				}				
			} while (ancestor != null);

			IOrderedEnumerable<TSourceItem> result;
			result = 
				ancestorListSortDirections[ancestorComparers.Count - 1] == ListSortDirection.Ascending 
					? source.OrderBy(ancestorOrderingValueSelectorFuncs[ancestorComparers.Count - 1], ancestorComparers[ancestorComparers.Count - 1]) 
					: source.OrderByDescending(ancestorOrderingValueSelectorFuncs[ancestorComparers.Count - 1], ancestorComparers[ancestorComparers.Count - 1]);


			for (int i = ancestorComparers.Count - 2; i >= 0; i--)
			{
				result = ancestorListSortDirections[i] == ListSortDirection.Ascending 
					? result.ThenBy(ancestorOrderingValueSelectorFuncs[i], ancestorComparers[i]) 
					: result.ThenByDescending(ancestorOrderingValueSelectorFuncs[i], ancestorComparers[i]);
			}

			TSourceItem[] resultArray = result.ToArray();

			int resultArrayLength = resultArray.Length;
			RangePosition rangePosition = null;
			int equalOrderingValueItemsCount = 0;
			int rangePositionIndex = 0;
			for (int orderedIndex = 0; orderedIndex < resultArrayLength; orderedIndex++)
			{
				TSourceItem orderedItem = this[orderedIndex];
				//TSourceItem resultItem = resultArray[orderedIndex];

				//if (!orderedItem.Equals(resultItem))
				//	throw new ObservableComputationsException("Consistency violation: ThenOrdering.12");

				if (orderedIndex > 0)
				{
					int compareResult = comparer.Compare(orderingValueSelector(this[orderedIndex - 1]), orderingValueSelector(this[orderedIndex]));

					if (_thenOrderingsCount > 0)
					{
						if (compareResult == 0 
						    && source.GetRangePosition(_orderedItemInfos[orderedIndex].ItemInfo.Index) == source.GetRangePosition(_orderedItemInfos[orderedIndex - 1].ItemInfo.Index))
						{
							equalOrderingValueItemsCount++;
							if (rangePosition != _orderedItemInfos[orderedIndex].RangePosition)
								throw new ObservableComputationsException("Consistency violation: ThenOrdering.14");
						}
						else				
						{
							if (rangePosition.Length != equalOrderingValueItemsCount)
								throw new ObservableComputationsException("Consistency violation: ThenOrdering.15");

							if (rangePosition.Index != rangePositionIndex)
								throw new ObservableComputationsException("Consistency violation: ThenOrdering.16");

							rangePositionIndex++;
							equalOrderingValueItemsCount = 1;

							rangePosition = _orderedItemInfos[orderedIndex].RangePosition;
						}
					}
				}
				else
				{
					if (_thenOrderingsCount > 0)
					{
						rangePosition = _orderedItemInfos[orderedIndex].RangePosition;
						equalOrderingValueItemsCount = 1;
					}
				}

				ItemInfo itemInfo = _orderedItemInfos[orderedIndex].ItemInfo;
				if (itemInfo.OrderedItemInfo.Index != orderedIndex) throw new ObservableComputationsException("Consistency violation: ThenOrdering.17");

				if (!EqualityComparer<TOrderingValue>.Default.Equals(_orderingValues[orderedIndex], orderingValueSelector(orderedItem)))
					throw new ObservableComputationsException("Consistency violation: ThenOrdering.18");
			}

			//for (int i = 0; i < _itemInfos.Count; i++)
			//{
			//	if (!_orderingInfoPositions.List.Contains(_itemInfos[i].OrderingInfoRangePosition))
			//		throw new ObservableComputationsException("Consistency violation: ThenOrdering.12");
			//}
		}

		RangePosition IOrderingInternal<TSourceItem>.GetRangePosition(int orderedIndex)
		{
			return _orderedItemInfos[orderedIndex].RangePosition;
		}

		RangePositions<RangePosition> IOrderingInternal<TSourceItem>.GetRangePositions()
		{
			return _equalOrderingValueRangePositions;
		}
	}
}
