using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Base;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class Dictionaring<TSourceItem, TKey, TValue> : Dictionary<TKey, TValue>, IHasSources
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

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public string InstantiatingStackTrace => _instantiatingStackTrace;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpression;
		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpressionOriginal;
		private readonly ExpressionWatcher.ExpressionInfo _keySelectorExpressionInfo;

		private readonly bool _keySelectorContainsParametrizedObservableCalculationsCalls;

		private readonly Expression<Func<TSourceItem, TValue>> _valueSelectorExpression;
		private readonly Expression<Func<TSourceItem, TValue>> _valueSelectorExpressionOriginal;
		private readonly ExpressionWatcher.ExpressionInfo _valueSelectorExpressionInfo;

		private readonly bool _valueSelectorContainsParametrizedObservableCalculationsCalls;

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


		private Dictionaring(
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			int sourceCapacity,
			int resultCapacity,
			IEqualityComparer<TKey> comparer = null
			) : base(resultCapacity, comparer)
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
			_keySelectorContainsParametrizedObservableCalculationsCalls = callToConstantConverter.ContainsParametrizedObservableCalculationCalls;

			if (!_keySelectorContainsParametrizedObservableCalculationsCalls)
			{
				_keySelectorExpressionInfo = ExpressionWatcher.GetExpressionInfo(_keySelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				_keySelectorFunc = _keySelectorExpression.Compile();
			}

			_valueSelectorExpressionOriginal = valueSelectorExpression;
			callToConstantConverter = new CallToConstantConverter(_valueSelectorExpressionOriginal.Parameters);
			_valueSelectorExpression = (Expression<Func<TSourceItem, TValue>>) callToConstantConverter.Visit(_valueSelectorExpressionOriginal);
			_valueSelectorContainsParametrizedObservableCalculationsCalls = callToConstantConverter.ContainsParametrizedObservableCalculationCalls;

			if (!_valueSelectorContainsParametrizedObservableCalculationsCalls)
			{
				_valueSelectorExpressionInfo = ExpressionWatcher.GetExpressionInfo(_valueSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				_valueSelectorFunc = _valueSelectorExpression.Compile();
			}
		}

		[ObservableCalculationsCall]
		public Dictionaring(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IEqualityComparer<TKey> comparer = null,
			int capacity = 0) : this(keySelectorExpression, valueSelectorExpression, Utils.getCapacity(sourceScalar), capacity, comparer)
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public Dictionaring(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			Expression<Func<TSourceItem, TValue>> valueSelectorExpression,
			IEqualityComparer<TKey> comparer = null,
			int capacity = 0) : this(keySelectorExpression, valueSelectorExpression, Utils.getCapacity(source), capacity, comparer)
		{
			_source = source;
			initializeFromSource();
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

				_lastProcessedSourceChangeMarker = _sourceAsList.ChangeMarker;

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
			if (!_valueSelectorContainsParametrizedObservableCalculationsCalls)
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
			if (!_keySelectorContainsParametrizedObservableCalculationsCalls)
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
			if (!_rootSourceWrapper && _lastProcessedSourceChangeMarker == _sourceAsList.ChangeMarker) return;
			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			TKey key;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem addedItem = _sourceAsList[newStartingIndex];
					ItemInfo itemInfo = registerSourceItem(addedItem, newStartingIndex);
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
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 == newStartingIndex2) return;

					_sourcePositions.Move(oldStartingIndex2, newStartingIndex2);
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
			}

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
		}

		private void handleSourceScalarValueChanged(object sender,  PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			initializeFromSource();
		}

		private void keyExpressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			if (_rootSourceWrapper || _sourceAsList.ChangeMarker ==_lastProcessedSourceChangeMarker)
			{
				processKeyExpressionWatcherValueChanged(expressionWatcher);
			}
			else
			{
				(_deferredKeyExpressionWatcherChangedProcessings = _deferredKeyExpressionWatcherChangedProcessings ??  new Queue<ExpressionWatcher>()).Enqueue(expressionWatcher);
			}
		}

		private void valueExpressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			if (_rootSourceWrapper || _sourceAsList.ChangeMarker ==_lastProcessedSourceChangeMarker)
			{
				processValueExpressionWatcherValueChanged(expressionWatcher);
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
			return _keySelectorContainsParametrizedObservableCalculationsCalls ? itemInfo._keySelectorFunc() : _keySelectorFunc(sourceItem);
		}

		public TValue ApplyValueSelector(int index)
		{
			return applyValueSelector(_itemInfos[index], _sourceAsList[index]);
		}

		private TValue applyValueSelector(ItemInfo itemInfo, TSourceItem sourceItem)
		{
			return _valueSelectorContainsParametrizedObservableCalculationsCalls ? itemInfo._valueSelectorFunc() : _valueSelectorFunc(sourceItem);
		}

		private void baseClearItems()
		{
			Clear();
		}

		private void baseAddItem(TKey key, TValue value)
		{
			Add(key, value);
		}

		private void baseSetItem(TKey key, TValue value)
		{
			base[key] = value;
		}

		private void baseRemoveItem(TKey key)
		{
			Remove(key);
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public TValue GetValueOrDefault(TKey key, TValue defaultValue)
		{
			return TryGetValue(key, out TValue value) ? value : defaultValue;
		}

		public TValue GetValueOrDefault(TKey key)
		{
			return GetValueOrDefault(key, default(TValue));
		}

		~Dictionaring()
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
			if (_itemInfos.Count != source.Count) throw new ObservableCalculationsException("Consistency violation: Dictionaring.1");
			if (Count != source.Count) throw new ObservableCalculationsException("Consistency violation: Dictionaring.16");
			Func<TSourceItem, TKey> keySelector = _keySelectorExpression.Compile();
			Func<TSourceItem, TValue> valueSelector = _valueSelectorExpression.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_sourcePositions.List.Count != source.Count)
					throw new ObservableCalculationsException("Consistency violation: Dictionaring.15");

				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];

					TKey key = keySelector(sourceItem);
					if (!ContainsKey(key))
						throw new ObservableCalculationsException("Consistency violation: Dictionaring.2");

					TValue value = valueSelector(sourceItem);
					if (!this[key].IsSameAs(value))
						throw new ObservableCalculationsException("Consistency violation: Dictionaring.3");

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableCalculationsException("Consistency violation: Dictionaring.4");
					if (itemInfo.KeyExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableCalculationsException("Consistency violation: Dictionaring.5");
					if (itemInfo.ValueExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableCalculationsException("Consistency violation: Dictionaring.6");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.KeyExpressionWatcher._position))
						throw new ObservableCalculationsException("Consistency violation: Dictionaring.7");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.ValueExpressionWatcher._position))
						throw new ObservableCalculationsException("Consistency violation: Dictionaring.8");

					if (itemInfo.KeyExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableCalculationsException("Consistency violation: Dictionaring.17");

					if (itemInfo.ValueExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableCalculationsException("Consistency violation: Dictionaring.18");

					if (!itemInfo.Key.IsSameAs(key))
						throw new ObservableCalculationsException("Consistency violation: Dictionaring.10");

					if (!itemInfo.Value.IsSameAs(value))
						throw new ObservableCalculationsException("Consistency violation: Dictionaring.9");
				}
			}			
		}
	}
}
