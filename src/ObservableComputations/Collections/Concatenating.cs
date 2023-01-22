// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace ObservableComputations
{
	public class Concatenating<TSourceItem> : CollectionComputing<TSourceItem>, IHasSources, ISourceIndexerPropertyTracker, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		private IList _sourcesAsList;

		RangePositions<ItemInfo> _sourceRangePositions;
		List<ItemInfo> _itemInfos;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private INotifyCollectionChanged _source;

		private bool _countPropertyChangedEventRaised;
		private bool _indexerPropertyChangedEventRaised;
		private INotifyPropertyChanged _sourceAsINotifyPropertyChanged;

		private IHasTickTackVersion _sourceAsIHasTickTackVersion;
		private bool _lastProcessedSourcesTickTackVersion;

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
		private bool _initialized;

		private sealed class ItemInfo : RangePosition, ISourceCollectionChangeProcessor
		{
			public Concatenating<TSourceItem> Concatenating;
			public INotifyCollectionChanged Source;
			public IReadScalar<object> SourceScalar;
			public IList<TSourceItem> SourceCopy;
			public PropertyChangedEventHandler SourceScalarPropertyChangedEventHandler;
			public NotifyCollectionChangedEventHandler SourceNotifyCollectionChangedEventHandler;
			public PropertyChangedEventHandler SourcePropertyChangedEventHandler;
			public bool IndexerPropertyChangedEventRaised;
			public bool CountPropertyChangedEventRaised;
			public INotifyPropertyChanged SourceAsINotifyPropertyChanged;
			public IHasTickTackVersion SourceAsIHasTickTackVersion;
			public bool LastProcessedSourceTickTackVersion;

			#region Implementation of ISourceCollectionChangeProcessor

			public void processSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				Concatenating.processSourceItemCollectionChanged(e, this);
			}

			public void ProcessSource()
			{
				Concatenating.resetSourceItem(this);
			}

			#endregion
		}

		private Concatenating(int initialCapacity) : base(initialCapacity)
		{
			Utils.initializeItemInfos(initialCapacity, out _itemInfos, out _sourceRangePositions);
			_thisAsSourceCollectionChangeProcessor = this;
		}

		[ObservableComputationsCall]
		public Concatenating(
			IReadScalar<INotifyCollectionChanged> sourceScalar) : this(calculateCapacity(sourceScalar.Value))
		{		
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public Concatenating(
			INotifyCollectionChanged source) : this(calculateCapacity(source))
		{
			_source = source;
		}

		private static int calculateCapacity(INotifyCollectionChanged source)
		{
			if (source == null) return 0;

			IList list = (IList)source;
			int result = 0;

			int listCount = list.Count;
			for (int index= 0; index < listCount; index++)
			{
				object innerList = list[index];
				result = result + (list[index] is IHasCapacity initialCapacity ? initialCapacity.Capacity : (innerList is IReadScalar<object> scalar ? (IList)scalar.Value : (IList)innerList)?.Count ?? 0);
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

		protected override void processSource()
		{
			processSource(true);
		}

		private void processSource(bool replaceSource)
		{
			int originalCount = _items.Count;

			if (_sourceReadAndSubscribed)
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
					{
						itemInfo.SourceScalar.PropertyChanged -= itemInfo.SourceScalarPropertyChangedEventHandler;
					}	
					
					if (itemInfo.SourceScalar is IComputingInternal sourceScalarComputing) 
						sourceScalarComputing.RemoveDownstreamConsumedComputing(this);
					else if (itemInfo.Source is IComputingInternal sourceComputing) 
						sourceComputing.RemoveDownstreamConsumedComputing(this);
				}

				Utils.initializeItemInfos(
					Utils.getCapacity(_sourceScalar, _source),
					out _itemInfos,
					out _sourceRangePositions);

				if (replaceSource)
				{
					Utils.unsubscribeSource(
						_source,
						ref _sourceAsINotifyPropertyChanged,
						this,
						handleSourceCollectionChanged);
				}

				_sourceReadAndSubscribed = false;
			}

			if (replaceSource)
				Utils.replaceSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this,
					out _sourcesAsList, true);

			if (_source != null && _isActive)
			{
				if (replaceSource)
					Utils.subscribeSource(
						out _sourceAsIHasTickTackVersion, 
						_sourcesAsList, 
						ref _lastProcessedSourcesTickTackVersion, 
						ref _sourceAsINotifyPropertyChanged,
						(ISourceIndexerPropertyTracker)this,
						_source,
						handleSourceCollectionChanged);

				int plainIndex = 0;
				int count = _sourcesAsList.Count;
				object[] sourcesCopy = new object[count];
				_sourcesAsList.CopyTo(sourcesCopy, 0);

				for (int index = 0; index < count; index++)
				{
					object sourceItemObject = sourcesCopy[index];
					addDownstreamConsumedComputing(sourceItemObject);
					IList sourceItem = sourceItemObject is IReadScalar<object> sourceItemScalar 
						? (IList)sourceItemScalar.Value : (IList) sourceItemObject;
					int sourceItemCount = sourceItem?.Count ?? 0;
					ItemInfo itemInfo = _sourceRangePositions.Add(sourceItemCount);
					registerSourceItem(sourceItemObject, itemInfo);

					IList<TSourceItem> sourceCopy = itemInfo.SourceCopy;

					for (int sourceSourceIndex = 0; sourceSourceIndex < sourceItemCount; sourceSourceIndex++)
					{
						if (originalCount > plainIndex)
							// ReSharper disable once PossibleNullReferenceException
							_items[plainIndex++] = sourceCopy[sourceSourceIndex];
						else
							// ReSharper disable once PossibleNullReferenceException
							_items.Insert(plainIndex++, sourceCopy[sourceSourceIndex]);
					}	
				}

				for (int index = originalCount - 1; index >= plainIndex; index--)
					_items.RemoveAt(index);

				_sourceReadAndSubscribed = true;
			}
			else
			{
				_items.Clear();
			}

			reset();
		}

		private void registerSourceItem(object sourceItemObject, ItemInfo itemInfo)
		{
			itemInfo.Concatenating = this;

			IReadScalar<object> sourceScalar = sourceItemObject as IReadScalar<object>;

			itemInfo.SourceScalar = sourceScalar;
			INotifyCollectionChanged source = sourceScalar != null ? (INotifyCollectionChanged)sourceScalar.Value : (INotifyCollectionChanged)sourceItemObject;

			registerSourceItem(itemInfo, source);

			if (sourceScalar != null)
			{
				itemInfo.SourceScalarPropertyChangedEventHandler = 
					(sender, eventArgs) =>
					{
						Utils.processChange(sender, eventArgs, 
							() =>
							{
								if (ReferenceEquals(itemInfo.SourceScalar, sourceScalar))
								{
									object sourceScalarValue = sourceScalar.Value;
									unregisterSourceItem(itemInfo);
									registerSourceItem(itemInfo, (INotifyCollectionChanged) sourceScalarValue);
									replaceItem(itemInfo.SourceCopy, itemInfo);
								}
							},
							ref _isConsistent,
							ref _handledEventSender,
							ref _handledEventArgs,
							1, _deferredQueuesCount,
							ref _deferredProcessings,
							this);

					};

				sourceScalar.PropertyChanged += itemInfo.SourceScalarPropertyChangedEventHandler;
			}
		}

		private void addDownstreamConsumedComputing(object sourceItemObject)
		{
			if (sourceItemObject is IComputingInternal sourceItemObjectcomputing)
				sourceItemObjectcomputing.AddDownstreamConsumedComputing(this);
		}

		private void registerSourceItem(ItemInfo itemInfo, INotifyCollectionChanged source)
		{
			itemInfo.Source = source;

			if (source != null)
			{
				itemInfo.SourceAsINotifyPropertyChanged = (INotifyPropertyChanged) source;

				itemInfo.SourcePropertyChangedEventHandler = (sender, args) =>				
					Utils.handleSourcePropertyChanged(args, ref itemInfo.CountPropertyChangedEventRaised, ref itemInfo.IndexerPropertyChangedEventRaised);
				
				itemInfo.SourceAsINotifyPropertyChanged.PropertyChanged +=
					itemInfo.SourcePropertyChangedEventHandler;

				itemInfo.SourceNotifyCollectionChangedEventHandler = (sender, eventArgs) =>
					handleSourceCollectionChanged(sender, eventArgs, itemInfo);
				source.CollectionChanged += itemInfo.SourceNotifyCollectionChangedEventHandler;

				IHasTickTackVersion sourceAsIHasTickTackVersion = source as IHasTickTackVersion;
				itemInfo.SourceAsIHasTickTackVersion = sourceAsIHasTickTackVersion;
				if (sourceAsIHasTickTackVersion != null)
					itemInfo.LastProcessedSourceTickTackVersion = sourceAsIHasTickTackVersion.TickTackVersion;
			}

			initializeSourceCopy(itemInfo);
		}

		private static void initializeSourceCopy(ItemInfo itemInfo)
		{
			IList source = (IList) itemInfo.Source;
			if (source != null)
			{
				int sourceCount = source.Count;
				IList<TSourceItem> sourceCopy = new List<TSourceItem>(sourceCount);
				itemInfo.SourceCopy = sourceCopy;
				for (var index = 0; index < sourceCount; index++)
					sourceCopy.Add((TSourceItem) source[index]);
				
			}
			else
				itemInfo.SourceCopy = null;

		}

		private ItemInfo unregisterSourceItem(int sourcesIndex, bool replace = false)
		{
			ItemInfo itemInfo =  _itemInfos[sourcesIndex];
			if (!replace) _sourceRangePositions.Remove(itemInfo.Index);

			unregisterSourceItem(itemInfo);

			if (itemInfo.SourceScalar is IComputingInternal sourceScalar) 
				sourceScalar.RemoveDownstreamConsumedComputing(this);

			if (itemInfo.SourceScalar != null)
			{
				itemInfo.SourceScalar.PropertyChanged -= itemInfo.SourceScalarPropertyChangedEventHandler;

				itemInfo.SourceScalar = null;
				itemInfo.SourceScalarPropertyChangedEventHandler = null;
			}

			return itemInfo;
		}

		private static void unregisterSourceItem(ItemInfo itemInfo)
		{
			if (itemInfo.Source != null)
			{
				itemInfo.Source.CollectionChanged -= 
					itemInfo.SourceNotifyCollectionChangedEventHandler;

				itemInfo.Source = null;
				itemInfo.SourceNotifyCollectionChangedEventHandler = null;
			}

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
				ref _isConsistent, 
				ref itemInfo.CountPropertyChangedEventRaised,
				ref itemInfo.IndexerPropertyChangedEventRaised, 
				ref itemInfo.LastProcessedSourceTickTackVersion, 
				itemInfo.SourceAsIHasTickTackVersion, 
				ref _handledEventSender, 
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, _deferredQueuesCount, itemInfo)) return;

			processSourceItemCollectionChanged(e, itemInfo);

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);

		}

		private void processSourceItemCollectionChanged(NotifyCollectionChangedEventArgs e, ItemInfo itemInfo)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					//if (newItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");
					TSourceItem addedItem = (TSourceItem) e.NewItems[0];
					_sourceRangePositions.ModifyLength(itemInfo.Index, 1);
					int newStartingIndex1 = e.NewStartingIndex;
					itemInfo.SourceCopy.Insert(newStartingIndex1, addedItem);
					baseInsertItem(itemInfo.PlainIndex + newStartingIndex1, addedItem);
					break;
				case NotifyCollectionChangedAction.Remove:
					//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
					_sourceRangePositions.ModifyLength(itemInfo.Index, -1);
					int oldStartingIndex1 = e.OldStartingIndex;
					itemInfo.SourceCopy.RemoveAt(oldStartingIndex1);
					baseRemoveItem(itemInfo.PlainIndex + oldStartingIndex1);
					break;
				case NotifyCollectionChangedAction.Replace:
					//if (newItems1.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");
					int newStartingIndex2 = e.NewStartingIndex;
					TSourceItem newItem1 = (TSourceItem) e.NewItems[0];
					itemInfo.SourceCopy[newStartingIndex2] = newItem1;
					baseSetItem(itemInfo.PlainIndex + newStartingIndex2, newItem1);
					break;
				case NotifyCollectionChangedAction.Reset:
					resetSourceItem(itemInfo);
					break;
				case NotifyCollectionChangedAction.Move:
					int oldStartingIndex = e.OldStartingIndex;
					int newStartingIndex = e.NewStartingIndex;
					if (oldStartingIndex != newStartingIndex)
					{
						int rangePositionPlainIndex = itemInfo.PlainIndex;
						TSourceItem movedItem = itemInfo.SourceCopy[oldStartingIndex];
						itemInfo.SourceCopy.RemoveAt(oldStartingIndex);
						itemInfo.SourceCopy.Insert(newStartingIndex, movedItem);
						baseMoveItem(rangePositionPlainIndex + oldStartingIndex,
							rangePositionPlainIndex + newStartingIndex);
					}

					break;
			}
		}

		private void resetSourceItem(ItemInfo itemInfo)
		{
			initializeSourceCopy(itemInfo);
			replaceItem(itemInfo.SourceCopy, itemInfo);
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref _isConsistent, 
				ref _countPropertyChangedEventRaised,
				ref _indexerPropertyChangedEventRaised, 
				ref _lastProcessedSourcesTickTackVersion, 
				_sourceAsIHasTickTackVersion, 
				ref _handledEventSender, 
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, _deferredQueuesCount, this)) return;

			_thisAsSourceCollectionChangeProcessor.processSourceCollectionChanged(sender, e);

			Utils.postHandleChange(
				ref _handledEventSender,
				ref _handledEventArgs,
				_deferredProcessings,
				ref _isConsistent,
				this);
		}

		void ISourceCollectionChangeProcessor.processSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			int count;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					ItemInfo itemInfo;
					//if (newItems.Count > 1) throw new ObservableComputationsException(this, "Adding of multiple items is not supported");

					object addedItem = e.NewItems[0];
					int newStartingIndex = e.NewStartingIndex;
					itemInfo = _sourceRangePositions.Insert(newStartingIndex, 0);
					addDownstreamConsumedComputing(addedItem);
					registerSourceItem(addedItem, itemInfo);
					int rangePositionPlainIndex1 = itemInfo.PlainIndex;
					IList<TSourceItem> sourceCopy = itemInfo.SourceCopy;
					count = sourceCopy?.Count ?? 0;
					_sourceRangePositions.ModifyLength(newStartingIndex, count);
					for (int index = 0; index < count; index++)				
						baseInsertItem(rangePositionPlainIndex1 + index, sourceCopy[index]);

					if (_involvedMembersTreeNodes != null && addedItem is IComputingInternal addedItemComputingInternal)
					{
						int count1 = _involvedMembersTreeNodes.Count;
						for (var index = 0; index < count1; index++)
							_involvedMembersTreeNodes[index].AddChild(addedItemComputingInternal);
					}
					break;
				case NotifyCollectionChangedAction.Remove:
					//if (e.OldItems.Count > 1) throw new ObservableComputationsException(this, "Removing of multiple items is not supported");
					ItemInfo itemInfo1;

					object oldItem = e.OldItems[0];
					IList removedItem = oldItem is IReadScalar<IList> scalar ? scalar.Value : (IList)oldItem;
					itemInfo1 = unregisterSourceItem(e.OldStartingIndex);
					int rangePositionPlainIndex = itemInfo1.PlainIndex;

					count = removedItem?.Count ?? 0;
					for (int index = count - 1; index >= 0; index--)
						baseRemoveItem(rangePositionPlainIndex + index);

					if (_involvedMembersTreeNodes != null && oldItem is IComputingInternal oldItemComputingInternal)
					{
						int count1 = _involvedMembersTreeNodes.Count;
						for (var index = 0; index < count1; index++)
							_involvedMembersTreeNodes[index].RemoveChild(oldItemComputingInternal);
					}

					break;
				case NotifyCollectionChangedAction.Replace:
					ItemInfo itemInfo2;
					//if (newItems1.Count > 1) throw new ObservableComputationsException(this, "Replacing of multiple items is not supported");

					object newItem = e.NewItems[0];
					int oldStartingIndex = e.OldStartingIndex;
					unregisterSourceItem(oldStartingIndex, true);
					itemInfo2 = _itemInfos[oldStartingIndex];
					addDownstreamConsumedComputing(newItem);
					registerSourceItem(newItem, itemInfo2);
					replaceItem(itemInfo2.SourceCopy, itemInfo2);

					if (_involvedMembersTreeNodes != null)
					{
						IComputingInternal oldItemComputingInternal1 = e.OldItems[0] as IComputingInternal;
						IComputingInternal newItemComputingInternal1 = newItem as IComputingInternal;

						if (oldItemComputingInternal1 != null || newItemComputingInternal1 != null)
						{
							int count1 = _involvedMembersTreeNodes.Count;
							for (var index = 0; index < count1; index++)
							{
								if (oldItemComputingInternal1 != null)
									_involvedMembersTreeNodes[index].RemoveChild(oldItemComputingInternal1);

								if (newItemComputingInternal1 != null)
									_involvedMembersTreeNodes[index].AddChild(newItemComputingInternal1);
							}
						}
					}
					break;
				case NotifyCollectionChangedAction.Move:
					int oldIndex = e.OldStartingIndex;
					int newIndex = e.NewStartingIndex;

					if (oldIndex != newIndex)
					{
						ItemInfo oldItemInfo = _itemInfos[oldIndex];
						RangePosition newRangePosition = _itemInfos[newIndex];
						int oldPlainIndex = oldItemInfo.PlainIndex;
						int newPlainIndex = newRangePosition.PlainIndex;
						int newRangePositionLength = newRangePosition.Length;

						if (oldPlainIndex != newPlainIndex)
						{
							IList<TSourceItem> movingItem = oldItemInfo.SourceCopy;

							count = movingItem?.Count ?? 0;

							if (oldIndex < newIndex)
								for (int index = 0; index < count; index++)
									baseMoveItem(oldPlainIndex, newPlainIndex + newRangePositionLength - 1);
							else
								for (int index = 0; index < count; index++)
									baseMoveItem(oldPlainIndex + index, newPlainIndex + index);
						}

						_sourceRangePositions.Move(oldItemInfo.Index, newRangePosition.Index);
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					void processInvolvedMembersTreeNodes(bool add)
					{
						if (_involvedMembersTreeNodes == null) return;

						int count3 = _involvedMembersTreeNodes.Count;
						for (var index3 = 0; index3 < count3; index3++)
						{
							int count2 = _sourcesAsList.Count;
							for (var index2 = 0; index2 < count2; index2++)
								if (_sourcesAsList[index2] is IComputingInternal sourceItemComputing)
									if (add) _involvedMembersTreeNodes[index3].AddChild(sourceItemComputing);
									else _involvedMembersTreeNodes[index3].RemoveChild(sourceItemComputing);
						}
					}

					processInvolvedMembersTreeNodes(false);
					processSource(false);
					processInvolvedMembersTreeNodes(true);
					break;
			}
		}

		private void replaceItem(IList<TSourceItem> newItem, ItemInfo itemInfo)
		{
			int i;
			int newItemCount = newItem?.Count ?? 0;

			int rangePositionLength = itemInfo.Length;
			int rangePositionPlainIndex = itemInfo.PlainIndex;
			for (i = 0; i < rangePositionLength && i < newItemCount; i++)
				baseSetItem(rangePositionPlainIndex + i, newItem[i]);
			
			if (rangePositionLength > newItemCount)
				for (i = rangePositionLength - newItemCount - 1; i >= 0; i--)
					baseRemoveItem(rangePositionPlainIndex + newItemCount + i);
			else if (rangePositionLength < newItemCount)
				for (i = 0; i < newItemCount - rangePositionLength; i++)
					baseInsertItem(
						rangePositionPlainIndex + rangePositionLength + i,
						// ReSharper disable once PossibleNullReferenceException
						newItem[rangePositionLength + i]);

			_sourceRangePositions.ModifyLength(itemInfo.Index, newItemCount - rangePositionLength);
		}

		public override IEnumerable<IComputing> UpstreamComputingsDirect
		{
			get
			{
				List<IComputing> computings = new List<IComputing>();
				Utils.FillUpstreamComputingsDirect(computings, _source, _sourceScalar);

				IComputing sourceItemComputing;
				int count = _sourcesAsList.Count;
				for (var index = 0; index < count; index++)
				{
					sourceItemComputing = _sourcesAsList[index] as IComputing;
					if (sourceItemComputing != null)
						computings.Add(sourceItemComputing);
				}

				return computings;
			}
		}

		internal override void addToUpstreamComputings(IComputingInternal computing)
		{
			Utils.AddDownstreamConsumedComputing(computing, _sourceScalar, _source);
		}

		internal override void removeFromUpstreamComputings(IComputingInternal computing)		
		{
			Utils.RemoveDownstreamConsumedComputing(computing, _sourceScalar, _source);
		}

		protected override void initialize()
		{
			Utils.initializeSourceScalar(_sourceScalar, ref _source, scalarValueChangedHandler);

			_initialized = true;
		}

		protected override void uninitialize()
		{
			Utils.unsubscribeSourceScalar(_sourceScalar, scalarValueChangedHandler);

			_initialized = false;
		}

		protected override void clearCachedScalarArgumentValues()
		{
			Utils.clearCachcedSourceScalarValue(_sourceScalar, ref _source);
		}

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			_sourceRangePositions.ValidateInternalConsistency();
			IList sources = _sourceScalar.getValue(_source, new ObservableCollection<ObservableCollection<TSourceItem>>()) as IList;
			// ReSharper disable once PossibleNullReferenceException
			if (_itemInfos.Count != sources.Count) throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.1");

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (sources != null)
			{
				int index = 0;
				int sourcesCount = sources.Count;
				for (int sourceIndex = 0; sourceIndex < sourcesCount; sourceIndex++)
				{
					object sourceItem = sources[sourceIndex];
					IList source = sourceItem is IReadScalar<object> scalar ? (IList)scalar.Value : (IList)sourceItem;
					int plainIndex = index;

					int sourceCount = source?.Count ?? 0;
					for (int sourceItemIndex = 0; sourceItemIndex < sourceCount; sourceItemIndex++)
					{
						if (!EqualityComparer<TSourceItem>.Default.Equals(this[index], (TSourceItem) source[sourceItemIndex]))
							throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.2");

						index++;
					}

					ItemInfo itemInfo = _itemInfos[sourceIndex];

					if (!Equals(itemInfo.Source, source)) throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.2");
					if (itemInfo.Index != sourceIndex)  throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.3");
					if (itemInfo.Length != sourceCount)  throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.4");
					if (itemInfo.PlainIndex != plainIndex)  throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.5");					

					if (_sourceRangePositions.List[sourceIndex].Index != sourceIndex) throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.6");
				}
			}
			
			for (int i = 0; i < _itemInfos.Count; i++)
			{
				if (!_sourceRangePositions.List.Contains(_itemInfos[i]))
					throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.7");
			}

			if (_sourceRangePositions.List.Count != sources.Count)
					throw new ValidateInternalConsistencyException("Consistency violation: Concatenating.15");
		}

		#region Implementation of ISourceIndexerPropertyTracker

		void ISourceIndexerPropertyTracker.HandleSourcePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			Utils.handleSourcePropertyChanged(propertyChangedEventArgs, ref _countPropertyChangedEventRaised, ref _indexerPropertyChangedEventRaised);
		}

		#endregion
	}
}
