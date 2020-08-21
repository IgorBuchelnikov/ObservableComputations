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
	public class Dictionaring<TSourceItem, TKey, TValue> : IDictionary<TKey, TValue>, IHasSourceCollections, IComputing, INotifyMethodChanged, ICanProcessSourceItemKeyChange, ICanProcessSourceItemValueChange
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

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public string DebugTag { get; set; }
		public object Tag { get; set; }

		private Dictionary<TKey, TValue> _dictionary;

		public bool IsConsistent => _isConsistent;

		public event EventHandler ConsistencyRestored;

        private List<IComputingInternal> _keyNestedComputings;
        private List<IComputingInternal> _valueNestedComputings;

        private ICanProcessSourceItemKeyChange _thisAsCanProcessSourceKeyItemChange;
        private ICanProcessSourceItemValueChange _thisAsCanProcessSourceValueItemChange;

		private Action<TKey, TValue> _addItemAction;
		public Action<TKey, TValue> AddItemAction
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

		private Action<TKey, TValue> _setItemAction;
		// ReSharper disable once MemberCanBePrivate.Global
		public Action<TKey, TValue> SetItemAction
		{
			get => _setItemAction;
			set
			{
				if (_setItemAction != value)
				{
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

		private readonly Expression<Func<TSourceItem, TValue>> _valueSelectorExpression;
		private readonly Expression<Func<TSourceItem, TValue>> _valueSelectorExpressionOriginal;
		private readonly ExpressionWatcher.ExpressionInfo _valueSelectorExpressionInfo;
        private int _valueSelectorExpressionСallCount;

		private readonly bool _valueSelectorContainsParametrizedObservableComputationsCalls;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
		private Queue<ExpressionWatcher.Raise> _deferredValueExpressionWatcherChangedProcessings;
		private Queue<ExpressionWatcher.Raise> _deferredKeyExpressionWatcherChangedProcessings;

		private bool _sourceInitialized;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Func<TSourceItem, TKey> _keySelectorFunc;
		private readonly Func<TSourceItem, TValue> _valueSelectorFunc;
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

		private sealed class ItemInfo : KeyValueExpressionItemInfo
		{
			public Func<TKey> _keySelectorFunc;
			public Func<TValue> _valueSelectorFunc;
			public TKey Key;
			public TValue Value;
		}

		private Dictionaring(
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
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

            Utils.construct(
                valueSelectorExpression, 
                ref _valueSelectorExpressionOriginal, 
                ref _valueSelectorExpression, 
                ref _valueSelectorContainsParametrizedObservableComputationsCalls, 
                ref _valueSelectorExpressionInfo, 
                ref _valueSelectorExpressionСallCount, 
                ref _valueSelectorFunc, 
                ref _valueNestedComputings);

            _thisAsCanProcessSourceKeyItemChange = this;
            _thisAsCanProcessSourceValueItemChange = this;
        }

		[ObservableComputationsCall]
		public Dictionaring(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IEqualityComparer<TKey> equalityComparer = null) : this(keySelectorExpression, valueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_equalityComparer = equalityComparer;
		}

		[ObservableComputationsCall]
		public Dictionaring(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IEqualityComparer<TKey> equalityComparer = null) : this(keySelectorExpression, valueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_equalityComparer = equalityComparer ?? EqualityComparer<TKey>.Default;
		}

		[ObservableComputationsCall]
		public Dictionaring(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar) : this(keySelectorExpression, valueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_equalityComparerScalar = equalityComparerScalar;
		}

		[ObservableComputationsCall]
		public Dictionaring(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IReadScalar<IEqualityComparer<TKey>> equalityComparerScalar) : this(keySelectorExpression, valueSelectorExpression, Utils.getCapacity(source))
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
				Utils.disposeKeyValueExpressionItemInfos(
                    _itemInfos,
                    _keySelectorExpressionСallCount,
                    _valueSelectorExpressionСallCount,
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
					TValue value = applyValueSelector(itemInfo, sourceItem);
					baseAddItem(key, value);
				}

                _sourceAsList.CollectionChanged += handleSourceCollectionChanged;
                _sourceInitialized = true;
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

			if (!replacing) _sourcePositions.Remove(index);
        }

		private void fillItemInfoWithValue(ItemInfo itemInfo, TSourceItem sourceItem)
		{
            Utils.getItemInfoContent(
                new object[]{sourceItem}, 
                out ExpressionWatcher watcher,
                out Func<TValue> func,
                out List<IComputingInternal> nestedComputings,
                _valueSelectorExpression,
                ref _valueSelectorExpressionСallCount,
                this,
                _valueSelectorContainsParametrizedObservableComputationsCalls,
                _valueSelectorExpressionInfo);

			watcher.ValueChanged = valueExpressionWatcher_OnValueChanged;
			watcher._position = itemInfo;
			itemInfo.ValueExpressionWatcher = watcher;
			itemInfo.Value = applyValueSelector(itemInfo, sourceItem);
            itemInfo._valueSelectorFunc = func;
            itemInfo.ValueNestedComputings = nestedComputings;
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
			itemInfo.KeyExpressionWatcher = watcher;
			itemInfo.Key = applyKeySelector(itemInfo, sourceItem);
            itemInfo._keySelectorFunc = func;
            itemInfo.KeyNestedComputings = nestedComputings;
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
					TValue value = applyValueSelector(itemInfo, addedItem);
					baseAddItem(key, value);
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
					disposeValueExpressionWatcher(replacingItemInfo);
					fillItemInfoWithKey(replacingItemInfo, newItem);
					fillItemInfoWithValue(replacingItemInfo, newItem);

					TKey newKey = replacingItemInfo.Key;
					TValue newValue = replacingItemInfo.Value;

                    if (_equalityComparer.Equals(oldKey, newKey))
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

            Utils.doDeferredExpressionWatcherChangedProcessings(
                _deferredKeyExpressionWatcherChangedProcessings, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                _thisAsCanProcessSourceKeyItemChange,
                ref _isConsistent,
                false); 

            Utils.doDeferredExpressionWatcherChangedProcessings(
                _deferredValueExpressionWatcherChangedProcessings, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                _thisAsCanProcessSourceValueItemChange,
                ref _isConsistent); 

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

		private void valueExpressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
            Utils.ProcessSourceItemChange(
                expressionWatcher, 
                sender, 
                eventArgs, 
                _rootSourceWrapper, 
                _sourceAsList, 
                _lastProcessedSourceChangeMarker, 
                _thisAsCanProcessSourceValueItemChange,
                ref _deferredValueExpressionWatcherChangedProcessings, 
                ref _isConsistent,
                ref _handledEventSender,
                ref _handledEventArgs,
                _isConsistent);
		}

        void ICanProcessSourceItemKeyChange.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
            itemInfo.Key = applyKeySelector(itemInfo, _sourceAsList[sourceIndex]);
			baseRemoveItem(key);
			TKey newKey = itemInfo.Key;
			baseAddItem(newKey, itemInfo.Value);
		}

        void ICanProcessSourceItemValueChange.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
            itemInfo.Value = applyValueSelector(itemInfo, _sourceAsList[sourceIndex]);
			baseSetItem(key, itemInfo.Value);
		}

		private void disposeValueExpressionWatcher(ItemInfo itemInfo)
		{
            ExpressionWatcher watcher = itemInfo.ValueExpressionWatcher;
			watcher.Dispose();
            EventUnsubscriber.QueueSubscriptions(watcher._propertyChangedEventSubscriptions, watcher._methodChangedEventSubscriptions);
            if (_valueSelectorContainsParametrizedObservableComputationsCalls)
                Utils.itemInfoRemoveDownstreamConsumedComputing(itemInfo.ValueNestedComputings, this);

        }

		private void disposeKeyExpressionWatcher(ItemInfo itemInfo)
		{
			ExpressionWatcher watcher = itemInfo.KeyExpressionWatcher;
			watcher.Dispose();
            EventUnsubscriber.QueueSubscriptions(watcher._propertyChangedEventSubscriptions, watcher._methodChangedEventSubscriptions);
            if (_keySelectorContainsParametrizedObservableComputationsCalls)
                Utils.itemInfoRemoveDownstreamConsumedComputing(itemInfo.KeyNestedComputings, this);
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

		public TValue ApplyValueSelector(int index)
		{
			return applyValueSelector(_itemInfos[index], _sourceAsList[index]);
		}

		private TValue applyValueSelector(ItemInfo itemInfo, TSourceItem sourceItem)
		{
            TValue getValue() => _valueSelectorContainsParametrizedObservableComputationsCalls ? itemInfo._valueSelectorFunc() : _valueSelectorFunc(sourceItem);

            if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
				TValue result = getValue();
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);

				return result;
			}

			return getValue();
		}

		private void baseClearItems()
		{
			int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
			_dictionary = new Dictionary<TKey, TValue>(capacity, _equalityComparer);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new MethodChangedEventArgs("GetValueOrDefault", args => true));
				MethodChanged(this, new MethodChangedEventArgs("Item[]", args => true));
				MethodChanged(this, new MethodChangedEventArgs("ContainsKey", args => true));
			}

		}

		private void baseAddItem(TKey key, TValue value)
		{
			_dictionary.Add(key, value);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new MethodChangedEventArgs("GetValueOrDefault", args => _equalityComparer.Equals(key, (TKey)args[0])));
				MethodChanged(this, new MethodChangedEventArgs("Item[]", args => _equalityComparer.Equals(key, (TKey)args[0])));
				MethodChanged(this, new MethodChangedEventArgs("ContainsKey", args => _equalityComparer.Equals(key, (TKey)args[0])));
			}
		}

		private void baseSetItem(TKey key, TValue value)
		{
			_dictionary[key] = value;
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged(this, new MethodChangedEventArgs("GetValueOrDefault", args => _equalityComparer.Equals(key, (TKey)args[0])));
				MethodChanged(this, new MethodChangedEventArgs("Item[]", args => _equalityComparer.Equals(key, (TKey)args[0])));
				MethodChanged(this, new MethodChangedEventArgs("ContainsKey", args => _equalityComparer.Equals(key, (TKey)args[0])));
			}
		}

		private void baseRemoveItem(TKey key)
		{
			_dictionary.Remove(key);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
			{
				MethodChanged?.Invoke(this, new MethodChangedEventArgs("GetValueOrDefault", args => _equalityComparer.Equals(key, (TKey)args[0])));
				MethodChanged?.Invoke(this, new MethodChangedEventArgs("Item[]", args => _equalityComparer.Equals(key, (TKey)args[0])));
				MethodChanged(this, new MethodChangedEventArgs("ContainsKey", args => _equalityComparer.Equals(key, (TKey)args[0])));
			}
		}

		protected void checkConsistent(object sender, EventArgs eventArgs)
		{
			if (!_isConsistent)
				throw new ObservableComputationsInconsistencyException(this,
					$"The source collection has been changed. It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.", sender, eventArgs);
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

		public void ValidateConsistency()
		{
			_sourcePositions.ValidateConsistency();
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			if (_itemInfos.Count != source.Count) throw new ObservableComputationsException("Consistency violation: Dictionaring.1");
			if (Count != source.Count) throw new ObservableComputationsException( "Consistency violation: Dictionaring.16");
			Func<TSourceItem, TKey> keySelector = _keySelectorExpression.Compile();
			Func<TSourceItem, TValue> valueSelector = _valueSelectorExpression.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_sourcePositions.List.Count != source.Count)
					throw new ObservableComputationsException("Consistency violation: Dictionaring.15");

				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];

					TKey key = keySelector(sourceItem);
					if (!ContainsKey(key))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.2");

					TValue value = valueSelector(sourceItem);
					if (!this[key].IsSameAs(value))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.3");

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableComputationsException("Consistency violation: Dictionaring.4");
					if (itemInfo.KeyExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: Dictionaring.5");
					if (itemInfo.ValueExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: Dictionaring.6");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.KeyExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.7");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.ValueExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.8");

					if (itemInfo.KeyExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: Dictionaring.17");

					if (itemInfo.ValueExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: Dictionaring.18");

					if (!itemInfo.Key.IsSameAs(key))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.10");

					if (!itemInfo.Value.IsSameAs(value))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.9");
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
            Utils.initializeNestedComputings(_valueNestedComputings, this);
            _dictionary = new Dictionary<TKey, TValue>(Utils.getCapacity(_sourceScalar, _source), _equalityComparer);
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
            Utils.uninitializeNestedComputings(_valueNestedComputings, this);
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

        void ICanProcessSourceItemChange.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
        {
            throw new NotImplementedException();
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
				if (!EqualityComparer<TValue>.Default.Equals(value, item.Value))
					return false;

				Remove(item.Key);
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

		public ICollection<TKey> Keys => _dictionary.Keys;
		public ICollection<TValue> Values => _dictionary.Values;

		#endregion

		#region Implementation of INotifyMethodChanged

		public event EventHandler<MethodChangedEventArgs> MethodChanged;

		#endregion
	}
}
