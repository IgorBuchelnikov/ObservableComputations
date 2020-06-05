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
		public bool Resuming => _resuming;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		public bool Paused
		{
			get => _paused;
			set
			{
				_resuming = _paused != value && value;
				_paused = value;
				OnPropertyChanged(Utils.PausedPropertyChangedEventArgs);

				DefferedCollectionAction defferedCollectionAction;
				int count = _defferedCollectionActions.Count;

				if (_resuming)
				{
					_isConsistent = false;

					for (int i = 0; i < count; i++)
					{
						defferedCollectionAction = _defferedCollectionActions.Dequeue();
						if (defferedCollectionAction.NotifyCollectionChangedEventAgs != null)
							handleSourceCollectionChanged(defferedCollectionAction.EventSender, defferedCollectionAction.NotifyCollectionChangedEventAgs);		
						else if (defferedCollectionAction.PropertyChangedEventAgs != null)
							handleSourceScalarValueChanged(defferedCollectionAction.EventSender, defferedCollectionAction.PropertyChangedEventAgs, defferedCollectionAction.SourceScalarValue);
						else
							initializeFromSource(defferedCollectionAction.SourceScalarValue);
					}

					_isConsistent = true;
					raiseConsistencyRestored();

					_resuming = false;
				}

			}
		}

		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private IReadScalar<INotifyCollectionChanged> _sourceScalar;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;
		private bool _paused;
		private bool _resuming;

		Queue<DefferedCollectionAction> _defferedCollectionActions = new Queue<DefferedCollectionAction>();

		[ObservableComputationsCall]
		public CollectionPausing(
			INotifyCollectionChanged source,
			bool paused = false)
		{
			_paused = paused;
			_source = source;

			if (paused)
				_defferedCollectionActions.Enqueue(new DefferedCollectionAction(null));
			else
				initializeFromSource(null);
		}


		[ObservableComputationsCall]
		public CollectionPausing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			bool paused = false)
		{
			_paused = paused;
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			if (paused)
				_defferedCollectionActions.Enqueue(new DefferedCollectionAction(null));
			else
				initializeFromSource(_sourceScalar.Value);
		}


		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			handleSourceScalarValueChanged(sender, e, _sourceScalar.Value);
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e, INotifyCollectionChanged newScalarValue)
		{

			if (_paused)
			{
				_defferedCollectionActions.Enqueue(new DefferedCollectionAction(sender, e, _sourceScalar.Value));
				return;
			}

			_handledEventSender = sender;
			_handledEventArgs = e;

			initializeFromSource(newScalarValue);

			_handledEventSender = null;
			_handledEventArgs = null;
		}

		private void initializeFromSource(INotifyCollectionChanged sourceScalarValue)
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
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

			if (_sourceScalar != null) _source = sourceScalarValue;
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

			if (_paused)
			{
				_defferedCollectionActions.Enqueue(new DefferedCollectionAction(sender, e));
				return;
			}

			_handledEventSender = sender;
			_handledEventArgs = e;
			if (!_resuming) _isConsistent = false;

			if (_indexerPropertyChangedEventRaised || _lastProcessedSourceChangeMarker != _sourceAsIHasChangeMarker.ChangeMarker)
			{
				_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;
				_indexerPropertyChangedEventRaised = false;

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
						baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;
						if (oldStartingIndex1 != newStartingIndex1)
						{
							baseMoveItem(oldStartingIndex1, newStartingIndex1);							
						}

						break;
					case NotifyCollectionChangedAction.Reset:
						initializeFromSource(_sourceScalar != null ? _sourceScalar.Value : null);
						break;
				}
			}

			if (!_resuming)
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

		}
	}

	internal struct DefferedCollectionAction
	{
		public object EventSender;
		public NotifyCollectionChangedEventArgs NotifyCollectionChangedEventAgs;
		public PropertyChangedEventArgs PropertyChangedEventAgs;
		public INotifyCollectionChanged SourceScalarValue;

		public DefferedCollectionAction(object eventSender, PropertyChangedEventArgs eventAgs, INotifyCollectionChanged sourceScalarValue)
		{
			EventSender = eventSender;
			PropertyChangedEventAgs = eventAgs;
			NotifyCollectionChangedEventAgs = null;
			SourceScalarValue = sourceScalarValue;
		}

		public DefferedCollectionAction(object eventSender, NotifyCollectionChangedEventArgs eventAgs) : this()
		{
			EventSender = eventSender;
			NotifyCollectionChangedEventAgs = eventAgs;
		}

		public DefferedCollectionAction(INotifyCollectionChanged sourceScalarValue)
		{
			EventSender = null;
			PropertyChangedEventAgs = null;
			NotifyCollectionChangedEventAgs = null;
			SourceScalarValue = sourceScalarValue;
		}
	}
}
