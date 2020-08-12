using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class CollectionProcessingVoid<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> NewItemProcessor => _newItemProcessor;
		public Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> OldItemProcessor => _oldItemProcessor;
		public Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> MoveItemProcessor => _moveItemProcessor;

		private readonly Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> _newItemProcessor;
		private readonly Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> _oldItemProcessor;
		private readonly Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> _moveItemProcessor;


		private IList<TSourceItem> _sourceAsList;
        private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private bool _sourceInitialized;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private INotifyCollectionChanged _source;

        private bool _indexerPropertyChangedEventRaised;
        private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		[ObservableComputationsCall]
		public CollectionProcessingVoid(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null) : this(newItemProcessor, oldItemProcessor, moveItemProcessor, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public CollectionProcessingVoid(
			INotifyCollectionChanged source,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> newItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> oldItemProcessor = null,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor = null) : this(newItemProcessor, oldItemProcessor, moveItemProcessor, Utils.getCapacity(source))
		{
			_source = source;
		}

		private CollectionProcessingVoid(
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> newItemProcessor,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> oldItemProcessor,
			Action<TSourceItem, CollectionProcessingVoid<TSourceItem>> moveItemProcessor, 
			int capacity) : base(capacity)
		{
			_newItemProcessor = newItemProcessor;
			_oldItemProcessor = oldItemProcessor;
			_moveItemProcessor = moveItemProcessor;
		}

        protected override void initializeFromSource()
		{
			if (_sourceInitialized)
			{
				int count = Count;
				for (int i = 0; i < count; i++)
				{
					TSourceItem sourceItem = _sourceAsList[i];
					baseRemoveItem(0);
					if (_oldItemProcessor!= null) processOldItem(sourceItem);
				}

                _source.CollectionChanged -= handleSourceCollectionChanged;
                _sourceInitialized = false;
            }

            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _sourceAsList, _source as IList<TSourceItem>);

            if (_sourceAsList != null && _isActive)
            {
                Utils.initializeFromHasChangeMarker(
                    ref _sourceAsIHasChangeMarker, 
                    _sourceAsList, 
                    ref _lastProcessedSourceChangeMarker, 
                    ref _sourceAsINotifyPropertyChanged,
                    this);

				int count = _sourceAsList.Count;
				for (int index = 0; index < count; index++)
				{
					TSourceItem sourceItem = _sourceAsList[index];
					if (_newItemProcessor != null) processNewItem(sourceItem);
					baseInsertItem(index, sourceItem);
				}

                _source.CollectionChanged += handleSourceCollectionChanged;
                _sourceInitialized = true;
            }
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
            if (!Utils.preHandleSourceCollectionChanged(
                sender, 
                e, 
                _isConsistent, 
                this, 
                ref _indexerPropertyChangedEventRaised, 
                ref _lastProcessedSourceChangeMarker, 
                _sourceAsIHasChangeMarker, 
                ref _handledEventSender, 
                ref _handledEventArgs)) return;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					_isConsistent = false;
					int newStartingIndex = e.NewStartingIndex;
					TSourceItem addedItem = _sourceAsList[newStartingIndex];
					if (_newItemProcessor != null) processNewItem(addedItem);
					baseInsertItem(newStartingIndex, addedItem);
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Remove:
					_isConsistent = false;
					int oldStartingIndex = e.OldStartingIndex;
					TSourceItem removedItem = (TSourceItem) e.OldItems[0];
					baseRemoveItem(oldStartingIndex);
					if (_oldItemProcessor!= null) processOldItem(removedItem);
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Replace:
					_isConsistent = false;
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem oldItem = (TSourceItem) e.OldItems[0];
					TSourceItem newItem = _sourceAsList[newStartingIndex1];

					if (_newItemProcessor != null) processNewItem(newItem);
					baseSetItem(newStartingIndex1, newItem);
					if (_oldItemProcessor!= null) processOldItem(oldItem);
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					if (oldStartingIndex2 != newStartingIndex2)
					{
						baseMoveItem(oldStartingIndex2, newStartingIndex2);
						if (_moveItemProcessor != null) processMovedItem(_sourceAsList[newStartingIndex2]);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					_isConsistent = false;
					initializeFromSource();
					_isConsistent = true;
					raiseConsistencyRestored();
					break;
			}

            Utils.postHandleSourceCollectionChanged(
                ref _handledEventSender,
                ref _handledEventArgs);
		}

		private void processNewItem(TSourceItem sourceItem)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
				_newItemProcessor(sourceItem, this);
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
				return;
			}

			_newItemProcessor(sourceItem, this);
		}

		private void processOldItem(TSourceItem sourceItem)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
				_oldItemProcessor(sourceItem, this);
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
				return;
			}

			_oldItemProcessor(sourceItem, this);
		}


		private void processMovedItem(TSourceItem sourceItem)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, ref _userCodeIsCalledFrom, this);
				_moveItemProcessor(sourceItem, this);
                Utils.endComputingExecutingUserCode(computing, currentThread, ref _userCodeIsCalledFrom);
				return;
			}

			_moveItemProcessor(sourceItem, this);
		}

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)        
        {
            (_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourceScalar, scalarValueChangedHandler, ref _source);
        }

        #region Implementation of ISourceIndexerPropertyTracker

        void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Utils.HandleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
        }

        #endregion
	}
}
