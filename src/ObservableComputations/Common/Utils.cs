using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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
            ref List<TItemInfo> itemInfos, 
            int expressionСallCount, 
            IComputingInternal computing)
            where TItemInfo : ExpressionItemInfo
        {
            int itemInfosCount = itemInfos.Count;

            int callCount = expressionСallCount * itemInfosCount;

            PropertyChangedEventSubscription[] propertyChangedEventSubscriptions =
                new PropertyChangedEventSubscription[callCount];

            MethodChangedEventSubscription[] methodChangedEventSubscriptions =
                new MethodChangedEventSubscription[callCount];

            int callIndexTotal = 0;
            int callIndex;
            for (var index = 0; index < itemInfosCount; index++)
            {
                ExpressionItemInfo itemInfo = itemInfos[index];
                ExpressionWatcher expressionWatcher = itemInfo.ExpressionWatcher;
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

                List<IComputingInternal> nestedComputings = itemInfo.NestedComputings;

                if (nestedComputings != null)
                {
                    int nestedComputingsCount = nestedComputings.Count;
                    for (var computingIndex = 0; computingIndex < nestedComputingsCount; computingIndex++)
                        nestedComputings[computingIndex].RemoveDownstreamConsumedComputing(computing);
                }
            }

            EventUnsubscriber.QueueSubscriptions(propertyChangedEventSubscriptions, methodChangedEventSubscriptions);
        }

        internal static void changeSource(
            ref INotifyCollectionChanged source, 
            IReadScalar<INotifyCollectionChanged> sourceScalar, 
            List<IComputingInternal> downstreamConsumedComputings, 
            List<Consumer> consumers, 
            IComputingInternal computing)
        {
            IComputingInternal originalSource = source as IComputingInternal;
            if (sourceScalar != null) source = sourceScalar.Value;
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
        }

        internal static void initializeFromObservableCollectionWithChangeMarker<TSourceItem>(INotifyCollectionChanged source, ref ObservableCollectionWithChangeMarker<TSourceItem> sourceAsList, ref bool rootSourceWrapper, ref bool lastProcessedSourceChangeMarker)
        {
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

        internal static void disposeSource<TItemInfo>(int capacity, ref List<TItemInfo> itemInfos, ref Positions<TItemInfo> sourcePositions,INotifyCollectionChanged sourceAsList, ref NotifyCollectionChangedEventHandler sourceNotifyCollectionChangedEventHandler)
            where TItemInfo : Position, new()
        {
            itemInfos = new List<TItemInfo>(capacity);
            sourcePositions = new Positions<TItemInfo>(itemInfos);

            sourceAsList.CollectionChanged -= sourceNotifyCollectionChangedEventHandler;
            sourceNotifyCollectionChangedEventHandler = null;
        }

        internal static void initializeSourceScalar(IReadScalar<INotifyCollectionChanged> sourceScalar, ref PropertyChangedEventHandler sourceScalarOnPropertyChanged, ref INotifyCollectionChanged source, PropertyChangedEventHandler newSourceScalarOnPropertyChanged)
        {
            if (sourceScalar != null)
            {
                sourceScalarOnPropertyChanged = newSourceScalarOnPropertyChanged;
                sourceScalar.PropertyChanged += sourceScalarOnPropertyChanged;
                source = sourceScalar.Value;
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

        internal static void uninitializeSourceScalar(IReadScalar<INotifyCollectionChanged> sourceScalar, PropertyChangedEventHandler sourceScalarOnPropertyChanged)
        {
            if (sourceScalar != null)
            {
                sourceScalar.PropertyChanged -= sourceScalarOnPropertyChanged;
            }
        }

        internal static  void checkConsistent(object sender, EventArgs eventArgs, bool isConsistent, IComputing computing)
        {
            if (!isConsistent)
                throw new ObservableComputationsInconsistencyException(computing,
                    $"The source collection has been changed. It is not possible to process this change (event sender = {sender.ToStringSafe(e => $"{e.ToString()} in sender.ToString()")}, event args = {eventArgs.ToStringAlt()}), as the processing of the previous change is not completed. Make the change on ConsistencyRestored event raising (after IsConsistent property becomes true). This exception is fatal and cannot be handled as the inner state is damaged.", sender, eventArgs);
        }

        internal static bool preHandleSourceCollectionChanged<TSourceItem>(object sender, NotifyCollectionChangedEventArgs e, bool rootSourceWrapper, ref bool lastProcessedSourceChangeMarker, ObservableCollectionWithChangeMarker<TSourceItem> sourceAsList, bool isConsistent, IComputing computing)
        {
            Utils.checkConsistent(sender, e, isConsistent, computing);
            if (!rootSourceWrapper && lastProcessedSourceChangeMarker == sourceAsList.ChangeMarkerField) return false;

            lastProcessedSourceChangeMarker = !lastProcessedSourceChangeMarker;
            return true;
        }

        internal static void doDeferredExpressionWatcherChangedProcessings(Queue<ExpressionWatcher.Raise> deferredExpressionWatcherChangedProcessings, ref object handledEventSender, ref EventArgs handledEventArgs, Action<ExpressionWatcher.Raise> processExpressionWatcherRaise)
        {
            if (deferredExpressionWatcherChangedProcessings != null)
                while (deferredExpressionWatcherChangedProcessings.Count > 0)
                {
                    ExpressionWatcher.Raise expressionWatcherRaise = deferredExpressionWatcherChangedProcessings.Dequeue();
                    if (!expressionWatcherRaise.ExpressionWatcher._disposed)
                    {
                        handledEventSender = expressionWatcherRaise.EventSender;
                        handledEventArgs = expressionWatcherRaise.EventArgs;
                        processExpressionWatcherRaise(expressionWatcherRaise);
                    }
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
            Expression<TExpression> selectorExpressionToProcess, 
            int capacity, 
            ref List<TItemInfo> itemInfos, 
            ref Positions<TItemInfo> sourcePositions, 
            ref Expression<TExpression> selectorExpressionOriginal, 
            ref Expression<TExpression> selectorExpression, 
            ref bool selectorContainsParametrizedObservableComputationsCalls, 
            ref ExpressionWatcher.ExpressionInfo selectorExpressionInfo, 
            ref int selectorExpressionСallCount, 
            ref TExpression selectorFunc, 
            ref List<IComputingInternal> nestedComputing) where TItemInfo : Position, new()
        {
            itemInfos = new List<TItemInfo>(capacity);
            sourcePositions = new Positions<TItemInfo>(itemInfos);

            selectorExpressionOriginal = selectorExpressionToProcess;
            CallToConstantConverter callToConstantConverter =
                new CallToConstantConverter(selectorExpressionOriginal.Parameters);
            selectorExpression =
                (Expression<TExpression>) callToConstantConverter.Visit(
                    selectorExpressionOriginal);
            selectorContainsParametrizedObservableComputationsCalls =
                callToConstantConverter.ContainsParametrizedObservableComputationCalls;

            if (!selectorContainsParametrizedObservableComputationsCalls)
            {
                selectorExpressionInfo = ExpressionWatcher.GetExpressionInfo(selectorExpression);
                selectorExpressionСallCount = selectorExpressionInfo._callCount;
                // ReSharper disable once PossibleNullReferenceException
                selectorFunc = selectorExpression.Compile();
                nestedComputing = callToConstantConverter.NestedComputings;
            }
        }

		internal static readonly PropertyChangedEventArgs InsertItemIntoGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("InsertItemIntoGroupAction");
		internal static readonly PropertyChangedEventArgs RemoveItemFromGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("RemoveItemFromGroupAction");
		internal static readonly PropertyChangedEventArgs SetGroupItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("SetGroupItemAction");
		internal static readonly PropertyChangedEventArgs MoveItemInGroupActionPropertyChangedEventArgs = new PropertyChangedEventArgs("MoveItemInGroupAction");
		internal static readonly PropertyChangedEventArgs ClearGroupItemsActionPropertyChangedEventArgs = new PropertyChangedEventArgs("ClearGroupItemsAction");
		internal static readonly PropertyChangedEventArgs OuterItemPropertyChangedEventArgs = new PropertyChangedEventArgs("OuterItem");
		internal static readonly PropertyChangedEventArgs KeyPropertyChangedEventArgs = new PropertyChangedEventArgs("Key");
		internal static readonly PropertyChangedEventArgs JoinPairSetOuterItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("JoinPairSetOuterItemAction");
		internal static readonly PropertyChangedEventArgs JoinPairSetInnerItemActionPropertyChangedEventArgs = new PropertyChangedEventArgs("JoinPairSetInnerItemAction");
		internal static readonly PropertyChangedEventArgs InnerItemPropertyChangedEventArgs = new PropertyChangedEventArgs("InnerItem");
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
        public static PropertyChangedEventArgs IsActivePropertyChangedEventArgs = new PropertyChangedEventArgs("IsActive");
    }
}
