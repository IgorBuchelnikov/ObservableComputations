using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	internal interface IRootSourceWrapper
	{
		void Unitialize();
	}

	internal sealed class RootSourceWrapper<TSourceItem> : ObservableCollectionWithChangeMarker<TSourceItem>, IRootSourceWrapper
	{
		private readonly INotifyCollectionChanged _source;
		private readonly IList<TSourceItem> _sourceAsList;

		private bool _sourceInitialized;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;
		private IList<TSourceItem> _items;

		internal RootSourceWrapper(
			INotifyCollectionChanged source)
		{
			_items = Items;
			_source = source;
			_sourceAsList = (IList<TSourceItem>) _source;
			_sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) _sourceAsList;
			initialize();
		}

		private void initialize()
		{
			_sourceAsINotifyPropertyChanged.PropertyChanged += handleSourcePropetyChanged;

			int count = _sourceAsList.Count;
			if (count > 0)
			{
				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					_items.Insert(sourceIndex, _sourceAsList[sourceIndex]);
				}
			}

			_source.CollectionChanged += handleSourceCollectionChanged;		
		}

		private void reset()
		{
			int originalCount = _items.Count;
			int count = _sourceAsList.Count;
			int sourceIndex;
			for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
			{
				TSourceItem sourceItem = _sourceAsList[sourceIndex];

				if (originalCount > sourceIndex)
					_items[sourceIndex] = sourceItem;
				else
					_items.Insert(sourceIndex, sourceItem);
			}

			for (int index = originalCount - 1; index >= sourceIndex; index--)
			{
				_items.RemoveAt(index);
			}

			CheckReentrancy();
			OnPropertyChanged(Utils.CountPropertyChangedEventArgs);
			OnPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			OnCollectionChanged(Utils.ResetNotifyCollectionChangedEventArgs);
		}

		void IRootSourceWrapper.Unitialize()
		{
			_sourceAsINotifyPropertyChanged.PropertyChanged -= handleSourcePropetyChanged;
			_source.CollectionChanged -= handleSourceCollectionChanged;
		}

		private void handleSourcePropetyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Item[]") _indexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
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
						reset();
						break;
				}
			}
			
		}
	}
}
