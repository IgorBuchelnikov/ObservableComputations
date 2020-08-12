using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ObservableComputations.ExtentionMethods;

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
            int expressionСallCount,
            IComputingInternal computing)
            where TItemInfo : ExpressionItemInfo
        {

            disposeExpressionItemInfos(itemInfos, expressionСallCount,
                (callIndexTotal, itemInfo, propertyChangedEventSubscriptions, methodChangedEventSubscriptions) =>
                {
                    int newCallIndexTotal = disposeExpressionWatcher(expressionСallCount, itemInfo.ExpressionWatcher, callIndexTotal, propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
                    disposeNestedComputings(computing, itemInfo.NestedComputings);
                    return newCallIndexTotal;
                });
        }

        internal static void disposeKeyValueExpressionItemInfos<TItemInfo>(
            List<TItemInfo> itemInfos,
            int keyExpressionСallCount,
            int valueExpressionСallCount,
            IComputingInternal computing)
            where TItemInfo : KeyValueExpressionItemInfo
        {
            disposeExpressionItemInfos(itemInfos, keyExpressionСallCount + valueExpressionСallCount,
                (callIndexTotal, itemInfo, propertyChangedEventSubscriptions, methodChangedEventSubscriptions) =>
                {
                    int newCallIndexTotal = disposeExpressionWatcher(keyExpressionСallCount, itemInfo.KeyExpressionWatcher, callIndexTotal, propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
                    disposeNestedComputings(computing, itemInfo.KeyNestedComputings);
                    newCallIndexTotal = disposeExpressionWatcher(valueExpressionСallCount, itemInfo.ValueExpressionWatcher, newCallIndexTotal, propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
                    disposeNestedComputings(computing, itemInfo.ValueNestedComputings);
                    return newCallIndexTotal;
                });
        }

        internal static void disposeExpressionItemInfos<TItemInfo>(
            List<TItemInfo> itemInfos,
            int expressionСallCount,
            Func<int, TItemInfo, PropertyChangedEventSubscription[], MethodChangedEventSubscription[], int> disposeItemInfo)
        {
            int itemInfosCount = itemInfos.Count;

            int callCount = expressionСallCount * itemInfosCount;

            PropertyChangedEventSubscription[] propertyChangedEventSubscriptions =
                new PropertyChangedEventSubscription[callCount];

            MethodChangedEventSubscription[] methodChangedEventSubscriptions =
                new MethodChangedEventSubscription[callCount];

            int callIndexTotal = 0;
            for (var index = 0; index < itemInfosCount; index++)
            {
                callIndexTotal = disposeItemInfo(callIndexTotal, itemInfos[index], propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
            }

            EventUnsubscriber.QueueSubscriptions(propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
        }

        private static void disposeNestedComputings(IComputingInternal computing, List<IComputingInternal> nestedComputings)
        {
            if (nestedComputings != null)
            {
                int nestedComputingsCount = nestedComputings.Count;
                for (var computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
                    nestedComputings[computingIndex].RemoveDownstreamConsumedComputing(computing);
            }
        }

        private static int disposeExpressionWatcher(int expressionСallCount, ExpressionWatcher expressionWatcher,
            int callIndexTotal, PropertyChangedEventSubscription[] propertyChangedEventSubscriptions,
            MethodChangedEventSubscription[] methodChangedEventSubscriptions) 
        {
            int callIndex;
            expressionWatcher.Dispose();
            int nextUpperCallIndexTotal = callIndexTotal + expressionСallCount;

            callIndex = 0;
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

        internal static int disposeSource<TItemInfo>(IReadScalar<INotifyCollectionChanged> sourceScalar, INotifyCollectionChanged source, ref List<TItemInfo> itemInfos, ref Positions<TItemInfo> sourcePositions,INotifyCollectionChanged sourceAsList, NotifyCollectionChangedEventHandler sourceNotifyCollectionChangedEventHandler)
            where TItemInfo : Position, new()
        {
            int capacity = getCapacity(sourceScalar, source);

            disposeSource(
                capacity, 
                ref itemInfos,
                ref sourcePositions,
                sourceAsList,
                ref sourceNotifyCollectionChangedEventHandler);

            return capacity;
        }

        internal static int getCapacity(IReadScalar<INotifyCollectionChanged> sourceScalar, INotifyCollectionChanged source)
        {
            return sourceScalar != null ? Utils.getCapacity(sourceScalar) : Utils.getCapacity(source);
        }

        internal static void disposeSource<TItemInfo>(int capacity, ref List<TItemInfo> itemInfos, ref Positions<TItemInfo> sourcePositions,INotifyCollectionChanged sourceAsList, ref NotifyCollectionChangedEventHandler sourceNotifyCollectionChangedEventHandler)
            where TItemInfo : Position, new()
        {
            initializeItemInfos(capacity, out itemInfos, out sourcePositions);

            unsubscribeSource(sourceAsList, sourceNotifyCollectionChangedEventHandler);
        }

        internal static void unsubscribeSource(INotifyCollectionChanged sourceAsList,
            NotifyCollectionChangedEventHandler sourceNotifyCollectionChangedEventHandler)
        {
            sourceAsList.CollectionChanged -= sourceNotifyCollectionChangedEventHandler;
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

        internal static void changeSource<TSource, TSourceAsList>(
            ref TSource source,
            IReadScalar<INotifyCollectionChanged> sourceScalar,
            List<IComputingInternal> downstreamConsumedComputings,
            List<Consumer> consumers,
            IComputingInternal computing, 
            ref TSourceAsList sourceAsList,
            TSourceAsList sourceAsListValue)
            where TSource : INotifyCollectionChanged where TSourceAsList : class
        {
            IComputingInternal originalSource = source as IComputingInternal;
            if (sourceScalar != null) source = (TSource) sourceScalar.Value;
            IComputingInternal newSource = source as IComputingInternal;

            if (originalSource != newSource)
            {
                for (var index = 0; index < downstreamConsumedComputings.Count; index++)
                {
                    IComputingInternal downstreamConsumedComputing = downstreamConsumedComputings[index];
                    originalSource?.RemoveDownstreamConsumedComputing(downstreamConsumedComputing);
                    newSource?.AddDownstreamConsumedComputing(downstreamConsumedComputing);
                }

                originalSource?.RemoveDownstreamConsumedComputing(computing);
                if (consumers.Count > 0)
                    newSource?.AddDownstreamConsumedComputing(computing);
            }

            sourceAsList = sourceAsListValue;
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
                for (var computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
                    nestedComputings[computingIndex].AddDownstreamConsumedComputing(computing);
            }
        }

        internal static  void uninitializeNestedComputings(List<IComputingInternal> computingInternals, IComputingInternal computing)
        {
            if (computingInternals != null)
            {
                int nestedComputingsCount = computingInternals.Count;
                for (var computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
                    computingInternals[computingIndex].RemoveDownstreamConsumedComputing(computing);
            }
        }

        internal static void uninitializeSourceScalar(IReadScalar<INotifyCollectionChanged> sourceScalar, PropertyChangedEventHandler sourceScalarOnPropertyChanged, ref INotifyCollectionChanged source)
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

        internal static bool preHandleSourceCollectionChanged<TSourceItem>(object sender,
            NotifyCollectionChangedEventArgs e, bool rootSourceWrapper, ref bool lastProcessedSourceChangeMarker,
            ObservableCollectionWithChangeMarker<TSourceItem> sourceAsList, bool isConsistent, IComputing computing,
            ref object handledEventSender, ref EventArgs handledEventArgs)
        {
            Utils.checkConsistent(sender, e, isConsistent, computing);
            if (!rootSourceWrapper && lastProcessedSourceChangeMarker == sourceAsList.ChangeMarkerField) return false;

            lastProcessedSourceChangeMarker = !lastProcessedSourceChangeMarker;

            handledEventSender = sender;
            handledEventArgs = e;
            return true;
        }

        internal static void postHandleSourceCollectionChanged(
            ref object handledEventSender, ref EventArgs handledEventArgs)
        {
            handledEventSender = null;
            handledEventArgs = null;
        }

        internal static void doDeferredExpressionWatcherChangedProcessings<TCanProcessSourceItemChange>(
            Queue<ExpressionWatcher.Raise> deferredExpressionWatcherChangedProcessings, ref object handledEventSender,
            ref EventArgs handledEventArgs, TCanProcessSourceItemChange canProcessSourceItemChange,
            ref bool isConsistent, bool setConsistencyRestored = true) where TCanProcessSourceItemChange : ICanProcessSourceItemChange
        {
            isConsistent = false;

            if (deferredExpressionWatcherChangedProcessings != null)
            {
                while (deferredExpressionWatcherChangedProcessings.Count > 0)
                {
                    ExpressionWatcher.Raise expressionWatcherRaise = deferredExpressionWatcherChangedProcessings.Dequeue();
                    if (!expressionWatcherRaise.ExpressionWatcher._disposed)
                    {
                        handledEventSender = expressionWatcherRaise.EventSender;
                        handledEventArgs = expressionWatcherRaise.EventArgs;

                        if (typeof(TCanProcessSourceItemChange) == typeof(ICanProcessSourceItemChange))               
                            canProcessSourceItemChange.ProcessSourceItemChange(expressionWatcherRaise.ExpressionWatcher);
                        else if (typeof(TCanProcessSourceItemChange) == typeof(ICanProcessSourceItemKeyChange))
                            ((ICanProcessSourceItemKeyChange) canProcessSourceItemChange).ProcessSourceItemChange(expressionWatcherRaise.ExpressionWatcher);
                        else if (typeof(TCanProcessSourceItemChange) == typeof(ICanProcessSourceItemValueChange))
                            ((ICanProcessSourceItemValueChange) canProcessSourceItemChange).ProcessSourceItemChange(expressionWatcherRaise.ExpressionWatcher);               
                    }
                }
            }


            if (setConsistencyRestored)
            {
                isConsistent = true;
                canProcessSourceItemChange.RaiseConsistencyRestored();
            }

        }

        internal static void endComputingExecutingUserCode(IComputing computing, Thread currentThread, ref IComputing userCodeIsCalledFrom)
        {
            if (computing == null) DebugInfo._computingsExecutingUserCode.TryRemove(currentThread, out IComputing _);
            else DebugInfo._computingsExecutingUserCode[currentThread] = computing;
            userCodeIsCalledFrom = null;
        }

        internal static Thread startComputingExecutingUserCode(out IComputing computing, ref IComputing userCodeIsCalledFrom, IComputing current)
        {
            Thread currentThread = Thread.CurrentThread;
            DebugInfo._computingsExecutingUserCode.TryGetValue(currentThread, out computing);
            DebugInfo._computingsExecutingUserCode[currentThread] = current;
            userCodeIsCalledFrom = computing;
            return currentThread;
        }

        internal static void construct<TItemInfo, TExpression>(
            Expression<TExpression> expressionToProcess,
            int capacity,
            out List<TItemInfo> itemInfos,
            out Positions<TItemInfo> sourcePositions,
            ref Expression<TExpression> expressionOriginal,
            ref Expression<TExpression> expression,
            ref bool expressionContainsParametrizedObservableComputationsCalls,
            ref ExpressionWatcher.ExpressionInfo expressionInfo,
            ref int expressionСallCount,
            ref TExpression func,
            ref List<IComputingInternal> nestedComputing) where TItemInfo : Position, new()
        {
            construct(capacity, out itemInfos, out sourcePositions);
            construct(expressionToProcess, ref expressionOriginal, ref expression, ref expressionContainsParametrizedObservableComputationsCalls, ref expressionInfo, ref expressionСallCount, ref func, ref nestedComputing);
        }

        internal static void construct<TItemInfo>(int capacity, out List<TItemInfo> itemInfos, out Positions<TItemInfo> sourcePositions)
            where TItemInfo : Position, new()
        {
            itemInfos = new List<TItemInfo>(capacity);
            sourcePositions = new Positions<TItemInfo>(itemInfos);
        }

        internal static void construct<TExpression>(
            Expression<TExpression> expressionToProcess, 
            ref Expression<TExpression> expressionOriginal,
            ref Expression<TExpression> expression, 
            ref bool expressionContainsParametrizedObservableComputationsCalls,
            ref ExpressionWatcher.ExpressionInfo expressionInfo, 
            ref int expressionСallCount, 
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
                expressionСallCount = expressionInfo._callCount;
                // ReSharper disable once PossibleNullReferenceException
                func = expression.Compile();
                nestedComputing = callToConstantConverter.NestedComputings;
            }
        }

        internal static void ProcessSourceItemChange<TSourceItem, TCanProcessSourceItemChange>(ExpressionWatcher expressionWatcher,
            object sender, EventArgs eventArgs,
            bool rootSourceWrapper,
            ObservableCollectionWithChangeMarker<TSourceItem> observableCollectionWithChangeMarker,
            bool lastProcessedSourceChangeMarker, TCanProcessSourceItemChange thisAsCanProcessSourceItemChange,
            ref Queue<ExpressionWatcher.Raise> deferredExpressionWatcherChangedProcessings,
            ref bool isConsistent, ref object handledEventSender, ref EventArgs handledEventArgs, bool consistent) where TCanProcessSourceItemChange : ICanProcessSourceItemChange
        {
            bool canProcessNow = rootSourceWrapper || observableCollectionWithChangeMarker.ChangeMarkerField == lastProcessedSourceChangeMarker;

            Utils.checkConsistent(sender, eventArgs, consistent, thisAsCanProcessSourceItemChange);

            processSourceItemChange(expressionWatcher, sender, eventArgs, thisAsCanProcessSourceItemChange, ref deferredExpressionWatcherChangedProcessings, out isConsistent, ref handledEventSender, ref handledEventArgs, canProcessNow);
        }

        internal static void ProcessSourceItemChange<TSourceItem1, TSourceItem2, TCanProcessSourceItemChange>(ExpressionWatcher expressionWatcher,
            object sender, EventArgs eventArgs,
            bool rootSourceWrapper1,
            ObservableCollectionWithChangeMarker<TSourceItem1> observableCollectionWithChangeMarker1,
            bool rootSourceWrapper2,
            ObservableCollectionWithChangeMarker<TSourceItem2> observableCollectionWithChangeMarker2,
            bool lastProcessedSource1ChangeMarker, bool lastProcessedSource2ChangeMarker, TCanProcessSourceItemChange thisAsCanProcessSourceItemChange,
            ref Queue<ExpressionWatcher.Raise> deferredExpressionWatcherChangedProcessings,
            ref bool isConsistent, ref object handledEventSender, ref EventArgs handledEventArgs, bool consistent) where TCanProcessSourceItemChange : ICanProcessSourceItemChange
        {
            bool canProcessNow = 
                (rootSourceWrapper1 || observableCollectionWithChangeMarker1.ChangeMarkerField == lastProcessedSource1ChangeMarker)
                && (rootSourceWrapper2 || observableCollectionWithChangeMarker2.ChangeMarkerField == lastProcessedSource2ChangeMarker);

            Utils.checkConsistent(sender, eventArgs, consistent, thisAsCanProcessSourceItemChange);

            processSourceItemChange(expressionWatcher, sender, eventArgs, thisAsCanProcessSourceItemChange, ref deferredExpressionWatcherChangedProcessings, out isConsistent, ref handledEventSender, ref handledEventArgs, canProcessNow);
        }

        private static void processSourceItemChange<TCanProcessSourceItemChange>(
            ExpressionWatcher expressionWatcher, object sender, EventArgs eventArgs,
            TCanProcessSourceItemChange thisAsCanProcessSourceItemChange, ref Queue<ExpressionWatcher.Raise> deferredExpressionWatcherChangedProcessings,
            out bool isConsistent, ref object handledEventSender, ref EventArgs handledEventArgs, bool canProcessNow)
            where TCanProcessSourceItemChange : ICanProcessSourceItemChange
        {
            if (canProcessNow)
            {
                handledEventSender = sender;
                handledEventArgs = eventArgs;
                isConsistent = false;

                if (typeof(TCanProcessSourceItemChange) == typeof(ICanProcessSourceItemChange))
                    thisAsCanProcessSourceItemChange.ProcessSourceItemChange(expressionWatcher);
                else if (typeof(TCanProcessSourceItemChange) == typeof(ICanProcessSourceItemKeyChange))
                    ((ICanProcessSourceItemKeyChange) thisAsCanProcessSourceItemChange).ProcessSourceItemChange(
                        expressionWatcher);
                else if (typeof(TCanProcessSourceItemChange) == typeof(ICanProcessSourceItemValueChange))
                    ((ICanProcessSourceItemValueChange) thisAsCanProcessSourceItemChange).ProcessSourceItemChange(
                        expressionWatcher);

                isConsistent = true;
                thisAsCanProcessSourceItemChange.RaiseConsistencyRestored();
                handledEventSender = null;
                handledEventArgs = null;
            }
            else
            {
                (deferredExpressionWatcherChangedProcessings =
                        deferredExpressionWatcherChangedProcessings
                        ?? new Queue<ExpressionWatcher.Raise>())
                    .Enqueue(new ExpressionWatcher.Raise(expressionWatcher, sender, eventArgs));
            }
        }

        internal static void getItemInfoContent<TExpression, TExpressionCompiled>(
            object[] sourceItems,
            out ExpressionWatcher watcher,
            out TExpressionCompiled func,
            out List<IComputingInternal> nestedComputings,
            Expression<TExpression> expression,
            ref int expressionСallCount,
            IComputingInternal current, 
            bool expressionContainsParametrizedLiveLinqCalls,
            ExpressionWatcher.ExpressionInfo orderingValueSelectorExpressionInfo)
        {
            if (!expressionContainsParametrizedLiveLinqCalls)
            {
                watcher = new ExpressionWatcher(orderingValueSelectorExpressionInfo, sourceItems);
                func = default(TExpressionCompiled);
                nestedComputings = null;
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
                for (var computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
                    nestedComputings[computingIndex].AddDownstreamConsumedComputing(current);

                ExpressionWatcher.ExpressionInfo expressionInfo =
                    ExpressionWatcher.GetExpressionInfo(predicateExpression);
                watcher = new ExpressionWatcher(expressionInfo);
                expressionСallCount = expressionInfo._callCount;
            }
        }

        internal static void itemInfoRemoveDownstreamConsumedComputing(List<IComputingInternal> nestedComputings, IComputingInternal current)
        {
            int nestedComputingsCount = nestedComputings.Count;
            for (var computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
                nestedComputings[computingIndex].RemoveDownstreamConsumedComputing(current);
        }

        internal static void construct<TOrderingValue>(int sourceCapacity, ref List<OrderedItemInfo<TOrderingValue>> orderedItemInfos, ref Positions<OrderedItemInfo<TOrderingValue>> orderedPositions, ref List<TOrderingValue> orderingValues)
        {
            orderedItemInfos = new List<OrderedItemInfo<TOrderingValue>>(sourceCapacity);
            orderedPositions = new Positions<OrderedItemInfo<TOrderingValue>>(orderedItemInfos);
            orderingValues = new List<TOrderingValue>(sourceCapacity);
        }

        internal static void AddComsumer(Consumer addingConsumer, List<Consumer> consumers, List<IComputingInternal> downstreamConsumedComputings, IComputingInternal current, ref bool isActive)
        {
            for (var index = 0; index < consumers.Count; index++)
            {
                Consumer consumer = consumers[index];
                if (ReferenceEquals(consumer, addingConsumer)) return;
            }

            consumers.Add(addingConsumer);
            addingConsumer.AddComputing(current);

            if (consumers.Count == 1)
            {
                if (downstreamConsumedComputings.Count == 0)
                {
                    isActive = true;
                    current.Initialize();
                    current.AddToUpstreamComputings(current);
                    current.InitializeFromSource();
                    current.OnPropertyChanged(Utils.IsActivePropertyChangedEventArgs);
                }
                else
                {
                    current.AddToUpstreamComputings(current);
                }
            }
        }

        internal static void RemoveConsumer(Consumer removingConsumer, List<Consumer> consumers, List<IComputingInternal> downstreamConsumedComputings, ref bool isActive, IComputingInternal current)
        {
            for (var index = 0; index < consumers.Count; index++)
            {
                Consumer consumer = consumers[index];
                if (ReferenceEquals(consumer, removingConsumer))
                {
                    consumers.RemoveAt(index);
                    break;
                }
            }

            if (consumers.Count == 0 && downstreamConsumedComputings.Count == 0)
            {
                isActive = false;
                current.InitializeFromSource();
                current.RemoveFromUpstreamComputings(current);
                current.Uninitialize();
                current.OnPropertyChanged(Utils.IsActivePropertyChangedEventArgs);    
            }
        }

        internal static void AddDownstreamConsumedComputing(IComputingInternal computing, List<IComputingInternal> downstreamConsumedComputings, List<Consumer> consumers, ref bool isActive, IComputingInternal current)
        {
            downstreamConsumedComputings.Add(computing);

            if (downstreamConsumedComputings.Count == 1 && consumers.Count == 0)
            {
                isActive = true;
                current.Initialize();
                current.AddToUpstreamComputings(computing);
                current.InitializeFromSource();
                current.OnPropertyChanged(Utils.IsActivePropertyChangedEventArgs);
            }
            else
                current.AddToUpstreamComputings(computing);
        }

        internal static void RemoveDownstreamConsumedComputing(IComputingInternal computing, List<IComputingInternal> downstreamConsumedComputings, ref bool isActive, IComputingInternal current, List<Consumer> consumers)
        {
            downstreamConsumedComputings.Remove(computing);

            if (consumers.Count == 0 && downstreamConsumedComputings.Count == 0)
            {
                isActive = true;
                current.InitializeFromSource();
                current.RemoveFromUpstreamComputings(computing);
                current.Uninitialize();
                current.OnPropertyChanged(Utils.IsActivePropertyChangedEventArgs);
            }
            else
            {
                current.RemoveFromUpstreamComputings(computing);               
            }
        }

        internal static void initializeSourceIndexerPropertyTracker<TSourceList>(
            ref INotifyPropertyChanged sourceAsINotifyPropertyChanged, 
            ISourceIndexerPropertyTracker current,
            TSourceList sourceAsList)
        {
            sourceAsINotifyPropertyChanged = (INotifyPropertyChanged) sourceAsList;
            sourceAsINotifyPropertyChanged.PropertyChanged += current.HandleSourcePropertyChanged;
        }

        internal static void initializeFromHasChangeMarker<TSourceList>(
            ref IHasChangeMarker sourceAsIHasChangeMarker, 
            TSourceList sourceAsList, 
            ref bool lastProcessedSourceChangeMarker,
            ref INotifyPropertyChanged sourceAsINotifyPropertyChanged, 
            ISourceIndexerPropertyTracker current)
        {
            sourceAsIHasChangeMarker = sourceAsList as IHasChangeMarker;

            if (sourceAsIHasChangeMarker != null)
            {
                lastProcessedSourceChangeMarker = sourceAsIHasChangeMarker.ChangeMarker;
            }
            else
            {
                Utils.initializeSourceIndexerPropertyTracker(ref sourceAsINotifyPropertyChanged,
                    current, sourceAsList);
            }
        }

        internal static void HandleSourcePropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs, ref bool indexerPropertyChangedEventRaised)
        {
            if (propertyChangedEventArgs.PropertyName == "Item[]")
                indexerPropertyChangedEventRaised = true;
        }

        internal static bool preHandleSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e, bool isConsistent, IComputing sourceItems, ref bool indexerPropertyChangedEventRaised, ref bool lastProcessedSourceChangeMarker, IHasChangeMarker sourceAsIHasChangeMarker, ref object handledEventSender, ref EventArgs handledEventArgs)
        {
            Utils.checkConsistent(sender, e, isConsistent, sourceItems);

            if (!indexerPropertyChangedEventRaised &&
                lastProcessedSourceChangeMarker == sourceAsIHasChangeMarker.ChangeMarker)
                return false;

            handledEventSender = sender;
            handledEventArgs = e;

            lastProcessedSourceChangeMarker = !lastProcessedSourceChangeMarker;
            indexerPropertyChangedEventRaised = false;
            return true;
        }

		internal static readonly PropertyChangedEventArgs InsertItemIntoGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("InsertItemIntoGroupAction");
		internal static readonly PropertyChangedEventArgs RemoveItemFromGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemFromGroupAction");
		internal static readonly PropertyChangedEventArgs SetGroupItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("SetGroupItemAction");
		internal static readonly PropertyChangedEventArgs MoveItemInGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("MoveItemInGroupAction");
		internal static readonly PropertyChangedEventArgs ClearGroupItemsActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ClearGroupItemsAction");
		internal static readonly PropertyChangedEventArgs OuterItemPropertyChangedEventArgs = new PropertyChangedEventArgs("OuterItem");
		internal static readonly PropertyChangedEventArgs KeyPropertyChangedEventArgs = new PropertyChangedEventArgs("Key");
		internal static readonly PropertyChangedEventArgs SetLeftItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("SetLeftItemRequestHandler");
		internal static readonly PropertyChangedEventArgs SetRightItemRequestHandlerPropertyChangedEventArgs = new PropertyChangedEventArgs("SetRightItemRequestHandler");
		internal static readonly PropertyChangedEventArgs ZipPairSetLeftItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ZipPairSetLeftItemAction");
		internal static readonly PropertyChangedEventArgs ZipPairSetRightItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ZipPairSetRightItemAction");
		internal static readonly PropertyChangedEventArgs LeftItemPropertyChangedEventArgs = new PropertyChangedEventArgs("LeftItem");
		internal static readonly PropertyChangedEventArgs RightItemPropertyChangedEventArgs = new PropertyChangedEventArgs("RightItem");
		internal static readonly PropertyChangedEventArgs InsertItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("InsertItemAction");
		internal static readonly PropertyChangedEventArgs AddItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("AddItemAction");
		internal static readonly PropertyChangedEventArgs RemoveItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemAction");
		internal static readonly PropertyChangedEventArgs RemoveItemFuncPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemFunc");
		internal static readonly PropertyChangedEventArgs SetItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("SetItemAction");
		internal static readonly PropertyChangedEventArgs MoveItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("MoveItemAction");
		internal static readonly PropertyChangedEventArgs ClearItemsActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ClearItemsAction");
		internal static readonly PropertyChangedEventArgs ValuePropertyChangedEventArgs = new PropertyChangedEventArgs("Value");
		internal static readonly PropertyChangedEventArgs ResultPropertyChangedEventArgs = new PropertyChangedEventArgs("Result");
		internal static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs("Count");
		internal static readonly PropertyChangedEventArgs IndexerPropertyChangedEventArgs = new PropertyChangedEventArgs("Item[]");
		internal static readonly PropertyChangedEventArgs ValueObjectPropertyChangedEventArgs = new PropertyChangedEventArgs("ValueObject");
		internal static readonly PropertyChangedEventArgs SetValueActionPropertyChangedEventArgs = new PropertyChangedEventArgs("SetValueAction");
		internal static readonly PropertyChangedEventArgs IsDisabledPropertyChangedEventArgs = new PropertyChangedEventArgs("IsDisabled");
		internal static readonly PropertyChangedEventArgs PreviousValuePropertyChangedEventArgs = new PropertyChangedEventArgs("PreviousValue");
		internal static readonly PropertyChangedEventArgs IsEverChangedPropertyChangedEventArgs = new PropertyChangedEventArgs("IsEverChanged");
		internal static readonly PropertyChangedEventArgs CurrentPagePropertyChangedEventArgs = new PropertyChangedEventArgs("CurrentPage");
		internal static readonly PropertyChangedEventArgs PageCountPropertyChangedEventArgs = new PropertyChangedEventArgs("PageCount");
		internal static readonly PropertyChangedEventArgs PageSizePropertyChangedEventArgs = new PropertyChangedEventArgs("PageSize");
		internal static readonly PropertyChangedEventArgs PausedPropertyChangedEventArgs = new PropertyChangedEventArgs("Paused");
		internal static readonly NotifyCollectionChangedEventArgs ResetNotifyCollectionChangedEventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
        internal static PropertyChangedEventArgs IsActivePropertyChangedEventArgs = new PropertyChangedEventArgs("IsActive");
    }
}
