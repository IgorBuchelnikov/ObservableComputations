﻿// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ObservableComputations
{
	public class AnyComputing<TSourceItem> : ScalarComputing<bool>, IHasSources, ISourceItemChangeProcessor, ISourceCollectionChangeProcessor
	{
		// ReSharper disable once MemberCanBePrivate.Global
		public virtual IReadScalar<INotifyCollectionChanged> SourceScalar => _sourceScalar;

		// ReSharper disable once MemberCanBePrivate.Global
		public Expression<Func<TSourceItem, bool>> PredicateExpression => _predicateExpressionOriginal;

		// ReSharper disable once MemberCanBePrivate.Global
		public virtual INotifyCollectionChanged Source => _source;

		public virtual ReadOnlyCollection<object> Sources => new ReadOnlyCollection<object>(new object[]{Source, SourceScalar});

		private readonly ISourceCollectionChangeProcessor _thisAsSourceCollectionChangeProcessor;
		// ReSharper disable once MemberCanBePrivate.Global
		//public Func<TSourceItem, bool> PredicateFunc => _predicateFunc;

		private sealed class ItemInfo : ExpressionItemInfo
		{
			public Func<bool> PredicateFunc;
			public bool PredicateResult;
		}

		private readonly Func<TSourceItem, bool> _predicateFunc;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpression;
		private readonly Expression<Func<TSourceItem, bool>> _predicateExpressionOriginal;

		private Positions<ItemInfo> _sourcePositions;
		private List<ItemInfo> _itemInfos;

		private readonly ExpressionWatcher.ExpressionInfo _predicateExpressionInfo;

		private readonly IReadScalar<INotifyCollectionChanged> _sourceScalar;

		private INotifyCollectionChanged _source;
		private ObservableCollectionWithTickTackVersion<TSourceItem> _sourceAsList;
		bool _rootSourceWrapper;

		private bool _lastProcessedSourceTickTackVersion;

		private int _predicatePassedCount;
		private readonly List<IComputingInternal> _nestedComputings;

		private int _predicateExpressionCallCount;
		private readonly ISourceItemChangeProcessor _thisAsSourceItemChangeProcessor;

		private readonly bool _predicateContainsParametrizedObservableComputationCalls;
		[ObservableComputationsCall]
		public AnyComputing(
			IReadScalar<INotifyCollectionChanged> sourceScalar, 
			Expression<Func<TSourceItem, bool>> predicateExpression) 
			: this(predicateExpression, Utils.getCapacity(sourceScalar))
		{
			_sourceScalar = sourceScalar;
		}

		[ObservableComputationsCall]
		public AnyComputing(
			INotifyCollectionChanged source, 
			Expression<Func<TSourceItem, bool>> predicateExpression) 
			: this(predicateExpression, Utils.getCapacity(source))
		{
			_source = source;
		}

		private AnyComputing(
			Expression<Func<TSourceItem, bool>> predicateExpression, 
			int capacity)
		{
			Utils.construct(
				predicateExpression, 
				capacity, 
				out _itemInfos, 
				out _sourcePositions, 
				out _predicateExpressionOriginal, 
				out _predicateExpression, 
				out _predicateContainsParametrizedObservableComputationCalls, 
				ref _predicateExpressionInfo, 
				ref _predicateExpressionCallCount, 
				ref _predicateFunc, 
				ref _nestedComputings);

			_deferredQueuesCount = 3;
			_thisAsSourceCollectionChangeProcessor = this;
			_thisAsSourceItemChangeProcessor = this;
		}

		private void calculateValue()
		{
			if (_value != _predicatePassedCount > 0)
			{
				setValue(!_value);				
			}
		}

		private void handleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!Utils.preHandleSourceCollectionChanged(
				sender, 
				e, 
				_rootSourceWrapper, 
				ref _lastProcessedSourceTickTackVersion, 
				_sourceAsList, 
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings,
				1, _deferredQueuesCount, 
				this)) return;

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
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:

					int newSourceIndex = e.NewStartingIndex;
					TSourceItem addedSourceItem = (TSourceItem) e.NewItems[0];
					ItemInfo itemInfo = registerSourceItem(addedSourceItem, newSourceIndex);
					if (applyPredicate(addedSourceItem, itemInfo.PredicateFunc))
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

					if (newSourceIndex1 != oldSourceIndex)
					{
						_sourcePositions.Move(oldSourceIndex, newSourceIndex1);
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					processSource(false);
					break;
				case NotifyCollectionChangedAction.Replace:
					int newStartingIndex = e.NewStartingIndex;
					ItemInfo replacingItemInfo = _itemInfos[newStartingIndex];
					ExpressionWatcher oldExpressionWatcher = replacingItemInfo.ExpressionWatcher;
					Utils.disposeExpressionWatcher(oldExpressionWatcher, replacingItemInfo.NestedComputings, this,
						_predicateContainsParametrizedObservableComputationCalls);

					if (replacingItemInfo.PredicateResult) _predicatePassedCount--;

					TSourceItem replacingSourceItem = (TSourceItem) e.NewItems[0];
					Utils.getItemInfoContent(
						new object[] {replacingSourceItem},
						out ExpressionWatcher watcher,
						out Func<bool> predicateFunc,
						out List<IComputingInternal> nestedComputings1,
						_predicateExpression,
						out _predicateExpressionCallCount,
						this,
						_predicateContainsParametrizedObservableComputationCalls,
						_predicateExpressionInfo);

					replacingItemInfo.PredicateFunc = predicateFunc;
					watcher.ValueChanged = expressionWatcher_OnValueChanged;
					watcher._position = oldExpressionWatcher._position;
					replacingItemInfo.ExpressionWatcher = watcher;
					replacingItemInfo.NestedComputings = nestedComputings1;

					if (applyPredicate(replacingSourceItem, predicateFunc))
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
			Utils.initializeNestedComputings(_nestedComputings, this);
		}

		protected override void uninitialize()
		{
			Utils.unsubscribeSourceScalar(_sourceScalar, scalarValueChangedHandler);
			Utils.uninitializeNestedComputings(_nestedComputings, this);
		}

		protected override void clearCachedScalarArgumentValues()
		{
			Utils.clearCachcedSourceScalarValue(_sourceScalar, ref _source);
		}

		protected override void processSource()
		{
			processSource(true);
		}

		private void processSource(bool replaceSource)
		{
			if (_sourceReadAndSubscribed)
			{
				Utils.disposeExpressionItemInfos(_itemInfos, _predicateExpressionCallCount, this);
				Utils.removeDownstreamConsumedComputing(_itemInfos, this);

				Utils.disposeSource(
					_sourceScalar, 
					_source,
					out _itemInfos,
					out _sourcePositions, 
					_sourceAsList, 
					handleSourceCollectionChanged,
					replaceSource);

				_sourceReadAndSubscribed = false;
			}

			_predicatePassedCount = 0;

			if (replaceSource)
				Utils.replaceSource(ref _source, _sourceScalar, _downstreamConsumedComputings, _consumers, this, out _sourceAsList, false);

			if (_isActive)
			{
				if (_source != null)
				{
					if (replaceSource)
						Utils.subscribeSource(
							_source, 
							ref _sourceAsList, 
							ref _rootSourceWrapper, 
							ref _lastProcessedSourceTickTackVersion,
							handleSourceCollectionChanged);

					int count = _sourceAsList.Count;
					for (int sourceIndex = 0; sourceIndex < count; sourceIndex++)
					{
						TSourceItem sourceItem = _sourceAsList[sourceIndex];
						ItemInfo itemInfo = registerSourceItem(sourceItem, sourceIndex);

						if (applyPredicate(sourceItem, itemInfo.PredicateFunc))
						{
							_predicatePassedCount++;
							itemInfo.PredicateResult = true;
						}
					}

					_sourceReadAndSubscribed = true;
				}

				calculateValue();
			}
			else
				setDefaultValue();
		}

		// ReSharper disable once MemberCanBePrivate.Global
		// TODO Uncomment and test
		//public bool ApplyPredicate(int sourceIndex)
		//{
		//	bool getValue() =>
		//		_predicateContainsParametrizedObservableComputationCalls 
		//			? _itemInfos[sourceIndex].PredicateFunc() 
		//			: _predicateFunc(_sourceAsList[sourceIndex]);

		//	if (OcConfiguration.TrackComputingsExecutingUserCode)
		//	{
		//		int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
		//		bool result = getValue();
		//		Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
		//		return result;
		//	}

		//	return  getValue();
		//}

		private bool applyPredicate(TSourceItem sourceItem, Func<bool> itemPredicateFunc)
		{
			bool getValue() =>
				_predicateContainsParametrizedObservableComputationCalls 
					? itemPredicateFunc()
					: _predicateFunc(sourceItem);

			if (OcConfiguration.TrackComputingsExecutingUserCode)
			{
				int currentThreadId = Utils.startComputingExecutingUserCode(out IComputing computing, out _userCodeIsCalledFrom, this);
				bool result = getValue();
				Utils.endComputingExecutingUserCode(computing, currentThreadId, out _userCodeIsCalledFrom);
				return result;
			}

			return getValue();
		}

		private ItemInfo registerSourceItem(TSourceItem sourceItem, int sourceIndex)
		{
			ItemInfo itemInfo =_sourcePositions.Insert(sourceIndex);

			Utils.getItemInfoContent(
				new object[]{sourceItem}, 
				out ExpressionWatcher watcher, 
				out Func<bool> predicateFunc, 
				out List<IComputingInternal> nestedComputings,
				_predicateExpression,
				out _predicateExpressionCallCount,
				this,
				_predicateContainsParametrizedObservableComputationCalls,
				_predicateExpressionInfo);	

			itemInfo.PredicateFunc = predicateFunc;	
			watcher.ValueChanged = expressionWatcher_OnValueChanged;
			watcher._position = itemInfo;
			itemInfo.ExpressionWatcher = watcher;
			itemInfo.NestedComputings = nestedComputings;
			return itemInfo;
		}

		private void unregisterSourceItem(int sourceIndex)
		{
			ItemInfo itemInfo = _itemInfos[sourceIndex];

			Utils.disposeExpressionWatcher(itemInfo.ExpressionWatcher, itemInfo.NestedComputings, this,
				_predicateContainsParametrizedObservableComputationCalls);

			_sourcePositions.Remove(sourceIndex);
		}

		private void expressionWatcher_OnValueChanged(ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs)
		{
			Utils.processSourceItemChange(
				expressionWatcher, 
				sender, 
				eventArgs, 
				_rootSourceWrapper, 
				_sourceAsList, 
				_lastProcessedSourceTickTackVersion, 
				_thisAsSourceItemChangeProcessor,
				ref _isConsistent,
				ref _handledEventSender,
				ref _handledEventArgs,
				ref _deferredProcessings, 
				2, _deferredQueuesCount, this);
		}

		void ISourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher expressionWatcher)
		{
			ItemInfo itemInfo = (ItemInfo) expressionWatcher._position;
			if (itemInfo.PredicateResult) _predicatePassedCount--;

			if (applyPredicate(_sourceAsList[itemInfo.Index], itemInfo.PredicateFunc))
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

		[ExcludeFromCodeCoverage]
		internal void ValidateInternalConsistency()
		{
			_sourcePositions.ValidateInternalConsistency();
			IList<TSourceItem> source = _sourceScalar.getValue(_source, new ObservableCollection<TSourceItem>()) as IList<TSourceItem>;
			// ReSharper disable once PossibleNullReferenceException
			int sourceCount = source.Count;
			if (_itemInfos.Count != sourceCount) throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.1");
			Func<TSourceItem, bool> predicate = _predicateExpressionOriginal.Compile();
			int predicatePassedCount = 0;

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (source != null)
			{
				if (_sourcePositions.List.Count != sourceCount)
					throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.9");

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
						if (!itemInfo.PredicateResult) throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.2");
					}
					else
					{
						if (itemInfo.PredicateResult) throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.3");
					}

					if (_sourcePositions.List[sourceIndex].Index != sourceIndex) throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.4");
					if (itemInfo.ExpressionWatcher._position != _sourcePositions.List[sourceIndex]) throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.5");

					if (!_sourcePositions.List.Contains((ItemInfo) itemInfo.ExpressionWatcher._position))
						throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.6");

					if (itemInfo.ExpressionWatcher._position.Index != sourceIndex)
						throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.10");

				}

				if (predicatePassedCount != _predicatePassedCount) throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.7");
				if (_value != _predicatePassedCount > 0) throw new ValidateInternalConsistencyException("Consistency violation: AnyComputing.8");


			}
		}
	}
}
