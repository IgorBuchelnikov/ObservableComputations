using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ObservableComputations.ExtentionMethods;


namespace ObservableComputations
{
	public class GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> : CollectionComputing<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>>, IHasSourceCollections
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> OuterSourceScalar => _outerSourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged OuterSource => _outerSource;

		// ReSharper disable once UnusedMember.Global
		public IReadScalar<INotifyCollectionChanged> InnerSourceScalar => _grouping._sourceScalar;
		public INotifyCollectionChanged InnerSource => _grouping._source;

		public IEqualityComparer<TKey> EqualityComparer => _grouping._equalityComparer;

		public IReadScalar<IEqualityComparer<TKey>> EqualityComparerScalar => _grouping._equalityComparerScalar;

		public Expression<Func<TInnerSourceItem, TKey>> InnerKeySelector =>  _grouping._keySelectorExpressionOriginal;

		public Expression<Func<TOuterSourceItem, TKey>> OuterKeySelector => _outerKeySelectorExpressionOriginal;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{OuterSource, InnerSource});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{OuterSourceScalar, InnerSourceScalar});

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, TInnerSourceItem> InsertItemIntoGroupAction
		{
			get => _insertItemIntoGroupAction;
			set
			{
				if (_insertItemIntoGroupAction != value)
				{
					_insertItemIntoGroupAction = value;
					OnPropertyChanged(Utils.InsertItemIntoGroupActionPropertyChangedEventArgs);
				}

			}
		}

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int> RemoveItemFromGroupAction
		{
			get => _removeItemFromGroupAction;
			set
			{
				if (_removeItemFromGroupAction != value)
				{
					_removeItemFromGroupAction = value;
					OnPropertyChanged(Utils.RemoveItemFromGroupActionPropertyChangedEventArgs);
				}
			}
		}

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, int> MoveItemInGroupAction
		{
			get => _moveItemInGroupAction;
			set
			{
				if (_moveItemInGroupAction != value)
				{
					_moveItemInGroupAction = value;
					OnPropertyChanged(Utils.MoveItemInGroupActionPropertyChangedEventArgs);
				}
			}
		}

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>> ClearGroupItemsAction
		{
			get => _clearGroupItemsAction;
			set
			{
				if (_clearGroupItemsAction != value)
				{
					_clearGroupItemsAction = value;
					OnPropertyChanged(Utils.ClearGroupItemsActionPropertyChangedEventArgs);
				}
			}
		}

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, TInnerSourceItem> SetGroupItemAction
		{
			get => _setGroupItemAction;
			set
			{
				if (_setGroupItemAction != value)
				{
					_setGroupItemAction = value;
					OnPropertyChanged(Utils.SetGroupItemActionPropertyChangedEventArgs);
				}
			}
		}

		private PropertyChangedEventHandler _outerSourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _outerSourceScalarWeakPropertyChangedEventHandler;
		private readonly Func<TOuterSourceItem, TKey> _outerKeySelectorFunc;
		private readonly Expression<Func<TOuterSourceItem, TKey>> _outerKeySelectorExpression;

		// ReSharper disable once MemberCanBePrivate.Global

		private readonly ExpressionWatcher.ExpressionInfo _outerKeySelectorExpressionInfo;
		private readonly bool _outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls;

		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, TInnerSourceItem> _insertItemIntoGroupAction;
		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int> _removeItemFromGroupAction;
		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, TInnerSourceItem> _setGroupItemAction;
		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, int> _moveItemInGroupAction;
		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>> _clearGroupItemsAction;

		Grouping<TInnerSourceItem, TKey> _grouping;

		private ObservableCollectionWithChangeMarker<TOuterSourceItem> _outerSourceAsList;
		bool _outerRootSourceWrapper;

		private NotifyCollectionChangedEventHandler _outerSourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _outerSourceWeakNotifyCollectionChangedEventHandler;

		private NotifyCollectionChangedEventHandler _groupingNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _groupingWeakNotifyCollectionChangedEventHandler;

		Positions<Position> _outerSourceItemPositions;
		Dictionary<TKey, List<Position>> _keyPositions;
		readonly List<Position> _nullKeyPositions = new List<Position>();

		private bool _lastProcessedSourceChangeMarker;
		private Queue<ExpressionWatcher.Raise> _deferredOuterSourceItemKeyExpressionWatcherChangedProcessings;
		private readonly IReadScalar<INotifyCollectionChanged> _outerSourceScalar;
		private INotifyCollectionChanged _outerSource;
		private readonly Expression<Func<TOuterSourceItem, TKey>> _outerKeySelectorExpressionOriginal;

		[ObservableComputationsCall]
		public GroupJoining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar = null) : this(outerKeySelector, Utils.getCapacity(outerSourceScalar))
		{
			_outerSourceScalar = outerSourceScalar;
			initializeOuterSourceScalar();

			initializeGrouping(innerSourceScalar, innerKeySelector, equalityComparerScalar);
			_keyPositions = new Dictionary<TKey, List<Position>>(_grouping._equalityComparer);

			initializeFromOuterSource();
		}

		[ObservableComputationsCall]
		public GroupJoining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			INotifyCollectionChanged innerSource,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar = null) : this(outerKeySelector, Utils.getCapacity(outerSourceScalar))
		{
			_outerSourceScalar = outerSourceScalar;
			initializeOuterSourceScalar();

			initializeGrouping(innerSource, innerKeySelector, equalityComparerScalar);
			_keyPositions = new Dictionary<TKey, List<Position>>(_grouping._equalityComparer);

			initializeFromOuterSource();
		}

		[ObservableComputationsCall]
		public GroupJoining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			INotifyCollectionChanged innerSource,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IEqualityComparer<TKey> equalityComparer = null) : this(outerKeySelector, Utils.getCapacity(outerSourceScalar))
		{
			_outerSourceScalar = outerSourceScalar;
			initializeOuterSourceScalar();

			initializeGrouping(innerSource, innerKeySelector, equalityComparer);
			_keyPositions = new Dictionary<TKey, List<Position>>(_grouping._equalityComparer);

			initializeFromOuterSource();
		}

		[ObservableComputationsCall]
		public GroupJoining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IEqualityComparer<TKey> equalityComparer = null) : this(outerKeySelector, Utils.getCapacity(outerSourceScalar))
		{
			_outerSourceScalar = outerSourceScalar;
			initializeOuterSourceScalar();

			initializeGrouping(innerSourceScalar, innerKeySelector, equalityComparer);
			_keyPositions = new Dictionary<TKey, List<Position>>( _grouping._equalityComparer);

			initializeFromOuterSource();
		}

		[ObservableComputationsCall]
		public GroupJoining(
			INotifyCollectionChanged outerSource,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar = null) : this(outerKeySelector, Utils.getCapacity(outerSource))
		{
			_outerSource = outerSource;

			initializeGrouping(innerSourceScalar, innerKeySelector, equalityComparerScalar);
			_keyPositions = new Dictionary<TKey, List<Position>>( _grouping._equalityComparer);

			initializeFromOuterSource();
		}

		[ObservableComputationsCall]
		public GroupJoining(
			INotifyCollectionChanged outerSource,
			INotifyCollectionChanged innerSource,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar = null) : this(outerKeySelector, Utils.getCapacity(outerSource))
		{
			_outerSource = outerSource;

			initializeGrouping(innerSource, innerKeySelector, equalityComparerScalar);
			_keyPositions = new Dictionary<TKey, List<Position>>(_grouping._equalityComparer);

			initializeFromOuterSource();
		}

		[ObservableComputationsCall]
		public GroupJoining(
			INotifyCollectionChanged outerSource,
			INotifyCollectionChanged innerSource,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IEqualityComparer<TKey> equalityComparer = null) : this(outerKeySelector, Utils.getCapacity(outerSource))
		{
			_outerSource = outerSource;

			initializeGrouping(innerSource, innerKeySelector, equalityComparer);
			_keyPositions = new Dictionary<TKey, List<Position>>(_grouping._equalityComparer);

			initializeFromOuterSource();
		}

		[ObservableComputationsCall]
		public GroupJoining(
			INotifyCollectionChanged outerSource,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IEqualityComparer<TKey> equalityComparer = null) : this(outerKeySelector, Utils.getCapacity(outerSource))
		{
			_outerSource = outerSource;

			initializeGrouping(innerSourceScalar, innerKeySelector, equalityComparer);
			_keyPositions = new Dictionary<TKey, List<Position>>(_grouping._equalityComparer);

			initializeFromOuterSource();
		}

		private void initializeOuterSourceScalar()
		{
			_outerSourceScalarPropertyChangedEventHandler = handleOuterSourceScalarValueChanged;
			_outerSourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_outerSourceScalarPropertyChangedEventHandler);
			_outerSourceScalar.PropertyChanged += _outerSourceScalarWeakPropertyChangedEventHandler.Handle;
		}


		private void initializeGrouping(IReadScalar<INotifyCollectionChanged> innerSourceScalar, Expression<Func<TInnerSourceItem, TKey>> innerKeySelector, IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar)
		{
			_grouping = innerSourceScalar.Grouping(innerKeySelector, equalityComparerScalar);
			subscribeToGroupingCollectionChanged();
		}

		private void initializeGrouping(IReadScalar<INotifyCollectionChanged> innerSourceScalar, Expression<Func<TInnerSourceItem, TKey>> innerKeySelector, IEqualityComparer<TKey> equalityComparer)
		{
			_grouping = innerSourceScalar.Grouping(innerKeySelector, equalityComparer);
			subscribeToGroupingCollectionChanged();
		}

		private void initializeGrouping(INotifyCollectionChanged innerSource, Expression<Func<TInnerSourceItem, TKey>> innerKeySelector, IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar)
		{
			_grouping = innerSource.Grouping(innerKeySelector, equalityComparerScalar);
			subscribeToGroupingCollectionChanged();
		}

		private void initializeGrouping(INotifyCollectionChanged innerSource, Expression<Func<TInnerSourceItem, TKey>> innerKeySelector, IEqualityComparer<TKey> equalityComparer)
		{
			_grouping = innerSource.Grouping(innerKeySelector, equalityComparer);
			subscribeToGroupingCollectionChanged();
		}

		private void initializeFromOuterSource()
		{
			if (_outerSourceNotifyCollectionChangedEventHandler != null)
			{
				int count = Count;
				for (int index = 0; index < count; index++)
				{
					JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> group = this[index];
					ExpressionWatcher expressionWatcher = group._outerSourceItemKeySelectorExpressionWatcher;
					expressionWatcher.Dispose();
				}

				int capacity = _outerSourceScalar != null ? Utils.getCapacity(_outerSourceScalar) : Utils.getCapacity(_outerSource);
				_outerSourceItemPositions = new Positions<Position>(new List<Position>(capacity));
				_nullKeyPositions.Clear();
				_keyPositions = new Dictionary<TKey, List<Position>>(_grouping._equalityComparer);

				baseClearItems();

				if (_outerRootSourceWrapper)
				{
					_outerSourceAsList.CollectionChanged -= _outerSourceNotifyCollectionChangedEventHandler;
				}
				else
				{
					_outerSourceAsList.CollectionChanged -= _outerSourceWeakNotifyCollectionChangedEventHandler.Handle;
					_outerSourceWeakNotifyCollectionChangedEventHandler = null;					
				}

				_outerSourceNotifyCollectionChangedEventHandler = null;
			}


			if (_outerSourceScalar != null) _outerSource = _outerSourceScalar.Value;
			_outerSourceAsList = null;


			if (_outerSource != null)
			{
				if (_outerSource is ObservableCollectionWithChangeMarker<TOuterSourceItem> sourceAsList)
				{
					_outerSourceAsList = sourceAsList;
					_outerRootSourceWrapper = false;
				}
				else
				{
					_outerSourceAsList = new RootSourceWrapper<TOuterSourceItem>(_outerSource);
					_outerRootSourceWrapper = true;
				}

				_lastProcessedSourceChangeMarker = _outerSourceAsList.ChangeMarkerField;

				int count = _outerSourceAsList.Count;
				for (int index = 0; index < count; index++)
				{
					TOuterSourceItem sourceItem = _outerSourceAsList[index];
					registerOuterSourceItem(sourceItem, index);
				}

				_outerSourceNotifyCollectionChangedEventHandler = handleOuterSourceCollectionChanged;

				if (_outerRootSourceWrapper)
				{
					_outerSourceAsList.CollectionChanged += _outerSourceNotifyCollectionChangedEventHandler;
				}
				else
				{
					_outerSourceWeakNotifyCollectionChangedEventHandler = 
						new WeakNotifyCollectionChangedEventHandler(_outerSourceNotifyCollectionChangedEventHandler);

					_outerSourceAsList.CollectionChanged += _outerSourceWeakNotifyCollectionChangedEventHandler.Handle;
				}
			}
		}

		private void subscribeToGroupingCollectionChanged()
		{
			_groupingNotifyCollectionChangedEventHandler = handleGroupingCollectionChanged;
			_groupingWeakNotifyCollectionChangedEventHandler =
				new WeakNotifyCollectionChangedEventHandler(_groupingNotifyCollectionChangedEventHandler);
			_grouping.CollectionChanged += _groupingWeakNotifyCollectionChangedEventHandler.Handle;
		}

		private GroupJoining(Expression<Func<TOuterSourceItem, TKey>> outerKeySelector, int outerSourceCapacity) : base(outerSourceCapacity)
		{
			_outerSourceItemPositions = new Positions<Position>(new List<Position>(outerSourceCapacity));

			_outerKeySelectorExpressionOriginal = outerKeySelector;

			CallToConstantConverter callToConstantConverter =
				new CallToConstantConverter(outerKeySelector.Parameters);
			_outerKeySelectorExpression =
				(Expression<Func<TOuterSourceItem, TKey>>) callToConstantConverter.Visit(outerKeySelector);
			_outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls =
				callToConstantConverter.ContainsParametrizedObservableComputationCalls;

			if (!_outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls)
			{
				_outerKeySelectorExpressionInfo = ExpressionWatcher.GetExpressionInfo(_outerKeySelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				_outerKeySelectorFunc = _outerKeySelectorExpression.Compile();
			}
		}

		List<Position> getPositionsByKey(TKey key)
		{
			return
				key != null
					? _keyPositions.TryGetValue(key, out List<Position> positions)
						? positions
						: null
					: _nullKeyPositions;	   
		}

		private void handleOuterSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			_isConsistent = false;

			initializeFromOuterSource();

			_isConsistent = true;
			raiseConsistencyRestored();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleGroupingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			_isConsistent = false;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					Group<TInnerSourceItem, TKey> addedGroup 
						= (Group<TInnerSourceItem, TKey>) e.NewItems[0];

					List<Position> positions = getPositionsByKey(addedGroup._key);
					if (positions != null)
					{
						int positionsCount = positions.Count;
						for (int index = 0; index < positionsCount; index++)
						{
							Position position = positions[index];
							this[position.Index].setGroup(addedGroup);
						}
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					Group<TInnerSourceItem, TKey> removedGroup 
						= (Group<TInnerSourceItem, TKey>) e.OldItems[0];

					List<Position> positions1 = getPositionsByKey(removedGroup._key);
					if (positions1 != null)
					{
						int positions1Count = positions1.Count;
						for (int index = 0; index < positions1Count; index++)
						{
							Position position = positions1[index];
							this[position.Index].setGroup(null);
						}
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromOuterSource();
					break;
			}

			_isConsistent = true;
			raiseConsistencyRestored();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		//private void processGrouping()
		//{
		//	foreach (Group<TInnerSourceItem, TKey> @group in _grouping)
		//	{
		//		@group.JoinGroupsPositions = new List<Position>();
		//	}
		//}

		private void handleOuterSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent(sender, e);
			if (!_outerRootSourceWrapper && _lastProcessedSourceChangeMarker == _outerSourceAsList.ChangeMarkerField) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					_isConsistent = false;
					int newStartingIndex = e.NewStartingIndex;
					TOuterSourceItem addedItem = _outerSourceAsList[newStartingIndex];
					registerOuterSourceItem(addedItem, newStartingIndex);
					_isConsistent = true;
					raiseConsistencyRestored();					
					break;
				case NotifyCollectionChangedAction.Remove:
					unregisterOuterSourceItem(e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					_isConsistent = false;
					int newIndex1 = e.NewStartingIndex;
					TOuterSourceItem newItem = _outerSourceAsList[newIndex1];
					JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> replacingJoinGroup = this[newIndex1];

					replacingJoinGroup._outerSourceItemKeySelectorExpressionWatcher.Dispose();

					getNewExpressionWatcherAndKeySelectorFunc(
						newItem, 
						out ExpressionWatcher watcher, 
						out Func<TKey> outerKeySelectorFunc);

					watcher.ValueChanged = expressionWatcher_OnValueChanged;

					Position outerSourceItemPosition = replacingJoinGroup._outerSourceItemPosition;
					watcher._position = outerSourceItemPosition;

					TKey key = applyKeySelector(newItem, outerKeySelectorFunc);

					if (!_grouping._equalityComparer.Equals(key, replacingJoinGroup.Key))
					{
						unregisterKey(newIndex1, outerSourceItemPosition);
						registerKey(key, outerSourceItemPosition);					
						replacingJoinGroup.Key = key;
						replacingJoinGroup.setGroup(_grouping.getGroup(key));
					}

					replacingJoinGroup.OuterItem = newItem;
					replacingJoinGroup._outerSourceItemKeySelectorExpressionWatcher = watcher;
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Move:
					_isConsistent = false;
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 != newStartingIndex2)
					{
						_outerSourceItemPositions.Move(oldStartingIndex2, newStartingIndex2);
						baseMoveItem(oldStartingIndex2, newStartingIndex2);
					}
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Reset:
					_isConsistent = false;
					initializeFromOuterSource();
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
			}	
			
			_isConsistent = false;
			
			if (_deferredOuterSourceItemKeyExpressionWatcherChangedProcessings != null)
				while (_deferredOuterSourceItemKeyExpressionWatcherChangedProcessings.Count > 0)
				{
					ExpressionWatcher.Raise expressionWatcherRaise = _deferredOuterSourceItemKeyExpressionWatcherChangedProcessings.Dequeue();
					if (!expressionWatcherRaise.ExpressionWatcher._disposed)
					{
						_handledEventSender = expressionWatcherRaise.EventSender;
						_handledEventArgs = expressionWatcherRaise.EventArgs;
						processExpressionWatcherValueChanged(expressionWatcherRaise.ExpressionWatcher);
					}
				} 

			_isConsistent = true;
			raiseConsistencyRestored();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			checkConsistent(sender, eventArgs);

			_handledEventSender = sender;
			_handledEventArgs = eventArgs;

			if (_outerRootSourceWrapper || _outerSourceAsList.ChangeMarkerField == _lastProcessedSourceChangeMarker)
			{
				_isConsistent = false;
				processExpressionWatcherValueChanged(expressionWatcher);
				_isConsistent = true;
				raiseConsistencyRestored();
			}
			else
			{
				(_deferredOuterSourceItemKeyExpressionWatcherChangedProcessings = _deferredOuterSourceItemKeyExpressionWatcherChangedProcessings 
					??  new Queue<ExpressionWatcher.Raise>())
				.Enqueue(new ExpressionWatcher.Raise(expressionWatcher, sender, eventArgs));
			}

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void unregisterOuterSourceItem(int index)
		{
			ExpressionWatcher watcher = this[index]._outerSourceItemKeySelectorExpressionWatcher;
			Position position = watcher._position;

			unregisterKey(index, position);

			watcher.Dispose();

			_outerSourceItemPositions.Remove(index);
			//this[position.Index].Group.JoinGroupsPositions.Remove(position);

			baseRemoveItem(index);
		}


		private void registerOuterSourceItem(TOuterSourceItem outerSourceItem, int index)
		{
			getNewExpressionWatcherAndKeySelectorFunc(
				outerSourceItem, 
				out ExpressionWatcher watcher, 
				out Func<TKey> outerKeySelectorFunc);

			TKey key = applyKeySelector(outerSourceItem, outerKeySelectorFunc);

			Group<TInnerSourceItem, TKey> group = _grouping.getGroup(key);

			Position outerSourceItemPosition = _outerSourceItemPositions.Insert(index);
			JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> joinGroup = new JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceItem, this, group, outerSourceItemPosition, watcher, key, outerKeySelectorFunc);
			//group.JoinGroupsPositions.Add(outerSourceItemPosition);

			watcher.ValueChanged = expressionWatcher_OnValueChanged;
			watcher._position = outerSourceItemPosition;
			
			registerKey(key, outerSourceItemPosition);

			baseInsertItem(index, joinGroup);
		}
		
		private void unregisterKey(int index, Position position)
		{
			TKey key = this[index].Key;
			List<Position> keyPositions = getPositionsByKey(key);
			keyPositions.Remove(position);

			if (keyPositions.Count == 0 && key != null) _keyPositions.Remove(key);
		}

		private void registerKey(TKey key, Position outerSourceItemPosition)
		{
			List<Position> keyPositions = getPositionsByKey(key);
			if (keyPositions == null)
			{
				keyPositions = new List<Position>();
				_keyPositions.Add(key, keyPositions);
			}

			keyPositions.Add(outerSourceItemPosition);
		}		

		private void getNewExpressionWatcherAndKeySelectorFunc(TOuterSourceItem outerSourceItem, out ExpressionWatcher watcher,
			out Func<TKey> outerKeySelectorFunc)
		{
			outerKeySelectorFunc = null;

			if (!_outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls)
			{
				watcher = new ExpressionWatcher(_outerKeySelectorExpressionInfo, outerSourceItem);
			}
			else
			{
				Expression<Func<TKey>> deparametrizedSelectorExpression =
					(Expression<Func<TKey>>) _outerKeySelectorExpression.ApplyParameters(new object[] {outerSourceItem});
				Expression<Func<TKey>> selectorExpression =
					(Expression<Func<TKey>>)
					new CallToConstantConverter().Visit(deparametrizedSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				outerKeySelectorFunc = selectorExpression.Compile();
				watcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(selectorExpression));
			}
		}

		private TKey applyKeySelector(TOuterSourceItem outerSourceItem, Func<TKey> outerSelectorFunc)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				TKey result = _outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls 
					? outerSelectorFunc() 
					: _outerKeySelectorFunc(outerSourceItem);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return result;
			}

			return _outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls 
				? outerSelectorFunc() 
				: _outerKeySelectorFunc(outerSourceItem);
		}

		private TKey applyKeySelector(int index)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				_userCodeIsCalledFrom = computing;

				TKey result = _outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls 
					? this[index]._outerKeySelectorFunc() 
					: _outerKeySelectorFunc(_outerSourceAsList[index]);

				if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				_userCodeIsCalledFrom = null;
				return result;
			}

			return _outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls 
				? this[index]._outerKeySelectorFunc() 
				: _outerKeySelectorFunc(_outerSourceAsList[index]);
		}

		public TKey ApplyKeySelector(int index)
		{
			checkConsistent(null, null);
			return applyKeySelector(index);
		}

		private void processExpressionWatcherValueChanged(ExpressionWatcher expressionWatcher)
		{
			int outerSourceIndex = expressionWatcher._position.Index;
			JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> joinGroup = this[outerSourceIndex];
			joinGroup.Key = applyKeySelector(outerSourceIndex);
			joinGroup.setGroup(_grouping.getGroup(joinGroup.Key));
		}

		~GroupJoining()
		{
			if (_outerSourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_outerSourceAsList.CollectionChanged -= _outerSourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_outerSourceScalarWeakPropertyChangedEventHandler != null)
			{
				_outerSourceScalar.PropertyChanged -= _outerSourceScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_groupingWeakNotifyCollectionChangedEventHandler != null)
			{
				_grouping.CollectionChanged -= _groupingWeakNotifyCollectionChangedEventHandler.Handle;			
			}
		}

		// ReSharper disable once InconsistentNaming
		internal void ValidateConsistency()
		{
			_outerSourceItemPositions.ValidateConsistency();
			IList<TOuterSourceItem> outerSource = (IList<TOuterSourceItem>) _outerSourceScalar.getValue(_outerSource, new ObservableCollection<TOuterSourceItem>());
			IList<TInnerSourceItem> innerSource = (IList<TInnerSourceItem>) _grouping._sourceScalar.getValue(_grouping._source, new ObservableCollection<TInnerSourceItem>());

			IEqualityComparer<TKey> equalityComparer = _grouping._equalityComparerScalar.getValue(EqualityComparer<TKey>.Default, EqualityComparer<TKey>.Default);
			Dictionary<TKey, List<int>> keyIndices = new Dictionary<TKey, List<int>>(equalityComparer);
			List<int> nullKeyIndices = new List<int>();

			Func<TOuterSourceItem, TKey> outerKeySelector = _outerKeySelectorExpressionOriginal.Compile();

			if (_outerSourceItemPositions.List.Count != outerSource.Count)
				throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.5");

			Func<TInnerSourceItem, TKey> innerKeySelector = _grouping._keySelectorExpression.Compile();

			//result = outerSource.GroupJoin(innerSource, outerKeySelector, innerKeySelector, 
			//	(outerItem, innerItems) =>
		 //            new
		 //            {
		 //                Key = outerItem,
		 //                InnerItems = innerItems
		 //            }
			//	, equalityComparer).ToList();

			var result = outerSource.Select(oi => 
				new
				{
					Key = oi,
					InnerItems = innerSource.Where(ii => equalityComparer.Equals(outerKeySelector(oi), innerKeySelector(ii)))
				}).ToList();

			if (Count !=  result.Count())
				throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.1");

			for (int index = 0; index < result.Count; index++)
			{
				var resultItem = result[index];
				JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> thisItem = this[index];

				if (!ReferenceEquals(resultItem.Key, thisItem.OuterItem))
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.2");

				if (resultItem.InnerItems.Count() !=  thisItem.Count)
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.3");

				if (!resultItem.InnerItems.SequenceEqual(thisItem))
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.4");

				if (thisItem._outerSourceItemKeySelectorExpressionWatcher._position.Index != index)
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.9");

				List<int> indices;
				TKey key = outerKeySelector(outerSource[index]);
				if (key != null)
				{
					if (!keyIndices.TryGetValue(key, out indices))
					{
						indices = new List<int>();
						keyIndices[key] = indices;
					}
				}
				else
				{
					indices = nullKeyIndices;
				}

				indices.Add(index);
			}

			if (keyIndices.Count != _keyPositions.Count)
				throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.6");

			foreach (KeyValuePair<TKey, List<int>> keyValuePair in keyIndices)
			{
				List<Position> positions = _keyPositions[keyValuePair.Key];
				if (!positions.Select(p => p.Index).OrderBy(i => i).SequenceEqual(keyValuePair.Value))
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.7");
			}

			if (!_nullKeyPositions.Select(p => p.Index).OrderBy(i => i).SequenceEqual(nullKeyIndices))
				throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.8");
		}
	}

	public class JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> : CollectionComputingChild<TInnerSourceItem>
	{
		public TOuterSourceItem OuterItem
		{
			get => _outerItem;
			internal set
			{
				_outerItem = value;
				OnPropertyChanged(Utils.OuterItemPropertyChangedEventArgs);
			}
		}

		private TKey _key;
		public TKey Key
		{
			get => _key;
			internal set
			{
				_key = value;
				OnPropertyChanged(Utils.KeyPropertyChangedEventArgs);
			}
		}

		// ReSharper disable once UnusedAutoPropertyAccessor.Global
		// ReSharper disable once MemberCanBePrivate.Global
		private readonly GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> _groupJoining;

		public GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> GroupJoining => _groupJoining;

		Group<TInnerSourceItem, TKey> _group;
		internal readonly Position _outerSourceItemPosition;

		internal ExpressionWatcher _outerSourceItemKeySelectorExpressionWatcher;
		private TOuterSourceItem _outerItem;

		internal readonly Func<TKey> _outerKeySelectorFunc;

		internal JoinGroup(
			TOuterSourceItem outerSourceItem,
			GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> groupJoining,
			Group<TInnerSourceItem, TKey> group,
			Position outerSourceItemPosition,
			ExpressionWatcher outerSourceItemKeySelectorExpressionWatcher,
			TKey key,
			Func<TKey> outerKeySelectorFunc)
		{
			_outerItem = outerSourceItem;
			_groupJoining = groupJoining;
			_outerSourceItemPosition = outerSourceItemPosition;
			_outerSourceItemKeySelectorExpressionWatcher = outerSourceItemKeySelectorExpressionWatcher;
			_key = key;
			_outerKeySelectorFunc = outerKeySelectorFunc;
			_group = group;
			initializeFromGroup();
		}

		internal void setGroup(Group<TInnerSourceItem, TKey> group)
		{
			_group?._copies.Remove(this);
			_group = group;

			clearItems();
			initializeFromGroup();
		}

		private void initializeFromGroup()
		{
			if (_group != null)
			{
				int count = _group.Count;
				for (int index = 0; index < count; index++)
				{
					TInnerSourceItem innerSourceItem = _group[index];
					insertItem(index, innerSourceItem);
				}

				if (_group._copies == null) _group._copies = new List<CollectionComputingChild<TInnerSourceItem>>();
				_group._copies.Add(this);
			}
		}

		#region Overrides of ObservableCollection<TResult>

		protected override void InsertItem(int index, TInnerSourceItem item)
		{
			_groupJoining._insertItemIntoGroupAction(this, index, item);
		}

		protected override void MoveItem(int oldIndex, int newIndex)
		{
			_groupJoining._moveItemInGroupAction(this, oldIndex, newIndex);
		}

		protected override void RemoveItem(int index)
		{
			_groupJoining._removeItemFromGroupAction(this, index);
		}

		protected override void SetItem(int index, TInnerSourceItem item)
		{
			_groupJoining._setGroupItemAction(this, index, item);
		}

		protected override void ClearItems()
		{
			_groupJoining._clearGroupItemsAction(this);
		}
		#endregion

		public override ICollectionComputing Parent => _groupJoining;
	}
}
