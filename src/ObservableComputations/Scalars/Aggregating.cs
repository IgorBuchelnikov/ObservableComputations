using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ObservableComputations.ExtentionMethods;

namespace ObservableComputations
{
	public class Aggregating<TSourceItem, TResult> : ScalarComputing<TResult>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TResult, TResult> AggregateFunc => _aggregateFunc;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TResult, TResult> DeaggregateFunc => _deaggregateFunc;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;

		private IList<TSourceItem> _sourceAsList;

		private List<TSourceItem> _sourceItems;

		private bool _sourceInitialized;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Func<TSourceItem, TResult, TResult> _aggregateFunc;
		private readonly Func<TSourceItem, TResult, TResult> _deaggregateFunc;
		private INotifyCollectionChanged _source;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		[ObservableComputationsCall]
		public Aggregating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			(Func<TSourceItem, TResult, TResult> aggregateFunc,
			Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs) : this(funcs, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			//_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			//_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			//_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			//initializeFromSource();
		}

		[ObservableComputationsCall]
		public Aggregating(
			INotifyCollectionChanged source,
			(Func<TSourceItem, TResult, TResult> aggregateFunc,
			Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
			: this(funcs, Utils.getCapacity(source))
		{
			_source = source;
			//initializeFromSource();
		}

		private Aggregating((Func<TSourceItem, TResult, TResult> aggregateFunc, Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs, int capacity)
		{
			_sourceItems = new List<TSourceItem>(capacity);
			_aggregateFunc = funcs.aggregateFunc;
			_deaggregateFunc = funcs.deaggregateFunc;
		}

        protected override void initializeFromSource()
		{
			if (_sourceInitialized)
			{
				_sourceItems = new List<TSourceItem>(Utils.getCapacity(_sourceScalar, _source) );
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

			if (_source != null && _isActive)
			{

                Utils.initializeFromHasChangeMarker(
                    out _sourceAsIHasChangeMarker, 
                    _sourceAsList, 
                    ref _lastProcessedSourceChangeMarker, 
                    ref _sourceAsINotifyPropertyChanged,
                    (ISourceIndexerPropertyTracker)this);

				_source.CollectionChanged += handleSourceCollectionChanged;
				calculateValueAndRegisterSourceItems();

                _sourceInitialized = true;
			}
			else
			{
				setValue(default(TResult));		
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
                    //if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
                    int newIndex = e.NewStartingIndex;
                    TSourceItem addedSourceItem = _sourceAsList[newIndex];
                    _sourceItems.Insert(newIndex, addedSourceItem);
                    setValue(aggregate(addedSourceItem, _value));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    //if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
                    int oldStartingIndex = e.OldStartingIndex;
                    TSourceItem removedSourceItem = _sourceItems[oldStartingIndex];
                    _sourceItems.RemoveAt(oldStartingIndex);
                    setValue(deaggregate(removedSourceItem, _value));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    //if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
                    int newStartingIndex = e.NewStartingIndex;
                    TSourceItem newItem = _sourceAsList[newStartingIndex];
                    TSourceItem oldItem = _sourceItems[newStartingIndex];
                    _sourceItems[newStartingIndex] = newItem;
                    TResult result = deaggregate(oldItem, _value);
                    setValue(aggregate(newItem, result));
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex == e.NewStartingIndex) return;
                    int oldStartingIndex1 = e.OldStartingIndex;
                    int newStartingIndex1 = e.NewStartingIndex;
                    TSourceItem movingItem = _sourceItems[oldStartingIndex1];
                    _sourceItems.RemoveAt(oldStartingIndex1);
                    _sourceItems.Insert(newStartingIndex1, movingItem);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    initializeFromSource();
                    break;
            }

            Utils.postHandleSourceCollectionChanged(
                out _handledEventSender,
                out _handledEventArgs);
		}

		private TResult aggregate(TSourceItem addedSourceItem, TResult value)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
				TResult result = _aggregateFunc(addedSourceItem, value);
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
				return result;
			}

			return _aggregateFunc(addedSourceItem, value);
		}

		private TResult deaggregate(TSourceItem removedSourceItem, TResult value)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
                var currentThread = Utils.startComputingExecutingUserCode(out var computing, out _userCodeIsCalledFrom, this);
				TResult result = _deaggregateFunc(removedSourceItem, value);
                Utils.endComputingExecutingUserCode(computing, currentThread, out _userCodeIsCalledFrom);
				return result;
			}

			return _deaggregateFunc(removedSourceItem, value);
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;

			_handledEventSender = sender;
			_handledEventArgs = e;

			initializeFromSource();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void calculateValueAndRegisterSourceItems()
		{
			TResult value = default(TResult);
			int count = _sourceAsList.Count;
			for (int index = 0; index < count; index++)
			{
				TSourceItem sourceItem = _sourceAsList[index];
				value = aggregate(sourceItem, value);
				_sourceItems.Add(sourceItem);
			}
			setValue(value);
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
            Utils.initializeSourceScalar(_sourceScalar, ref _source, handleSourceScalarValueChanged);
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourceScalar, handleSourceScalarValueChanged, ref _source);
        }

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			if (_sourceItems.Count != sourceCount) throw new ObservableComputationsException(this, "Consistency violation: Aggregating.1");
			TResult result = default(TResult);

			for (int i = 0; i < sourceCount; i++)
			{
				TSourceItem sourceItem = source[i];
				TSourceItem savedSourceItem = _sourceItems[i];
				result = _aggregateFunc(sourceItem, result);
				if (!savedSourceItem.IsSameAs(sourceItem)) throw new ObservableComputationsException(this, "Consistency violation: Aggregating.2");
			}

			// ReSharper disable once PossibleNullReferenceException
			if (!result.Equals(_value)) throw new ObservableComputationsException(this, "Consistency violation: Aggregating.3");
		}

        #region Implementation of ISourceIndexerPropertyTracker

        void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Utils.HandleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
        }

        #endregion
    }
}
