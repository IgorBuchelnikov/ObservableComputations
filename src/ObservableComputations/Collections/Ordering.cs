using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace ObservableComputations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class Ordering<TSourceItem, TOrderingValue> : CollectionComputing<TSourceItem>, INotifyPropertyChanged, IOrderingInternal<TSourceItem>, IHasSourceCollections, ISourceItemChangeProcessor, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TOrderingValue>> OrderingValueSelectorExpression => _orderingValueSelectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<ListSortDirection> SortDirectionScalar => _sortDirectionScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IComparer<TOrderingValue>> ComparerScalar => _comparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ListSortDirection SortDirection => _sortDirection;

		public IComparer<TOrderingValue> Comparer => _comparer;

		public Func<TSourceItem, TOrderingValue> OrderingValueSelectorFunc => _orderingValueSelectorFunc;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		internal INotifyCollectionChanged _source;
		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;

		private Positions<OrderedItemInfo<TOrderingValue>> _orderedPositions;
		private Positions<OrderingItemInfo<TOrderingValue>> _sourcePositions;

		//private readonly List<Expression<Func<TResultElement>>> _selectorExpressions;
		private List<OrderingItemInfo<TOrderingValue>> _itemInfos;
		private List<OrderedItemInfo<TOrderingValue>> _orderedItemInfos;

		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorExpression;

		private readonly ExpressionWatcher.ExpressionInfo _orderingValueSelectorExpressionInfo;

		private readonly bool _orderingValueSelectorContainsParametrizedLiveLinqCalls;

		private PropertyChangedEventHandler _comparerScalarPropertyChangedEventHandler;

		private PropertyChangedEventHandler _sortDirectionScalarPropertyChangedEventHandler;

		List<TOrderingValue> _orderingValues;
		
		private bool _sourceInitialized;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorExpressionOriginal;
		private readonly IReadScalar<ListSortDirection> _sortDirectionScalar;
		private readonly IReadScalar<IComparer<TOrderingValue>> _comparerScalar;
		internal readonly Func<TSourceItem, TOrderingValue> _orderingValueSelectorFunc;
		internal IComparer<TOrderingValue> _comparer;
		internal ListSortDirection _sortDirection;

		private RangePositions<RangePosition> _equalOrderingValueRangePositions;
		private int _thenOrderingsCount;
		private List<IThenOrderingInternal<TSourceItem>> _thenOrderings;

		private int _orderingValueSelectorExpressionCallCount;
		private readonly List<IComputingInternal> _nestedComputings;
		private readonly ISourceItemChangeProcessor _thisAsSourceItemChangeProcessor;

		private void initializeComparer()
		{
			if (_comparerScalar != null)
			{
				_comparerScalarPropertyChangedEventHandler = getScalarValueChangedHandler(() =>
					_comparer = _comparerScalar.Value ?? Comparer<TOrderingValue>.Default);
				_comparerScalar.PropertyChanged += _comparerScalarPropertyChangedEventHandler;
				_comparer = _comparerScalar.Value;
			}
			
			if (_comparer == null)
				_comparer = Comparer<TOrderingValue>.Default;
		}

		private void initializeSortDirectionScalar()
		{
			if (_sortDirectionScalar != null)
			{
				_sortDirectionScalarPropertyChangedEventHandler = getScalarValueChangedHandler(
					() => _sortDirection = _sortDirectionScalar.Value);
				_sortDirectionScalar.PropertyChanged += _sortDirectionScalarPropertyChangedEventHandler;
				_sortDirection = _sortDirectionScalar.Value;
			}
		}

		[ObservableComputationsCall]
		public Ordering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_sortDirectionScalar = sortDirectionScalar;
			_comparerScalar = comparerScalar;
		}

		[ObservableComputationsCall]
		public Ordering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_sortDirection = sortDirection;
			_comparerScalar = comparerScalar;
		}


		[ObservableComputationsCall]
		public Ordering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_sortDirectionScalar = sortDirectionScalar;
			_comparer = comparer ?? Comparer<TOrderingValue>.Default;
		}

		[ObservableComputationsCall]
		public Ordering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_sortDirection = sortDirection;
			_comparer = comparer ?? Comparer<TOrderingValue>.Default;
		}

		[ObservableComputationsCall]
		public Ordering(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sortDirectionScalar = sortDirectionScalar;
			_comparerScalar = comparerScalar;
		}

		[ObservableComputationsCall]
		public Ordering(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sortDirection = sortDirection;
			_comparerScalar = comparerScalar;
		}

		[ObservableComputationsCall]
		public Ordering(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sortDirectionScalar = sortDirectionScalar;
			_comparer = comparer ?? Comparer<TOrderingValue>.Default;
		}

		[ObservableComputationsCall]
		public Ordering(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sortDirection = sortDirection;
			_comparer = comparer ?? Comparer<TOrderingValue>.Default;
		}

		private Ordering(
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression, 
			int sourceCapacity) : base(sourceCapacity)
		{
			Utils.construct(
				sourceCapacity, 
				out _orderedItemInfos, 
				out _orderedPositions, 
				out _orderingValues);

			Utils.construct(
				orderingValueSelectorExpression, 
				sourceCapacity, 
				out _itemInfos, 
				out _sourcePositions, 
				out _orderingValueSelectorExpressionOriginal, 
				out _orderingValueSelectorExpression, 
				out _orderingValueSelectorContainsParametrizedLiveLinqCalls, 
				ref _orderingValueSelectorExpressionInfo, 
				ref _orderingValueSelectorExpressionCallCount, 
				ref _orderingValueSelectorFunc, 
				ref _nestedComputings);

			_deferredQueuesCount = 3;
			_thisAsSourceCollectionChangeProcessor = this;
			_thisAsSourceItemChangeProcessor = this;
		}

		protected override void initialize()
		{
			initializeComparer();
			initializeSortDirectionScalar();
			Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
			Utils.initializeNestedComputings(_nestedComputings, this);
		}

		protected override void uninitialize()
		{
			if (_comparerScalar != null)
			{
				_comparerScalar.PropertyChanged -= _comparerScalarPropertyChangedEventHandler;
				_comparer = null;
			}			

			if (_sortDirectionScalar != null)
				_sortDirectionScalar.PropertyChanged -= _sortDirectionScalarPropertyChangedEventHandler;			

			Utils.uninitializeSourceScalar(_sourceScalar, scalarValueChangedHandler, ref _source);
			Utils.uninitializeNestedComputings(_nestedComputings, this);
		}

		protected override void initializeFromSource()
		{
			if (_sourceInitialized)
			{
				Utils.disposeExpressionItemInfos(_itemInfos, _orderingValueSelectorExpressionCallCount, this);
				Utils.removeDownstreamConsumedComputing(_itemInfos, this);

				int capacity = Utils.disposeSource(
					_sourceScalar, 
					_source,
					out _itemInfos,
					out _sourcePositions, 
					_sourceAsList, 
					handleSourceCollectionChanged);

				Utils.construct(capacity, out _orderedItemInfos, out _orderedPositions, out _orderingValues);

				_items.Clear();

				_sourceInitialized = false;
			}

			Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this, out _sourceAsList, false);

			if (_source != null && _isActive)
			{
				Utils.initializeFromObservableCollectionWithChangeMarker(
					_source, 
					ref _sourceAsList, 
					ref _rootSourceWrapper, 
					ref _lastProcessedSourceChangeMarker);

				int count = _sourceAsList.Count;
				TSourceItem[] sourceCopy = new TSourceItem[count];
				_sourceAsList.CopyTo(sourceCopy, 0);

				_sourceAsList.CollectionChanged += handleSourceCollectionChanged;

				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
					registerSourceItem(sourceCopy[sourceIndex], sourceIndex, true);

				_sourceInitialized = true;
			}

			reset();
		}


		private void registerSourceItem(TSourceItem sourceItem, int sourceIndex, bool initializing = false)
		{
			OrderingItemInfo<TOrderingValue> itemInfo = _sourcePositions.Insert(sourceIndex);

			Utils.getItemInfoContent(
				new object[]{sourceItem}, 
				out ExpressionWatcher expressionWatcher, 
				out Func<TOrderingValue> getOrderingValueFunc, 
				out List<IComputingInternal> nestedComputings,
				_orderingValueSelectorExpression,
				out _orderingValueSelectorExpressionCallCount,
				this,
				_orderingValueSelectorContainsParametrizedLiveLinqCalls,
				_orderingValueSelectorExpressionInfo);

			itemInfo.GetOrderingValueFunc = getOrderingValueFunc;
			itemInfo.ExpressionWatcher = expressionWatcher;
			itemInfo.NestedComputings = nestedComputings;

			TOrderingValue orderingValue = getOrderingValue(itemInfo, sourceItem);
			int orderedIndex = getOrderedIndex(orderingValue);

			itemInfo.ExpressionWatcher.ValueChanged = expressionWatcher_OnValueChanged;
			itemInfo.ExpressionWatcher._position = itemInfo;
			OrderedItemInfo<TOrderingValue> orderedItemInfo = _orderedPositions.Insert(orderedIndex);
			itemInfo.OrderedItemInfo = orderedItemInfo;
			orderedItemInfo.ItemInfo = itemInfo;
			_orderingValues.Insert(orderedIndex, orderingValue);

			if (_thenOrderingsCount > 0)
				adjustEqualOrderingValueRangePosition(orderingValue, orderedItemInfo, orderedIndex, orderedIndex - 1, orderedIndex);

			if (initializing)
				_items.Insert(orderedIndex, sourceItem);
			else
				baseInsertItem(orderedIndex, sourceItem);
		}

		private void adjustEqualOrderingValueRangePosition(
			TOrderingValue orderingValue, 
			OrderedItemInfo<TOrderingValue> orderedItemInfo,
			int orderedIndex,
			int previousOrderedIndex,
			int nextOrderedIndex)
		{
			bool isIncludedInRange = false;

			void tryIncludeInRange(OrderedItemInfo<TOrderingValue> nearbyOrderedItemInfo, int nearbyOrderedIndex)
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

			OrderedItemInfo<TOrderingValue> previousOrderedItemInfo = null;
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

		private void unregisterSourceItem(int sourceIndex)
		{
			OrderingItemInfo<TOrderingValue> itemInfo = _itemInfos[sourceIndex];
			Utils.disposeExpressionWatcher(itemInfo.ExpressionWatcher, itemInfo.NestedComputings, this, _orderingValueSelectorContainsParametrizedLiveLinqCalls);

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
			TOrderingValue getValue() => 
				!_orderingValueSelectorContainsParametrizedLiveLinqCalls 
				? _orderingValueSelectorFunc(_sourceAsList[sourceIndex]) 
				: _itemInfos[sourceIndex].GetOrderingValueFunc();

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TOrderingValue result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		public TOrderingValue GetOrderingValueByOrderedIndex(int orderedIndex)
		{
			TOrderingValue getValue() => 
				!_orderingValueSelectorContainsParametrizedLiveLinqCalls 
					? _orderingValueSelectorFunc(this[orderedIndex]) 
					: _orderedItemInfos[orderedIndex].ItemInfo.GetOrderingValueFunc();

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TOrderingValue result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		private TOrderingValue getOrderingValue(OrderingItemInfo<TOrderingValue> itemInfo, TSourceItem sourceItem)
		{
			TOrderingValue getValue() => 
				!_orderingValueSelectorContainsParametrizedLiveLinqCalls 
					? _orderingValueSelectorFunc(sourceItem) 
					: itemInfo.GetOrderingValueFunc();

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TOrderingValue result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				_rootSourceWrapper, 
				ref _lastProcessedSourceChangeMarker, 
				_sourceAsList, 
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, _deferredQueuesCount, 
				this)) return;

			_thisAsSourceCollectionChangeProcessor.processSourceCollectionChanged(sender, e);

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		void ISourceCollectionChangeProcessor.processSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newIndex = e.NewStartingIndex;
					TSourceItem addedItem = (TSourceItem) e.NewItems[0];
					registerSourceItem(addedItem, newIndex);
					break;
				case NotifyCollectionChangedAction.Remove:
					unregisterSourceItem(e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					int replacingSourceIndex = e.NewStartingIndex;
					TSourceItem replacingSourceItem = (TSourceItem) e.NewItems[0];
					OrderingItemInfo<TOrderingValue> replacingItemInfo = _itemInfos[replacingSourceIndex];
					ExpressionWatcher oldExpressionWatcher = replacingItemInfo.ExpressionWatcher;

					Utils.disposeExpressionWatcher(oldExpressionWatcher, replacingItemInfo.NestedComputings, this,
						_orderingValueSelectorContainsParametrizedLiveLinqCalls);

					Utils.getItemInfoContent(
						new object[] {replacingSourceItem},
						out ExpressionWatcher newExpressionWatcher,
						out Func<TOrderingValue> newGetOrderingValueFunc,
						out List<IComputingInternal> nestedComputings,
						_orderingValueSelectorExpression,
						out _orderingValueSelectorExpressionCallCount,
						this,
						_orderingValueSelectorContainsParametrizedLiveLinqCalls,
						_orderingValueSelectorExpressionInfo);

					replacingItemInfo.GetOrderingValueFunc = newGetOrderingValueFunc;
					replacingItemInfo.ExpressionWatcher = newExpressionWatcher;
					replacingItemInfo.ExpressionWatcher.ValueChanged = expressionWatcher_OnValueChanged;
					newExpressionWatcher._position = oldExpressionWatcher._position;
					replacingItemInfo.NestedComputings = nestedComputings;

					baseSetItem(replacingItemInfo.OrderedItemInfo.Index, replacingSourceItem);
					processSourceItemChange(replacingSourceIndex, replacingSourceItem);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex = e.OldStartingIndex;
					int newStartingIndex = e.NewStartingIndex;
					if (oldStartingIndex != newStartingIndex)
						_sourcePositions.Move(oldStartingIndex, newStartingIndex);
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
			}
		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(_sortDirectionScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(_comparerScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_sortDirectionScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_comparerScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			Utils.processSourceItemChange(
				expressionWatcher, 
				sender, 
				eventArgs, 
				_rootSourceWrapper, 
				_sourceAsList, 
				_lastProcessedSourceChangeMarker, 
				_thisAsSourceItemChangeProcessor,
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings, 
				2, _deferredQueuesCount, this);
		}


		private void processSourceItemChange(int sourceIndex, TSourceItem sourceItem)
		{
			void notifyThenOrderings(int newOrderedIndex)
			{
				int processedThenOrderingsCount = 0;
				for (int thenOrderingIndex = 0; thenOrderingIndex < _thenOrderingsCount; thenOrderingIndex++)
				{
					_thenOrderings[thenOrderingIndex].ProcessSourceItemChange(newOrderedIndex, sourceItem);
					processedThenOrderingsCount++;
					if (processedThenOrderingsCount == _thenOrderingsCount) break;
				}
			}

			OrderingItemInfo<TOrderingValue> itemInfo = _itemInfos[sourceIndex];
			OrderedItemInfo<TOrderingValue> orderedItemInfo = itemInfo.OrderedItemInfo;
			int orderedIndex = orderedItemInfo.Index;
			TOrderingValue orderingValue = getOrderingValue(itemInfo, sourceItem);

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

		void ISourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			if (expressionWatcher._disposed) return;
			int sourceIndex = expressionWatcher._position.Index;
			processSourceItemChange(
				sourceIndex, 
				_sourceAsList[sourceIndex]);
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

				int comparisonWithMiddleItem;
				int nextAfterMiddleIncrement;
				int comparisonWithNextAfterMiddleItem;

				int length = upperIndex - lowerIndex + 1;
				if (length <= 2)
				{
					if (length == 2)
					{
						int comparisonWithUpperItem = _comparer.Compare(orderingValue, _orderingValues[upperIndex]);
						int comparisonWithLowerEntity = _comparer.Compare(orderingValue, _orderingValues[lowerIndex]);

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
						newIndex = getNearIndex(
							_comparer.Compare(orderingValue,
							_orderingValues[lowerIndex]), lowerIndex);
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

				comparisonWithMiddleItem = _comparer.Compare(orderingValue, middleItemOrderingValue);

				if (comparisonWithMiddleItem == 0)
				{
					newIndex = middleIndex;
					return newIndex;
				}

				nextAfterMiddleIncrement = comparisonWithMiddleItem * (_sortDirection == ListSortDirection.Descending ? -1 : 1);
				int nextAfterMiddleIndex = middleIndex;
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
					comparisonWithNextAfterMiddleItem = _comparer.Compare(orderingValue, nextAfterMiddleItemOrderingValue);
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
							lowerIndex = nextAfterMiddleIndex;
						else
							upperIndex = nextAfterMiddleIndex;
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
			return _itemInfos[sourceIndex].OrderedItemInfo.Index;
		}

		public int GetSourceIndexByOrderedIndex(int orderedIndex)
		{
			return _orderedItemInfos[orderedIndex].ItemInfo.Index;	
		}

		//#region Implementation of IOrdering<TSourceItem>

		public bool Compare(int resultIndex1, int resultIndex2)
		{
			return
				_comparer.Compare(
					GetOrderingValueByOrderedIndex(resultIndex1),
					GetOrderingValueByOrderedIndex(resultIndex2)) == 0;
		}

		//public IOrdering<TSourceItem> Parent => null;
		//#endregion

		void IOrderingInternal<TSourceItem>.AddThenOrdering(IThenOrdering<TSourceItem> thenOrdering)
		{
			_thenOrderingsCount++;
			_thenOrderings = _thenOrderings ?? new List<IThenOrderingInternal<TSourceItem>>(4);
			_thenOrderings.Add((IThenOrderingInternal<TSourceItem>) thenOrdering);

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
							// ReSharper disable once PossibleNullReferenceException
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
					// ReSharper disable once PossibleNullReferenceException
					rangePosition.Length = equalOrderingValueItemsCount;
			}
		}

		void IOrderingInternal<TSourceItem>.RemoveThenOrdering(IThenOrderingInternal<TSourceItem> thenOrdering)
		{
			_thenOrderingsCount--;
			_thenOrderings.Remove(thenOrdering);
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
					if (_comparer.Compare(orderingValue, GetOrderingValueByOrderedIndex(lowerIndex)) == 0)
						return new OrderingIndicesRange(0, 0);

					return null;
				}

				if (length == 2)
				{
					bool lower = _comparer.Compare(orderingValue, GetOrderingValueByOrderedIndex(lowerIndex)) == 0;
					bool upper = _comparer.Compare(orderingValue, GetOrderingValueByOrderedIndex(upperIndex)) == 0;

					if (!upper && !lower) return null;
					return new OrderingIndicesRange(lower ? 0 : 1, upper ? 0 : 1);
				}

				int middleIndex = lowerIndex + (length >> 1);

				TOrderingValue middleItemOrderingValue = GetOrderingValueByOrderedIndex(middleIndex);
				int comparisonWithMiddleItem = _comparer.Compare(orderingValue, middleItemOrderingValue);

				if (comparisonWithMiddleItem == 0)
				{
					int nextAfterMiddleIndex = middleIndex;
					int from = middleIndex;
					do
					{
						nextAfterMiddleIndex = nextAfterMiddleIndex - 1;
						if (_comparer.Compare(GetOrderingValueByOrderedIndex(nextAfterMiddleIndex), middleItemOrderingValue) == 0)
							from = nextAfterMiddleIndex;
						else
							break;	
					} while (
						nextAfterMiddleIndex != lowerIndex);


					nextAfterMiddleIndex = middleIndex;
					int to = middleIndex;
					do
					{
						nextAfterMiddleIndex = nextAfterMiddleIndex + 1;
						if (_comparer.Compare(GetOrderingValueByOrderedIndex(nextAfterMiddleIndex), middleItemOrderingValue) == 0)
							to = nextAfterMiddleIndex;
						else
							break;	
					} while (
						nextAfterMiddleIndex != upperIndex);

					return new OrderingIndicesRange(from, to);
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

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			if (_itemInfos.Count != Count) throw new ObservableComputationsException(this, "Consistency violation: Ordering.7");

			List<TSourceItem> copy = this.ToList();			
			// ReSharper disable once PossibleNullReferenceException
			for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
			{
				TSourceItem sourceItem = source[sourceIndex];
				if (!copy.Remove(sourceItem)) throw new ObservableComputationsException(this, "Consistency violation: Ordering.1");
			}

			_orderedPositions.ValidateConsistency();

			Func<TSourceItem, TOrderingValue> orderingValueSelector = _orderingValueSelectorExpression.Compile();
			IComparer<TOrderingValue> comparer = _comparerScalar.getValue(_comparer) ?? Comparer<TOrderingValue>.Default;
			ListSortDirection listSortDirection = _sortDirectionScalar.getValue(_sortDirection);

			if (_orderingValues.Count != Count) 
				throw new ObservableComputationsException(this, "Consistency violation: Ordering.14");

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
						throw new ObservableComputationsException(this, "Consistency violation: Ordering.3");

					if (_thenOrderingsCount > 0)
					{
						if (compareResult == 0)
						{
							equalOrderingValueItemsCount++;
							if (rangePosition !=  _orderedItemInfos[orderedIndex].RangePosition)
								throw new ObservableComputationsException(this, "Consistency violation: Ordering.17");
						}
						else				
						{
							// ReSharper disable once PossibleNullReferenceException
							if (rangePosition.Length != equalOrderingValueItemsCount)
								throw new ObservableComputationsException(this, "Consistency violation: Ordering.20");

							if (rangePosition.Index != rangePositionIndex)
								throw new ObservableComputationsException(this, "Consistency violation: Ordering.21");

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

				OrderingItemInfo<TOrderingValue> itemInfo = _orderedItemInfos[orderedIndex].ItemInfo;
				if (itemInfo.OrderedItemInfo.Index != orderedIndex) throw new ObservableComputationsException(this, "Consistency violation: Ordering.13");

				if (!EqualityComparer<TOrderingValue>.Default.Equals(_orderingValues[orderedIndex], orderingValueSelector(orderedItem)))
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.10");
			}

			if (_thenOrderingsCount > 0 && Count > 0)
			{
				// ReSharper disable once PossibleNullReferenceException
				if (rangePosition.Length != equalOrderingValueItemsCount)
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.22");

				if (rangePosition.Index != rangePositionIndex)
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.23");
			}

			_sourcePositions.ValidateConsistency();

			if (_sourcePositions.List.Count != source.Count)
				throw new ObservableComputationsException(this, "Consistency violation: Ordering.15");

			for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
			{
				TSourceItem sourceItem = source[sourceIndex];
				OrderingItemInfo<TOrderingValue> itemInfo = _itemInfos[sourceIndex];
				if (itemInfo.ExpressionWatcher._position != _sourcePositions.List[sourceIndex])
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.8");
				if (!EqualityComparer<TOrderingValue>.Default.Equals(_orderingValues[itemInfo.OrderedItemInfo.Index], orderingValueSelector(sourceItem)))
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.9");

				if (!_orderedItemInfos.Contains(_itemInfos[sourceIndex].OrderedItemInfo))
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.11");

				if (!_sourcePositions.List.Contains(_itemInfos[sourceIndex].ExpressionWatcher._position))
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.12");

				if (_itemInfos[sourceIndex].ExpressionWatcher._position.Index != sourceIndex)
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.16");
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

		public OrderingIndicesRange(int from, int to)
		{
			From = from;
			To = to;
		}
	}
}
