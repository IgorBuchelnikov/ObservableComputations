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
    public class
        Joining<TLeftSourceItem, TRightSourceItem> : CollectionComputing<JoinPair<TLeftSourceItem, TRightSourceItem>>,
            IHasSourceCollections, IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>>
    {
        public IReadScalar<INotifyCollectionChanged> LeftSourceScalar => _leftSourceScalar;

        // ReSharper disable once MemberCanBePrivate.Global
        public IReadScalar<INotifyCollectionChanged> RightSourceScalar => _rightSourceScalar;

        // ReSharper disable once MemberCanBePrivate.Global
        public INotifyCollectionChanged LeftSource => _leftSource;

        // ReSharper disable once MemberCanBePrivate.Global
        public INotifyCollectionChanged RightSource => _rightSource;

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> JoinPredicateExpression { get; }

        public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections =>
            new ReadOnlyCollection<INotifyCollectionChanged>(new[] {LeftSource, RightSource});

        public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars =>
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

        private bool _sourceInitialized;

        private ObservableCollectionWithChangeMarker<TLeftSourceItem> _leftSourceAsList;
        private ObservableCollectionWithChangeMarker<TRightSourceItem> _rightSourceAsList;
        private List<TLeftSourceItem> _leftSourceCopy;
        private List<TRightSourceItem> _rightSourceCopy;
        bool _rootLeftSourceWrapper;
        bool _rootRightSourceWrapper;
        private bool _lastProcessedLeftSourceChangeMarker;
        private bool _lastProcessedRightSourceChangeMarker;

        private readonly bool _predicateContainsParametrizedObservableComputationsCalls;

        private ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
        private IReadScalar<INotifyCollectionChanged> _leftSourceScalar;
        internal INotifyCollectionChanged _leftSource;
        private readonly IReadScalar<INotifyCollectionChanged> _rightSourceScalar;
        internal INotifyCollectionChanged _rightSource;

        private readonly Func<TLeftSourceItem, TRightSourceItem, bool> _predicateFunc;
        private IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>> _thisAsFiltering;
        internal Action<JoinPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> _setLeftItemRequestHandler;
        internal Action<JoinPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> _setRightItemRequestHandler;

        private ISourceItemChangeProcessor _thisAsSourceItemChangeProcessor;
        //private bool _isLeft;
        //private TRightSourceItem _defaultRightSourceItemForLeftJoin;

        //private List<LeftItemInfo> _leftItemInfos;
        //private sealed class LeftItemInfo
        //{
        //    public bool Defaulted; // for left join

        //    public LeftItemInfo(bool defaulted)
        //    {
        //        Defaulted = defaulted;
        //    }
        //}

        public Action<JoinPair<TLeftSourceItem, TRightSourceItem>, TLeftSourceItem> SetLeftItemRequestHandler
        {
            get => _setLeftItemRequestHandler;
            set
            {
                if (_setLeftItemRequestHandler != value)
                {
                    _setLeftItemRequestHandler = value;
                    OnPropertyChanged(Utils.SetLeftItemRequestHandlerPropertyChangedEventArgs);
                }
            }
        }

        public Action<JoinPair<TLeftSourceItem, TRightSourceItem>, TRightSourceItem> SetRightItemAction
        {
            get => _setRightItemRequestHandler;
            set
            {
                if (_setRightItemRequestHandler != value)
                {
                    _setRightItemRequestHandler = value;
                    OnPropertyChanged(Utils.SetRightItemRequestHandlerPropertyChangedEventArgs);
                }
            }
        }

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

        //[ObservableComputationsCall]
        //public Joining(
        //    IReadScalar<INotifyCollectionChanged> leftSourceScalar,
        //    IReadScalar<INotifyCollectionChanged> rightSourceScalar,
        //    Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
        //    TRightSourceItem defaultRightSourceItemForLeftJoin,
        //    int capacity = 0) : this(predicateExpression, Utils.getCapacity(leftSourceScalar) * Utils.getCapacity(rightSourceScalar), capacity)
        //{
        //    _leftSourceScalar = leftSourceScalar;
        //    _rightSourceScalar = rightSourceScalar;
        //    _isLeft = true;
        //    _defaultRightSourceItemForLeftJoin = defaultRightSourceItemForLeftJoin;
        //    _leftItemInfos = new List<LeftItemInfo>(Utils.getCapacity(leftSourceScalar));
        //}

        //[ObservableComputationsCall]
        //public Joining(
        //    INotifyCollectionChanged leftSource,
        //    IReadScalar<INotifyCollectionChanged> rightSourceScalar,
        //    Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
        //    TRightSourceItem defaultRightSourceItemForLeftJoin,
        //    int capacity = 0) : this(predicateExpression, Utils.getCapacity(leftSource) * Utils.getCapacity(rightSourceScalar), capacity)
        //{
        //    _leftSource = leftSource;
        //    _rightSourceScalar = rightSourceScalar;
        //    _isLeft = true;
        //    _defaultRightSourceItemForLeftJoin = defaultRightSourceItemForLeftJoin;
        //    _leftItemInfos = new List<LeftItemInfo>( Utils.getCapacity(leftSource));
        //}

        //[ObservableComputationsCall]
        //public Joining(
        //    IReadScalar<INotifyCollectionChanged> leftSourceScalar,
        //    INotifyCollectionChanged rightSource,
        //    Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
        //    TRightSourceItem defaultRightSourceItemForLeftJoin,
        //    int capacity = 0) : this(predicateExpression, Utils.getCapacity(leftSourceScalar) * Utils.getCapacity(rightSource), capacity)
        //{
        //    _leftSourceScalar = leftSourceScalar;
        //    _rightSource = rightSource;
        //    _isLeft = true;
        //    _defaultRightSourceItemForLeftJoin = defaultRightSourceItemForLeftJoin;
        //    _leftItemInfos = new List<LeftItemInfo>(Utils.getCapacity(leftSourceScalar));
        //}

        //[ObservableComputationsCall]
        //public Joining(
        //    INotifyCollectionChanged leftSource,
        //    INotifyCollectionChanged rightSource,
        //    Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
        //    TRightSourceItem defaultRightSourceItemForLeftJoin,
        //    int capacity = 0) : this(predicateExpression, Utils.getCapacity(leftSource) * Utils.getCapacity(rightSource), capacity)
        //{
        //    _leftSource = leftSource;
        //    _rightSource = rightSource;
        //    _isLeft = true;
        //    _defaultRightSourceItemForLeftJoin = defaultRightSourceItemForLeftJoin;
        //    _leftItemInfos = new List<LeftItemInfo>( Utils.getCapacity(leftSource));
        //}

        private Joining(Expression<Func<TLeftSourceItem, TRightSourceItem, bool>> predicateExpression,
            int sourceCapacity, int capacity) : base(capacity)
        {
            Utils.construct(
                predicateExpression,
                sourceCapacity,
                out _itemInfos,
                out _sourcePositions,
                out _predicateExpressionOriginal,
                out _predicateExpression,
                out _predicateContainsParametrizedObservableComputationsCalls,
                ref _predicateExpressionInfo,
                ref _predicateExpressionСallCount,
                ref _predicateFunc,
                ref _nestedComputings);

            _thisAsFiltering = this;
            _thisAsSourceCollectionChangeProcessor = this;
            _thisAsSourceItemChangeProcessor = this;

            _initialCapacity = capacity;
            _filteredPositions = new Positions<Position>(new List<Position>(_initialCapacity));

            _deferredQueuesCount = 3;
        }

        protected override void initializeFromSource()
        {
            int originalCount = _items.Count;

            if (_sourceInitialized)
            {
                Utils.disposeExpressionItemInfos(_itemInfos, _predicateExpressionСallCount, this);
                Utils.RemoveDownstreamConsumedComputing(_itemInfos, this);

                //int leftCapacity = Utils.getCapacity(_leftSourceScalar, _leftSource);
                Utils.initializeItemInfos(
                    Utils.getCapacity(_leftSourceScalar, _leftSource) * Utils.getCapacity(_rightSourceScalar, _rightSource),
                    out _itemInfos,
                    out _sourcePositions);

                _filteredPositions = new Positions<Position>(new List<Position>(_initialCapacity));
                //if (_isLeft) _leftItemInfos = new List<LeftItemInfo>(leftCapacity);

                Utils.unsubscribeSource(
                    _leftSourceAsList,
                    handleLeftSourceCollectionChanged);

                Utils.unsubscribeSource(
                    _rightSourceAsList,
                    handleRightSourceCollectionChanged);

                _leftSourceCopy = null;
                _rightSourceCopy = null;
                _sourceInitialized = false;
            }

            Utils.changeSource(ref _leftSource, _leftSourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _leftSourceAsList, false);

            Utils.changeSource(ref _rightSource, _rightSourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _rightSourceAsList, false);

            if (_leftSource != null && _rightSource != null && _isActive)
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
                int rightCount = _rightSourceAsList.Count;

                _leftSourceCopy = new List<TLeftSourceItem>(_leftSourceAsList);
                _rightSourceCopy = new List<TRightSourceItem>(_rightSourceAsList);

                _leftSourceAsList.CollectionChanged += handleLeftSourceCollectionChanged;
               _rightSourceAsList.CollectionChanged += handleRightSourceCollectionChanged;

                int insertingIndex = 0;
                int leftSourceIndex;
                int rightSourceIndex;

                for (leftSourceIndex = 0; leftSourceIndex < leftCount; leftSourceIndex++)
                {
                    TLeftSourceItem leftSourceItem = _leftSourceCopy[leftSourceIndex];

                    //bool anyAdded = false;

                    int doRegisterSourceItem(TRightSourceItem rightSourceItem)
                    {
                        Position currentFilteredItemPosition = null;

                        FilteringItemInfo itemInfo =
                            registerSourceItem(
                                leftSourceItem, 
                                rightSourceItem, 
                                leftSourceIndex * rightCount + rightSourceIndex, 
                                null, null);

                        if (ApplyPredicate(leftSourceIndex, rightSourceIndex, rightCount))
                        {
                            JoinPair<TLeftSourceItem, TRightSourceItem> joinPair
                                = new JoinPair<TLeftSourceItem, TRightSourceItem>(leftSourceItem, rightSourceItem, this);

                            if (originalCount > insertingIndex)
                                _items[insertingIndex++] = joinPair;
                            else
                                _items.Insert(insertingIndex++, joinPair);

                            currentFilteredItemPosition = nextItemPosition;
                            nextItemPosition = _filteredPositions.Add();
                            //anyAdded = true;
                        }

                        itemInfo.FilteredPosition = currentFilteredItemPosition;
                        itemInfo.NextFilteredItemPosition = nextItemPosition;
                        return insertingIndex;
                    }

                    for (rightSourceIndex = 0; rightSourceIndex < rightCount; rightSourceIndex++)
                    {
                        insertingIndex = doRegisterSourceItem(
                            // ReSharper disable once PossibleNullReferenceException
                            _rightSourceCopy[rightSourceIndex]);
                    }

                    //if (_isLeft)
                    //{
                    //    if (!anyAdded)
                    //    {
                    //        doRegisterSourceItem(_defaultRightSourceItemForLeftJoin, true);
                    //        _leftItemInfos.Add(new LeftItemInfo(true));
                    //    }

                    //    _leftItemInfos.Add(new LeftItemInfo(true));
                    //}
                }

                for (int index = originalCount - 1; index >= insertingIndex; index--)
                {
                    _items.RemoveAt(index);
                }

                _sourceInitialized = true;

            }
            else
            {
                _items.Clear();
            }

            reset();
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_leftSourceScalar, ref _leftSource,
                scalarValueChangedHandler);
            Utils.initializeSourceScalar(_rightSourceScalar, ref _rightSource,
                scalarValueChangedHandler);
            Utils.initializeNestedComputings(_nestedComputings, this);
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_leftSourceScalar, scalarValueChangedHandler, ref _leftSource);
            Utils.uninitializeSourceScalar(_rightSourceScalar, scalarValueChangedHandler, ref _rightSource);
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
            if (ReferenceEquals(sender, _leftSourceAsList))
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:

                        int newLeftSourceIndex = e.NewStartingIndex;
                        TLeftSourceItem addedLeftSourceItem = (TLeftSourceItem) e.NewItems[0];
                        _leftSourceCopy.Insert(newLeftSourceIndex, addedLeftSourceItem);
                        int count = _rightSourceCopy.Count;
                        int baseIndex = newLeftSourceIndex * count;

                        //bool anyAdded = false;
                        for (int rightSourceIndex = 0; rightSourceIndex < count; rightSourceIndex++)
                        {
                            TRightSourceItem rightSourceItem = _rightSourceCopy[rightSourceIndex];
                            processAddSourceItem(addedLeftSourceItem, rightSourceItem, baseIndex + rightSourceIndex);
                            //if (processAddSourceItem(addedLeftSourceItem, rightSourceItem, baseIndex + rightSourceIndex))
                            //    anyAdded = true;
                        }

                        //if (_isLeft && !anyAdded)
                        //    processAddSourceItem(addedLeftSourceItem, _defaultRightSourceItemForLeftJoin, baseIndex);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        int oldIndex = e.OldStartingIndex;
                        _leftSourceCopy.RemoveAt(oldIndex);
                        int count1 = _rightSourceCopy.Count;
                        int baseIndex1 = oldIndex * count1;

                        //if (_isLeft)
                        //{
                        //    LeftItemInfo leftItemInfo = _leftItemInfos[oldIndex];
                        //    if (leftItemInfo.Defaulted)  unregisterSourceItem(baseIndex1);
                        //    _leftItemInfos.RemoveAt(oldIndex);
                        //}

                        for (int innerSourceIndex = count1 - 1; innerSourceIndex >= 0; innerSourceIndex--)
                            unregisterSourceItem(baseIndex1 + innerSourceIndex);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        int newIndex1 = e.NewStartingIndex;
                        int oldIndex1 = e.OldStartingIndex;
                        if (newIndex1 != oldIndex1)
                        {
                            TLeftSourceItem leftSourceItem = (TLeftSourceItem) e.NewItems[0];
                            _leftSourceCopy.RemoveAt(oldIndex1);
                            _leftSourceCopy.Insert(newIndex1, leftSourceItem);
                            //    LeftItemInfo leftItemInfo = _leftItemInfos[oldIndex1];
                            //    _leftItemInfos.RemoveAt(oldIndex1);
                            //    _leftItemInfos.Insert(newIndex1, leftItemInfo);

                            int count3 = _rightSourceCopy.Count;
                            int oldResultIndex = oldIndex1 * count3;
                            int newBaseIndex = newIndex1 * count3;
                            if (oldIndex1 < newIndex1)
                            {
                                for (int index = 0; index < count3; index++)
                                {
                                    FilteringUtils.ProcessMoveSourceItems(oldResultIndex, newBaseIndex + count3 - 1, _itemInfos,
                                        _filteredPositions, _sourcePositions, this);
                                }
                            }
                            else
                            {
                                for (int index = 0; index < count3; index++)
                                {
                                    FilteringUtils.ProcessMoveSourceItems(oldResultIndex + index, newBaseIndex + index, _itemInfos,
                                        _filteredPositions, _sourcePositions, this);
                                }
                            }
                        }

                        break;
                    case NotifyCollectionChangedAction.Reset:
                        initializeFromSource();
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        int newIndex3 = e.NewStartingIndex;
                        TLeftSourceItem newLeftSourceItem1 = (TLeftSourceItem) e.NewItems[0];
                        _leftSourceCopy[newIndex3] = newLeftSourceItem1;

                        int count2 = _rightSourceCopy.Count;
                        int baseIndex2 = newIndex3 * count2;
                        for (int rightSourceIndex = 0; rightSourceIndex < count2; rightSourceIndex++)
                        {
                            TRightSourceItem rightSourceItem = _rightSourceCopy[rightSourceIndex];
                            FilteringItemInfo replacingItemInfo = _itemInfos[baseIndex2 + rightSourceIndex];

                            bool replace = FilteringUtils.ProcessReplaceSourceItem(
                                replacingItemInfo,
                                new JoinPair<TLeftSourceItem, TRightSourceItem>(newLeftSourceItem1, rightSourceItem, this),
                                new object[] {newLeftSourceItem1, rightSourceItem},
                                baseIndex2 + rightSourceIndex,
                                _predicateContainsParametrizedObservableComputationsCalls,
                                _predicateExpression,
                                out _predicateExpressionСallCount,
                                _predicateExpressionInfo,
                                _itemInfos,
                                _filteredPositions,
                                _thisAsFiltering,
                                this);

                            if (replace)
                                this[replacingItemInfo.FilteredPosition.Index].setLeftItem(newLeftSourceItem1);
                        }

                        break;
                }
            else //if (ReferenceEquals(sender, _rightSourceAsList))
			    switch (e.Action)
			    {
				    case NotifyCollectionChangedAction.Add:
					    //if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
					    int newIndex = e.NewStartingIndex;
					    TRightSourceItem sourceRightItem = (TRightSourceItem) e.NewItems[0];
                        _rightSourceCopy.Insert(newIndex, sourceRightItem);
	    
					    int index1 = newIndex;
					    int outerSourceCount1 = _leftSourceCopy.Count;
					    int innerSourceCount1 = _rightSourceCopy.Count;
					    for (int leftSourceIndex = 0; leftSourceIndex < outerSourceCount1; leftSourceIndex++)
					    {
						    TLeftSourceItem sourceLeftItem = _leftSourceCopy[leftSourceIndex];
                            processAddSourceItem(sourceLeftItem, sourceRightItem, index1);
						    index1 = index1 + innerSourceCount1;
					    }
					    break;
				    case NotifyCollectionChangedAction.Remove:
					    //if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
					    int oldIndex = e.OldStartingIndex;
                        _rightSourceCopy.RemoveAt(oldIndex);
					    int leftSourceCount3 = _leftSourceCopy.Count;
					    int oldRightSourceCount = _rightSourceCopy.Count + 1;
					    int baseIndex = (leftSourceCount3 - 1) * oldRightSourceCount;
					    for (int outerSourceIndex =  leftSourceCount3 - 1; outerSourceIndex >= 0; outerSourceIndex--)
					    {
                            unregisterSourceItem(baseIndex + oldIndex);
						    baseIndex = baseIndex - oldRightSourceCount;
					    }	
					    break;
				    case NotifyCollectionChangedAction.Replace:
					    //if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
					    TRightSourceItem sourceRightItem3 = (TRightSourceItem) e.NewItems[0];
					    newIndex = e.NewStartingIndex;
                        _rightSourceCopy[newIndex] = sourceRightItem3;
					    int index2 = newIndex;
					    int leftSourceCount2 = _leftSourceCopy.Count;
					    int rightSourceCount2 = _rightSourceCopy.Count;
					    for (int leftSourceIndex = 0; leftSourceIndex < leftSourceCount2; leftSourceIndex++)
					    {
                            TLeftSourceItem leftSourceItem = _leftSourceCopy[leftSourceIndex];
                            FilteringItemInfo replacingItemInfo = _itemInfos[index2];

                            var replace = FilteringUtils.ProcessReplaceSourceItem(
                                replacingItemInfo, 
                                new JoinPair<TLeftSourceItem, TRightSourceItem>(leftSourceItem, sourceRightItem3, this), 
                                new object[]{leftSourceItem, sourceRightItem3}, 
                                index2, 
                                _predicateContainsParametrizedObservableComputationsCalls, 
                                _predicateExpression, 
                                out _predicateExpressionСallCount, 
                                _predicateExpressionInfo, 
                                _itemInfos, 
                                _filteredPositions, 
                                _thisAsFiltering, 
                                this);

                            if (replace)
                                this[replacingItemInfo.FilteredPosition.Index].setRightItem(sourceRightItem3);	                        

						    index2 = index2 + rightSourceCount2;
					    }
					    break;
				    case NotifyCollectionChangedAction.Move:
					    newIndex = e.NewStartingIndex;
					    oldIndex = e.OldStartingIndex;

					    if (newIndex != oldIndex)
					    {
                            TRightSourceItem rightSourceItem = (TRightSourceItem) e.NewItems[0];
                            _rightSourceCopy.RemoveAt(oldIndex);
                            _rightSourceCopy.Insert(newIndex, rightSourceItem);

						    int index = 0;
						    int leftSourceCount = _leftSourceCopy.Count;
						    int rightSourceCount = _rightSourceCopy.Count;
						    for (int leftSourceIndex = 0; leftSourceIndex < leftSourceCount; leftSourceIndex++)
						    {
				    
                                FilteringUtils.ProcessMoveSourceItems(index + oldIndex, index + newIndex, _itemInfos, _filteredPositions, _sourcePositions, this);
							    index = index + rightSourceCount;
						    }
					    }
					    break;
				    case NotifyCollectionChangedAction.Reset:
                        initializeFromSource();
					    break;
			    }
        }

        private /*bool*/ void processAddSourceItem(TLeftSourceItem leftSourceItem, TRightSourceItem rightSourceItem, int newSourceIndex)
        {
            Position newItemPosition = null;
            Position nextItemPosition;

            int? newFilteredIndex = null;

            Utils.getItemInfoContent(
                new object[] {leftSourceItem, rightSourceItem},
                out ExpressionWatcher newWatcher,
                out Func<bool> newPredicateFunc,
                out List<IComputingInternal> nestedComputings,
                _predicateExpression,
                out _predicateExpressionСallCount,
                this,
                _predicateContainsParametrizedObservableComputationsCalls,
                _predicateExpressionInfo);

            bool predicateValue = applyPredicate(leftSourceItem, rightSourceItem, newPredicateFunc);
            nextItemPosition = FilteringUtils.processAddSourceItem(newSourceIndex, predicateValue, ref newItemPosition,
                ref newFilteredIndex, _itemInfos, _filteredPositions, Count);

            registerSourceItem(leftSourceItem, rightSourceItem, newSourceIndex, newItemPosition, nextItemPosition,
                newWatcher, newPredicateFunc, nestedComputings);

            if (newFilteredIndex != null)
            {
                baseInsertItem(newFilteredIndex.Value,
                    new JoinPair<TLeftSourceItem, TRightSourceItem>(leftSourceItem, rightSourceItem, this));

                //return true;
            }

            //return false;
        }

        private void handleRightSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
            if (!Utils.preHandleSourceCollectionChanged(
                sender, 
                e, 
                _rootRightSourceWrapper, 
                ref _lastProcessedRightSourceChangeMarker, 
                _rightSourceAsList, 
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

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_leftSource as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_rightSource as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_leftSourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_rightSourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)
        {
            (_leftSource as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_rightSource as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_leftSourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_rightSourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        void IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>>.expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender,
            EventArgs eventArgs)
        {
            Utils.ProcessSourceItemChange(
                expressionWatcher, 
                sender, 
                eventArgs, 
                _rootLeftSourceWrapper,
                _leftSourceAsList,
                _rootRightSourceWrapper,
                _rightSourceAsList,
                _lastProcessedLeftSourceChangeMarker,
                _lastProcessedRightSourceChangeMarker,
                _thisAsSourceItemChangeProcessor,
                ref _isConsistent,
                ref _handledEventSender,
                ref _handledEventArgs,
                ref _deferredProcessings, 
                2, _deferredQueuesCount, this);
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
        bool IFiltering<JoinPair<TLeftSourceItem, TRightSourceItem>>.applyPredicate(JoinPair<TLeftSourceItem, TRightSourceItem> sourceItem, Func<bool> itemPredicateFunc)
        {
            return  applyPredicate(sourceItem._leftItem, sourceItem._rightItem, itemPredicateFunc);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public bool ApplyPredicate(int leftSourceIndex, int rightSourceIndex, int rightCount)
        {
            bool getValue() =>
                _predicateContainsParametrizedObservableComputationsCalls
                    ? _itemInfos[leftSourceIndex * rightCount + rightSourceIndex].PredicateFunc()
                    : _predicateFunc(_leftSourceAsList[leftSourceIndex], _rightSourceAsList[rightSourceIndex]);

            if (Configuration.TrackComputingsExecutingUserCode)
            {
                var currentThread =
                    Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
                bool result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
                return result;
            }

            return getValue();
        }

        private bool applyPredicate(TLeftSourceItem leftSourceItem, TRightSourceItem rightSourceItem, Func<bool> itemPredicateFunc)
        {
            bool getValue() =>
                _predicateContainsParametrizedObservableComputationsCalls
                    ? itemPredicateFunc()
                    : _predicateFunc(leftSourceItem, rightSourceItem);

            if (Configuration.TrackComputingsExecutingUserCode)
            {
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
                var result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
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
                    out _predicateExpressionСallCount,
                    this,
                    _predicateContainsParametrizedObservableComputationsCalls,
                    _predicateExpressionInfo);

            itemInfo.PredicateFunc = predicateFunc;
            watcher.ValueChanged = _thisAsFiltering.expressionWatcher_OnValueChanged;
            watcher._position = itemInfo;
            itemInfo.ExpressionWatcher = watcher;
            itemInfo.NestedComputings = nestedComputings;

            return itemInfo;
        }

        private void unregisterSourceItem(int sourceIndex)
        {
            int? removeIndex = null;
            FilteringItemInfo itemInfo = _itemInfos[sourceIndex];

            Utils.disposeExpressionWatcher(itemInfo.ExpressionWatcher, itemInfo.NestedComputings, this, _predicateContainsParametrizedObservableComputationsCalls);

            Position itemInfoFilteredPosition = itemInfo.FilteredPosition;

            if (itemInfoFilteredPosition != null)
            {
                removeIndex = itemInfoFilteredPosition.Index;
                _filteredPositions.Remove(itemInfoFilteredPosition.Index);
                modifyNextFilteredItemIndex(sourceIndex, itemInfo.NextFilteredItemPosition);
            }

            _sourcePositions.Remove(sourceIndex);

            if (removeIndex.HasValue) baseRemoveItem(removeIndex.Value);
        }

        void ISourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
        {
            int sourceIndex = expressionWatcher._position.Index;
            int rightCount = _rightSourceAsList.Count;

            FilteringUtils.ProcessChangeSourceItem(
                sourceIndex, 
                new JoinPair<TLeftSourceItem, TRightSourceItem>(
                    _leftSourceAsList[sourceIndex / rightCount],
                    _rightSourceAsList[sourceIndex % rightCount],
                    this), 
                _itemInfos, 
                this,
                _filteredPositions, 
                this,
                expressionWatcher);
        }

		internal void ValidateConsistency()
		{
			_filteredPositions.ValidateConsistency();
			_sourcePositions.ValidateConsistency();

			IList<TLeftSourceItem> leftSource = _leftSourceScalar.getValue(_leftSource, new ObservableCollection<TLeftSourceItem>()) as IList<TLeftSourceItem>;
            IList<TRightSourceItem> rightSource = _rightSourceScalar.getValue(_rightSource, new ObservableCollection<TRightSourceItem>()) as IList<TRightSourceItem>;
		
			// ReSharper disable once PossibleNullReferenceException

            if (rightSource == null || 
                (_itemInfos.Count != leftSource.Count * rightSource.Count)) throw new ObservableComputationsException(this, "Consistency violation: Joining.9");
			Func<TLeftSourceItem, TRightSourceItem, bool> predicate = _predicateExpression.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (_leftSource != null && _rightSource != null)
			{
				if (_leftSource != null && _rightSource != null && _filteredPositions.List.Count - 1 != Count)
					throw new ObservableComputationsException(this, "Consistency violation: Joining.15");

				if ((_leftSource == null || _rightSource == null) && _filteredPositions.List.Count != 0)
					throw new ObservableComputationsException(this, "Consistency violation: Joining.16");

				int index = 0;
				for (int leftSourceIndex = 0; leftSourceIndex < leftSource.Count; leftSourceIndex++)
				{
					TLeftSourceItem leftSourceItem = leftSource[leftSourceIndex];


                    for (int rightSourceIndex = 0; rightSourceIndex < rightSource.Count; rightSourceIndex++)
                    {
                        TRightSourceItem rightSourceItem = rightSource[rightSourceIndex];
                        var internalIndex = leftSourceIndex * rightSource.Count + rightSourceIndex;
                        FilteringItemInfo itemInfo = _itemInfos[internalIndex];
                        if (predicate(leftSourceItem, rightSourceItem))
                        {
                            if (itemInfo.FilteredPosition == null) throw new ObservableComputationsException(this, "Consistency violation: Joining.2");

                            if (!Equals(this[index].LeftItem, leftSourceItem) || !Equals(this[index].RightItem, rightSourceItem))
                            {
                                throw new ObservableComputationsException(this, "Consistency violation: Joining.1");
                            }

                            if (itemInfo.FilteredPosition.Index != index) throw new ObservableComputationsException(this, "Consistency violation: Joining.5");
						
                            index++;
                        }
                        else
                        {
                            if (itemInfo.FilteredPosition != null) throw new ObservableComputationsException(this, "Consistency violation: Joining.3");
                        }

                        if (_itemInfos[internalIndex].Index != internalIndex) throw new ObservableComputationsException(this, "Consistency violation: Joining.7");
                        if (itemInfo.ExpressionWatcher._position != _itemInfos[internalIndex]) throw new ObservableComputationsException(this, "Consistency violation: Joining.8");

                        if (itemInfo.FilteredPosition != null && !_filteredPositions.List.Contains(itemInfo.FilteredPosition))
                            throw new ObservableComputationsException(this, "Consistency violation: Joining.10");

                        if (!_filteredPositions.List.Contains(itemInfo.NextFilteredItemPosition))
                            throw new ObservableComputationsException(this, "Consistency violation: Joining.11");

                        if (!_itemInfos.Contains(itemInfo.ExpressionWatcher._position))
                            throw new ObservableComputationsException(this, "Consistency violation: Joining.12");
                    }
				}

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (_leftSource != null && _rightSource != null)
				{
					int count = leftSource.SelectMany(li => rightSource.Select(ri => new {li, ri})).Where(p => predicate(p.li, p.ri)).Count();

					if (_filteredPositions.List.Count != count + 1)
					{
						throw new ObservableComputationsException(this, "Consistency violation: Joining.6");
					}
                    
					Position nextFilteredItemPosition;
					nextFilteredItemPosition = _filteredPositions.List[count];
                    for (int leftSourceIndex = leftSource.Count - 1; leftSourceIndex >= 0; leftSourceIndex--)
                    {
                        TLeftSourceItem leftSourceItem = leftSource[leftSourceIndex];


                        for (int rightSourceIndex = rightSource.Count - 1; rightSourceIndex >= 0; rightSourceIndex--)
                        {
                            TRightSourceItem rightSourceItem = rightSource[rightSourceIndex];
                            var internalIndex = leftSourceIndex * rightSource.Count + rightSourceIndex;
                            FilteringItemInfo itemInfo = _itemInfos[internalIndex];

                            if (itemInfo.NextFilteredItemPosition != nextFilteredItemPosition)
                                throw new ObservableComputationsException(this, "Consistency violation: Joining.4");

                            if (predicate(leftSourceItem, rightSourceItem))
                            {
                                nextFilteredItemPosition = itemInfo.FilteredPosition;
                            }
                        }
                    }
				}
			}
		}
    }

	public class JoinPair<TLeftSourceItem, TRightSourceItem> : IEquatable<JoinPair<TLeftSourceItem, TRightSourceItem>>, INotifyPropertyChanged
	{
		public TLeftSourceItem LeftItem
		{
			get => _leftItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set => _joining._setLeftItemRequestHandler(this, value);
		}

		public TRightSourceItem RightItem
		{
			get => _rightItem;
			// ReSharper disable once MemberCanBePrivate.Global
			set =>_joining._setRightItemRequestHandler(this, value);
		}

		readonly EqualityComparer<TLeftSourceItem> _leftSourceItemEqualityComparer = EqualityComparer<TLeftSourceItem>.Default;
		readonly EqualityComparer<TRightSourceItem> _rightSourceItemEqualityComparer = EqualityComparer<TRightSourceItem>.Default;

		internal TLeftSourceItem _leftItem;

		internal TRightSourceItem _rightItem;
        private Joining<TLeftSourceItem, TRightSourceItem> _joining;

        // ReSharper disable once UnusedMember.Local
        private Joining<TLeftSourceItem, TRightSourceItem> Joining => _joining;

		public JoinPair(
			TLeftSourceItem leftItem, TRightSourceItem rightItem,
            Joining<TLeftSourceItem, TRightSourceItem> joining)
		{
			_leftItem = leftItem;
			_rightItem = rightItem;
            _joining = joining;
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
