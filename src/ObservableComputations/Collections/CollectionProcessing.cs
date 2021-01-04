using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ObservableComputations
{
	public class CollectionProcessing<TSourceItem, TReturnValue> : CollectionComputing<TReturnValue>, IHasSourceCollections, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public Func<TSourceItem[], ICollectionComputing, TReturnValue[]> NewItemsProcessor => _newItemsProcessor;
		public Action<TSourceItem[], ICollectionComputing, TReturnValue[]> OldItemsProcessor => _oldItemsProcessor;
		public Action<TSourceItem, ICollectionComputing, TReturnValue> MoveItemProcessor => _moveItemProcessor;

		private readonly Func<TSourceItem[], ICollectionComputing, TReturnValue[]> _newItemsProcessor;
		private readonly Action<TSourceItem[], ICollectionComputing, TReturnValue[]> _oldItemsProcessor;
		private readonly Action<TSourceItem, ICollectionComputing, TReturnValue> _moveItemProcessor;


		private IList<TSourceItem> _sourceAsList;
		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private bool _sourceInitialized;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private INotifyCollectionChanged _source;

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

		private CollectionProcessing(
			Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemsProcessor,
			Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemsProcessor,
			Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor, 
			int capacity) : base(capacity)
		{
			_newItemsProcessor = newItemsProcessor;
			_oldItemsProcessor = oldItemsProcessor;
			_moveItemProcessor = moveItemProcessor;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		protected override void initializeFromSource()
		{
			if (_sourceInitialized)
			{
				_source.CollectionChanged -= handleSourceCollectionChanged;

				if (_oldItemsProcessor != null) processOldItems(_sourceAsList.ToArray(), this.ToArray());

				int count = Count;
				for (int i = 0; i < count; i++)
					baseRemoveItem(0);

				_sourceInitialized = false;
			}

			Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
				out _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
				Utils.initializeFromHasChangeMarker(
					out _sourceAsIHasChangeMarker, 
					_sourceAsList, 
					ref _lastProcessedSourceChangeMarker, 
					ref _sourceAsINotifyPropertyChanged,
					(ISourceIndexerPropertyTracker)this);

				int count = _sourceAsList.Count;

				TSourceItem[] sourceCopy = new TSourceItem[count];
				_sourceAsList.CopyTo(sourceCopy, 0);

				_source.CollectionChanged += handleSourceCollectionChanged;

				TReturnValue[] returnValues = null;
				if (_newItemsProcessor != null)
					returnValues = processNewItems(sourceCopy);

				for (int index = 0; index < count; index++)
					baseInsertItem(index, returnValues != null ? returnValues[index] : default);

				_sourceInitialized = true;
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
					TReturnValue returnValue = processNewItems(new []{addedItem})[0];
					baseInsertItem(newStartingIndex, returnValue);
					raiseConsistencyRestored();
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					TSourceItem removedItem = (TSourceItem) e.OldItems[0];
					TReturnValue returnValue1 = this[oldStartingIndex];
					baseRemoveItem(oldStartingIndex);
					if (_oldItemsProcessor != null) processOldItems(new []{removedItem}, new []{returnValue1});
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex1 = e.NewStartingIndex;
					TSourceItem oldItem = (TSourceItem) e.OldItems[0];
					TSourceItem newItem = _sourceAsList[newStartingIndex1];
					TReturnValue returnValueOld = this[newStartingIndex1];

					TReturnValue returnValue2 = processNewItems(new []{newItem})[0];
					baseSetItem(newStartingIndex1, returnValue2);
					if (_oldItemsProcessor != null) processOldItems(new []{oldItem}, new []{returnValueOld});
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
					initializeFromSource();
					break;
			}
		}

		private TReturnValue[] processNewItems(TSourceItem[] sourceItems)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
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
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				_oldItemsProcessor(sourceItems, this, returnValues);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return;
			}

			_oldItemsProcessor(sourceItems, this, returnValues);
		}


		private void processMovedItem(TSourceItem sourceItem, TReturnValue returnValue)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
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
			(_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
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
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
		}

		#endregion
	}
}
