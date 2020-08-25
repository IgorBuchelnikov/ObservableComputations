using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ObservableComputations
{
	public class Concatenating<TSourceItem> : CollectionComputing<TSourceItem>, IHasSourceCollections, ISourceIndexerPropertyTracker
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourcesScalar => _sourcesScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Sources => _sources;

		public ReadOnlyCollection<INotifyCollectionChanged> SourceCollections => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Sources});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceCollectionScalars => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourcesScalar});

		private IList _sourcesAsList;
		private bool _sourceInitialized;

		RangePositions<ItemInfo> _sourceRangePositions;
		List<ItemInfo> _itemInfos;
		private readonly IReadScalar<INotifyCollectionChanged> _sourcesScalar;
		private INotifyCollectionChanged _sources;

		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourcesAsINotifyPropertyChanged;

		private IHasChangeMarker _sourcesAsIHasChangeMarker;
		private bool _lastProcessedSourcesChangeMarker;

		private sealed class ItemInfo : RangePosition
		{
			public INotifyCollectionChanged Source;
			public IReadScalar<object> SourceScalar;
			public PropertyChangedEventHandler SourceScalarPropertyChangedEventHandler;
			public NotifyCollectionChangedEventHandler SourceNotifyCollectionChangedEventHandler;
			public PropertyChangedEventHandler SourcePropertyChangedEventHandler;
			public bool IndexerPropertyChangedEventRaised;
			public INotifyPropertyChanged SourceAsINotifyPropertyChanged;
			public IHasChangeMarker SourceAsIHasChangeMarker;
			public bool LastProcessedSourceChangeMarker;
		}

        private Concatenating(int capacity) : base(capacity)
        {
            Utils.initializeItemInfos(capacity,out _itemInfos, out _sourceRangePositions);
        }

		[ObservableComputationsCall]
		public Concatenating(
			IReadScalar<INotifyCollectionChanged> sourcesScalar) : this(calculateCapacity(sourcesScalar.Value))
		{		
			_sourcesScalar = sourcesScalar;
		}

		[ObservableComputationsCall]
		public Concatenating(
			INotifyCollectionChanged sources) : this(calculateCapacity(sources))
		{
			_sources = sources;
		}

		private static int calculateCapacity(INotifyCollectionChanged sources)
		{
			if (sources == null) return 0;

			IList list = (IList)sources;
			int result = 0;

			int listCount = list.Count;
			for (var index= 0; index < listCount; index++)
			{
				object innerList = list[index];
				result = result + (innerList is IHasCapacity capacity ? capacity.Capacity : (innerList is IReadScalar<object> scalar ? (IList)scalar.Value : (IList)innerList)?.Count ?? 0);
			}

			return result;
		}


		[ObservableComputationsCall]
		public Concatenating(INotifyCollectionChanged source1, INotifyCollectionChanged source2) 
			: this(new FreezedObservableCollection<INotifyCollectionChanged>(new []{source1, source2}))
		{
		}

		[ObservableComputationsCall]
		public Concatenating(IReadScalar<INotifyCollectionChanged> source1Scalar, INotifyCollectionChanged source2) 
			: this(new FreezedObservableCollection<object>(new object[]{source1Scalar, source2}))
		{
		}

		[ObservableComputationsCall]
		public Concatenating(IReadScalar<INotifyCollectionChanged> source1Scalar, IReadScalar<INotifyCollectionChanged> source2Scalar) 
			: this(new FreezedObservableCollection<object>(new object[]{source1Scalar, source2Scalar}))
		{
		}

		[ObservableComputationsCall]
		public Concatenating(INotifyCollectionChanged source1, IReadScalar<INotifyCollectionChanged> source2Scalar) 
			: this(new FreezedObservableCollection<object>(new object[]{source1, source2Scalar}))
		{
		}

        protected override void initializeFromSource()
		{
			int originalCount = _items.Count;

			if (_sourceInitialized)
			{
				int itemInfosCount = _itemInfos.Count;
				for (int index = 0; index < itemInfosCount; index++)
				{
					ItemInfo itemInfo = _itemInfos[index];
					if (itemInfo.Source != null)
                    {
                        itemInfo.Source.CollectionChanged -=
                            itemInfo.SourceNotifyCollectionChangedEventHandler;

                        if (itemInfo.SourceAsINotifyPropertyChanged != null)
                            itemInfo.SourceAsINotifyPropertyChanged.PropertyChanged -=
                                itemInfo.SourcePropertyChangedEventHandler;
                    }

                    if (itemInfo.SourceScalar != null)
                        itemInfo.SourceScalar.PropertyChanged -= itemInfo.SourceScalarPropertyChangedEventHandler;                   
				}

                Utils.initializeItemInfos(
                    Utils.getCapacity(_sourcesScalar, _sources),
                    out _itemInfos,
                    out _sourceRangePositions);

				_sources.CollectionChanged -= handleSourcesCollectionChanged;

                if (_sourcesAsINotifyPropertyChanged != null)
                {
                    _sourcesAsINotifyPropertyChanged.PropertyChanged -=
                        ((ISourceIndexerPropertyTracker) this).HandleSourcePropertyChanged;
                    _sourcesAsINotifyPropertyChanged = null;
                }

                _sourceInitialized = false;
            }

            Utils.changeSource(ref _sources, _sourcesScalar, _downstreamConsumedComputings, _consumers, this,
                ref _sourcesAsList, true);

			if (_sources != null && _isActive)
			{
                Utils.initializeFromHasChangeMarker(
                    ref _sourcesAsIHasChangeMarker, 
                    _sourcesAsList, 
                    ref _lastProcessedSourcesChangeMarker, 
                    ref _sourcesAsINotifyPropertyChanged,
                    (ISourceIndexerPropertyTracker)this);


				int plainIndex = 0;
				int count = _sourcesAsList.Count;
				for (int index = 0; index < count; index++)
				{
					object sourceItemObject = _sourcesAsList[index];
					IReadScalar<object> sourceItemScalar = sourceItemObject as IReadScalar<object>;
					IList sourceItem = sourceItemScalar != null ? (IList)sourceItemScalar.Value : (IList) sourceItemObject;
					int sourceItemCount = sourceItem?.Count ?? 0;
					ItemInfo itemInfo = _sourceRangePositions.Add(sourceItemCount);
					registerSourceItem(sourceItemObject, itemInfo);

					for (int sourceSourceIndex = 0; sourceSourceIndex < sourceItemCount; sourceSourceIndex++)
					{
						if (originalCount > plainIndex)
							// ReSharper disable once PossibleNullReferenceException
							_items[plainIndex++] = (TSourceItem) sourceItem[sourceSourceIndex];
						else
							// ReSharper disable once PossibleNullReferenceException
							_items.Insert(plainIndex++, (TSourceItem) sourceItem[sourceSourceIndex]);						
					}	
				}

				for (int index = originalCount - 1; index >= plainIndex; index--)
				{
					_items.RemoveAt(index);
				}

				_sources.CollectionChanged += handleSourcesCollectionChanged;
                _sourceInitialized = true;
            }
			else
			{
				_items.Clear();
			}

			reset();
		}

		private void registerSourceItem(object sourceItemObject, ItemInfo itemInfo)
		{
			IReadScalar<object> sourceScalar = sourceItemObject as IReadScalar<object>;
			itemInfo.SourceScalar = sourceScalar;
			INotifyCollectionChanged source = sourceScalar != null ? (INotifyCollectionChanged)sourceScalar.Value : (INotifyCollectionChanged)sourceItemObject;
			registerSourceItem(itemInfo, source);

			if (sourceScalar != null)
			{
				itemInfo.SourceScalarPropertyChangedEventHandler = 
					(sender, eventArgs) =>
					{
						checkConsistent(sender, eventArgs);
						_isConsistent = false;
						object sourceScalarValue = sourceScalar.Value;
						replaceItem((IList) sourceScalarValue, itemInfo);
						unregisterSourceItem(itemInfo);
						registerSourceItem(itemInfo, (INotifyCollectionChanged) sourceScalarValue);
						_isConsistent = true;
						raiseConsistencyRestored();
					};

				sourceScalar.PropertyChanged += itemInfo.SourceScalarPropertyChangedEventHandler;
			}
		}

		private void registerSourceItem(ItemInfo itemInfo, INotifyCollectionChanged source)
		{
			itemInfo.Source = source;

			if (source != null)
			{
				itemInfo.SourceAsINotifyPropertyChanged = (INotifyPropertyChanged) source;

				itemInfo.SourcePropertyChangedEventHandler = (sender, args) =>				
                    Utils.HandleSourcePropertyChanged(args, ref itemInfo.IndexerPropertyChangedEventRaised);
				
				itemInfo.SourceAsINotifyPropertyChanged.PropertyChanged +=
                    itemInfo.SourcePropertyChangedEventHandler;

				itemInfo.SourceNotifyCollectionChangedEventHandler = (sender, eventArgs) =>
					handleSourceCollectionChanged(sender, eventArgs, itemInfo);
				source.CollectionChanged += itemInfo.SourceNotifyCollectionChangedEventHandler;

				IHasChangeMarker sourceAsIHasChangeMarker = source as IHasChangeMarker;
				itemInfo.SourceAsIHasChangeMarker = sourceAsIHasChangeMarker;
				if (sourceAsIHasChangeMarker != null)
				{
					itemInfo.LastProcessedSourceChangeMarker = sourceAsIHasChangeMarker.ChangeMarker;
				}
			}
		}

		private ItemInfo unregisterSourceItem(int sourcesIndex)
		{
			ItemInfo itemInfo =  _itemInfos[sourcesIndex];
			_sourceRangePositions.Remove(itemInfo.Index);

			unregisterSourceItem(itemInfo);

			if (itemInfo.SourceScalar != null)
			{
				itemInfo.SourceScalar.PropertyChanged -= itemInfo.SourceScalarPropertyChangedEventHandler;
			}

			return itemInfo;
		}

		private static void unregisterSourceItem(ItemInfo itemInfo)
		{
			if (itemInfo.Source != null)
				itemInfo.Source.CollectionChanged -= itemInfo.SourceNotifyCollectionChangedEventHandler;

			if (itemInfo.SourceAsINotifyPropertyChanged != null)
			{
				itemInfo.SourceAsINotifyPropertyChanged.PropertyChanged -=
					itemInfo.SourcePropertyChangedEventHandler;

				itemInfo.SourceAsINotifyPropertyChanged = null;
				itemInfo.SourcePropertyChangedEventHandler = null;
			}
		}	

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, ItemInfo itemInfo)
		{
            if (!Utils.preHandleSourceCollectionChanged(
                sender, 
                e, 
                _isConsistent, 
                this, 
                ref itemInfo.IndexerPropertyChangedEventRaised, 
                ref itemInfo.LastProcessedSourceChangeMarker, 
                itemInfo.SourceAsIHasChangeMarker, 
                ref _handledEventSender, 
                ref _handledEventArgs)) return;

            _isConsistent = false;
            IList sourceItem = (IList) sender;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    IList newItems = e.NewItems;
                    //if (newItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
                    TSourceItem addedItem = (TSourceItem) newItems[0];
                    _sourceRangePositions.ModifyLength(itemInfo.Index, 1);
                    baseInsertItem(itemInfo.PlainIndex + e.NewStartingIndex, addedItem);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    //if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
                    _sourceRangePositions.ModifyLength(itemInfo.Index, -1);
                    baseRemoveItem(itemInfo.PlainIndex + e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    IList newItems1 = e.NewItems;
                    //if (newItems1.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
                    baseSetItem(itemInfo.PlainIndex + e.NewStartingIndex, (TSourceItem) newItems1[0]);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    replaceItem(sourceItem, itemInfo);
                    break;
                case NotifyCollectionChangedAction.Move:
                    int oldStartingIndex = e.OldStartingIndex;
                    int newStartingIndex = e.NewStartingIndex;
                    if (oldStartingIndex != newStartingIndex)
                    {
                        int rangePositionPlainIndex = itemInfo.PlainIndex;
                        baseMoveItem(rangePositionPlainIndex + oldStartingIndex,
                            rangePositionPlainIndex + newStartingIndex);
                    }

                    break;
            }

            _isConsistent = true;
            raiseConsistencyRestored();

            Utils.postHandleSourceCollectionChanged(
                ref _handledEventSender,
                ref _handledEventArgs);

		}

		private void handleSourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
            if (!Utils.preHandleSourceCollectionChanged(
                sender, 
                e, 
                _isConsistent, 
                this, 
                ref _indexerPropertyChangedEventRaised, 
                ref _lastProcessedSourcesChangeMarker, 
                _sourcesAsIHasChangeMarker, 
                ref _handledEventSender, 
                ref _handledEventArgs)) return;

            _isConsistent = false;

            int count;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ItemInfo itemInfo;
                    IList newItems = e.NewItems;
                    //if (newItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");

                    IList addedItem = (IList) newItems[0];
                    count = addedItem?.Count ?? 0;
                    itemInfo = _sourceRangePositions.Insert(e.NewStartingIndex, count);
                    registerSourceItem((INotifyCollectionChanged) addedItem, itemInfo);
                    int rangePositionPlainIndex1 = itemInfo.PlainIndex;

                    for (int index = 0; index < count; index++)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        TSourceItem item = (TSourceItem) addedItem[index];
                        baseInsertItem(rangePositionPlainIndex1 + index, item);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    //if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
                    ItemInfo itemInfo1;

                    IList removedItem = (IList) e.OldItems[0];
                    itemInfo1 = unregisterSourceItem(e.OldStartingIndex);
                    int rangePositionPlainIndex = itemInfo1.PlainIndex;

                    count = removedItem?.Count ?? 0;
                    for (int index = count - 1; index >= 0; index--)
                    {
                        baseRemoveItem(rangePositionPlainIndex + index);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    ItemInfo itemInfo2;
                    IList newItems1 = e.NewItems;
                    //if (newItems1.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");

                    INotifyCollectionChanged newItem = (INotifyCollectionChanged) newItems1[0];

                    itemInfo2 = _itemInfos[e.OldStartingIndex];
                    replaceItem((IList) newItem, itemInfo2);
                    if (itemInfo2.Source != null)
                    {
                        itemInfo2.Source.CollectionChanged -= itemInfo2.SourceNotifyCollectionChangedEventHandler;
                        itemInfo2.SourceNotifyCollectionChangedEventHandler = (sender1, eventArgs) =>
                            handleSourceCollectionChanged(sender1, eventArgs, itemInfo2);
                    }

                    itemInfo2.Source = newItem;
                    if (itemInfo2.Source != null)
                        itemInfo2.Source.CollectionChanged += itemInfo2.SourceNotifyCollectionChangedEventHandler;
                    break;
                case NotifyCollectionChangedAction.Move:
                    int oldIndex = e.OldStartingIndex;
                    int newIndex = e.NewStartingIndex;

                    if (oldIndex != newIndex)
                    {
                        RangePosition oldRangePosition = _sourceRangePositions.List[oldIndex];
                        RangePosition newRangePosition = _sourceRangePositions.List[newIndex];
                        int oldPlainIndex = oldRangePosition.PlainIndex;
                        int newPlainIndex = newRangePosition.PlainIndex;

                        if (oldPlainIndex != newPlainIndex)
                        {
                            IList movingItem = (IList) e.OldItems[0];

                            count = movingItem?.Count ?? 0;

                            if (oldIndex < newIndex)
                            {
                                int newRangePositionLength = newRangePosition.Length;
                                for (int index = 0; index < count; index++)
                                {
                                    baseMoveItem(oldPlainIndex, newPlainIndex + newRangePositionLength - 1);
                                }
                            }
                            else
                            {
                                for (int index = 0; index < count; index++)
                                {
                                    baseMoveItem(oldPlainIndex + index, newPlainIndex + index);
                                }
                            }
                        }

                        _sourceRangePositions.Move(oldRangePosition.Index, newRangePosition.Index);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    initializeFromSource();
                    break;
            }

            _isConsistent = true;
            raiseConsistencyRestored();

            Utils.postHandleSourceCollectionChanged(
                ref _handledEventSender,
                ref _handledEventArgs);
		}

		private void replaceItem(IList newItem, ItemInfo itemInfo)
		{
			int i;
			int newItemCount = newItem?.Count ?? 0;

			int rangePositionLength = itemInfo.Length;
			int rangePositionPlainIndex = itemInfo.PlainIndex;
			for (i = 0; i < rangePositionLength && i < newItemCount; i++)
			{

				// ReSharper disable once PossibleNullReferenceException
				baseSetItem(rangePositionPlainIndex + i, (TSourceItem) newItem[i]);
			}
			
			if (rangePositionLength > newItemCount)
			{
				for (i = rangePositionLength - newItemCount - 1; i >= 0; i--)
				{
					baseRemoveItem(rangePositionPlainIndex + newItemCount + i);
				}
			}
			else if (rangePositionLength < newItemCount)
			{
				for (i = 0; i < newItemCount - rangePositionLength; i++)
				{
				
					baseInsertItem(
						rangePositionPlainIndex + rangePositionLength + i,
						// ReSharper disable once PossibleNullReferenceException
						(TSourceItem) newItem[rangePositionLength + i]);
				}				
			}

			_sourceRangePositions.ModifyLength(itemInfo.Index, newItemCount - rangePositionLength);
		}

        internal override void addToUpstreamComputings(IComputingInternal computing)
        {
            (_sources as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            (_sourcesScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
            processSourceUpstreamComputings(computing, true);
        }

        internal override void removeFromUpstreamComputings(IComputingInternal computing)        
        {
            (_sources as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            (_sourcesScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
            processSourceUpstreamComputings(computing, false);
        }

        private void processSourceUpstreamComputings(IComputingInternal computing, bool addOrRemove)
        {
            IList sourceAsList = _sources as IList;
            if (sourceAsList != null)
            {
                int count = sourceAsList.Count;
                for (int sourceIndex = 0; sourceIndex < count; sourceIndex++)
                {
                    IReadScalar<IComputingInternal> sourceScalar = sourceAsList[sourceIndex] as IReadScalar<IComputingInternal>;
                    IComputingInternal computingInternal =
                        sourceScalar != null ? sourceScalar.Value : (sourceAsList[sourceIndex] as IComputingInternal);
                    
                    if (addOrRemove)
                        computingInternal?.AddDownstreamConsumedComputing(computing);
                    else
                        computingInternal?.RemoveDownstreamConsumedComputing(computing);
                }
            }
        }

        protected override void initialize()
        {
            Utils.initializeSourceScalar(_sourcesScalar, ref _sources, scalarValueChangedHandler);
        }

        protected override void uninitialize()
        {
            Utils.uninitializeSourceScalar(_sourcesScalar, scalarValueChangedHandler, ref _sources);
        }

		public void ValidateConsistency()
		{
			_sourceRangePositions.ValidateConsistency();
			IList sources = _sourcesScalar.getValue(_sources, new ObservableCollection<ObservableCollection<TSourceItem>>()) as IList;
			// ReSharper disable once PossibleNullReferenceException
			if (_itemInfos.Count != sources.Count) throw new ObservableComputationsException(this, "Consistency violation: Concatenating.1");

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (sources != null)
			{
				int index = 0;
				int sourcesCount = sources.Count;
				for (int sourceIndex = 0; sourceIndex < sourcesCount; sourceIndex++)
				{
					IList source = sources[sourceIndex] is IReadScalar<object> scalar ? (IList)scalar.Value : (IList)sources[sourceIndex];
					int plainIndex = index;

					int sourceCount = source?.Count ?? 0;
					for (int sourceItemIndex = 0; sourceItemIndex < sourceCount; sourceItemIndex++)
					{
						// ReSharper disable once PossibleNullReferenceException
						TSourceItem sourceItem = (TSourceItem) source[sourceItemIndex];

						if (!EqualityComparer<TSourceItem>.Default.Equals(this[index], sourceItem))
							throw new ObservableComputationsException(this, "Consistency violation: Concatenating.2");

						index++;
					}

					ItemInfo itemInfo = _itemInfos[sourceIndex];

					if (!Equals(itemInfo.Source, source)) throw new ObservableComputationsException(this, "Consistency violation: Concatenating.2");
					if (itemInfo.Index != sourceIndex)  throw new ObservableComputationsException(this, "Consistency violation: Concatenating.3");
					if (itemInfo.Length != sourceCount)  throw new ObservableComputationsException(this, "Consistency violation: Concatenating.4");
					if (itemInfo.PlainIndex != plainIndex)  throw new ObservableComputationsException(this, "Consistency violation: Concatenating.5");					

					if (_sourceRangePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableComputationsException(this, "Consistency violation: Concatenating.6");
				}
			}
			
			for (int i = 0; i < _itemInfos.Count; i++)
			{
				if (!_sourceRangePositions.List.Contains(_itemInfos[i]))
					throw new ObservableComputationsException(this, "Consistency violation: Concatenating.7");
			}

			if (_sourceRangePositions.List.Count != sources.Count)
					throw new ObservableComputationsException(this, "Consistency violation: Concatenating.15");
		}

        #region Implementation of ISourceIndexerPropertyTracker

        public void HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Utils.HandleSourcePropertyChanged(propertyChangedEventArgs, ref _indexerPropertyChangedEventRaised);
        }

        #endregion
    }
}
