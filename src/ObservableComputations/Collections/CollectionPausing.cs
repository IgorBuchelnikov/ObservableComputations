using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace ObservableComputations
{
	public class CollectionPausing<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		public INotifyCollectionChanged Source => _source;
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;
		public IReadScalar<bool> IsPausedScalar => _isPausedScalar;
		public bool Resuming => _resuming;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public bool IsPaused
		{
			get => _isPaused;
			set
			{
				checkConsistent(null, null);
				
				if (_isPausedScalar != null) 
					throw new ObservableComputationsException("Modifying of IsPaused property is controlled by IsPausedScalar");
				
				_resuming = _isPaused != value && !value;
				_isPaused = value;
				OnPropertyChanged(Utils.PausedPropertyChangedEventArgs);

				if (_resuming) resume();
			}
		}

		public CollectionPausingResumeType ResumeType
		{
			get => _resumeType;
			set
			{
				if (_isPaused)
				{
					if (_resumeType == CollectionPausingResumeType.Reset 
						&& value == CollectionPausingResumeType.ReplayChanges)
						throw new ObservableComputationsException(this, "It is impossible to change ResumeType from Reset to ReplayChanges while IsPaused is true");

					if (_resumeType == CollectionPausingResumeType.ReplayChanges
						&& value == CollectionPausingResumeType.Reset)
						_deferredCollectionActions.Clear();

				}

				_resumeType = value;
				OnPropertyChanged(Utils.ResumeTypePropertyChangedEventArgs);
			}
		}

		private void resume()
		{
			_isConsistent = false;
			if (_resumeType == CollectionPausingResumeType.ReplayChanges)
			{
				int count = _deferredCollectionActions.Count;

				for (int i = 0; i < count; i++)
				{
					DeferredCollectionAction<TSourceItem> deferredCollectionAction = _deferredCollectionActions.Dequeue();
					if (deferredCollectionAction.NotifyCollectionChangedEventAgs != null)
						handleSourceCollectionChanged(deferredCollectionAction.EventSender,
							deferredCollectionAction.NotifyCollectionChangedEventAgs);
					else if (deferredCollectionAction.Clear)
					{
						_items.Clear();
					}
					else if (deferredCollectionAction.Reset)
					{
						_handledEventSender = deferredCollectionAction.EventSender;
						_handledEventArgs = deferredCollectionAction.EventArgs;

						reset();

						_handledEventSender = null;
						_handledEventArgs = null;
					}
					else if (deferredCollectionAction.NewItems != null)
					{
						int originalCount = _items.Count;
						TSourceItem[] newItems = deferredCollectionAction.NewItems;
						int count1 = newItems.Length;
						int sourceIndex;
						for (sourceIndex = 0; sourceIndex < count1; sourceIndex++)
						{
							if (originalCount > sourceIndex)
								_items[sourceIndex] = newItems[sourceIndex];
							else
								_items.Insert(sourceIndex, newItems[sourceIndex]);
						}

						for (int index = originalCount - 1; index >= sourceIndex; index--)
						{
							_items.RemoveAt(index);
						}
					}
				}

				_resuming = false;

				Utils.postHandleChange(
					ref _handledEventSender,
					ref _handledEventArgs,
					_deferredProcessings,
					ref _isConsistent,
					this);
			}
			else //if (_resumeType == CollectionPausingResumeType.Reset)
			{
				initializeFromSource();
			}

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private readonly IReadScalar<bool> _isPausedScalar;

		private bool _sourceInitialized;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;
		private bool _isPaused;
		private bool _resuming;
		private CollectionPausingResumeType _resumeType;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;

		readonly Queue<DeferredCollectionAction<TSourceItem>> _deferredCollectionActions = new Queue<DeferredCollectionAction<TSourceItem>>();

		[ObservableComputationsCall]
		public CollectionPausing(
			INotifyCollectionChanged source,
			bool initialIsPaused = false,
			CollectionPausingResumeType resumeType = CollectionPausingResumeType.Reset)
		{
			_isPaused = initialIsPaused;
			_source = source;
			_resumeType = resumeType;
			_thisAsSourceCollectionChangeProcessor = this;		  
		}

		[ObservableComputationsCall]
		public CollectionPausing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			bool initialIsPaused = false,
			CollectionPausingResumeType resumeType = CollectionPausingResumeType.Reset)
		{
			_isPaused = initialIsPaused;
			_sourceScalar = sourceScalar;
			_resumeType = resumeType;
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public CollectionPausing(
			INotifyCollectionChanged source,
			IReadScalar<bool> isPausedScalar,
			CollectionPausingResumeType resumeType = CollectionPausingResumeType.Reset)
		{
			_isPausedScalar = isPausedScalar;
			_source = source;
			_thisAsSourceCollectionChangeProcessor = this;
			_resumeType = resumeType;
		}

		[ObservableComputationsCall]
		public CollectionPausing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<bool> isPausedScalar,
			CollectionPausingResumeType resumeType = CollectionPausingResumeType.Reset)
		{
			_isPausedScalar = isPausedScalar;
			_sourceScalar = sourceScalar;
			_thisAsSourceCollectionChangeProcessor = this;
			_resumeType = resumeType;
		}

		private void initializeIsPausedScalar()
		{
			if (_isPausedScalar != null)
			{
				_isPausedScalar.PropertyChanged += handleIsPausedScalarValueChanged;
				_isPaused = _isPausedScalar.Value;
			}
		}

		private void handleIsPausedScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			if (_sourceInitialized) checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			bool newValue = _isPausedScalar.Value;
			_resuming = _isPaused != newValue && !newValue;
			_isPaused = newValue;

			if (_resuming) resume();

			_handledEventSender = null;
			_handledEventArgs = null;
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
				out _sourceAsList, true);

			if (_sourceAsList != null && _isActive)
			{
				Utils.initializeFromHasChangeMarker(
					out _sourceAsIHasChangeMarker, 
					_sourceAsList, 
					ref _lastProcessedSourceChangeMarker, 
					ref _sourceAsINotifyPropertyChanged,
					(ISourceIndexerPropertyTracker)this);

				int sourceIndex = 0;

				if (_isPaused)
					_deferredCollectionActions.Enqueue(new DeferredCollectionAction<TSourceItem>(_sourceAsList.ToArray()));
				else
				{
					int count = _sourceAsList.Count;
					TSourceItem[] sourceCopy = new TSourceItem[count];
					_sourceAsList.CopyTo(sourceCopy, 0);

					_source.CollectionChanged += handleSourceCollectionChanged;

					for (; sourceIndex < count; sourceIndex++)
					{
						if (originalCount > sourceIndex)
							_items[sourceIndex] = sourceCopy[sourceIndex];
						else
							_items.Insert(sourceIndex, sourceCopy[sourceIndex]);
					}					
				}

				if (!_isPaused)
				{
					for (int index = originalCount - 1; index >= sourceIndex; index--)
					{
						_items.RemoveAt(index);
					}
				}
		  
				_sourceInitialized = true;
			}			
			else 
			{
				if (_isPaused)
					_deferredCollectionActions.Enqueue(new DeferredCollectionAction<TSourceItem>(true));
				else
					_items.Clear();
			}

			if (_isPaused)
				_deferredCollectionActions.Enqueue(new DeferredCollectionAction<TSourceItem>(true, _handledEventSender, _handledEventArgs));
			else
				reset();
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

			_isConsistent = !_resuming && !_isPaused;

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
			if (_isPaused && e.Action != NotifyCollectionChangedAction.Reset)
			{
				_deferredCollectionActions.Enqueue(new DeferredCollectionAction<TSourceItem>(sender, e));
				return;
			}

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
					baseInsertItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
					break;
				case NotifyCollectionChangedAction.Remove:
					// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
					baseRemoveItem(e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
					baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex1 = e.OldStartingIndex;
					int newStartingIndex1 = e.NewStartingIndex;
					if (oldStartingIndex1 != newStartingIndex1)
						baseMoveItem(oldStartingIndex1, newStartingIndex1);

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
			(_isPausedScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			(_source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(_isPausedScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		protected override void initialize()
		{	 
			Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);
			initializeIsPausedScalar();
		}

		protected override void uninitialize()
		{
			Utils.uninitializeSourceScalar(_sourceScalar, scalarValueChangedHandler, ref _source);
			if (_isPausedScalar != null) _isPausedScalar.PropertyChanged -= handleIsPausedScalarValueChanged;
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
		}

		#endregion
	}

	internal struct DeferredCollectionAction<TSourceItem>
	{
		public object EventSender;
		public readonly NotifyCollectionChangedEventArgs NotifyCollectionChangedEventAgs;
		public EventArgs EventArgs;
		public TSourceItem[] NewItems;
		public bool Clear;
		public bool Reset;

		public DeferredCollectionAction(object eventSender, NotifyCollectionChangedEventArgs eventAgs) : this()
		{
			EventSender = eventSender;
			NotifyCollectionChangedEventAgs = eventAgs;
		}

		public DeferredCollectionAction(TSourceItem[] newItems) : this()
		{
			NewItems = newItems;
		}

		public DeferredCollectionAction(bool clear) : this()
		{
			Clear = clear;
		}

		public DeferredCollectionAction(bool reset, object eventSender, EventArgs eventArgs) : this()
		{
			Reset = reset;
			EventSender = eventSender;
			EventArgs = eventArgs;
		}
	}

	public enum CollectionPausingResumeType
	{
		Reset,
		ReplayChanges
	}
}
