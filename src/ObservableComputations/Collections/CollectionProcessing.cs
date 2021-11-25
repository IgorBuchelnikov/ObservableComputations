// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ObservableComputations
{
	public class CollectionProcessing<TSourceItem, TReturnValue> : CollectionComputing<TReturnValue>, IHasSources, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		public Func<TSourceItem[], ICollectionComputing, TReturnValue[]> NewItemsProcessor => _newItemsProcessor;
		public Action<TSourceItem[], ICollectionComputing, TReturnValue[]> OldItemsProcessor => _oldItemsProcessor;
		public Action<TSourceItem, ICollectionComputing, TReturnValue> MoveItemProcessor => _moveItemProcessor;

		public Func<TSourceItem, ICollectionComputing, TReturnValue> NewItemProcessor => _newItemProcessor;
		public Action<TSourceItem, ICollectionComputing, TReturnValue> OldItemProcessor => _oldItemProcessor;

		private readonly Func<TSourceItem[], ICollectionComputing, TReturnValue[]> _newItemsProcessor;
		private readonly Action<TSourceItem[], ICollectionComputing, TReturnValue[]> _oldItemsProcessor;
		private readonly Action<TSourceItem, ICollectionComputing, TReturnValue> _moveItemProcessor;

		private readonly Func<TSourceItem, ICollectionComputing, TReturnValue> _newItemProcessor;
		private readonly Action<TSourceItem, ICollectionComputing, TReturnValue> _oldItemProcessor;


		private IList<TSourceItem> _sourceAsList;
		private IHasTickTackVersion _sourceAsIHasTickTackVersion;
		private bool _lastProcessedSourceTickTackVersion;

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private INotifyCollectionChanged _source;

		private bool _countPropertyChangedEventRaised;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;

		[ObservableComputationsCall]
		public CollectionProcessing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemsProcessor,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemsProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null) : this(newItemsProcessor, oldItemsProcessor, moveItemProcessor, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public CollectionProcessing(
			INotifyCollectionChanged source,
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemsProcessor,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemsProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null) : this(newItemsProcessor, oldItemsProcessor, moveItemProcessor, Utils.getCapacity(source))
		{
			_source = source;
		}

		[ObservableComputationsCall]
		public CollectionProcessing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor,
			Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null) : this(newItemProcessor, oldItemProcessor, moveItemProcessor, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public CollectionProcessing(
			INotifyCollectionChanged source,
			Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor,
			Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor = null,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null) : this(newItemProcessor, oldItemProcessor, moveItemProcessor, Utils.getCapacity(source))
		{
			_source = source;
		}

		private CollectionProcessing(
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemsProcessor,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemsProcessor,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor, 
			int initialCapacity) : this(moveItemProcessor, initialCapacity)
		{
			_newItemsProcessor = newItemsProcessor;
			_oldItemsProcessor = oldItemsProcessor;

			if (_newItemsProcessor != null)
				_newItemProcessor = (newItem, computing) =>
					_newItemsProcessor(new []{newItem}, computing)[0];
			
			if (_oldItemsProcessor != null)
				_oldItemProcessor = (oldItem, computing, returnValue) =>
					_oldItemsProcessor(new []{oldItem}, computing, new []{returnValue});
		}

		private CollectionProcessing(
			Func<TSourceItem, ICollectionComputing, TReturnValue> newItemProcessor,
			Action<TSourceItem, ICollectionComputing, TReturnValue> oldItemProcessor,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor, 
			int initialCapacity) : this(moveItemProcessor, initialCapacity)
		{
			_newItemProcessor = newItemProcessor;
			_oldItemProcessor = oldItemProcessor;

			if (_newItemProcessor != null)
			{
				_newItemsProcessor = (newItems, computing) =>
				{
					int newItemsLength = newItems.Length;
					TReturnValue[] returnValues = new TReturnValue[newItemsLength];
					for (int index = 0; index < newItemsLength; index++)
						returnValues[index] = newItemProcessor(newItems[index], computing);
					return returnValues;
				};
			}

			if (_oldItemProcessor != null)
			{
				_oldItemsProcessor = (oldItems, _, returnValues) =>
				{
					int newItemsLength = oldItems.Length;
					for (int index = 0; index < newItemsLength; index++)
						oldItemProcessor(oldItems[index], _, returnValues[index]);
				};
			}
		}

		private CollectionProcessing(
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor, 
			int initialCapacity) : base(initialCapacity)
		{
			_moveItemProcessor = moveItemProcessor;
			_thisAsSourceCollectionChangeProcessor = this;
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
				if (replaceSource)
					Utils.unsubscribeSource(
						_source, 
						ref _sourceAsINotifyPropertyChanged, 
						this, 
						handleSourceCollectionChanged);

				if (_oldItemsProcessor != null) processOldItems(_sourceAsList.ToArray(), this.ToArray());

				_sourceReadAndSubscribed = false;
			}

			if (replaceSource)
				Utils.replaceSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
					out _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
				if (replaceSource)
					Utils.subscribeSource(
						out _sourceAsIHasTickTackVersion, 
						_sourceAsList, 
						ref _lastProcessedSourceTickTackVersion, 
						ref _sourceAsINotifyPropertyChanged,
						(ISourceIndexerPropertyTracker)this,
						_source,
						handleSourceCollectionChanged);

				int count = _sourceAsList.Count;

				TSourceItem[] sourceCopy = new TSourceItem[count];
				_sourceAsList.CopyTo(sourceCopy, 0);

				TReturnValue[] returnValues = processNewItems(sourceCopy);

				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					TReturnValue returnValue = returnValues != null ? returnValues[sourceIndex] : default;
					if (originalCount > sourceIndex)
						_items[sourceIndex] = returnValue;
					else
						_items.Insert(sourceIndex, returnValue);
				}

				for (int index = originalCount - 1; index >= sourceIndex; index--)
				{
					_items.RemoveAt(index);
				}

				_sourceReadAndSubscribed = true;
			}
			else
				_items.Clear();

			reset();
		}


		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref _isConsistent, 
				ref _countPropertyChangedEventRaised,
				ref _indexerPropertyChangedEventRaised, 
				ref _lastProcessedSourceTickTackVersion, 
				_sourceAsIHasTickTackVersion, 
				ref _handledEventSender, 
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, _deferredQueuesCount, this)) return;

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
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem addedItem = (TSourceItem) e.NewItems[0];
					TReturnValue returnValue = processNewItem(addedItem);
					baseInsertItem(newStartingIndex, returnValue);
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					TSourceItem removedItem = (TSourceItem) e.OldItems[0];
					TReturnValue returnValue1 = this[oldStartingIndex];
					baseRemoveItem(oldStartingIndex);
					if (_oldItemProcessor != null) processOldItem(removedItem, returnValue1);
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem oldItem = (TSourceItem) e.OldItems[0];
					TSourceItem newItem = _sourceAsList[newStartingIndex1];
					TReturnValue returnValueOld = this[newStartingIndex1];

					TReturnValue returnValue2 = processNewItem(newItem);
					baseSetItem(newStartingIndex1, returnValue2);
					if (_oldItemProcessor != null) processOldItem(oldItem, returnValueOld);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 != newStartingIndex2)
					{
						baseMoveItem(oldStartingIndex2, newStartingIndex2);
						if (_moveItemProcessor != null) processMovedItem((TSourceItem) e.NewItems[0], this[newStartingIndex2]);
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					processSource(false);
					break;
			}
		}

		private TReturnValue[] processNewItems(TSourceItem[] sourceItems)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TReturnValue[] returnValues = _newItemsProcessor(sourceItems, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return returnValues;
			}

			return _newItemsProcessor(sourceItems, this);
		}

		private void processOldItems(TSourceItem[] sourceItems, TReturnValue[] returnValues)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_oldItemsProcessor(sourceItems, this, returnValues);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_oldItemsProcessor(sourceItems, this, returnValues);
		}

		private TReturnValue processNewItem(TSourceItem sourceItem)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TReturnValue returnValue = _newItemProcessor(sourceItem, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return returnValue;
			}

			return _newItemProcessor(sourceItem, this);
		}

		private void processOldItem(TSourceItem sourceItem, TReturnValue returnValue)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_oldItemProcessor(sourceItem, this, returnValue);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_oldItemProcessor(sourceItem, this, returnValue);
		}


		private void processMovedItem(TSourceItem sourceItem, TReturnValue returnValue)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_moveItemProcessor(sourceItem, this, returnValue);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_moveItemProcessor(sourceItem, this, returnValue);
		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			Utils.AddDownstreamConsumedComputing(computing, _sourceScalar, _source);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			Utils.RemoveDownstreamConsumedComputing(computing, _sourceScalar, _source);
		}

		protected override void initialize()
		{
			Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
		}

		protected override void uninitialize()
		{
			Utils.unsubscribeSourceScalar(_sourceScalar, scalarValueChangedHandler);
		}

		protected override void clearCachedScalarArgumentValues()
		{
			Utils.clearCachcedSourceScalarValue(_sourceScalar, ref _source);
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _countPropertyChangedEventRaised, ref _indexerPropertyChangedEventRaised);
		}

		#endregion

		internal override void InitializeInvolvedMembersTreeNodeImpl(InvolvedMembersTreeNode involvedMembersTreeNode)
		{
			Utils.AddInvolvedMembersTreeNodeChildren(involvedMembersTreeNode, _sourceScalar, _source);
		}
	}
}
