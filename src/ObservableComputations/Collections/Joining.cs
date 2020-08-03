using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ObservableComputations.ExtentionMethods;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace ObservableComputations
{
    public class
        Joining<TLeftSourceItem, TRightSourceItem> : CollectionComputing<JoinPair<TLeftSourceItem, TRightSourceItem>>,
            IHasSourceCollections, ICanProcessSourceItemChange, IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>>
    {
        public IReadScalar<INotifyCollectionChanged> LeftSourceScalar => _leftSourceScalar;

        // ReSharper disable once MemberCanBePrivate.Global
        public IReadScalar<INotifyCollectionChanged> RightSourceScalar => _rightSourceScalar;

        // ReSharper disable once MemberCanBePrivate.Global
        public INotifyCollectionChanged LeftSource => _leftSource;

        // ReSharper disable once MemberCanBePrivate.Global
        public INotifyCollectionChanged RightSource => _rightSource;

        // ReSharper disable once MemberCanBePrivate.Global
        public Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> JoinPredicateExpression { get; }

        public new ReadOnlyCollection<INotifyCollectionChanged> SourceCollections =>
            new ReadOnlyCollection<INotifyCollectionChanged>(new[] {LeftSource, RightSource});

        public new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars =>
            new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new[] {LeftSourceScalar, RightSourceScalar});


        // ReSharper disable once MemberCanBePrivate.Global
        public Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> PredicateExpression => _predicateExpressionOriginal;


        // ReSharper disable once MemberCanBePrivate.Global
        public Func<TLeftSourceItem, TRightSourceItem, bool> PredicateFunc => _predicateFunc;


        private List<IComputingInternal> _nestedComputings;

        private readonly Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> _predicateExpression;
        private readonly Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> _predicateExpressionOriginal;
        private int _predicateExpressionСallCount;

        private Positions<Position> _filteredPositions;
        private Positions<FilteringItemInfo> _sourcePositions;
        private List<FilteringItemInfo> _itemInfos;

        private readonly ExpressionWatcher.ExpressionInfo _predicateExpressionInfo;

        private NotifyCollectionChangedEventHandler _leftSourceNotifyCollectionChangedEventHandler;
        private NotifyCollectionChangedEventHandler _rightSourceNotifyCollectionChangedEventHandler;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private PropertyChangedEventHandler _leftSourceScalarPropertyChangedEventHandler;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private PropertyChangedEventHandler _rightSourceScalarPropertyChangedEventHandler;

        private ObservableCollectionWithChangeMarker<TLeftSourceItem> _leftSourceAsList;
        private ObservableCollectionWithChangeMarker<TRightSourceItem> _rightSourceAsList;
        bool _rootLeftSourceWrapper;
        bool _rootRightSourceWrapper;
        private bool _lastProcessedLeftSourceChangeMarker;
        private bool _lastProcessedRightSourceChangeMarker;

        private readonly bool _predicateContainsParametrizedObservableComputationsCalls;

        private Queue<ExpressionWatcher.Raise> _deferredExpressionWatcherChangedProcessings;
        private IReadScalar<INotifyCollectionChanged> _leftSourceScalar;
        internal INotifyCollectionChanged _leftSource;
        private readonly IReadScalar<INotifyCollectionChanged> _rightSourceScalar;
        internal INotifyCollectionChanged _rightSource;

        private readonly Func<TLeftSourceItem, TRightSourceItem, bool> _predicateFunc;
        private IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>> _thisAsFiltering;

        [ObservableComputationsCall]
        public Joining(
            IReadScalar<INotifyCollectionChanged> leftSourceScalar,
            IReadScalar<INotifyCollectionChanged> rightSourceScalar,
            Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
            int capacity = 0) : this(predicateExpression, Utils.getCapacity(leftSourceScalar) * Utils.getCapacity(rightSourceScalar), capacity)
        {
            _leftSourceScalar = leftSourceScalar;
            _rightSourceScalar = rightSourceScalar;
        }

        [ObservableComputationsCall]
        public Joining(
            INotifyCollectionChanged leftSource,
            IReadScalar<INotifyCollectionChanged> rightSourceScalar,
            Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
            int capacity = 0) : this(predicateExpression, Utils.getCapacity(leftSource) * Utils.getCapacity(rightSourceScalar), capacity)
        {
            _leftSource = leftSource;
            _rightSourceScalar = rightSourceScalar;
        }

        [ObservableComputationsCall]
        public Joining(
            IReadScalar<INotifyCollectionChanged> leftSourceScalar,
            INotifyCollectionChanged rightSource,
            Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
            int capacity = 0) : this(predicateExpression, Utils.getCapacity(leftSourceScalar) * Utils.getCapacity(rightSource), capacity)
        {
            _leftSourceScalar = leftSourceScalar;
            _rightSource = rightSource;
        }

        [ObservableComputationsCall]
        public Joining(
            INotifyCollectionChanged leftSource,
            INotifyCollectionChanged rightSource,
            Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
            int capacity = 0) : this(predicateExpression, Utils.getCapacity(leftSource) * Utils.getCapacity(rightSource), capacity)
        {
            _leftSource = leftSource;
            _rightSource = rightSource;
        }

        private Joining(Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
            int sourceCapacity, int capacity) : base(capacity)
        {
            Utils.construct(
                predicateExpression,
                sourceCapacity,
                ref _itemInfos,
                ref _sourcePositions,
                ref _predicateExpressionOriginal,
                ref _predicateExpression,
                ref _predicateContainsParametrizedObservableComputationsCalls,
                ref _predicateExpressionInfo,
                ref _predicateExpressionСallCount,
                ref _predicateFunc,
                ref _nestedComputings);

            _thisAsFiltering = this;

            _initialCapacity = capacity;
            _filteredPositions = new Positions<Position>(new List<Position>(_initialCapacity));
        }

        protected override void initializeFromSource()
        {
            int originalCount = _items.Count;

            if (_leftSourceNotifyCollectionChangedEventHandler != null || _rightSourceScalarPropertyChangedEventHandler != null)
            {
                Utils.disposeExpressionItemInfos(_itemInfos, _predicateExpressionСallCount, this);

                Utils.initializeItemInfos(
                    Utils.getCapacity(_leftSourceScalar, _leftSource) * Utils.getCapacity(_rightSourceScalar, _rightSource),
                    out _itemInfos,
                    out _sourcePositions);

                _filteredPositions = new Positions<Position>(new List<Position>(_initialCapacity));
            }

            if (_leftSourceNotifyCollectionChangedEventHandler != null)
            {
                Utils.unsubscribeSource(
                    _leftSourceAsList,
                    ref _leftSourceNotifyCollectionChangedEventHandler);
            }

            if (_rightSourceNotifyCollectionChangedEventHandler != null)
            {
                Utils.unsubscribeSource(
                    _rightSourceAsList,
                    ref _rightSourceNotifyCollectionChangedEventHandler);
            }

            Utils.changeSource(ref _leftSource, _leftSourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _leftSourceAsList, null);

            Utils.changeSource(ref _rightSource, _rightSourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _rightSourceAsList, null);

            if (_leftSource != null && _isActive)
            {
                Utils.initializeFromObservableCollectionWithChangeMarker(
                    _leftSource,
                    ref _leftSourceAsList,
                    ref _rootLeftSourceWrapper,
                    ref _lastProcessedLeftSourceChangeMarker);

                Utils.initializeFromObservableCollectionWithChangeMarker(
                    _rightSource,
                    ref _rightSourceAsList,
                    ref _rootRightSourceWrapper,
                    ref _lastProcessedRightSourceChangeMarker);

                Position nextItemPosition = _filteredPositions.Add();
                int leftCount = _leftSourceAsList.Count;
                int rightCount = (_leftSourceAsList?.Count).GetValueOrDefault();
                int insertingIndex = 0;
                int leftSourceIndex;
                int rightSourceIndex;

                for (leftSourceIndex = 0; leftSourceIndex < leftCount; leftSourceIndex++)
                {
                    TLeftSourceItem leftSourceItem = _leftSourceAsList[leftSourceIndex];

                    for (rightSourceIndex = 0; rightSourceIndex < rightCount; rightSourceIndex++)
                    {
                        TRightSourceItem rightSourceItem = _rightSourceAsList[rightSourceIndex];
                        Position currentFilteredItemPosition = null;

                        FilteringItemInfo itemInfo = registerSourceItem(leftSourceItem, rightSourceItem, leftSourceIndex + rightSourceIndex, null, null);

                        if (ApplyPredicate(leftSourceIndex, rightSourceIndex))
                        {
                            JoinPair<TLeftSourceItem, TRightSourceItem> joinPair 
                                = new JoinPair<TLeftSourceItem, TRightSourceItem>(leftSourceItem, rightSourceItem);
                            
                            if (originalCount > insertingIndex)
                                _items[insertingIndex++] = joinPair;
                            else
                                _items.Insert(insertingIndex++, joinPair);

                            currentFilteredItemPosition = nextItemPosition;
                            nextItemPosition = _filteredPositions.Add();
                        }

                        itemInfo.FilteredPosition = currentFilteredItemPosition;
                        itemInfo.NextFilteredItemPosition = nextItemPosition;
                    }
                }

                for (int index = originalCount - 1; index >= insertingIndex; index--)
                {
                    _items.RemoveAt(index);
                }

                _leftSourceNotifyCollectionChangedEventHandler = handleLeftSourceCollectionChanged;
                _leftSourceAsList.CollectionChanged += _leftSourceNotifyCollectionChangedEventHandler;

                if (_rightSourceAsList != null)
                {
                    _rightSourceNotifyCollectionChangedEventHandler = handleRightSourceCollectionChanged;
                    _rightSourceAsList.CollectionChanged += _rightSourceNotifyCollectionChangedEventHandler;
                }
            }
            else
            {
                _items.Clear();
            }

            reset();
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_leftSourceScalar, ref _leftSourceScalarPropertyChangedEventHandler, ref _leftSource,
                getScalarValueChangedHandler());
            Utils.initializeSourceScalar(_rightSourceScalar, ref _rightSourceScalarPropertyChangedEventHandler, ref _rightSource,
                getScalarValueChangedHandler());
            Utils.initializeNestedComputings(_nestedComputings, this);
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_leftSourceScalar, _leftSourceScalarPropertyChangedEventHandler);
            Utils.uninitializeSourceScalar(_rightSourceScalar, _rightSourceScalarPropertyChangedEventHandler);
            Utils.uninitializeNestedComputings(_nestedComputings, this);
        }

        private void handleLeftSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!Utils.preHandleSourceCollectionChanged(
                sender,
                e,
                _rootLeftSourceWrapper,
                ref _lastProcessedLeftSourceChangeMarker,
                _leftSourceAsList,
                _isConsistent,
                this,
                ref _handledEventSender,
                ref _handledEventArgs)) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _isConsistent = false;
                    int newLeftSourceIndex = e.NewStartingIndex;
                    TLeftSourceItem addedLeftSourceItem = _leftSourceAsList[newLeftSourceIndex];
                    int count = _rightSourceAsList.Count;
                    int baseIndex = newLeftSourceIndex * count;

                    for (int rightSourceIndex = 0; rightSourceIndex < count; rightSourceIndex++)
                    {
                        TRightSourceItem rightSourceItem = _rightSourceAsList[rightSourceIndex];

                        Position newItemPosition = null;
                        Position nextItemPosition;

                        int? newFilteredIndex  = null;

                        Utils.getItemInfoContent(
                            new object[]{addedLeftSourceItem, rightSourceItem}, 
                            out ExpressionWatcher newWatcher, 
                            out Func<bool> newPredicateFunc, 
                            out List<IComputingInternal> nestedComputings,
                            _predicateExpression,
                            ref _predicateExpressionСallCount,
                            this,
                            _predicateContainsParametrizedObservableComputationsCalls,
                            _predicateExpressionInfo);

                        bool predicateValue = applyPredicate(addedLeftSourceItem, rightSourceItem, newPredicateFunc);
                        int newSourceIndex = baseIndex + rightSourceIndex;
                        nextItemPosition = FilteringUtils.processAddSourceItem(newSourceIndex, predicateValue, ref newItemPosition, ref newFilteredIndex, _itemInfos, _filteredPositions, Count);

                        registerSourceItem(addedLeftSourceItem, rightSourceItem, newSourceIndex, newItemPosition, nextItemPosition, newWatcher, newPredicateFunc, nestedComputings);

                        if (newFilteredIndex != null)
                            baseInsertItem(newFilteredIndex.Value, new JoinPair<TLeftSourceItem, TRightSourceItem>(addedLeftSourceItem, rightSourceItem));					
                    }            

                    _isConsistent = true;
                    raiseConsistencyRestored();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    int oldIndex = e.OldStartingIndex;

                    int count1 = _rightSourceAsList.Count;
                    int baseIndex1 = oldIndex * count1;
                    for (int innerSourceIndex = count1 - 1; innerSourceIndex >= 0; innerSourceIndex--)
                    {
                        unregisterSourceItem(baseIndex1 + innerSourceIndex);										
                    }        
                    break;
                case NotifyCollectionChangedAction.Move:
                    _isConsistent = false;
                    int newIndex1 = e.NewStartingIndex;
                    int oldIndex1 = e.OldStartingIndex;
                    if (newIndex1 != oldIndex1)
                    {
                        int count3 = _rightSourceAsList.Count;
                        int oldResultIndex = oldIndex1 * count3;
                        int newBaseIndex = newIndex1 * count3;
                        if (oldIndex1 < newIndex1)
                        {
                            for (int index = 0; index < count3; index++)
                            {
                                FilteringUtils.ProcessMoveSourceItems(oldResultIndex, newBaseIndex + count3 - 1, _itemInfos, _filteredPositions, _sourcePositions, this);
                            }						
                        }
                        else
                        {
                            for (int index = 0; index < count3; index++)
                            {
                                FilteringUtils.ProcessMoveSourceItems(oldResultIndex + index, newBaseIndex + index, _itemInfos, _filteredPositions, _sourcePositions, this);
                            }						
                        }
                    }

                    _isConsistent = true;
                    raiseConsistencyRestored();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _isConsistent = false;
                    initializeFromSource();
                    _isConsistent = true;
                    raiseConsistencyRestored();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    _isConsistent = false;
                    int newIndex3 = e.NewStartingIndex;
                    TLeftSourceItem newLeftSourceItem1 = _leftSourceAsList[newIndex3];

                    int count2 = _rightSourceAsList.Count;
                    int baseIndex2 = newIndex3 * count2;
                    for (int rightSourceIndex = 0; rightSourceIndex < count2; rightSourceIndex++)
                    {
                        TRightSourceItem rightSourceItem = _rightSourceAsList[rightSourceIndex];
                        FilteringItemInfo replacingItemInfo = _itemInfos[baseIndex2 + rightSourceIndex];

                        var replace = FilteringUtils.ProcessReplaceSourceItem(replacingItemInfo, new object[]{newLeftSourceItem1, rightSourceItem}, baseIndex2 + rightSourceIndex, _predicateContainsParametrizedObservableComputationsCalls, _predicateExpression, ref _predicateExpressionСallCount, _predicateExpressionInfo, _itemInfos, _filteredPositions, _thisAsFiltering, this);

                        if (replace)
                            this[baseIndex2 + rightSourceIndex].setLeftItem(newLeftSourceItem1);	                   										
                    }
                    _isConsistent = true;
                    raiseConsistencyRestored();
                    break;
            }

            Utils.doDeferredExpressionWatcherChangedProcessings(
                _deferredExpressionWatcherChangedProcessings,
                ref _handledEventSender,
                ref _handledEventArgs,
                this,
                ref _isConsistent);

            Utils.postHandleSourceCollectionChanged(
                ref _handledEventSender,
                ref _handledEventArgs);
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        void IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>>.expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender,
            EventArgs eventArgs)
        {
            Utils.ProcessSourceItemChange(
                expressionWatcher,
                sender,
                eventArgs,
                _rootSourceWrapper,
                _sourceAsList,
                _lastProcessedSourceChangeMarker,
                _thisAsFiltering,
                ref _deferredExpressionWatcherChangedProcessings,
                ref _isConsistent,
                ref _handledEventSender,
                ref _handledEventArgs,
                _isConsistent);
        }

        private void modifyNextFilteredItemIndex(int sourceIndex, Position nextItemPosition)
        {
            for (int previousSourceIndex = sourceIndex - 1; previousSourceIndex >= 0; previousSourceIndex--)
            {
                FilteringItemInfo itemInfo = _itemInfos[previousSourceIndex];
                itemInfo.NextFilteredItemPosition = nextItemPosition;
                if (itemInfo.FilteredPosition != null) break;
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        bool IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>>.ApplyPredicate(int sourceIndex)
        {
            int count = _leftSourceAsList.Count;
            return ApplyPredicate(sourceIndex / count, sourceIndex % count);
        }

        JoinPair<TLeftSourceItem, TRightSourceItem> IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>>.GetItem(int sourceIndex)
        {
            int count = _leftSourceAsList.Count;
            return new JoinPair<TLeftSourceItem, TRightSourceItem>(_leftSourceAsList[sourceIndex / count], _rightSourceAsList[sourceIndex % count]);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public bool ApplyPredicate(int leftSourceIndex, int rightSourceIndex)
        {
            bool getValue() =>
                _predicateContainsParametrizedObservableComputationsCalls
                    ? _itemInfos[leftSourceIndex + rightSourceIndex].PredicateFunc()
                    : _predicateFunc(_leftSourceAsList[leftSourceIndex], _rightSourceAsList[rightSourceIndex]);

            if (Configuration.TrackComputingsExecutingUserCode)
            {
                var currentThread =
                    Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
                bool result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
                return result;
            }

            return getValue();
        }

        private bool applyPredicate(TLeftSourceItem leftSourceItem, TRightSourceItem rightSourceItem,Func<bool> itemPredicateFunc)
        {
            bool getValue() =>
                _predicateContainsParametrizedObservableComputationsCalls
                    ? itemPredicateFunc()
                    : _predicateFunc(leftSourceItem, rightSourceItem);

            if (Configuration.TrackComputingsExecutingUserCode)
            {
                var currentThread =
                    Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
                var result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
                return result;
            }

            return getValue();
        }

        private FilteringItemInfo registerSourceItem(TLeftSourceItem leftSourceItem, TRightSourceItem rightSourceItem, int sourceIndex, Position position,
            Position nextItemPosition, ExpressionWatcher watcher = null, Func<bool> predicateFunc = null,
            List<IComputingInternal> nestedComputings = null)
        {
            FilteringItemInfo itemInfo = _sourcePositions.Insert(sourceIndex);
            itemInfo.FilteredPosition = position;
            itemInfo.NextFilteredItemPosition = nextItemPosition;

            if (watcher == null /*&& predicateFunc == null*/)
                Utils.getItemInfoContent(
                    new object[]{leftSourceItem, rightSourceItem},
                    out watcher,
                    out predicateFunc,
                    out nestedComputings,
                    _predicateExpression,
                    ref _predicateExpressionСallCount,
                    this,
                    _predicateContainsParametrizedObservableComputationsCalls,
                    _predicateExpressionInfo);

            itemInfo.PredicateFunc = predicateFunc;
            watcher.ValueChanged = expressionWatcher_OnValueChanged;
            watcher._position = itemInfo;
            itemInfo.ExpressionWatcher = watcher;
            itemInfo.NestedComputings = nestedComputings;

            return itemInfo;
        }

        private void unregisterSourceItem(int sourceIndex)
        {
            int? removeIndex = null;
            FilteringItemInfo itemInfo = _itemInfos[sourceIndex];

            ExpressionWatcher watcher = itemInfo.ExpressionWatcher;
            watcher.Dispose();
            EventUnsubscriber.QueueSubscriptions(watcher._propertyChangedEventSubscriptions,
                watcher._methodChangedEventSubscriptions);

            Position itemInfoFilteredPosition = itemInfo.FilteredPosition;

            if (itemInfoFilteredPosition != null)
            {
                removeIndex = itemInfoFilteredPosition.Index;
                _filteredPositions.Remove(itemInfoFilteredPosition.Index);
                modifyNextFilteredItemIndex(sourceIndex, itemInfo.NextFilteredItemPosition);
            }

            _sourcePositions.Remove(sourceIndex);

            if (_predicateContainsParametrizedObservableComputationsCalls)
                Utils.itemInfoRemoveDownstreamConsumedComputing(itemInfo.NestedComputings, this);

            if (removeIndex.HasValue) baseRemoveItem(removeIndex.Value);
        }


        private bool processChangeSourceItem(int sourceIndex)
        {
            TSourceItem item = _sourceAsList[sourceIndex];
            FilteringItemInfo itemInfo = _itemInfos[sourceIndex];

            bool newPredicateValue = ApplyPredicate(sourceIndex);
            bool oldPredicateValue = itemInfo.FilteredPosition != null;

            if (newPredicateValue != oldPredicateValue)
            {
                if (newPredicateValue)
                {
                    int newIndex = itemInfo.NextFilteredItemPosition.Index;
                    itemInfo.FilteredPosition = _filteredPositions.Insert(newIndex);
                    newIndex = itemInfo.FilteredPosition.Index;
                    modifyNextFilteredItemIndex(sourceIndex, itemInfo.FilteredPosition);
                    baseInsertItem(newIndex, item);
                    return true;
                }
                else // if (oldPredicaeValue)
                {
                    int index = itemInfo.FilteredPosition.Index;
                    _filteredPositions.Remove(index);
                    itemInfo.FilteredPosition = null;
                    modifyNextFilteredItemIndex(sourceIndex, itemInfo.NextFilteredItemPosition);
                    baseRemoveItem(index);
                    return true;
                }
            }

            return false;
        }

        void ICanProcessSourceItemChange.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
        {
            processChangeSourceItem(expressionWatcher._position.Index);
        }
    }

	public class JoinPair<TLeftSourceItem, TRightSourceItem> : IEquatable<JoinPair<TLeftSourceItem, TRightSourceItem>>, INotifyPropertyChanged
	{
		public Action<JoinPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> JoinPairSetLeftItemAction
		{
			get => _joinPairSetLeftItemAction;
			set
			{
				if (_joinPairSetLeftItemAction != value)
				{
					_joinPairSetLeftItemAction = value;
					PropertyChanged?.Invoke(this, Utils.JoinPairSetLeftItemActionPropertyChangedEventArgs);
				}
			}
		}

		public Action<JoinPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> JoinPairSetRightItemAction
		{
			get => _joinPairSetRightItemAction;
			set
			{
				if (_joinPairSetRightItemAction != value)
				{
					_joinPairSetRightItemAction = value;
					PropertyChanged?.Invoke(this, Utils.JoinPairSetRightItemActionPropertyChangedEventArgs);
				}
			}
		}

		public TLeftSourceItem LeftItem
		{
			get => _leftItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set => _joinPairSetLeftItemAction(this, value);
		}

		public TRightSourceItem RightItem
		{
			get => _rightItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set => _joinPairSetRightItemAction(this, value);
		}

		readonly EqualityComparer<TLeftSourceItem> _leftSourceItemEqualityComparer = EqualityComparer<TLeftSourceItem>.Default;
		readonly EqualityComparer<TRightSourceItem> _rightSourceItemEqualityComparer = EqualityComparer<TRightSourceItem>.Default;

		private Action<JoinPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> _joinPairSetLeftItemAction;

		private Action<JoinPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> _joinPairSetRightItemAction;

		private TLeftSourceItem _leftItem;

		private TRightSourceItem _rightItem;

		public JoinPair(
			TLeftSourceItem leftItem, TRightSourceItem rightItem)
		{
			_leftItem = leftItem;
			_rightItem = rightItem;
		}

		internal void setRightItem(TRightSourceItem rightSourceItem)
		{
			_rightItem = rightSourceItem;
			PropertyChanged?.Invoke(this, Utils.RightItemPropertyChangedEventArgs);
		}

		internal void setLeftItem(TLeftSourceItem leftSourceItem)
		{
			_leftItem = leftSourceItem;
			PropertyChanged?.Invoke(this, Utils.LeftItemPropertyChangedEventArgs);
		}

		public override int GetHashCode()
		{
			return _leftSourceItemEqualityComparer.GetHashCode(LeftItem) +_rightSourceItemEqualityComparer.GetHashCode(RightItem);
		}

		public override bool Equals(object obj)
		{
			return obj is JoinPair<TLeftSourceItem, TRightSourceItem> other && Equals(other);
		}

		public bool Equals(JoinPair<TLeftSourceItem, TRightSourceItem> other)
		{
			return other != null && (_leftSourceItemEqualityComparer.Equals(LeftItem, other.LeftItem) && _rightSourceItemEqualityComparer.Equals(RightItem, other.RightItem));
		}

		public override string ToString()
		{
			return $"JoinPair: LeftItem = {(LeftItem != null ? $"{LeftItem.ToString()}" : "null")}    RightItem = {(RightItem != null ? $"{RightItem.ToString()}" : "null")}";
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
