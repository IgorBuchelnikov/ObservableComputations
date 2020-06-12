using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
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
		private IList<TSourceItem> _items;

		internal RootSourceWrapper(
			INotifyCollectionChanged source)
		{
			_items = Items;
			_source = source;
			_sourceAsList = (IList<TSourceItem>) _source;

			initializeFromSource();
		}

		private void initializeFromSource()
		{
			int originalCount = _items.Count;
			if (_sourceWeakPropertyChangedEventHandler == null)
			{
				_sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _sourceAsList;

				_sourcePropertyChangedEventHandler = (sender, args) =>
				{
					if (args.PropertyName == "Item[]") _indexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
				};

				_sourceWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourcePropertyChangedEventHandler);

				_sourceAsINotifyPropertyChanged.PropertyChanged += _sourceWeakPropertyChangedEventHandler.Handle;
			}

			int count = _sourceAsList.Count;
			if (count > 0)
			{
				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					if (originalCount > sourceIndex)
						_items[sourceIndex] = _sourceAsList[sourceIndex];
					else
						_items.Insert(sourceIndex, _sourceAsList[sourceIndex]);
				}

				for (int index = originalCount - 1; index >= sourceIndex; index--)
				{
					_items.RemoveAt(index);
				}
			}
			else
			{
				_items.Clear();
			}

			if (_sourceWeakNotifyCollectionChangedEventHandler == null)
			{
				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
				_sourceWeakNotifyCollectionChangedEventHandler =
					new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

				_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;

			}

			ChangeMarkerField = !ChangeMarkerField;
			this.CheckReentrancy();
			this.OnPropertyChanged(Utils.CountPropertyChangedEventArgs);
			this.OnPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			this.OnCollectionChanged(Utils.ResetNotifyCollectionChangedEventArgs);
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
