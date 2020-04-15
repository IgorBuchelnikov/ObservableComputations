using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using ObservableComputations;

namespace ObservableComputations
{
	public class Concatenating<TSourceItem> : CollectionComputing<TSourceItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourcesScalar => _sourcesScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Sources => _sources;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Sources});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourcesScalar});

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourcesScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourcesScalarWeakPropertyChangedEventHandler;

		private IList _sourcesAsList;

		private NotifyCollectionChangedEventHandler _sourcesNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourcesWeakNotifyCollectionChangedEventHandler;

		RangePositions<ItemInfo> _sourceRangePositions;
		List<ItemInfo> _itemInfos;
		private readonly IReadScalar<INotifyCollectionChanged> _sourcesScalar;
		private INotifyCollectionChanged _sources;

		private PropertyChangedEventHandler _sourcesPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourcesWeakPropertyChangedEventHandler;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourcesAsINotifyPropertyChanged;

		private IHasChangeMarker _sourcesAsIHasChangeMarker;
		private bool _lastProcessedSourcesChangeMarker;

		private sealed class ItemInfo : RangePosition
		{
			public INotifyCollectionChanged Source;
			public IReadScalar<object> SourceScalar;

			public PropertyChangedEventHandler SourceScalarPropertyChangedEventHandler;
			public WeakPropertyChangedEventHandler SourceScalarWeakPropertyChangedEventHandler;

			public NotifyCollectionChangedEventHandler SourceNotifyCollectionChangedEventHandler;
			public WeakNotifyCollectionChangedEventHandler SourceWeakNotifyCollectionChangedEventHandler;

			public PropertyChangedEventHandler SourcePropertyChangedEventHandler;
			public WeakPropertyChangedEventHandler SourceWeakPropertyChangedEventHandler;
			public bool IndexerPropertyChangedEventRaised;
			public INotifyPropertyChanged SourceAsINotifyPropertyChanged;

			public IHasChangeMarker SourceAsIHasChangeMarker;
			public bool LastProcessedSourceChangeMarker;
		}

		[ObservableComputationsCall]
		public Concatenating(
			IReadScalar<INotifyCollectionChanged> sourcesScalar) : base(calculateCapacity(sourcesScalar.Value))
		{		
			int capacity = Utils.getCapacity(sourcesScalar);
			_itemInfos = new List<ItemInfo>(capacity);
			_sourceRangePositions = new RangePositions<ItemInfo>(_itemInfos);

			_sourcesScalar = sourcesScalar;
			_sourcesScalarPropertyChangedEventHandler = handleSourcesScalarValueChanged;
			_sourcesScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourcesScalarPropertyChangedEventHandler);
			_sourcesScalar.PropertyChanged += _sourcesScalarWeakPropertyChangedEventHandler.Handle;
			initializeFromSources();
		}

		[ObservableComputationsCall]
		public Concatenating(
			INotifyCollectionChanged sources) : base(calculateCapacity(sources))
		{
			int capacity = Utils.getCapacity(sources);
			_itemInfos = new List<ItemInfo>(capacity);
			_sourceRangePositions = new RangePositions<ItemInfo>(_itemInfos);

			_sources = sources;
			initializeFromSources();
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

		private void initializeFromSources()
		{
			if (_sourcesNotifyCollectionChangedEventHandler != null)
			{
				int itemInfosCount = _itemInfos.Count;
				for (int index = 0; index < itemInfosCount; index++)
				{
					ItemInfo itemInfo = _itemInfos[index];
					if (itemInfo.Source != null)
						itemInfo.Source.CollectionChanged -=
							itemInfo.SourceWeakNotifyCollectionChangedEventHandler.Handle;
				}

				int capacity = _sourcesScalar != null ? Utils.getCapacity(_sourcesScalar) : Utils.getCapacity(_sources);
				_itemInfos = new List<ItemInfo>(capacity);
				_sourceRangePositions = new RangePositions<ItemInfo>(_itemInfos);
				baseClearItems();

				_sources.CollectionChanged -= _sourcesWeakNotifyCollectionChangedEventHandler.Handle;
				_sourcesNotifyCollectionChangedEventHandler = null;
				_sourcesWeakNotifyCollectionChangedEventHandler = null;
			}

			if (_sourcesAsINotifyPropertyChanged != null)
			{
				_sourcesAsINotifyPropertyChanged.PropertyChanged -=
					_sourcesWeakPropertyChangedEventHandler.Handle;

				_sourcesAsINotifyPropertyChanged = null;
				_sourcesPropertyChangedEventHandler = null;
				_sourcesWeakPropertyChangedEventHandler = null;
			}

			if (_sourcesScalar != null) _sources = _sourcesScalar.Value;
			_sourcesAsList = (IList)_sources;

			if (_sources != null)
			{
				_sourcesAsIHasChangeMarker = _sourcesAsList as IHasChangeMarker;

				if (_sourcesAsIHasChangeMarker != null)
				{
					_lastProcessedSourcesChangeMarker = _sourcesAsIHasChangeMarker.ChangeMarker;
				}
				else
				{
					_sourcesAsINotifyPropertyChanged = (INotifyPropertyChanged) _sourcesAsList;

					_sourcesPropertyChangedEventHandler = (sender, args) =>
					{
						if (args.PropertyName == "Item[]") _indexerPropertyChangedEventRaised = true; // ObservableCollection raises this before CollectionChanged event raising
					};

					_sourcesWeakPropertyChangedEventHandler =
						new WeakPropertyChangedEventHandler(_sourcesPropertyChangedEventHandler);

					_sourcesAsINotifyPropertyChanged.PropertyChanged +=
						_sourcesWeakPropertyChangedEventHandler.Handle;
				}


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
						// ReSharper disable once PossibleNullReferenceException
						TSourceItem sourceSourceItem = (TSourceItem) sourceItem[sourceSourceIndex];
						baseInsertItem(plainIndex, sourceSourceItem);
						plainIndex++;
					}	
				}

				_sourcesNotifyCollectionChangedEventHandler = handleSourcesCollectionChanged;
				_sourcesWeakNotifyCollectionChangedEventHandler = 
					new WeakNotifyCollectionChangedEventHandler(_sourcesNotifyCollectionChangedEventHandler);

				_sources.CollectionChanged += _sourcesWeakNotifyCollectionChangedEventHandler.Handle;
			}
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
						checkConsistent();
						_isConsistent = false;
						object sourceScalarValue = sourceScalar.Value;
						replaceItem((IList) sourceScalarValue, itemInfo);
						unregisterSourceItem(itemInfo);
						registerSourceItem(itemInfo, (INotifyCollectionChanged) sourceScalarValue);
						_isConsistent = true;
						raiseConsistencyRestored();
					};
				itemInfo.SourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(itemInfo.SourceScalarPropertyChangedEventHandler);
				sourceScalar.PropertyChanged += itemInfo.SourceScalarWeakPropertyChangedEventHandler.Handle;
			}
		}

		private void registerSourceItem(ItemInfo itemInfo, INotifyCollectionChanged source)
		{
			itemInfo.Source = source;

			if (source != null)
			{
				itemInfo.SourceAsINotifyPropertyChanged = (INotifyPropertyChanged) source;

				itemInfo.SourcePropertyChangedEventHandler = (sender, args) =>
				{
					if (args.PropertyName == "Item[]")
						itemInfo.IndexerPropertyChangedEventRaised =
							true; // ObservableCollection raises this before CollectionChanged event raising
				};

				itemInfo.SourceWeakPropertyChangedEventHandler =
					new WeakPropertyChangedEventHandler(itemInfo.SourcePropertyChangedEventHandler);

				itemInfo.SourceAsINotifyPropertyChanged.PropertyChanged +=
					itemInfo.SourceWeakPropertyChangedEventHandler.Handle;

				itemInfo.SourceNotifyCollectionChangedEventHandler = (sender, eventArgs) =>
					handleSourceCollectionChanged(sender, eventArgs, itemInfo);
				itemInfo.SourceWeakNotifyCollectionChangedEventHandler =
					new WeakNotifyCollectionChangedEventHandler(itemInfo.SourceNotifyCollectionChangedEventHandler);
				if (source != null) source.CollectionChanged += itemInfo.SourceWeakNotifyCollectionChangedEventHandler.Handle;

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
				itemInfo.SourceScalar.PropertyChanged -= itemInfo.SourceScalarWeakPropertyChangedEventHandler.Handle;
			}

			return itemInfo;
		}

		private static void unregisterSourceItem(ItemInfo itemInfo)
		{
			if (itemInfo.Source != null)
				itemInfo.Source.CollectionChanged -= itemInfo.SourceWeakNotifyCollectionChangedEventHandler.Handle;

			if (itemInfo.SourceAsINotifyPropertyChanged != null)
			{
				itemInfo.SourceAsINotifyPropertyChanged.PropertyChanged -=
					itemInfo.SourceWeakPropertyChangedEventHandler.Handle;

				itemInfo.SourceAsINotifyPropertyChanged = null;
				itemInfo.SourcePropertyChangedEventHandler = null;
				itemInfo.SourceWeakPropertyChangedEventHandler = null;
			}
		}

		private void handleSourcesScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<INotifyCollectionChanged>.Value)) return;
			checkConsistent();
			_isConsistent = false;

			initializeFromSources();

			_isConsistent = true;
			raiseConsistencyRestored();
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, ItemInfo itemInfo)
		{
			checkConsistent();
			if (itemInfo.IndexerPropertyChangedEventRaised || itemInfo.SourceAsIHasChangeMarker != null && itemInfo.LastProcessedSourceChangeMarker != itemInfo.SourceAsIHasChangeMarker.ChangeMarker)
			{
				itemInfo.IndexerPropertyChangedEventRaised = false;
				itemInfo.LastProcessedSourceChangeMarker = !itemInfo.LastProcessedSourceChangeMarker;


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
							baseMoveItem(rangePositionPlainIndex + oldStartingIndex, rangePositionPlainIndex + newStartingIndex);
						}
						break;
				}

				_isConsistent = true;
				raiseConsistencyRestored();
			}

		}

		private void handleSourcesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();
			if (_indexerPropertyChangedEventRaised || _lastProcessedSourcesChangeMarker != _sourcesAsIHasChangeMarker.ChangeMarker)
			{
				_lastProcessedSourcesChangeMarker = !_lastProcessedSourcesChangeMarker;
				_indexerPropertyChangedEventRaised = false;

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

						IList removedItem =  (IList) e.OldItems[0];
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
							itemInfo2.Source.CollectionChanged -= itemInfo2.SourceWeakNotifyCollectionChangedEventHandler.Handle;
							itemInfo2.SourceNotifyCollectionChangedEventHandler =  (sender1, eventArgs) => handleSourceCollectionChanged(sender1, eventArgs, itemInfo2);						
						}

						itemInfo2.Source = newItem;
						if (itemInfo2.Source != null)
						{
							itemInfo2.SourceWeakNotifyCollectionChangedEventHandler = 
								new WeakNotifyCollectionChangedEventHandler(itemInfo2.SourceNotifyCollectionChangedEventHandler);
							itemInfo2.Source.CollectionChanged += itemInfo2.SourceWeakNotifyCollectionChangedEventHandler.Handle;						
						}
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
						initializeFromSources();
						break;
				}

				_isConsistent = true;
				raiseConsistencyRestored();
			}

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

		~Concatenating()
		{
			if (_sourcesScalarWeakPropertyChangedEventHandler != null)
			{
				_sourcesScalar.PropertyChanged -= _sourcesScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_sourcesAsINotifyPropertyChanged != null)
				_sourcesAsINotifyPropertyChanged.PropertyChanged -=
					_sourcesWeakPropertyChangedEventHandler.Handle;

			if (_sourcesWeakNotifyCollectionChangedEventHandler != null)
			{
				_sources.CollectionChanged -= _sourcesWeakNotifyCollectionChangedEventHandler.Handle;
				int count = _itemInfos.Count;
				for (int i = 0; i < count; i++)
				{
					ItemInfo itemInfo = _itemInfos[i];
					INotifyCollectionChanged source = itemInfo.Source;
					if (source != null)
					{
						source.CollectionChanged -= itemInfo.SourceWeakNotifyCollectionChangedEventHandler.Handle;
						
						if (itemInfo.SourceAsINotifyPropertyChanged != null)
							itemInfo.SourceAsINotifyPropertyChanged.PropertyChanged -=
								itemInfo.SourceWeakPropertyChangedEventHandler.Handle;
					}

					if (itemInfo.SourceScalar != null)
					{
						itemInfo.SourceScalar.PropertyChanged -= itemInfo.SourceScalarWeakPropertyChangedEventHandler.Handle;
					}
				}
			}

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
	}
}
