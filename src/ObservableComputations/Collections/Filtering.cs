using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	public class Filtering<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ICanProcessSourceItemChange
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, bool> PredicateFunc => _predicateFunc;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

        private List<IComputingInternal> _nestedComputings;

        private sealed class ItemInfo : ExpressionItemInfo
		{
			public Func<bool> PredicateFunc;
			public Position FilteredPosition;
			public Position NextFilteredItemPosition;
		}

		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpressionOriginal;
        private int _predicateExpressionСallCount;

		private Positions<Position> _filteredPositions;	
		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly ExpressionWatcher.ExpressionInfo _predicateExpressionInfo;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;
		private bool _lastProcessedSourceChangeMarker;

		private readonly bool _predicateContainsParametrizedObservableComputationsCalls;

		private Queue<ExpressionWatcher.Raise> _deferredExpressionWatcherChangedProcessings;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		internal INotifyCollectionChanged _source;
		private readonly Func<TSourceItem, bool> _predicateFunc;
        private ICanProcessSourceItemChange _thisAsCanProcessSourceItemChange;

		[ObservableComputationsCall]
		public Filtering(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int capacity = 0) : this(predicateExpression, Utils.getCapacity(sourceScalar), capacity)
		{
			_sourceScalar = sourceScalar;
		}
		
		[ObservableComputationsCall]
		public Filtering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int capacity = 0) : this(predicateExpression, Utils.getCapacity(source), capacity)
		{
			_source = source;
		}

		private Filtering(Expression<Func<TSourceItem, bool>> predicateExpression, 
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

            _thisAsCanProcessSourceItemChange = this;

			_initialCapacity = capacity;
			_filteredPositions = new Positions<Position>(new List<Position>(_initialCapacity));	
		}

		[ObservableComputationsCall]
		public Filtering<TSourceItem> TryConstructInsertOrRemoveActions()
		{
			ConstructInsertOrRemoveActionsVisitor
				constructInsertOrRemoveActionsVisitor =
					new ConstructInsertOrRemoveActionsVisitor();

			constructInsertOrRemoveActionsVisitor.Visit(_predicateExpressionOriginal);

			Action<TSourceItem> transformSourceItemIntoMember =
				constructInsertOrRemoveActionsVisitor.TransformSourceItemIntoMember;
			if (transformSourceItemIntoMember != null)
			{
				InsertItemAction = (index, item) => transformSourceItemIntoMember(item);
			}

			Action<TSourceItem> transformSourceItemNotIntoMember =
				constructInsertOrRemoveActionsVisitor.TransformSourceItemIntoNotMember;
			if (transformSourceItemNotIntoMember != null)
			{
				RemoveItemAction = index => transformSourceItemNotIntoMember(this[index]);
			}

			return this;
		}

		private class ConstructInsertOrRemoveActionsVisitor : ExpressionVisitor
		{
			public Action<TSourceItem> TransformSourceItemIntoMember;
			public Action<TSourceItem> TransformSourceItemIntoNotMember;
			#region Overrides of ExpressionVisitor

			//TODO !sourceItem.NullableProp.HasValue должно преобразооваться в sourceItem.NullableProp ==  null
			// TODO обрабытывать выражения типа '25 == sourceItem.Id'
			protected override Expression VisitBinary(BinaryExpression node) 
			{
				ExpressionType nodeType = node.NodeType;
				if (nodeType == ExpressionType.Equal || nodeType == ExpressionType.NotEqual)
				{
					if (node.Left is MemberExpression memberExpression)
					{
						Expression rightExpression = node.Right;

						// ReSharper disable once PossibleNullReferenceException
						MemberInfo memberExpressionMember = memberExpression.Member;
						Type declaringType = memberExpressionMember.DeclaringType;
						// ReSharper disable once PossibleNullReferenceException
						if (declaringType.IsConstructedGenericType 					                                                 
						    && declaringType.GetGenericTypeDefinition() == typeof(Nullable<>)
						    && memberExpressionMember.Name == "Value")
						{
							memberExpression = (MemberExpression) memberExpression.Expression;
							rightExpression = Expression.Convert(rightExpression,
								typeof (Nullable<>).MakeGenericType(node.Left.Type));
						}

						ParameterExpression parameterExpression = getParameterExpression(memberExpression);
						if (parameterExpression != null)
						{
							if (memberExpression.Type != rightExpression.Type)
							{
								rightExpression = Expression.Convert(rightExpression, memberExpression.Type);
							}

							if (!memberExpressionMember.IsReadOnly())
							{
								if (nodeType == ExpressionType.Equal)
								{
									TransformSourceItemIntoMember = Expression.Lambda<Action<TSourceItem>>(Expression.Assign(memberExpression, rightExpression), parameterExpression).Compile();	
								}
								else if (nodeType == ExpressionType.NotEqual)
								{
									TransformSourceItemIntoNotMember = Expression.Lambda<Action<TSourceItem>>(Expression.Assign(memberExpression, rightExpression), parameterExpression).Compile();	
								}
							}	
						}						
					}
				}

				return node;
			}

			 
			private ParameterExpression getParameterExpression(MemberExpression memberExpression)
			{
				if (memberExpression.Expression is ParameterExpression parameterExpression)
				{
					return parameterExpression;
				}

				if (memberExpression.Expression is MemberExpression expressionMemberExpression)
				{
					return getParameterExpression(expressionMemberExpression); // memberExpression.Expression может быть не MemberExpression				
				}

				return null;
			}


			#endregion
		}

        protected override void initializeFromSource()
		{
			int originalCount = _items.Count;

			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				Utils.disposeExpressionItemInfos(_itemInfos, _predicateExpressionСallCount, this);

				_filteredPositions = new Positions<Position>(new List<Position>(_initialCapacity));	

                Utils.disposeSource(
                    _sourceScalar, 
                    _source,
                    ref _itemInfos,
                    ref _sourcePositions, 
                    _sourceAsList, 
                    ref _sourceNotifyCollectionChangedEventHandler);
			}

            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this, ref _sourceAsList, null);

			if (_source != null && _isActive)
			{
                Utils.initializeFromObservableCollectionWithChangeMarker(
                    _source, 
                    ref _sourceAsList, 
                    ref _rootSourceWrapper, 
                    ref _lastProcessedSourceChangeMarker);

				Position nextItemPosition = _filteredPositions.Add();
				int count = _sourceAsList.Count;
				int insertingIndex = 0;
				int sourceIndex;

				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];
					Position currentFilteredItemPosition = null;

					ItemInfo itemInfo =  registerSourceItem(sourceItem, sourceIndex, null, null);

					if (ApplyPredicate(sourceIndex))
					{
						if (originalCount > insertingIndex)
							_items[insertingIndex++] = sourceItem;
						else
							_items.Insert(insertingIndex++, sourceItem);

						currentFilteredItemPosition = nextItemPosition;
						nextItemPosition = _filteredPositions.Add();
					}

					itemInfo.FilteredPosition = currentFilteredItemPosition;
					itemInfo.NextFilteredItemPosition = nextItemPosition;
				}

				for (int index = originalCount - 1; index >= insertingIndex; index--)
				{
					_items.RemoveAt(index);
				}

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
                _sourceAsList.CollectionChanged += _sourceNotifyCollectionChangedEventHandler;
			}
			else
			{
				_items.Clear();
			}

			reset();
		}

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_sourceScalar, ref _sourceScalarPropertyChangedEventHandler, ref _source, getScalarValueChangedHandler());
            Utils.initializeNestedComputings(_nestedComputings, this);
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourceScalar, _sourceScalarPropertyChangedEventHandler);
            Utils.uninitializeNestedComputings(_nestedComputings, this);
        }

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
            if (!Utils.preHandleSourceCollectionChanged(
                sender, 
                e, 
                _rootSourceWrapper, 
                ref _lastProcessedSourceChangeMarker, 
                _sourceAsList, 
                _isConsistent,
                this,
                ref _handledEventSender,
                ref _handledEventArgs)) return;	

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:		
					_isConsistent = false;
					int newSourceIndex = e.NewStartingIndex;
					TSourceItem addedSourceItem = _sourceAsList[newSourceIndex];
					Position newItemPosition = null;
					Position nextItemPosition;

					int? newFilteredIndex  = null;

                    Utils.getItemInfoContent(
                        addedSourceItem, 
                        out ExpressionWatcher newWatcher, 
                        out Func<bool> newPredicateFunc, 
                        out List<IComputingInternal> nestedComputings,
                        _predicateExpression,
                        ref _predicateExpressionСallCount,
                        this,
                        _predicateContainsParametrizedObservableComputationsCalls,
                        _predicateExpressionInfo);

					if (newSourceIndex < _itemInfos.Count)
					{
						ItemInfo oldItemInfo = _itemInfos[newSourceIndex];

						Position oldItemInfoNextFilteredItemPosition = oldItemInfo.NextFilteredItemPosition;
						Position oldItemInfoFilteredPosition = oldItemInfo.FilteredPosition;
						if (applyPredicate(addedSourceItem, newPredicateFunc))
						{
							if (oldItemInfoFilteredPosition == null)
							{
								newItemPosition = _filteredPositions.Insert(oldItemInfoNextFilteredItemPosition.Index);
								nextItemPosition = oldItemInfoNextFilteredItemPosition;
								newFilteredIndex = newItemPosition.Index;
							}
							else
							{
								int filteredIndex = oldItemInfoFilteredPosition.Index;
								newFilteredIndex = filteredIndex;
								newItemPosition = _filteredPositions.Insert(filteredIndex);
								nextItemPosition = oldItemInfoFilteredPosition;
							}
							
							modifyNextFilteredItemIndex(newSourceIndex, newItemPosition);
						}
						else
						{
							nextItemPosition = oldItemInfoFilteredPosition ?? oldItemInfoNextFilteredItemPosition;
						}	
					}
					else
					{			
						if (applyPredicate(addedSourceItem, newPredicateFunc))
						{
							newItemPosition = _filteredPositions.List[Count];
							newFilteredIndex = Count;		
							nextItemPosition = _filteredPositions.Add();
						}
						else
						{
							//здесь newPositionIndex = null; так и должно быть
							nextItemPosition = _filteredPositions.List[Count];							
						}
					}

					registerSourceItem(addedSourceItem, newSourceIndex, newItemPosition, nextItemPosition, newWatcher, newPredicateFunc, nestedComputings);

					if (newFilteredIndex != null)
						baseInsertItem(newFilteredIndex.Value, addedSourceItem);
					
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Remove:
					unregisterSourceItem(e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Move:
					_isConsistent = false;
					int newSourceIndex1 = e.NewStartingIndex;
					int oldSourceIndex = e.OldStartingIndex;

					if (newSourceIndex1 != oldSourceIndex)
					{
						ItemInfo itemInfoOfOldSourceIndex = _itemInfos[oldSourceIndex];
						ItemInfo itemInfoOfNewSourceIndex = _itemInfos[newSourceIndex1];

						Position newPosition1;
						Position nextPosition1;

						Position itemInfoOfOldSourceIndexFilteredPosition = itemInfoOfOldSourceIndex.FilteredPosition;
						Position itemInfoOfNewSourceIndexNextFilteredItemPosition = itemInfoOfNewSourceIndex.NextFilteredItemPosition;
						Position itemInfoOfNewSourceIndexFilteredPosition = itemInfoOfNewSourceIndex.FilteredPosition;
						if (itemInfoOfOldSourceIndexFilteredPosition != null)
						{
							int oldFilteredIndex = itemInfoOfOldSourceIndexFilteredPosition.Index;
							int newFilteredIndex1;

							newPosition1 = itemInfoOfOldSourceIndexFilteredPosition;
							if (itemInfoOfNewSourceIndexFilteredPosition == null)
							{
								//nextPositionIndex = itemInfoOfNewSourceIndex.NextFilteredItemIndex;
								nextPosition1 =
									itemInfoOfNewSourceIndexNextFilteredItemPosition != itemInfoOfOldSourceIndexFilteredPosition
										? itemInfoOfNewSourceIndexNextFilteredItemPosition
										: itemInfoOfOldSourceIndex.NextFilteredItemPosition;
								newFilteredIndex1 =  oldSourceIndex < newSourceIndex1 ? 
									itemInfoOfNewSourceIndexNextFilteredItemPosition.Index - 1 : 
									itemInfoOfNewSourceIndexNextFilteredItemPosition.Index;			
							}
							else
							{
								nextPosition1 = oldSourceIndex < newSourceIndex1 ? 
									itemInfoOfNewSourceIndexNextFilteredItemPosition : 
									itemInfoOfNewSourceIndexFilteredPosition;
								newFilteredIndex1 = itemInfoOfNewSourceIndexFilteredPosition.Index;
							}

							_filteredPositions.Move(
								oldFilteredIndex, 
								newFilteredIndex1);

							modifyNextFilteredItemIndex(oldSourceIndex, itemInfoOfOldSourceIndex.NextFilteredItemPosition);

							_sourcePositions.Move(oldSourceIndex, newSourceIndex1);

							itemInfoOfOldSourceIndex.NextFilteredItemPosition = nextPosition1;

							modifyNextFilteredItemIndex(newSourceIndex1, newPosition1);
				
							baseMoveItem(oldFilteredIndex, newFilteredIndex1);				
						}
						else
						{
							nextPosition1 = oldSourceIndex < newSourceIndex1
								? itemInfoOfNewSourceIndexNextFilteredItemPosition
								: (itemInfoOfNewSourceIndexFilteredPosition ?? itemInfoOfNewSourceIndexNextFilteredItemPosition);

							itemInfoOfOldSourceIndex.NextFilteredItemPosition = nextPosition1;
							
							_sourcePositions.Move(oldSourceIndex, newSourceIndex1);
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
					int sourceIndex = e.NewStartingIndex;
					ItemInfo replacingItemInfo = _itemInfos[sourceIndex];
					ExpressionWatcher oldExpressionWatcher = replacingItemInfo.ExpressionWatcher;
					oldExpressionWatcher.Dispose();
                    EventUnsubscriber.QueueSubscriptions(oldExpressionWatcher._propertyChangedEventSubscriptions, oldExpressionWatcher._methodChangedEventSubscriptions);

                    if (_predicateContainsParametrizedObservableComputationsCalls)
                        Utils.itemInfoRemoveDownstreamConsumedComputing(replacingItemInfo.NestedComputings, this);

					TSourceItem replacingSourceItem = _sourceAsList[sourceIndex];

                    Utils.getItemInfoContent(
                        replacingSourceItem, 
                        out ExpressionWatcher watcher, 
                        out Func<bool> predicateFunc, 
                        out List<IComputingInternal> nestedComputings1,
                        _predicateExpression,
                        ref _predicateExpressionСallCount,
                        this,
                        _predicateContainsParametrizedObservableComputationsCalls,
                        _predicateExpressionInfo);	
                    
                    replacingItemInfo.PredicateFunc = predicateFunc;	
					watcher.ValueChanged = expressionWatcher_OnValueChanged;
					watcher._position = oldExpressionWatcher._position;
					replacingItemInfo.ExpressionWatcher = watcher;
                    replacingItemInfo.NestedComputings = nestedComputings1;

					if (!processChangeSourceItem(sourceIndex) && replacingItemInfo.FilteredPosition != null)
					{
						baseSetItem(replacingItemInfo.FilteredPosition.Index, replacingSourceItem);
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

        private void modifyNextFilteredItemIndex(int sourceIndex, Position nextItemPosition)
		{
			for (int previousSourceIndex = sourceIndex - 1; previousSourceIndex >= 0; previousSourceIndex--)
			{
				ItemInfo itemInfo = _itemInfos[previousSourceIndex];
				itemInfo.NextFilteredItemPosition = nextItemPosition;
				if (itemInfo.FilteredPosition != null) break;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public bool ApplyPredicate(int sourceIndex)
		{
            bool getValue() =>
                _predicateContainsParametrizedObservableComputationsCalls
                    ? _itemInfos[sourceIndex].PredicateFunc()
                    : _predicateFunc(_sourceAsList[sourceIndex]);

            if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);			
				bool result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		private bool applyPredicate(TSourceItem sourceItem, Func<bool> itemPredicateFunc)
		{
            bool getValue() =>
                _predicateContainsParametrizedObservableComputationsCalls
                    ? itemPredicateFunc()
                    : _predicateFunc(sourceItem);

            if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
				var result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
				return result;
			}

			return getValue() ;
		}

		private ItemInfo registerSourceItem(TSourceItem sourceItem, int sourceIndex, Position position,
            Position nextItemPosition, ExpressionWatcher watcher = null, Func<bool> predicateFunc = null,
            List<IComputingInternal> nestedComputings = null)
		{
			ItemInfo itemInfo = _sourcePositions.Insert(sourceIndex);
			itemInfo.FilteredPosition = position;
			itemInfo.NextFilteredItemPosition = nextItemPosition;

			if (watcher == null /*&& predicateFunc == null*/) 
                Utils.getItemInfoContent(
                    sourceItem, 
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
			ItemInfo itemInfo = _itemInfos[sourceIndex];

			ExpressionWatcher watcher = itemInfo.ExpressionWatcher;
			watcher.Dispose();
            EventUnsubscriber.QueueSubscriptions(watcher._propertyChangedEventSubscriptions, watcher._methodChangedEventSubscriptions);

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
			ItemInfo itemInfo = _itemInfos[sourceIndex];

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

		public void ValidateConsistency()
		{
			_filteredPositions.ValidateConsistency();
			_sourcePositions.ValidateConsistency();

			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			if (_itemInfos.Count != source.Count) throw new ObservableComputationsException(this, "Consistency violation: Filtering.9");
			Func<TSourceItem, bool> predicate = _predicateExpression.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_itemInfos.Count != source.Count)
					throw new ObservableComputationsException(this, "Consistency violation: Filtering.14");

				if (_source != null && _filteredPositions.List.Count - 1 != Count)
					throw new ObservableComputationsException(this, "Consistency violation: Filtering.15");

				if (_source == null && _filteredPositions.List.Count != 0)
					throw new ObservableComputationsException(this, "Consistency violation: Filtering.16");

				int index = 0;
				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];
					if (predicate(sourceItem))
					{
						if (itemInfo.FilteredPosition == null) throw new ObservableComputationsException(this, "Consistency violation: Filtering.2");

						if (!EqualityComparer<TSourceItem>.Default.Equals(this[index], sourceItem))
						{
							throw new ObservableComputationsException(this, "Consistency violation: Filtering.1");
						}

						if (itemInfo.FilteredPosition.Index != index) throw new ObservableComputationsException(this, "Consistency violation: Filtering.5");
						
						index++;
					}
					else
					{
						if (itemInfo.FilteredPosition != null) throw new ObservableComputationsException(this, "Consistency violation: Filtering.3");
					}

					if (_itemInfos[sourceIndex].Index != sourceIndex) throw new ObservableComputationsException(this, "Consistency violation: Filtering.7");
					if (itemInfo.ExpressionWatcher._position != _itemInfos[sourceIndex]) throw new ObservableComputationsException(this, "Consistency violation: Filtering.8");

					if (itemInfo.FilteredPosition != null && !_filteredPositions.List.Contains(itemInfo.FilteredPosition))
						throw new ObservableComputationsException(this, "Consistency violation: Filtering.10");

					if (!_filteredPositions.List.Contains(itemInfo.NextFilteredItemPosition))
						throw new ObservableComputationsException(this, "Consistency violation: Filtering.11");

					if (!_itemInfos.Contains(itemInfo.ExpressionWatcher._position))
						throw new ObservableComputationsException(this, "Consistency violation: Filtering.12");
				}

				if (_source != null)
				{
					int count = source.Where(sourceItem => predicate(sourceItem)).Count();
					if (_filteredPositions.List.Count != count + 1)
					{
						throw new ObservableComputationsException(this, "Consistency violation: Filtering.6");
					}

					Position nextFilteredItemPosition;
					nextFilteredItemPosition = _filteredPositions.List[count];
					for (int sourceIndex = source.Count - 1; sourceIndex >= 0; sourceIndex--)
					{
						TSourceItem sourceItem = source[sourceIndex];
						ItemInfo itemInfo = _itemInfos[sourceIndex];

						if (itemInfo.NextFilteredItemPosition != nextFilteredItemPosition) 
								throw new ObservableComputationsException(this, "Consistency violation: Filtering.4");

						if (predicate(sourceItem))
						{
							nextFilteredItemPosition = itemInfo.FilteredPosition;
						}
					}
				}
			}
		}
	}
}
