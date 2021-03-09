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

		private readonly Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> _newItemsProcessor;
		private readonly Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> _oldItemsProcessor;
		private readonly Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> _moveItemProcessor;

		private IList<TSourceItem> _sourceAsList;
		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private INotifyCollectionChanged _source;

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

		private CollectionProcessingVoid(
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> newItemsProcessor,
			Action<TSourceItem[], CollectionProcessingVoid<TSourceItem>> oldItemsProcessor,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor, 
			int initialCapacity) : base(initialCapacity)
		{
			_newItemsProcessor = newItemsProcessor;
			_oldItemsProcessor = oldItemsProcessor;
			_moveItemProcessor = moveItemProcessor;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		protected override void processSource()
		{
			processSource(true);
		}

		private void processSource(bool replaceSource)
		{
			if (_sourceReadAndSubscribed)
			{
				if (replaceSource)
					Utils.unsubscribeSource(
						_source, 
						ref _sourceAsINotifyPropertyChanged, 
						this, 
						handleSourceCollectionChanged);

				int count = Count;
				for (int i = 0; i < count; i++)
				{
					baseRemoveItem(0);
				}

				if (_oldItemsProcessor!= null) processOldItems(_sourceAsList.ToArray());

				_sourceReadAndSubscribed = false;
			}

			if (replaceSource)
				Utils.replaceSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
					out _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
				if (replaceSource)
					Utils.subscribeSource(
						out _sourceAsIHasChangeMarker, 
						_sourceAsList, 
						ref _lastProcessedSourceChangeMarker, 
						ref _sourceAsINotifyPropertyChanged,
						(ISourceIndexerPropertyTracker)this,
						_source,
						handleSourceCollectionChanged);

				int count = _sourceAsList.Count;

				TSourceItem[] sourceCopy = new TSourceItem[count];
				_sourceAsList.CopyTo(sourceCopy, 0);

				for (int index = 0; index < count; index++)
					baseInsertItem(index, sourceCopy[index]);
				

				if (_newItemsProcessor != null) processNewItems(sourceCopy);
			 
				_sourceReadAndSubscribed = true;
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref _isConsistent, 
				ref _indexerPropertyChangedEventRaised, 
				ref _lastProcessedSourceChangeMarker, 
				_sourceAsIHasChangeMarker, 
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
					if (_newItemsProcessor != null) processNewItems(new []{addedItem});
					baseInsertItem(newStartingIndex, addedItem);
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					TSourceItem removedItem = (TSourceItem) e.OldItems[0];
					baseRemoveItem(oldStartingIndex);
					if (_oldItemsProcessor != null) processOldItems(new []{removedItem});
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem oldItem = (TSourceItem) e.OldItems[0];
					TSourceItem newItem = _sourceAsList[newStartingIndex1];

					if (_newItemsProcessor != null) processNewItems(new []{newItem});
					baseSetItem(newStartingIndex1, newItem);
					if (_oldItemsProcessor != null) processOldItems(new []{oldItem});
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
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
		}

		#endregion
	}
}
