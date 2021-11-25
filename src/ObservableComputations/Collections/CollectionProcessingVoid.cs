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
	public class CollectionProcessingVoid<TSourceItem> : CollectionComputing<TSourceItem>, IHasSources, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		public Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> NewItemsProcessor => _newItemsProcessor;
		public Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> OldItemsProcessor => _oldItemsProcessor;
		public Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> MoveItemProcessor => _moveItemProcessor;

		public Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> NewItemProcessor => _newItemProcessor;
		public Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> OldItemProcessor => _oldItemProcessor;

		private readonly Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> _newItemsProcessor;
		private readonly Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> _oldItemsProcessor;
		private readonly Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> _moveItemProcessor;

		private readonly Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> _newItemProcessor;
		private readonly Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> _oldItemProcessor;

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
		public CollectionProcessingVoid(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemsProcessor = null,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemsProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null) : this(newItemsProcessor, oldItemsProcessor, moveItemProcessor, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public CollectionProcessingVoid(
			INotifyCollectionChanged source,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemsProcessor = null,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemsProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null) : this(newItemsProcessor, oldItemsProcessor, moveItemProcessor, Utils.getCapacity(source))
		{
			_source = source;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public CollectionProcessingVoid(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null) : this(newItemProcessor, oldItemProcessor, moveItemProcessor, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public CollectionProcessingVoid(
			INotifyCollectionChanged source,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null) : this(newItemProcessor, oldItemProcessor, moveItemProcessor, Utils.getCapacity(source))
		{
			_source = source;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		private CollectionProcessingVoid(
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemsProcessor,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemsProcessor,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor, 
			int initialCapacity) : this(moveItemProcessor, initialCapacity)
		{
			_newItemsProcessor = newItemsProcessor;
			_oldItemsProcessor = oldItemsProcessor;

			if (_newItemsProcessor != null)
				_newItemProcessor = (newItem, computing) =>
					_newItemsProcessor(new []{newItem}, computing);
			
			if (_oldItemsProcessor != null)
				_oldItemProcessor = (oldItem, computing) =>
					_oldItemsProcessor(new []{oldItem}, computing);
		}

		private CollectionProcessingVoid(
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> newItemProcessor,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> oldItemProcessor,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor, 
			int initialCapacity) : this(moveItemProcessor, initialCapacity)
		{
			_newItemProcessor = newItemProcessor;
			_oldItemProcessor = oldItemProcessor;

			if (_newItemProcessor != null)
			{
				_newItemsProcessor = (newItems, computing) =>
				{
					int newItemsLength = newItems.Length;
					for (int index = 0; index < newItemsLength; index++)
						newItemProcessor(newItems[index], computing);
				};
			}

			if (_oldItemProcessor != null)
			{
				_oldItemsProcessor = (oldItems, _) =>
				{
					int newItemsLength = oldItems.Length;
					for (int index = 0; index < newItemsLength; index++)
						oldItemProcessor(oldItems[index], _);
				};
			}
		}

		private CollectionProcessingVoid(
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor, 
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

				if (_oldItemsProcessor != null) processOldItems(this.ToArray());

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

				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					if (originalCount > sourceIndex)
						_items[sourceIndex] = sourceCopy[sourceIndex];
					else
						_items.Insert(sourceIndex, sourceCopy[sourceIndex]);
				}

				for (int index = originalCount - 1; index >= sourceIndex; index--)
				{
					_items.RemoveAt(index);
				}

				if (_newItemsProcessor != null) processNewItems(sourceCopy);
			 
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
					if (_newItemProcessor != null) processNewItem(addedItem);
					baseInsertItem(newStartingIndex, addedItem);
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					TSourceItem removedItem = (TSourceItem) e.OldItems[0];
					baseRemoveItem(oldStartingIndex);
					if (_oldItemProcessor != null) processOldItem(removedItem);
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem oldItem = (TSourceItem) e.OldItems[0];
					TSourceItem newItem = _sourceAsList[newStartingIndex1];

					if (_newItemProcessor != null) processNewItem(newItem);
					baseSetItem(newStartingIndex1, newItem);
					if (_oldItemProcessor != null) processOldItem(oldItem);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 != newStartingIndex2)
					{
						baseMoveItem(oldStartingIndex2, newStartingIndex2);
						if (_moveItemProcessor != null) processMovedItem((TSourceItem) e.NewItems[0]);
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					processSource(false);
					break;
			}
		}

		private void processNewItems(TSourceItem[] sourceItems)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_newItemsProcessor(sourceItems, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_newItemsProcessor(sourceItems, this);
		}

		private void processOldItems(TSourceItem[] sourceItems)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_oldItemsProcessor(sourceItems, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_oldItemsProcessor(sourceItems, this);
		}

		private void processNewItem(TSourceItem sourceItem)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_newItemProcessor(sourceItem, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_newItemProcessor(sourceItem, this);
		}

		private void processOldItem(TSourceItem sourceItem)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_oldItemProcessor(sourceItem, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_oldItemProcessor(sourceItem, this);
		}

		private void processMovedItem(TSourceItem sourceItem)
		{
			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_moveItemProcessor(sourceItem, this);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_moveItemProcessor(sourceItem, this);
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
