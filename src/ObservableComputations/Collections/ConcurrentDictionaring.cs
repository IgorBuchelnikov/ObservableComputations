using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class ConcurrentDictionaring<TSourceItem, TKey, TValue> : IDictionary<TKey, TValue>, IHasSources, IComputing, INotifyMethodChanged
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TKey>> KeySelectorExpression => _keySelectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TKey> KeySelectorFunc => _keySelectorFunc;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TValue>> ValueSelectorExpression => _valueSelectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TValue> ValueSelectorFunc => _valueSelectorFunc;

		public IReadScalar<IEqualityComparer<TKey>> EqualityComparerScalar => _equalityComparerScalar;

		public IEqualityComparer<TKey> EqualityComparer => _equalityComparer;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public string InstantiatingStackTrace => _instantiatingStackTrace;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public string DebugTag { get; set; }
		public object Tag { get; set; }

		private ConcurrentDictionary<TKey, TValue> _dictionary;

		public bool IsConsistent => _isConsistent;

		public event EventHandler ConsistencyRestored;

		Dictionary<DictionaryChangeAction, object> _lockModifyChangeActionsKeys;
		private Dictionary<DictionaryChangeAction, object> lockModifyChangeActionsKeys => _lockModifyChangeActionsKeys = 
			_lockModifyChangeActionsKeys ?? new Dictionary<DictionaryChangeAction, object>();

		public void LockModifyChangeAction(DictionaryChangeAction collectionChangeAction, object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (!lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction))
				lockModifyChangeActionsKeys[collectionChangeAction] = key;
			else
				throw new ObservableComputationsException(this,
					$"Modifying of '{collectionChangeAction.ToString()}' change action is already locked. Unlock first.");
		}

		public void UnlockModifyChangeAction(DictionaryChangeAction collectionChangeAction, object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (!lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction))
				throw new ObservableComputationsException(this,
					"Modifying of '{collectionChangeAction.ToString()}' change action is not locked. Lock first.");

			if (ReferenceEquals(lockModifyChangeActionsKeys[collectionChangeAction], key))
				lockModifyChangeActionsKeys.Remove(collectionChangeAction);
			else
				throw new ObservableComputationsException(this,
					"Wrong key to unlock modifying of '{collectionChangeAction.ToString()}' change action.");
		}

		public bool IsModifyChangeActionLocked(DictionaryChangeAction collectionChangeAction)
		{
			return lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction);
		}

		private void checkLockModifyChangeAction(DictionaryChangeAction collectionChangeAction)
		{
			if (lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction))
				throw new ObservableComputationsException(this,
					"Modifying of '{collectionChangeAction.ToString()}' change action is locked. Unlock first.");
		}


		private Action<TKey, TValue> _addItemAction;
		public Action<TKey, TValue> InsertItemAction
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _addItemAction;
			set
			{
				if (_addItemAction != value)
				{
					checkLockModifyChangeAction(DictionaryChangeAction.AddItem);

					_addItemAction = value;
					onPropertyChanged(Utils.AddItemActionPropertyChangedEventArgs);
				}

			}
		}

		private Func<TKey, bool> _removeItemFunc;
		public Func<TKey, bool> RemoveItemFunc
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _removeItemFunc;
			set
			{
				if (_removeItemFunc != value)
				{
					checkLockModifyChangeAction(DictionaryChangeAction.RemoveItem);

					_removeItemFunc = value;
					onPropertyChanged(Utils.RemoveItemFuncPropertyChangedEventArgs);
				}
			}
		}

		private Action<TKey, TValue> _setItemAction;
		// ReSharper disable once MemberCanBePrivate.Global
		public Action<TKey, TValue> SetItemAction
		{
			get => _setItemAction;
			set
			{
				if (_setItemAction != value)
				{
					checkLockModifyChangeAction(DictionaryChangeAction.SetItem);

					_setItemAction = value;
					onPropertyChanged(Utils.SetItemActionPropertyChangedEventArgs);
				}
			}
		}

		private Action _clearItemsAction;

		// ReSharper disable once MemberCanBePrivate.Global
		public Action ClearItemsAction
		{
			get => _clearItemsAction;
			set
			{
				if (_clearItemsAction != value)
				{
					checkLockModifyChangeAction(DictionaryChangeAction.ClearItems);

					_clearItemsAction = value;
					onPropertyChanged(Utils.ClearItemsActionPropertyChangedEventArgs);
				}
			}
		}

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpression;
		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpressionOriginal;
		private readonly ExpressionWatcher.ExpressionInfo _keySelectorExpressionInfo;

		private readonly bool _keySelectorContainsParametrizedObservableComputationsCalls;

		private readonly Expression<Func<TSourceItem, TValue>> _valueSelectorExpression;
		private readonly Expression<Func<TSourceItem, TValue>> _valueSelectorExpressionOriginal;
		private readonly ExpressionWatcher.ExpressionInfo _valueSelectorExpressionInfo;

		private readonly bool _valueSelectorContainsParametrizedObservableComputationsCalls;

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
		private Queue<ExpressionWatcher> _deferredValueExpressionWatcherChangedProcessings;
		private Queue<ExpressionWatcher> _deferredKeyExpressionWatcherChangedProcessings;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Func<TSourceItem, TKey> _keySelectorFunc;
		private readonly Func<TSourceItem, TValue> _valueSelectorFunc;
		private INotifyCollectionChanged _source;
		private readonly string _instantiatingStackTrace;
		private bool _isConsistent = true;

		private IDispatcher _destinationDispatcher;
		private IConcurrentDictionaringDestinationDispatcher _concurrentDictionaringDestinationDispatcher;

		internal readonly IReadScalar<IEqualityComparer<TKey>> _equalityComparerScalar;
		internal IEqualityComparer<TKey> _equalityComparer;
		private PropertyChangedEventHandler _equalityComparerScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _equalityComparerScalarWeakPropertyChangedEventHandler;


		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global

		private sealed class ItemInfo : Position
		{
			public ExpressionWatcher KeyExpressionWatcher;
			public ExpressionWatcher ValueExpressionWatcher;
			public Func<TKey> _keySelectorFunc;
			public Func<TValue> _valueSelectorFunc;
			public TKey Key;
			public TValue Value;
		}


		private ConcurrentDictionaring(
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IDispatcher destinationDispatcher,
			int sourceCapacity)
		{
			_destinationDispatcher = destinationDispatcher;
			_concurrentDictionaringDestinationDispatcher = destinationDispatcher as IConcurrentDictionaringDestinationDispatcher;

			_itemInfos = new List<ItemInfo>(sourceCapacity);
			_sourcePositions = new Positions<ItemInfo>(_itemInfos);

			if (Configuration.SaveInstantiatingStackTrace)
			{
				_instantiatingStackTrace = Environment.StackTrace;
			}

			_keySelectorExpressionOriginal = keySelectorExpression;
			CallToConstantConverter callToConstantConverter = new CallToConstantConverter(_keySelectorExpressionOriginal.Parameters);
			_keySelectorExpression = (Expression<Func<TSourceItem, TKey>>) callToConstantConverter.Visit(_keySelectorExpressionOriginal);
			_keySelectorContainsParametrizedObservableComputationsCalls = callToConstantConverter.ContainsParametrizedObservableComputationCalls;

			if (!_keySelectorContainsParametrizedObservableComputationsCalls)
			{
				_keySelectorExpressionInfo = ExpressionWatcher.GetExpressionInfo(_keySelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				_keySelectorFunc = _keySelectorExpression.Compile();
			}

			_valueSelectorExpressionOriginal = valueSelectorExpression;
			callToConstantConverter = new CallToConstantConverter(_valueSelectorExpressionOriginal.Parameters);
			_valueSelectorExpression = (Expression<Func<TSourceItem, TValue>>) callToConstantConverter.Visit(_valueSelectorExpressionOriginal);
			_valueSelectorContainsParametrizedObservableComputationsCalls = callToConstantConverter.ContainsParametrizedObservableComputationCalls;

			if (!_valueSelectorContainsParametrizedObservableComputationsCalls)
			{
				_valueSelectorExpressionInfo = ExpressionWatcher.GetExpressionInfo(_valueSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				_valueSelectorFunc = _valueSelectorExpression.Compile();
			}
		}

		[ObservableComputationsCall]
		public ConcurrentDictionaring(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IDispatcher destinationDispatcher,
			IEqualityComparer<TKey> equalityComparer = null) : this(keySelectorExpression, valueSelectorExpression, destinationDispatcher, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
			_equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_dictionary = new ConcurrentDictionary<TKey, TValue>(1, capacity, _equalityComparer);

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ConcurrentDictionaring(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IDispatcher destinationDispatcher,
			IEqualityComparer<TKey> equalityComparer = null) : this(keySelectorExpression, valueSelectorExpression, destinationDispatcher, Utils.getCapacity(source))
		{
			_source = source;
			_equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_dictionary = new ConcurrentDictionary<TKey, TValue>(1, capacity, _equalityComparer);

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ConcurrentDictionaring(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IDispatcher destinationDispatcher,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar) : this(keySelectorExpression, valueSelectorExpression, destinationDispatcher, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
			_equalityComparerScalar = equalityComparerScalar;
			initializeEqualityComparer();

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_dictionary = new ConcurrentDictionary<TKey, TValue>(1, capacity, _equalityComparer);

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public ConcurrentDictionaring(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IDispatcher destinationDispatcher,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar) : this(keySelectorExpression, valueSelectorExpression, destinationDispatcher, Utils.getCapacity(source))
		{
			_source = source;
			_equalityComparerScalar = equalityComparerScalar;
			initializeEqualityComparer();

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_dictionary = new ConcurrentDictionary<TKey, TValue>(1, capacity, _equalityComparer);

			initializeFromSource();
		}

		private void initializeEqualityComparer()
		{
			if (_equalityComparerScalar != null)
			{
				_equalityComparerScalarPropertyChangedEventHandler = handleEqualityComparerScalarValueChanged;
				_equalityComparerScalarWeakPropertyChangedEventHandler =
					new WeakPropertyChangedEventHandler(_equalityComparerScalarPropertyChangedEventHandler);
				_equalityComparerScalar.PropertyChanged += _equalityComparerScalarWeakPropertyChangedEventHandler.Handle;
				_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TKey>.Default;
			}
			else
			{
				_equalityComparer = EqualityComparer<TKey>.Default;
			}
		}

		private void handleEqualityComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent();
			_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TKey>.Default;
			_isConsistent = false;
			initializeFromSource();
			_isConsistent = true;
			ConsistencyRestored?.Invoke(this, null);
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				int itemInfosCount = _itemInfos.Count;
				for (int index = 0; index < itemInfosCount; index++)
				{
					ItemInfo itemInfo = _itemInfos[index];
					disposeKeyExpressionWatcher(itemInfo);
					disposeValueExpressionWatcher(itemInfo);
				}

				int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
				_itemInfos = new List<ItemInfo>(capacity);
				_sourcePositions = new Positions<ItemInfo>(_itemInfos);

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

				_lastProcessedSourceChangeMarker = _sourceAsList.ChangeMarkerField;

				int count = _sourceAsList.Count;
				for (int index = 0; index < count; index++)
				{
					TSourceItem sourceItem = _sourceAsList[index];
					ItemInfo itemInfo = registerSourceItem(sourceItem, index);
					TKey key = applyKeySelector(itemInfo, sourceItem);
					TValue value = applyValueSelector(itemInfo, sourceItem);
					baseAddItem(key, value);
				}

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

		private ItemInfo registerSourceItem(TSourceItem sourceItem, int index, ItemInfo itemInfo = null)
		{
			itemInfo = itemInfo == null ? _sourcePositions.Insert(index) : _itemInfos[index];

			fillItemInfoWithKey(itemInfo, sourceItem);
			fillItemInfoWithValue(itemInfo, sourceItem);

			return itemInfo;
		}

		private void unregisterSourceItem(int index, bool replacing = false)
		{
			ItemInfo itemInfo = _itemInfos[index];
			disposeKeyExpressionWatcher(itemInfo);
			disposeValueExpressionWatcher(itemInfo);

			if (!replacing)
			{
				_sourcePositions.Remove(index);
			}
		}

		private void fillItemInfoWithValue(ItemInfo itemInfo, TSourceItem sourceItem)
		{
			ExpressionWatcher watcher;
			if (!_valueSelectorContainsParametrizedObservableComputationsCalls)
			{
				watcher = new ExpressionWatcher(_valueSelectorExpressionInfo, sourceItem);
			}
			else
			{
				Expression<Func<TValue>> deparametrizedSelectorExpression =
					(Expression<Func<TValue>>) _valueSelectorExpression.ApplyParameters(new object[] {sourceItem});
				Expression<Func<TValue>> selectorExpression =
					(Expression<Func<TValue>>)
					new CallToConstantConverter().Visit(deparametrizedSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				itemInfo._valueSelectorFunc = selectorExpression.Compile();
				watcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(selectorExpression));
			}

			watcher.ValueChanged = valueExpressionWatcher_OnValueChanged;
			watcher._position = itemInfo;
			itemInfo.ValueExpressionWatcher = watcher;
			itemInfo.Value = applyValueSelector(itemInfo, sourceItem);
		}

		private void fillItemInfoWithKey(ItemInfo itemInfo, TSourceItem sourceItem)
		{
			ExpressionWatcher watcher;
			if (!_keySelectorContainsParametrizedObservableComputationsCalls)
			{
				watcher = new ExpressionWatcher(_keySelectorExpressionInfo, sourceItem);
			}
			else
			{
				Expression<Func<TKey>> deparametrizedSelectorExpression =
					(Expression<Func<TKey>>) _keySelectorExpression.ApplyParameters(new object[] {sourceItem});
				Expression<Func<TKey>> selectorExpression =
					(Expression<Func<TKey>>)
					new CallToConstantConverter().Visit(deparametrizedSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				itemInfo._keySelectorFunc = selectorExpression.Compile();
				watcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(selectorExpression));
			}

			watcher.ValueChanged = keyExpressionWatcher_OnValueChanged;
			watcher._position = itemInfo;
			itemInfo.KeyExpressionWatcher = watcher;
			itemInfo.Key = applyKeySelector(itemInfo, sourceItem);
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();

			if (!_rootSourceWrapper && _lastProcessedSourceChangeMarker == _sourceAsList.ChangeMarkerField) return;
			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			TKey key;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					_isConsistent = false;
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem addedItem = _sourceAsList[newStartingIndex];
					ItemInfo itemInfo = registerSourceItem(addedItem, newStartingIndex);
					key = applyKeySelector(itemInfo, addedItem);
					TValue value = applyValueSelector(itemInfo, addedItem);
					baseAddItem(key, value);
					_isConsistent = true;
					ConsistencyRestored?.Invoke(this, null);
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					key = _itemInfos[oldStartingIndex].Key;
					unregisterSourceItem(oldStartingIndex);
					baseRemoveItem(key);
					break;
				case NotifyCollectionChangedAction.Replace:
					_isConsistent = false;
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem newItem = _sourceAsList[newStartingIndex1];
					ItemInfo replacingItemInfo = _itemInfos[newStartingIndex1];
					TKey oldKey = replacingItemInfo.Key;
					disposeKeyExpressionWatcher(replacingItemInfo);
					disposeValueExpressionWatcher(replacingItemInfo);
					fillItemInfoWithKey(replacingItemInfo, newItem);
					fillItemInfoWithValue(replacingItemInfo, newItem);

					TKey newKey = replacingItemInfo.Key;
					TValue newValue = replacingItemInfo.Value;

					if (Comparer.Equals(oldKey, newKey))
					{
						baseSetItem(replacingItemInfo.Key, newValue);
					}
					else
					{
						baseRemoveItem(oldKey);
						baseAddItem(replacingItemInfo.Key, newValue);
					}	
					_isConsistent = true;
					ConsistencyRestored?.Invoke(this, null);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 != newStartingIndex2)
					{
						_sourcePositions.Move(oldStartingIndex2, newStartingIndex2);
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					_isConsistent = false;
					initializeFromSource();
					_isConsistent = true;
					ConsistencyRestored?.Invoke(this, null);
					break;
			}

			_isConsistent = false;
			if (_deferredKeyExpressionWatcherChangedProcessings != null)
				while (_deferredKeyExpressionWatcherChangedProcessings.Count > 0)
				{
					ExpressionWatcher expressionWatcher = _deferredKeyExpressionWatcherChangedProcessings.Dequeue();
					if (!expressionWatcher._disposed)
						processKeyExpressionWatcherValueChanged(expressionWatcher);
				}
			

			if (_deferredValueExpressionWatcherChangedProcessings != null)
				while (_deferredValueExpressionWatcherChangedProcessings.Count > 0)
				{
					ExpressionWatcher expressionWatcher = _deferredValueExpressionWatcherChangedProcessings.Dequeue();
					if (!expressionWatcher._disposed)
						processValueExpressionWatcherValueChanged(expressionWatcher);
				}
			_isConsistent = true;
			ConsistencyRestored?.Invoke(this, null);
		}

		private void handleSourceScalarValueChanged(object sender,  PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			checkConsistent();
			_isConsistent = false;
			initializeFromSource();
			_isConsistent = true;
			ConsistencyRestored?.Invoke(this, null);
		}

		private void keyExpressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			if (_rootSourceWrapper || _sourceAsList.ChangeMarkerField ==_lastProcessedSourceChangeMarker)
			{
				_isConsistent = false;
				processKeyExpressionWatcherValueChanged(expressionWatcher);
				_isConsistent = true;
				ConsistencyRestored?.Invoke(this, null);
			}
			else
			{
				(_deferredKeyExpressionWatcherChangedProcessings = _deferredKeyExpressionWatcherChangedProcessings ??  new Queue<ExpressionWatcher>()).Enqueue(expressionWatcher);
			}
		}

		private void valueExpressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			checkConsistent();
			if (_rootSourceWrapper || _sourceAsList.ChangeMarkerField ==_lastProcessedSourceChangeMarker)
			{
				_isConsistent = false;
				processValueExpressionWatcherValueChanged(expressionWatcher);
				_isConsistent = true;
				ConsistencyRestored?.Invoke(this, null);
			}
			else
			{
				(_deferredValueExpressionWatcherChangedProcessings = _deferredValueExpressionWatcherChangedProcessings ??  new Queue<ExpressionWatcher>()).Enqueue(expressionWatcher);
			}
		}

		private void processKeyExpressionWatcherValueChanged(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
			disposeKeyExpressionWatcher(itemInfo);
			fillItemInfoWithKey(itemInfo, _sourceAsList[sourceIndex]);
			baseRemoveItem(key);
			TKey newKey = itemInfo.Key;
			baseAddItem(newKey, itemInfo.Value);
		}

		private void processValueExpressionWatcherValueChanged(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
			disposeValueExpressionWatcher(itemInfo);
			fillItemInfoWithValue(itemInfo, _sourceAsList[sourceIndex]);
			baseSetItem(key, itemInfo.Value);
		}

		private void disposeValueExpressionWatcher(ItemInfo itemInfo)
		{
			ExpressionWatcher watcher;
			watcher = itemInfo.ValueExpressionWatcher;
			watcher.Dispose();
		}

		private void disposeKeyExpressionWatcher(ItemInfo itemInfo)
		{
			ExpressionWatcher watcher = itemInfo.KeyExpressionWatcher;
			watcher.Dispose();
		}

		public TKey ApplyKeySelector(int index)
		{
			return applyKeySelector(_itemInfos[index], _sourceAsList[index]);
		}

		private TKey applyKeySelector(ItemInfo itemInfo, TSourceItem sourceItem)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				
				TKey result = _keySelectorContainsParametrizedObservableComputationsCalls ? itemInfo._keySelectorFunc() : _keySelectorFunc(sourceItem);

				if (computing == null) DebugInfo._computingsExecutingUserCode.Remove(currentThread);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				return result;
			}

			return _keySelectorContainsParametrizedObservableComputationsCalls ? itemInfo._keySelectorFunc() : _keySelectorFunc(sourceItem);
		}

		public TValue ApplyValueSelector(int index)
		{
			return applyValueSelector(_itemInfos[index], _sourceAsList[index]);
		}

		private TValue applyValueSelector(ItemInfo itemInfo, TSourceItem sourceItem)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				Thread currentThread = Thread.CurrentThread;
				DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out IComputing computing);
				DebugInfo._computingsExecutingUserCode[currentThread] = this;	
				
				TValue result = _valueSelectorContainsParametrizedObservableComputationsCalls ? itemInfo._valueSelectorFunc() : _valueSelectorFunc(sourceItem);

				if (computing == null) DebugInfo._computingsExecutingUserCode.Remove(currentThread);
				else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
				return result;
			}

			return _valueSelectorContainsParametrizedObservableComputationsCalls ? itemInfo._valueSelectorFunc() : _valueSelectorFunc(sourceItem);
		}

		private void baseClearItems()
		{
			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_dictionary = new ConcurrentDictionary<TKey, TValue>(1, capacity, _equalityComparer);

			void raiseEvents()
			{
				onPropertyChanged(Utils.CountPropertyChangedEventArgs);
				onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);

				if (MethodChanged != null)
				{
					MethodChanged(this, new NotifyMethodChangedEventArgs("GetValueOrDefault", args => true));
					MethodChanged(this, new NotifyMethodChangedEventArgs("Item[]", args => true));
					MethodChanged(this, new NotifyMethodChangedEventArgs("ContainsKey", args => true));
				}
			}

			if (_concurrentDictionaringDestinationDispatcher != null)
			{
				_concurrentDictionaringDestinationDispatcher.Invoke(raiseEvents, DictionaryChangeAction.ClearItems, null, null);
			}
			else
			{
				_destinationDispatcher.Invoke(raiseEvents);				
			}
		}

		private void baseAddItem(TKey key, TValue value)
		{
			if (!_dictionary.TryAdd(key, value))
			{
				throw new ObservableComputationsException(this, "An element with the same key already exists in the ConcurrentDictionaring");
			}

			void raiseEvents()
			{
				onPropertyChanged(Utils.CountPropertyChangedEventArgs);
				onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);

				if (MethodChanged != null)
				{
					MethodChanged(this,
						new NotifyMethodChangedEventArgs("GetValueOrDefault",
							args => _equalityComparer.Equals(key, (TKey) args[0])));
					MethodChanged(this,
						new NotifyMethodChangedEventArgs("Item[]", args => _equalityComparer.Equals(key, (TKey) args[0])));
					MethodChanged(this,
						new NotifyMethodChangedEventArgs("ContainsKey", args => _equalityComparer.Equals(key, (TKey) args[0])));
				}
			}

			if (_concurrentDictionaringDestinationDispatcher != null)
			{
				_concurrentDictionaringDestinationDispatcher.Invoke(raiseEvents, DictionaryChangeAction.AddItem, key, value);
			}
			else
			{
				_destinationDispatcher.Invoke(raiseEvents);				
			}
		}

		private void baseSetItem(TKey key, TValue value)
		{
			_dictionary[key] = value;


			void raiseEvents()
			{
				if (MethodChanged != null)
				{
					onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);

					MethodChanged(this,
						new NotifyMethodChangedEventArgs("GetValueOrDefault",
							args => _equalityComparer.Equals(key, (TKey) args[0])));
					MethodChanged(this,
						new NotifyMethodChangedEventArgs("Item[]", args => _equalityComparer.Equals(key, (TKey) args[0])));
					MethodChanged(this,
						new NotifyMethodChangedEventArgs("ContainsKey", args => _equalityComparer.Equals(key, (TKey) args[0])));
				}
			}

			if (_concurrentDictionaringDestinationDispatcher != null)
			{
				_concurrentDictionaringDestinationDispatcher.Invoke(raiseEvents, DictionaryChangeAction.SetItem, key, value);
			}
			else
			{
				_destinationDispatcher.Invoke(raiseEvents);				
			}

		}

		private void baseRemoveItem(TKey key)
		{
			_dictionary.TryRemove(key, out TValue value);

			void raiseEvents()
			{
				if (MethodChanged != null)
				{
					onPropertyChanged(Utils.CountPropertyChangedEventArgs);
					onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);

					MethodChanged(this,
						new NotifyMethodChangedEventArgs("GetValueOrDefault",
							args => _equalityComparer.Equals(key, (TKey) args[0])));
					MethodChanged(this,
						new NotifyMethodChangedEventArgs("Item[]", args => _equalityComparer.Equals(key, (TKey) args[0])));
					MethodChanged(this,
						new NotifyMethodChangedEventArgs("ContainsKey", args => _equalityComparer.Equals(key, (TKey) args[0])));
				}
			}


			if (_concurrentDictionaringDestinationDispatcher != null)
			{
				_concurrentDictionaringDestinationDispatcher.Invoke(raiseEvents, DictionaryChangeAction.RemoveItem, key, value);
			}
			else
			{
				_destinationDispatcher.Invoke(raiseEvents);				
			}
		}

		protected void checkConsistent()
		{
			if (!_isConsistent)
				throw new ObservableComputationsException(this,
					"The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.");
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public TValue GetValueOrDefault(TKey key, TValue defaultValue)
		{
			return _dictionary.TryGetValue(key, out TValue value) ? value : defaultValue;
		}

		public TValue GetValueOrDefault(TKey key)
		{
			return GetValueOrDefault(key, default(TValue));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void onPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}

		~ConcurrentDictionaring()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_sourceAsList.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}
		}

		public void ValidateConsistency()
		{
			_sourcePositions.ValidateConsistency();
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			if (_itemInfos.Count != source.Count) throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.1");
			if (Count != source.Count) throw new ObservableComputationsException( "Consistency violation: ConcurrentDictionaring.16");
			Func<TSourceItem, TKey> keySelector = _keySelectorExpression.Compile();
			Func<TSourceItem, TValue> valueSelector = _valueSelectorExpression.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_sourcePositions.List.Count != source.Count)
					throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.15");

				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];

					TKey key = keySelector(sourceItem);
					if (!ContainsKey(key))
						throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.2");

					TValue value = valueSelector(sourceItem);
					if (!this[key].IsSameAs(value))
						throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.3");

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.4");
					if (itemInfo.KeyExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.5");
					if (itemInfo.ValueExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.6");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.KeyExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.7");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.ValueExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.8");

					if (itemInfo.KeyExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.17");

					if (itemInfo.ValueExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.18");

					if (!itemInfo.Key.IsSameAs(key))
						throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.10");

					if (!itemInfo.Value.IsSameAs(value))
						throw new ObservableComputationsException("Consistency violation: ConcurrentDictionaring.9");
				}
			}			
		}

		#region IEnumerable<out T>

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion

		#region IEnumerable

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion

		#region Implementation of ICollection<KeyValuePair<TKey,TValue>>

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		public void Clear()
		{
			_clearItemsAction();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey,TValue>>) _dictionary).Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey,TValue>>) _dictionary).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (_dictionary.TryGetValue(item.Key, out TValue value))
			{
				if (!EqualityComparer<TValue>.Default.Equals(this._dictionary[item.Key], item.Value))
					return false;

				this.Remove(item.Key);
				return true;
			}

			return false;
		}

		public int Count => _dictionary.Count;
		public bool IsReadOnly => ((ICollection<KeyValuePair<TKey,TValue>>) _dictionary).IsReadOnly;

		#endregion

		#region Implementation of IDictionary<TKey,TValue>

		public void Add(TKey key, TValue value)
		{
			_addItemAction(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			return _removeItemFunc(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public TValue this[TKey key]
		{
			get => _dictionary[key];
			set => _setItemAction(key, value);
		}

		public ICollection<TKey> Keys { get; }
		public ICollection<TValue> Values { get; }

		#endregion

		#region Implementation of INotifyMethodChanged

		public event EventHandler<NotifyMethodChangedEventArgs> MethodChanged;

		#endregion
	}
}
