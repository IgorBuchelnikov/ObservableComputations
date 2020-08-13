using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class CollectionDispatching<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		public INotifyCollectionChanged Source => _source;
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;
		public ICollectionDestinationDispatcher CollectionDestinationDispatcher => _collectionDestinationDispatcher;
		public IDispatcher DestinationDispatcher => _destinationDispatcher;
		public IDispatcher SourceDispatcher => _sourceDispatcher;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});


		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private bool _sourceInitialized;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private readonly IDispatcher _destinationDispatcher;
		private ICollectionDestinationDispatcher _collectionDestinationDispatcher;
		private IDispatcher _sourceDispatcher;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;


		[ObservableComputationsCall]
		public CollectionDispatching(
			INotifyCollectionChanged source,
			IDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_destinationDispatcher = destinationDispatcher;
            _sourceDispatcher = sourceDispatcher;
            _source = source;
        }

		[ObservableComputationsCall]
		public CollectionDispatching(
			INotifyCollectionChanged source,
			ICollectionDestinationDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_collectionDestinationDispatcher = destinationDispatcher;
            _sourceDispatcher = sourceDispatcher;
            _source = source;
        }

        protected override void initializeFromSource()
		{
			if (_sourceDispatcher != null)
				_sourceDispatcher.Invoke(doInitializeFromSource, this);
			else
				doInitializeFromSource();
		}

		[ObservableComputationsCall]
		public CollectionDispatching(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_destinationDispatcher = destinationDispatcher;
            _sourceDispatcher = sourceDispatcher;
            _sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public CollectionDispatching(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			ICollectionDestinationDispatcher destinationDispatcher,
			IDispatcher sourceDispatcher = null)
		{
			_collectionDestinationDispatcher = destinationDispatcher;
            _sourceDispatcher = sourceDispatcher;
            _sourceScalar = sourceScalar;
		}


		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			doInitializeFromSource();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void doInitializeFromSource()
		{
			int originalCount = _items.Count;

			if (_sourceInitialized != null)
			{
				if (_destinationDispatcher != null) _destinationDispatcher.Invoke(baseClearItems, this);
				else _collectionDestinationDispatcher.Invoke(baseClearItems, this, NotifyCollectionChangedAction.Reset, null, null, 0, 0);
				
                _source.CollectionChanged -= handleSourceCollectionChanged;

                if (_sourceAsINotifyPropertyChanged != null)
                {
                    _sourceAsINotifyPropertyChanged.PropertyChanged -=
                        ((ISourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
                    _sourceAsINotifyPropertyChanged = null;
                }

                _sourceInitialized = false;
            }

            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
                Utils.initializeFromHasChangeMarker(
                    ref _sourceAsIHasChangeMarker, 
                    _sourceAsList, 
                    ref _lastProcessedSourceChangeMarker, 
                    ref _sourceAsINotifyPropertyChanged,
                    this);

				int count = _sourceAsList.Count;
				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					if (originalCount > sourceIndex)
						_items[sourceIndex] = _sourceAsList[sourceIndex];
					else
						_items.Insert(sourceIndex, _sourceAsList[sourceIndex]);
				}

				for (int index = originalCount - 1; index >= sourceIndex; index--)
				{
					_items.RemoveAt(index);
				}

                _source.CollectionChanged += handleSourceCollectionChanged;           
                _sourceInitialized = true;
            }
			else
			{
				_items.Clear();
			}

			if (_destinationDispatcher != null) _destinationDispatcher.Invoke(reset, this);
			else _collectionDestinationDispatcher.Invoke(reset, this, NotifyCollectionChangedAction.Reset, null, null, 0, 0);
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

                    //if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
                    void add()
                    {
                        baseInsertItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
                    }

                    if (_destinationDispatcher != null) _destinationDispatcher.Invoke(add, this);
                    else
                        _collectionDestinationDispatcher.Invoke(add, this, NotifyCollectionChangedAction.Add,
                            (TSourceItem) e.NewItems[0], null, e.NewStartingIndex, 0);

                    break;
                case NotifyCollectionChangedAction.Remove:
                    // (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");


                    void remove()
                    {
                        baseRemoveItem(e.OldStartingIndex);
                    }

                    if (_destinationDispatcher != null) _destinationDispatcher.Invoke(remove, this);
                    else
                        _collectionDestinationDispatcher.Invoke(remove, this, NotifyCollectionChangedAction.Remove,
                            null, e.OldItems[0], 0, e.OldStartingIndex);

                    break;
                case NotifyCollectionChangedAction.Replace:

                    //if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
                    void replace()
                    {
                        baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
                    }

                    if (_destinationDispatcher != null) _destinationDispatcher.Invoke(replace, this);
                    else
                        _collectionDestinationDispatcher.Invoke(replace, this, NotifyCollectionChangedAction.Replace,
                            (TSourceItem) e.NewItems[0], (TSourceItem) e.OldItems[0], e.NewStartingIndex,
                            e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    int oldStartingIndex1 = e.OldStartingIndex;
                    int newStartingIndex1 = e.NewStartingIndex;
                    if (oldStartingIndex1 == newStartingIndex1) return;

                    void move()
                    {
                        baseMoveItem(oldStartingIndex1, newStartingIndex1);
                    }

                    if (_destinationDispatcher != null) _destinationDispatcher.Invoke(move, this);
                    else
                        _collectionDestinationDispatcher.Invoke(move, this, NotifyCollectionChangedAction.Move,
                            (TSourceItem) e.NewItems[0], (TSourceItem) e.OldItems[0], e.NewStartingIndex,
                            e.OldStartingIndex);

                    break;
                case NotifyCollectionChangedAction.Reset:
                    doInitializeFromSource();
                    break;
            }

            Utils.postHandleSourceCollectionChanged(
                ref _handledEventSender,
                ref _handledEventArgs);
		}

        #region Implementation of ISourceIndexerPropertyTracker

        void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Utils.HandleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
        }

        #endregion

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
	}
}
