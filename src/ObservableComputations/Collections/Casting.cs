using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	public class Casting<TResultItem> : CollectionComputing<TResultItem>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private IList _sourceAsList;

		private bool _sourceInitialized;
		private INotifyCollectionChanged _source;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		[ObservableComputationsCall]
		public Casting(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : base(Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Casting(
			INotifyCollectionChanged source) : base(Utils.getCapacity(source))
		{
			_source = source;
		}

        protected override void initializeFromSource()
		{
			int originalCount = _items.Count;

			if (_sourceInitialized)
			{	
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
						_items[sourceIndex] = (TResultItem)_sourceAsList[sourceIndex];
					else
						_items.Insert(sourceIndex, (TResultItem)_sourceAsList[sourceIndex]);
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

			reset();
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

            _isConsistent = false;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    IList newItems = e.NewItems;
                    //if (newItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
                    baseInsertItem(e.NewStartingIndex, (TResultItem) newItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    //if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
                    baseRemoveItem(e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    int oldStartingIndex = e.OldStartingIndex;
                    int newStartingIndex = e.NewStartingIndex;
                    if (oldStartingIndex != newStartingIndex)
                    {
                        baseMoveItem(oldStartingIndex, newStartingIndex);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    IList newItems1 = e.NewItems;
                    //if (newItems1.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
                    baseSetItem(e.NewStartingIndex, (TResultItem) newItems1[0]);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    initializeFromSource();
                    break;
            }

            _isConsistent = true;
            raiseConsistencyRestored();

            Utils.postHandleSourceCollectionChanged(
                ref _handledEventSender,
                ref _handledEventArgs);
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

		public void ValidateConsistency()
		{
			IList source = _sourceScalar.getValue(_source, new ObservableCollection<object>()) as IList;
			// ReSharper disable once PossibleNullReferenceException
			if (Count != source.Count) throw new ObservableComputationsException(this, "Consistency violation: Casting.1");

			for (int i = 0; i < source.Count; i++)
			{
				object sourceItem = source[i];
				TResultItem resultItem = this[i];

				if (!resultItem.IsSameAs(sourceItem)) throw new ObservableComputationsException(this, "Consistency violation: Casting.2");
			}
		}
    }
}
