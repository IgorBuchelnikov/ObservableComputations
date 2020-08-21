using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class HashSetting<TSourceItem, TKey> : ICollection<TKey>, IHasSourceCollections, IComputing, INotifyMethodChanged, ICanProcessSourceItemChange
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

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public string DebugTag { get; set; }
		public object Tag { get; set; }

		private HashSet<TKey> _hashSet;

		public bool IsConsistent => _isConsistent;

		public event EventHandler ConsistencyRestored;

        private List<IComputingInternal> _keyNestedComputings;

        private ICanProcessSourceItemChange _thisAsCanProcessSourceKeyItemChange;


		private Action<TKey> _addItemAction;
		public Action<TKey> AddItemAction
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _addItemAction;
			set
			{
				if (_addItemAction != value)
				{
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
        private int _keySelectorExpressionСallCount;

		private readonly bool _keySelectorContainsParametrizedObservableComputationsCalls;


		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
		private Queue<ExpressionWatcher.Raise> _deferredKeyExpressionWatcherChangedProcessings;

		private bool _sourceInitialized;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Func<TSourceItem, TKey> _keySelectorFunc;
		private INotifyCollectionChanged _source;
		private readonly string _instantiatingStackTrace;
		private bool _isConsistent = true;

		internal readonly IReadScalar<IEqualityComparer<TKey>> _equalityComparerScalar;
		internal IEqualityComparer<TKey> _equalityComparer;

		private IComputing _userCodeIsCalledFrom;
		public IComputing UserCodeIsCalledFrom => _userCodeIsCalledFrom;

		internal object _handledEventSender;
		internal EventArgs _handledEventArgs;
		public object HandledEventSender => _handledEventSender;
		public EventArgs HandledEventArgs => _handledEventArgs;

		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once UnusedAutoPropertyAccessor.Global

		private sealed class ItemInfo : ExpressionItemInfo
		{
			public Func<TKey> _keySelectorFunc;
			public TKey Key;
		}

		private HashSetting(
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			int sourceCapacity)
		{
            if (Configuration.SaveInstantiatingStackTrace) _instantiatingStackTrace = Environment.StackTrace;

            Utils.construct(sourceCapacity, out _itemInfos, out _sourcePositions);

            Utils.construct(
                keySelectorExpression, 
                ref _keySelectorExpressionOriginal, 
                ref _keySelectorExpression, 
                ref _keySelectorContainsParametrizedObservableComputationsCalls, 
                ref _keySelectorExpressionInfo, 
                ref _keySelectorExpressionСallCount, 
                ref _keySelectorFunc, 
                ref _keyNestedComputings);


            _thisAsCanProcessSourceKeyItemChange = this;
        }

		[ObservableComputationsCall]
		public HashSetting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IEqualityComparer<TKey> equalityComparer = null) : this(keySelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public HashSetting(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IEqualityComparer<TKey> equalityComparer = null) : this(keySelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;
		}

		[ObservableComputationsCall]
		public HashSetting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar) : this(keySelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public HashSetting(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar) : this(keySelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_equalityComparerScalar = equalityComparerScalar;
		}

		private void initializeEqualityComparer()
		{
			if (_equalityComparerScalar != null)
			{
				_equalityComparerScalar.PropertyChanged += handleEqualityComparerScalarValueChanged;
				_equalityComparer = _equalityComparerScalar.Value;
			}
			
            if (_equalityComparer == null)
				_equalityComparer = EqualityComparer<TKey>.Default;
		}

		private void handleEqualityComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			_equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TKey>.Default;
			_isConsistent = false;
			initializeFromSource();
			_isConsistent = true;
			ConsistencyRestored?.Invoke(this, null);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void initializeFromSource()
		{
			if (_sourceInitialized)
			{
				Utils.disposeExpressionItemInfos(
                    _itemInfos,
                    _keySelectorExpressionСallCount,
                    this);

                Utils.disposeSource(
                    _sourceScalar, 
                    _source,
                    ref _itemInfos,
                    ref _sourcePositions, 
                    _sourceAsList, 
                    handleSourceCollectionChanged);

				baseClearItems();
                _sourceInitialized = false;
            }

            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this, ref _sourceAsList, false);

			if (_source != null && _isActive)
			{
                Utils.initializeFromObservableCollectionWithChangeMarker(
                    _source, 
                    ref _sourceAsList, 
                    ref _rootSourceWrapper, 
                    ref _lastProcessedSourceChangeMarker);

				int count = _sourceAsList.Count;
				for (int index = 0; index < count; index++)
				{
					TSourceItem sourceItem = _sourceAsList[index];
					ItemInfo itemInfo = registerSourceItem(sourceItem, index);
					TKey key = applyKeySelector(itemInfo, sourceItem);
					baseAddItem(key);
				}

                _sourceAsList.CollectionChanged += handleSourceCollectionChanged;
                _sourceInitialized = true;
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

			if (!replacing) _sourcePositions.Remove(index);
        }


		private void fillItemInfoWithKey(ItemInfo itemInfo, TSourceItem sourceItem)
		{
            Utils.getItemInfoContent(
                new object[]{sourceItem}, 
                out ExpressionWatcher watcher,
                out Func<TKey> func,
                out List<IComputingInternal> nestedComputings,
                _keySelectorExpression,
                ref _keySelectorExpressionСallCount,
                this,
                _keySelectorContainsParametrizedObservableComputationsCalls,
                _keySelectorExpressionInfo);

			watcher.ValueChanged = keyExpressionWatcher_OnValueChanged;
			watcher._position = itemInfo;
			itemInfo.ExpressionWatcher = watcher;
			itemInfo.Key = applyKeySelector(itemInfo, sourceItem);
            itemInfo._keySelectorFunc = func;
            itemInfo.NestedComputings = nestedComputings;
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
					_isConsistent = false;
					int oldStartingIndex = e.OldStartingIndex;
					key = _itemInfos[oldStartingIndex].Key;
					unregisterSourceItem(oldStartingIndex);
					baseRemoveItem(key);
					_isConsistent = true;
					ConsistencyRestored?.Invoke(this, null);
					break;
				case NotifyCollectionChangedAction.Replace:
					_isConsistent = false;
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem newItem = _sourceAsList[newStartingIndex1];
					ItemInfo replacingItemInfo = _itemInfos[newStartingIndex1];
					TKey oldKey = replacingItemInfo.Key;
					disposeKeyExpressionWatcher(replacingItemInfo);
					fillItemInfoWithKey(replacingItemInfo, newItem);
                    replacingItemInfo.Key = applyKeySelector(replacingItemInfo, newItem);
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

            Utils.doDeferredExpressionWatcherChangedProcessings(
                _deferredKeyExpressionWatcherChangedProcessings, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                _thisAsCanProcessSourceKeyItemChange,
                ref _isConsistent,
                false); 

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void keyExpressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
            Utils.ProcessSourceItemChange(
                expressionWatcher, 
                sender, 
                eventArgs, 
                _rootSourceWrapper, 
                _sourceAsList, 
                _lastProcessedSourceChangeMarker, 
                _thisAsCanProcessSourceKeyItemChange,
                ref _deferredKeyExpressionWatcherChangedProcessings, 
                ref _isConsistent,
                ref _handledEventSender,
                ref _handledEventArgs,
                _isConsistent);
		}


        void ICanProcessSourceItemChange.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
            itemInfo.Key = applyKeySelector(itemInfo, _sourceAsList[sourceIndex]);
			baseRemoveItem(key);
			TKey newKey = itemInfo.Key;
			baseAddItem(newKey);
		}

		private void disposeKeyExpressionWatcher(ItemInfo itemInfo)
		{
			ExpressionWatcher watcher = itemInfo.ExpressionWatcher;
			watcher.Dispose();
            EventUnsubscriber.QueueSubscriptions(watcher._propertyChangedEventSubscriptions, watcher._methodChangedEventSubscriptions);
            if (_keySelectorContainsParametrizedObservableComputationsCalls)
                Utils.itemInfoRemoveDownstreamConsumedComputing(itemInfo.NestedComputings, this);
        }

		public TKey ApplyKeySelector(int index)
		{
			return applyKeySelector(_itemInfos[index], _sourceAsList[index]);
		}

		private TKey applyKeySelector(ItemInfo itemInfo, TSourceItem sourceItem)
		{
            TKey getValue() => _keySelectorContainsParametrizedObservableComputationsCalls ? itemInfo._keySelectorFunc() : _keySelectorFunc(sourceItem);

            if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
				TKey result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}


		private void baseClearItems()
		{
			//int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_hashSet = new HashSet<TKey>(/*capacity, */_equalityComparer);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new MethodChangedEventArgs("Contains", args => true));
			}

		}

		private void baseAddItem(TKey key)
		{
			_hashSet.Add(key);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new MethodChangedEventArgs("Contains", args => _equalityComparer.Equals(key, (TKey)args[0])));
			}
		}

		private void baseRemoveItem(TKey key)
		{
			_hashSet.Remove(key);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new MethodChangedEventArgs("Contains", args => _equalityComparer.Equals(key, (TKey)args[0])));
			}
		}

		protected void checkConsistent(object sender, EventArgs eventArgs)
		{
			if (!_isConsistent)
				throw new ObservableComputationsInconsistencyException(this,
					$"The source collection has been changed. It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.", sender, eventArgs);
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void onPropertyChanged(PropertyChangedEventArgs eventArgs)
		{
			PropertyChanged?.Invoke(this, eventArgs);
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
					if (itemInfo.ExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: HashSetting.5");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.ExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: HashSetting.7");

					if (itemInfo.ExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: HashSetting.17");

					if (!itemInfo.Key.IsSameAs(key))
						throw new ObservableComputationsException("Consistency violation: HashSetting.10");
				}
			}			
		}

        protected List<Consumer> _consumers = new List<Consumer>();
        internal  List<IComputingInternal> _downstreamConsumedComputings = new List<IComputingInternal>();
        protected bool _isActive;
        public bool IsActive => _isActive;

        private void handleSourceScalarValueChanged(object sender,  PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
            checkConsistent(sender, e);

            _handledEventSender = sender;
            _handledEventArgs = e;

            _isConsistent = false;
            initializeFromSource();
            _isConsistent = true;
            ConsistencyRestored?.Invoke(this, null);

            _handledEventSender = null;
            _handledEventArgs = null;
        }

        #region Implementation of IComputingInternal
        IEnumerable<Consumer> IComputingInternal.Consumers => _consumers;

        void IComputingInternal.AddToUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        void IComputingInternal.RemoveFromUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        void IComputingInternal.Initialize()
        {
            initializeEqualityComparer();
            Utils.initializeSourceScalar(_sourceScalar, ref _source, handleSourceScalarValueChanged);
            Utils.initializeNestedComputings(_keyNestedComputings, this);
            _hashSet = new HashSet<TKey>(/*Utils.getCapacity(_sourceScalar, _source),*/ _equalityComparer);
        }

        void IComputingInternal.Uninitialize()
        {
            if (_equalityComparerScalar != null)
            {
                _equalityComparerScalar.PropertyChanged -= handleEqualityComparerScalarValueChanged;
                _equalityComparer = null;
            }

            Utils.uninitializeSourceScalar(_sourceScalar, handleSourceScalarValueChanged, ref _source);
            Utils.uninitializeNestedComputings(_keyNestedComputings, this);
        }

        void IComputingInternal.InitializeFromSource()
        {
            initializeFromSource();
        }

        void IComputingInternal.AddConsumer(Consumer addingConsumer)
        {
            Utils.AddComsumer(addingConsumer, _consumers, _downstreamConsumedComputings, this, ref _isActive);
        }

        void IComputingInternal.OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            PropertyChanged?.Invoke(this, propertyChangedEventArgs);
        }

        void IComputingInternal.RemoveConsumer(Consumer removingConsumer)
        {
            Utils.RemoveConsumer(removingConsumer, _consumers, _downstreamConsumedComputings, ref _isActive, this);
        }

        void IComputingInternal.AddDownstreamConsumedComputing(IComputingInternal computing)
        {
            Utils.AddDownstreamConsumedComputing(computing, _downstreamConsumedComputings, _consumers, ref _isActive, this);
        }

        void IComputingInternal.RemoveDownstreamConsumedComputing(IComputingInternal computing)
        {
            Utils.RemoveDownstreamConsumedComputing(computing, _downstreamConsumedComputings, ref _isActive, this, _consumers);
        }

        void IComputingInternal.RaiseConsistencyRestored()
        {
            ConsistencyRestored?.Invoke(this, null);
        }

        #endregion

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

		public event EventHandler<MethodChangedEventArgs> MethodChanged;

		#endregion
	}

	public enum HashSetChangeAction
	{
		AddItem,
		RemoveItem,
		ClearItems,
	}
}
