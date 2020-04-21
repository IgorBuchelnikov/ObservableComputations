using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class CollectionSynchronizing<TSourceItem> : CollectionComputing<TSourceItem>
	{
		public INotifyCollectionChanged Source => _source;
		public IDestinationCollectionSynchronizer DestinationCollectionSynchronizer => _destinationCollectionSynchronizer;
		public ISourceCollectionSynchronizer SourceCollectionSynchronizer => _sourceCollectionSynchronizer;

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

		private IPostingSynchronizer _destinationPostingSynchronizer;
		private IDestinationCollectionSynchronizer _destinationCollectionSynchronizer;

		private ISourceCollectionSynchronizer _sourceCollectionSynchronizer;

		private IHasChangeMarker _sourceAsIHasChangeMarker;
		private bool _lastProcessedSourceChangeMarker;

		[ObservableComputationsCall]
		public CollectionSynchronizing(
			INotifyCollectionChanged source,
			ISourceCollectionSynchronizer sourceCollectionSynchronizer,
			IDestinationCollectionSynchronizer destinationCollectionSynchronizer) : this(sourceCollectionSynchronizer, destinationCollectionSynchronizer)
		{
			_source = source;
			initializeFromSource();
		}

		[ObservableComputationsCall]
		public CollectionSynchronizing(
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			ISourceCollectionSynchronizer sourceCollectionSynchronizer,
			IDestinationCollectionSynchronizer destinationCollectionSynchronizer) : this(sourceCollectionSynchronizer, destinationCollectionSynchronizer)
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		private CollectionSynchronizing(ISourceCollectionSynchronizer sourceCollectionSynchronizer,
			IDestinationCollectionSynchronizer destinationCollectionSynchronizer)
		{
			_sourceCollectionSynchronizer = sourceCollectionSynchronizer;
			_destinationCollectionSynchronizer = destinationCollectionSynchronizer;
			_destinationPostingSynchronizer = destinationCollectionSynchronizer as IPostingSynchronizer;
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


		private void initializeFromSource(object senderSourceScalar = null, PropertyChangedEventArgs sourceScalarPropertyChangedEventArgs = null)
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

					_sourcePropertyChangedEventHandler = (sender, args) =>
					{
						if (args.PropertyName == "Item[]") _indexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_sourceWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourcePropertyChangedEventHandler);

					_sourceAsINotifyPropertyChanged.PropertyChanged += _sourceWeakPropertyChangedEventHandler.Handle;
				}

				_sourceCollectionSynchronizer.InvokeReadAndSubscribe(() =>
				{
					int index = 0;
					for (var sourceIndex = 0; sourceIndex < _sourceAsList.Count; sourceIndex++)
					{
						TSourceItem sourceItem = _sourceAsList[sourceIndex];
						_destinationPostingSynchronizer?.WaitLastPostComplete();
						_destinationCollectionSynchronizer.InvokeInitializeFromSourceCollection(
							() => { baseInsertItem(index++, sourceItem); }, 
							this, sourceItem, 
							senderSourceScalar, sourceScalarPropertyChangedEventArgs);
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
			_destinationPostingSynchronizer?.WaitLastPostComplete();
			checkConsistent();
			if (_indexerPropertyChangedEventRaised || _lastProcessedSourceChangeMarker != _sourceAsIHasChangeMarker.ChangeMarker)
			{
				_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;
				_indexerPropertyChangedEventRaised = false;


				_isConsistent = false;

				_indexerPropertyChangedEventRaised = false;

				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
						int newStartingIndex = e.NewStartingIndex;
						_destinationCollectionSynchronizer.InvokeCollectionChange(
							() => baseInsertItem(newStartingIndex, (TSourceItem) e.NewItems[0]), 
							this, sender, e);
						break;
					case NotifyCollectionChangedAction.Remove:
						// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
						_destinationCollectionSynchronizer.InvokeCollectionChange(
							() => baseRemoveItem(e.OldStartingIndex),
							this, sender, e);
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
						_destinationCollectionSynchronizer.InvokeCollectionChange(
							() => baseSetItem(e.NewStartingIndex, (TSourceItem) e.NewItems[0]),
							this, sender, e);
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;
						if (oldStartingIndex1 == newStartingIndex1) return;
						_destinationCollectionSynchronizer.InvokeCollectionChange(
							() => baseMoveItem(oldStartingIndex1, newStartingIndex1), 
							this, sender, e);
						break;
					case NotifyCollectionChangedAction.Reset:
						_destinationCollectionSynchronizer.InvokeCollectionChange(() =>
						{
							_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
							_sourceNotifyCollectionChangedEventHandler = null;
							_sourceWeakNotifyCollectionChangedEventHandler = null;
							baseClearItems();
							initializeFromSource();
						}, this, sender, e);
						break;
				}

				_isConsistent = true;
				_destinationCollectionSynchronizer.InvokeRaiseConsistencyRestored(
					raiseConsistencyRestored, this);
			}
		}

		~CollectionSynchronizing()
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
