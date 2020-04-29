using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class CollectionDispatching<TSourceItem> : CollectionComputing<TSourceItem>
	{
		public INotifyCollectionChanged Source => _source;
		public IDispatcher DestinationCollectionDispatcher => _destinationDispatcher;
		public IDispatcher SourceCollectionDispatcher => _sourceDispatcher;

		private INotifyCollectionChanged _source;
		private IList<TSourceItem> _sourceAsList;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IDestinationCollectionDispatcher _destinationCollectionDispatcher;
		private ISourceCollectionDispatcher _sourceCollectionDispatcher;

		private IDispatcher _destinationDispatcher;
		private IDispatcher _sourceDispatcher;



		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		[ObservableComputationsCall]
		public CollectionDispatching(
			INotifyCollectionChanged source,
			IDispatcher sourceDispatcher,
			IDispatcher destinationDispatcher) : this(sourceDispatcher, destinationDispatcher)
		{
			_source = source;
			initializeFromSource();
		}

		[ObservableComputationsCall]
		public CollectionDispatching(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			IDispatcher sourceDispatcher,
			IDispatcher destinationDispatcher) : this(sourceDispatcher, destinationDispatcher)
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		private CollectionDispatching(
			IDispatcher sourceDispatcher,
			IDispatcher destinationDispatcher)
		{
			_sourceDispatcher = sourceDispatcher;
			_destinationDispatcher = destinationDispatcher;

			_sourceCollectionDispatcher = sourceDispatcher as ISourceCollectionDispatcher;
			_destinationCollectionDispatcher = destinationDispatcher as IDestinationCollectionDispatcher;
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent();

			_isConsistent = false;

			initializeFromSource(sender, e);

			_isConsistent = true;
			raiseConsistencyRestored();
		}


		private void initializeFromSource(object sender = null, EventArgs eventArgs = null)
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
						if (args1.PropertyName == "Item[]") _indexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_sourceWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourcePropertyChangedEventHandler);

					_sourceAsINotifyPropertyChanged.PropertyChanged += _sourceWeakPropertyChangedEventHandler.Handle;
				}

				void readAndSubscribe()
				{
					int index = 0;
					for (var sourceIndex = 0; sourceIndex < _sourceAsList.Count; sourceIndex++)
					{
						TSourceItem sourceItem = _sourceAsList[sourceIndex];

						void insertItem() => baseInsertItem(index++, sourceItem);

						if (sender == null)
							insertItem();
						else
						{
							if (_destinationCollectionDispatcher != null)
								_destinationCollectionDispatcher.InvokeInitializeFromSourceCollection(insertItem, this, sourceItem, sender, eventArgs);
							else
								_destinationDispatcher.Invoke(insertItem);
						}
					}

					_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
					_sourceWeakNotifyCollectionChangedEventHandler = new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);
					_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				}

				if (_sourceCollectionDispatcher != null)
					_sourceCollectionDispatcher.InvokeReadAndSubscribe(readAndSubscribe, this);
				else
					_sourceDispatcher.Invoke(readAndSubscribe);
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_indexerPropertyChangedEventRaised || _lastProcessedSourceChangeMarker != _sourceAsIHasChangeMarker.ChangeMarker)
			{
				_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;
				_indexerPropertyChangedEventRaised = false;

				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
						int newStartingIndex = e.NewStartingIndex;

						void add()
						{
							checkConsistent();
							_isConsistent = false;

							baseInsertItem(newStartingIndex, (TSourceItem) e.NewItems[0]);

							_isConsistent = true;
							raiseConsistencyRestored();
						}

						if (_destinationCollectionDispatcher != null)
							_destinationCollectionDispatcher.InvokeCollectionChange(
								add,
								this, sender, e);
						else
							_destinationDispatcher.Invoke(add);	
						break;
					case NotifyCollectionChangedAction.Remove:
						// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
						void remove()
						{
							checkConsistent();
							_isConsistent = false;

							baseRemoveItem(e.OldStartingIndex);

							_isConsistent = true;
							raiseConsistencyRestored();
						}

						if (_destinationCollectionDispatcher != null)
							_destinationCollectionDispatcher.InvokeCollectionChange(
								remove,
								this, sender, e);
						else
							_destinationDispatcher.Invoke(remove);	
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
						void replace()
						{
							checkConsistent();
							_isConsistent = false;

							baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);

							_isConsistent = true;
							raiseConsistencyRestored();
						}

						if (_destinationCollectionDispatcher != null)
							_destinationCollectionDispatcher.InvokeCollectionChange(
								replace,
								this, sender, e);
						else
							_destinationDispatcher.Invoke(replace);	
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;
						if (oldStartingIndex1 == newStartingIndex1) return;

						void move()
						{
							checkConsistent();
							_isConsistent = false;

							baseMoveItem(oldStartingIndex1, newStartingIndex1);

							_isConsistent = true;
							raiseConsistencyRestored();
						}

						if (_destinationCollectionDispatcher != null)
							_destinationCollectionDispatcher.InvokeCollectionChange(
								move,
								this, sender, e);
						else
							_destinationDispatcher.Invoke(move);	
						break;
					case NotifyCollectionChangedAction.Reset:
						void reset()
						{
							checkConsistent();
							_isConsistent = false;

							_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
							_sourceNotifyCollectionChangedEventHandler = null;
							_sourceWeakNotifyCollectionChangedEventHandler = null;
							baseClearItems();
							initializeFromSource(sender, e);

							_isConsistent = true;
							raiseConsistencyRestored();
						}

						if (_destinationCollectionDispatcher != null)
							_destinationCollectionDispatcher.InvokeCollectionChange(
								reset,
								this, sender, e);
						else
							_destinationDispatcher.Invoke(reset);	
						break;
				}
			}
		}

		~CollectionDispatching()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}
;
			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}
		}
	}
}
