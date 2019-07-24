using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using IBCode.ObservableCalculations.Common;
using IBCode.ObservableCalculations.Common.Base;
using IBCode.ObservableCalculations.Common.Interface;

namespace IBCode.ObservableCalculations
{
	public class AnyCalculating<TSourceItem> : ScalarCalculating<bool>, IHasSources
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public INotifyCollectionChanged Source => _source;

		public ReadOnlyCollection<INotifyCollectionChanged> SourcesCollection => new ReadOnlyCollection<INotifyCollectionChanged>(new []{Source});
		public ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>> SourceScalarsCollection => new ReadOnlyCollection<IReadScalar<INotifyCollectionChanged>>(new []{SourceScalar});

		// ReSharper disable once MemberCanBePrivate.Global
		public Func<TSourceItem, bool> PredicateFunc => _predicateFunc;

		private sealed class ItemInfo : Position
		{
			public ExpressionWatcher ExpressionWatcher;
			public Func<bool> PredicateFunc;
			public bool PredicateResult;
		}

		private readonly Func<TSourceItem, bool> _predicateFunc;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpressionOriginal;

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly ExpressionWatcher.ExpressionInfo _predicateExpressionInfo;

		private NotifyCollectionChangedEventHandler _sourceNotifyCollectionChangedEventHandler;
		private WeakNotifyCollectionChangedEventHandler _sourceWeakNotifyCollectionChangedEventHandler;

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly PropertyChangedEventHandler _sourceScalarPropertyChangedEventHandler;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly WeakPropertyChangedEventHandler _sourceScalarWeakPropertyChangedEventHandler;

		private INotifyCollectionChanged _source;
		private ObservableCollectionWithChangeMarker<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceChangeMarker;
		private Queue<ExpressionWatcher> _deferredExpressionWatcherChangedProcessings;

		private int _predicatePassedCount;

		private readonly bool _predicateContainsParametrizedObservableCalculationCalls;
		[ObservableCalculationsCall]
		public AnyCalculating(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression) : this(predicateExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
			_sourceScalarPropertyChangedEventHandler = handleSourceScalarValueChanged;
			_sourceScalarWeakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(_sourceScalarPropertyChangedEventHandler);
			_sourceScalar.PropertyChanged += _sourceScalarWeakPropertyChangedEventHandler.Handle;

			initializeFromSource();
		}

		[ObservableCalculationsCall]
		public AnyCalculating(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, bool>> predicateExpression) : this(predicateExpression, Utils.getCapacity(source))
		{
			_source = source;
			initializeFromSource();
		}

		private AnyCalculating(Expression<Func<TSourceItem, bool>> predicateExpression, int capacity)
		{
			_itemInfos = new List<ItemInfo>(capacity);
			_sourcePositions = new Positions<ItemInfo>(_itemInfos);

			_predicateExpressionOriginal = predicateExpression;

			CallToConstantConverter callToConstantConverter =
				new CallToConstantConverter(predicateExpression.Parameters);

			_predicateExpression =
				(Expression<Func<TSourceItem, bool>>) callToConstantConverter.Visit(_predicateExpressionOriginal);
			_predicateContainsParametrizedObservableCalculationCalls =
				callToConstantConverter.ContainsParametrizedObservableCalculationCalls;

			if (!_predicateContainsParametrizedObservableCalculationCalls)
			{
				_predicateExpressionInfo = ExpressionWatcher.GetExpressionInfo(_predicateExpression);
				// ReSharper disable once PossibleNullReferenceException
				_predicateFunc = _predicateExpression.Compile();
			}
		}

		private void calculateValue()
		{
			if (_value != _predicatePassedCount > 0)
			{
				setValue(!_value);				
			}
		}

		private void handleSourceScalarValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != nameof(IReadScalar<object>.Value)) return;
			initializeFromSource();
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{		
			if (!_rootSourceWrapper && _lastProcessedSourceChangeMarker == _sourceAsList.ChangeMarker) return;
			_lastProcessedSourceChangeMarker = !_lastProcessedSourceChangeMarker;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					int newSourceIndex = e.NewStartingIndex;
					TSourceItem addedSourceItem = _sourceAsList[newSourceIndex];
					ItemInfo itemInfo = registerSourceItem(addedSourceItem, newSourceIndex);
					if (ApplyPredicate(newSourceIndex))
					{
						_predicatePassedCount++;			
						itemInfo.PredicateResult = true;	
					}

					calculateValue();
					break;
				case NotifyCollectionChangedAction.Remove:
					int oldStartingIndex = e.OldStartingIndex;
					if (_itemInfos[oldStartingIndex].PredicateResult) _predicatePassedCount--;
					unregisterSourceItem(oldStartingIndex);
					calculateValue();
					break;
				case NotifyCollectionChangedAction.Move:
					int newSourceIndex1 = e.NewStartingIndex;
					int oldSourceIndex = e.OldStartingIndex;

					if (newSourceIndex1 == oldSourceIndex) return;

					_sourcePositions.Move(oldSourceIndex, newSourceIndex1);
					break;
				case NotifyCollectionChangedAction.Reset:
					initializeFromSource();
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex = e.NewStartingIndex;
					ItemInfo replacingItemInfo = _itemInfos[newStartingIndex];
					ExpressionWatcher oldExpressionWatcher = replacingItemInfo.ExpressionWatcher;
					oldExpressionWatcher.Dispose();
					if (replacingItemInfo.PredicateResult) _predicatePassedCount--;

					ExpressionWatcher watcher;
					Func<bool> predicateFunc;
					TSourceItem replacingSourceItem = _sourceAsList[newStartingIndex];
					getNewExpressionWatcherAndPredicateFunc(replacingSourceItem, out watcher, out predicateFunc);
					replacingItemInfo.PredicateFunc = predicateFunc;	
					watcher.ValueChanged = expressionWatcher_OnValueChanged;
					watcher._position = oldExpressionWatcher._position;
					replacingItemInfo.ExpressionWatcher = watcher;

					if (ApplyPredicate(newStartingIndex))
					{
						_predicatePassedCount++;			
						replacingItemInfo.PredicateResult = true;	
					}
					else
					{
						replacingItemInfo.PredicateResult = false;	
					}

					calculateValue();					
					break;
			}

			if (_deferredExpressionWatcherChangedProcessings != null)
				while (_deferredExpressionWatcherChangedProcessings.Count > 0)
				{
					ExpressionWatcher expressionWatcher = _deferredExpressionWatcherChangedProcessings.Dequeue();
					if (!expressionWatcher._disposed)
						processExpressionWatcherValueChanged(expressionWatcher);
				} 
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
				_itemInfos = new List<ItemInfo>(capacity);
				_sourcePositions = new Positions<ItemInfo>(_itemInfos);

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

			_predicatePassedCount = 0;
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

				int count = _sourceAsList.Count;
				for (int sourceIndex = 0; sourceIndex < count; sourceIndex++)
				{
					TSourceItem sourceItem = _sourceAsList[sourceIndex];
					ItemInfo itemInfo = registerSourceItem(sourceItem, sourceIndex);

					if (ApplyPredicate(sourceIndex))
					{
						_predicatePassedCount++;
						itemInfo.PredicateResult = true;
					}
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

				calculateValue();
			}
			else
			{
				if (_value)
				{
					setValue(false);
				}
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public bool ApplyPredicate(int sourceIndex)
		{
			return _predicateContainsParametrizedObservableCalculationCalls ? _itemInfos[sourceIndex].PredicateFunc() : _predicateFunc(_sourceAsList[sourceIndex]);
		}

		private ItemInfo registerSourceItem(TSourceItem sourceItem, int sourceIndex)
		{
			ItemInfo itemInfo =_sourcePositions.Insert(sourceIndex);

			getNewExpressionWatcherAndPredicateFunc(sourceItem, out ExpressionWatcher watcher, out Func<bool> predicateFunc);

			itemInfo.PredicateFunc = predicateFunc;	
			watcher.ValueChanged = expressionWatcher_OnValueChanged;
			watcher._position = itemInfo;
			itemInfo.ExpressionWatcher = watcher;		
			return itemInfo;
		}

		private void getNewExpressionWatcherAndPredicateFunc(TSourceItem sourceItem, out ExpressionWatcher watcher, out Func<bool> predicateFunc)
		{
			predicateFunc = null;

			if (!_predicateContainsParametrizedObservableCalculationCalls)
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
				predicateFunc = predicateExpression.Compile();
				watcher = new ExpressionWatcher(ExpressionWatcher.GetExpressionInfo(predicateExpression));
			}
		}

		private void unregisterSourceItem(int sourceIndex)
		{
			ItemInfo itemInfo = _itemInfos[sourceIndex];

			ExpressionWatcher watcher = itemInfo.ExpressionWatcher;
			watcher.Dispose();

			_sourcePositions.Remove(sourceIndex);
		}

		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher)
		{
			if (_rootSourceWrapper || _sourceAsList.ChangeMarker == _lastProcessedSourceChangeMarker)
			{
				processExpressionWatcherValueChanged(expressionWatcher);
			}
			else
			{
				(_deferredExpressionWatcherChangedProcessings = _deferredExpressionWatcherChangedProcessings ??  new Queue<ExpressionWatcher>()).Enqueue(expressionWatcher);
			}
		}

		private void processExpressionWatcherValueChanged(ExpressionWatcher expressionWatcher)
		{
			int sourceIndex = expressionWatcher._position.Index;
			ItemInfo itemInfo = _itemInfos[sourceIndex];
			if (itemInfo.PredicateResult) _predicatePassedCount--;

			if (ApplyPredicate(sourceIndex))
			{
				_predicatePassedCount++;
				itemInfo.PredicateResult = true;
			}
			else
			{
				itemInfo.PredicateResult = false;
			}

			calculateValue();
		}

		~AnyCalculating()
		{
			if (_sourceWeakNotifyCollectionChangedEventHandler != null)
			{
				_sourceAsList.CollectionChanged -= _sourceWeakNotifyCollectionChangedEventHandler.Handle;			
			}
		}

		public void ValidateConsistency()
		{
			_sourcePositions.ValidateConsistency();
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			if (_itemInfos.Count != sourceCount) throw new ObservableCalculationsException("Consistency violation: AnyCalculating.1");
			Func<TSourceItem, bool> predicate = _predicateExpressionOriginal.Compile();
			int predicatePassedCount = 0;

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_sourcePositions.List.Count != sourceCount)
					throw new ObservableCalculationsException("Consistency violation: AnyCalculating.9");

				// ReSharper disable once NotAccessedVariable
				int index = 0;
				for (int sourceIndex = 0; sourceIndex < sourceCount; sourceIndex++)
				{
					TSourceItem sourceItem = source[sourceIndex];
					ItemInfo itemInfo = _itemInfos[sourceIndex];
					if (predicate(sourceItem))
					{
						predicatePassedCount++;
						index++;
						if (!itemInfo.PredicateResult) throw new ObservableCalculationsException("Consistency violation: AnyCalculating.2");
					}
					else
					{
						if (itemInfo.PredicateResult) throw new ObservableCalculationsException("Consistency violation: AnyCalculating.3");
					}

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ObservableCalculationsException("Consistency violation: AnyCalculating.4");
					if (itemInfo.ExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ObservableCalculationsException("Consistency violation: AnyCalculating.5");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.ExpressionWatcher._position))
						throw new ObservableCalculationsException("Consistency violation: AnyCalculating.6");

					if (itemInfo.ExpressionWatcher._position.Index != sourceIndex)
						throw new ObservableCalculationsException("Consistency violation: AnyCalculating.10");

				}

				if (predicatePassedCount != _predicatePassedCount) throw new ObservableCalculationsException("Consistency violation: AnyCalculating.7");
				if (_value != _predicatePassedCount > 0) throw new ObservableCalculationsException("Consistency violation: AnyCalculating.8");


			}
		}
	}
}
