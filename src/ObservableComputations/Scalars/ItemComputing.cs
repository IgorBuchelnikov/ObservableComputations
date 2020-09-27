using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	public class ItemComputing<TSourceItem> : ScalarComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<int> IndexValueScalar => _indexValueScalar;

		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public int Index => _index;

		// ReSharper disable once MemberCanBePrivate.Global
		public bool IsDefaulted => _isDefaulted;

		// ReSharper disable once MemberCanBeProtected.Global
		public TSourceItem DefaultValue => _defaultValue;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		protected readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		protected INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;

		private bool _sourceInitialized;
		private readonly IReadScalar<int> _indexValueScalar;
		private int _index;
		private bool _isDefaulted;
		internal TSourceItem _defaultValue;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

        List<TSourceItem> _sourceCopy;

        private ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
        Action _changeIndexAction;

		private void initializeIndexScalar()
		{
            if (_indexValueScalar != null)
            {
			    _indexValueScalar.PropertyChanged += handleIndexScalarValueChanged;
			    _index = _indexValueScalar.Value;
            }

		}

		[ObservableComputationsCall]
		public ItemComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			int index, 
			TSourceItem defaultValue = default(TSourceItem)) : this()
		{
			_sourceScalar = sourceScalar;
			//initializeSourceScalar();

			_index = index;
			_defaultValue = defaultValue;
            //initializeFromSource();
        }

		[ObservableComputationsCall]
		public ItemComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<int> indexScalar, 
			TSourceItem defaultValue = default) : this()
		{
			_sourceScalar = sourceScalar;
			//initializeSourceScalar();

			_defaultValue = defaultValue;

			_indexValueScalar = indexScalar;
			//initializeIndexScalar();

			//initializeFromSource();
		}

		[ObservableComputationsCall]
		public ItemComputing(
			INotifyCollectionChanged source,
			int index, 
			TSourceItem defaultValue = default) : this()
		{
			_source = source;
			_index = index;
			_defaultValue = defaultValue;
			//initializeFromSource();
		}

		[ObservableComputationsCall]
		public ItemComputing(
			INotifyCollectionChanged source,
			IReadScalar<int> indexScalar, 
			TSourceItem defaultValue = default) : this()
		{
			_source = source;
			_defaultValue = defaultValue;
			_indexValueScalar = indexScalar;
            
			//initializeIndexScalar();
			//initializeFromSource();
		}

        private ItemComputing()
        {
            _thisAsSourceCollectionChangeProcessor = this;
            _changeIndexAction = () =>
            {
                _index = _indexValueScalar.Value;
                recalculateValue();
            };      
        }


		private void handleIndexScalarValueChanged(object sender, PropertyChangedEventArgs e)
        {
            Utils.processChange(
                sender, 
                e, 
                _changeIndexAction,
                ref _isConsistent, 
                ref _handledEventSender, 
                ref _handledEventArgs, 
                0, 1,
                ref _deferredProcessings, this);
		}

        protected override void initializeFromSource()
		{
			if (_sourceInitialized)
			{
				_source.CollectionChanged -= handleSourceCollectionChanged;

                if (_sourceAsINotifyPropertyChanged != null)
                {
                    _sourceAsINotifyPropertyChanged.PropertyChanged -=
                        ((ISourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
                    _sourceAsINotifyPropertyChanged = null;
                }

                _sourceCopy = null;

                _sourceInitialized = false;
            }

            Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
                ref _sourceAsList, true);


			if (_source != null && _isActive)
			{
                Utils.initializeFromHasChangeMarker(
                    out _sourceAsIHasChangeMarker, 
                    _sourceAsList, 
                    ref _lastProcessedSourceChangeMarker, 
                    ref _sourceAsINotifyPropertyChanged,
                    (ISourceIndexerPropertyTracker)this);

				_source.CollectionChanged += handleSourceCollectionChanged;
                _sourceCopy = new List<TSourceItem>(_sourceAsList);
			    recalculateValue();


                _sourceInitialized = true;

            }
            else
            {
                _isDefaulted = true;
                setValue(_defaultValue);
            }
		}

		private void recalculateValue()
		{
			if (_sourceCopy != null && _sourceCopy.Count > _index)
			{
				if (_isDefaulted)
				{
					_isDefaulted = false;
					raisePropertyChanged(nameof(IsDefaulted));
				}

				setValue(_sourceCopy[_index]);
			}
			else
			{
				if (!_isDefaulted)
				{
					_isDefaulted = true;
					raisePropertyChanged(nameof(IsDefaulted));
				}

				setValue(_defaultValue);
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
                1, 1, this)) return;


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
                    //if (e.NewItems.Count > 1) throw new Exception("Adding of multiple items is not supported");
                    _sourceCopy.Insert(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
                    if (e.NewStartingIndex <= _index) recalculateValue();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    //if (e.OldItems.Count > 1) throw new Exception("Removing of multiple items is not supported");
                    _sourceCopy.RemoveAt(e.OldStartingIndex);
                    if (e.OldStartingIndex <= _index) recalculateValue();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    //if (e.NewItems.Count > 1) throw new Exception("Replacing of multiple items is not supported");
                    _sourceCopy[e.NewStartingIndex] = (TSourceItem) e.NewItems[0];
                    if (e.OldStartingIndex == _index) recalculateValue();
                    break;
                case NotifyCollectionChangedAction.Move:
                    int oldStartingIndex = e.OldStartingIndex;
                    int newStartingIndex = e.NewStartingIndex;
                    if (newStartingIndex == oldStartingIndex) return;

                    TSourceItem sourceItem = (TSourceItem) e.NewItems[0];
                    _sourceCopy.RemoveAt(oldStartingIndex);
                    _sourceCopy.Insert(newStartingIndex, sourceItem);

                    if (newStartingIndex < oldStartingIndex)
                    {
                        if (_index >= newStartingIndex && _index <= oldStartingIndex)
                            setValue(_sourceCopy[_index]);
                    }
                    else
                    {
                        if (_index >= oldStartingIndex && _index <= newStartingIndex)
                            setValue(_sourceCopy[_index]);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    initializeFromSource();
                    break;
            }
        }

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_indexValueScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)        
        {
            (_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_indexValueScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
            initializeIndexScalar();
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourceScalar, scalarValueChangedHandler, ref _source);
            if (_indexValueScalar != null) 
                _indexValueScalar.PropertyChanged -= handleIndexScalarValueChanged;
        }

        #region Implementation of ISourceIndexerPropertyTracker

        void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Utils.HandleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
        }

        #endregion


		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			int index = _indexValueScalar.getValue(_index);
			TSourceItem defaultValue = _defaultValue;

			// ReSharper disable once PossibleNullReferenceException
			if (source.Count > index)
			{
				if (!source[index].IsSameAs(_value))
					throw new ObservableComputationsException(this, "Consistency violation: ItemComputing.1");
			}
			else
			{
				if (!defaultValue.IsSameAs(_value))
					throw new ObservableComputationsException(this, "Consistency violation: ItemComputing.2");			
			}
		}

	}
}
