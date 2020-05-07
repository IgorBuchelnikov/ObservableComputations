using System;
using System.Collections;
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
	public class HashSetting<TSourceItem, TKey> : ICollection<TKey>, IHasSources, IComputing, INotifyMethodChanged
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TKey>> KeySelectorExpression => _keySelectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TKey> KeySelectorFunc => _keySelectorFunc;


		public IReadScalar<IEqualityComparer<TKey>> EqualityComparerScalar => _equalityComparerScalar;

		public IEqualityComparer<TKey> EqualityComparer => _equalityComparer;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public string InstantiatingStackTrace => _instantiatingStackTrace;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public string DebugTag { get; set; }
		public object Tag { get; set; }

		private HashSet<TKey> _hashSet;

		public bool IsConsistent => _isConsistent;

		public event EventHandler ConsistencyRestored;

		Dictionary<HashSetChangeAction, object> _lockModifyChangeActionsKeys;
		private Dictionary<HashSetChangeAction, object> lockModifyChangeActionsKeys => _lockModifyChangeActionsKeys = 
			_lockModifyChangeActionsKeys ?? new Dictionary<HashSetChangeAction, object>();

		public void LockModifyChangeAction(HashSetChangeAction collectionChangeAction, object key)
		{
			if (key == null) throw new ArgumentNullException("key");

			if (!lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction))
				lockModifyChangeActionsKeys[collectionChangeAction] = key;
			else
				throw new ObservableComputationsException(this,
					$"Modifying of '{collectionChangeAction.ToString()}' change action is already locked. Unlock first.");
		}

		public void UnlockModifyChangeAction(HashSetChangeAction collectionChangeAction, object key)
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

		public bool IsModifyChangeActionLocked(HashSetChangeAction collectionChangeAction)
		{
			return lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction);
		}

		private void checkLockModifyChangeAction(HashSetChangeAction collectionChangeAction)
		{
			if (lockModifyChangeActionsKeys.ContainsKey(collectionChangeAction))
				throw new ObservableComputationsException(this,
					"Modifying of '{collectionChangeAction.ToString()}' change action is locked. Unlock first.");
		}


		private Action<TKey> _addItemAction;
		public Action<TKey> AddItemAction
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _addItemAction;
			set
			{
				if (_addItemAction != value)
				{
					checkLockModifyChangeAction(HashSetChangeAction.AddItem);

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
					checkLockModifyChangeAction(HashSetChangeAction.RemoveItem);

					_removeItemFunc = value;
					onPropertyChanged(Utils.RemoveItemFuncPropertyChangedEventArgs);
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
					checkLockModifyChangeAction(HashSetChangeAction.ClearItems);

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

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
		private Queue<ExpressionWatcher> _deferredKeyExpressionWatcherChangedProcessings;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Func<TSourceItem, TKey> _keySelectorFunc;
		private INotifyCollectionChanged _source;
		private readonly string _instantiatingStackTrace;
		private bool _isConsistent = true;

		internal readonly IReadScalar<IEqualityComparer<TKey>> _equalityComparerScalar;
		internal IEqualityComparer<TKey> _equalityComparer;
		private PropertyChangedEventHandler _equalityComparerScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _equalityComparerScalarWeakPropertyChangedEventHandler;


		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global

		private sealed class ItemInfo : Position
		{
			public ExpressionWatcher KeyExpressionWatcher;
			public Func<TKey> _keySelectorFunc;
			public TKey Key;
		}


		private HashSetting(
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			int sourceCapacity)
		{
		
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
		}

		[ObservableComputationsCall]
		public HashSetting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IEqualityComparer<TKey> equalityComparer = null) : this(keySelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
			_equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_hashSet = new HashSet<TKey>(/*capacity, */_equalityComparer);

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public HashSetting(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IEqualityComparer<TKey> equalityComparer = null) : this(keySelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_hashSet = new HashSet<TKey>(/*capacity, */_equalityComparer);

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public HashSetting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar) : this(keySelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
			_equalityComparerScalar = equalityComparerScalar;
			initializeEqualityComparer();

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_hashSet = new HashSet<TKey>(/*capacity, */_equalityComparer);

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public HashSetting(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar) : this(keySelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_equalityComparerScalar = equalityComparerScalar;
			initializeEqualityComparer();

			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_hashSet = new HashSet<TKey>(/*capacity, */_equalityComparer);

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
					baseAddItem(key);
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

			return itemInfo;
		}

		private void unregisterSourceItem(int index, bool replacing = false)
		{
			ItemInfo itemInfo = _itemInfos[index];
			disposeKeyExpressionWatcher(itemInfo);

			if (!replacing)
			{
				_sourcePositions.Remove(index);
			}
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
					baseAddItem(key);
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
					fillItemInfoWithKey(replacingItemInfo, newItem);

					TKey newKey = replacingItemInfo.Key;

					baseRemoveItem(oldKey);
					baseAddItem(replacingItemInfo.Key);
					
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


		private void processKeyExpressionWatcherValueChanged(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
			disposeKeyExpressionWatcher(itemInfo);
			fillItemInfoWithKey(itemInfo, _sourceAsList[sourceIndex]);
			baseRemoveItem(key);
			TKey newKey = itemInfo.Key;
			baseAddItem(newKey);
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


		private void baseClearItems()
		{
			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_hashSet = new HashSet<TKey>(/*capacity, */_equalityComparer);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new NotifyMethodChangedEventArgs("Contains", args => true));
			}

		}

		private void baseAddItem(TKey key)
		{
			_hashSet.Add(key);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new NotifyMethodChangedEventArgs("Contains", args => _equalityComparer.Equals(key, (TKey)args[0])));
			}
		}

		private void baseRemoveItem(TKey key)
		{
			_hashSet.Remove(key);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new NotifyMethodChangedEventArgs("Contains", args => _equalityComparer.Equals(key, (TKey)args[0])));
			}
		}

		protected void checkConsistent()
		{
			if (!_isConsistent)
				throw new ObservableComputationsException(this,
					"The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.");
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void onPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
		}

		~HashSetting()
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
			if (_itemInfos.Count != source.Count) throw new ObservableComputationsException("Consistency violation: HashSetting.1");
			if (Count != source.Count) throw new ObservableComputationsException( "Consistency violation: HashSetting.16");
			Func<TSourceItem, TKey> keySelector = _keySelectorExpression.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_sourcePositions.List.Count != source.Count)
					throw new ObservableComputationsException("Consistency violation: HashSetting.15");

				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];

					TKey key = keySelector(sourceItem);
					if (!Contains(key))
						throw new ObservableComputationsException("Consistency violation: HashSetting.2");

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableComputationsException("Consistency violation: HashSetting.4");
					if (itemInfo.KeyExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: HashSetting.5");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.KeyExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: HashSetting.7");

					if (itemInfo.KeyExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: HashSetting.17");

					if (!itemInfo.Key.IsSameAs(key))
						throw new ObservableComputationsException("Consistency violation: HashSetting.10");
				}
			}			
		}

		#region IEnumerable<out T>

		public IEnumerator<TKey> GetEnumerator()
		{
			return _hashSet.GetEnumerator();
		}

		#endregion

		#region IEnumerable

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _hashSet.GetEnumerator();
		}

		#endregion

		#region Implementation of ICollection<TKey>

		public void Add(TKey item)
		{
			_addItemAction(item);
		}

		public void Clear()
		{
			_clearItemsAction();
		}

		public bool Contains(TKey item)
		{
			return ((ICollection<TKey>) _hashSet).Contains(item);
		}

		public void CopyTo(TKey[] array, int arrayIndex)
		{
			((ICollection<TKey>) _hashSet).CopyTo(array, arrayIndex);
		}

		public bool Remove(TKey item)
		{
			return _removeItemFunc(item);
		}

		public int Count => _hashSet.Count;
		public bool IsReadOnly => ((ICollection<TKey>) _hashSet).IsReadOnly;

		#endregion

		#region Implementation of INotifyMethodChanged

		public event EventHandler<NotifyMethodChangedEventArgs> MethodChanged;

		#endregion
	}

	public enum HashSetChangeAction
	{
		AddItem,
		RemoveItem,
		ClearItems,
	}
}
