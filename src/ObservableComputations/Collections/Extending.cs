using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class Extending<TSourceItem> : ObservableCollectionWithChangeMarker<TSourceItem>
	{
		public INotifyCollectionChanged Source => _source;	

		private readonly INotifyCollectionChanged _source;
		private readonly IList<TSourceItem> _sourceAsList;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private Extending(
			INotifyCollectionChanged source)
		{
			_source = source;
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
			foreach (TSourceItem sourceItem in _sourceAsList)
			{
				InsertItem(index++, sourceItem);
			}

			_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
			_sourceWeakNotifyCollectionChangedEventHandler =
				new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

			_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_indexerPropertyChangedEventRaised)
			{
				_indexerPropertyChangedEventRaised = false;
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Adding of multiple items is not supported");
						int newStartingIndex = e.NewStartingIndex;
						InsertItem(newStartingIndex, _sourceAsList[newStartingIndex]);							
						break;
					case NotifyCollectionChangedAction.Remove:
						// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
						RemoveItem(e.OldStartingIndex);
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
						int newStartingIndex2 = e.NewStartingIndex;
						SetItem(newStartingIndex2, _sourceAsList[newStartingIndex2]);
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;
						if (oldStartingIndex1 == newStartingIndex1) return;
						MoveItem(oldStartingIndex1, newStartingIndex1);
						break;
					case NotifyCollectionChangedAction.Reset:
						_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
						_sourceNotifyCollectionChangedEventHandler = null;
						_sourceWeakNotifyCollectionChangedEventHandler = null;
						ClearItems();
						initializeFromSource();
						break;
				}
			}
		}

		~Extending()
		{
			_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;

			if (_sourceAsINotifyPropertyChanged != null)
				_sourceAsINotifyPropertyChanged.PropertyChanged -=
					_sourceWeakPropertyChangedEventHandler.Handle;
		}
	}
}
