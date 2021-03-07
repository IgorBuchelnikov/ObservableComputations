// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	internal interface IRootSourceWrapper
	{
		void Uninitialize();
	}

	internal sealed class RootSourceWrapper<TSourceItem> : ObservableCollectionWithChangeMarker<TSourceItem>, IRootSourceWrapper
	{
		private readonly INotifyCollectionChanged _source;
		private readonly IList<TSourceItem> _sourceAsList;

		private bool _indexerPropertyChangedEventRaised;
		private readonly INotifyPropertyChanged _sourceAsINotifyPropertyChanged;
		private readonly IList<TSourceItem> _items;
		internal object HandledEventSender;
		internal NotifyCollectionChangedEventArgs HandledEventArgs;

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
			_sourceAsINotifyPropertyChanged.PropertyChanged += handleSourcePropertyChanged;

			int count = _sourceAsList.Count;
			if (count > 0)
			{
				int sourceIndex;
				for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
					_items.Insert(sourceIndex, _sourceAsList[sourceIndex]);
			}

			_source.CollectionChanged += handleSourceCollectionChanged;		
		}

		private void reset()
		{
			int originalCount = _items.Count;
			int count = _sourceAsList.Count;
			int sourceIndex;
			for (sourceIndex = 0; sourceIndex < count; sourceIndex++)
				if (originalCount > sourceIndex)
					_items[sourceIndex] = _sourceAsList[sourceIndex];
				else
					_items.Insert(sourceIndex, _sourceAsList[sourceIndex]);

			for (int index = originalCount - 1; index >= sourceIndex; index--)
				_items.RemoveAt(index);

			CheckReentrancy();
			OnPropertyChanged(Utils.CountPropertyChangedEventArgs);
			OnPropertyChanged(Utils.IndexerPropertyChangedEventArgs);
			OnCollectionChanged(Utils.ResetNotifyCollectionChangedEventArgs);
		}

		void IRootSourceWrapper.Uninitialize()
		{
			_sourceAsINotifyPropertyChanged.PropertyChanged -= handleSourcePropertyChanged;
			_source.CollectionChanged -= handleSourceCollectionChanged;
		}

		private void handleSourcePropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Item[]") _indexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			HandledEventSender = sender;
			HandledEventArgs = e;

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
