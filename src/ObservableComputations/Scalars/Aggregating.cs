using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public class Aggregating<TSourceItem, TResult> : ScalarComputing<TResult>, IHasSourceCollections, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
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

		private IList<TSourceItem> _sourceAsList;

		private bool _sourceInitialized;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly Func<TSourceItem, TResult, TResult> _aggregateFunc;
		private readonly Func<TSourceItem, TResult, TResult> _deaggregateFunc;
		private INotifyCollectionChanged _source;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;

		[ObservableComputationsCall]
		public Aggregating(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			(Func<TSourceItem, TResult, TResult> aggregateFunc,
			Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs) : this(funcs)
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
			: this(funcs)
		{
			_source = source;
			//initializeFromSource();
		}

		private Aggregating((Func<TSourceItem, TResult, TResult> aggregateFunc, Func<TSourceItem, TResult, TResult> deaggregateFunc) funcs)
		{
			_aggregateFunc = funcs.aggregateFunc;
			_deaggregateFunc = funcs.deaggregateFunc;
			_thisAsSourceCollectionChangeProcessor = this;
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

				_sourceInitialized = false;
			}


			Utils.changeSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
				out _sourceAsList, true);

			if (_source != null && _isActive)
			{

				Utils.initializeFromHasChangeMarker(
					out _sourceAsIHasChangeMarker, 
					_sourceAsList, 
					ref _lastProcessedSourceChangeMarker, 
					ref _sourceAsINotifyPropertyChanged,
					(ISourceIndexerPropertyTracker)this);

				_source.CollectionChanged += handleSourceCollectionChanged;

				TResult value = default(TResult);
				int count = _sourceAsList.Count;
				for (int index = 0; index < count; index++)
					value = aggregate(_sourceAsList[index], value);
				setValue(value);

				_sourceInitialized = true;
			}
			else
			{
				setDefaultValue();		
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
					TSourceItem addedSourceItem = (TSourceItem) e.NewItems[0];
					setValue(aggregate(addedSourceItem, _value));
					break;
				case NotifyCollectionChangedAction.Remove:
					//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
					TSourceItem removedSourceItem = (TSourceItem) e.OldItems[0];
					setValue(deaggregate(removedSourceItem, _value));
					break;
				case NotifyCollectionChangedAction.Replace:
					//if (e.NewItems.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
					TSourceItem newItem = (TSourceItem) e.NewItems[0];
					TSourceItem oldItem = (TSourceItem) e.OldItems[0];
					TResult result = deaggregate(oldItem, _value);
					setValue(aggregate(newItem, result));
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
			}
		}

		private TResult aggregate(TSourceItem addedSourceItem, TResult value)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TResult result = _aggregateFunc(addedSourceItem, value);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return _aggregateFunc(addedSourceItem, value);
		}

		private TResult deaggregate(TSourceItem removedSourceItem, TResult value)
		{
			if (Configuration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				TResult result = _deaggregateFunc(removedSourceItem, value);
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return _deaggregateFunc(removedSourceItem, value);
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

		public void ValidateConsistency()
		{
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;

			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			TResult result = default(TResult);

			for (int i = 0; i < sourceCount; i++)
				result = _aggregateFunc(source[i], result);

			// ReSharper disable once PossibleNullReferenceException
			if (!result.Equals(_value)) throw new ObservableComputationsException(this, "Consistency violation: Aggregating.3");
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
		}

		#endregion
	}
}
