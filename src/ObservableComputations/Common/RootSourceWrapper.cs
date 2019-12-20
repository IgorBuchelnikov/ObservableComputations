using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ObservableComputations.Common.Base;

namespace ObservableComputations.Common
{
	internal sealed class RootSourceWrapper<TSourceItem> : ObservableCollectionWithChangeMarker<TSourceItem>
	{
		private readonly INotifyCollectionChanged _source;
		private readonly IList<TSourceItem> _sourceAsList;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private PropertyChangedEventHandler _sourcePropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;


		internal RootSourceWrapper(
			INotifyCollectionChanged source)
		{
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

			int count = _sourceAsList.Count;
			for (int index = 0; index < count; index++)
			{
				TSourceItem sourceItem = _sourceAsList[index];
				InsertItem(Count, sourceItem);
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
						TSourceItem addedItem = _sourceAsList[newStartingIndex];
						InsertItem(newStartingIndex, addedItem);								
						break;
					case NotifyCollectionChangedAction.Remove:
						// (e.OldItems.Count > 1) throw new ObservableComputationsException("Removing of multiple items is not supported");
						int oldStartingIndex = e.OldStartingIndex;
						RemoveItem(oldStartingIndex);
						break;
					case NotifyCollectionChangedAction.Replace:
						//if (e.NewItems.Count > 1) throw new ObservableComputationsException("Replacing of multiple items is not supported");
						TSourceItem newItem = _sourceAsList[e.NewStartingIndex];
						SetItem(e.NewStartingIndex, newItem);
						break;
					case NotifyCollectionChangedAction.Move:
						int oldStartingIndex1 = e.OldStartingIndex;
						int newStartingIndex1 = e.NewStartingIndex;
						if (oldStartingIndex1 == newStartingIndex1) return;

						MoveItem(oldStartingIndex1, newStartingIndex1);
						break;
					case NotifyCollectionChangedAction.Reset:
						ClearItems();

						_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
						_sourceNotifyCollectionChangedEventHandler = null;
						_sourceWeakNotifyCollectionChangedEventHandler = null;
						initializeFromSource();
						break;
				}
			}
			
		}

		~RootSourceWrapper()
		{
			_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;

			if (_sourceAsINotifyPropertyChanged != null)
				_sourceAsINotifyPropertyChanged.PropertyChanged -=
					_sourceWeakPropertyChangedEventHandler.Handle;
		}
	}
}
