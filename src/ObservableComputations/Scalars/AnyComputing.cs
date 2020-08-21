using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class AnyComputing<TSourceItem> : ScalarComputing<bool>, IHasSourceCollections, ICanProcessSourceItemChange
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, bool> PredicateFunc => _predicateFunc;

		private sealed class ItemInfo : ExpressionItemInfo
		{
			public Func<bool> PredicateFunc;
			public bool PredicateResult;
		}

		private readonly Func<TSourceItem, bool> _predicateFunc;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpressionOriginal;

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly ExpressionWatcher.ExpressionInfo _predicateExpressionInfo;

		private bool _sourceInitialized;

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private INotifyCollectionChanged _source;
		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
		private Queue<ExpressionWatcher.Raise> _deferredExpressionWatcherChangedProcessings;

		private int _predicatePassedCount;
        private ICanProcessSourceItemChange _thisAsCanProcessSourceItemChange;
        private List<IComputingInternal> _nestedComputings;

        private int _predicateExpressionСallCount;

		private readonly bool _predicateContainsParametrizedObservableComputationCalls;
		[ObservableComputationsCall]
		public AnyComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression) : this(predicateExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public AnyComputing(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, bool>> predicateExpression) : this(predicateExpression, Utils.getCapacity(source))
		{
			_source = source;
		}

		private AnyComputing(Expression<Func<TSourceItem, bool>> predicateExpression, int capacity)
		{
            Utils.construct(
                predicateExpression, 
                capacity, 
                out _itemInfos, 
                out _sourcePositions, 
                ref _predicateExpressionOriginal, 
                ref _predicateExpression, 
                ref _predicateContainsParametrizedObservableComputationCalls, 
                ref _predicateExpressionInfo, 
                ref _predicateExpressionСallCount, 
                ref _predicateFunc, 
                ref _nestedComputings);

            _thisAsCanProcessSourceItemChange = this;
		}

		private void calculateValue()
		{
			if (_value != _predicatePassedCount > 0)
			{
				setValue(!_value);				
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent(sender, e);
			if (!_rootSourceWrapper && _lastProcessedSourceChangeMarker == _sourceAsList.ChangeMarkerField) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					_isConsistent = false;
					int newSourceIndex = e.NewStartingIndex;
					TSourceItem addedSourceItem = _sourceAsList[newSourceIndex];
					ItemInfo itemInfo = registerSourceItem(addedSourceItem, newSourceIndex);
					if (ApplyPredicate(newSourceIndex))
					{
						_predicatePassedCount++;			
						itemInfo.PredicateResult = true;	
					}

					calculateValue();
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					if (_itemInfos[oldStartingIndex].PredicateResult) _predicatePassedCount--;
					unregisterSourceItem(oldStartingIndex);
					calculateValue();
					break;
				case NotifyCollectionChangedAction.Move:
					int newSourceIndex1 = e.NewStartingIndex;
					int oldSourceIndex = e.OldStartingIndex;

					if (newSourceIndex1 != oldSourceIndex)
					{
						_sourcePositions.Move(oldSourceIndex, newSourceIndex1);
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					_isConsistent = false;
					initializeFromSource();
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Replace:
					_isConsistent = false;
					int newStartingIndex = e.NewStartingIndex;
					ItemInfo replacingItemInfo = _itemInfos[newStartingIndex];
					ExpressionWatcher oldExpressionWatcher = replacingItemInfo.ExpressionWatcher;
					oldExpressionWatcher.Dispose();
                    EventUnsubscriber.QueueSubscriptions(oldExpressionWatcher._propertyChangedEventSubscriptions, oldExpressionWatcher._methodChangedEventSubscriptions);

					if (replacingItemInfo.PredicateResult) _predicatePassedCount--;

					ExpressionWatcher watcher;
					Func<bool> predicateFunc;
					TSourceItem replacingSourceItem = _sourceAsList[newStartingIndex];
                    Utils.getItemInfoContent(
                        new object[]{replacingSourceItem}, 
                        out watcher, 
                        out predicateFunc, 
                        out List<IComputingInternal> nestedComputings1,
                        _predicateExpression,
                        ref _predicateExpressionСallCount,
                        this,
                        _predicateContainsParametrizedObservableComputationCalls,
                        _predicateExpressionInfo);	

					replacingItemInfo.PredicateFunc = predicateFunc;	
					watcher.ValueChanged = expressionWatcher_OnValueChanged;
					watcher._position = oldExpressionWatcher._position;
					replacingItemInfo.ExpressionWatcher = watcher;
                    replacingItemInfo.NestedComputings = nestedComputings1;

					if (ApplyPredicate(newStartingIndex))
					{
						_predicatePassedCount++;			
						replacingItemInfo.PredicateResult = true;	
					}
					else
					{
						replacingItemInfo.PredicateResult = false;	
					}

					calculateValue();	
					_isConsistent = true;
					raiseConsistencyRestored();					
					break;
			}

			_isConsistent = false;
			if (_deferredExpressionWatcherChangedProcessings != null)
				while (_deferredExpressionWatcherChangedProcessings.Count > 0)
				{
					ExpressionWatcher.Raise expressionWatcherRaise = _deferredExpressionWatcherChangedProcessings.Dequeue();
					if (!expressionWatcherRaise.ExpressionWatcher._disposed)
					{
						_handledEventSender = expressionWatcherRaise.EventSender;
						_handledEventArgs = expressionWatcherRaise.EventArgs;
						_thisAsCanProcessSourceItemChange.ProcessSourceItemChange(expressionWatcherRaise.ExpressionWatcher);
					}
				} 

			_isConsistent = true;
			raiseConsistencyRestored();

            Utils.postHandleSourceCollectionChanged(
                ref _handledEventSender,
                ref _handledEventArgs);
		}

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)        
        {
            (_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
            Utils.initializeNestedComputings(_nestedComputings, this);
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourceScalar, scalarValueChangedHandler, ref _source);
            Utils.uninitializeNestedComputings(_nestedComputings, this);
        }

        protected override void initializeFromSource()
		{
			if (_sourceInitialized)
			{
                Utils.disposeExpressionItemInfos(_itemInfos, _predicateExpressionСallCount, this);
                Utils.disposeSource(
                    _sourceScalar, 
                    _source,
                    ref _itemInfos,
                    ref _sourcePositions, 
                    _sourceAsList, 
                    handleSourceCollectionChanged);

                _sourceInitialized = false;
            }

			_predicatePassedCount = 0;
            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this, ref _sourceAsList, false);

			if (_source != null && _isActive)
			{
                Utils.initializeFromObservableCollectionWithChangeMarker(
                    _source, 
                    ref _sourceAsList, 
                    ref _rootSourceWrapper, 
                    ref _lastProcessedSourceChangeMarker);

				int count = _sourceAsList.Count;
				for (int sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];
					ItemInfo itemInfo = registerSourceItem(sourceItem, sourceIndex);

					if (ApplyPredicate(sourceIndex))
					{
						_predicatePassedCount++;
						itemInfo.PredicateResult = true;
					}
				}

                _sourceAsList.CollectionChanged += handleSourceCollectionChanged;

				calculateValue();

                _sourceInitialized = true;
            }
			else
			{
				if (_value)
				{
					setValue(false);
				}
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public bool ApplyPredicate(int sourceIndex)
		{
            bool getValue() =>
                _predicateContainsParametrizedObservableComputationCalls 
                    ? _itemInfos[sourceIndex].PredicateFunc() 
                    : _predicateFunc(_sourceAsList[sourceIndex]);

            if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
				bool result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
				return result;
			}

			return  getValue();
		}

		private ItemInfo registerSourceItem(TSourceItem sourceItem, int sourceIndex)
		{
			ItemInfo itemInfo =_sourcePositions.Insert(sourceIndex);

            Utils.getItemInfoContent(
                new object[]{sourceItem}, 
                out ExpressionWatcher watcher, 
                out Func<bool> predicateFunc, 
                out List<IComputingInternal> nestedComputings,
                _predicateExpression,
                ref _predicateExpressionСallCount,
                this,
                _predicateContainsParametrizedObservableComputationCalls,
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
			ItemInfo itemInfo = _itemInfos[sourceIndex];

			ExpressionWatcher watcher = itemInfo.ExpressionWatcher;
			watcher.Dispose();
            EventUnsubscriber.QueueSubscriptions(watcher._propertyChangedEventSubscriptions, watcher._methodChangedEventSubscriptions);

			_sourcePositions.Remove(sourceIndex);

            if (_predicateContainsParametrizedObservableComputationCalls)
                Utils.itemInfoRemoveDownstreamConsumedComputing(itemInfo.NestedComputings, this);
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
                _thisAsCanProcessSourceItemChange,
                ref _deferredExpressionWatcherChangedProcessings, 
                ref _isConsistent,
                ref _handledEventSender,
                ref _handledEventArgs,
                _isConsistent);
		}

		void ICanProcessSourceItemChange.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			if (itemInfo.PredicateResult) _predicatePassedCount--;

			if (ApplyPredicate(sourceIndex))
			{
				_predicatePassedCount++;
				itemInfo.PredicateResult = true;
			}
			else
			{
				itemInfo.PredicateResult = false;
			}

			calculateValue();
		}

		public void ValidateConsistency()
		{
			_sourcePositions.ValidateConsistency();
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			if (_itemInfos.Count != sourceCount) throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.1");
			Func<TSourceItem, bool> predicate = _predicateExpressionOriginal.Compile();
			int predicatePassedCount = 0;

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_sourcePositions.List.Count != sourceCount)
					throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.9");

				// ReSharper disable once NotAccessedVariable
				int index = 0;
				for (int sourceIndex = 0; sourceIndex < sourceCount; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];
					if (predicate(sourceItem))
					{
						predicatePassedCount++;
						index++;
						if (!itemInfo.PredicateResult) throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.2");
					}
					else
					{
						if (itemInfo.PredicateResult) throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.3");
					}

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.4");
					if (itemInfo.ExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.5");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.ExpressionWatcher._position))
						throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.6");

					if (itemInfo.ExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.10");

				}

				if (predicatePassedCount != _predicatePassedCount) throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.7");
				if (_value != _predicatePassedCount > 0) throw new ObservableComputationsException(this, "Consistency violation: AnyComputing.8");


			}
		}
    }
}
