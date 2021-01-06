using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ObservableComputations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class HashSetting<TSourceItem, TKey> : ICollection<TKey>, IHasSourceCollections, IComputing, INotifyMethodChanged, ISourceItemChangeProcessor, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TKey>> KeySelectorExpression => _keySelectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TKey> KeySelectorFunc => _keySelectorFunc;

		public IReadScalar<IEqualityComparer<TKey>> EqualityComparerScalar => _equalityComparerScalar;

		public IEqualityComparer<TKey> EqualityComparer => _equalityComparer;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		public string InstantiatingStackTrace => _instantiatingStackTrace;

		public virtual ReadOnlyCollection<INotifyCollectionChanged> Sources => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public virtual ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public string DebugTag { get; set; }
		public object Tag { get; set; }

		private HashSet<TKey> _hashSet;

		public bool IsConsistent => _isConsistent;

		public event EventHandler ConsistencyRestored;

		private readonly List<IComputingInternal> _keyNestedComputings;

		private readonly ISourceItemChangeProcessor _thisAsSourceItemKeyChangeProcessor;
		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
		private Queue<IProcessable>[] _deferredProcessings;


		private Action<TKey> _addItemRequestHandler;
		public Action<TKey> AddItemRequestHandler
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

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpression;
		private readonly Expression<Func<TSourceItem, TKey>> _keySelectorExpressionOriginal;
		private readonly ExpressionWatcher.ExpressionInfo _keySelectorExpressionInfo;
		private int _keySelectorExpressionCallCount;

		private readonly bool _keySelectorContainsParametrizedObservableComputationsCalls;


		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;

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
				out _keySelectorExpressionOriginal, 
				out _keySelectorExpression, 
				out _keySelectorContainsParametrizedObservableComputationsCalls, 
				ref _keySelectorExpressionInfo, 
				ref _keySelectorExpressionCallCount, 
				ref _keySelectorFunc, 
				ref _keyNestedComputings);

			_thisAsSourceItemKeyChangeProcessor = this;
			_thisAsSourceCollectionChangeProcessor = this;
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
				Utils.disposeExpressionItemInfos(
					_itemInfos,
					_keySelectorExpressionCallCount,
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
					TSourceItem sourceItem = _sourceAsList[index];
					baseAddItem(applyKeySelector(registerSourceItem(sourceItem, index), sourceItem));
				}

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
			Utils.disposeExpressionWatcher(itemInfo.ExpressionWatcher, itemInfo.NestedComputings, this, _keySelectorContainsParametrizedObservableComputationsCalls);

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
				out _keySelectorExpressionCallCount,
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
					TSourceItem addedItem = _sourceAsList[newStartingIndex];
					ItemInfo itemInfo = registerSourceItem(addedItem, newStartingIndex);
					key = applyKeySelector(itemInfo, addedItem);
					baseAddItem(key);
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
					Utils.disposeExpressionWatcher(replacingItemInfo.ExpressionWatcher, replacingItemInfo.NestedComputings,
						this, _keySelectorContainsParametrizedObservableComputationsCalls);

					fillItemInfoWithKey(replacingItemInfo, newItem);
					replacingItemInfo.Key = applyKeySelector(replacingItemInfo, newItem);
					baseRemoveItem(oldKey);
					baseAddItem(replacingItemInfo.Key);
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


		void ISourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			if (expressionWatcher._disposed) return;
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			TKey key = itemInfo.Key;
			itemInfo.Key = applyKeySelector(itemInfo, _sourceAsList[sourceIndex]);
			baseRemoveItem(key);
			TKey newKey = itemInfo.Key;
			baseAddItem(newKey);
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
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TKey result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
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
				MethodChanged(this, new MethodChangedEventArgs("Contains", args => true));
			

		}

		private void baseAddItem(TKey key)
		{
			_hashSet.Add(key);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)			
				MethodChanged(this, new MethodChangedEventArgs("Contains", args => _equalityComparer.Equals(key, (TKey)args[0])));
		}

		private void baseRemoveItem(TKey key)
		{
			_hashSet.Remove(key);
			onPropertyChanged(Utils.CountPropertyChangedEventArgs);
			onPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			if (MethodChanged != null)
				MethodChanged(this, new MethodChangedEventArgs("Contains", args => _equalityComparer.Equals(key, (TKey)args[0])));
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

		[ExcludeFromCodeCoverage]
		internal void ValidateConsistency()
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
			_isActive = true;
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
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_addItemRequestHandler(item);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_addItemRequestHandler(item);
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
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				bool result =  _removeItemRequestHandler(item);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return _removeItemRequestHandler(item);
		}

		public int Count => _hashSet.Count;
		public bool IsReadOnly => ((ICollection<TKey>) _hashSet).IsReadOnly;

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

	public enum HashSetChangeAction
	{
		AddItem,
		RemoveItem,
		ClearItems,
	}
}
