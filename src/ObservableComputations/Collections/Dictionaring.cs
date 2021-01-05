using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableComputations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class Dictionaring<TSourceItem, TKey, TValue> : IDictionary<TKey, TValue>, IHasSourceCollections, IComputing, INotifyMethodChanged, ISourceItemKeyChangeProcessor, ISourceItemValueChangeProcessor, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

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
		public virtual INotifyCollectionChanged Source => _source;

		public string InstantiatingStackTrace => _instantiatingStackTrace;

		public virtual ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public virtual ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public string DebugTag { get; set; }
		public object Tag { get; set; }

		private Dictionary<TKey, TValue> _dictionary;

		public bool IsConsistent => _isConsistent;

		public event EventHandler ConsistencyRestored;

		private readonly List<IComputingInternal> _keyNestedComputings;
		private readonly List<IComputingInternal> _valueNestedComputings;

		private readonly ISourceItemKeyChangeProcessor _thisAsSourceItemKeyChangeProcessor;
		private readonly ISourceItemValueChangeProcessor _thisAsSourceValueItemChangeProcessor;
		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
		private Queue<IProcessable>[] _deferredProcessings;

		private Action<TKey, TValue> _addItemRequestHandler;
		public Action<TKey, TValue> AddItemRequestHandler
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _addItemRequestHandler;
			set
			{
				if (_addItemRequestHandler != value)
				{
					_addItemRequestHandler = value;
					onPropertyChanged(Utils.AddItemRequestHandlerPropertyChangedEventArgs);
				}

			}
		}

		private Func<TKey, bool> _removeItemRequestHandler;
		public Func<TKey, bool> RemoveItemRequestHandler
		{
			// ReSharper disable once MemberCanBePrivate.Global
			get => _removeItemRequestHandler;
			set
			{
				if (_removeItemRequestHandler != value)
				{
					_removeItemRequestHandler = value;
					onPropertyChanged(Utils.RemoveItemRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		private Action<TKey, TValue> _setItemRequestHandler;
		// ReSharper disable once MemberCanBePrivate.Global
		public Action<TKey, TValue> SetItemRequestHandler
		{
			get => _setItemRequestHandler;
			set
			{
				if (_setItemRequestHandler != value)
				{
					_setItemRequestHandler = value;
					onPropertyChanged(Utils.SetItemRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		private Action _clearItemsRequestHandler;

		// ReSharper disable once MemberCanBePrivate.Global
		public Action ClearItemsRequestHandler
		{
			get => _clearItemsRequestHandler;
			set
			{
				if (_clearItemsRequestHandler != value)
				{
					_clearItemsRequestHandler = value;
					onPropertyChanged(Utils.ClearItemsRequestHandlerPropertyChangedEventArgs);
				}
			}
		}

		private Positions<KeyValueExpressionItemInfo<TKey, TValue>> _sourcePositions;
		private List<KeyValueExpressionItemInfo<TKey, TValue>> _itemInfos;

		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpression;
		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpressionOriginal;
		private readonly ExpressionWatcher.ExpressionInfo _keySelectorExpressionInfo;
		private int _keySelectorExpressionCallCount;

		private readonly bool _keySelectorContainsParametrizedObservableComputationsCalls;

		private readonly Expression<Func<TSourceItem, TValue>> _valueSelectorExpression;
		private readonly Expression<Func<TSourceItem, TValue>> _valueSelectorExpressionOriginal;
		private readonly ExpressionWatcher.ExpressionInfo _valueSelectorExpressionInfo;
		private int _valueSelectorExpressionCallCount;

		private readonly bool _valueSelectorContainsParametrizedObservableComputationsCalls;


		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;

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

		private Dictionaring(
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			int sourceCapacity)
		{
			if (Configuration.SaveInstantiatingStackTrace) _instantiatingStackTrace = Environment.StackTrace;

			Utils.construct(sourceCapacity, out _itemInfos, out _sourcePositions);

			Utils.construct(
				keySelectorExpression, 
				out _keySelectorExpressionOriginal, 
				out _keySelectorExpression, 
				out _keySelectorContainsParametrizedObservableComputationsCalls, 
				ref _keySelectorExpressionInfo, 
				ref _keySelectorExpressionCallCount, 
				ref _keySelectorFunc, 
				ref _keyNestedComputings);

			Utils.construct(
				valueSelectorExpression, 
				out _valueSelectorExpressionOriginal, 
				out _valueSelectorExpression, 
				out _valueSelectorContainsParametrizedObservableComputationsCalls, 
				ref _valueSelectorExpressionInfo, 
				ref _valueSelectorExpressionCallCount, 
				ref _valueSelectorFunc, 
				ref _valueNestedComputings);

			_thisAsSourceItemKeyChangeProcessor = this;
			_thisAsSourceValueItemChangeProcessor = this;
			_thisAsSourceCollectionChangeProcessor = this;
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
			Utils.processResetChange(
				sender, 
				e, 
				ref _isConsistent, 
				ref _handledEventSender, 
				ref _handledEventArgs,
				() => { _equalityComparer = _equalityComparerScalar.Value ?? EqualityComparer<TKey>.Default;}, 
				3,
				ref _deferredProcessings, this);
		}

		private void initializeFromSource()
		{
			if (_sourceInitialized)
			{
				Utils.disposeKeyValueExpressionItemInfos(
					_itemInfos,
					_keySelectorExpressionCallCount,
					_valueSelectorExpressionCallCount,
					this);

				Utils.removeDownstreamConsumedComputing(_itemInfos, this);

				Utils.disposeSource(
					_sourceScalar, 
					_source,
					out _itemInfos,
					out _sourcePositions, 
					_sourceAsList, 
					handleSourceCollectionChanged);

				baseClearItems();

				_sourceInitialized = false;
			}

			Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this, out _sourceAsList, false);

			if (_source != null && _isActive)
			{
				Utils.initializeFromObservableCollectionWithChangeMarker(
					_source, 
					ref _sourceAsList, 
					ref _rootSourceWrapper, 
					ref _lastProcessedSourceChangeMarker);

				int count = _sourceAsList.Count;
				TSourceItem[] sourceCopy = new TSourceItem[count];
				_sourceAsList.CopyTo(sourceCopy, 0);

				_sourceAsList.CollectionChanged += handleSourceCollectionChanged;
				for (int index = 0; index < count; index++)
				{
					TSourceItem sourceItem = sourceCopy[index];
					KeyValueExpressionItemInfo<TKey, TValue> itemInfo = registerSourceItem(sourceItem, index);
					TKey key = applyKeySelector(itemInfo, sourceItem);
					TValue value = applyValueSelector(itemInfo, sourceItem);
					baseAddItem(key, value);
				}
		 
				_sourceInitialized = true;
			}
		}

		private KeyValueExpressionItemInfo<TKey, TValue> registerSourceItem(TSourceItem sourceItem, int index, KeyValueExpressionItemInfo<TKey, TValue> itemInfo = null)
		{
			itemInfo = itemInfo == null ? _sourcePositions.Insert(index) : _itemInfos[index];

			fillItemInfoWithKey(itemInfo, sourceItem);
			fillItemInfoWithValue(itemInfo, sourceItem);

			return itemInfo;
		}

		private void unregisterSourceItem(int index, bool replacing = false)
		{
			KeyValueExpressionItemInfo<TKey, TValue> itemInfo = _itemInfos[index];
			Utils.disposeExpressionWatcher(itemInfo.KeyExpressionWatcher, itemInfo.KeyNestedComputings, this, _keySelectorContainsParametrizedObservableComputationsCalls);
			Utils.disposeExpressionWatcher(itemInfo.ValueExpressionWatcher, itemInfo.ValueNestedComputings, this, _valueSelectorContainsParametrizedObservableComputationsCalls);

			if (!replacing) _sourcePositions.Remove(index);
		}

		private void fillItemInfoWithValue(KeyValueExpressionItemInfo<TKey, TValue> itemInfo, TSourceItem sourceItem)
		{
			Utils.getItemInfoContent(
				new object[]{sourceItem}, 
				out ExpressionWatcher watcher,
				out Func<TValue> func,
				out List<IComputingInternal> nestedComputings,
				_valueSelectorExpression,
				out _valueSelectorExpressionCallCount,
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

		private void fillItemInfoWithKey(KeyValueExpressionItemInfo<TKey, TValue> itemInfo, TSourceItem sourceItem)
		{
			Utils.getItemInfoContent(
				new object[]{sourceItem}, 
				out ExpressionWatcher watcher,
				out Func<TKey> func,
				out List<IComputingInternal> nestedComputings,
				_keySelectorExpression,
				out _keySelectorExpressionCallCount,
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
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, 3, 
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
			TKey key;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem addedItem = (TSourceItem) e.NewItems[0];
					KeyValueExpressionItemInfo<TKey, TValue> itemInfo = registerSourceItem(addedItem, newStartingIndex);
					key = applyKeySelector(itemInfo, addedItem);
					TValue value = applyValueSelector(itemInfo, addedItem);
					baseAddItem(key, value);
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					key = _itemInfos[oldStartingIndex].Key;
					unregisterSourceItem(oldStartingIndex);
					baseRemoveItem(key);
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem newItem = (TSourceItem) e.NewItems[0];
					KeyValueExpressionItemInfo<TKey, TValue> replacingItemInfo = _itemInfos[newStartingIndex1];
					TKey oldKey = replacingItemInfo.Key;
					Utils.disposeExpressionWatcher(replacingItemInfo.KeyExpressionWatcher,
						replacingItemInfo.KeyNestedComputings, this,
						_keySelectorContainsParametrizedObservableComputationsCalls);
					Utils.disposeExpressionWatcher(replacingItemInfo.ValueExpressionWatcher,
						replacingItemInfo.ValueNestedComputings, this,
						_valueSelectorContainsParametrizedObservableComputationsCalls);

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
					initializeFromSource();
					break;
			}
		}

		private void keyExpressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			Utils.processSourceItemChange(
				expressionWatcher, 
				sender, 
				eventArgs, 
				_rootSourceWrapper, 
				_sourceAsList, 
				_lastProcessedSourceChangeMarker, 
				_thisAsSourceItemKeyChangeProcessor,
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings, 
				2, 3, this);
		}

		private void valueExpressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			Utils.processSourceItemChange(
				expressionWatcher, 
				sender, 
				eventArgs, 
				_rootSourceWrapper, 
				_sourceAsList, 
				_lastProcessedSourceChangeMarker, 
				_thisAsSourceValueItemChangeProcessor,
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings, 
				2, 3, this);
		}

		void ISourceItemKeyChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			if (expressionWatcher._disposed) return;
			int sourceIndex = expressionWatcher._position.Index;
			KeyValueExpressionItemInfo<TKey, TValue> itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
			itemInfo.Key = applyKeySelector(itemInfo, _sourceAsList[sourceIndex]);
			baseRemoveItem(key);
			TKey newKey = itemInfo.Key;
			baseAddItem(newKey, itemInfo.Value);
		}

		void ISourceItemValueChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			if (expressionWatcher._disposed) return;
			int sourceIndex = expressionWatcher._position.Index;
			KeyValueExpressionItemInfo<TKey, TValue> itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
			itemInfo.Value = applyValueSelector(itemInfo, _sourceAsList[sourceIndex]);
			baseSetItem(key, itemInfo.Value);
		}


		public TKey ApplyKeySelector(int index)
		{
			return applyKeySelector(_itemInfos[index], _sourceAsList[index]);
		}

		private TKey applyKeySelector(KeyValueExpressionItemInfo<TKey, TValue> itemInfo, TSourceItem sourceItem)
		{
			TKey getValue() => _keySelectorContainsParametrizedObservableComputationsCalls ? itemInfo._keySelectorFunc() : _keySelectorFunc(sourceItem);

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TKey result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		public TValue ApplyValueSelector(int index)
		{
			return applyValueSelector(_itemInfos[index], _sourceAsList[index]);
		}

		private TValue applyValueSelector(KeyValueExpressionItemInfo<TKey, TValue> itemInfo, TSourceItem sourceItem)
		{
			TValue getValue() => _valueSelectorContainsParametrizedObservableComputationsCalls ? itemInfo._valueSelectorFunc() : _valueSelectorFunc(sourceItem);

			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TValue result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);

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
					KeyValueExpressionItemInfo<TKey, TValue> itemInfo = _itemInfos[sourceIndex];

					TKey key = keySelector(sourceItem);
					if (!ContainsKey(key))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.2");

					TValue value = valueSelector(sourceItem);
					if (!this[key].IsSameAs(value))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.3");

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableComputationsException("Consistency violation: Dictionaring.4");
					if (itemInfo.KeyExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: Dictionaring.5");
					if (itemInfo.ValueExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: Dictionaring.6");

					if (!_sourcePositions.List.Contains((KeyValueExpressionItemInfo<TKey, TValue>) itemInfo.KeyExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: Dictionaring.7");

					if (!_sourcePositions.List.Contains((KeyValueExpressionItemInfo<TKey, TValue>) itemInfo.ValueExpressionWatcher._position))
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

		protected List<OcConsumer> _consumers = new List<OcConsumer>();
		internal  List<IComputingInternal> _downstreamConsumedComputings = new List<IComputingInternal>();
		protected bool _isActive;
		public bool IsActive => _isActive;

		bool _initializationInProgress;
		bool _uninitializationInProgress;
		public bool ActivationInProgress => _initializationInProgress;
		public bool InactivationInProgress => _uninitializationInProgress;

		void IComputingInternal.SetInactivationInProgress(bool value)
		{
			_uninitializationInProgress = value;
		}

		void IComputingInternal.SetActivationInProgress(bool value)
		{
			_initializationInProgress = value;
		}

		private void handleSourceScalarValueChanged(object sender,  PropertyChangedEventArgs e)
		{
			Utils.processResetChange(
				sender, 
				e, 
				ref _isConsistent, 
				ref _handledEventSender, 
				ref _handledEventArgs, 
				null, 
				3,
				ref _deferredProcessings, this);
		}

		#region Implementation of IComputingInternal
		IEnumerable<OcConsumer> IComputingInternal.Consumers => _consumers;

		void IComputingInternal.AddToUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		void IComputingInternal.RemoveFromUpstreamComputings(IComputingInternal computing)
		{
			(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
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

		void ICanInitializeFromSource.InitializeFromSource()
		{
			initializeFromSource();
		}



		void IComputingInternal.OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
		{
			PropertyChanged?.Invoke(this, propertyChangedEventArgs);
		}

		public void SetIsActive(bool value)
		{
			_isActive = value;
		}

		void ISourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			throw new NotImplementedException();
		}

		void IComputingInternal.AddConsumer(OcConsumer addingOcConsumer)
		{
			Utils.addConsumer(
				addingOcConsumer, 
				_consumers,
				_downstreamConsumedComputings, 
				this, 
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings,
				3);
		}


		void IComputingInternal.RemoveConsumer(OcConsumer removingOcConsumer)
		{
			Utils.removeConsumer(
				removingOcConsumer, 
				_consumers, 
				_downstreamConsumedComputings, 
				this,
				ref _isConsistent, 
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				3);
		}

		void IComputingInternal.AddDownstreamConsumedComputing(IComputingInternal computing)
		{
			Utils.addDownstreamConsumedComputing(
				computing, 
				_downstreamConsumedComputings, 
				_consumers, 
				this,
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings,
				3);
		}

		void IComputingInternal.RemoveDownstreamConsumedComputing(IComputingInternal computing)
		{
			Utils.removeDownstreamConsumedComputing(
				computing, 
				_downstreamConsumedComputings, 
				this, 
				ref _isConsistent,
				_consumers,
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				3);
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
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_clearItemsRequestHandler();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_clearItemsRequestHandler();
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
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_addItemRequestHandler(key, value);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_addItemRequestHandler(key, value);
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				bool result =  _removeItemRequestHandler(key);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return _removeItemRequestHandler(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public TValue this[TKey key]
		{
			get => _dictionary[key];
			set
			{
				if (Configuration.TrackComputingsExecutingUserCode)
				{
					int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
					_setItemRequestHandler(key, value);
					Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
					return;
				}

				_setItemRequestHandler(key, value);
			}
		}

		public ICollection<TKey> Keys => _dictionary.Keys;
		public ICollection<TValue> Values => _dictionary.Values;

		#endregion

		#region Implementation of INotifyMethodChanged

		public event EventHandler<MethodChangedEventArgs> MethodChanged;

		#endregion

		#region Overrides of Object

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(DebugTag))
				return $"{DebugTag} ({base.ToString()})";

			return base.ToString();
		}

		#endregion
	}
}
