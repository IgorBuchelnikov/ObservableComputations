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
	public class ThenOrdering<TSourceItem, TOrderingValue> : CollectionComputing<TSourceItem>, INotifyPropertyChanged, IOrderingInternal<TSourceItem>, IThenOrderingInternal<TSourceItem>, IHasSourceCollections, ISourceItemChangeProcessor, ISourceCollectionChangeProcessor
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

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new INotifyCollectionChanged[]{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new IReadScalar<INotifyCollectionChanged>[]{SourceScalar});


		internal IOrderingInternal<TSourceItem> _source;
		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
        private ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;

		private Positions<OrderedItemInfo<TOrderingValue>> _orderedPositions;
		private Positions<OrderingItemInfo<TOrderingValue>> _sourcePositions;

		//private readonly List<Expression<Func<TResultElement>>> _selectorExpressions;
		private List<OrderingItemInfo<TOrderingValue>> _itemInfos;
		private List<OrderedItemInfo<TOrderingValue>> _orderedItemInfos;

		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorExpression;

		private readonly ExpressionWatcher.ExpressionInfo _orderingValueSelectorExpressionInfo;

		private readonly bool _orderingValueSelectorContainsParametrizedObservableComputationsCalls;

		private PropertyChangedEventHandler _comparerScalarPropertyChangedEventHandler;

		private PropertyChangedEventHandler _sortDirectionScalarPropertyChangedEventHandler;

		List<TOrderingValue> _orderingValues;
		
		private bool _sourceInitialized;
		private readonly IReadScalar<IOrdering<TSourceItem>> _sourceScalar;
		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorExpressionOriginal;
		private readonly IReadScalar<ListSortDirection> _sortDirectionScalar;
		private readonly IReadScalar<IComparer<TOrderingValue>> _comparerScalar;
		internal readonly Func<TSourceItem, TOrderingValue> _orderingValueSelectorFunc;
		internal IComparer<TOrderingValue> _comparer;
		internal ListSortDirection _sortDirection;

		private RangePositions<RangePosition> _equalOrderingValueRangePositions;
		private int _thenOrderingsCount;
		private List<IThenOrderingInternal<TSourceItem>> _thenOrderings;

        private int _orderingValueSelectorExpressionСallCount;
        private List<IComputingInternal> _nestedComputings;
        private ISourceItemChangeProcessor _thisAsSourceItemChangeProcessor;

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
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = (IOrderingInternal<TSourceItem>) source;
            //_source.AddThenOrdering(this);
			_sortDirectionScalar = sortDirectionScalar;
			_comparerScalar = comparerScalar;

		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = (IOrderingInternal<TSourceItem>) source;
			_sortDirection = sortDirection;
			_comparerScalar = comparerScalar;
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = (IOrderingInternal<TSourceItem>) source;
			_sortDirectionScalar = sortDirectionScalar;
			_comparer = comparer;
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = (IOrderingInternal<TSourceItem>) source;
			_sortDirection = sortDirection;
			_comparer = comparer;
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sortDirectionScalar = sortDirectionScalar;
			_comparerScalar = comparerScalar;
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sortDirection = sortDirection;
			_comparerScalar = comparerScalar;
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sortDirectionScalar = sortDirectionScalar;
			_comparer = comparer;
		}

		[ObservableComputationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sortDirection = sortDirection;
			_comparer = comparer;
		}

		private ThenOrdering(
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			int sourceCapacity) : base(sourceCapacity)
		{
            Utils.construct(sourceCapacity, out _orderedItemInfos, out _orderedPositions, out _orderingValues);

            Utils.construct(
                orderingValueSelectorExpression, 
                sourceCapacity, 
                out _itemInfos, 
                out _sourcePositions, 
                out _orderingValueSelectorExpressionOriginal, 
                out _orderingValueSelectorExpression, 
                out _orderingValueSelectorContainsParametrizedObservableComputationsCalls, 
                ref _orderingValueSelectorExpressionInfo, 
                ref _orderingValueSelectorExpressionСallCount, 
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
            _source?.AddThenOrdering(this);
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

            _source?.RemoveThenOrdering(this);
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

		protected override void initializeFromSource()
		{

			if (_sourceInitialized)
			{
                Utils.disposeExpressionItemInfos(_itemInfos, _orderingValueSelectorExpressionСallCount, this);
                Utils.RemoveDownstreamConsumedComputing(_itemInfos, this);

                int capacity = Utils.disposeSource(
                    _sourceScalar, 
                    _source,
                    out _itemInfos,
                    out _sourcePositions, 
                    _sourceAsList, 
                    handleSourceCollectionChanged);

                Utils.construct(capacity, out _orderedItemInfos, out _orderedPositions, out _orderingValues);

                _source.RemoveThenOrdering(this);

				_items.Clear();

                _sourceInitialized = false;
            }

            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this, out _sourceAsList, false);

			if (_source != null && _isActive)
			{
				_source.AddThenOrdering(this);

                Utils.initializeFromObservableCollectionWithChangeMarker(
                    _source, 
                    ref _sourceAsList, 
                    ref _rootSourceWrapper, 
                    ref _lastProcessedSourceChangeMarker);


				int count = _sourceAsList.Count;
                TSourceItem[] sourceCopy = new TSourceItem[count];
                _sourceAsList.CopyTo(sourceCopy, 0);

                _sourceAsList.CollectionChanged += handleSourceCollectionChanged;

				if (count > 0)
                {
                    Range[] ranges = new Range[count];

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

                        ranges[index] = new Range(sourceLowerIndex, sourceUpperIndex);
					}

                    for (int index = 0; index < count; index++)
                    {
                        Range range = ranges[index];
                        registerSourceItem(sourceCopy[index], index, range.sourceLowerIndex, range.sourceUpperIndex, true);
                    }
				}

                _sourceInitialized = true;
            }

			reset();
		}

        private struct Range
        {
            public int sourceLowerIndex;
            public int sourceUpperIndex;

            public Range(int sourceLowerIndex, int sourceUpperIndex)
            {
                this.sourceLowerIndex = sourceLowerIndex;
                this.sourceUpperIndex = sourceUpperIndex;
            }
        }

		private void registerSourceItem(TSourceItem sourceItem, int sourceIndex, int sourceLowerIndex, int sourceUpperIndex, bool initializing = false)
		{
			OrderingItemInfo<TOrderingValue> itemInfo = _sourcePositions.Insert(sourceIndex);

			Utils.getItemInfoContent(
                new object[]{sourceItem}, 
                out ExpressionWatcher expressionWatcher, 
                out Func<TOrderingValue> getOrderingValueFunc, 
                out List<IComputingInternal> nestedComputings,
                _orderingValueSelectorExpression,
                out _orderingValueSelectorExpressionСallCount,
                this,
                _orderingValueSelectorContainsParametrizedObservableComputationsCalls,
                _orderingValueSelectorExpressionInfo);

			itemInfo.GetOrderingValueFunc = getOrderingValueFunc;
			itemInfo.ExpressionWatcher = expressionWatcher;
            itemInfo.NestedComputings = nestedComputings;

			TOrderingValue orderingValue = getOrderingValue(itemInfo, sourceItem);

			int orderedIndex = sourceLowerIndex <= sourceUpperIndex 
				? getOrderedIndex(orderingValue, sourceLowerIndex, sourceUpperIndex)
				: sourceIndex;

			if (orderedIndex < sourceLowerIndex) orderedIndex = sourceLowerIndex;

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

			RangePosition sourceRangePosition = _source.GetRangePosition(orderedIndex);
			OrderedItemInfo<TOrderingValue> previousOrderedItemInfo = null;
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

		private void unregisterSourceItem(int sourceIndex)
		{
			OrderingItemInfo<TOrderingValue> itemInfo = _itemInfos[sourceIndex];
            Utils.disposeExpressionWatcher(itemInfo.ExpressionWatcher, itemInfo.NestedComputings, this, _orderingValueSelectorContainsParametrizedObservableComputationsCalls);

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
			return getOrderingValueBySourceIndex(sourceIndex);
		}

		internal TOrderingValue getOrderingValueBySourceIndex(int sourceIndex)
		{
            TOrderingValue getValue() =>  
                !_orderingValueSelectorContainsParametrizedObservableComputationsCalls 
                    ? _orderingValueSelectorFunc(_sourceAsList[sourceIndex]) 
                    : _itemInfos[sourceIndex].GetOrderingValueFunc();

            if (Configuration.TrackComputingsExecutingUserCode)
            {
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
                TOrderingValue result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
                return result;
            }

            return getValue();
        }

		public TOrderingValue GetOrderingValueByOrderedIndex(int orderedIndex)
		{
			return getOrderingValueByOrderedIndex(orderedIndex);
		}

		private TOrderingValue getOrderingValueByOrderedIndex(int orderedIndex)
		{
            TOrderingValue getValue() =>  
                !_orderingValueSelectorContainsParametrizedObservableComputationsCalls 
				    ? _orderingValueSelectorFunc(this[orderedIndex]) 
				    : _orderedItemInfos[orderedIndex].ItemInfo.GetOrderingValueFunc();

            if (Configuration.TrackComputingsExecutingUserCode)
            {
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
                TOrderingValue result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
                return result;
            }

            return getValue();
		}

		private TOrderingValue getOrderingValue(OrderingItemInfo<TOrderingValue> itemInfo, TSourceItem sourceItem)
		{
            TOrderingValue getValue() => 
                !_orderingValueSelectorContainsParametrizedObservableComputationsCalls 
				    ? _orderingValueSelectorFunc(sourceItem) 
				    : itemInfo.GetOrderingValueFunc();

            if (Configuration.TrackComputingsExecutingUserCode)
            {
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
                TOrderingValue result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
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
                this,
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
                    RangePosition sourceRangePosition = _source.GetRangePosition(newIndex);
                    registerSourceItem(addedItem, newIndex, sourceRangePosition.PlainIndex,
                        sourceRangePosition.PlainIndex + sourceRangePosition.Length - 2);
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
                        _orderingValueSelectorContainsParametrizedObservableComputationsCalls);

                    Utils.getItemInfoContent(
                        new object[] {replacingSourceItem},
                        out ExpressionWatcher newExpressionWatcher,
                        out Func<TOrderingValue> newGetOrderingValueFunc,
                        out List<IComputingInternal> nestedComputings,
                        _orderingValueSelectorExpression,
                        out _orderingValueSelectorExpressionСallCount,
                        this,
                        _orderingValueSelectorContainsParametrizedObservableComputationsCalls,
                        _orderingValueSelectorExpressionInfo);

                    replacingItemInfo.GetOrderingValueFunc = newGetOrderingValueFunc;
                    replacingItemInfo.ExpressionWatcher = newExpressionWatcher;
                    replacingItemInfo.NestedComputings = nestedComputings;
                    replacingItemInfo.ExpressionWatcher.ValueChanged = expressionWatcher_OnValueChanged;
                    replacingItemInfo.ExpressionWatcher._position = oldExpressionWatcher._position;

                    baseSetItem(replacingItemInfo.OrderedItemInfo.Index, replacingSourceItem);
                    break;
                case NotifyCollectionChangedAction.Move:
                    int oldStartingIndex = e.OldStartingIndex;
                    int newStartingIndex = e.NewStartingIndex;
                    if (oldStartingIndex != newStartingIndex)
                    {
                        _sourcePositions.Move(oldStartingIndex, newStartingIndex);
                        processSourceItemChange(newStartingIndex, false, (TSourceItem) e.NewItems[0]);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    initializeFromSource();

                    break;
            }
        }


        private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
            Utils.ProcessSourceItemChange(
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


		private void processSourceItemChange(int sourceIndex, bool checkOrderingValues, TSourceItem sourceItem)
		{
			void notifyThenOrderings(int newOrderedIndex)
			{
				int processedThenOrderingsCount = 0;
				for (int thenOrderingIndex = 0; thenOrderingIndex < _thenOrderingsCount; thenOrderingIndex++)
				{
					IThenOrderingInternal<TSourceItem> thenOrdering = _thenOrderings[thenOrderingIndex];
                    thenOrdering.ProcessSourceItemChange(newOrderedIndex, sourceItem);
                    processedThenOrderingsCount++;
                    if (processedThenOrderingsCount == _thenOrderingsCount) break;
				}
			}

			OrderingItemInfo<TOrderingValue> itemInfo = _itemInfos[sourceIndex];
			OrderedItemInfo<TOrderingValue> orderedItemInfo = itemInfo.OrderedItemInfo;
			int orderedIndex = orderedItemInfo.Index;
			TOrderingValue orderingValue = !_orderingValueSelectorContainsParametrizedObservableComputationsCalls 
				? _orderingValueSelectorFunc(sourceItem) 
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
		}

        void ISourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
        {
            if (expressionWatcher._disposed) return;
            int sourceIndex = expressionWatcher._position.Index;
            processSourceItemChange(sourceIndex, true, _sourceAsList[sourceIndex]);
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
			return _itemInfos[sourceIndex].OrderedItemInfo.Index;
		}

		public int GetSourceIndexByOrderedIndex(int orderedIndex)
		{
			return _orderedItemInfos[orderedIndex].ItemInfo.Index;	
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
                            // ReSharper disable once PossibleNullReferenceException
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

		void IThenOrderingInternal<TSourceItem>.ProcessSourceItemChange(int sourceIndex, TSourceItem sourceItem)
		{
			processSourceItemChange(sourceIndex, false, sourceItem);
		}

		public bool Compare(int resultIndex1, int resultIndex2)
		{
			int sourceIndex1 = getSourceIndexByOrderedIndex(resultIndex1);
			int sourceIndex2 = getSourceIndexByOrderedIndex(resultIndex2);
			return
				_comparer.Compare(
					getOrderingValueByOrderedIndex(resultIndex1),
					getOrderingValueByOrderedIndex(resultIndex2)) == 0
				&& _source.Compare(sourceIndex1, sourceIndex2);
		}

		public void ValidateConsistency()
		{
			IComparer<TOrderingValue> comparer = _comparerScalar.getValue(_comparer) ?? Comparer<TOrderingValue>.Default;
			ListSortDirection listSortDirection = _sortDirectionScalar.getValue(_sortDirection);
			Func<TSourceItem, TOrderingValue> orderingValueSelector = _orderingValueSelectorExpressionOriginal.Compile();
            IOrderingInternal<TSourceItem> source = (IOrderingInternal<TSourceItem>) _sourceScalar.getValue(_source) ?? new ObservableCollection<TSourceItem>().Ordering(i => 0);
			source.ValidateConsistency();

			IOrdering<TSourceItem > orderingSource = source;

			if (_itemInfos.Count != Count) throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.1");

			if (_orderingValues.Count != Count) 
				throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.2");

			if (_sourcePositions.List.Count != source.Count)
				throw new ObservableComputationsException(this, "Consistency violation: Ordering.3");

			if (_thenOrderingsCount > 0)
			{
				_equalOrderingValueRangePositions.ValidateConsistency();
			}

			List<TSourceItem> copy = this.ToList();	
			List<TSourceItem> buffer = new List<TSourceItem>();	
			for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
			{
				TSourceItem sourceItem = source[sourceIndex];
				if (!copy.Remove(sourceItem)) throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.4");

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
								throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.5");
						}

						for (int bufferIndex = 0; bufferIndex < orderedBuffer.Count; bufferIndex++)
						{
							if (bufferIndex > 0)
							{
								int compareResult = comparer.Compare(orderingValueSelector(orderedBuffer[bufferIndex - 1]), orderingValueSelector(orderedBuffer[bufferIndex]));
								if ((compareResult < 0 && listSortDirection == ListSortDirection.Descending)
									|| (compareResult > 0 && listSortDirection == ListSortDirection.Ascending)) 
									throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.6");								
							}
						}


						buffer.Clear();
						buffer.Add(source[sourceIndex]);
					}
				}
				else
				{
					buffer.Add(source[sourceIndex]);
				}

				OrderingItemInfo<TOrderingValue> itemInfo = _itemInfos[sourceIndex];
				if (itemInfo.ExpressionWatcher._position != _sourcePositions.List[sourceIndex])
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.7");
				if (!EqualityComparer<TOrderingValue>.Default.Equals(_orderingValues[itemInfo.OrderedItemInfo.Index], orderingValueSelector(sourceItem)))
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.8");

				if (!_orderedItemInfos.Contains(_itemInfos[sourceIndex].OrderedItemInfo))
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.9");

				if (!_sourcePositions.List.Contains(_itemInfos[sourceIndex].ExpressionWatcher._position))
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.10");

				if (_itemInfos[sourceIndex].ExpressionWatcher._position.Index != sourceIndex)
					throw new ObservableComputationsException(this, "Consistency violation: Ordering.11");
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
				//	throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.12");

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
								throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.14");
						}
						else				
						{
                            // ReSharper disable once PossibleNullReferenceException
                            if (rangePosition.Length != equalOrderingValueItemsCount)
								throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.15");

							if (rangePosition.Index != rangePositionIndex)
								throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.16");

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
				if (itemInfo.OrderedItemInfo.Index != orderedIndex) throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.17");

				if (!EqualityComparer<TOrderingValue>.Default.Equals(_orderingValues[orderedIndex], orderingValueSelector(orderedItem)))
					throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.18");
			}

			//for (int i = 0; i < _itemInfos.Count; i++)
			//{
			//	if (!_orderingInfoPositions.List.Contains(_itemInfos[i].OrderingInfoRangePosition))
			//		throw new ObservableComputationsException(this, "Consistency violation: ThenOrdering.12");
			//}
		}

		RangePosition IOrderingInternal<TSourceItem>.GetRangePosition(int orderedIndex)
		{
			return _orderedItemInfos[orderedIndex].RangePosition;
		}
	}
}
