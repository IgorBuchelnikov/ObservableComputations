using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using ObservableComputations.Common;
using ObservableComputations.Common.Base;
using ObservableComputations.Common.Interface;

namespace ObservableComputations
{
	public class Selecting<TSourceItem, TResultItem> : CollectionComputing<TResultItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TResultItem>> SelectorExpression => _selectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TResultItem> SelectorFunc => _selectorFunc;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly Expression<Func<TSourceItem, TResultItem>> _selectorExpression;
		private readonly ExpressionWatcher.ExpressionInfo _selectorExpressionInfo;

		private readonly bool _selectorContainsParametrizedObservableComputationsCalls;

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;
		private bool _lastProcessedSourceChangeMarker;

		private Queue<ExpressionWatcher> _deferredExpressionWatcherChangedProcessings;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Expression<Func<TSourceItem, TResultItem>> _selectorExpressionOriginal;
		private INotifyCollectionChanged _source;
		private readonly Func<TSourceItem, TResultItem> _selectorFunc;

		private sealed class ItemInfo : Position
		{
			public ExpressionWatcher ExpressionWatcher;
			public Func<TResultItem> SelectorFunc;
		}

		[ObservableComputationsCall]
		public Selecting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TResultItem>> selectorExpression) : this(selectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public Selecting(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TResultItem>> selectorExpression) : this(selectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			initializeFromSource();
		}

		private Selecting(Expression<Func<TSourceItem, TResultItem>> selectorExpression, int capacity) : base(capacity)
		{
			_itemInfos = new List<ItemInfo>(capacity);
			_sourcePositions = new Positions<ItemInfo>(_itemInfos);

			_selectorExpressionOriginal = selectorExpression;
			CallToConstantConverter callToConstantConverter =
				new CallToConstantConverter(_selectorExpressionOriginal.Parameters);
			_selectorExpression =
				(Expression<Func<TSourceItem, TResultItem>>)callToConstantConverter.Visit(
					_selectorExpressionOriginal);
			_selectorContainsParametrizedObservableComputationsCalls =
				callToConstantConverter.ContainsParametrizedObservableComputationCalls;

			if (!_selectorContainsParametrizedObservableComputationsCalls)
			{
				_selectorExpressionInfo = ExpressionWatcher.GetExpressionInfo(_selectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				_selectorFunc = _selectorExpression.Compile();
			}
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				int itemInfosCount = _itemInfos.Count;
				for (int index = 0; index < itemInfosCount; index++)
				{
					ItemInfo itemInfo = _itemInfos[index];
					ExpressionWatcher expressionWatcher = itemInfo.ExpressionWatcher;
					expressionWatcher.Dispose();
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

			if (_sourceScalar != null)
				_source = _sourceScalar.Value;
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
					baseInsertItem(index, applySelector(itemInfo, sourceItem));
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

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value))
				return;
			checkConsistent();
			_consistent = false;

			initializeFromSource();

			_consistent = true;
			raiseConsistencyRestored();
		}


		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!_rootSourceWrapper && _lastProcessedSourceChangeMarker == _sourceAsList.ChangeMarker) return;
			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			checkConsistent();
			_consistent = false;

			ItemInfo itemInfo;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem addedItem = _sourceAsList[newStartingIndex];
					itemInfo = registerSourceItem(addedItem, newStartingIndex);
					baseInsertItem(newStartingIndex, applySelector(itemInfo, addedItem));
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					unregisterSourceItem(oldStartingIndex);
					baseRemoveItem(oldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem newItem = _sourceAsList[newStartingIndex1];
					ItemInfo replacingItemInfo = _itemInfos[newStartingIndex1];
					unregisterSourceItem(newStartingIndex1, true);
					itemInfo = registerSourceItem(newItem, newStartingIndex1, replacingItemInfo);
					baseSetItem(newStartingIndex1, applySelector(itemInfo, newItem));
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 != newStartingIndex2)
					{
						_sourcePositions.Move(oldStartingIndex2, newStartingIndex2);
						baseMoveItem(oldStartingIndex2, newStartingIndex2);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
			}

			if (_deferredExpressionWatcherChangedProcessings != null)
				while (_deferredExpressionWatcherChangedProcessings.Count > 0)
				{
					ExpressionWatcher expressionWatcher = _deferredExpressionWatcherChangedProcessings.Dequeue();
					if (!expressionWatcher._disposed)
						processExpressionWatcherValueChanged(expressionWatcher);
				}

			_consistent = true;
			raiseConsistencyRestored();
		}

		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			checkConsistent();

			if (_rootSourceWrapper || _sourceAsList.ChangeMarker == _lastProcessedSourceChangeMarker)
			{
				processExpressionWatcherValueChanged(expressionWatcher);
			}
			else
			{
				(_deferredExpressionWatcherChangedProcessings = _deferredExpressionWatcherChangedProcessings ??  new Queue<ExpressionWatcher>()).Enqueue(expressionWatcher);
			}
		}

		private ItemInfo registerSourceItem(TSourceItem sourceItem, int index, ItemInfo itemInfo = null)
		{
			itemInfo = itemInfo == null ? _sourcePositions.Insert(index) : _itemInfos[index];

			ExpressionWatcher watcher;
			if (!_selectorContainsParametrizedObservableComputationsCalls)
			{
				watcher = new ExpressionWatcher(_selectorExpressionInfo, sourceItem);
			}
			else
			{
				Expression<Func<TResultItem>> deparametrizedSelectorExpression =
					(Expression<Func<TResultItem>>)_selectorExpression.ApplyParameters(new object[] { sourceItem });
				Expression<Func<TResultItem>> selectorExpression =
					(Expression<Func<TResultItem>>)
						new CallToConstantConverter().Visit(deparametrizedSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				itemInfo.SelectorFunc = selectorExpression.Compile();
				watcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(selectorExpression));
			}

			watcher.ValueChanged = expressionWatcher_OnValueChanged;
			itemInfo.ExpressionWatcher = watcher;
			watcher._position = itemInfo;

			return itemInfo;
		}


		private void unregisterSourceItem(int index, bool replacing = false)
		{
			ExpressionWatcher watcher = _itemInfos[index].ExpressionWatcher;
			watcher.Dispose();

			if (!replacing)
			{
				_sourcePositions.Remove(index);
			}
		}

		private void processExpressionWatcherValueChanged(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			baseSetItem(sourceIndex, ApplySelector(sourceIndex));
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public TResultItem ApplySelector(int index) => 
			_selectorContainsParametrizedObservableComputationsCalls ? _itemInfos[index].SelectorFunc() : _selectorFunc(_sourceAsList[index]);


		private TResultItem applySelector(ItemInfo itemInfo, TSourceItem sourceItem) => 
			_selectorContainsParametrizedObservableComputationsCalls ? itemInfo.SelectorFunc() : _selectorFunc(sourceItem);

		~Selecting()
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

		// ReSharper disable once InconsistentNaming
		internal void ValidateConsistency()
		{
			_sourcePositions.ValidateConsistency();
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			if (_itemInfos.Count != source.Count)
				throw new ObservableComputationsException("Consistency violation: Selecting.1");
			Func<TSourceItem, TResultItem> selector = _selectorExpressionOriginal.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_itemInfos.Count != source.Count)
					throw new ObservableComputationsException("Consistency violation: Selecting.6");

				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];
					
					if (!EqualityComparer<TResultItem>.Default.Equals(this[sourceIndex], selector(sourceItem)))
						throw new ObservableComputationsException("Consistency violation: Selecting.2");

					if (_itemInfos[sourceIndex].Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: Selecting.3");
					if (itemInfo.ExpressionWatcher._position != _itemInfos[sourceIndex])
						throw new ObservableComputationsException("Consistency violation: Selecting.4");

					if (!_itemInfos.Contains((ItemInfo) itemInfo.ExpressionWatcher._position))
						throw new ObservableComputationsException("Consistency violation: Selecting.5");

					if (itemInfo.ExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableComputationsException("Consistency violation: Selecting.7");

				}
			}
		}


	}
}
