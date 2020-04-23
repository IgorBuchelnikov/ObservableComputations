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
		public IDestinationCollectionDispatcher DestinationCollectionDispatcher => _destinationCollectionDispatcher;
		public ISourceCollectionDispatcher SourceCollectionDispatcher => _sourceCollectionDispatcher;

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

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		[ObservableComputationsCall]
		public CollectionDispatching(
			INotifyCollectionChanged source,
			ISourceCollectionDispatcher sourceCollectionDispatcher,
			IDestinationCollectionDispatcher destinationCollectionDispatcher) : this(sourceCollectionDispatcher, destinationCollectionDispatcher)
		{
			_source = source;
			initializeFromSource();
		}

		[ObservableComputationsCall]
		public CollectionDispatching(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			ISourceCollectionDispatcher sourceCollectionDispatcher,
			IDestinationCollectionDispatcher destinationCollectionDispatcher) : this(sourceCollectionDispatcher, destinationCollectionDispatcher)
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		private CollectionDispatching(ISourceCollectionDispatcher sourceCollectionDispatcher,
			IDestinationCollectionDispatcher destinationCollectionDispatcher)
		{
			_sourceCollectionDispatcher = sourceCollectionDispatcher;
			_destinationCollectionDispatcher = destinationCollectionDispatcher;
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

				_sourceCollectionDispatcher.InvokeReadAndSubscribe(() =>
				{
					int index = 0;
					for (var sourceIndex = 0; sourceIndex < _sourceAsList.Count; sourceIndex++)
					{
						TSourceItem sourceItem = _sourceAsList[sourceIndex];

						void insertItem() => baseInsertItem(index++, sourceItem);

						if (sender == null)
							insertItem();
						else
							_destinationCollectionDispatcher.InvokeInitializeFromSourceCollection(
								insertItem, this, sourceItem, sender, eventArgs);							

					}

					_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
					_sourceWeakNotifyCollectionChangedEventHandler =
						new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);
					_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				}, this);
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
						_destinationCollectionDispatcher.InvokeCollectionChange(
							() =>
							{
								checkConsistent();
								_isConsistent = false;

								baseInsertItem(newStartingIndex, (TSourceItem) e.NewItems[0]);

								_isConsistent = true;
								raiseConsistencyRestored();
							}, 
							this, sender, e);
						break;
					case NotifyCollectionChangedAction.Remove:
						// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
						_destinationCollectionDispatcher.InvokeCollectionChange(
							() =>
							{
								checkConsistent();
								_isConsistent = false;

								baseRemoveItem(e.OldStartingIndex);

								_isConsistent = true;
								raiseConsistencyRestored();
							},
							this, sender, e);
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
						_destinationCollectionDispatcher.InvokeCollectionChange(
							() =>
							{
								checkConsistent();
								_isConsistent = false;

								baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]);
		
								_isConsistent = true;
								raiseConsistencyRestored();
							},
							this, sender, e);
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;
						if (oldStartingIndex1 == newStartingIndex1) return;
						_destinationCollectionDispatcher.InvokeCollectionChange(
							() =>
							{
								checkConsistent();
								_isConsistent = false;

								baseMoveItem(oldStartingIndex1, newStartingIndex1);
								
								_isConsistent = true;
								raiseConsistencyRestored();
							}, 
							this, sender, e);
						break;
					case NotifyCollectionChangedAction.Reset:
						_destinationCollectionDispatcher.InvokeCollectionChange(() =>
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
						}, this, sender, e);
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
