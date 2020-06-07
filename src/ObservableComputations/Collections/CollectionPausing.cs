using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class CollectionPausing<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections
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
				if (_isPausedScalar != null) throw new ObservableComputationsException("Modifying of IsPaused property is controlled by IsPausedScalar");
				_resuming = _isPaused != value && value;
				_isPaused = value;
				OnPropertyChanged(Utils.PausedPropertyChangedEventArgs);

				if (_resuming)
				{
					resume();
				}
			}
		}

		private void resume()
		{
			DefferedCollectionAction<TSourceItem> defferedCollectionAction;
			int count = _defferedCollectionActions.Count;

			_isConsistent = false;

			for (int i = 0; i < count; i++)
			{
				defferedCollectionAction = _defferedCollectionActions.Dequeue();
				if (defferedCollectionAction.NotifyCollectionChangedEventAgs != null)
					handleSourceCollectionChanged(defferedCollectionAction.EventSender,
						defferedCollectionAction.NotifyCollectionChangedEventAgs);
				else if (defferedCollectionAction.Clear)
				{
					_handledEventSender = defferedCollectionAction.EventSender;
					_handledEventArgs = defferedCollectionAction.EventArgs;

					baseClearItems();

					_handledEventSender = null;
					_handledEventArgs = null;
				}
				else
				{
					_handledEventSender = defferedCollectionAction.EventSender;
					_handledEventArgs = defferedCollectionAction.EventArgs;

					baseInsertItem(defferedCollectionAction.NewItemIndex, defferedCollectionAction.NewItem);

					_handledEventSender = null;
					_handledEventArgs = null;
				}
			}

			_isConsistent = true;
			raiseConsistencyRestored();

			_resuming = false;
		}

		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private IReadScalar<INotifyCollectionChanged> _sourceScalar;

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private IReadScalar<bool> _isPausedScalar;
		private PropertyChangedEventHandler _isPausedScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _isPausedScalarWeakPropertyChangedEventHandler;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;
		private bool _isPaused;
		private bool _resuming;

		Queue<DefferedCollectionAction<TSourceItem>> _defferedCollectionActions = new Queue<DefferedCollectionAction<TSourceItem>>();

		[ObservableComputationsCall]
		public CollectionPausing(
			INotifyCollectionChanged source,
			bool initialIsPaused = false)
		{
			_isPaused = initialIsPaused;
			_source = source;

			initializeFromSource();
		}


		[ObservableComputationsCall]
		public CollectionPausing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			bool initialIsPaused = false)
		{
			_isPaused = initialIsPaused;
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		[ObservableComputationsCall]
		public CollectionPausing(
			INotifyCollectionChanged source,
			IReadScalar<bool> isPausedScalar)
		{
			_isPausedScalar = isPausedScalar;
			_isPausedScalarPropertyChangedEventHandler = handleIsPausedScalarValueChanged;
			_isPausedScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_isPausedScalarPropertyChangedEventHandler);
			_isPausedScalar.PropertyChanged += _isPausedScalarWeakPropertyChangedEventHandler.Handle;
			_isPaused = isPausedScalar.Value;

			_source = source;

			initializeFromSource();
		}


		[ObservableComputationsCall]
		public CollectionPausing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IReadScalar<bool> isPausedScalar)
		{
			_isPausedScalar = isPausedScalar;
			_isPausedScalarPropertyChangedEventHandler = handleIsPausedScalarValueChanged;
			_isPausedScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_isPausedScalarPropertyChangedEventHandler);
			_isPausedScalar.PropertyChanged += _isPausedScalarWeakPropertyChangedEventHandler.Handle;
			_isPaused = isPausedScalar.Value;

			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;
			_isConsistent = false;

			initializeFromSource();

			_isConsistent = true;
			raiseConsistencyRestored();
			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void handleIsPausedScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;

			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			bool newValue = _isPausedScalar.Value;
			_resuming = _isPaused != newValue && newValue;
			_isPaused = newValue;

			if (_resuming) resume();

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				if (_isPaused)
					_defferedCollectionActions.Enqueue(new DefferedCollectionAction<TSourceItem>(_handledEventSender, _handledEventArgs));
				else
					baseClearItems();
				
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				_sourceNotifyCollectionChangedEventHandler = null;
				_sourceWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_sourceAsINotifyPropertyChanged != null)
			{
				_sourceAsINotifyPropertyChanged.PropertyChanged -=
					_sourceWeakPropertyChangedEventHandler.Handle;

				_sourceAsINotifyPropertyChanged = null;
				_sourcePropertyChangedEventHandler = null;
				_sourceWeakPropertyChangedEventHandler = null;
			}

			if (_sourceScalar != null) _source = _sourceScalar.Value;
			_sourceAsList = _source as IList<TSourceItem>;

			if (_sourceAsList != null)
			{
				_sourceAsIHasChangeMarker = _sourceAsList as IHasChangeMarker;

				if (_sourceAsIHasChangeMarker != null)
				{
					_lastProcessedSourceChangeMarker = _sourceAsIHasChangeMarker.ChangeMarker;
				}
				else
				{
					_sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _sourceAsList;

					_sourcePropertyChangedEventHandler = (sender1, args1) =>
					{
						if (args1.PropertyName == "Item[]")
							_indexerPropertyChangedEventRaised =
								true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_sourceWeakPropertyChangedEventHandler =
						new WeakPropertyChangedEventHandler(_sourcePropertyChangedEventHandler);

					_sourceAsINotifyPropertyChanged.PropertyChanged += _sourceWeakPropertyChangedEventHandler.Handle;
				}

				int count = _sourceAsList.Count;
				for (var sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];

					if (_isPaused)
						_defferedCollectionActions.Enqueue(new DefferedCollectionAction<TSourceItem>(_handledEventSender, _handledEventArgs, sourceItem, sourceIndex));
					else			
						baseInsertItem(sourceIndex, sourceItem);
				}

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
				_sourceWeakNotifyCollectionChangedEventHandler =
					new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);
				_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent(sender, e);

			_handledEventSender = sender;
			_handledEventArgs = e;

			if (!_resuming && !_isPaused)
			{
				_isConsistent = false;
			}

			if (_indexerPropertyChangedEventRaised || _lastProcessedSourceChangeMarker != _sourceAsIHasChangeMarker.ChangeMarker)
			{
				_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;
				_indexerPropertyChangedEventRaised = false;

				if (_isPaused && e.Action != NotifyCollectionChangedAction.Reset)
				{
					_defferedCollectionActions.Enqueue(new DefferedCollectionAction<TSourceItem>(sender, e));
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

			if (!_resuming && !_isPaused)
			{		
				_isConsistent = true;
				raiseConsistencyRestored();
			}

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		~CollectionPausing()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_isPausedScalarWeakPropertyChangedEventHandler != null)
			{
				_isPausedScalar.PropertyChanged -= _isPausedScalarWeakPropertyChangedEventHandler.Handle;			
			}
		}
	}

	internal struct DefferedCollectionAction<TSourceItem>
	{
		public object EventSender;
		public NotifyCollectionChangedEventArgs NotifyCollectionChangedEventAgs;
		public EventArgs EventArgs;
		public bool Clear;
		public TSourceItem NewItem;
		public int NewItemIndex;

		public DefferedCollectionAction(object eventSender, NotifyCollectionChangedEventArgs eventAgs) : this()
		{
			EventSender = eventSender;
			NotifyCollectionChangedEventAgs = eventAgs;
		}

		public DefferedCollectionAction(object eventSender, EventArgs eventArgs) : this()
		{
			EventSender = eventSender;
			EventArgs = eventArgs;
			Clear = true;
		}

		public DefferedCollectionAction(object eventSender, EventArgs eventArgs, TSourceItem newItem, int newItemIndex) : this()
		{
			EventSender = eventSender;
			EventArgs = eventArgs;
			NewItem = newItem;
			NewItemIndex = -newItemIndex;
		}
	}
}
