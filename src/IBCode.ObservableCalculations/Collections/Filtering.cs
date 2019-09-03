using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Base;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class Filtering<TSourceItem> : CollectionCalculating<TSourceItem>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, bool> PredicateFunc => _predicateFunc;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		private sealed class ItemInfo : Position
		{
			public ExpressionWatcher ExpressionWatcher;
			public Func<bool> PredicateFunc;
			public Position FilteredPosition;
			public Position NextFilteredItemPosition;
		}

		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpressionOriginal;


		private Positions<Position> _filteredPositions;	
		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly ExpressionWatcher.ExpressionInfo _predicateExpressionInfo;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private readonly ConstructTransformSourceItemIntoMemberActionVisitor 
			_constructTransformSourceItemIntoMemberActionVisitor = new ConstructTransformSourceItemIntoMemberActionVisitor();

		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;
		private bool _lastProcessedSourceChangeMarker;

		private readonly bool _predicateContainsParametrizedObservableCalculationsCalls;

		private Queue<ExpressionWatcher> _deferredExpressionWatcherChangedProcessings;
		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		private INotifyCollectionChanged _source;
		private readonly Func<TSourceItem, bool> _predicateFunc;

		[ObservableCalculationsCall]
		public Filtering(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int capacity = 0) : this(predicateExpression, Utils.getCapacity(sourceScalar), capacity)
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;
			initializeFromSource();
		}
		
		[ObservableCalculationsCall]
		public Filtering(
			INotifyCollectionChanged source,
			Expression<Func<TSourceItem, bool>> predicateExpression,
			int capacity = 0) : this(predicateExpression, Utils.getCapacity(source), capacity)
		{
			_source = source;
			initializeFromSource();
		}

		private Filtering(Expression<Func<TSourceItem, bool>> predicateExpression, 
			int sourceCapacity, int capacity) : base(capacity)
		{
			_initialCapacity = capacity;
			_filteredPositions = new Positions<Position>(new List<Position>(_initialCapacity));	
			_itemInfos = new List<ItemInfo>(sourceCapacity);
			_sourcePositions = new Positions<ItemInfo>(_itemInfos);

			_predicateExpressionOriginal = predicateExpression;

			CallToConstantConverter callToConstantConverter =
				new CallToConstantConverter(predicateExpression.Parameters);

			_predicateExpression =
				(Expression<Func<TSourceItem, bool>>) callToConstantConverter.Visit(predicateExpression);
			_predicateContainsParametrizedObservableCalculationsCalls =
				callToConstantConverter.ContainsParametrizedObservableCalculationCalls;

			if (!_predicateContainsParametrizedObservableCalculationsCalls)
			{
				_predicateExpressionInfo = ExpressionWatcher.GetExpressionInfo(_predicateExpression);
				// ReSharper disable once PossibleNullReferenceException
				_predicateFunc = _predicateExpression.Compile();
			}

			if (Configuration.AutoConstructFilteringInsertAndRemoveItemAction)
			{
				_constructTransformSourceItemIntoMemberActionVisitor.Visit(predicateExpression);

				Action<TSourceItem> transformSourceItemIntoMember =
					_constructTransformSourceItemIntoMemberActionVisitor.TransformSourceItemIntoMember;
				if (transformSourceItemIntoMember != null)
				{
					InsertItemAction = (index, item) => transformSourceItemIntoMember(item);
				}

				Action<TSourceItem> transformSourceItemNotIntoMember =
					_constructTransformSourceItemIntoMemberActionVisitor.TransformSourceItemIntoNotMember;
				if (transformSourceItemNotIntoMember != null)
				{
					RemoveItemAction = index => transformSourceItemNotIntoMember(this[index]);
				}
			}
		}

		private class ConstructTransformSourceItemIntoMemberActionVisitor : ExpressionVisitor
		{
			public Action<TSourceItem> TransformSourceItemIntoMember;
			public Action<TSourceItem> TransformSourceItemIntoNotMember;
			#region Overrides of ExpressionVisitor

			//TODO !sourceItem.NullableProp.HasValue должно преобразооваться в sourceItem.NullableProp ==  null
			// TODO обрабытывать выражения типа '25 == sourceItem.Id'
			protected override Expression VisitBinary(BinaryExpression node) 
			{
				ExpressionType nodeType = node.NodeType;
				if (nodeType == ExpressionType.Equal || nodeType == ExpressionType.NotEqual)
				{
					if (node.Left is MemberExpression memberExpression)
					{
						Expression rightExpression = node.Right;

						// ReSharper disable once PossibleNullReferenceException
						MemberInfo memberExpressionMember = memberExpression.Member;
						Type declaringType = memberExpressionMember.DeclaringType;
						// ReSharper disable once PossibleNullReferenceException
						if (declaringType.IsConstructedGenericType 					                                                 
						    && declaringType.GetGenericTypeDefinition() == typeof(Nullable<>)
						    && memberExpressionMember.Name == "Value")
						{
							memberExpression = (MemberExpression) memberExpression.Expression;
							rightExpression = Expression.Convert(rightExpression,
								typeof (Nullable<>).MakeGenericType(node.Left.Type));
						}

						ParameterExpression parameterExpression = getParameterExpression(memberExpression);
						if (parameterExpression != null)
						{
							if (memberExpression.Type != rightExpression.Type)
							{
								rightExpression = Expression.Convert(rightExpression, memberExpression.Type);
							}

							if (!memberExpressionMember.IsReadOnly())
							{
								if (nodeType == ExpressionType.Equal)
								{
									TransformSourceItemIntoMember = Expression.Lambda<Action<TSourceItem>>(Expression.Assign(memberExpression, rightExpression), parameterExpression).Compile();	
								}
								else if (nodeType == ExpressionType.NotEqual)
								{
									TransformSourceItemIntoNotMember = Expression.Lambda<Action<TSourceItem>>(Expression.Assign(memberExpression, rightExpression), parameterExpression).Compile();	
								}
							}	
						}						
					}
				}

				return node;
			}

			 
			private ParameterExpression getParameterExpression(MemberExpression memberExpression)
			{
				if (memberExpression.Expression is ParameterExpression parameterExpression)
				{
					return parameterExpression;
				}

				if (memberExpression.Expression is MemberExpression expressionMemberExpression)
				{
					return getParameterExpression(expressionMemberExpression); // memberExpression.Expression может быть не MemberExpression				
				}

				return null;
			}


			#endregion
		}

		private void initializeFromSource()
		{
			if (_sourceNotifyCollectionChangedEventHandler != null)
			{
				int itemInfosCount = _itemInfos.Count;
				for (int index = 0; index < itemInfosCount; index++)
				{
					ItemInfo itemInfo = _itemInfos[index];
					ExpressionWatcher expressionWatcher = itemInfo.ExpressionWatcher;
					expressionWatcher.Dispose();
				}

				int capacity = _sourceScalar != null ? Utils.getCapacity(_sourceScalar) : Utils.getCapacity(_source);
				_filteredPositions = new Positions<Position>(new List<Position>(_initialCapacity));	
				_itemInfos = new List<ItemInfo>(capacity);
				_sourcePositions = new Positions<ItemInfo>(_itemInfos);

				baseClearItems();

				if (_rootSourceWrapper)
				{
					_sourceAsList.CollectionChanged -= _sourceNotifyCollectionChangedEventHandler;
				}
				else
				{
					_sourceAsList.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;
					_sourceWeakNotifyCollectionChangedEventHandler = null;					
				}

				_sourceNotifyCollectionChangedEventHandler = null;
			}

			if (_sourceScalar != null) _source = _sourceScalar.Value;
			_sourceAsList = null;

			if (_source != null)
			{
				if (_source is ObservableCollectionWithChangeMarker<TSourceItem> sourceAsList)
				{
					_sourceAsList = sourceAsList;
					_rootSourceWrapper = false;
				}
				else
				{
					_sourceAsList = new RootSourceWrapper<TSourceItem>(_source);
					_rootSourceWrapper = true;
				}

				_lastProcessedSourceChangeMarker = _sourceAsList.ChangeMarker;

				Position nextItemPosition = _filteredPositions.Add();
				int count = _sourceAsList.Count;
				int insertingIndex = 0;
				for (int sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];
					Position currentFilteredItemPosition = null;

					ItemInfo itemInfo =  registerSourceItem(sourceItem, sourceIndex, null, null);

					if (ApplyPredicate(sourceIndex))
					{
						baseInsertItem(insertingIndex++, sourceItem);
						currentFilteredItemPosition = nextItemPosition;
						nextItemPosition = _filteredPositions.Add();
					}

					itemInfo.FilteredPosition = currentFilteredItemPosition;
					itemInfo.NextFilteredItemPosition = nextItemPosition;
				}

				_sourceNotifyCollectionChangedEventHandler = handleSourceCollectionChanged;

				if (_rootSourceWrapper)
				{
					_sourceAsList.CollectionChanged += _sourceNotifyCollectionChangedEventHandler;
				}
				else
				{
					_sourceWeakNotifyCollectionChangedEventHandler = 
						new WeakNotifyCollectionChangedEventHandler(_sourceNotifyCollectionChangedEventHandler);

					_sourceAsList.CollectionChanged += _sourceWeakNotifyCollectionChangedEventHandler.Handle;
				}
			}
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

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{		
			checkConsistent();
			_consistent = false;

			if (!_rootSourceWrapper && _lastProcessedSourceChangeMarker == _sourceAsList.ChangeMarker) return;
			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:		
					int newSourceIndex = e.NewStartingIndex;
					TSourceItem addedSourceItem = _sourceAsList[newSourceIndex];
					Position newItemPosition = null;
					Position nextItemPosition;

					int? newFilteredIndex  = null;

					getNewExpressionWatcherAndPredicateFunc(addedSourceItem, out ExpressionWatcher newWatcher, out Func<bool> newPredicateFunc);

					if (newSourceIndex < _itemInfos.Count)
					{
						ItemInfo oldItemInfo = _itemInfos[newSourceIndex];

						Position oldItemInfoNextFilteredItemPosition = oldItemInfo.NextFilteredItemPosition;
						Position oldItemInfoFilteredPosition = oldItemInfo.FilteredPosition;
						if (applyPredicate(addedSourceItem, newPredicateFunc))
						{

							if (oldItemInfoFilteredPosition == null)
							{
								newItemPosition = _filteredPositions.Insert(oldItemInfoNextFilteredItemPosition.Index);
								nextItemPosition = oldItemInfoNextFilteredItemPosition;
								newFilteredIndex = newItemPosition.Index;
							}
							else
							{
								int filteredIndex = oldItemInfoFilteredPosition.Index;
								newFilteredIndex = filteredIndex;
								newItemPosition = _filteredPositions.Insert(filteredIndex);
								nextItemPosition = oldItemInfoFilteredPosition;
							}

							
							modifyNextFilteredItemIndex(newSourceIndex, newItemPosition);
						}
						else
						{
							nextItemPosition = oldItemInfoFilteredPosition ?? oldItemInfoNextFilteredItemPosition;
						}	
					}
					else
					{			
						if (applyPredicate(addedSourceItem, newPredicateFunc))
						{
							newItemPosition = _filteredPositions.List[Count];
							newFilteredIndex = Count;		
							nextItemPosition = _filteredPositions.Add();
						}
						else
						{
							//здесь newPositionIndex = null; так и должно быть
							nextItemPosition = _filteredPositions.List[Count];							
						}
					}

					registerSourceItem(addedSourceItem, newSourceIndex, newItemPosition, nextItemPosition, newWatcher, newPredicateFunc);

					if (newFilteredIndex != null)
						baseInsertItem(newFilteredIndex.Value, addedSourceItem);	
					break;
				case NotifyCollectionChangedAction.Remove:
					unregisterSourceItem(e.OldStartingIndex);
					break;
				case NotifyCollectionChangedAction.Move:			
					int newSourceIndex1 = e.NewStartingIndex;
					int oldSourceIndex = e.OldStartingIndex;

					if (newSourceIndex1 == oldSourceIndex) return;

					ItemInfo itemInfoOfOldSourceIndex = _itemInfos[oldSourceIndex];
					ItemInfo itemInfoOfNewSourceIndex = _itemInfos[newSourceIndex1];

					Position newPosition1;
					Position nextPosition1;

					Position itemInfoOfOldSourceIndexFilteredPosition = itemInfoOfOldSourceIndex.FilteredPosition;
					Position itemInfoOfNewSourceIndexNextFilteredItemPosition = itemInfoOfNewSourceIndex.NextFilteredItemPosition;
					Position itemInfoOfNewSourceIndexFilteredPosition = itemInfoOfNewSourceIndex.FilteredPosition;
					if (itemInfoOfOldSourceIndexFilteredPosition != null)
					{
						int oldFilteredIndex = itemInfoOfOldSourceIndexFilteredPosition.Index;
						int newFilteredIndex1;

						newPosition1 = itemInfoOfOldSourceIndexFilteredPosition;
						if (itemInfoOfNewSourceIndexFilteredPosition == null)
						{
							//nextPositionIndex = itemInfoOfNewSourceIndex.NextFilteredItemIndex;
							nextPosition1 =
								itemInfoOfNewSourceIndexNextFilteredItemPosition != itemInfoOfOldSourceIndexFilteredPosition
									? itemInfoOfNewSourceIndexNextFilteredItemPosition
									: itemInfoOfOldSourceIndex.NextFilteredItemPosition;
							newFilteredIndex1 =  oldSourceIndex < newSourceIndex1 ? 
								itemInfoOfNewSourceIndexNextFilteredItemPosition.Index - 1 : 
								itemInfoOfNewSourceIndexNextFilteredItemPosition.Index;			
						}
						else
						{
							nextPosition1 = oldSourceIndex < newSourceIndex1 ? 
								itemInfoOfNewSourceIndexNextFilteredItemPosition : 
								itemInfoOfNewSourceIndexFilteredPosition;
							newFilteredIndex1 = itemInfoOfNewSourceIndexFilteredPosition.Index;
						}

						_filteredPositions.Move(
							oldFilteredIndex, 
							newFilteredIndex1);

						modifyNextFilteredItemIndex(oldSourceIndex, itemInfoOfOldSourceIndex.NextFilteredItemPosition);

						_sourcePositions.Move(oldSourceIndex, newSourceIndex1);

						itemInfoOfOldSourceIndex.NextFilteredItemPosition = nextPosition1;

						modifyNextFilteredItemIndex(newSourceIndex1, newPosition1);
			
						baseMoveItem(oldFilteredIndex, newFilteredIndex1);				
					}
					else
					{
						nextPosition1 = oldSourceIndex < newSourceIndex1
							? itemInfoOfNewSourceIndexNextFilteredItemPosition
							: (itemInfoOfNewSourceIndexFilteredPosition ?? itemInfoOfNewSourceIndexNextFilteredItemPosition);

						itemInfoOfOldSourceIndex.NextFilteredItemPosition = nextPosition1;
						
						_sourcePositions.Move(oldSourceIndex, newSourceIndex1);
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
				case NotifyCollectionChangedAction.Replace:
					int sourceIndex = e.NewStartingIndex;
					ItemInfo replacingItemInfo = _itemInfos[sourceIndex];
					ExpressionWatcher oldExpressionWatcher = replacingItemInfo.ExpressionWatcher;
					oldExpressionWatcher.Dispose();

					ExpressionWatcher watcher;
					Func<bool> predicateFunc;
					TSourceItem replacingSourceItem = _sourceAsList[sourceIndex];
					getNewExpressionWatcherAndPredicateFunc(replacingSourceItem, out watcher, out predicateFunc);
					replacingItemInfo.PredicateFunc = predicateFunc;	
					watcher.ValueChanged = expressionWatcher_OnValueChanged;
					watcher._position = oldExpressionWatcher._position;
					replacingItemInfo.ExpressionWatcher = watcher;

					if (!processChangeSourceItem(sourceIndex) && replacingItemInfo.FilteredPosition != null)
					{
						baseSetItem(replacingItemInfo.FilteredPosition.Index, replacingSourceItem);
					}

					break;
			}

			if (_deferredExpressionWatcherChangedProcessings != null)
				while (_deferredExpressionWatcherChangedProcessings.Count > 0)
				{
					ExpressionWatcher expressionWatcher = _deferredExpressionWatcherChangedProcessings.Dequeue();
					if (!expressionWatcher._disposed)
						processChangeSourceItem(expressionWatcher._position.Index);
				} 

			_consistent = true;
			raiseConsistencyRestored();
		}

		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			checkConsistent();

			if (_rootSourceWrapper || _sourceAsList.ChangeMarker ==_lastProcessedSourceChangeMarker)
			{
				_consistent = false;
				processChangeSourceItem(expressionWatcher._position.Index);
				_consistent = true;
				raiseConsistencyRestored();
			}
			else
			{
				(_deferredExpressionWatcherChangedProcessings = _deferredExpressionWatcherChangedProcessings ??  new Queue<ExpressionWatcher>()).Enqueue(expressionWatcher);
			}
		}

		private void modifyNextFilteredItemIndex(int sourceIndex, Position nextItemPosition)
		{
			for (int previousSourceIndex = sourceIndex - 1; previousSourceIndex >= 0; previousSourceIndex--)
			{
				ItemInfo itemInfo = _itemInfos[previousSourceIndex];
				itemInfo.NextFilteredItemPosition = nextItemPosition;
				if (itemInfo.FilteredPosition != null) break;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public bool ApplyPredicate(int sourceIndex) => 
			_predicateContainsParametrizedObservableCalculationsCalls ? _itemInfos[sourceIndex].PredicateFunc() : _predicateFunc(_sourceAsList[sourceIndex]);

		private bool applyPredicate(TSourceItem sourceItem, Func<bool> itemPredicateFunc) => 
			_predicateContainsParametrizedObservableCalculationsCalls ? itemPredicateFunc() : _predicateFunc(sourceItem);

		private ItemInfo registerSourceItem(TSourceItem sourceItem, int sourceIndex, Position position, Position nextItemPosition, ExpressionWatcher watcher = null, Func<bool> predicateFunc = null)
		{
			ItemInfo itemInfo = _sourcePositions.Insert(sourceIndex);
			itemInfo.FilteredPosition = position;
			itemInfo.NextFilteredItemPosition = nextItemPosition;

			if (watcher == null /*&& predicateFunc == null*/) 
				getNewExpressionWatcherAndPredicateFunc(sourceItem, out watcher, out predicateFunc);

			itemInfo.PredicateFunc = predicateFunc;	
			watcher.ValueChanged = expressionWatcher_OnValueChanged;
			watcher._position = itemInfo;
			itemInfo.ExpressionWatcher = watcher;	
			
			return itemInfo;
		}

		private void unregisterSourceItem(int sourceIndex)
		{
			int? removeIndex = null;
			ItemInfo itemInfo = _itemInfos[sourceIndex];

			ExpressionWatcher watcher = itemInfo.ExpressionWatcher;
			watcher.Dispose();

			Position itemInfoFilteredPosition = itemInfo.FilteredPosition;

			if (itemInfoFilteredPosition != null)
			{
				removeIndex = itemInfoFilteredPosition.Index;			
				_filteredPositions.Remove(itemInfoFilteredPosition.Index);
				modifyNextFilteredItemIndex(sourceIndex, itemInfo.NextFilteredItemPosition);
			}

			_sourcePositions.Remove(sourceIndex);

			if (removeIndex.HasValue) baseRemoveItem(removeIndex.Value);
		}

		private void getNewExpressionWatcherAndPredicateFunc(TSourceItem sourceItem, out ExpressionWatcher watcher, out Func<bool> predicateFunc)
		{
			predicateFunc = null;

			if (!_predicateContainsParametrizedObservableCalculationsCalls)
			{
				watcher = new ExpressionWatcher(_predicateExpressionInfo, sourceItem);
			}
			else
			{
				Expression<Func<bool>> deparametrizedPredicateExpression =
					(Expression<Func<bool>>) _predicateExpression.ApplyParameters(new object[] {sourceItem});
				Expression<Func<bool>> predicateExpression =
					(Expression<Func<bool>>)
						new CallToConstantConverter().Visit(deparametrizedPredicateExpression);
				// ReSharper disable once PossibleNullReferenceException
				predicateFunc = predicateExpression.Compile();
				watcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(predicateExpression));
			}
		}

		private bool processChangeSourceItem(int sourceIndex)
		{
			TSourceItem item = _sourceAsList[sourceIndex];
			ItemInfo itemInfo = _itemInfos[sourceIndex];

			bool newPredicateValue = ApplyPredicate(sourceIndex);
			bool oldPredicateValue = itemInfo.FilteredPosition != null;

			if (newPredicateValue != oldPredicateValue)
			{
				if (newPredicateValue)
				{
					int newIndex = itemInfo.NextFilteredItemPosition.Index;
					itemInfo.FilteredPosition = _filteredPositions.Insert(newIndex);
					newIndex = itemInfo.FilteredPosition.Index;
					modifyNextFilteredItemIndex(sourceIndex, itemInfo.FilteredPosition);
					baseInsertItem(newIndex, item);
					return true;
				}
				else // if (oldPredicaeValue)
				{
					int index = itemInfo.FilteredPosition.Index;
					_filteredPositions.Remove(index);
					itemInfo.FilteredPosition = null;
					modifyNextFilteredItemIndex(sourceIndex, itemInfo.NextFilteredItemPosition);
					baseRemoveItem(index);
					return true;
				}
			}

			return false;
		}

		~Filtering()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_sourceAsList.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}

			if (_sourceScalarWeakPropertyChangedEventHandler != null)
			{
				_sourceScalar.PropertyChanged -= _sourceScalarWeakPropertyChangedEventHandler.Handle;			
			}
		}

		public void ValidateConsistency()
		{
			_filteredPositions.ValidateConsistency();
			_sourcePositions.ValidateConsistency();

			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			if (_itemInfos.Count != source.Count) throw new ObservableCalculationsException("Consistency violation: Filtering.9");
			Func<TSourceItem, bool> predicate = _predicateExpression.Compile();

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_itemInfos.Count != source.Count)
					throw new ObservableCalculationsException("Consistency violation: Filtering.14");

				if (_source != null && _filteredPositions.List.Count - 1 != Count)
					throw new ObservableCalculationsException("Consistency violation: Filtering.15");

				if (_source == null && _filteredPositions.List.Count != 0)
					throw new ObservableCalculationsException("Consistency violation: Filtering.16");

				int index = 0;
				for (int sourceIndex = 0; sourceIndex < source.Count; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];
					if (predicate(sourceItem))
					{
						if (itemInfo.FilteredPosition == null) throw new ObservableCalculationsException("Consistency violation: Filtering.2");

						if (!EqualityComparer<TSourceItem>.Default.Equals(this[index], sourceItem))
						{
							throw new ObservableCalculationsException("Consistency violation: Filtering.1");
						}

						if (itemInfo.FilteredPosition.Index != index) throw new ObservableCalculationsException("Consistency violation: Filtering.5");
						
						index++;
					}
					else
					{
						if (itemInfo.FilteredPosition != null) throw new ObservableCalculationsException("Consistency violation: Filtering.3");
					}

					if (_itemInfos[sourceIndex].Index != sourceIndex) throw new ObservableCalculationsException("Consistency violation: Filtering.7");
					if (itemInfo.ExpressionWatcher._position != _itemInfos[sourceIndex]) throw new ObservableCalculationsException("Consistency violation: Filtering.8");

					if (itemInfo.FilteredPosition != null && !_filteredPositions.List.Contains(itemInfo.FilteredPosition))
						throw new ObservableCalculationsException("Consistency violation: Filtering.10");

					if (!_filteredPositions.List.Contains(itemInfo.NextFilteredItemPosition))
						throw new ObservableCalculationsException("Consistency violation: Filtering.11");

					if (!_itemInfos.Contains(itemInfo.ExpressionWatcher._position))
						throw new ObservableCalculationsException("Consistency violation: Filtering.12");
				}

				if (_source != null)
				{
					int count = source.Where(sourceItem => predicate(sourceItem)).Count();
					if (_filteredPositions.List.Count != count + 1)
					{
						throw new ObservableCalculationsException("Consistency violation: Filtering.6");
					}

					Position nextFilteredItemPosition;
					nextFilteredItemPosition = _filteredPositions.List[count];
					for (int sourceIndex = source.Count - 1; sourceIndex >= 0; sourceIndex--)
					{
						TSourceItem sourceItem = source[sourceIndex];
						ItemInfo itemInfo = _itemInfos[sourceIndex];

						if (itemInfo.NextFilteredItemPosition != nextFilteredItemPosition) 
								throw new ObservableCalculationsException("Consistency violation: Filtering.4");

						if (predicate(sourceItem))
						{
							nextFilteredItemPosition = itemInfo.FilteredPosition;
						}
					}
				}
			}
		}
	}
}
