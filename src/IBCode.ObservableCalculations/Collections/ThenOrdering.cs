using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Interface;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace IBCode.ObservableCalculations
{
	// ReSharper disable once RedundantExtendsListEntry
	public class ThenOrdering<TSourceItem, TOrderingValue> : CollectionCalculating<TSourceItem>, INotifyPropertyChanged, IOrdering<TSourceItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IOrdering<TSourceItem>> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, TOrderingValue>> OrderingValueSelector => _orderingValueSelectorOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<ListSortDirection> SortDirectionScalar => _sortDirectionScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<IComparer<TOrderingValue>> ComparerScalar => _comparerScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public IOrdering<TSourceItem> Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public ListSortDirection SortDirection => _sortDirection;

		// ReSharper disable once MemberCanBePrivate.Global
		public IComparer<TOrderingValue> Comparer => _comparer;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, TOrderingValue> OrderingValueSelectorFunc => _orderingValueSelectorFunc;

		// ReSharper disable once CoVariantArrayConversion
		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		// ReSharper disable once CoVariantArrayConversion
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorExpression;
		private readonly ExpressionWatcher.ExpressionInfo _orderingValueSelectorExpressionInfo;

		private readonly bool _orderingValueSelectorContainsParametrizedObservableCalculationsCalls;

		private PropertyChangedEventHandler _comparerScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _comparerScalarWeakPropertyChangedEventHandler;

		private PropertyChangedEventHandler _sortDirectionScalarPropertyChangedEventHandler;
		private WeakPropertyChangedEventHandler _sortDirectionScalarWeakPropertyChangedEventHandler;

		private List<OrderingInfo> _orderingInfos;
		RangePositions<OrderingInfo> _orderingInfoPositions;
		private List<ItemInfo> _itemInfos;

		private bool _movingInProgress;
		
		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;
		int? _deferredBaseRemoveIndex;
		private readonly IReadScalar<IOrdering<TSourceItem>> _sourceScalar;
		private readonly Expression<Func<TSourceItem, TOrderingValue>> _orderingValueSelectorOriginal;
		private readonly IReadScalar<ListSortDirection> _sortDirectionScalar;
		private readonly IReadScalar<IComparer<TOrderingValue>> _comparerScalar;
		private IOrdering<TSourceItem> _source;
		private ListSortDirection _sortDirection;
		private IComparer<TOrderingValue> _comparer;
		private readonly Func<TSourceItem, TOrderingValue> _orderingValueSelectorFunc;

		private sealed class ItemInfo
		{
			public ItemInfo(RangePosition orderingInfoRangePosition)
			{
				OrderingInfoRangePosition = orderingInfoRangePosition;
			}

			public RangePosition OrderingInfoRangePosition;
		}

		private sealed class OrderingInfo : RangePosition
		{
			public Ordering<TSourceItem, TOrderingValue> Ordering;
			public ObservableCollection<TSourceItem> Source;
		}

		[ObservableCalculationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_sortDirection = sortDirection;

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}


		[ObservableCalculationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;
			_sortDirection = sortDirection;
			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IReadScalar<IComparer<TOrderingValue>> comparerScalar = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirection = sortDirection;

			_comparerScalar = comparerScalar;
			initializeComparerScalar();

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public ThenOrdering(
			IOrdering<TSourceItem> source,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			IReadScalar<ListSortDirection> sortDirectionScalar = null,
			IComparer<TOrderingValue> comparer = null) : this(orderingValueSelectorExpression, Utils.getCapacity(source))
		{
			_source = source;

			_sortDirectionScalar = sortDirectionScalar;
			initializeSortDirectionScalar();

			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public ThenOrdering(
			IReadScalar<IOrdering<TSourceItem>> sourceScalar,
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			ListSortDirection sortDirection = ListSortDirection.Ascending,
			IComparer<TOrderingValue> comparer= null) : this(orderingValueSelectorExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			initializeSourceScalar();

			_sortDirection = sortDirection;
			_comparer = comparer ?? Comparer<TOrderingValue>.Default;

			initializeFromSource();
		}

		private ThenOrdering(
			Expression<Func<TSourceItem, TOrderingValue>> orderingValueSelectorExpression,
			int sourceCapacity)
		{
			_orderingInfos = new List<OrderingInfo>(sourceCapacity);
			_orderingInfoPositions = new RangePositions<OrderingInfo>(_orderingInfos);
			_itemInfos = new List<ItemInfo>(sourceCapacity);

			_orderingValueSelectorOriginal = orderingValueSelectorExpression;
			CallToConstantConverter callToConstantConverter =
				new CallToConstantConverter(_orderingValueSelectorOriginal.Parameters);
			_orderingValueSelectorExpression =
				(Expression<Func<TSourceItem, TOrderingValue>>)
				callToConstantConverter.Visit(_orderingValueSelectorOriginal);
			_orderingValueSelectorContainsParametrizedObservableCalculationsCalls =
				callToConstantConverter.ContainsParametrizedObservableCalculationCalls;

			if (!_orderingValueSelectorContainsParametrizedObservableCalculationsCalls)
			{
				_orderingValueSelectorExpressionInfo =
					ExpressionWatcher.GetExpressionInfo(_orderingValueSelectorExpression);
				// ReSharper disable once PossibleNullReferenceException
				_orderingValueSelectorFunc = _orderingValueSelectorExpression.Compile();
			}
		}

		private void initializeSortDirectionScalar()
		{
			if (_sortDirectionScalar != null)
			{
				_sortDirectionScalarPropertyChangedEventHandler = handleSortDirectionScalarValueChanged;
				_sortDirectionScalarWeakPropertyChangedEventHandler =
					new WeakPropertyChangedEventHandler(_sortDirectionScalarPropertyChangedEventHandler);
				_sortDirectionScalar.PropertyChanged += _sortDirectionScalarWeakPropertyChangedEventHandler.Handle;
				_sortDirection = _sortDirectionScalar.Value;
			}
		}

		private void initializeSourceScalar()
		{
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler =
				new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
		}

		private void initializeComparerScalar()
		{
			if (_comparerScalar != null)
			{
				_comparerScalarPropertyChangedEventHandler = handleComparerScalarValueChanged;
				_comparerScalarWeakPropertyChangedEventHandler =
					new WeakPropertyChangedEventHandler(_comparerScalarPropertyChangedEventHandler);
				_comparerScalar.PropertyChanged += _comparerScalarWeakPropertyChangedEventHandler.Handle;
				_comparer = _comparerScalar.Value ?? Comparer<TOrderingValue>.Default;
			}
			else
			{
				_comparer = Comparer<TOrderingValue>.Default;
			}
		}

		private void handleComparerScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent();
		
			_consistent = false;

			_comparer = _comparerScalar.Value ?? Comparer<TOrderingValue>.Default;
			initializeFromSource();

			_consistent = true;
			raiseConsistencyRestored();
		}

		private void handleSortDirectionScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent();
		
			_consistent = false;

			_sortDirection = _sortDirectionScalar.Value;
			initializeFromSource();

			_consistent = true;
			raiseConsistencyRestored();
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			checkConsistent();
			_consistent = false;

			initializeFromSource();

			_consistent = true;
			raiseConsistencyRestored();
		}

		private void handleOrderingCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_movingInProgress) return;

			Ordering<TSourceItem, TOrderingValue> ordering = (Ordering<TSourceItem, TOrderingValue>) sender;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newIndex = e.NewStartingIndex;
					baseInsertItem(ordering._thenOrderingRangePosition.PlainIndex + newIndex, ordering[newIndex]);
					break;
				case NotifyCollectionChangedAction.Remove:
					_deferredBaseRemoveIndex = ordering._thenOrderingRangePosition.PlainIndex + e.OldStartingIndex;
					break;
				case NotifyCollectionChangedAction.Move:
					int plainIndex = ordering._thenOrderingRangePosition.PlainIndex;
					baseMoveItem(plainIndex + e.OldStartingIndex, plainIndex + e.NewStartingIndex);
					break;
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			checkConsistent();
			_consistent = false;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newIndex = e.NewStartingIndex;
					processAddSourceItem(newIndex, _source[newIndex]);
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					processRemoveSourceItem(oldStartingIndex);
					mergeOrderingsIfNeeded(oldStartingIndex);
					if (_deferredBaseRemoveIndex.HasValue) baseRemoveItem(_deferredBaseRemoveIndex.Value);
					_deferredBaseRemoveIndex = null;
					break;
				case NotifyCollectionChangedAction.Move:
					//if (e.OldStartingIndex == e.NewStartingIndex) return;
					int oldStartingIndex2 = e.OldStartingIndex;
					int newStartingIndex2 = e.NewStartingIndex;
					_movingInProgress = true;
					ItemInfo movingItemInfo = _itemInfos[oldStartingIndex2];
					int oldResultIndex = processRemoveSourceItem(oldStartingIndex2);
					_itemInfos.RemoveAt(oldStartingIndex2);
					_itemInfos.Insert(newStartingIndex2, movingItemInfo);
					_itemInfos[newStartingIndex2].OrderingInfoRangePosition = null;
					int newResultIndex = processAddSourceItem(newStartingIndex2, _source[newStartingIndex2]);
					mergeOrderingsIfNeeded(oldStartingIndex2);
					_movingInProgress = false;			
					baseMoveItem(oldResultIndex, newResultIndex);
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex3 = e.NewStartingIndex;
					getSourceItemInfo(newStartingIndex3, out _, out OrderingInfo removingOrderingInfo, out int orderingSourceIndex);
					TSourceItem newSourceItem = _source[newStartingIndex3];
					removingOrderingInfo.Source[orderingSourceIndex] = newSourceItem;
					getSourceItemInfo(newStartingIndex3, out _, out removingOrderingInfo, out orderingSourceIndex, out int resultIndex);
					baseSetItem(resultIndex, newSourceItem);
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
			}

			_consistent = true;
			raiseConsistencyRestored();
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				int orderingInfosCount = _orderingInfos.Count;
				for (int index = 0; index < orderingInfosCount; index++)
				{
					OrderingInfo orderingInfo = _orderingInfos[index];
					orderingInfo.Ordering.CollectionChanged -= handleOrderingCollectionChanged;
				}

				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				_sourceNotifyCollectionChangedEventHandler = null;
				_sourceWeakNotifyCollectionChangedEventHandler = null;

				int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
				_orderingInfos = new List<OrderingInfo>(capacity);
				_orderingInfoPositions = new RangePositions<OrderingInfo>(_orderingInfos);
				_itemInfos = new List<ItemInfo>(capacity);

				baseClearItems();
			}

			if (_sourceScalar != null) _source = _sourceScalar.Value;

			if (_source != null)
			{
				int sourceCount = _source.Count;

				ObservableCollection<TSourceItem> currentOrderingSource = new ObservableCollection<TSourceItem>();
				OrderingInfo orderingInfo = null;

				for (int sourceIndex = 0; sourceIndex < sourceCount; sourceIndex++)
				{
					if (sourceIndex == 0)
					{
						orderingInfo = _orderingInfoPositions.Add(0);
						currentOrderingSource.Add(_source[0]);

					}
					else
					{
						if (_source.Compare(sourceIndex, sourceIndex - 1))
						{
							currentOrderingSource.Add(_source[sourceIndex]);
						}
						else
						{
							int count = currentOrderingSource.Count;
							registerOrderingSource(currentOrderingSource, sourceIndex - count, orderingInfo, true);
							// ReSharper disable once PossibleNullReferenceException
							_orderingInfoPositions.ModifyLength(orderingInfo.Index, count);
							
							orderingInfo = _orderingInfoPositions.Add(0);
							currentOrderingSource = new ObservableCollection<TSourceItem> {_source[sourceIndex]};
						}
					}

					ItemInfo itemInfo = new ItemInfo(orderingInfo);
					_itemInfos.Add(itemInfo);			
				}

				if (sourceCount > 0)
				{
					int count = currentOrderingSource.Count;
					registerOrderingSource(currentOrderingSource, sourceCount - count, orderingInfo, true);
					// ReSharper disable once PossibleNullReferenceException
					_orderingInfoPositions.ModifyLength(orderingInfo.Index, count);
				}

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;
				_sourceWeakNotifyCollectionChangedEventHandler =
					new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

				_source.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
			}
		}

		private OrderingInfo registerOrderingSource(
			ObservableCollection<TSourceItem> currentOrderingSource, 
			int firstItemSourceIndex, 
			OrderingInfo orderingInfo, 
			bool insert)
		{
			Ordering<TSourceItem, TOrderingValue> ordering = new Ordering<TSourceItem, TOrderingValue>(
				currentOrderingSource,
				_orderingValueSelectorExpression,
				_orderingValueSelectorFunc,
				_orderingValueSelectorExpressionInfo,
				_orderingValueSelectorContainsParametrizedObservableCalculationsCalls,
				_sortDirection, _comparer);

			orderingInfo.Source = currentOrderingSource;
			orderingInfo.Ordering = ordering;
			ordering._thenOrderingRangePosition = orderingInfo;

			ordering.CollectionChanged += handleOrderingCollectionChanged;

			if (!_movingInProgress && insert)
			{
				int count = ordering.Count;
				for (int orderingIndex = 0; orderingIndex < count; orderingIndex++)
				{
					TSourceItem sourceItem = ordering[orderingIndex];
					baseInsertItem(firstItemSourceIndex + orderingIndex, sourceItem);
				}
			}
			return orderingInfo;
		}

		private void registerInItemInfos(int newSourceIndex, OrderingInfo orderingInfo)
		{
			if (!_movingInProgress)
				_itemInfos.Insert(newSourceIndex, new ItemInfo(orderingInfo));
			else
				_itemInfos[newSourceIndex].OrderingInfoRangePosition = orderingInfo;
		}

		private void unregisterOrderingSource(int orderingInfoIndex)
		{
			_orderingInfos[orderingInfoIndex].Ordering.CollectionChanged -= handleOrderingCollectionChanged;
			_orderingInfoPositions.Remove(orderingInfoIndex);
		}

		private int processAddSourceItem(int newSourceIndex, TSourceItem newItem)
		{
			OrderingInfo orderingInfo = null;

			if (newSourceIndex >= 1 && _source.Compare(newSourceIndex, newSourceIndex - 1))
			{
				orderingInfo = _orderingInfos[_itemInfos[newSourceIndex - 1].OrderingInfoRangePosition.Index];
			}
			else if (newSourceIndex <= _source.Count - 2 && _source.Compare(newSourceIndex, newSourceIndex + 1))
			{
				orderingInfo = _orderingInfos[_itemInfos[_movingInProgress ? newSourceIndex + 1 : newSourceIndex].OrderingInfoRangePosition.Index];
			}

			int orderingSourceIndex;
			bool split = false;
			ItemInfo splitUpperItemInfo = null;

			if (orderingInfo != null)
			{
				orderingSourceIndex = newSourceIndex - orderingInfo.PlainIndex;
				registerInItemInfos(newSourceIndex, orderingInfo);
				_orderingInfoPositions.ModifyLength(orderingInfo.Index, 1);
				orderingInfo.Source.Insert(orderingSourceIndex, newItem);
			}
			else
			{
				
				if (newSourceIndex > 0 && newSourceIndex < Count - 1)
				{
					splitUpperItemInfo = _itemInfos[newSourceIndex - 1];
					ItemInfo oldItemInfo = _itemInfos[newSourceIndex];
					ItemInfo splitLowerItemInfo = oldItemInfo.OrderingInfoRangePosition != null 
						? oldItemInfo
						: _itemInfos[newSourceIndex + 1];

					split = splitUpperItemInfo.OrderingInfoRangePosition == splitLowerItemInfo.OrderingInfoRangePosition;
				}

				int orderingInfoRangeIndex;
				int itemInfosCount = _itemInfos.Count;
				if (newSourceIndex <= itemInfosCount - 1)
				{
					ItemInfo oldItemInfo = _itemInfos[newSourceIndex];
					orderingInfoRangeIndex =
						split
						? splitUpperItemInfo.OrderingInfoRangePosition.Index + 1
						: 
							oldItemInfo.OrderingInfoRangePosition?.Index ?? 
								(newSourceIndex < itemInfosCount - 1
									? _itemInfos[newSourceIndex + 1].OrderingInfoRangePosition.Index
									: _orderingInfoPositions.List.Count);				
				}
				else
				{
					orderingInfoRangeIndex = _orderingInfoPositions.List.Count;							
				}

				ObservableCollection<TSourceItem> newOrderingSource = new ObservableCollection<TSourceItem> {newItem};
				orderingSourceIndex = 0;
				orderingInfo = _orderingInfoPositions.Insert(orderingInfoRangeIndex, 1);
				registerInItemInfos(newSourceIndex, orderingInfo);
				orderingInfo = registerOrderingSource(newOrderingSource, newSourceIndex, orderingInfo, false);	

				if (split)
				{
					OrderingInfo splittingOrderingInfo = _orderingInfos[_itemInfos[newSourceIndex - 1].OrderingInfoRangePosition.Index];
					RangePosition splittingOrderingInfoRangePosition = splittingOrderingInfo;
					int startOrderingIndex = newSourceIndex - splittingOrderingInfoRangePosition.PlainIndex;
					int splittingOrderingInfoRangePositionLength = splittingOrderingInfoRangePosition.Length;
					int length = splittingOrderingInfoRangePositionLength - startOrderingIndex;
					TSourceItem[] movingItems = new TSourceItem[length];

					bool initialMovingInProgress = _movingInProgress;
					_movingInProgress = true;
					for (int index = splittingOrderingInfoRangePositionLength - 1; index >= startOrderingIndex; index--)
					{
						movingItems[index - startOrderingIndex] = splittingOrderingInfo.Source[index];
						splittingOrderingInfo.Source.RemoveAt(index);
					}

					int splittingOrderingInfoRangePositionIndex = splittingOrderingInfoRangePosition.Index;
					_orderingInfoPositions.ModifyLength(splittingOrderingInfoRangePositionIndex, -length);
					orderingInfo = _orderingInfoPositions.Insert(splittingOrderingInfoRangePositionIndex + 2, length);
					registerOrderingSource(
						new ObservableCollection<TSourceItem>(movingItems), newSourceIndex + 1, orderingInfo, false);

					for (int index = newSourceIndex + 1; index <= newSourceIndex + length; index++)
					{
						_itemInfos[index].OrderingInfoRangePosition = orderingInfo;
					}
					_movingInProgress = initialMovingInProgress;		
				}

				if (!_movingInProgress)
					baseInsertItem(newSourceIndex, newItem);
			}

			return 
				orderingInfo.PlainIndex
				+ orderingInfo.Ordering.GetOrderedIndexBySourceIndex(orderingSourceIndex);
		}

		private void mergeOrderingsIfNeeded(int sourceIndex)
		{
			if (sourceIndex > 0 && sourceIndex < Count - 1)
			{
				if (_source.Compare(sourceIndex - 1, sourceIndex))
				{
					RangePosition upperRangePosition = _itemInfos[sourceIndex - 1].OrderingInfoRangePosition;
					RangePosition lowerRangePosition = _itemInfos[sourceIndex].OrderingInfoRangePosition;

					if (upperRangePosition != lowerRangePosition)
					{
						OrderingInfo upperOrderingInfo = _orderingInfos[upperRangePosition.Index];

						OrderingInfo lowerOrderingInfo = _orderingInfos[lowerRangePosition.Index];

						ObservableCollection<TSourceItem> movingItems = lowerOrderingInfo.Source;
						unregisterOrderingSource(lowerOrderingInfo.Index);

						bool initialMovingInProgress = _movingInProgress;
						_movingInProgress = true;
						int count = movingItems.Count;
						for (int index = 0; index < count; index++)
						{
							TSourceItem movingItem = movingItems[index];
							upperOrderingInfo.Source.Add(movingItem);
							_itemInfos[sourceIndex + index].OrderingInfoRangePosition = upperOrderingInfo;
						}
					
						_orderingInfoPositions.ModifyLength(upperOrderingInfo.Index, movingItems.Count);

						_movingInProgress = initialMovingInProgress;						
					}
				}
			}			
		}

		private int processRemoveSourceItem(int sourceIndex)
		{
			getSourceItemInfo(sourceIndex, out int oldOrderingInfoIndex, out OrderingInfo removingOrderingInfo, out int orderingSourceIndex, out int resultIndex);

			_orderingInfoPositions.ModifyLength(oldOrderingInfoIndex, -1);

			bool orderingSourceUnregistered = false;

			if (removingOrderingInfo.Source.Count - 1 == 0)
			{
				unregisterOrderingSource(oldOrderingInfoIndex);
				orderingSourceUnregistered = true;
			}

			if (!_movingInProgress)
				_itemInfos.RemoveAt(sourceIndex);

			if (!orderingSourceUnregistered)
				removingOrderingInfo.Source.RemoveAt(orderingSourceIndex);
			else
			{
				if (!_movingInProgress) baseRemoveItem(resultIndex);				
			}

			return resultIndex;
		}

		private void getSourceItemInfo(int sourceIndex, out int orderingInfoIndex, out OrderingInfo removingOrderingInfo,
			out int orderingSourceIndex, out int resultIndex)
		{
			getSourceItemInfo(sourceIndex, out orderingInfoIndex, out removingOrderingInfo,
				out orderingSourceIndex);
			resultIndex = 
				removingOrderingInfo.PlainIndex		    
				+ removingOrderingInfo.Ordering.getOrderedIndexBySourceIndex(orderingSourceIndex);
		}

		private void getSourceItemInfo(int sourceIndex, out int orderingInfoIndex, out OrderingInfo removingOrderingInfo,
			out int orderingSourceIndex)
		{
			orderingInfoIndex = _itemInfos[sourceIndex].OrderingInfoRangePosition.Index;
			removingOrderingInfo = _orderingInfos[orderingInfoIndex];
			orderingSourceIndex = sourceIndex - removingOrderingInfo.PlainIndex;
		}

		#region Implementation of IOrdering<TSourceItem>

		public bool Compare(int resultIndex1, int resultIndex2)
		{
			OrderingInfo orderingInfo1 = _orderingInfos[_itemInfos[resultIndex1].OrderingInfoRangePosition.Index];
			OrderingInfo orderingInfo2 = _orderingInfos[_itemInfos[resultIndex2].OrderingInfoRangePosition.Index];

			int ordering1SourceIndex = orderingInfo1.Ordering.getSourceIndexByOrderedIndex(resultIndex1 - orderingInfo1.PlainIndex);
			int ordering2SourceIndex = orderingInfo2.Ordering.getSourceIndexByOrderedIndex(resultIndex2 - orderingInfo2.PlainIndex);
			
			bool compare = 
				_comparer.Compare(
					orderingInfo1.Ordering.getOrderingValueBySourceIndex(
						ordering1SourceIndex), 
					orderingInfo2.Ordering.getOrderingValueBySourceIndex(
						ordering2SourceIndex)) == 0;

			return compare && 
				_source.Compare(
					orderingInfo1.PlainIndex + ordering1SourceIndex, 
					orderingInfo2.PlainIndex + ordering2SourceIndex);
		}

		public IOrdering<TSourceItem> Parent => _source;

		#endregion

		~ThenOrdering()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_source.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_comparerScalarWeakPropertyChangedEventHandler != null)
			{
				_comparerScalar.PropertyChanged -= _comparerScalarWeakPropertyChangedEventHandler.Handle;			
			}

			if (_sortDirectionScalarWeakPropertyChangedEventHandler != null)
			{
				_sortDirectionScalar.PropertyChanged -= _sortDirectionScalarWeakPropertyChangedEventHandler.Handle;			
			}
		}

		public void ValidateConsistency()
		{
			IComparer<TOrderingValue> comparer = _comparerScalar.getValue(_comparer) ?? Comparer<TOrderingValue>.Default;
			ListSortDirection listSortDirection = _sortDirectionScalar.getValue(_sortDirection);
			Func<TSourceItem, TOrderingValue> orderingValueSelector = _orderingValueSelectorOriginal.Compile();
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>().Ordering(_orderingValueSelectorOriginal));

			IOrdering<TSourceItem > orderingSource = (IOrdering<TSourceItem>)source;

			if (_itemInfos.Count != Count) throw new ObservableCalculationsException("Consistency violation: ThenOrdering.1");

			_orderingInfoPositions.ValidateConsistency();

			int orderingRangeIndex = 0;
			int orderingRangeStartPlainIndex = 0;
			List<TSourceItem> copy = this.ToList();	
			List<TSourceItem> buffer = new List<TSourceItem>();	
			for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
			{
				TSourceItem sourceItem = source[sourceIndex];
				if (!copy.Remove(sourceItem)) throw new ObservableCalculationsException("Consistency violation: ThenOrdering.2");

				if (sourceIndex > 0)
				{
					bool validateBuffer = true;
					int bufferLastSourceIndex = sourceIndex - 1;
					if (orderingSource.Compare(sourceIndex, sourceIndex - 1))
					{
						buffer.Add(source[sourceIndex]);
						if (sourceIndex != source.Count - 1)
						{
							validateBuffer = false;
						}
						else
						{
							bufferLastSourceIndex = sourceIndex;
						}
					}
					
					if (validateBuffer)
					{
						OrderingInfo orderingInfo = _orderingInfos[orderingRangeIndex];
						//orderingInfo.Ordering.ValidateConsistency();

						List<TSourceItem> orderedBuffer = new List<TSourceItem>();

						for (int i = bufferLastSourceIndex - buffer.Count + 1 ; i <= bufferLastSourceIndex; i++)
						{
							orderedBuffer.Add(this[i]);
						}

						List<TSourceItem> bufferCopy = buffer.ToList();
						int orderedBufferCount = orderedBuffer.Count;
						for (int index = 0; index < orderedBufferCount; index++)
						{
							TSourceItem item = orderedBuffer[index];
							if (!bufferCopy.Remove(item))
								throw new ObservableCalculationsException("Consistency violation: ThenOrdering.13");
						}

						for (int bufferIndex = 0; bufferIndex < orderedBuffer.Count; bufferIndex++)
						{
							if (bufferIndex > 0)
							{
								int compareResult = comparer.Compare(orderingValueSelector(orderedBuffer[bufferIndex - 1]), orderingValueSelector(orderedBuffer[bufferIndex]));
								if ((compareResult < 0 && listSortDirection == ListSortDirection.Descending)
									|| (compareResult > 0 && listSortDirection == ListSortDirection.Ascending)) 
									throw new ObservableCalculationsException("Consistency violation: ThenOrdering.3");								
							}
						}


						if (orderingInfo.Ordering.Count != buffer.Count)
							throw new ObservableCalculationsException("Consistency violation: ThenOrdering.4");	

						if (orderingInfo.Source.Count != buffer.Count)
							throw new ObservableCalculationsException("Consistency violation: ThenOrdering.5");	
						
						if (orderingInfo.Index != orderingRangeIndex)
							throw new ObservableCalculationsException("Consistency violation: ThenOrdering.6");	

						if (orderingInfo.Length != buffer.Count)
							throw new ObservableCalculationsException("Consistency violation: ThenOrdering.7");
						
						if (orderingInfo.Ordering._source != orderingInfo.Source)
							throw new ObservableCalculationsException("Consistency violation: ThenOrdering.8");

						bufferCopy = buffer.ToList();
						int sourceCount = orderingInfo.Source.Count;
						for (int index = 0; index < sourceCount; index++)
						{
							TSourceItem item = orderingInfo.Source[index];
							if (!bufferCopy.Remove(item))
								throw new ObservableCalculationsException("Consistency violation: ThenOrdering.9");
						}

						for (int i = orderingRangeStartPlainIndex; i < sourceIndex; i++)
						{
							ItemInfo itemInfo = _itemInfos[i];
							if (itemInfo.OrderingInfoRangePosition != orderingInfo)
								throw new ObservableCalculationsException("Consistency violation: ThenOrdering.10");
						}

						orderingRangeStartPlainIndex = sourceIndex + 1;
						buffer.Clear();
						orderingRangeIndex++;
						buffer.Add(source[sourceIndex]);
					}
				}
				else
				{
					buffer.Add(source[sourceIndex]);
				}
			}

			List<IComparer<TOrderingValue>> ancestorComparers = new List<IComparer<TOrderingValue>>();
			List<ListSortDirection> ancestorListSortDirections = new List<ListSortDirection>();
			List<Func<TSourceItem, TOrderingValue>> ancestorOrderingValueSelectorFuncs = new List<Func<TSourceItem, TOrderingValue>>();
			IOrdering<TSourceItem> ancestor;
			ancestor = this;

			do
			{
				// ReSharper disable once ConditionIsAlwaysTrueOrFalse
				if (ancestor is Ordering<TSourceItem, int?> || ancestor is Ordering<TSourceItem, int>)
					// ReSharper disable once HeuristicUnreachableCode
				{
					// ReSharper disable once HeuristicUnreachableCode
					Ordering<TSourceItem, TOrderingValue> orderingAncestor = ((Ordering<TSourceItem, TOrderingValue>)ancestor);
					ancestorComparers.Add(orderingAncestor._comparer);
					ancestorListSortDirections.Add(orderingAncestor._sortDirection);
					ancestorOrderingValueSelectorFuncs.Add(orderingAncestor._orderingValueSelectorFunc);
					ancestor = null;
				}
				else
				{
					ThenOrdering<TSourceItem, TOrderingValue> thenOrderingAncestor = ((ThenOrdering<TSourceItem, TOrderingValue>)ancestor);
					ancestorComparers.Add(thenOrderingAncestor._comparer);
					ancestorListSortDirections.Add(thenOrderingAncestor._sortDirection);
					ancestorOrderingValueSelectorFuncs.Add(thenOrderingAncestor._orderingValueSelectorFunc);		
					ancestor = thenOrderingAncestor._source;
				}				
			} while (ancestor != null);

			IOrderedEnumerable<TSourceItem> result;
			result = 
				ancestorListSortDirections[ancestorComparers.Count - 1] == ListSortDirection.Ascending 
					? source.OrderBy(ancestorOrderingValueSelectorFuncs[ancestorComparers.Count - 1], ancestorComparers[ancestorComparers.Count - 1]) 
					: source.OrderByDescending(ancestorOrderingValueSelectorFuncs[ancestorComparers.Count - 1], ancestorComparers[ancestorComparers.Count - 1]);


			for (int i = ancestorComparers.Count - 2; i >= 0; i--)
			{
				result = ancestorListSortDirections[i] == ListSortDirection.Ascending 
					? result.ThenBy(ancestorOrderingValueSelectorFuncs[i], ancestorComparers[i]) 
					: result.ThenByDescending(ancestorOrderingValueSelectorFuncs[i], ancestorComparers[i]);
			}

			TSourceItem[] resultArray = result.ToArray();

			int resultArrayLength = resultArray.Length;
			for (int orderedIndex = 0; orderedIndex < resultArrayLength; orderedIndex++)
			{
				TSourceItem orderedItem = this[orderedIndex];
				TSourceItem resultItem = resultArray[orderedIndex];

				if (!orderedItem.Equals(resultItem))
					throw new ObservableCalculationsException("Consistency violation: ThenOrdering.11");
			}

			for (int i = 0; i < _itemInfos.Count; i++)
			{
				if (!_orderingInfoPositions.List.Contains(_itemInfos[i].OrderingInfoRangePosition))
					throw new ObservableCalculationsException("Consistency violation: ThenOrdering.12");
			}
		}
	}
}
