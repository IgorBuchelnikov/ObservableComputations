// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;

namespace ObservableComputations
{
	internal static class Utils
	{
		internal static TScalarType getValue<TScalarType>(this IReadScalar<TScalarType> scalar, TScalarType defaultValue)
		{
			return scalar != null ? scalar.Value : defaultValue;
		}

		internal static TScalarType getValue<TScalarType>(this IReadScalar<TScalarType> scalar, TScalarType defaultValue, TScalarType defaultDefaultValue)
		{
			return scalar != null 
				? scalar.Value == null
					? defaultDefaultValue
					: scalar.Value
				: defaultValue == null
					? defaultDefaultValue
					: defaultValue;
		}

		internal static Expression<Func<TSourceItem, int, bool>> getIndexedPredicate<TSourceItem>(this Expression<Func<TSourceItem, bool>> predicate)
		{
			Expression<Func<TSourceItem, int, bool>> predicate1 =
				Expression.Lambda<Func<TSourceItem, int, bool>>(predicate.Body,
					new[] {predicate.Parameters[0], Expression.Parameter(typeof(int), "index")});
			return predicate1;
		}

		
		internal static int getCapacity(INotifyCollectionChanged source)
		{
			//return 0;
			return source is IHasCapacity sourceCapacity ? sourceCapacity.Capacity : ((IList) source)?.Count ?? 0;
		}

		
		internal static int getCapacity(IReadScalar<INotifyCollectionChanged> sourceScalar)
		{
			//return 0;
			return sourceScalar.Value is IHasCapacity sourceCapacity ? sourceCapacity.Capacity : ((IList) sourceScalar.Value)?.Count ?? 0;
		}

		internal static void disposeExpressionItemInfos<TItemInfo>(
			List<TItemInfo> itemInfos,
			int expressionCallCount,
			IComputingInternal computing)
			where TItemInfo : ExpressionItemInfo
		{

			disposeExpressionItemInfos(itemInfos, expressionCallCount,
				(callIndexTotal, itemInfo, propertyChangedEventSubscriptions, methodChangedEventSubscriptions) =>
				{
					int newCallIndexTotal = disposeExpressionWatcher(expressionCallCount, itemInfo.ExpressionWatcher, callIndexTotal, propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
					disposeNestedComputings(computing, itemInfo.NestedComputings);
					return newCallIndexTotal;
				});
		}

		internal static void disposeKeyValueExpressionItemInfos<TKey, TValue>(
			List<KeyValueExpressionItemInfo<TKey, TValue>> itemInfos,
			int keyExpressionCallCount,
			int valueExpressionCallCount,
			IComputingInternal computing)
 
		{
			disposeExpressionItemInfos(itemInfos, keyExpressionCallCount + valueExpressionCallCount,
				(callIndexTotal, itemInfo, propertyChangedEventSubscriptions, methodChangedEventSubscriptions) =>
				{
					int newCallIndexTotal = disposeExpressionWatcher(keyExpressionCallCount, itemInfo.KeyExpressionWatcher, callIndexTotal, propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
					disposeNestedComputings(computing, itemInfo.KeyNestedComputings);
					newCallIndexTotal = disposeExpressionWatcher(valueExpressionCallCount, itemInfo.ValueExpressionWatcher, newCallIndexTotal, propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
					disposeNestedComputings(computing, itemInfo.ValueNestedComputings);
					return newCallIndexTotal;
				});
		}

		private static void disposeExpressionItemInfos<TItemInfo>(
			List<TItemInfo> itemInfos,
			int expressionCallCount,
			Func<int, TItemInfo, PropertyChangedEventSubscription[], MethodChangedEventSubscription[], int> disposeItemInfo)
		{
			int itemInfosCount = itemInfos.Count;

			int callCount = expressionCallCount * itemInfosCount;

			PropertyChangedEventSubscription[] propertyChangedEventSubscriptions =
				new PropertyChangedEventSubscription[callCount];

			MethodChangedEventSubscription[] methodChangedEventSubscriptions =
				new MethodChangedEventSubscription[callCount];

			int callIndexTotal = 0;
			for (int index = 0; index < itemInfosCount; index++)
				callIndexTotal = disposeItemInfo(callIndexTotal, itemInfos[index], propertyChangedEventSubscriptions,
					methodChangedEventSubscriptions);

			EventUnsubscriber.QueueSubscriptions(propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
		}

		private static void disposeNestedComputings(IComputingInternal computing, List<IComputingInternal> nestedComputings)
		{
			if (nestedComputings != null)
			{
				int nestedComputingsCount = nestedComputings.Count;
				for (int computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
					nestedComputings[computingIndex].RemoveDownstreamConsumedComputing(computing);
			}
		}

		private static int disposeExpressionWatcher(int expressionCallCount, ExpressionWatcher expressionWatcher,
			int callIndexTotal, PropertyChangedEventSubscription[] propertyChangedEventSubscriptions,
			MethodChangedEventSubscription[] methodChangedEventSubscriptions) 
		{
			expressionWatcher.Dispose();
			int nextUpperCallIndexTotal = callIndexTotal + expressionCallCount;

			int callIndex = 0;
			for (; callIndexTotal < nextUpperCallIndexTotal; callIndexTotal++)
			{
				propertyChangedEventSubscriptions[callIndexTotal] =
					expressionWatcher._propertyChangedEventSubscriptions[callIndex];
				methodChangedEventSubscriptions[callIndexTotal] =
					expressionWatcher._methodChangedEventSubscriptions[callIndex];
				callIndex++;
			}

			return callIndexTotal;
		}

		internal static int disposeSource<TItemInfo>(IReadScalar<INotifyCollectionChanged> sourceScalar,
			INotifyCollectionChanged source, out List<TItemInfo> itemInfos, out Positions<TItemInfo> sourcePositions,
			INotifyCollectionChanged sourceAsList,
			NotifyCollectionChangedEventHandler sourceNotifyCollectionChangedEventHandler)
			where TItemInfo : Position, new()
		{
			int capacity = getCapacity(sourceScalar, source);

			disposeSource(
				capacity, 
				out itemInfos,
				out sourcePositions,
				sourceAsList,
				sourceNotifyCollectionChangedEventHandler);

			return capacity;
		}

		internal static int getCapacity(IReadScalar<INotifyCollectionChanged> sourceScalar, INotifyCollectionChanged source)
		{
			return sourceScalar != null ? getCapacity(sourceScalar) : getCapacity(source);
		}

		private static void disposeSource<TItemInfo>(int capacity, out List<TItemInfo> itemInfos,
			out Positions<TItemInfo> sourcePositions, INotifyCollectionChanged sourceAsList,
			NotifyCollectionChangedEventHandler sourceNotifyCollectionChangedEventHandler)
			where TItemInfo : Position, new()
		{
			initializeItemInfos(capacity, out itemInfos, out sourcePositions);

			unsubscribeSource(sourceAsList, sourceNotifyCollectionChangedEventHandler);
		}

		internal static void unsubscribeSource(INotifyCollectionChanged sourceAsList,
			NotifyCollectionChangedEventHandler sourceNotifyCollectionChangedEventHandler)
		{
			if (sourceAsList != null)
				sourceAsList.CollectionChanged -= sourceNotifyCollectionChangedEventHandler;

			if (sourceAsList is IRootSourceWrapper rootSourceWrapper)
				rootSourceWrapper.Uninitialize();
		}

		internal static void initializeItemInfos<TItemInfo>(int capacity, out List<TItemInfo> itemInfos, out Positions<TItemInfo> sourcePositions)
			where TItemInfo : Position, new()
		{
			itemInfos = new List<TItemInfo>(capacity);
			sourcePositions = new Positions<TItemInfo>(itemInfos);
		}

		internal static void initializeItemInfos<TItemInfo>(int capacity, out List<TItemInfo> itemInfos, out RangePositions<TItemInfo> sourcePositions)
			where TItemInfo : RangePosition, new()
		{
			itemInfos = new List<TItemInfo>(capacity);
			sourcePositions = new RangePositions<TItemInfo>(itemInfos);
		}

		internal static void changeSource<TSource, TSourceAsList>(ref TSource source,
			IReadScalar<INotifyCollectionChanged> sourceScalar,
			List<IComputingInternal> downstreamConsumedComputings,
			List<OcConsumer> consumers,
			IComputingInternal computing,
			out TSourceAsList sourceAsList,
			bool setSourceAsList)
			where TSource : INotifyCollectionChanged where TSourceAsList : class
		{
			IComputingInternal originalSource = source as IComputingInternal;
			if (sourceScalar != null) source = (TSource) sourceScalar.Value;
			IComputingInternal newSource = source as IComputingInternal;

			if (originalSource != newSource)
			{
				for (int index = 0; index < downstreamConsumedComputings.Count; index++)
				{
					IComputingInternal downstreamConsumedComputing = downstreamConsumedComputings[index];
					originalSource?.RemoveDownstreamConsumedComputing(downstreamConsumedComputing);
					newSource?.AddDownstreamConsumedComputing(downstreamConsumedComputing);
				}

				originalSource?.RemoveDownstreamConsumedComputing(computing);
				if (consumers.Count > 0)
					newSource?.AddDownstreamConsumedComputing(computing);
			}

			if (setSourceAsList)
				sourceAsList = source as TSourceAsList;
			else
				sourceAsList = null;
		}

		internal static void initializeFromObservableCollectionWithChangeMarker<TSourceItem>(INotifyCollectionChanged source, ref ObservableCollectionWithChangeMarker<TSourceItem> sourceAsList, ref bool rootSourceWrapper, ref bool lastProcessedSourceChangeMarker)
		{
			if (source == null) return;

			if (source is ObservableCollectionWithChangeMarker<TSourceItem> sourceAsObservableCollectionWithChangeMarker)
			{
				sourceAsList = sourceAsObservableCollectionWithChangeMarker;
				rootSourceWrapper = false;
			}
			else
			{
				sourceAsList = new RootSourceWrapper<TSourceItem>(source);
				rootSourceWrapper = true;
			}

			lastProcessedSourceChangeMarker = sourceAsList.ChangeMarkerField;
		}

		internal static void initializeSourceScalar<TSource>(IReadScalar<INotifyCollectionChanged> sourceScalar, ref TSource source, PropertyChangedEventHandler newSourceScalarOnPropertyChanged)
		{
			if (sourceScalar != null)
			{
				sourceScalar.PropertyChanged += newSourceScalarOnPropertyChanged;
				source = (TSource) sourceScalar.Value;
			}
		}

		internal static void initializeNestedComputings(List<IComputingInternal> nestedComputings, IComputingInternal computing)
		{
			if (nestedComputings != null)
			{
				int nestedComputingsCount = nestedComputings.Count;
				for (int computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
					nestedComputings[computingIndex].AddDownstreamConsumedComputing(computing);
			}
		}

		internal static  void uninitializeNestedComputings(List<IComputingInternal> computingInternals, IComputingInternal computing)
		{
			if (computingInternals != null)
			{
				int nestedComputingsCount = computingInternals.Count;
				for (int computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
					computingInternals[computingIndex].RemoveDownstreamConsumedComputing(computing);
			}
		}

		internal static void uninitializeSourceScalar<TSource>(IReadScalar<INotifyCollectionChanged> sourceScalar, PropertyChangedEventHandler sourceScalarOnPropertyChanged, ref TSource source) 
			where TSource : class
		{
			if (sourceScalar != null)
			{
				sourceScalar.PropertyChanged -= sourceScalarOnPropertyChanged;
				source = null;
			}
		}

		internal static  void checkConsistent(object sender, EventArgs eventArgs, bool isConsistent, IComputing computing)
		{
			if (!isConsistent)
				throw new ObservableComputationsInconsistencyException(computing,
					$"The source collection has been changed. It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.", sender, eventArgs);
		}

		private static Queue<IProcessable> getOrInitializeDeferredProcessingsQueue(
			ref Queue<IProcessable>[] deferredProcessings,
			int deferredSourceCollectionChangedEventProcessingsIndex, 
			int deferredProcessingsCount)
		{
			if (deferredProcessings == null)
				deferredProcessings = new Queue<IProcessable>[deferredProcessingsCount];

			Queue<IProcessable> deferredSourceCollectionChangedEventProcessings;
			if (deferredProcessings[deferredSourceCollectionChangedEventProcessingsIndex] == null)
			{
				deferredSourceCollectionChangedEventProcessings = new Queue<IProcessable>();
				deferredProcessings[deferredSourceCollectionChangedEventProcessingsIndex] =
					deferredSourceCollectionChangedEventProcessings;
			}
			else
			{
				deferredSourceCollectionChangedEventProcessings =
					deferredProcessings[deferredSourceCollectionChangedEventProcessingsIndex];
			}

			return deferredSourceCollectionChangedEventProcessings;
		}

		internal static void postHandleChange(
			ref object handledEventSender,
			ref EventArgs handledEventArgs,
			Queue<IProcessable>[] deferredProcessings,
			ref bool isConsistent, IComputingInternal computing)
		{
			processQueue(ref handledEventSender, ref handledEventArgs, deferredProcessings, ref isConsistent, computing);

			postHandleChange(out handledEventSender, out handledEventArgs);
		}

		private static void processQueue(
			ref object handledEventSender, 
			ref EventArgs handledEventArgs,
			Queue<IProcessable>[] deferredProcessings, 
			ref bool isConsistent, 
			IComputingInternal computing)
		{
			if (deferredProcessings != null)
			{
				bool processed = true;
				while (processed)
				{
					processed = false;
					int deferredProcessingsLength = deferredProcessings.Length;
					for (int queueIndex = 0; queueIndex < deferredProcessingsLength; queueIndex++)
					{
						Queue<IProcessable> queue = deferredProcessings[queueIndex];
						if (queue != null && queue.Count > 0)
						{
							IProcessable processable = queue.Dequeue();
							handledEventSender = processable.EventSender;
							handledEventArgs = processable.EventArgs;
							processable.Process(deferredProcessings);
							processed = true;
							break;
						}
					}
				}
			}

			if (!isConsistent)
			{
				isConsistent = true;
				computing.RaiseConsistencyRestored();
			}
		}

		internal static void postHandleChange(out object handledEventSender, out EventArgs handledEventArgs)
		{
			handledEventSender = null;
			handledEventArgs = null;
		}


		internal static void endComputingExecutingUserCode(IComputing computing, int currentThreadId,
			out IComputing userCodeIsCalledFrom)
		{
			if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThreadId, out IComputing _);
			else DebugInfo._computingsExecutingUserCode[currentThreadId] = computing;
			userCodeIsCalledFrom = null;
		}

		internal static int startComputingExecutingUserCode(out IComputing computing,
			out IComputing userCodeIsCalledFrom, IComputing current)
		{
			int currentThreadId = Thread.CurrentThread.ManagedThreadId;
			DebugInfo._computingsExecutingUserCode.TryGetValue(currentThreadId, out computing);
			DebugInfo._computingsExecutingUserCode[currentThreadId] = current;
			userCodeIsCalledFrom = computing;
			return currentThreadId;
		}

		internal static void construct<TItemInfo, TExpression>(Expression<TExpression> expressionToProcess,
			int capacity,
			out List<TItemInfo> itemInfos,
			out Positions<TItemInfo> sourcePositions,
			out Expression<TExpression> expressionOriginal,
			out Expression<TExpression> expression,
			out bool expressionContainsParametrizedObservableComputationsCalls,
			ref ExpressionWatcher.ExpressionInfo expressionInfo,
			ref int expressionCallCount,
			ref TExpression func,
			ref List<IComputingInternal> nestedComputing) where TItemInfo : Position, new()
		{
			construct(capacity, out itemInfos, out sourcePositions);
			construct(expressionToProcess, out expressionOriginal, out expression, out expressionContainsParametrizedObservableComputationsCalls, ref expressionInfo, ref expressionCallCount, ref func, ref nestedComputing);
		}

		internal static void construct<TItemInfo>(int capacity, out List<TItemInfo> itemInfos, out Positions<TItemInfo> sourcePositions)
			where TItemInfo : Position, new()
		{
			itemInfos = new List<TItemInfo>(capacity);
			sourcePositions = new Positions<TItemInfo>(itemInfos);
		}

		internal static void construct<TExpression>(Expression<TExpression> expressionToProcess,
			out Expression<TExpression> expressionOriginal,
			out Expression<TExpression> expression,
			out bool expressionContainsParametrizedObservableComputationsCalls,
			ref ExpressionWatcher.ExpressionInfo expressionInfo,
			ref int expressionCallCount,
			ref TExpression func,
			ref List<IComputingInternal> nestedComputing)
		{
			expressionOriginal = expressionToProcess;
			CallToConstantConverter callToConstantConverter =
				new CallToConstantConverter(expressionOriginal.Parameters);
			expression =
				(Expression<TExpression>) callToConstantConverter.Visit(
					expressionOriginal);
			expressionContainsParametrizedObservableComputationsCalls =
				callToConstantConverter.ContainsParametrizedObservableComputationCalls;

			if (!expressionContainsParametrizedObservableComputationsCalls)
			{
				expressionInfo = ExpressionWatcher.GetExpressionInfo(expression);
				expressionCallCount = expressionInfo._callCount;
				// ReSharper disable once PossibleNullReferenceException
				func = expression.Compile();
				nestedComputing = callToConstantConverter.NestedComputings;
			}
		}

		internal static void processSourceItemChange<TSourceItem, TCanProcessSourceItemChange>(
			ExpressionWatcher expressionWatcher,
			object sender,
			EventArgs eventArgs,
			bool rootSourceWrapper,
			ObservableCollectionWithChangeMarker<TSourceItem> observableCollectionWithChangeMarker,
			bool lastProcessedSourceChangeMarker,
			TCanProcessSourceItemChange thisAsCanProcessSourceItemChange,
			ref bool isConsistent, 
			ref object handledEventSender, 
			ref EventArgs handledEventArgs,
			ref Queue<IProcessable>[] deferredProcessings,
			int deferredSourceItemChangedEventProcessingsIndex,
			int deferredProcessingsCount,
			IComputingInternal computing) 
			where TCanProcessSourceItemChange : ISourceItemChangeProcessor
		{
			processExpressionWatcherNestedComputings(expressionWatcher, thisAsCanProcessSourceItemChange);

			bool canProcessNow = isConsistent && (rootSourceWrapper || observableCollectionWithChangeMarker.ChangeMarkerField == lastProcessedSourceChangeMarker);

			processSourceItemChange(
				expressionWatcher, 
				sender, 
				eventArgs, 
				thisAsCanProcessSourceItemChange, 
				ref deferredProcessings, 
				deferredSourceItemChangedEventProcessingsIndex, 
				deferredProcessingsCount,
				ref isConsistent, 
				ref handledEventSender, 
				ref handledEventArgs, canProcessNow, computing);
		}

		internal static void processSourceItemChange<TSourceItem1, TSourceItem2, TCanProcessSourceItemChange>(
			ExpressionWatcher expressionWatcher,
			object sender, 
			EventArgs eventArgs,
			bool rootSourceWrapper1,
			ObservableCollectionWithChangeMarker<TSourceItem1> observableCollectionWithChangeMarker1,
			bool rootSourceWrapper2,
			ObservableCollectionWithChangeMarker<TSourceItem2> observableCollectionWithChangeMarker2,
			bool lastProcessedSource1ChangeMarker, 
			bool lastProcessedSource2ChangeMarker, 
			TCanProcessSourceItemChange thisAsCanProcessSourceItemChange,		   
			ref bool isConsistent, 
			ref object handledEventSender,
			ref EventArgs handledEventArgs,
			ref Queue<IProcessable>[] deferredProcessings,
			int deferredSourceItemChangedEventProcessingsIndex,
			int deferredProcessingsCount,
			IComputingInternal computing) where TCanProcessSourceItemChange : ISourceItemChangeProcessor
		{
			processExpressionWatcherNestedComputings(expressionWatcher, thisAsCanProcessSourceItemChange);

			bool canProcessNow = isConsistent
				&& (rootSourceWrapper1 || observableCollectionWithChangeMarker1.ChangeMarkerField == lastProcessedSource1ChangeMarker)
				&& (rootSourceWrapper2 || observableCollectionWithChangeMarker2.ChangeMarkerField == lastProcessedSource2ChangeMarker);

			processSourceItemChange(
				expressionWatcher, 
				sender, 
				eventArgs, 
				thisAsCanProcessSourceItemChange, 
				ref deferredProcessings, 
				deferredSourceItemChangedEventProcessingsIndex, 
				deferredProcessingsCount,
				ref isConsistent, 
				ref handledEventSender, 
				ref handledEventArgs, canProcessNow, computing);
		}

		internal static void processExpressionWatcherNestedComputings(
			ExpressionWatcher expressionWatcher, 
			IComputingInternal computing)
		{
			if (!expressionWatcher._computingsChanged) return;

			int callCount = expressionWatcher._currentComputings.Length;
			IComputingInternal[] newComputings = new IComputingInternal[callCount];
			expressionWatcher._currentComputings.CopyTo(newComputings, 0);

			for (int callIndex = 0; callIndex < callCount; callIndex++)
			{
				IComputingInternal oldComputing = expressionWatcher._oldComputings[callIndex];
				IComputingInternal newComputing = newComputings[callIndex];
				bool newExistsInOld = false;
				bool oldExistsInNew = false;

				if (oldComputing != null || newComputing != null)
				{
					for (int callIndex1 = 0; callIndex1 < callCount; callIndex1++)
					{
						if (newComputing != null && expressionWatcher._oldComputings[callIndex1] == newComputing)
						{
							newExistsInOld = true;
							expressionWatcher._oldComputings[callIndex1] = null;
							newComputings[callIndex] = null;
						}

						if (oldComputing != null && newComputings[callIndex1] == oldComputing)
						{
							oldExistsInNew = true;
							expressionWatcher._oldComputings[callIndex] = null;
							newComputings[callIndex1] = null;
						}
					}
				}

				if (oldComputing != null && !oldExistsInNew)
					oldComputing.RemoveDownstreamConsumedComputing(computing);

				if (newComputing != null && !newExistsInOld)
					newComputing.AddDownstreamConsumedComputing(computing);
			}
		}

		private static void processSourceItemChange<TCanProcessSourceItemChange>(
			ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs,
			TCanProcessSourceItemChange thisAsCanProcessSourceItemChange, 
			ref Queue<IProcessable>[] deferredProcessings,
			int deferredSourceItemChangedEventProcessingsIndex, 
			int deferredProcessingsCount,
			ref bool isConsistent, 
			ref object handledEventSender, 
			ref EventArgs handledEventArgs, 
			bool canProcessNow,
			IComputingInternal computing)
			where TCanProcessSourceItemChange : ISourceItemChangeProcessor
		{
			if (canProcessNow)
			{
				handledEventSender = sender;
				handledEventArgs = eventArgs;
				isConsistent = false;

				if (typeof(TCanProcessSourceItemChange) == typeof(ISourceItemChangeProcessor))
					thisAsCanProcessSourceItemChange.ProcessSourceItemChange(expressionWatcher);
				else if (typeof(TCanProcessSourceItemChange) == typeof(ISourceItemKeyChangeProcessor))
					((ISourceItemKeyChangeProcessor) thisAsCanProcessSourceItemChange).ProcessSourceItemChange(
						expressionWatcher);
				else if (typeof(TCanProcessSourceItemChange) == typeof(ISourceItemValueChangeProcessor))
					((ISourceItemValueChangeProcessor) thisAsCanProcessSourceItemChange).ProcessSourceItemChange(
						expressionWatcher);


				postHandleChange(
					ref handledEventSender,
					ref handledEventArgs,
					deferredProcessings,
					ref isConsistent,
					computing);
			}
			else
			{
				getOrInitializeDeferredProcessingsQueue(
					ref deferredProcessings, 
					deferredSourceItemChangedEventProcessingsIndex, 
					deferredProcessingsCount)
				.Enqueue(new ExpressionWatcher.Raise(expressionWatcher, sender, eventArgs, thisAsCanProcessSourceItemChange, typeof(TCanProcessSourceItemChange)));
			}
		}

		internal static void getItemInfoContent<TExpression, TExpressionCompiled>(object[] sourceItems,
			out ExpressionWatcher watcher,
			out TExpressionCompiled func,
			out List<IComputingInternal> nestedComputings,
			Expression<TExpression> expression,
			out int expressionCallCount,
			IComputingInternal current,
			bool expressionContainsParametrizedLiveLinqCalls,
			ExpressionWatcher.ExpressionInfo valueSelectorExpressionInfo)
		{
			if (!expressionContainsParametrizedLiveLinqCalls)
			{
				watcher = new ExpressionWatcher(valueSelectorExpressionInfo, sourceItems);
				func = default(TExpressionCompiled);
				nestedComputings = null;
				expressionCallCount = valueSelectorExpressionInfo._callCount;
			}
			else
			{
				Expression<TExpressionCompiled> deparametrizedPredicateExpression =
					(Expression<TExpressionCompiled>) expression.ApplyParameters(sourceItems);
				CallToConstantConverter callToConstantConverter = new CallToConstantConverter();
				Expression<TExpressionCompiled> predicateExpression =
					(Expression<TExpressionCompiled>)
					callToConstantConverter.Visit(deparametrizedPredicateExpression);
				// ReSharper disable once PossibleNullReferenceException
				func = predicateExpression.Compile();

				nestedComputings = callToConstantConverter.NestedComputings;
				int nestedComputingsCount = nestedComputings.Count;

				for (int computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
					nestedComputings[computingIndex].AddDownstreamConsumedComputing(current);

				ExpressionWatcher.ExpressionInfo expressionInfo =
					ExpressionWatcher.GetExpressionInfo(predicateExpression);
				watcher = new ExpressionWatcher(expressionInfo);

				expressionCallCount = expressionInfo._callCount;
			}

			initializeExpressionWatcherCurrentComputings(watcher, expressionCallCount, current);
		}

		internal static void initializeExpressionWatcherCurrentComputings(
			ExpressionWatcher watcher, 
			int expressionCallCount, 
			IComputingInternal current)
		{
			for (int computingIndex = 0; computingIndex < expressionCallCount; computingIndex++)
				watcher._currentComputings[computingIndex]?.AddDownstreamConsumedComputing(current);
		}

		private static void itemInfoRemoveDownstreamConsumedComputing(List<IComputingInternal> nestedComputings, IComputingInternal current)
		{
			int nestedComputingsCount = nestedComputings.Count;
			for (int computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
				nestedComputings[computingIndex].RemoveDownstreamConsumedComputing(current);
		}

		internal static void construct<TOrderingValue>(int sourceCapacity,
			out List<OrderedItemInfo<TOrderingValue>> orderedItemInfos,
			out Positions<OrderedItemInfo<TOrderingValue>> orderedPositions, out List<TOrderingValue> orderingValues)
		{
			orderedItemInfos = new List<OrderedItemInfo<TOrderingValue>>(sourceCapacity);
			orderedPositions = new Positions<OrderedItemInfo<TOrderingValue>>(orderedItemInfos);
			orderingValues = new List<TOrderingValue>(sourceCapacity);
		}

		internal static void addConsumer(
			OcConsumer addingOcConsumer,
			List<OcConsumer> consumers,
			List<IComputingInternal> downstreamConsumedComputings,
			IComputingInternal current,
			ref bool isConsistent,
			ref object handledEventSender,
			ref EventArgs handledEventArgs,
			ref Queue<IProcessable>[] deferredProcessings,
			int deferredQueuesCount)
		{
			processChange(null, null, () =>
			{
				int consumersCount = consumers.Count;
				for (int index = 0; index < consumersCount; index++)
					if (consumers[index] == addingOcConsumer)
						return;

				consumers.Add(addingOcConsumer);
				addingOcConsumer.AddComputing(current);

				if (consumers.Count == 1)
				{
					if (downstreamConsumedComputings.Count == 0)
					{
						current.SetActivationInProgress(true);
						current.OnPropertyChanged(ActivationInProgressPropertyChangedEventArgs);
						current.SetIsActive(true);
						current.Initialize();
						current.AddToUpstreamComputings(current);
						current.ProcessSource();
						current.InitializedFromSource = true;
						current.OnPropertyChanged(IsActivePropertyChangedEventArgs);
						current.SetActivationInProgress(false);
						current.OnPropertyChanged(ActivationInProgressPropertyChangedEventArgs);
					}
					else
					{
						current.AddToUpstreamComputings(current);
					}
				}
			},
			ref isConsistent,
			ref handledEventSender,
			ref handledEventArgs,
			0, deferredQueuesCount,
			ref deferredProcessings, 
			current, false);
		}

		internal static void removeConsumer(
			OcConsumer removingOcConsumer, 
			List<OcConsumer> consumers, 
			List<IComputingInternal> downstreamConsumedComputings, 
			IComputingInternal current,
			ref bool isConsistent,
			ref object handledEventSender,
			ref EventArgs handledEventArgs,
			Queue<IProcessable>[] deferredProcessings,
			int deferredQueuesCount)
		{
			processChange(null, null, () =>
				{
					consumers.Remove(removingOcConsumer);

					if (consumers.Count == 0 && downstreamConsumedComputings.Count == 0)
					{
						current.SetInactivationInProgress(true);
						current.OnPropertyChanged(InactivationInProgressPropertyChangedEventArgs);
						current.SetIsActive(false);
						current.ProcessSource();
						current.InitializedFromSource = false;
						current.RemoveFromUpstreamComputings(current);
						current.Uninitialize();
						current.OnPropertyChanged(IsActivePropertyChangedEventArgs); 
						current.SetInactivationInProgress(false);
						current.OnPropertyChanged(InactivationInProgressPropertyChangedEventArgs);
						// ReSharper disable once AccessToModifiedClosure
						clearDeferredProcessings(deferredProcessings, 0);						
					}
				},
				ref isConsistent,
				ref handledEventSender,
				ref handledEventArgs,
				0, deferredQueuesCount,
				ref deferredProcessings, 
				current, false);
		}

		internal static void addDownstreamConsumedComputing(
			IComputingInternal computing,
			List<IComputingInternal> downstreamConsumedComputings,
			List<OcConsumer> consumers,
			IComputingInternal current,
			ref bool isConsistent,
			ref object handledEventSender,
			ref EventArgs handledEventArgs,
			ref Queue<IProcessable>[] deferredProcessings,
			int deferredQueuesCount)
		{
			processChange(null, null, () =>
				{
					downstreamConsumedComputings.Add(computing);

					if (downstreamConsumedComputings.Count == 1 && consumers.Count == 0)
					{
						current.SetActivationInProgress(true);
						current.OnPropertyChanged(ActivationInProgressPropertyChangedEventArgs);
						current.SetIsActive(true);
						current.Initialize();
						current.AddToUpstreamComputings(computing);
						current.ProcessSource();
						current.InitializedFromSource = true;
						current.OnPropertyChanged(IsActivePropertyChangedEventArgs);
						current.SetActivationInProgress(false);
						current.OnPropertyChanged(ActivationInProgressPropertyChangedEventArgs);
					}
					else
						current.AddToUpstreamComputings(computing);
				},
				ref isConsistent,
				ref handledEventSender,
				ref handledEventArgs,
				0, deferredQueuesCount,
				ref deferredProcessings, 
				current, false);
		}

		internal static void removeDownstreamConsumedComputing(
			IComputingInternal computing, 
			List<IComputingInternal> downstreamConsumedComputings, 
			IComputingInternal current, 
			ref bool isConsistent,
			List<OcConsumer> consumers,
			ref object handledEventSender,
			ref EventArgs handledEventArgs,
			Queue<IProcessable>[] deferredProcessings,
			int deferredQueuesCount)
		{
			processChange(null, null, () =>
				{
					downstreamConsumedComputings.Remove(computing);

					if (consumers.Count == 0 && downstreamConsumedComputings.Count == 0)
					{
						current.SetInactivationInProgress(true);
						current.OnPropertyChanged(InactivationInProgressPropertyChangedEventArgs);
						current.SetIsActive(false);
						current.ProcessSource();
						current.InitializedFromSource = false;
						current.RemoveFromUpstreamComputings(computing);
						current.Uninitialize();
						current.OnPropertyChanged(IsActivePropertyChangedEventArgs);
						current.SetInactivationInProgress(false);
						current.OnPropertyChanged(InactivationInProgressPropertyChangedEventArgs);
						// ReSharper disable once AccessToModifiedClosure
						clearDeferredProcessings(deferredProcessings, 0);
					}
					else
						current.RemoveFromUpstreamComputings(computing);
				},
				ref isConsistent,
				ref handledEventSender,
				ref handledEventArgs,
				0, deferredQueuesCount,
				ref deferredProcessings, 
				current, false);
		}

		private static void initializeSourceIndexerPropertyTracker<TSourceList, TSourceIndexerPropertyTracker>(
			out INotifyPropertyChanged sourceAsINotifyPropertyChanged,
			TSourceIndexerPropertyTracker current,
			TSourceList sourceAsList) 
			where TSourceIndexerPropertyTracker : ISourceIndexerPropertyTracker
		{
			sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) sourceAsList;

			if (typeof(TSourceIndexerPropertyTracker) == typeof(ISourceIndexerPropertyTracker))
				sourceAsINotifyPropertyChanged.PropertyChanged += current.HandleSourcePropertyChanged;
			else if (typeof(TSourceIndexerPropertyTracker) == typeof(ILeftSourceIndexerPropertyTracker))
				sourceAsINotifyPropertyChanged.PropertyChanged += ((ILeftSourceIndexerPropertyTracker) current).HandleSourcePropertyChanged;
			else if (typeof(TSourceIndexerPropertyTracker) == typeof(IRightSourceIndexerPropertyTracker))
				sourceAsINotifyPropertyChanged.PropertyChanged += ((IRightSourceIndexerPropertyTracker) current).HandleSourcePropertyChanged;
		}

		internal static void initializeFromHasChangeMarker<TSourceList, TSourceIndexerPropertyTracker>(
			out IHasChangeMarker sourceAsIHasChangeMarker,
			TSourceList sourceAsList,
			ref bool lastProcessedSourceChangeMarker,
			ref INotifyPropertyChanged sourceAsINotifyPropertyChanged,
			TSourceIndexerPropertyTracker current) where TSourceIndexerPropertyTracker : ISourceIndexerPropertyTracker
		{
			sourceAsIHasChangeMarker = sourceAsList as IHasChangeMarker;

			if (sourceAsIHasChangeMarker != null)
			{
				lastProcessedSourceChangeMarker = sourceAsIHasChangeMarker.ChangeMarker;
			}
			else
			{
				initializeSourceIndexerPropertyTracker(out sourceAsINotifyPropertyChanged,
					current, sourceAsList);
			}
		}

		internal static void handleSourcePropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs, ref bool indexerPropertyChangedEventRaised)
		{
			if (propertyChangedEventArgs.PropertyName == "Item[]")
				indexerPropertyChangedEventRaised = true;
		}

		internal static void clearDeferredProcessings(Queue<IProcessable>[] deferredProcessings, int fromIndex = 1)
		{
			if (deferredProcessings == null) return;

			for (int index = fromIndex; index < deferredProcessings.Length; index++)
				deferredProcessings[index]?.Clear();
		}

		internal static bool preHandleSourceCollectionChanged<TSourceItem>(
			object sender,
			NotifyCollectionChangedEventArgs e,
			bool rootSourceWrapper,
			ref bool lastProcessedSourceChangeMarker,
			ObservableCollectionWithChangeMarker<TSourceItem> sourceAsList,
			ref bool isConsistent,
			ref object handledEventSender,
			ref EventArgs handledEventArgs,
			ref Queue<IProcessable>[] deferredProcessings,
			int deferredSourceCollectionChangedEventProcessingsIndex,
			int deferredProcessingsCount,
			ISourceCollectionChangeProcessor sourceCollectionChangeProcessor)
		{
			if (!rootSourceWrapper && lastProcessedSourceChangeMarker == sourceAsList.ChangeMarkerField) return false;

			lastProcessedSourceChangeMarker = !lastProcessedSourceChangeMarker;

			return preHandleSourceCollectionChanged(
				sender, 
				e, 
				ref isConsistent, 
				ref handledEventSender, 
				ref handledEventArgs, 
				ref deferredProcessings, 
				deferredSourceCollectionChangedEventProcessingsIndex, 
				deferredProcessingsCount, 
				sourceCollectionChangeProcessor);
		}

		internal static bool preHandleSourceCollectionChanged(
			object sender, 
			NotifyCollectionChangedEventArgs e,
			ref bool isConsistent, 
			ref object handledEventSender, 
			ref EventArgs handledEventArgs,
			ref Queue<IProcessable>[] deferredProcessings, 
			int deferredSourceCollectionChangedEventProcessingsIndex,
			int deferredProcessingsCount, 
			ISourceCollectionChangeProcessor sourceCollectionChangeProcessor)
		{
			if (isConsistent)
			{
				if (e.Action == NotifyCollectionChangedAction.Reset)
					clearDeferredProcessings(deferredProcessings);

				handledEventSender = sender;
				handledEventArgs = e;
				isConsistent = false;
				return true;
			}

			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				getOrInitializeDeferredProcessingsQueue(
						ref deferredProcessings,
						0,
						deferredProcessingsCount)
					.Enqueue(
						new CollectionReset(sender, e, sourceCollectionChangeProcessor, null));

				return false;
			}

			getOrInitializeDeferredProcessingsQueue(
					ref deferredProcessings,
					deferredSourceCollectionChangedEventProcessingsIndex,
					deferredProcessingsCount)
				.Enqueue(
					new CollectionChangedEventRaise(sender, e, sourceCollectionChangeProcessor));
			return false;
		}

		internal static bool preHandleSourceCollectionChanged(
			object sender, 
			NotifyCollectionChangedEventArgs e, 
			ref bool isConsistent, 
			ref bool indexerPropertyChangedEventRaised, 
			ref bool lastProcessedSourceChangeMarker, 
			IHasChangeMarker sourceAsIHasChangeMarker, 
			ref object handledEventSender, 
			ref EventArgs handledEventArgs,
			ref Queue<IProcessable>[] deferredProcessings, 
			int deferredSourceCollectionChangedEventProcessingsIndex, 
			int deferredProcessingsCount,
			ISourceCollectionChangeProcessor sourceCollectionChangeProcessor)
		{
			if (!preHandleSourceCollectionChanged(
				ref indexerPropertyChangedEventRaised, 
				ref lastProcessedSourceChangeMarker, 
				sourceAsIHasChangeMarker))
				return false;

			return preHandleSourceCollectionChanged(sender, e, ref isConsistent, ref handledEventSender, ref handledEventArgs, ref deferredProcessings, deferredSourceCollectionChangedEventProcessingsIndex, deferredProcessingsCount, sourceCollectionChangeProcessor);
		}

		internal static bool preHandleSourceCollectionChanged(ref bool indexerPropertyChangedEventRaised,
			ref bool lastProcessedSourceChangeMarker, IHasChangeMarker sourceAsIHasChangeMarker)
		{
			if (!indexerPropertyChangedEventRaised &&
				(sourceAsIHasChangeMarker == null
				 || lastProcessedSourceChangeMarker == sourceAsIHasChangeMarker.ChangeMarker))
				return false;

			lastProcessedSourceChangeMarker = !lastProcessedSourceChangeMarker;
			indexerPropertyChangedEventRaised = false;
			return true;
		}

		internal static void disposeExpressionWatcher(ExpressionWatcher watcher, List<IComputingInternal> nestedComputings, IComputingInternal current, bool expressionContainsParametrizedObservableComputationsCalls)
		{
			watcher.Dispose();
			EventUnsubscriber.QueueSubscriptions(watcher._propertyChangedEventSubscriptions,
				watcher._methodChangedEventSubscriptions);

			if (expressionContainsParametrizedObservableComputationsCalls)
				itemInfoRemoveDownstreamConsumedComputing(nestedComputings, current);

			removeDownstreamConsumedComputing(watcher, current);
		}

		internal static void processResetChange(
			object sender, 
			PropertyChangedEventArgs args, 
			ref bool isConsistent, 
			ref object handledEventSender, 
			ref EventArgs handledEventArgs, 
			Action scalarValueChangedHandlerAction, 
			int deferredQueuesCount,			
			ref Queue<IProcessable>[] deferredProcessings, IComputingInternal computing)
		{
			processChange(sender, args, 
				() =>
				{
					scalarValueChangedHandlerAction?.Invoke();
					computing.ProcessSource();
				}, 
				() => new CollectionReset(sender, args, computing, scalarValueChangedHandlerAction),
				ref isConsistent, ref handledEventSender, ref handledEventArgs,
				0, deferredQueuesCount, ref deferredProcessings,
				computing);
		}

		internal static void processChange(
			object sender, 
			EventArgs args, 
			Action action,
			ref bool isConsistent, 
			ref object handledEventSender, 
			ref EventArgs handledEventArgs, 
			int deferredQueueIndex,
			int deferredQueuesCount,			  
			ref Queue<IProcessable>[] deferredProcessings, 
			IComputingInternal computing,
			bool propertyNameValue = true)
		{
			processChange(sender, args, action, () => new Processable(sender, args, action),
				ref isConsistent, ref handledEventSender, ref handledEventArgs,
				deferredQueueIndex, deferredQueuesCount, ref deferredProcessings,
				computing, propertyNameValue);
		}

		private static void processChange(
			object sender, 
			EventArgs args, 
			Action action,
			Func<IProcessable> getProcessable,
			ref bool isConsistent, 
			ref object handledEventSender, 
			ref EventArgs handledEventArgs, 
			int deferredQueueIndex,
			int deferredQueuesCount,			  
			ref Queue<IProcessable>[] deferredProcessings, 
			IComputingInternal computing,
			bool propertyNameValue = true)
		{
			if (propertyNameValue 
				&& args is PropertyChangedEventArgs propertyChangedEventArgs 
				&&  propertyChangedEventArgs.PropertyName != nameof(IReadScalar<object>.Value)) return;

			if (isConsistent)
			{
				handledEventSender = sender;
				handledEventArgs = args;
				isConsistent = false;

				action();

				postHandleChange(
					ref handledEventSender,
					ref handledEventArgs,
					deferredProcessings,
					ref isConsistent,
					computing);
			}
			else
			{
				getOrInitializeDeferredProcessingsQueue(ref deferredProcessings, deferredQueueIndex, deferredQueuesCount)
					.Enqueue(getProcessable());
			}
		}

		internal static void removeDownstreamConsumedComputing(ExpressionWatcher watcher, IComputingInternal current)
		{
			int currentComputingsLength = watcher._currentComputings.Length;
			for (int computingIndex = 0; computingIndex < currentComputingsLength; computingIndex++)
				watcher._currentComputings[computingIndex]?.RemoveDownstreamConsumedComputing(current);
		}

		internal static void removeDownstreamConsumedComputing<TItemInfo>(List<TItemInfo> itemInfos, IComputingInternal current) where TItemInfo : ExpressionItemInfo
		{
			int itemInfosCount = itemInfos.Count;
			for (int index = 0; index < itemInfosCount; index++)
				removeDownstreamConsumedComputing(itemInfos[index].ExpressionWatcher, current);
		}

		internal static void removeDownstreamConsumedComputing<TKey, TValue>(List<KeyValueExpressionItemInfo<TKey, TValue>> itemInfos, IComputingInternal current)
		{
			int itemInfosCount = itemInfos.Count;
			for (int index = 0; index < itemInfosCount; index++)
			{
				KeyValueExpressionItemInfo<TKey, TValue> itemInfo = itemInfos[index];
				removeDownstreamConsumedComputing(itemInfo.KeyExpressionWatcher, current);
				removeDownstreamConsumedComputing(itemInfo.ValueExpressionWatcher, current);
			}
		}

		private static void addDownstreamConsumedComputing(ExpressionWatcher watcher, IComputingInternal current)
		{
			int currentComputingsLength = watcher._currentComputings.Length;
			for (int computingIndex = 0; computingIndex < currentComputingsLength; computingIndex++)
				watcher._currentComputings[computingIndex]?.AddDownstreamConsumedComputing(current);
		}

		internal static void AddDownstreamConsumedComputing(IComputingInternal computing, IReadScalar<INotifyCollectionChanged> sourceScalar, INotifyCollectionChanged source)
		{
			(sourceScalar as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
			(source as IComputingInternal)?.AddDownstreamConsumedComputing(computing);
		}

		internal static void RemoveDownstreamConsumedComputing(IComputingInternal computing, IReadScalar<INotifyCollectionChanged> sourceScalar, INotifyCollectionChanged source)
		{
			(source as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
			(sourceScalar as IComputingInternal)?.RemoveDownstreamConsumedComputing(computing);
		}

		internal static readonly PropertyChangedEventArgs InsertItemIntoGroupRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("InsertItemIntoGroupRequestHandler");
		internal static readonly PropertyChangedEventArgs RemoveItemFromGroupRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemFromGroupRequestHandler");
		internal static readonly PropertyChangedEventArgs SetGroupItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("SetGroupItemRequestHandler");
		internal static readonly PropertyChangedEventArgs MoveItemInGroupRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("MoveItemInGroupRequestHandler");
		internal static readonly PropertyChangedEventArgs ClearGroupItemsRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("ClearGroupItemsRequestHandler");
		internal static readonly PropertyChangedEventArgs OuterItemPropertyChangedEventArgs = new PropertyChangedEventArgs("OuterItem");
		internal static readonly PropertyChangedEventArgs KeyPropertyChangedEventArgs = new PropertyChangedEventArgs("Key");
		internal static readonly PropertyChangedEventArgs SetLeftItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("SetLeftItemRequestHandler");
		internal static readonly PropertyChangedEventArgs SetRightItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("SetRightItemRequestHandler");
		internal static readonly PropertyChangedEventArgs LeftItemPropertyChangedEventArgs = new PropertyChangedEventArgs("LeftItem");
		internal static readonly PropertyChangedEventArgs RightItemPropertyChangedEventArgs = new PropertyChangedEventArgs("RightItem");
		internal static readonly PropertyChangedEventArgs InsertItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("InsertItemRequestHandler");
		internal static readonly PropertyChangedEventArgs AddItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("AddItemRequestHandler");
		internal static readonly PropertyChangedEventArgs RemoveItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemRequestHandler");
		internal static readonly PropertyChangedEventArgs SetItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("SetItemRequestHandler");
		internal static readonly PropertyChangedEventArgs MoveItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("MoveItemRequestHandler");
		internal static readonly PropertyChangedEventArgs ClearItemsRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("ClearItemsRequestHandler");
		internal static readonly PropertyChangedEventArgs ValuePropertyChangedEventArgs = new PropertyChangedEventArgs("Value");
		internal static readonly PropertyChangedEventArgs ResultPropertyChangedEventArgs = new PropertyChangedEventArgs("Result");
		internal static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs("Count");
		internal static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
		internal static readonly PropertyChangedEventArgs ValueObjectPropertyChangedEventArgs = new PropertyChangedEventArgs("ValueObject");
		internal static readonly PropertyChangedEventArgs SetValueRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("SetValueRequestHandler");
		internal static readonly PropertyChangedEventArgs IsDisabledPropertyChangedEventArgs = new PropertyChangedEventArgs("IsDisabled");
		internal static readonly PropertyChangedEventArgs PreviousValuePropertyChangedEventArgs = new PropertyChangedEventArgs("PreviousValue");
		internal static readonly PropertyChangedEventArgs IsEverChangedPropertyChangedEventArgs = new PropertyChangedEventArgs("IsEverChanged");
		internal static readonly PropertyChangedEventArgs CurrentPagePropertyChangedEventArgs = new PropertyChangedEventArgs("CurrentPage");
		internal static readonly PropertyChangedEventArgs PageCountPropertyChangedEventArgs = new PropertyChangedEventArgs("PageCount");
		internal static readonly PropertyChangedEventArgs PageSizePropertyChangedEventArgs = new PropertyChangedEventArgs("PageSize");
		internal static readonly PropertyChangedEventArgs IsPausedPropertyChangedEventArgs = new PropertyChangedEventArgs("IsPaused");
		internal static readonly NotifyCollectionChangedEventArgs ResetNotifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
		private static readonly PropertyChangedEventArgs IsActivePropertyChangedEventArgs = new PropertyChangedEventArgs("IsActive");
		internal static readonly PropertyChangedEventArgs ResumeTypePropertyChangedEventArgs = new PropertyChangedEventArgs("ResumeType");
		internal static readonly PropertyChangedEventArgs IsDefaultedPropertyChangedEventArgs = new PropertyChangedEventArgs("IsDefaulted");
		internal static readonly PropertyChangedEventArgs IsDisposedPropertyChangedEventArgs = new PropertyChangedEventArgs("IsDisposed");
		private static readonly PropertyChangedEventArgs ActivationInProgressPropertyChangedEventArgs = new PropertyChangedEventArgs("ActivationInProgress");
		private static readonly PropertyChangedEventArgs InactivationInProgressPropertyChangedEventArgs = new PropertyChangedEventArgs("InactivationInProgress");
		public static PropertyChangedEventArgs ApplyOnSourceChangedPropertyChangedEventArgs = new PropertyChangedEventArgs("ApplyOnSourceChanged");
		public static PropertyChangedEventArgs ApplyOnActivationPropertyChangedEventArgs = new PropertyChangedEventArgs("ApplyOnActivation");
	}
}
