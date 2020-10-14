using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	public class MinimazingOrMaximazing<TSourceItem> : ScalarComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		// ReSharper disable once ConvertToAutoProperty
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IComparer<TSourceItem>> ComparerScalar => _comparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public MinimazingOrMaximazingMode Mode => _mode;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public IComparer<TSourceItem> Comparer => _comparer;

		// ReSharper disable once MemberCanBePrivate.Global
		public TSourceItem DefaultValue => _defaultValue;

		// ReSharper disable once MemberCanBePrivate.Global
		public bool IsDefaulted => _isDefaulted;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once StaticMemberInGenericType
		private static readonly Func<int, bool> __checkCompareResultPositive = compareResult => compareResult > 0;
		// ReSharper disable once StaticMemberInGenericType
		private static readonly Func<int, bool> __checkCompareResultNegative = compareResult => compareResult < 0;
		private readonly Func<int, bool> _checkCompareResult; 
		private readonly Func<int, bool> _antiCheckCompareResult;

		private IList<TSourceItem> _sourceAsList;

		private int _valueCount;
		private List<TSourceItem> _sourceCopy;

		private bool _sourceInitialized;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly IReadScalar<IComparer<TSourceItem>> _comparerScalar;
		private readonly MinimazingOrMaximazingMode _mode;
		private INotifyCollectionChanged _source;
		private IComparer<TSourceItem> _comparer;
		private TSourceItem _defaultValue;
		private bool _isDefaulted;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

        private ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
        private PropertyChangedEventHandler _comparerScalarValueChangedHandler;

        private void initializeComparer()
		{
			if (_comparerScalar != null)
			{
                _comparerScalarValueChangedHandler = getScalarValueChangedHandler(
                    () => { _comparer = _comparerScalar.Value ?? Comparer<TSourceItem>.Default; });

				_comparerScalar.PropertyChanged += _comparerScalarValueChangedHandler;
				_comparer = _comparerScalar.Value;
			}

            if (_comparer == null)
			{
				_comparer = Comparer<TSourceItem>.Default;
			}
		}


		[ObservableComputationsCall]
		public MinimazingOrMaximazing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			MinimazingOrMaximazingMode mode,
			IComparer<TSourceItem> comparer = null,
			TSourceItem defaultValue = default(TSourceItem)) : this(mode, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_comparer = comparer;
			_defaultValue = defaultValue;
		}

		[ObservableComputationsCall]
		public MinimazingOrMaximazing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			MinimazingOrMaximazingMode mode,
			IReadScalar<IComparer<TSourceItem>> comparerScalar = null,
			TSourceItem defaultValue = default(TSourceItem)) : this(mode, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_comparerScalar = comparerScalar;
			_defaultValue = defaultValue;

		}

		[ObservableComputationsCall]
		public MinimazingOrMaximazing(
			INotifyCollectionChanged source,
			MinimazingOrMaximazingMode mode,
			IComparer<TSourceItem> comparer = null,
			TSourceItem defaultValue = default(TSourceItem)) : this(mode, Utils.getCapacity(source))
		{
			_source = source;       
			_comparer = comparer;
			_defaultValue = defaultValue;
		}

		[ObservableComputationsCall]
		public MinimazingOrMaximazing(
			INotifyCollectionChanged source,
			MinimazingOrMaximazingMode mode,
			IReadScalar<IComparer<TSourceItem>> comparerScalar = null,
			TSourceItem defaultValue = default(TSourceItem)) : this(mode, Utils.getCapacity(source))
		{
			_source = source;
			_comparerScalar = comparerScalar;
			_defaultValue = defaultValue;
		}

		private MinimazingOrMaximazing(MinimazingOrMaximazingMode mode, int capacity)
		{
			_sourceCopy = new List<TSourceItem>(capacity);
			_mode = mode;
			switch (_mode)
			{
				case MinimazingOrMaximazingMode.Maximazing:
					_checkCompareResult = __checkCompareResultPositive;
					_antiCheckCompareResult = __checkCompareResultNegative;
					break;
				case MinimazingOrMaximazingMode.Minimazing:
					_checkCompareResult = __checkCompareResultNegative;
					_antiCheckCompareResult = __checkCompareResultPositive;
					break;
			}

            _thisAsSourceCollectionChangeProcessor = this;
            _deferredQueuesCount = 2;
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

			    _sourceCopy = new List<TSourceItem>(Utils.getCapacity(_sourceScalar, _source));
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
                _sourceInitialized = true;
            }

			recalculateValue(true);
		}

		private void recalculateValue(bool initializeSourceItems = false)
		{
			int count = initializeSourceItems ? _sourceAsList?.Count ?? 0 : _sourceCopy.Count;

			if (count > 0)
			{
				// ReSharper disable once PossibleNullReferenceException
				TSourceItem value;
                value = initializeSourceItems ? _sourceAsList[0] : _sourceCopy[0];
                _valueCount = 1;

				if (initializeSourceItems) _sourceCopy.Add(value);
				for (int sourceIndex = 1; sourceIndex < count; sourceIndex++)
				{
					TSourceItem sourceItem;
					if (initializeSourceItems)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        sourceItem = _sourceAsList[sourceIndex];
                        _sourceCopy.Add(sourceItem);
                    }
                    else
                    {
                        sourceItem = _sourceCopy[sourceIndex];
                    }

					int compareResult = _comparer.Compare(sourceItem, value);
					if (_checkCompareResult(compareResult))
					{
						value = sourceItem;
						_valueCount = 1;
					}
					else if (compareResult == 0)
					{
						_valueCount++;
					}
				}

				if (_isDefaulted)
				{
					_isDefaulted = false;
					raisePropertyChanged(nameof(IsDefaulted));
				}

				setValue(value);
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
                    //if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
                    int newIndex = e.NewStartingIndex;
                    TSourceItem addedSourceItem = (TSourceItem) e.NewItems[0];
                    _sourceCopy.Insert(newIndex, addedSourceItem);

                    if (!_isDefaulted)
                    {
                        int compareResult = _comparer.Compare(addedSourceItem, Value);
                        if (_checkCompareResult(compareResult))
                        {
                            _valueCount = 1;
                            setValue(addedSourceItem);
                        }
                        else if (compareResult == 0)
                        {
                            _valueCount++;
                        }
                    }
                    else
                    {
                        _isDefaulted = false;
                        raisePropertyChanged(nameof(IsDefaulted));
                        _valueCount = 1;
                        setValue(addedSourceItem);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    //if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
                    int oldStartingIndex = e.OldStartingIndex;
                    TSourceItem removingSourceItem = (TSourceItem) e.OldItems[0];
                    _sourceCopy.RemoveAt(oldStartingIndex);

                    if (_comparer.Compare(removingSourceItem, Value) == 0)
                    {
                        _valueCount--;
                        if (_valueCount == 0)
                        {
                            recalculateValue();
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    //if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
                    int replacingSourceIndex = e.NewStartingIndex;
                    TSourceItem newItem = (TSourceItem) e.NewItems[0];
                    TSourceItem oldItem = (TSourceItem) e.OldItems[0];
                    _sourceCopy[replacingSourceIndex] = newItem;

                    int newCompareResult = _comparer.Compare(newItem, Value);
                    int oldCompareResult = _comparer.Compare(oldItem, Value);

                    if (_checkCompareResult(newCompareResult))
                    {
                        _valueCount = 1;
                        setValue(newItem);
                    }
                    else if (_antiCheckCompareResult(newCompareResult))
                    {
                        if (oldCompareResult == 0)
                        {
                            _valueCount--;
                            if (_valueCount == 0)
                            {
                                recalculateValue();
                            }
                        }
                    }
                    else //if (newCompareResult == 0)
                    {
                        if (_antiCheckCompareResult(oldCompareResult))
                        {
                            _valueCount++;
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    int oldStartingIndex1 = e.OldStartingIndex;
                    int newStartingIndex1 = e.NewStartingIndex;
                    if (oldStartingIndex1 == newStartingIndex1) return;
                    TSourceItem movingSourceItem = (TSourceItem) e.NewItems[0];
                    _sourceCopy.RemoveAt(oldStartingIndex1);
                    _sourceCopy.Insert(newStartingIndex1, movingSourceItem);
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
            (_comparerScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)        
        {
            (_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_comparerScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
            initializeComparer();
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourceScalar, scalarValueChangedHandler, ref _source);
            if (_comparerScalar != null)
                _comparerScalar.PropertyChanged -= _comparerScalarValueChangedHandler;
            
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
			// ReSharper disable once PossibleNullReferenceException
			if (_sourceCopy.Count != source.Count) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.1");
			TSourceItem defaultValue = _defaultValue;

			for (int i = 0; i < source.Count; i++)
			{
				TSourceItem sourceItem = source[i];
				TSourceItem savedSourceItem = _sourceCopy[i];
				if (!savedSourceItem.IsSameAs(sourceItem)) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.2");
			}

			if (source.Count > 0)
			{
				TSourceItem result = _mode == MinimazingOrMaximazingMode.Maximazing 
					?  source.Max() 
					: source.Min();

				if (!result.Equals(_value)) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.3");

				if (source.Count(i => i.Equals(result)) != _valueCount) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.4");

				if (IsDefaulted) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.4");
			}
			else
			{
				if (!defaultValue.Equals(_value)) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.5");
				if (!IsDefaulted) throw new ObservableComputationsException(this, "Consistency violation: MinimazingOrMaximazing.4");
			}

		}
	}

	public enum MinimazingOrMaximazingMode
	{
		Maximazing,
		Minimazing
	}
}
