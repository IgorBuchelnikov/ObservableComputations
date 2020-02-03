using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace ObservableComputations
{
	public class Synchronizing<TSourceItem> : ObservableCollectionWithChangeMarker<TSourceItem>
	{
		public INotifyCollectionChanged Source => _source;
		public IPostingSynchronizer PostingSynchronizer => _postingSynchronizer;
		public ISendingSynchronizer SendingSynchronizer => _sendingSynchronizer;		

		private readonly INotifyCollectionChanged _source;
		private readonly IList<TSourceItem> _sourceAsList;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IPostingSynchronizer _postingSynchronizer;
		private ISendingSynchronizer _sendingSynchronizer;
		private ISynchronizer _synchronizer;

		private object _lockObject;

		[ObservableComputationsCall]
		public Synchronizing(
			INotifyCollectionChanged source,
			IPostingSynchronizer postingSynchronizer,
			object lockObject) : this(source, lockObject)
		{
			_postingSynchronizer = postingSynchronizer;
			_synchronizer = postingSynchronizer;
		}

		[ObservableComputationsCall]
		public Synchronizing(
			INotifyCollectionChanged source,
			ISendingSynchronizer sendingSynchronizer,
			object lockObject) : this(source, lockObject)
		{
			_sendingSynchronizer = sendingSynchronizer;
			_synchronizer = sendingSynchronizer;
		}

		private Synchronizing(
			INotifyCollectionChanged source,
			object lockObject)
		{
			_lockObject = lockObject;
			_source = source;
			_sourceAsList = (IList<TSourceItem>) _source;

			initializeFromSource();
		}

		private void initializeFromSource()
		{
			_sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _sourceAsList;

			_sourcePropertyChangedEventHandler = (sender, args) =>
			{
				if (args.PropertyName == "Item[]") _indexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
			};

			_sourceWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourcePropertyChangedEventHandler);

			_sourceAsINotifyPropertyChanged.PropertyChanged += _sourceWeakPropertyChangedEventHandler.Handle;

			// foreach to control modifications of _sourceAsList from another thread
			int index = 0;

			Monitor.Enter(_lockObject);
			foreach (TSourceItem sourceItem in _sourceAsList)
			{
				InsertItem(index++, sourceItem);
			}

			_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
			_sourceWeakNotifyCollectionChangedEventHandler =
				new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

			_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
			Monitor.Exit(_lockObject);
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			_postingSynchronizer?.WaitLastPostComplete();

			if (_indexerPropertyChangedEventRaised)
			{
				_indexerPropertyChangedEventRaised = false;
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
						int newStartingIndex = e.NewStartingIndex;
						_synchronizer.Invoke(() => InsertItem(newStartingIndex, _sourceAsList[newStartingIndex]), e);							
						break;
					case NotifyCollectionChangedAction.Remove:
						// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
						_synchronizer.Invoke(() => RemoveItem(e.OldStartingIndex), e);
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
						int newStartingIndex2 = e.NewStartingIndex;
						_synchronizer.Invoke(() => SetItem(newStartingIndex2, _sourceAsList[newStartingIndex2]), e);
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;
						if (oldStartingIndex1 == newStartingIndex1) return;
						_synchronizer.Invoke(() => MoveItem(oldStartingIndex1, newStartingIndex1), e);
						break;
					case NotifyCollectionChangedAction.Reset:
						_synchronizer.Invoke(() =>
						{
							_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
							_sourceNotifyCollectionChangedEventHandler = null;
							_sourceWeakNotifyCollectionChangedEventHandler = null;
							ClearItems();
							initializeFromSource();
						}, e);
						break;
				}
			}
		}

		~Synchronizing()
		{
			_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;

			if (_sourceAsINotifyPropertyChanged != null)
				_sourceAsINotifyPropertyChanged.PropertyChanged -=
					_sourceWeakPropertyChangedEventHandler.Handle;
		}
	}
}
