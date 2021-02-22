// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> : CollectionComputing<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>>, IHasSourceCollections, ISourceItemChangeProcessor, ISourceCollectionChangeProcessor
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

		public virtual ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{OuterSource, InnerSource});
		public virtual ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{OuterSourceScalar, InnerSourceScalar});

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, TInnerSourceItem> InsertItemIntoGroupRequestHandler
		{
			get => _insertItemIntoGroupRequestHandler;
			set
			{
				if (_insertItemIntoGroupRequestHandler != value)
				{
					_insertItemIntoGroupRequestHandler = value;
					OnPropertyChanged(Utils.InsertItemIntoGroupRequestHandlerPropertyChangedEventArgs);
				}

			}
		}

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int> RemoveItemFromGroupRequestHandler
		{
			get => _removeItemFromGroupRequestHandler;
			set
			{
				if (_removeItemFromGroupRequestHandler != value)
				{
					_removeItemFromGroupRequestHandler = value;
					OnPropertyChanged(Utils.RemoveItemFromGroupRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, int> MoveItemInGroupRequestHandler
		{
			get => _moveItemInGroupRequestHandler;
			set
			{
				if (_moveItemInGroupRequestHandler != value)
				{
					_moveItemInGroupRequestHandler = value;
					OnPropertyChanged(Utils.MoveItemInGroupRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>> ClearGroupItemsRequestHandler
		{
			get => _clearGroupItemsRequestHandler;
			set
			{
				if (_clearGroupItemsRequestHandler != value)
				{
					_clearGroupItemsRequestHandler = value;
					OnPropertyChanged(Utils.ClearGroupItemsRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		public Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, TInnerSourceItem> SetGroupItemRequestHandler
		{
			get => _setGroupItemRequestHandler;
			set
			{
				if (_setGroupItemRequestHandler != value)
				{
					_setGroupItemRequestHandler = value;
					OnPropertyChanged(Utils.SetGroupItemRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		private readonly Func<TOuterSourceItem, TKey> _outerKeySelectorFunc;
		private readonly Expression<Func<TOuterSourceItem, TKey>> _outerKeySelectorExpression;

		// ReSharper disable once MemberCanBePrivate.Global

		private readonly ExpressionWatcher.ExpressionInfo _outerKeySelectorExpressionInfo;
		private readonly bool _outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls;

		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, TInnerSourceItem> _insertItemIntoGroupRequestHandler;
		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int> _removeItemFromGroupRequestHandler;
		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, TInnerSourceItem> _setGroupItemRequestHandler;
		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>, int, int> _moveItemInGroupRequestHandler;
		internal Action<JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>> _clearGroupItemsRequestHandler;

		Grouping<TInnerSourceItem, TKey> _grouping;

		private ObservableCollectionWithChangeMarker<TOuterSourceItem> _outerSourceAsList;
		bool _outerRootSourceWrapper;

		private NotifyCollectionChangedEventHandler _groupingNotifyCollectionChangedEventHandler;

		Positions<OuterItemInfo> _outerSourceItemPositions;
		private List<OuterItemInfo> _itemInfos;
		Dictionary<TKey, List<OuterItemInfo>> _keyPositions;
		readonly List<OuterItemInfo> _nullKeyPositions = new List<OuterItemInfo>();

		private bool _lastProcessedSourceChangeMarker;
		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
		private readonly IReadScalar<INotifyCollectionChanged> _outerSourceScalar;
		private INotifyCollectionChanged _outerSource;
		private readonly Expression<Func<TOuterSourceItem, TKey>> _outerKeySelectorExpressionOriginal;

		private readonly IReadScalar<INotifyCollectionChanged> _innerSourceScalar;
		private readonly INotifyCollectionChanged _innerSource;
		private readonly Expression<Func<TInnerSourceItem, TKey>> _innerKeySelector;
		readonly IReadScalar<IEqualityComparer<TKey>> _equalityComparerScalar;
		private readonly IEqualityComparer<TKey> _equalityComparer;

		private readonly List<IComputingInternal> _nestedComputings;
		private int _outerKeySelectorExpressionCallCount;

		private readonly ISourceItemChangeProcessor _thisAsSourceItemChangeProcessor;

		private sealed class OuterItemInfo : ExpressionItemInfo
		{
			public Func<TKey> SelectorFunc;
		}

		[ObservableComputationsCall]
		public GroupJoining(
			IReadScalar<INotifyCollectionChanged> outerSourceScalar,
			IReadScalar<INotifyCollectionChanged> innerSourceScalar,
			Expression<Func<TOuterSourceItem, TKey>> outerKeySelector,
			Expression<Func<TInnerSourceItem, TKey>> innerKeySelector,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar = null) : this(outerKeySelector, Utils.getCapacity(outerSourceScalar))
		{
			_outerSourceScalar = outerSourceScalar;
			_innerSourceScalar = innerSourceScalar;
			_innerKeySelector = innerKeySelector;
			_equalityComparerScalar = equalityComparerScalar;
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
			_innerSource = innerSource;
			_innerKeySelector = innerKeySelector;
			_equalityComparerScalar = equalityComparerScalar;
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
			_innerSource = innerSource;
			_innerKeySelector = innerKeySelector;
			_equalityComparer = equalityComparer;
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
			_innerSourceScalar = innerSourceScalar;
			_innerKeySelector = innerKeySelector;
			_equalityComparer = equalityComparer;
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
			_innerSourceScalar = innerSourceScalar;
			_innerKeySelector = innerKeySelector;
			_equalityComparerScalar = equalityComparerScalar;
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
			_innerSource = innerSource;
			_innerKeySelector = innerKeySelector;
			_equalityComparerScalar = equalityComparerScalar;
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
			_innerSource = innerSource;
			_innerKeySelector = innerKeySelector;
			_equalityComparer = equalityComparer;
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
			_innerSourceScalar = innerSourceScalar;
			_innerKeySelector = innerKeySelector;
			_equalityComparer = equalityComparer;
		}

		protected override void initialize()
		{
			if (_innerSourceScalar != null)
			{
				if (_equalityComparerScalar != null)
					_grouping = _innerSourceScalar.Grouping(_innerKeySelector, _equalityComparerScalar);
				else
					_grouping = _innerSourceScalar.Grouping(_innerKeySelector, _equalityComparer);
			}
			else
			{
				if (_equalityComparerScalar != null)
					_grouping = _innerSource.Grouping(_innerKeySelector, _equalityComparerScalar);
				else
					_grouping = _innerSource.Grouping(_innerKeySelector, _equalityComparer);			   
			}

			((IComputingInternal) _grouping).AddDownstreamConsumedComputing(this);
			subscribeToGroupingCollectionChanged();

			_keyPositions = new Dictionary<TKey, List<OuterItemInfo>>(_grouping._equalityComparer);

			Utils.initializeSourceScalar(_outerSourceScalar, ref _outerSource, scalarValueChangedHandler);
			Utils.initializeNestedComputings(_nestedComputings, this);
		}

		protected override void uninitialize()
		{
			if (_groupingNotifyCollectionChangedEventHandler != null)
				_grouping.CollectionChanged -= _groupingNotifyCollectionChangedEventHandler;	
			((IComputingInternal) _grouping).RemoveDownstreamConsumedComputing(this);

			Utils.unsubscribeSourceScalar(_outerSourceScalar, scalarValueChangedHandler);
			Utils.uninitializeNestedComputings(_nestedComputings, this);
		}

		protected override void clearCachedScalarArgumentValues()
		{
			Utils.clearCachcedSourceScalarValue(_outerSourceScalar, ref _outerSource);
		}

		protected override void processSource()
		{
			processSource(true);
		}

		private void processSource(bool replaceSource)
		{
			int originalCount = _items.Count;

			if (_sourceReadAndSubscribed)
			{
				Utils.disposeExpressionItemInfos(_itemInfos, _outerKeySelectorExpressionCallCount, this);
				Utils.removeDownstreamConsumedComputing(_itemInfos, this);

				Utils.disposeSource(
					_outerSourceScalar, 
					_outerSource,
					out _itemInfos,
					out _outerSourceItemPositions, 
					_outerSourceAsList, 
					handleOuterSourceCollectionChanged,
					replaceSource);

				_nullKeyPositions.Clear();
				_keyPositions = new Dictionary<TKey, List<OuterItemInfo>>(_grouping._equalityComparer);
				_sourceReadAndSubscribed = false;
			}

			if (replaceSource)
				Utils.replaceSource(ref _outerSource, _outerSourceScalar, _downstreamConsumedComputings, _consumers, this, out _outerSourceAsList, false);

			if (_outerSource != null && _isActive)
			{
				if (replaceSource)
					Utils.subscribeSource(
						_outerSource, 
						ref _outerSourceAsList, 
						ref _outerRootSourceWrapper, 
						ref _lastProcessedSourceChangeMarker,
						handleOuterSourceCollectionChanged);

				int count = _outerSourceAsList.Count;

				TOuterSourceItem[] outerSourceCopy = new TOuterSourceItem[count];
				_outerSourceAsList.CopyTo(outerSourceCopy, 0);

				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
					registerOuterSourceItem(outerSourceCopy[sourceIndex], sourceIndex, originalCount);
				

				for (int index = originalCount - 1; index >= sourceIndex; index--)
					_items.RemoveAt(index);				

				_sourceReadAndSubscribed = true;
			}
			else
				_items.Clear();

			reset();
		}

		private void subscribeToGroupingCollectionChanged()
		{
			_groupingNotifyCollectionChangedEventHandler = handleGroupingCollectionChanged;
			_grouping.CollectionChanged += _groupingNotifyCollectionChangedEventHandler;
		}

		private GroupJoining(Expression<Func<TOuterSourceItem, TKey>> outerKeySelector, int outerSourceCapacity) : base(outerSourceCapacity)
		{
			Utils.construct(
				outerKeySelector, 
				outerSourceCapacity, 
				out _itemInfos, 
				out _outerSourceItemPositions, 
				out _outerKeySelectorExpressionOriginal, 
				out _outerKeySelectorExpression, 
				out _outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls, 
				ref _outerKeySelectorExpressionInfo, 
				ref _outerKeySelectorExpressionCallCount, 
				ref _outerKeySelectorFunc, 
				ref _nestedComputings);

			_deferredQueuesCount = 3;
			
			_thisAsSourceCollectionChangeProcessor = this;
			_thisAsSourceItemChangeProcessor = this;
		}

		List<OuterItemInfo> getPositionsByKey(TKey key)
		{
			return
				key != null
					? _keyPositions.TryGetValue(key, out List<OuterItemInfo> positions)
						? positions
						: null
					: _nullKeyPositions;	   
		}


		private void handleGroupingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
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

		private void handleOuterSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				_outerRootSourceWrapper,				 
				ref _lastProcessedSourceChangeMarker, 
				_outerSourceAsList, 
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
			if (ReferenceEquals(sender, _outerSourceAsList))
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						TOuterSourceItem addedItem = (TOuterSourceItem) e.NewItems[0];
						registerOuterSourceItem(addedItem, e.NewStartingIndex);
						break;
					case NotifyCollectionChangedAction.Remove:
						unregisterOuterSourceItem(e.OldStartingIndex);

						break;
					case NotifyCollectionChangedAction.Replace:
						int newIndex1 = e.NewStartingIndex;
						TOuterSourceItem newItem = (TOuterSourceItem) e.NewItems[0];
						JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> replacingJoinGroup = this[newIndex1];
						OuterItemInfo itemInfo = _itemInfos[newIndex1];

						Utils.disposeExpressionWatcher(itemInfo.ExpressionWatcher, itemInfo.NestedComputings, this,
							_outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls);

						Utils.getItemInfoContent(
							new object[] {newItem},
							out ExpressionWatcher watcher,
							out Func<TKey> outerKeySelectorFunc,
							out List<IComputingInternal> nestedComputings,
							_outerKeySelectorExpression,
							out _outerKeySelectorExpressionCallCount,
							this,
							_outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls,
							_outerKeySelectorExpressionInfo);

						watcher.ValueChanged = expressionWatcher_OnValueChanged;

						watcher._position = itemInfo;

						TKey key = applyKeySelector(newItem, outerKeySelectorFunc);

						if (!_grouping._equalityComparer.Equals(key, replacingJoinGroup.Key))
						{
							unregisterKey(newIndex1, itemInfo);
							registerKey(key, itemInfo);
							replacingJoinGroup.Key = key;
							replacingJoinGroup.setGroup(_grouping.getGroup(key));
						}

						replacingJoinGroup.OuterItem = newItem;
						itemInfo.ExpressionWatcher = watcher;
						itemInfo.SelectorFunc = outerKeySelectorFunc;
						itemInfo.NestedComputings = nestedComputings;
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex2 = e.OldStartingIndex;
						int newStartingIndex2 = e.NewStartingIndex;
						if (oldStartingIndex2 != newStartingIndex2)
						{
							_outerSourceItemPositions.Move(oldStartingIndex2, newStartingIndex2);
							baseMoveItem(oldStartingIndex2, newStartingIndex2);
						}

						break;
					case NotifyCollectionChangedAction.Reset:
						processSource(false);
						break;
				}
			else //if (ReferenceEquals(sender, _grouping)
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						Group<TInnerSourceItem, TKey> addedGroup 
							= (Group<TInnerSourceItem, TKey>) e.NewItems[0];

						List<OuterItemInfo> positions = getPositionsByKey(addedGroup._key);
						if (positions != null)
						{
							int positionsCount = positions.Count;
							for (int index = 0; index < positionsCount; index++)
								this[positions[index].Index].setGroup(addedGroup);						
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						Group<TInnerSourceItem, TKey> removedGroup 
							= (Group<TInnerSourceItem, TKey>) e.OldItems[0];

						List<OuterItemInfo> positions1 = getPositionsByKey(removedGroup._key);
						if (positions1 != null)
						{
							int positions1Count = positions1.Count;
							for (int index = 0; index < positions1Count; index++)
								this[positions1[index].Index].setGroup(null);							
						}
						break;
					case NotifyCollectionChangedAction.Reset:
						processSource(false);
						break;
				}
		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			Utils.AddDownstreamConsumedComputing(computing, _outerSourceScalar, _outerSource);
			(_equalityComparerScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			Utils.RemoveDownstreamConsumedComputing(computing, _outerSourceScalar, _outerSource);
			(_equalityComparerScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			Utils.processSourceItemChange(
				expressionWatcher, 
				sender, 
				eventArgs, 
				_outerRootSourceWrapper, 
				_outerSourceAsList,
				_lastProcessedSourceChangeMarker, 
				_thisAsSourceItemChangeProcessor,
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings, 
				2, _deferredQueuesCount, this);
		}

		private void unregisterOuterSourceItem(int index)
		{
			OuterItemInfo itemInfo = _itemInfos[index];

			unregisterKey(index, itemInfo);

			Utils.disposeExpressionWatcher(itemInfo.ExpressionWatcher, itemInfo.NestedComputings, this,
				_outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls);

			_outerSourceItemPositions.Remove(index);
			//this[position.Index].Group.JoinGroupsPositions.Remove(position);

			baseRemoveItem(index);
		}

		private void registerOuterSourceItem(TOuterSourceItem outerSourceItem, int index, int? originalCount = null)
		{
			Utils.getItemInfoContent(
				new object[]{outerSourceItem}, 
				out ExpressionWatcher watcher, 
				out Func<TKey> outerKeySelectorFunc, 
				out List<IComputingInternal> nestedComputings,
				_outerKeySelectorExpression,
				out _outerKeySelectorExpressionCallCount,
				this,
				_outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls,
				_outerKeySelectorExpressionInfo);

			TKey key = applyKeySelector(outerSourceItem, outerKeySelectorFunc);

			Group<TInnerSourceItem, TKey> group = _grouping.getGroup(key);

			OuterItemInfo outerSourceItemPosition = _outerSourceItemPositions.Insert(index);
			outerSourceItemPosition.NestedComputings = nestedComputings;
			outerSourceItemPosition.ExpressionWatcher = watcher;
			outerSourceItemPosition.SelectorFunc = outerKeySelectorFunc;
			JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> joinGroup = new JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey>(
				outerSourceItem, this, group, key);
			//group.JoinGroupsPositions.Add(outerSourceItemPosition);

			watcher.ValueChanged = expressionWatcher_OnValueChanged;
			watcher._position = outerSourceItemPosition;
			
			registerKey(key, outerSourceItemPosition);

			if (originalCount == null)
			{
				baseInsertItem(index, joinGroup);
			}
			else
			{
				if (originalCount > index)
					_items[index] = joinGroup;
				else
					_items.Insert(index, joinGroup);			
			}

		}
		
		private void unregisterKey(int index, OuterItemInfo position)
		{
			TKey key = this[index].Key;
			List<OuterItemInfo> keyPositions = getPositionsByKey(key);
			keyPositions.Remove(position);

			if (keyPositions.Count == 0 && key != null) _keyPositions.Remove(key);
		}

		private void registerKey(TKey key, OuterItemInfo outerSourceItemPosition)
		{
			List<OuterItemInfo> keyPositions = getPositionsByKey(key);
			if (keyPositions == null)
			{
				keyPositions = new List<OuterItemInfo>();
				_keyPositions.Add(key, keyPositions);
			}

			keyPositions.Add(outerSourceItemPosition);
		}		

		private TKey applyKeySelector(TOuterSourceItem outerSourceItem, Func<TKey> outerSelectorFunc)
		{
			TKey getValue() =>
				_outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls 
					? outerSelectorFunc() 
					: _outerKeySelectorFunc(outerSourceItem);

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TKey result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		private TKey applyKeySelector(int index)
		{
			TKey getValue()
			{
				return _outerKeySelectorExpressionContainsParametrizedObservableComputationsCalls 
					? _itemInfos[index].SelectorFunc()
					: _outerKeySelectorFunc(_outerSourceAsList[index]);
			}

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TKey result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		public TKey ApplyKeySelector(int index)
		{
			return applyKeySelector(index);
		}

		void ISourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			if (expressionWatcher._disposed) return;
			int outerSourceIndex = expressionWatcher._position.Index;
			JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> joinGroup = this[outerSourceIndex];
			OuterItemInfo outerItemInfo = _itemInfos[outerSourceIndex];
			unregisterKey(outerSourceIndex, outerItemInfo);
			joinGroup.Key = applyKeySelector(joinGroup._outerItem, outerItemInfo.SelectorFunc);
			registerKey(joinGroup.Key, outerItemInfo);
			joinGroup.setGroup(_grouping.getGroup(joinGroup.Key));
		}

		// ReSharper disable once InconsistentNaming
		[ExcludeFromCodeCoverage]
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
		 //			new
		 //			{
		 //				Key = outerItem,
		 //				InnerItems = innerItems
		 //			}
			//	, equalityComparer).ToList();

			var result = outerSource.Select(oi => 
				new
				{
					Key = oi,
					InnerItems = innerSource.Where(ii => equalityComparer.Equals(outerKeySelector(oi), innerKeySelector(ii)))
				}).ToList();

			var groups = innerSource.GroupBy(innerKeySelector, equalityComparer).ToArray();

			if (Count !=  result.Count)
				throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.1");

			for (int index = 0; index < result.Count; index++)
			{
				var resultItem = result[index];
				JoinGroup<TOuterSourceItem, TInnerSourceItem, TKey> thisItem = this[index];
				OuterItemInfo itemInfo = _itemInfos[index];

				if (!ReferenceEquals(resultItem.Key, thisItem.OuterItem))
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.2");

				if (resultItem.InnerItems.Count() !=  thisItem.Count)
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.3");

				if (!resultItem.InnerItems.SequenceEqual(thisItem))
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.4");

				if (itemInfo.ExpressionWatcher._position.Index != index)
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

				int? groupIndex = null;
				for (int i = 0; i < groups.Length; i++)
				{
					if (equalityComparer.Equals(groups[i].Key, key))
					{
						groupIndex = i;
					}
				}

				if (groupIndex != thisItem.InnserSourceGroupIndex)
					throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.10");
			}

			if (keyIndices.Count != _keyPositions.Count)
				throw new ObservableComputationsException(this, "Consistency violation: GroupJoining.6");

			foreach (KeyValuePair<TKey, List<int>> keyValuePair in keyIndices)
			{
				List<OuterItemInfo> positions = _keyPositions[keyValuePair.Key];
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

		internal TOuterSourceItem _outerItem;

		internal JoinGroup(
			TOuterSourceItem outerSourceItem,
			GroupJoining<TOuterSourceItem, TInnerSourceItem, TKey> groupJoining,
			Group<TInnerSourceItem, TKey> group,
			TKey key)
		{
			_outerItem = outerSourceItem;
			_groupJoining = groupJoining;
			_key = key;
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
					insertItem(index, _group[index]);

				if (_group._copies == null) _group._copies = new List<CollectionComputingChild<TInnerSourceItem>>();
				_group._copies.Add(this);
			}
		}

		#region Overrides of ObservableCollection<TResult>

		protected override void InsertItem(int index, TInnerSourceItem item)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_groupJoining._insertItemIntoGroupRequestHandler(this, index, item);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_groupJoining._insertItemIntoGroupRequestHandler(this, index, item);
		}

		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_groupJoining._moveItemInGroupRequestHandler(this, oldIndex, newIndex);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_groupJoining._moveItemInGroupRequestHandler(this, oldIndex, newIndex);
		}

		protected override void RemoveItem(int index)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_groupJoining._removeItemFromGroupRequestHandler(this, index);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_groupJoining._removeItemFromGroupRequestHandler(this, index);
		}

		protected override void SetItem(int index, TInnerSourceItem item)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_groupJoining._setGroupItemRequestHandler(this, index, item);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_groupJoining._setGroupItemRequestHandler(this, index, item);
		}

		protected override void ClearItems()
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_groupJoining._clearGroupItemsRequestHandler(this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_groupJoining._clearGroupItemsRequestHandler(this);
		}
		#endregion

		public override ICollectionComputing Parent => _groupJoining;

		public ReadOnlyCollection<int> InnerSourceItemIndices => 
			_group != null ? _group.SourceItemIndices : new ReadOnlyCollection<int>(new List<int>());

		public int? InnserSourceGroupIndex => _group?.Index;
	}
}
