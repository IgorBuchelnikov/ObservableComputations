using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Base;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class Ordering<TSourceItem, TOrderingValue> : CollectionCalculating<TSourceItem>, INotifyPropertyChanged, IOrderingInternal<TSourceItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TOrderingValue>> OrderingValueSelector => _orderingValueSelectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<ListSortDirection> SortDirectionScalar => _sortDirectionScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IComparer<TOrderingValue>> ComparerScalar => _comparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ListSortDirection SortDirection => _sortDirection;

		public IComparer<TOrderingValue> Comparer => _comparer;

		public Func<TSourceItem, TOrderingValue> OrderingValueSelectorFunc => _orderingValueSelectorFunc;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		internal INotifyCollectionChanged _source;
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
		
		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorExpressionOriginal;
		private readonly IReadScalar<ListSortDirection> _sortDirectionScalar;
		private readonly IReadScalar<IComparer<TOrderingValue>> _comparerScalar;
		internal readonly Func<TSourceItem, TOrderingValue> _orderingValueSelectorFunc;
		internal IComparer<TOrderingValue> _comparer;
		internal ListSortDirection _sortDirection;

		private RangePositions<RangePosition> _equalOrderingValueRangePositions;
		private int _thenOrderingsCount;
		private List<WeakReference<IThenOrderingInternal<TSourceItem>>> _thenOrderings;

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

		[ObservableCalculationsCall]
		public Ordering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Ordering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_sortDirection = sortDirection;

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}


		[ObservableCalculationsCall]
		public Ordering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Ordering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_sortDirection = sortDirection;

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Ordering(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Ordering(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirection = sortDirection;

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Ordering(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Ordering(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirection = sortDirection;

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		private Ordering(Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression, int sourceCapacity) : base(sourceCapacity)
		{
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
				callToConstantConverter.ContainsParametrizedObservableCalculationCalls;

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

			ListSortDirection newListSortDirection = _sortDirectionScalar.Value;
			if (_sortDirection != newListSortDirection)
			{
				_sortDirection = newListSortDirection;
				int count = Count;
				for (int lowerIndex = 0, upperIndex = count - 1; lowerIndex < upperIndex; lowerIndex++, upperIndex--)
				{
					TOrderingValue tempOrderingValue = _orderingValues[lowerIndex];
					_orderingValues[lowerIndex] = _orderingValues[upperIndex];
					_orderingValues[upperIndex] = tempOrderingValue;

					OrderedItemInfo tempItemPosition = _orderedItemInfos[lowerIndex];
					 _orderedItemInfos[lowerIndex] = _orderedItemInfos[upperIndex];
					 _orderedItemInfos[upperIndex] = tempItemPosition;
					_orderedItemInfos[lowerIndex].Index = lowerIndex;
					tempItemPosition.Index = upperIndex;

					baseMoveItem(lowerIndex, upperIndex);
					baseMoveItem(upperIndex - 1, lowerIndex);
				}

				count = _equalOrderingValueRangePositions.List.Count;
				for (int lowerIndex = 0, upperIndex = count - 1; lowerIndex < upperIndex; lowerIndex++, upperIndex--)
				{
					RangePosition tempRangePosition =  _equalOrderingValueRangePositions.List[lowerIndex];
					_equalOrderingValueRangePositions.List[lowerIndex] =  _equalOrderingValueRangePositions.List[upperIndex];
					_equalOrderingValueRangePositions.List[upperIndex] = tempRangePosition;
					_equalOrderingValueRangePositions.List[lowerIndex].Index = lowerIndex;
					tempRangePosition.Index = upperIndex;
				}
			}

			_consistent = true;
			raiseConsistencyRestored();
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
			}

			if (_sourceScalar != null) _source = _sourceScalar.Value;
			_sourceAsList = null;

			if (_source != null)
			{
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
			for (int index = 0; index < count; index++)
			{
				TSourceItem sourceItem = _sourceAsList[index];
				registerSourceItem(sourceItem, index);
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

		private void registerSourceItem(TSourceItem sourceItem, int sourceIndex)
		{
			ItemInfo itemInfo = _sourcePositions.Insert(sourceIndex);

			getNewExpressionWatcherAndGetOrderingValueFunc(sourceItem, out ExpressionWatcher expressionWatcher, out Func<TOrderingValue> getOrderingValueFunc);

			itemInfo.GetOrderingValueFunc = getOrderingValueFunc;
			itemInfo.ExpressionWatcher = expressionWatcher;

			TOrderingValue orderingValue = getOrderingValue(itemInfo, sourceItem);
			int orderedIndex = getOrderedIndex(orderingValue);

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

			OrderedItemInfo previousOrderedItemInfo = null;
			if (orderedIndex > 0)
			{
				previousOrderedItemInfo = _orderedItemInfos[orderedIndex - 1];
				tryIncludeInRange(previousOrderedItemInfo, previousOrderedIndex);
			}

			int count = Count;
			if (!isIncludedInRange && orderedIndex < count && nextOrderedIndex < count)
			{
				tryIncludeInRange(_orderedItemInfos[orderedIndex + 1], nextOrderedIndex);
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



		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();
			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newIndex = e.NewStartingIndex;
					TSourceItem addedItem = _sourceAsList[newIndex];
					registerSourceItem(addedItem, newIndex);
					break;
				case NotifyCollectionChangedAction.Remove:
					unregisterSourceItem(e.OldStartingIndex);
					break;

				case NotifyCollectionChangedAction.Replace:
					_consistent = false;

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
					processSourceItemChange(replacingSourceIndex);

					_consistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex = e.OldStartingIndex;
					int newStartingIndex = e.NewStartingIndex;
					if (oldStartingIndex == newStartingIndex) return;
					_sourcePositions.Move(oldStartingIndex, newStartingIndex);
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
						processSourceItemChange(expressionWatcher._position.Index);
				} 
		}


		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			checkConsistent();

			if (_sourceAsList == null || _rootSourceWrapper || _sourceAsList.ChangeMarker ==_lastProcessedSourceChangeMarker)
			{
				_consistent = false;
				processSourceItemChange(expressionWatcher._position.Index);
				_consistent = true;
				raiseConsistencyRestored();
			}
			else
			{
				(_deferredExpressionWatcherChangedProcessings = _deferredExpressionWatcherChangedProcessings ??  new Queue<ExpressionWatcher>()).Enqueue(expressionWatcher);
			}
		}


		private void processSourceItemChange(int sourceIndex)
		{
			void notifyThenOrderings(int newOrderedIndex)
			{
				Monitor.Enter(_itemInfos);
				IThenOrderingInternal<TSourceItem>[] thenOrderings = new IThenOrderingInternal<TSourceItem>[_thenOrderingsCount];

				for (int thenOrderingIndex = 0; thenOrderingIndex < _thenOrderingsCount; thenOrderingIndex++)
				{
					if (_thenOrderings[thenOrderingIndex].TryGetTarget(out IThenOrderingInternal<TSourceItem> thenOrdering))
					{
						thenOrderings[thenOrderingIndex] = thenOrdering;
					}
				}

				Monitor.Exit(_itemInfos);

				for (int thenOrderingIndex = 0; thenOrderingIndex < _thenOrderingsCount; thenOrderingIndex++)
				{
					IThenOrderingInternal<TSourceItem> thenOrdering = thenOrderings[thenOrderingIndex];
					thenOrdering?.ProcessSourceItemChange(newOrderedIndex);
				}
			}

			ItemInfo itemInfo = _itemInfos[sourceIndex];
			OrderedItemInfo orderedItemInfo = itemInfo.OrderedItemInfo;
			int orderedIndex = orderedItemInfo.Index;
			TOrderingValue orderingValue = !_orderingValueSelectorContainsParametrizedLiveLinqCalls 
				? _orderingValueSelectorFunc(_sourceAsList[sourceIndex]) 
				: itemInfo.GetOrderingValueFunc();

			if (_comparer.Compare(_orderingValues[orderedIndex], orderingValue) != 0)
			{
				int newOrderedIndex = getOrderedIndex(orderingValue);
				if (newOrderedIndex == Count)
					newOrderedIndex = newOrderedIndex - 1;
				else if (newOrderedIndex > orderedIndex) newOrderedIndex--;

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
		}

		private int getOrderedIndex(TOrderingValue orderingValue)
		{
			int lowerIndex = 0;
			int upperIndex = Count - 1;
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
					else if (length == 0) 
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

		public bool Compare(int resultIndex1, int resultIndex2)
		{
			return
				_comparer.Compare(
					getOrderingValueBySourceIndex(getSourceIndexByOrderedIndex(resultIndex1)),
					getOrderingValueBySourceIndex(getSourceIndexByOrderedIndex(resultIndex2))) == 0;
		}

		//public IOrdering<TSourceItem> Parent => null;
		//#endregion

		void IOrderingInternal<TSourceItem>.AddThenOrdering(IThenOrdering<TSourceItem> thenOrdering)
		{
			Monitor.Enter(_itemInfos);
			_thenOrderingsCount++;
			_thenOrderings = _thenOrderings ?? new List<WeakReference<IThenOrderingInternal<TSourceItem>>>();
			_thenOrderings.Add(new WeakReference<IThenOrderingInternal<TSourceItem>>((IThenOrderingInternal<TSourceItem>) thenOrdering));

			if (_thenOrderingsCount == 1)
			{
				_equalOrderingValueRangePositions = new RangePositions<RangePosition>(
					new List<RangePosition>(
						_sourceScalar != null 
							? Utils.getCapacity(_sourceScalar) 
							: Utils.getCapacity(_source)));

				int equalOrderingValueItemsCount = 0;
				RangePosition rangePosition = null;

				int count = Count;
				for (int orderedIndex = 0; orderedIndex < count; orderedIndex++)
				{
					if (orderedIndex > 0)
					{
						int previousOrderedIndex = orderedIndex - 1;
						if (_comparer.Compare(
								getOrderingValue(_orderedItemInfos[orderedIndex].ItemInfo, this[orderedIndex]),
								getOrderingValue(_orderedItemInfos[previousOrderedIndex].ItemInfo, this[previousOrderedIndex])) == 0)
						{
							equalOrderingValueItemsCount++;
							_orderedItemInfos[orderedIndex].RangePosition = rangePosition;
						}
						else
						{
							rangePosition.Length = equalOrderingValueItemsCount;
							rangePosition = _equalOrderingValueRangePositions.Add(1);
							_orderedItemInfos[orderedIndex].RangePosition = rangePosition;
							equalOrderingValueItemsCount = 1;
						}
					}
					else
					{
						rangePosition = _equalOrderingValueRangePositions.Add(1);
						equalOrderingValueItemsCount = 1;
						_orderedItemInfos[orderedIndex].RangePosition = rangePosition;
					}
				}

				if (count > 0)
					rangePosition.Length = equalOrderingValueItemsCount;
			}

			Monitor.Exit(_itemInfos);
		}

		void IOrderingInternal<TSourceItem>.RemoveThenOrdering(IThenOrdering<TSourceItem> thenOrdering)
		{
			Monitor.Enter(_itemInfos);
			for (int i = 0; i < _thenOrderingsCount; i++)
			{
				if (!_thenOrderings[i].TryGetTarget(out IThenOrderingInternal<TSourceItem> targetThenOrdering) || ReferenceEquals(targetThenOrdering, thenOrdering))
				{
					_thenOrderings.RemoveAt(i);
					break;
				}

			}
			_thenOrderingsCount--;

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

		RangePosition IOrderingInternal<TSourceItem>.GetRangePosition(int orderedIndex)
		{
			return _orderedItemInfos[orderedIndex].RangePosition;
		}

		RangePositions<RangePosition> IOrderingInternal<TSourceItem>.GetRangePositions()
		{
			return _equalOrderingValueRangePositions;
		}

		~Ordering()
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
		}

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			if (_itemInfos.Count != Count) throw new ObservableCalculationsException("Consistency violation: Ordering.7");

			List<TSourceItem> copy = this.ToList();			
			// ReSharper disable once PossibleNullReferenceException
			for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
			{
				TSourceItem sourceItem = source[sourceIndex];
				if (!copy.Remove(sourceItem)) throw new ObservableCalculationsException("Consistency violation: Ordering.1");
			}

			_orderedPositions.ValidateConsistency();

			Func<TSourceItem, TOrderingValue> orderingValueSelector = _orderingValueSelectorExpression.Compile();
			IComparer<TOrderingValue> comparer = _comparerScalar.getValue(_comparer) ?? Comparer<TOrderingValue>.Default;
			ListSortDirection listSortDirection = _sortDirectionScalar.getValue(_sortDirection);

			if (_orderingValues.Count != Count) 
				throw new ObservableCalculationsException("Consistency violation: Ordering.14");

			if (_thenOrderingsCount > 0)
			{
				_equalOrderingValueRangePositions.ValidateConsistency();
			}

			RangePosition rangePosition = null;
			int equalOrderingValueItemsCount = 0;
			int rangePositionIndex = 0;
			for (int orderedIndex = 0; orderedIndex < Count; orderedIndex++)
			{
				TSourceItem orderedItem = this[orderedIndex];

				if (orderedIndex > 0)
				{
					int compareResult = comparer.Compare(orderingValueSelector(this[orderedIndex - 1]), orderingValueSelector(this[orderedIndex]));
					if ((compareResult < 0 && listSortDirection == ListSortDirection.Descending)
						|| (compareResult > 0 && listSortDirection == ListSortDirection.Ascending)) 
						throw new ObservableCalculationsException("Consistency violation: Ordering.3");

					if (_thenOrderingsCount > 0)
					{
						if (compareResult == 0)
						{
							equalOrderingValueItemsCount++;
							if (rangePosition !=  _orderedItemInfos[orderedIndex].RangePosition)
								throw new ObservableCalculationsException("Consistency violation: Ordering.17");
						}
						else				
						{
							if (rangePosition.Length != equalOrderingValueItemsCount)
								throw new ObservableCalculationsException("Consistency violation: Ordering.20");

							if (rangePosition.Index != rangePositionIndex)
								throw new ObservableCalculationsException("Consistency violation: Ordering.21");

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
				if (itemInfo.OrderedItemInfo.Index != orderedIndex) throw new ObservableCalculationsException("Consistency violation: Ordering.13");

				if (!EqualityComparer<TOrderingValue>.Default.Equals(_orderingValues[orderedIndex], orderingValueSelector(orderedItem)))
					throw new ObservableCalculationsException("Consistency violation: Ordering.10");
			}

			if (_thenOrderingsCount > 0 && Count > 0)
			{
				if (rangePosition.Length != equalOrderingValueItemsCount)
					throw new ObservableCalculationsException("Consistency violation: Ordering.22");

				if (rangePosition.Index != rangePositionIndex)
					throw new ObservableCalculationsException("Consistency violation: Ordering.23");
			}

			_sourcePositions.ValidateConsistency();

			if (_sourcePositions.List.Count != source.Count)
				throw new ObservableCalculationsException("Consistency violation: Ordering.15");

			for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
			{
				TSourceItem sourceItem = source[sourceIndex];
				ItemInfo itemInfo = _itemInfos[sourceIndex];
				if (itemInfo.ExpressionWatcher._position != _sourcePositions.List[sourceIndex])
					throw new ObservableCalculationsException("Consistency violation: Ordering.8");
				if (!EqualityComparer<TOrderingValue>.Default.Equals(_orderingValues[itemInfo.OrderedItemInfo.Index], orderingValueSelector(sourceItem)))
					throw new ObservableCalculationsException("Consistency violation: Ordering.9");

				if (!_orderedItemInfos.Contains(_itemInfos[sourceIndex].OrderedItemInfo))
					throw new ObservableCalculationsException("Consistency violation: Ordering.11");

				if (!_sourcePositions.List.Contains(_itemInfos[sourceIndex].ExpressionWatcher._position))
					throw new ObservableCalculationsException("Consistency violation: Ordering.12");

				if (_itemInfos[sourceIndex].ExpressionWatcher._position.Index != sourceIndex)
					throw new ObservableCalculationsException("Consistency violation: Ordering.16");
			}
		}
	}

	public struct OrderingIndicesRange
	{
		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		// ReSharper disable once MemberCanBePrivate.Global
		public int From { get; }

		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		// ReSharper disable once MemberCanBePrivate.Global
		public int To { get; }

		public OrderingIndicesRange(int @from, int to)
		{
			From = @from;
			To = to;
		}
	}
}
