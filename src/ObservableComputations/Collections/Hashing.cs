using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using ObservableComputations;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class Hashing<TSourceItem, TKey> : HashSet<TKey>, IHasSources, IComputing
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TKey>> KeySelectorExpression => _keySelectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TKey> KeySelectorFunc => _keySelectorFunc;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public string InstantiatingStackTrace => _instantiatingStackTrace;

		public string DebugTag { get; set; }
		public object Tag { get; set; }

		public bool IsConsistent => _isConsistent;

		public event EventHandler ConsistencyRestored;


		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpression;
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
		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpressionOriginal;
		private readonly Func<TSourceItem, TKey> _keySelectorFunc;
		private INotifyCollectionChanged _source;
		private readonly string _instantiatingStackTrace;
		private bool _isConsistent = true;

		private sealed class ItemInfo : Position
		{
			public ExpressionWatcher KeyExpressionWatcher;
			public Func<TKey> KeySelectorFunc;
			public TKey Key;
		}

		// ReSharper disable once MemberCanBePrivate.Global

		private Hashing(
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			int sourceCapacity,
			// ReSharper disable once UnusedParameter.Local
			int resultCapacity,
			IEqualityComparer<TKey> comparer = null) : base(/*resultCapacity,*/ comparer) //ability to set capacity of HashSet starts form .Net 4.7.3
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
		public Hashing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IEqualityComparer<TKey> comparer = null,
			int capacity = 0) : this(keySelectorExpression, Utils.getCapacity(sourceScalar), capacity, comparer)
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
			initializeFromSource();
		}

		[ObservableComputationsCall]
		public Hashing(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TKey>> keySelectorExpression,
			IEqualityComparer<TKey> comparer = null,
			int capacity = 0) : this(keySelectorExpression, Utils.getCapacity(source), capacity, comparer)
		{
			_source = source;
			initializeFromSource();
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
			checkConsistent();
			if (_rootSourceWrapper || _sourceAsList.ChangeMarker == _lastProcessedSourceChangeMarker)
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

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();

			if (!_rootSourceWrapper && _lastProcessedSourceChangeMarker == _sourceAsList.ChangeMarker) return;
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
					key = _itemInfos[e.OldStartingIndex].Key;
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

					if (!Comparer.Equals(oldKey, newKey))
					{
						baseRemoveItem(oldKey);
						baseAddItem(newKey);
					}		
					_isConsistent = true;
					ConsistencyRestored?.Invoke(this, null);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 == newStartingIndex2) return;
					_sourcePositions.Move(oldStartingIndex2, newStartingIndex2);
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

				_lastProcessedSourceChangeMarker = _sourceAsList.ChangeMarker;

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
				itemInfo.KeySelectorFunc = selectorExpression.Compile();
				watcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(selectorExpression));
			}

			watcher.ValueChanged = keyExpressionWatcher_OnValueChanged;
			watcher._position = itemInfo;
			itemInfo.KeyExpressionWatcher = watcher;
			itemInfo.Key = applyKeySelector(itemInfo, sourceItem);
		}

		private void processKeyExpressionWatcherValueChanged(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
			disposeKeyExpressionWatcher(itemInfo);
			fillItemInfoWithKey(itemInfo, _sourceAsList[sourceIndex]);
			baseRemoveItem(key);
			baseAddItem(itemInfo.Key);
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
			bool trackComputingsExecutingUserCode = Configuration.TrackComputingsExecutingUserCode;
			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode[Thread.CurrentThread] = this;
			}

			TKey result = _keySelectorContainsParametrizedObservableComputationsCalls ? itemInfo.KeySelectorFunc() : _keySelectorFunc(sourceItem);

			if (trackComputingsExecutingUserCode)
			{
				DebugInfo._computingsExecutingUserCode.Remove(Thread.CurrentThread);
			}

			return result;
		}

		private void baseClearItems()
		{
			Clear();
		}

		private void baseAddItem(TKey key)
		{
			Add(key);
		}

		private void baseRemoveItem(TKey key)
		{
			Remove(key);
		}

		protected void checkConsistent()
		{
			if (!_isConsistent)
				throw new ObservableComputationsException(this,
					"The source collection has been changed. It is not possible to process this change, as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.");
		}


		~Hashing()
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
			if (_itemInfos.Count != source.Count) throw new ObservableComputationsException("Consistency violation: Hashing.1");
			if (Count != source.Count) throw new ObservableComputationsException("Consistency violation: Hashing.10");
			Func<TSourceItem, TKey> keySelector = _keySelectorExpressionOriginal.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_sourcePositions.List.Count != source.Count)
					throw new ObservableComputationsException("Consistency violation: Hashing.15");

				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];

					TKey key = keySelector(sourceItem);
					if (!Contains(key))
						throw new ObservableComputationsException("Consistency violation: Hashing.2");

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableComputationsException("Consistency violation: Hashing.4");
					if (itemInfo.KeyExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableComputationsException("Consistency violation: Hashing.5");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.KeyExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: Hashing.7");

					if (itemInfo.KeyExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: Hashing.17");

					if (!itemInfo.Key.IsSameAs(key))
						throw new ObservableComputationsException("Consistency violation: Hashing.9");
				}
			}			
		}
	}
}
