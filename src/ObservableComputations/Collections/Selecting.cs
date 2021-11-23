// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class Selecting<TSourceItem, TResultItem> : CollectionComputing<TResultItem>, IHasSources, ISourceItemChangeProcessor, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TResultItem>> SelectorExpression => _selectorExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TResultItem> SelectorFunc => _selectorFunc;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly Expression<Func<TSourceItem, TResultItem>> _selectorExpression;
		private readonly ExpressionWatcher.ExpressionInfo _selectorExpressionInfo;
		private int _selectorExpressionCallCount;

		private readonly bool _selectorContainsParametrizedObservableComputationsCalls;

		private ObservableCollectionWithTickTackVersion<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;
		private bool _lastProcessedSourceTickTackVersion;

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Expression<Func<TSourceItem, TResultItem>> _selectorExpressionOriginal;
		internal INotifyCollectionChanged _source;
		private readonly Func<TSourceItem, TResultItem> _selectorFunc;

		private readonly List<IComputingInternal> _nestedComputings;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
		private readonly ISourceItemChangeProcessor _thisAsSourceItemChangeProcessor;

		private sealed class ItemInfo : ExpressionItemInfo
		{
			public Func<TResultItem> SelectorFunc;
		}

		[ObservableComputationsCall]
		public Selecting(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Expression<Func<TSourceItem, TResultItem>> selectorExpression) : this(selectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Selecting(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, TResultItem>> selectorExpression) : this(selectorExpression, Utils.getCapacity(source))
		{
			_source = source;
		}

		private Selecting(Expression<Func<TSourceItem, TResultItem>> selectorExpression, int initialCapacity) : base(initialCapacity)
		{
			Utils.construct(
				selectorExpression, 
				initialCapacity, 
				out _itemInfos, 
				out _sourcePositions, 
				out _selectorExpressionOriginal, 
				out _selectorExpression, 
				out _selectorContainsParametrizedObservableComputationsCalls, 
				ref _selectorExpressionInfo, 
				ref _selectorExpressionCallCount, 
				ref _selectorFunc, 
				ref _nestedComputings);

			_deferredQueuesCount = 3;
			_thisAsSourceCollectionChangeProcessor = this;
			_thisAsSourceItemChangeProcessor = this;
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
				Utils.disposeExpressionItemInfos(_itemInfos, _selectorExpressionCallCount, this);
				Utils.removeDownstreamConsumedComputing(_itemInfos, this);

				Utils.disposeSource(
					_sourceScalar, 
					_source,
					out _itemInfos,
					out _sourcePositions, 
					_sourceAsList, 
					handleSourceCollectionChanged,
					replaceSource);

				_sourceReadAndSubscribed = false;
			}

			if (replaceSource)
				Utils.replaceSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this, out _sourceAsList, false);

			if (_source != null && _isActive)
			{
				if (replaceSource)
					Utils.subscribeSource(
						_source, 
						ref _sourceAsList, 
						ref _rootSourceWrapper, 
						ref _lastProcessedSourceTickTackVersion,
						handleSourceCollectionChanged);

				int count = _sourceAsList.Count;

				TSourceItem[] sourceCopy = new TSourceItem[count];
				_sourceAsList.CopyTo(sourceCopy, 0);

				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					ItemInfo itemInfo = registerSourceItem(sourceCopy[sourceIndex], sourceIndex);

					if (originalCount > sourceIndex)
						_items[sourceIndex] = applySelector(itemInfo, sourceCopy[sourceIndex]);
					else
						_items.Insert(sourceIndex, applySelector(itemInfo, sourceCopy[sourceIndex]));
				}

				for (int index = originalCount - 1; index >= sourceIndex; index--)
					_items.RemoveAt(index);

				_sourceReadAndSubscribed = true;
			}
			else
			{
				_items.Clear();
			}

			reset();
		}

		protected override void initialize()
		{
			Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
			Utils.initializeNestedComputings(_nestedComputings, this);
		}

		protected override void uninitialize()
		{
			Utils.unsubscribeSourceScalar(_sourceScalar, scalarValueChangedHandler);
			Utils.uninitializeNestedComputings(_nestedComputings, this);
		}

		protected override void clearCachedScalarArgumentValues()
		{
			Utils.clearCachcedSourceScalarValue(_sourceScalar, ref _source);
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
					sender, 
					e, 
					_rootSourceWrapper, 
					ref _lastProcessedSourceTickTackVersion, 
					_sourceAsList, 
					ref _isConsistent,
					ref _handledEventSender,
					ref _handledEventArgs,
					ref _deferredProcessings,
					1, _deferredQueuesCount, 
					this)) return;
	
			_thisAsSourceCollectionChangeProcessor.processSourceCollectionChanged(sender,e);	  
		   
			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		void ISourceCollectionChangeProcessor.processSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ItemInfo itemInfo;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem addedItem = (TSourceItem) e.NewItems[0];
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
					TSourceItem newItem = (TSourceItem) e.NewItems[0];
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
					processSource(false);
					break;
			}
		}

		internal override void InitializeInvolvedMembersTreeNodeImpl(InvolvedMembersTreeNode involvedMembersTreeNode)
		{
			int itemInfosCount = _itemInfos.Count;

			for (var index = 0; index < itemInfosCount; index++)
				_itemInfos[index].ExpressionWatcher.FillInvolvedMembers(involvedMembersTreeNode);

			if (_sourceScalar is IComputingInternal sourceScalarComputing)
				involvedMembersTreeNode.AddChild(sourceScalarComputing);

			if (_source is IComputingInternal sourceComputing)
				involvedMembersTreeNode.AddChild(sourceComputing);
		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			Utils.AddDownstreamConsumedComputing(computing, _sourceScalar, _source);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			Utils.RemoveDownstreamConsumedComputing(computing, _sourceScalar, _source);
		}

		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			Utils.processSourceItemChange(
				expressionWatcher, 
				sender, 
				eventArgs, 
				_rootSourceWrapper, 
				_sourceAsList, 
				_lastProcessedSourceTickTackVersion, 
				_thisAsSourceItemChangeProcessor,
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings, 
				2, _deferredQueuesCount, this);
		}

		private ItemInfo registerSourceItem(TSourceItem sourceItem, int index, ItemInfo itemInfo = null)
		{
			itemInfo = itemInfo == null ? _sourcePositions.Insert(index) : _itemInfos[index];

			Utils.getItemInfoContent(
				new object[]{sourceItem}, 
				out ExpressionWatcher watcher, 
				out Func<TResultItem> predicateFunc, 
				out List<IComputingInternal> nestedComputings,
				_selectorExpression,
				out _selectorExpressionCallCount,
				this,
				_selectorContainsParametrizedObservableComputationsCalls,
				_selectorExpressionInfo);

			itemInfo.SelectorFunc = predicateFunc;
			itemInfo.NestedComputings = nestedComputings;
			watcher.ValueChanged = expressionWatcher_OnValueChanged;
			itemInfo.ExpressionWatcher = watcher;
			watcher._position = itemInfo;

			return itemInfo;
		}

		void ISourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			baseSetItem(sourceIndex, applySelector((ItemInfo)expressionWatcher._position, _sourceAsList[sourceIndex]));
		}

		private void unregisterSourceItem(int index, bool replacing = false)
		{
			Utils.disposeExpressionWatcher(_itemInfos[index].ExpressionWatcher, _itemInfos[index].NestedComputings, this,
				_selectorContainsParametrizedObservableComputationsCalls);

			if (!replacing) _sourcePositions.Remove(index);
		}

		private TResultItem applySelector(ItemInfo itemInfo, TSourceItem sourceItem)
		{
			TResultItem getValue() =>
				_selectorContainsParametrizedObservableComputationsCalls
					? itemInfo.SelectorFunc()
					: _selectorFunc(sourceItem);

			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);		
				TResultItem result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		// ReSharper disable once InconsistentNaming
		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			_sourcePositions.ValidateInternalConsistency();
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			if (_itemInfos.Count != source.Count)
				throw new ValidateInternalConsistencyException("Consistency violation: Selecting.1");
			Func<TSourceItem, TResultItem> selector = _selectorExpressionOriginal.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_itemInfos.Count != source.Count)
					throw new ValidateInternalConsistencyException("Consistency violation: Selecting.6");

				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];
					
					if (!EqualityComparer<TResultItem>.Default.Equals(this[sourceIndex], selector(sourceItem)))
						throw new ValidateInternalConsistencyException("Consistency violation: Selecting.2");

					if (_itemInfos[sourceIndex].Index != sourceIndex)
						throw new ValidateInternalConsistencyException("Consistency violation: Selecting.3");
					if (itemInfo.ExpressionWatcher._position != _itemInfos[sourceIndex])
						throw new ValidateInternalConsistencyException("Consistency violation: Selecting.4");

					if (!_itemInfos.Contains((ItemInfo) itemInfo.ExpressionWatcher._position))
						throw new ValidateInternalConsistencyException("Consistency violation: Selecting.5");

					if (itemInfo.ExpressionWatcher._position.Index != sourceIndex)
						throw new ValidateInternalConsistencyException("Consistency violation: Selecting.7");

				}
			}
		}
	}
}
