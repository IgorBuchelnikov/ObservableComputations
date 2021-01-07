// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ObservableComputations
{
	internal sealed class ExpressionWatcher
	{
		internal Position _position;
		internal readonly PropertyChangedEventSubscription[] _propertyChangedEventSubscriptions;
		internal readonly MethodChangedEventSubscription[] _methodChangedEventSubscriptions;
		internal readonly IComputingInternal[] _oldComputings;
		internal readonly IComputingInternal[] _currentComputings;
		internal bool _computingsChanged;
		private readonly ExpressionWatcher _rootExpressionWatcher;

		internal class Raise : IProcessable
		{
			private readonly ExpressionWatcher ExpressionWatcher;
			private readonly object _eventSender;
			private readonly EventArgs _eventArgs;
			private readonly ISourceItemChangeProcessor SourceItemChangeProcessor;
			private readonly Type SourceItemChangeProcessorType;

			public Raise(ExpressionWatcher expressionWatcher, object eventSender, EventArgs eventArgs, ISourceItemChangeProcessor sourceItemChangeProcessor, Type sourceItemChangeProcessorType)
			{
				ExpressionWatcher = expressionWatcher;
				_eventSender = eventSender;
				_eventArgs = eventArgs;
				SourceItemChangeProcessor = sourceItemChangeProcessor;
				SourceItemChangeProcessorType = sourceItemChangeProcessorType;
			}

			#region Implementation of IProcessable

			public void Process(Queue<IProcessable>[] deferredProcessings)
			{
				if (SourceItemChangeProcessorType == typeof(ISourceItemChangeProcessor))
					SourceItemChangeProcessor.ProcessSourceItemChange(ExpressionWatcher);
				else if (SourceItemChangeProcessorType == typeof(ISourceItemKeyChangeProcessor))
					((ISourceItemKeyChangeProcessor) SourceItemChangeProcessor).ProcessSourceItemChange(
						ExpressionWatcher);
				else if (SourceItemChangeProcessorType == typeof(ISourceItemValueChangeProcessor))
					((ISourceItemValueChangeProcessor) SourceItemChangeProcessor).ProcessSourceItemChange(
						ExpressionWatcher);
			}

			public object EventSender => _eventSender;
			public EventArgs EventArgs => _eventArgs;

			#endregion
		}

		public struct ExpressionInfo
		{
			internal readonly LambdaExpression _expressionToWatch;
			internal readonly ConstantCallTreesInfo[] _constantCallTrees;
			internal readonly ParameterCallTreesInfo[] _parameterCallTrees;
			internal readonly ExpressionCallTreesInfo[] _expressionCallTrees;
			internal int _callCount;
#if DEBUG
			// ReSharper disable once MemberCanBePrivate.Global
			internal List<ConstantCallPaths> _constantCallPaths;
			// ReSharper disable once MemberCanBePrivate.Global
			internal List<ParameterCallPaths> _parameterCallPaths;
#endif


#if DEBUG
			internal ExpressionInfo(
				LambdaExpression expressionToWatch, 
				ConstantCallTreesInfo[] constantCallTreesInfo,
				List<ConstantCallPaths> constantCallPaths,
				ParameterCallTreesInfo[] parameterCallTreesInfo, 
				List<ParameterCallPaths> parameterCallPaths, 
				ExpressionCallTreesInfo[] expressionCallTreesInfo) : this()
			{
				_expressionToWatch = expressionToWatch;
				_constantCallTrees = constantCallTreesInfo;
				_constantCallPaths = constantCallPaths;
				_parameterCallTrees = parameterCallTreesInfo;
				_parameterCallPaths = parameterCallPaths;
				_expressionCallTrees = expressionCallTreesInfo;
			}
#else
			internal ExpressionInfo(
				LambdaExpression expressionToWatch, 
				ConstantCallTreesInfo[] constantCallTreesInfo,
				ParameterCallTreesInfo[] parameterCallTreesInfo,  
				ExpressionCallTreesInfo[] expressionCallTreesInfo) : this()
			{
				_expressionToWatch = expressionToWatch;
				_constantCallTrees = constantCallTreesInfo;
				_parameterCallTrees = parameterCallTreesInfo;
				_expressionCallTrees = expressionCallTreesInfo;
			}
#endif
		}



#if DEBUG
		public enum RootType
		{
			Constant,
			Parameter,
			Expression
		}

		public abstract class Root
		{
			protected RootType _type;

			// ReSharper disable once UnusedAutoPropertyAccessor.Global
			public RootType Type
			{
				get => _type;
				protected set => _type = value;
			}

			public object Value { get; }

			protected Root(object value)
			{
				Value = value;
			}
		}
 
		// ReSharper disable once MemberCanBePrivate.Global
		public class RootConstant : Root
		{
			public RootConstant(object value) : base(value)
			{
				_type = RootType.Constant;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public class RootParameter : Root
		{
			// ReSharper disable once UnusedAutoPropertyAccessor.Global
			// ReSharper disable once MemberCanBePrivate.Global
			public int Index { get; }

			public RootParameter(object value, int index)  : base(value)
			{
				Index = index;
				_type = RootType.Parameter;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public class  RootExpression : Root
		{
			// ReSharper disable once UnusedAutoPropertyAccessor.Global
			// ReSharper disable once MemberCanBePrivate.Global
			public LambdaExpression ExpressionToWatch { get; }

			public RootExpression(object value, LambdaExpression expressionToWatch) : base(value)
			{
				ExpressionToWatch = expressionToWatch;
				_type = RootType.Expression;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public Root LastChangedRoot;

#endif

		public Action<ExpressionWatcher, object, EventArgs> ValueChanged;
		//public event EventHandler<EventArgs> ValueChanged;

		public struct ConstantCallTreesInfo
		{
			public object ConstantValue;
			public readonly List<CallTreeNodeInfo> CallTrees;

			public ConstantCallTreesInfo(object constantValue, List<CallTreeNodeInfo> callTrees)
			{
				ConstantValue = constantValue;
				CallTrees = callTrees;
			}
		}

		public struct ParameterCallTreesInfo
		{
			public readonly int ParameterIndex;
			public readonly List<CallTreeNodeInfo> CallTrees;

			public ParameterCallTreesInfo(int parameterIndex, List<CallTreeNodeInfo> callTrees)
			{
				ParameterIndex = parameterIndex;
				CallTrees = callTrees;
			}
		}

		public struct ExpressionCallTreesInfo
		{
			public ExpressionInfo ExpressionInfo;
			public readonly List<CallTreeNodeInfo> CallTrees;

			public ExpressionCallTreesInfo(ExpressionInfo expressionInfo, List<CallTreeNodeInfo> callTrees)
			{
				ExpressionInfo = expressionInfo;
				CallTrees = callTrees;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public struct ConstantCallTrees
		{
			public readonly object ConstantValue;
			public readonly CallTreeNode[] CallTrees;

			public ConstantCallTrees(object constantValue, CallTreeNode[] callTrees)
			{
				ConstantValue = constantValue;
				CallTrees = callTrees;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public struct ParameterCallTrees
		{
			public readonly int ParameterIndex;
			public readonly CallTreeNode[] CallTrees;

			public ParameterCallTrees(int parameterIndex, CallTreeNode[] callTrees)
			{
				ParameterIndex = parameterIndex;
				CallTrees = callTrees;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public struct ExpressionCallTrees
		{
			public readonly ExpressionWatcher ExpressionWatcher;
			public readonly CallTreeNode[] CallTrees;

			public ExpressionCallTrees(ExpressionWatcher expressionWatcher, CallTreeNode[] callTrees) : this()
			{
				ExpressionWatcher = expressionWatcher;
				CallTrees = callTrees;
			}
		}

		public static ExpressionInfo GetExpressionInfo(LambdaExpression expression)
		{
			int callIndex = 0;
			ExpressionInfo expressionInfo = GetExpressionInfo(expression, ref callIndex);
			expressionInfo._callCount = callIndex + 1;
			return expressionInfo;
		}


		public static ExpressionInfo GetExpressionInfo(LambdaExpression expression, ref int callIndex)
		{
			CallPathsConstructor callPathsConstructor =  new CallPathsConstructor(expression);
			callPathsConstructor.Visit(expression);

			int count = callPathsConstructor.ConstantCallPaths.Count;
			ConstantCallTreesInfo[] constantsCallTreesInfo = null;
			if (count > 0)
			{
				constantsCallTreesInfo = new ConstantCallTreesInfo[count];
				for (int index = 0; index < count; index++)
				{
					ConstantCallPaths constantCallPaths = callPathsConstructor.ConstantCallPaths[index];
					constantsCallTreesInfo[index] = new ConstantCallTreesInfo(constantCallPaths.ConstantValue, getCallTrees(constantCallPaths.CallPaths, ref callIndex));
				}
			}

			count = callPathsConstructor.ParameterCallPaths.Count;
			ParameterCallTreesInfo[] parameterCallTreesInfo = null;
			if (count > 0)
			{
				parameterCallTreesInfo = new ParameterCallTreesInfo[count];
				for (int index = 0; index < count; index++)
				{
					ParameterCallPaths parameterCallPaths = callPathsConstructor.ParameterCallPaths[index];
					parameterCallTreesInfo[index] = new ParameterCallTreesInfo(parameterCallPaths.ParameterIndex, getCallTrees(parameterCallPaths.CallPaths, ref callIndex));
				}
			}

			count = callPathsConstructor.ExpressionCallPaths.Count;
			ExpressionCallTreesInfo[] expressionCallTreesInfo = null;
			if (count > 0)
			{
				expressionCallTreesInfo = new ExpressionCallTreesInfo[count];
				for (int index = 0; index < count; index++)
				{
					ExpressionCallPaths expressionCallPaths = callPathsConstructor.ExpressionCallPaths[index];
					expressionCallTreesInfo[index] = new ExpressionCallTreesInfo(GetExpressionInfo(expressionCallPaths.Expression, ref callIndex), getCallTrees(expressionCallPaths.CallPaths, ref callIndex));
				}
			}


#if DEBUG
			return new ExpressionInfo(expression, constantsCallTreesInfo, callPathsConstructor.ConstantCallPaths, parameterCallTreesInfo, callPathsConstructor.ParameterCallPaths, expressionCallTreesInfo);
#else
			return new ExpressionInfo(expression, constantsCallTreesInfo, parameterCallTreesInfo, expressionCallTreesInfo);
#endif
		}

		private void initialize(ExpressionInfo expressionInfo)
		{
			workWithCallTrees(expressionInfo._parameterCallTrees, expressionInfo._expressionCallTrees, _parameterValues);
			if (expressionInfo._constantCallTrees != null) workWithCallTrees(expressionInfo._constantCallTrees);
		}

		internal ExpressionWatcher(ExpressionInfo expressionInfo)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = null;
			_propertyChangedEventSubscriptions = new PropertyChangedEventSubscription[expressionInfo._callCount];
			_methodChangedEventSubscriptions = new MethodChangedEventSubscription[expressionInfo._callCount];
			_oldComputings = new IComputingInternal[expressionInfo._callCount];
			_currentComputings = new IComputingInternal[expressionInfo._callCount];
			_rootExpressionWatcher = this;
			initialize(expressionInfo);
		}

		// ReSharper disable once MemberCanBePrivate.Global
		private ExpressionWatcher(object[] parameterValues, ExpressionInfo expressionInfo,
			PropertyChangedEventSubscription[] propertyChangedEventSubscriptions,
			MethodChangedEventSubscription[] methodChangedEventSubscriptions, 
			IComputingInternal[] oldComputings,
			IComputingInternal[] currentComputings,
			ExpressionWatcher rootExpressionWatcher)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = parameterValues;
			_propertyChangedEventSubscriptions = propertyChangedEventSubscriptions;
			_methodChangedEventSubscriptions = methodChangedEventSubscriptions;
			_oldComputings = oldComputings;
			_currentComputings = currentComputings;
			_rootExpressionWatcher = rootExpressionWatcher;
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object[] parameters)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = parameters;
			_propertyChangedEventSubscriptions = new PropertyChangedEventSubscription[expressionInfo._callCount];
			_methodChangedEventSubscriptions = new MethodChangedEventSubscription[expressionInfo._callCount];
			_oldComputings = new IComputingInternal[expressionInfo._callCount];
			_currentComputings = new IComputingInternal[expressionInfo._callCount];
			_rootExpressionWatcher = this;
			initialize(expressionInfo);
		}

		private void workWithCallTrees(ParameterCallTreesInfo[] parameterCallTreesInfo,  ExpressionCallTreesInfo[] expressionCallTreesInfo, object[] parameterValues)
		{
			int length;
			if (parameterCallTreesInfo != null)
			{
				length = parameterCallTreesInfo.Length;
				_parameterCallTrees = new ParameterCallTrees[length];
				for (int index = 0; index < length; index++)
				{
					ParameterCallTreesInfo parameterCallTreesInfoItem = parameterCallTreesInfo[index];
					int callTreesCount = parameterCallTreesInfoItem.CallTrees.Count;
					CallTreeNode[] callTrees = new CallTreeNode[callTreesCount];
					for (int i = 0; i < callTreesCount; i++)
					{
						callTrees[i] = parameterCallTreesInfoItem.CallTrees[i].getCallTreeNode(
							_parameterValues, 
							_propertyChangedEventSubscriptions, 
							_methodChangedEventSubscriptions,
							_oldComputings, 
							_currentComputings,
							_rootExpressionWatcher);
					}

					_parameterCallTrees[index] = new ParameterCallTrees(parameterCallTreesInfoItem.ParameterIndex, callTrees);
				}

				length = _parameterCallTrees.Length;
				for (int index= 0; index < length; index++)
				{
					ParameterCallTrees parameterCallTree = _parameterCallTrees[index];
					object parameterValue = parameterValues[parameterCallTree.ParameterIndex];
#if DEBUG

					workWithCallTrees(parameterCallTree.CallTrees,
						new RootParameter(parameterValue, parameterCallTree.ParameterIndex),
						WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
#else
					workWithCallTrees(parameterCallTree.CallTrees, parameterValue, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
#endif
				}
			}


			if (expressionCallTreesInfo == null) return;
			
			length = expressionCallTreesInfo.Length;
			_expressionCallTrees = new ExpressionCallTrees[length];
			for (int index = 0; index < length; index++)
			{
				ExpressionCallTreesInfo expressionCallTreesInfoItem = expressionCallTreesInfo[index];
				ExpressionWatcher expressionWatcher = new ExpressionWatcher(
					_parameterValues, 
					expressionCallTreesInfoItem.ExpressionInfo, 
					_propertyChangedEventSubscriptions, 
					_methodChangedEventSubscriptions, 
					_oldComputings, 
					_currentComputings,
					_rootExpressionWatcher);
				expressionWatcher.compileExpressionToWatch();

				int callTreesCount = expressionCallTreesInfoItem.CallTrees.Count;
				CallTreeNode[] callTrees = new CallTreeNode[callTreesCount];
				for (int i = 0; i < callTreesCount; i++)
				{
					callTrees[i] = expressionCallTreesInfoItem.CallTrees[i].getCallTreeNode(
						_parameterValues, 
						_propertyChangedEventSubscriptions, 
						_methodChangedEventSubscriptions, 
						_oldComputings, 
						_currentComputings,
						_rootExpressionWatcher);
				}

				_expressionCallTrees[index] = new ExpressionCallTrees(expressionWatcher, callTrees);

			}

			foreach (ExpressionCallTrees expressionCallTrees in _expressionCallTrees)
			{
#if DEBUG
				Root getRoot()
#else
				object getRoot()
#endif
				{
					object value = expressionCallTrees.ExpressionWatcher._expressionToWatchCompiled.DynamicInvoke(parameterValues);

#if DEBUG
					return new RootExpression(value, expressionCallTrees.ExpressionWatcher.ExpressionToWatch);
#else
					return value;
#endif
				}
#if DEBUG
				Root root = getRoot();
				workWithCallTrees(expressionCallTrees.CallTrees, root, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
#else
				workWithCallTrees(expressionCallTrees.CallTrees, getRoot(), WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
#endif


				expressionCallTrees.ExpressionWatcher.ValueChanged =
					(ew, sender, eventArgs) =>
					{
#if DEBUG
						Root root1 = getRoot();
						workWithCallTrees(expressionCallTrees.CallTrees, root1, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
						raiseValueChanged(LastChangedRoot, sender, eventArgs);
#else
						workWithCallTrees(expressionCallTrees.CallTrees, getRoot(), WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
						raiseValueChanged(sender, eventArgs);
#endif
					};
			}
			
		}

		private void workWithCallTrees(ConstantCallTreesInfo[] constantCallTreesInfo)
		{
			int length = constantCallTreesInfo.Length;
			_constantCallTrees = new ConstantCallTrees[length];
			for (int index = 0; index < length; index++)
			{
				ConstantCallTreesInfo constantCallTreesInfoItem = constantCallTreesInfo[index];
				int callTreesCount = constantCallTreesInfoItem.CallTrees.Count;
				CallTreeNode[] callTrees = new CallTreeNode[callTreesCount];
				for (int i = 0; i < callTreesCount; i++)
				{
					callTrees[i] = constantCallTreesInfoItem.CallTrees[i].getCallTreeNode(
						_parameterValues, 
						_propertyChangedEventSubscriptions, 
						_methodChangedEventSubscriptions, 
						_oldComputings, 
						_currentComputings,
						_rootExpressionWatcher);
				}

				_constantCallTrees[index] = new ConstantCallTrees(constantCallTreesInfoItem.ConstantValue, callTrees);
			}

			length = _constantCallTrees.Length;
			for (int index = 0; index < length; index++)
			{
				ConstantCallTrees constantCallTree = _constantCallTrees[index];
#if DEBUG
				workWithCallTrees(constantCallTree.CallTrees, new RootConstant(constantCallTree.ConstantValue),
					WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
#else
				workWithCallTrees(constantCallTree.CallTrees, constantCallTree.ConstantValue, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
#endif
			}
		}

		private readonly object[] _parameterValues;
		public IEnumerable<object> ParameterValues => _parameterValues;

		// ReSharper disable once MemberCanBePrivate.Global
		public readonly LambdaExpression ExpressionToWatch;

		private Delegate _expressionToWatchCompiled;

		private void compileExpressionToWatch()
		{
			_expressionToWatchCompiled = ExpressionToWatch.Compile();
		}

		private ConstantCallTrees[] _constantCallTrees;
		private ParameterCallTrees[]_parameterCallTrees;
		private ExpressionCallTrees[] _expressionCallTrees;

		internal bool _disposed;

		private static List<CallTreeNodeInfo> getCallTrees(List<List<Call>> callPaths, ref int callIndex)
		{
			List<CallTreeNodeInfo> resultCallTrees = new List<CallTreeNodeInfo>(8);
			int callPathsCount = callPaths.Count;
			int resultCallTreesCount = 0;
			
			for (int index = 0; index < callPathsCount; index++)
			{
				List<Call> callPath = callPaths[index];
				CallTreeNodeInfo? currentResultPpa = null;
				int count = callPath.Count;
				for (int pathElementIndex = 0; pathElementIndex < count; pathElementIndex++)
				{
					Call currentCall = callPath[pathElementIndex];
					if (pathElementIndex == 0)
					{
						CallTreeNodeInfo? rootResultPpa = null;
						for (int i = 0; i < resultCallTreesCount; i++)
						{
							CallTreeNodeInfo callTreeNode = resultCallTrees[i];
							Call call = callTreeNode._call;
							if (call.Name == currentCall.Name
								&& (call.Type != CallType.Method || call.Arguments.Count == 0)) //Если метод с аргументами, то считаем, что по любому это разные вызовы. Не учитываем, что метод мог быть вызван с одинаковыми аргуметами
							{
								rootResultPpa = callTreeNode;
								break;
							}
						}

						if (rootResultPpa == null)
						{
							rootResultPpa = covertPathToTree(callPath, 0, ref callIndex);
							resultCallTrees.Add(rootResultPpa.Value);
							resultCallTreesCount++;
						}

						currentResultPpa = rootResultPpa;
					}
					else
					{
						CallTreeNodeInfo? newCurrentResultPpa = null;

						// ReSharper disable once PossibleInvalidOperationException
						CallTreeNodeInfo callTreeNodeInfo = currentResultPpa.Value;
						int childrenCount = callTreeNodeInfo._children.Count;
						for (int i = 0; i < childrenCount; i++)
						{
							CallTreeNodeInfo callTreeNode = callTreeNodeInfo._children[i];
							if (callTreeNode._call.Name == currentCall.Name)
							{
								newCurrentResultPpa = callTreeNode;
								break;
							}
						}

						if (newCurrentResultPpa == null)
						{
							newCurrentResultPpa = covertPathToTree(callPath, pathElementIndex, ref callIndex);
							callTreeNodeInfo._children.Add(newCurrentResultPpa.Value);
						}

						currentResultPpa = newCurrentResultPpa;
					}
				}
			}

			return resultCallTrees;
		}


		private static CallTreeNodeInfo covertPathToTree(List<Call> callPath, int startIndex, ref int callIndex)
		{
			CallTreeNodeInfo result = new CallTreeNodeInfo(callPath[startIndex], new List<CallTreeNodeInfo>(8), ref callIndex);
			CallTreeNodeInfo current = result;

			int count = callPath.Count;
			for (int index = startIndex + 1; index < count; index++)
			{
				CallTreeNodeInfo callPathElement = new CallTreeNodeInfo(callPath[index], new List<CallTreeNodeInfo>(8), ref callIndex);
				current._children.Add(callPathElement);
				current = callPathElement;
			}

			return result;
		}

#if DEBUG
		private void workWithCallTrees(CallTreeNode[] trees, Root root, WorkWithCallTreeNodeType workType)
		{
			int treesCount = trees.Length;
			for (int index = 0; index < treesCount; index++)
				workWithCallTreeNode(trees[index], root?.Value, root, workType);
		}
#else
		private void workWithCallTrees(CallTreeNode[] trees, object root, WorkWithCallTreeNodeType workType)
		{
			int treesCount = trees.Length;
			for (int index = 0; index < treesCount; index++)
				workWithCallTreeNode(trees[index], root, workType);	
		}
#endif


		private enum WorkWithCallTreeNodeType
		{
			UpdateSubscriptionAndHolder,
			Dispose
		}

		private Queue<ProcessChangeTask> _processChangeTasksQueue;
		private bool _processingChange;
		private struct ProcessChangeTask
		{
			public ProcessChangeTask(
#if DEBUG
				Root root,
#endif			   
				object sender, 
				EventArgs args, 
				CallTreeNode node)
			{
#if DEBUG
				Root = root;
#endif
				Sender = sender;
				Args = args;
				Node = node;
			}
#if DEBUG
			internal readonly Root Root;
#endif
			internal readonly object Sender;
			internal readonly EventArgs Args;
			internal readonly CallTreeNode Node;
		}

#if DEBUG
		private void workWithCallTreeNode(CallTreeNode node, object holder, Root root, WorkWithCallTreeNodeType workType)
#else
		private void workWithCallTreeNode(CallTreeNode node, object holder, WorkWithCallTreeNodeType workType)
#endif
		{
			if (workType == WorkWithCallTreeNodeType.Dispose)
			{
				if (node._call.Type == CallType.Method)
				{
					ExpressionWatcher[] nodeCallArguments = node._callArguments;
					if (nodeCallArguments != null)
					{
						int callArgumentsLength = nodeCallArguments.Length;
						for (int index = 0; index < callArgumentsLength; index++)
						{
							nodeCallArguments[index].Dispose();
						}
					}
				}
			}
			else if (!ReferenceEquals(holder, node._holder))
			{
				switch (node._call.Type)
				{
					case CallType.PropertyOrField:
						if (node._propertyChangedEventHandler != null)
						{
							((INotifyPropertyChanged)node._holder).PropertyChanged -= node._propertyChangedEventHandler;
							node._propertyChangedEventHandler = null;
						}
						break;
					case CallType.Method:
						if (node._methodChangedEventHandler != null)
						{
							((INotifyMethodChanged)node._holder).MethodChanged -= node._methodChangedEventHandler;
							node._methodChangedEventHandler = null;
						}

						break;
				}

				node._holder = holder;
				int callIndex = node._call.Index;

				IComputingInternal newComputingInternal = holder as IComputingInternal;

				if (newComputingInternal != _oldComputings[callIndex])
				{
					_oldComputings[callIndex] = _currentComputings[callIndex];
					_currentComputings[callIndex] = newComputingInternal;
					_rootExpressionWatcher._computingsChanged = true;
				}
			
				switch (node._call.Type)
				{
					case CallType.PropertyOrField:

						if (node._holder is INotifyPropertyChanged notifyPropertyChanged)
						{
							string propertyName = node._call.Name;
							node._propertyChangedEventHandler = (sender, args) =>
							{
								if (!_disposed && args.PropertyName == propertyName)
#if DEBUG
									processChange(node, root, sender, args);
#else
									processChange(node, sender, args);
#endif
							};

							notifyPropertyChanged.PropertyChanged += node._propertyChangedEventHandler;
							_propertyChangedEventSubscriptions[callIndex] = new PropertyChangedEventSubscription(notifyPropertyChanged, node._propertyChangedEventHandler);
						}
						break;
					case CallType.Method:
						if (node._holder is INotifyMethodChanged notifyMethodChanged)
						{
							CallTreeNode nodeCopy = node;
							node._methodChangedEventHandler = (sender, args) =>
							{
								if (!_disposed && args.MethodName == nodeCopy._call.Name)
								{
									int length = nodeCopy._call.GetArgumentValues.Length;
									object[] argumentValues = new object[length];
									for (int index = 0; index < length; index++)
										argumentValues[index] = nodeCopy._call.GetArgumentValues[index].DynamicInvoke(_parameterValues);

									if (args.ArgumentsPredicate(argumentValues))
#if DEBUG
										processChange(node, root, sender, args);
#else
										processChange(node, sender, args);
#endif
								}

							};
			
							notifyMethodChanged.MethodChanged += node._methodChangedEventHandler;
							_methodChangedEventSubscriptions[callIndex] = new MethodChangedEventSubscription(notifyMethodChanged, node._methodChangedEventHandler);
						}

						ExpressionWatcher[] nodeCallArguments = node._callArguments;
						if (nodeCallArguments != null)
						{
							node._callArgumentChangedEventHandler = (ew, sender, args) =>
							{
								if (!_disposed)
#if DEBUG
									processChange(node, root, sender, args);
#else
									processChange(node, sender, args);
#endif

							};

							int callArgumentsLength = nodeCallArguments.Length;
							for (int index = 0; index < callArgumentsLength; index++)
								nodeCallArguments[index].ValueChanged = node._callArgumentChangedEventHandler;
						}
						break;
				}			

#if DEBUG
				workWithCallTreeNodeChildren(node, root, workType);	
#else
				workWithCallTreeNodeChildren(node, workType);	
#endif

			}

		}


#if DEBUG
		private void processChange(CallTreeNode node, Root root, object sender, EventArgs args)
#else
		private void processChange(CallTreeNode node, object sender, EventArgs args)
#endif
		{
			if (_processingChange)
			{
				(_processChangeTasksQueue = _processChangeTasksQueue ?? new Queue<ProcessChangeTask>())
					.Enqueue(new ProcessChangeTask(
#if DEBUG
						root,
#endif
						sender, args, node
					));
			}
			else
			{
				_processingChange = true;
				_computingsChanged = false;
#if DEBUG
				workWithCallTreeNodeChildren(node, root, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
				raiseValueChanged(root, sender, args);
#else
				workWithCallTreeNodeChildren(node, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
				raiseValueChanged(sender, args);
#endif
				if (_processChangeTasksQueue != null)
				{
					while (_processChangeTasksQueue.Count > 0)
					{
						ProcessChangeTask processChangeTask = _processChangeTasksQueue.Dequeue();
						_computingsChanged = false;
#if DEBUG
						workWithCallTreeNodeChildren(processChangeTask.Node, processChangeTask.Root, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
						raiseValueChanged(processChangeTask.Root, processChangeTask.Sender, processChangeTask.Args);
#else
						workWithCallTreeNodeChildren(processChangeTask.Node, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
						raiseValueChanged(processChangeTask.Sender, processChangeTask.Args);
#endif
					}
				}

				_processingChange = false;
			}
		}


#if DEBUG
		private void workWithCallTreeNodeChildren(CallTreeNode node, Root root, WorkWithCallTreeNodeType workType)
#else
		private void workWithCallTreeNodeChildren(CallTreeNode node, WorkWithCallTreeNodeType workType)
#endif
		{
			CallTreeNode[] nodeChildren = node._children;
			object nodeHolder = node._holder;
			if (nodeChildren != null && nodeHolder != null)
			{
				int nodeChildrenLength = nodeChildren.Length;
				if (nodeChildrenLength > 0)
				{
					object value = null;
					
					if (workType == WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder)
					{
						switch (node._call.Type)
						{
							case CallType.PropertyOrField:
								value = node._call.GetMemberValue(nodeHolder);
								break;
							case CallType.Method:
								int length = node._call.GetArgumentValues.Length;
								object[] args = new object[length + 1];
								args[0] = nodeHolder;
								for (int index = 0; index < length; index++)
									args[index + 1] = node._call.GetArgumentValues[index]
										.DynamicInvoke(_parameterValues);

								value = node._call.GetMethodValue.DynamicInvoke(args);
								break;
						}

						node._call.Result = value;					
					}
					else //if (workType == WorkWithCallTreeNodeType.Dispose)
					{
						value = node._call.Result;
					}


					if (value != null)
					{
						int childrenCount = nodeChildrenLength;
						for (int index = 0; index < childrenCount; index++)
#if DEBUG
							workWithCallTreeNode(nodeChildren[index], value, root, workType);
#else
							workWithCallTreeNode(nodeChildren[index], value, workType);
#endif						
					}			
				}
			}
		}


		public struct ConstantCallPaths
		{
			public readonly object ConstantValue;
			public readonly List<List<Call>> CallPaths;

			public ConstantCallPaths(object constantValue, List<List<Call>> callPaths)
			{
				ConstantValue = constantValue;
				CallPaths = callPaths;
			}
		}

		public struct ParameterCallPaths
		{
			public readonly int ParameterIndex;
			public readonly List<List<Call>> CallPaths;

			public ParameterCallPaths(int parameterIndex, List<List<Call>> callPaths)
			{
				ParameterIndex = parameterIndex;
				CallPaths = callPaths;
			}
		}

		private readonly struct ExpressionCallPaths
		{
			public readonly LambdaExpression Expression;
			public readonly List<List<Call>> CallPaths;

			public ExpressionCallPaths(LambdaExpression expression, List<List<Call>> callPaths)
			{
				Expression = expression;
				CallPaths = callPaths;
			}
		}


		private sealed class CallPathsConstructor : ExpressionVisitor
		{
			// ReSharper disable once MemberHidesStaticFromOuterClass
			public readonly List<ConstantCallPaths> ConstantCallPaths;
			// ReSharper disable once MemberHidesStaticFromOuterClass
			public readonly List<ParameterCallPaths> ParameterCallPaths;
			// ReSharper disable once MemberHidesStaticFromOuterClass
			public readonly List<ExpressionCallPaths> ExpressionCallPaths;

			private List<Call> _callPathBuffer = new List<Call>(8);
			private readonly LambdaExpression _expression;
			private readonly List<Expression> _visitedExpressions =  new List<Expression>();

			public CallPathsConstructor(LambdaExpression expression)
			{
				ConstantCallPaths = new List<ConstantCallPaths>(8);
				ParameterCallPaths = new List<ParameterCallPaths>(8);
				ExpressionCallPaths = new List<ExpressionCallPaths>(8);

				_expression = expression;
			}

			public override Expression Visit(Expression node)
			{
				int visitedExpressionsCount = _visitedExpressions.Count;
				for (int index = 0; index < visitedExpressionsCount; index++)
				{
					if (_visitedExpressions[index] == node)
					{
						_visitedExpressions.RemoveAt(index);
						return node;
					}
				}

				if (node is LambdaExpression) return base.Visit(node);
				MemberExpression memberExpression = node as MemberExpression;
				MethodCallExpression methodCallExpression = node as MethodCallExpression;

				if (memberExpression == null && methodCallExpression == null
					|| (memberExpression != null && memberExpression.IsStatic())
					|| (methodCallExpression != null && methodCallExpression.Method.IsStatic))
				{
					return base.Visit(node);
				}			

				ConstantExpression parentConstantExpression;
				ParameterExpression parentParameterExpression;
				MethodCallExpression parentMethodCallExpression;
				MemberExpression parentMemberExpression;
				Expression parentExpression;
				List<List<Call>> memberCallPaths;

				if (memberExpression != null)
				{
					_callPathBuffer.Insert(0,
						new Call
						(
							getMemberValueFunc(memberExpression), 
							memberExpression.Member.Name			
						));

					parentConstantExpression = memberExpression.Expression as ConstantExpression;
					parentParameterExpression = memberExpression.Expression as ParameterExpression;
					parentMethodCallExpression = memberExpression.Expression as MethodCallExpression;
					parentMemberExpression = memberExpression.Expression as MemberExpression;
					parentExpression = memberExpression.Expression;	
				}
				else //if (methodCallExpression != null)
				{
					int argumentsCount = methodCallExpression.Arguments.Count;
					List<LambdaExpression> argumentExpressions = new List<LambdaExpression>(argumentsCount);
					Delegate[] argumentValues = new Delegate[argumentsCount];

					for (int index = 0; index < argumentsCount; index++)
					{
						Expression argument = methodCallExpression.Arguments[index];
						_visitedExpressions.Add(argument);

						if (!(argument is LambdaExpression) || ((LambdaExpression) argument).Parameters.Count == 0)
						{
							argumentExpressions.Add(Expression.Lambda(argument, _expression.Parameters));
						}

						argumentValues[index] = getMethodArgumentValueFunc(argument);
					}

					_callPathBuffer.Insert(0,
						new Call
						(
							getMethodValueFunc(methodCallExpression).Compile(), 
							//GetMethodValueExpression = methodValueExpression, 
							methodCallExpression.Method.Name, 
							argumentExpressions,
							argumentValues
						));

					parentConstantExpression = methodCallExpression.Object as ConstantExpression;
					parentParameterExpression = methodCallExpression.Object as ParameterExpression;	
					parentMethodCallExpression = methodCallExpression.Object as MethodCallExpression;
					parentMemberExpression = methodCallExpression.Object as MemberExpression;
					parentExpression = methodCallExpression.Object;				
				}

				if (parentParameterExpression != null)
				{
					int parameterIndex = _expression.Parameters.IndexOf(parentParameterExpression);
					if (parameterIndex >= 0)
					{
						memberCallPaths = null;
						int count = ParameterCallPaths.Count;
						for (int index = 0; index < count; index++)
						{
							ParameterCallPaths parameterCallPaths = ParameterCallPaths[index];
							if (parameterCallPaths.ParameterIndex == parameterIndex)
							{
								memberCallPaths = parameterCallPaths.CallPaths;
								break;
							}

						}

						if (memberCallPaths == null)
						{
							memberCallPaths = new List<List<Call>>(8);
							ParameterCallPaths.Add(new ParameterCallPaths(parameterIndex, memberCallPaths));
						}

						memberCallPaths.Add(_callPathBuffer);						
					}

					_callPathBuffer = new List<Call>(8);
				}
				else if (parentConstantExpression != null)
				{
					object key = parentConstantExpression.Value;
					if (key != null)
					{
						memberCallPaths = null;
						int count = ConstantCallPaths.Count;
						for (int index = 0; index < count; index++)
						{
							ConstantCallPaths constantCallPaths = ConstantCallPaths[index];
							if (ReferenceEquals(constantCallPaths.ConstantValue, key))
							{
								memberCallPaths = constantCallPaths.CallPaths;
								break;
							}

						}

						if (memberCallPaths == null)
						{
							memberCallPaths = new List<List<Call>>(8);
							ConstantCallPaths.Add(new ConstantCallPaths(key, memberCallPaths));
						}

						memberCallPaths.Add(_callPathBuffer);
						_callPathBuffer = new List<Call>(8);
					}
				}
				else if (
					((parentMemberExpression != null && parentMemberExpression.IsStatic())
					|| (parentMethodCallExpression != null && parentMethodCallExpression.Method.IsStatic)
					|| (parentMemberExpression == null && parentMethodCallExpression == null)))
				{
					_visitedExpressions.Add(parentExpression);

					// ReSharper disable once AssignNullToNotNullAttribute
					LambdaExpression expressionToWatch = Expression.Lambda(parentExpression, _expression.Parameters);

					//memberCallPaths = null;
					//int count = ExpressionCallPaths.Count;
					//for (var index = 0; index < count; index++)
					//{
					//	ExpressionCallPaths expressionCallPaths = ExpressionCallPaths[index];
					//	if (expressionCallPaths.ExpressionInfo._expressionToWatch == expressionToWatch)
					//	{
					//		memberCallPaths = expressionCallPaths.CallPaths;
					//		break;
					//	}

					//}

					//if (memberCallPaths == null)
					//{

					memberCallPaths = new List<List<Call>>(8);
					ExpressionCallPaths.Add(new ExpressionCallPaths(expressionToWatch, memberCallPaths));
					//}

					memberCallPaths.Add(_callPathBuffer);
					_callPathBuffer = new List<Call>(8);

				}

				return base.Visit(node);
			}


			private static Func<object, object> getMemberValueFunc(MemberExpression node)
			{
				ParameterExpression memberHolderParameter = Expression.Parameter(typeof (object), "memberHolder");
				MemberExpression memberExpression;

				MemberInfo nodeMember = node.Member;

				if (nodeMember is PropertyInfo propertyInfo)
				{
					memberExpression = Expression.Property(Expression.Convert(memberHolderParameter, node.Expression.Type), propertyInfo);
				}
				else
				{
					memberExpression = Expression.Field(Expression.Convert(memberHolderParameter, node.Expression.Type), (FieldInfo)nodeMember);
				}

				UnaryExpression conversion = Expression.Convert(memberExpression, typeof (object));
				Expression<Func<object, object>> exp = Expression.Lambda<Func<object, object>>(conversion, new [] {memberHolderParameter});

				Func<object, object> getMemberValue = exp.Compile();
				return getMemberValue;
			}

			private LambdaExpression getMethodValueFunc(MethodCallExpression node)
			{
				// ReSharper disable once PossibleNullReferenceException
				ParameterExpression methodHolderParameter = Expression.Parameter(node.Object.Type, "methodHolder");

				//char argumentNum = 'A';
				ParameterExpression[] arguments = new ParameterExpression[node.Arguments.Count + 1];
				ParameterExpression[] parameters = new ParameterExpression[node.Arguments.Count];
				arguments[0] = methodHolderParameter;
				for (int index = 0; index < node.Arguments.Count; index++)
				{
					//argumentNum++;
					ParameterExpression parameterExpression = Expression.Parameter(node.Arguments[index].Type);
					arguments[index + 1] = parameterExpression;
					parameters[index] = parameterExpression;
				}

				//	= node.Arguments.Select(a =>
				//{
				//	argumentNum++;
				//	return Expression.Parameter(a.Type, $"argument{argumentNum.ToString()}");
				//}).ToArray();

				// ReSharper disable once CoVariantArrayConversion
				MethodCallExpression methodCallExpression = Expression.Call(methodHolderParameter, node.Method, parameters);				
				UnaryExpression conversion = Expression.Convert(methodCallExpression, typeof (object));
				LambdaExpression lambdaExpression = Expression.Lambda(conversion, arguments);
				return lambdaExpression;
			}

			private Delegate getMethodArgumentValueFunc(Expression node)
			{				
				UnaryExpression conversion = Expression.Convert(node, typeof (object));
				return Expression.Lambda(conversion, _expression.Parameters).Compile();
			}
		}

		public struct Call
		{
			public Call(Func<object, object> memberValue, string name) : this()
			{
				GetMemberValue = memberValue;
				Name = name;
				Type = CallType.PropertyOrField;
			}

			public Call(Delegate methodValue, string name, List<LambdaExpression> arguments, Delegate[] argumentValues) : this()
			{
				GetMethodValue = methodValue;
				Name = name;
				Arguments = arguments;
				GetArgumentValues = argumentValues;
				Type = CallType.Method;
			}

			public int Index;

			public readonly Func<object, object> GetMemberValue;
			public readonly Delegate GetMethodValue;

			public readonly string Name;
			public readonly List<LambdaExpression> Arguments;
			public ExpressionInfo[] ArgumentExpressionInfos;
			public readonly Delegate[] GetArgumentValues;

			public object Result;

			public readonly CallType Type;

			//public LambdaExpression GetMethodValueExpression { get; set; }
		}

		public enum CallType
		{
			PropertyOrField,
			Method
		}

		public struct CallTreeNodeInfo
		{
			internal Call _call;

			internal CallTreeNodeInfo(Call call, List<CallTreeNodeInfo> children, ref int callIndex)
			{
				_call = call;
				_call.Index = callIndex++;
				_children = children;

				if (_call.Arguments != null)
				{
					int argumentsLength = _call.Arguments.Count;
					if (argumentsLength > 0)
					{
						ExpressionInfo[] callArgumentExpressionInfos = new ExpressionInfo[_call.Arguments.Count];

						for (int index = 0; index < argumentsLength; index++)
							callArgumentExpressionInfos[index] =
								GetExpressionInfo(_call.Arguments[index], ref callIndex);

						_call.ArgumentExpressionInfos = callArgumentExpressionInfos;
					}
				}
			}

			internal readonly List<CallTreeNodeInfo> _children;

			internal CallTreeNode getCallTreeNode(object[] parameterValues,
				PropertyChangedEventSubscription[] propertyChangedEventSubscriptions,
				MethodChangedEventSubscription[] methodChangedEventSubscription, IComputingInternal[] oldComputings,
				IComputingInternal[] currentComputings, ExpressionWatcher rootExpressionWatcher)
			{
				CallTreeNode[] childNodes = null;
				int childrenCount = _children.Count;
				if (childrenCount > 0)
				{
					childNodes = new CallTreeNode[childrenCount];
					for (int index = 0; index < childrenCount; index++)
						childNodes[index] = _children[index].getCallTreeNode(parameterValues,
							propertyChangedEventSubscriptions, methodChangedEventSubscription, oldComputings,
							currentComputings, rootExpressionWatcher);
				}

				ExpressionWatcher[] callArguments = null;
				if (_call.Arguments != null)
				{
					int argumentsLength = _call.Arguments.Count;
					if (argumentsLength > 0)
					{
						callArguments =  new ExpressionWatcher[_call.Arguments.Count];

						for (int index = 0; index < argumentsLength; index++)
							callArguments[index] = new ExpressionWatcher(
								parameterValues,
								_call.ArgumentExpressionInfos[index],
								propertyChangedEventSubscriptions,
								methodChangedEventSubscription,
								oldComputings,
								currentComputings,
								rootExpressionWatcher);
					}
					
				}
				return new CallTreeNode(_call, childNodes, callArguments);
			}
		}

		public sealed class CallTreeNode
		{
			internal PropertyChangedEventHandler _propertyChangedEventHandler;

			internal object _holder;
			internal Call _call;

			internal CallTreeNode(Call call, CallTreeNode[] callTreeNodes, ExpressionWatcher[] callArguments)
			{
				_call = call;
				_children = callTreeNodes;
				_callArguments = callArguments;
			}

			internal readonly CallTreeNode[] _children;

			internal readonly ExpressionWatcher[] _callArguments;
			internal Action<ExpressionWatcher, object, EventArgs> _callArgumentChangedEventHandler;

			internal EventHandler<MethodChangedEventArgs> _methodChangedEventHandler;
		}

#if DEBUG
		private void raiseValueChanged(Root root, object sender, EventArgs eventArgs)
		{
			LastChangedRoot = root;
			if (ValueChanged != null) ValueChanged(this, sender, eventArgs);
		}
#else
		private void raiseValueChanged(object sender, EventArgs eventArgs)
		{
			if (ValueChanged != null) ValueChanged(this, sender, eventArgs);
		}
#endif



		internal void Dispose()
		{
			_disposed = true;

			if (_constantCallTrees != null)
			{
				int length = _constantCallTrees.Length;
				for (int index = 0; index < length; index++)
					workWithCallTrees(_constantCallTrees[index].CallTrees, null, WorkWithCallTreeNodeType.Dispose);
			}

			if (_parameterCallTrees != null)
			{
				int length = _parameterCallTrees.Length;
				for (int index = 0; index < length; index++)
				{
					ParameterCallTrees parameterCallTree = _parameterCallTrees[index];
					if (_parameterValues[parameterCallTree.ParameterIndex] is INotifyPropertyChanged)
						workWithCallTrees(parameterCallTree.CallTrees, null, WorkWithCallTreeNodeType.Dispose);
				}
			}

			if (_expressionCallTrees != null)
			{		
				foreach (ExpressionCallTrees expressionCallTree in _expressionCallTrees)
				{
					workWithCallTrees(expressionCallTree.CallTrees, null, WorkWithCallTreeNodeType.Dispose);
					expressionCallTree.ExpressionWatcher.Dispose();
				}
			}
		}

	}
}
