using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using ObservableComputations.Common.Interface;

namespace ObservableComputations.Common
{
	internal sealed class ExpressionWatcher : IDisposable
	{
		internal Position _position;

		public struct ExpressionInfo
		{
			internal readonly LambdaExpression _expressionToWatch;
			internal readonly ConstantCallTreesInfo[] _constantCallTrees;
			internal readonly ParameterCallTreesInfo[] _parameterCallTrees;
			internal readonly ExpressionCallTreesInfo[] _expressionCallTrees;
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

		public Action<ExpressionWatcher> ValueChanged;
		//public event EventHandler<EventArgs> ValueChanged;

		public struct ConstantCallTreesInfo
		{
			public object ConstantValue;
			public List<CallTreeNodeInfo> CallTrees;

			public ConstantCallTreesInfo(object constantValue, List<CallTreeNodeInfo> callTrees)
			{
				ConstantValue = constantValue;
				CallTrees = callTrees;
			}
		}

		public struct ParameterCallTreesInfo
		{
			public int ParameterIndex;
			public List<CallTreeNodeInfo> CallTrees;

			public ParameterCallTreesInfo(int parameterIndex, List<CallTreeNodeInfo> callTrees)
			{
				ParameterIndex = parameterIndex;
				CallTrees = callTrees;
			}
		}

		public struct ExpressionCallTreesInfo
		{
			public ExpressionInfo ExpressionInfo;
			public List<CallTreeNodeInfo> CallTrees;

			public ExpressionCallTreesInfo(ExpressionInfo expressionInfo, List<CallTreeNodeInfo> callTrees)
			{
				ExpressionInfo = expressionInfo;
				CallTrees = callTrees;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public struct ConstantCallTrees
		{
			public object ConstantValue;
			public CallTreeNode[] CallTrees;

			public ConstantCallTrees(object constantValue, CallTreeNode[] callTrees)
			{
				ConstantValue = constantValue;
				CallTrees = callTrees;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public struct ParameterCallTrees
		{
			public int ParameterIndex;
			public CallTreeNode[] CallTrees;

			public ParameterCallTrees(int parameterIndex, CallTreeNode[] callTrees)
			{
				ParameterIndex = parameterIndex;
				CallTrees = callTrees;
			}
		}

		// ReSharper disable once MemberCanBePrivate.Global
		public struct ExpressionCallTrees
		{
			public ExpressionWatcher ExpressionWatcher;
			public CallTreeNode[] CallTrees;

			public ExpressionCallTrees(ExpressionWatcher expressionWatcher, CallTreeNode[] callTrees) : this()
			{
				ExpressionWatcher = expressionWatcher;
				CallTrees = callTrees;
			}
		}


		public static ExpressionInfo GetExpressionInfo(LambdaExpression expression)
		{
			CallPathsConstructor callPathsConstructor =  new CallPathsConstructor(expression);
			callPathsConstructor.Visit(expression);

			int count;

			count = callPathsConstructor.ConstantCallPaths.Count;
			ConstantCallTreesInfo[] constantsCallTreesInfo = null;
			if (count > 0)
			{
				constantsCallTreesInfo = new ConstantCallTreesInfo[count];
				for (var index = 0; index < count; index++)
				{
					ConstantCallPaths constantCallPaths = callPathsConstructor.ConstantCallPaths[index];
					constantsCallTreesInfo[index] = new ConstantCallTreesInfo(constantCallPaths.ConstantValue, getCallTrees(constantCallPaths.CallPaths));
				}
			}

			count = callPathsConstructor.ParameterCallPaths.Count;
			ParameterCallTreesInfo[] parameterCallTreesInfo = null;
			if (count > 0)
			{
				parameterCallTreesInfo = new ParameterCallTreesInfo[count];
				for (var index = 0; index < count; index++)
				{
					ParameterCallPaths parameterCallPaths = callPathsConstructor.ParameterCallPaths[index];
					parameterCallTreesInfo[index] = new ParameterCallTreesInfo(parameterCallPaths.ParameterIndex, getCallTrees(parameterCallPaths.CallPaths));
				}
			}

			count = callPathsConstructor.ExpressionCallPaths.Count;
			ExpressionCallTreesInfo[] expressionCallTreesInfo = null;
			if (count > 0)
			{
				expressionCallTreesInfo = new ExpressionCallTreesInfo[count];
				for (var index = 0; index < count; index++)
				{
					ExpressionCallPaths expressionCallPaths = callPathsConstructor.ExpressionCallPaths[index];
					expressionCallTreesInfo[index] = new ExpressionCallTreesInfo(expressionCallPaths.ExpressionInfo, getCallTrees(expressionCallPaths.CallPaths));
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

		public ExpressionWatcher(ExpressionInfo expressionInfo)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = null;
			initialize(expressionInfo);
		}

		// ReSharper disable once MemberCanBePrivate.Global
		internal ExpressionWatcher(object[] parameterValues, ExpressionInfo expressionInfo)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = parameterValues;
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object parameter1)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = new []{parameter1};
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object parameter1, object parameter2)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = new []{parameter1, parameter2};
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object parameter1, object parameter2, object parameter3)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = new []{parameter1, parameter2, parameter3};
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object parameter1, object parameter2, object parameter3, object parameter4)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = new []{parameter1, parameter2, parameter3, parameter4};
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = new []{parameter1, parameter2, parameter3, parameter4, parameter5};
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = new []{parameter1, parameter2, parameter3, parameter4, parameter5, parameter6};
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = new []{parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7};
			initialize(expressionInfo);
		}

		public ExpressionWatcher(ExpressionInfo expressionInfo, object parameter1, object parameter2, object parameter3, object parameter4, object parameter5, object parameter6, object parameter7, object parameter8)
		{
			ExpressionToWatch = expressionInfo._expressionToWatch;
			_parameterValues = new []{parameter1, parameter2, parameter3, parameter4, parameter5, parameter6, parameter7, parameter8};
			initialize(expressionInfo);
		}


		private void workWithCallTrees(ParameterCallTreesInfo[] parameterCallTreesInfo,  ExpressionCallTreesInfo[] expressionCallTreesInfo, object[] parameterValues)
		{
			int length;
			if (parameterCallTreesInfo != null)
			{
				length = parameterCallTreesInfo.Length;
				_parameterCallTrees = new ParameterCallTrees[length];
				for (var index = 0; index < length; index++)
				{
					ParameterCallTreesInfo parameterCallTreesInfoItem = parameterCallTreesInfo[index];
					int callTreesCount = parameterCallTreesInfoItem.CallTrees.Count;
					CallTreeNode[] callTrees = new CallTreeNode[callTreesCount];
					for (int i = 0; i < callTreesCount; i++)
					{
						callTrees[i] = parameterCallTreesInfoItem.CallTrees[i].getCallTreeNode();
					}

					_parameterCallTrees[index] = new ParameterCallTrees(parameterCallTreesInfoItem.ParameterIndex, callTrees);
				}

				length = _parameterCallTrees.Length;
				for (var index= 0; index < length; index++)
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
			for (var index = 0; index < length; index++)
			{
				ExpressionCallTreesInfo expressionCallTreesInfoItem = expressionCallTreesInfo[index];
				ExpressionWatcher expressionWatcher = new ExpressionWatcher(expressionCallTreesInfoItem.ExpressionInfo);
				expressionWatcher.complileExpressionToWatch();

				int callTreesCount = expressionCallTreesInfoItem.CallTrees.Count;
				CallTreeNode[] callTrees = new CallTreeNode[callTreesCount];
				for (int i = 0; i < callTreesCount; i++)
				{
					callTrees[i] = expressionCallTreesInfoItem.CallTrees[i].getCallTreeNode();
				}

				_expressionCallTrees[index] = new ExpressionCallTrees(expressionWatcher, callTrees);

			}

			foreach (ExpressionCallTrees parameterCallTree in _expressionCallTrees)
			{
#if DEBUG
				Root getRoot()
#else
				object getRoot()
#endif
				{
					object value = parameterCallTree.ExpressionWatcher._expressionToWatchCompiled.DynamicInvoke(parameterValues);

#if DEBUG
					return new RootExpression(value, parameterCallTree.ExpressionWatcher.ExpressionToWatch);
#else
					return value;
#endif
				}
#if DEBUG
				Root root = getRoot();
				workWithCallTrees(parameterCallTree.CallTrees, root, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
#else
				workWithCallTrees(parameterCallTree.CallTrees, getRoot(), WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
#endif


				parameterCallTree.ExpressionWatcher.ValueChanged =
					ew =>
					{
#if DEBUG
						Root root1 = getRoot();
						workWithCallTrees(parameterCallTree.CallTrees, root1, WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
						raiseValueChanged(LastChangedRoot);
#else
						workWithCallTrees(parameterCallTree.CallTrees, getRoot(), WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder);
						raiseValueChanged();
#endif
					};
			}
			
		}

		private void workWithCallTrees(ConstantCallTreesInfo[] constantCallTreesInfo)
		{
			int length = constantCallTreesInfo.Length;
			_constantCallTrees = new ConstantCallTrees[length];
			for (var index = 0; index < length; index++)
			{
				ConstantCallTreesInfo constantCallTreesInfoItem = constantCallTreesInfo[index];
				int callTreesCount = constantCallTreesInfoItem.CallTrees.Count;
				CallTreeNode[] callTrees = new CallTreeNode[callTreesCount];
				for (int i = 0; i < callTreesCount; i++)
				{
					callTrees[i] = constantCallTreesInfoItem.CallTrees[i].getCallTreeNode();
				}

				_constantCallTrees[index] = new ConstantCallTrees(constantCallTreesInfoItem.ConstantValue, callTrees);
			}

			length = _constantCallTrees.Length;
			for (var index = 0; index < length; index++)
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

		private object[] _parameterValues;
		public IEnumerable<object> ParameterValues => _parameterValues;

		// ReSharper disable once MemberCanBePrivate.Global
		public LambdaExpression ExpressionToWatch { get; }

		private Delegate _expressionToWatchCompiled;

		private void complileExpressionToWatch()
		{
			_expressionToWatchCompiled = ExpressionToWatch.Compile();
		}

		private ConstantCallTrees[] _constantCallTrees;
		private ParameterCallTrees[]_parameterCallTrees;
		private ExpressionCallTrees[] _expressionCallTrees;

		internal bool _disposed;

		private static List<CallTreeNodeInfo> getCallTrees(List<List<Call>> callPaths)
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
					CallTreeNodeInfo traversingPpa = new CallTreeNodeInfo(callPath[pathElementIndex], new List<CallTreeNodeInfo>(8));
					if (pathElementIndex == 0)
					{
						CallTreeNodeInfo? rootResultPpa = null;
						for (int i = 0; i < resultCallTreesCount; i++)
						{
							CallTreeNodeInfo callTreeNode = resultCallTrees[i];
							Call call = callTreeNode._call;
							if 
								(
									call.Name == traversingPpa._call.Name
									&& (call.Type != CallType.Method || call.Arguments.Count == 0) //Если метод с аргументами, то считаем, что по любому это разные выховы. Не учитываем, что метод мог быть вызван с одинаковыми аргуметами
								)
							{
								rootResultPpa = callTreeNode;
								break;
							}
						}

						if (rootResultPpa == null)
						{
							rootResultPpa = covertPathToTree(callPath, 0);
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
							if (callTreeNode._call.Name == traversingPpa._call.Name)
							{
								newCurrentResultPpa = callTreeNode;
								break;
							}
						}

						if (newCurrentResultPpa == null)
						{
							newCurrentResultPpa = covertPathToTree(callPath, pathElementIndex);
							callTreeNodeInfo._children.Add(newCurrentResultPpa.Value);
						}

						currentResultPpa = newCurrentResultPpa;
					}
				}
			}

			return resultCallTrees;
		}


		private static CallTreeNodeInfo covertPathToTree(List<Call> callPath, int startIndex)
		{
			CallTreeNodeInfo result = new CallTreeNodeInfo(callPath[startIndex], new List<CallTreeNodeInfo>(8));
			CallTreeNodeInfo current = result;

			int count = callPath.Count;
			for (int index = startIndex + 1; index < count; index++)
			{
				CallTreeNodeInfo callPathElement = new CallTreeNodeInfo(callPath[index], new List<CallTreeNodeInfo>(8));
				current._children.Add(callPathElement);
				current = callPathElement;
			}

			return result;
		}

#if DEBUG
		private void workWithCallTrees(CallTreeNode[] trees, Root root, WorkWithCallTreeNodeType workType)
		{
			int treesCount = trees.Length;
			for (var index = 0; index < treesCount; index++)
			{
				CallTreeNode tree = trees[index];
				workWithCallTreeNode(tree, root?.Value, root, workType);
			}
		}
#else
		private void workWithCallTrees(CallTreeNode[] trees, object root, WorkWithCallTreeNodeType workType)
		{
			int treesCount = trees.Length;
			for (int index = 0; index < treesCount; index++)
			{
				CallTreeNode tree = trees[index];
				workWithCallTreeNode(tree, root, workType);
			}	
		}
#endif


		private enum WorkWithCallTreeNodeType
		{
			UpdateSubscriptionAndHolder,
			Dispose
		}

#if DEBUG
		private void workWithCallTreeNode(CallTreeNode node, object holder, Root root, WorkWithCallTreeNodeType workType)
#else
		private void workWithCallTreeNode(CallTreeNode node, object holder, WorkWithCallTreeNodeType workType)
#endif
		{
			switch (node._call.Type)
			{
				case CallType.PropertyOrField:
					if (node._propertyChangedEventHandler != null)
					{
						((INotifyPropertyChanged)node._holder).PropertyChanged -= node._weakPropertyChangedEventHandler.Handle;
						node._propertyChangedEventHandler = null;
						node._weakPropertyChangedEventHandler = null;
					}
					break;
				case CallType.Method:
					if (node._methodChangedEventHandler != null)
					{
						((INotifyMethodChanged)node._holder).MethodChanged -= node._weakMethodChangedEventHandler.Handle;
						node._methodChangedEventHandler = null;
						node._weakMethodChangedEventHandler = null;
					}			

					if (workType == WorkWithCallTreeNodeType.Dispose)
					{
						ExpressionWatcher[] nodeCallArguments = node._callArguments;
						if (nodeCallArguments != null)
						{
							int callArgumentsLength = nodeCallArguments.Length;
							for (int index = 0; index < callArgumentsLength; index++)
							{
								ExpressionWatcher expressionWatcher = nodeCallArguments[index];
								expressionWatcher.Dispose();
							}
						}

						node._callArgumentChangedEventHandler = null;
					}

					break;
			}

			if (workType == WorkWithCallTreeNodeType.UpdateSubscriptionAndHolder)
			{
				node._holder = holder;

				switch (node._call.Type)
				{
					case CallType.PropertyOrField:

						if (node._holder is INotifyPropertyChanged notifyPropertyChanged)
						{
							node._propertyChangedEventHandler = (sender, args) =>
							{
								if (!_disposed && args.PropertyName == node._call.Name)
								{
#if DEBUG
									raiseValueChanged(root);
									workWithCallTreeNodeChildren(node, root, workType);
#else
									raiseValueChanged();
									workWithCallTreeNodeChildren(node, workType);
#endif
								}
							};	

							node._weakPropertyChangedEventHandler = new WeakPropertyChangedEventHandler(node._propertyChangedEventHandler);
		
							notifyPropertyChanged.PropertyChanged += node._weakPropertyChangedEventHandler.Handle;
						}
						break;
					case CallType.Method:
						INotifyMethodChanged notifyMethodChanged = node._holder as INotifyMethodChanged;
						if (notifyMethodChanged != null)
						{
							node._methodChangedEventHandler = (sender, args) =>
							{
								if (!_disposed && args.MethodName == node._call.Name)
								{
									int length = node._call.GetArgumentValues.Length;
									object[] argumentValues = new object[length];
									for (var index = 0; index < length; index++)
									{
										Delegate callGetArgumentValue = node._call.GetArgumentValues[index];
										argumentValues[index] = callGetArgumentValue.DynamicInvoke(_parameterValues);
									}

									if (args.ArgumentsPredicate(argumentValues))
									{
	#if DEBUG
										raiseValueChanged(root);
										workWithCallTreeNodeChildren(node, root, workType);
	#else
										raiseValueChanged();
										workWithCallTreeNodeChildren(node, workType);
	#endif
									}
								}

							};
	
							node._weakMethodChangedEventHandler = new WeakEventHandler<NotifyMethodChangedEventArgs>(node._methodChangedEventHandler);
		
							notifyMethodChanged.MethodChanged += node._methodChangedEventHandler;
						}

						ExpressionWatcher[] nodeCallArguments = node._callArguments;
						if (nodeCallArguments == null)
						{
							node._callArguments = nodeCallArguments = new ExpressionWatcher[node._call.Arguments.Count];
							int argumentsLength = node._call.Arguments.Count;
							for (var index = 0; index < argumentsLength; index++)
							{
								ExpressionInfo expressionInfo = node._call.Arguments[index];
								nodeCallArguments[index] = new ExpressionWatcher(_parameterValues, expressionInfo);
							}

							node._callArgumentChangedEventHandler = ew =>
							{
#if DEBUG
									raiseValueChanged(root);
									workWithCallTreeNodeChildren(node, root, workType);
#else
									raiseValueChanged();
									workWithCallTreeNodeChildren(node, workType);
#endif
							};

							int callArgumentsLength = nodeCallArguments.Length;
							for (int index = 0; index < callArgumentsLength; index++)
							{
								ExpressionWatcher expressionWatcher = nodeCallArguments[index];
								expressionWatcher.ValueChanged = node._callArgumentChangedEventHandler;
							}
						}
						break;
				}			
			}

#if DEBUG
			workWithCallTreeNodeChildren(node, root, workType);	
#else
			workWithCallTreeNodeChildren(node, workType);	
#endif
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
								for (var index = 0; index < length; index++)
								{
									Delegate callGetArgumentValue = node._call.GetArgumentValues[index];
									args[index + 1] = callGetArgumentValue.DynamicInvoke(_parameterValues);
								}

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
						{
							CallTreeNode child = nodeChildren[index];
	#if DEBUG
							workWithCallTreeNode(child, value, root, workType);
	#else
							workWithCallTreeNode(child, value, workType);
	#endif
						}
					}			
				}
			}
		}


		public struct ConstantCallPaths
		{
			public object ConstantValue;
			public List<List<Call>> CallPaths;

			public ConstantCallPaths(object constantValue, List<List<Call>> callPaths)
			{
				ConstantValue = constantValue;
				CallPaths = callPaths;
			}
		}

		public struct ParameterCallPaths
		{
			public int ParameterIndex;
			public List<List<Call>> CallPaths;

			public ParameterCallPaths(int parameterIndex, List<List<Call>> callPaths)
			{
				ParameterIndex = parameterIndex;
				CallPaths = callPaths;
			}
		}

		private readonly struct ExpressionCallPaths
		{
			public readonly ExpressionInfo ExpressionInfo;
			public readonly List<List<Call>> CallPaths;

			public ExpressionCallPaths(ExpressionInfo expressionInfo, List<List<Call>> callPaths)
			{
				ExpressionInfo = expressionInfo;
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
				for (var index = 0; index < visitedExpressionsCount; index++)
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
					List<ExpressionInfo> argumentExpressionInfos = new List<ExpressionInfo>(argumentsCount);
					Delegate[] argumentValues = new Delegate[argumentsCount];

					for (int index = 0; index < argumentsCount; index++)
					{
						Expression argument = methodCallExpression.Arguments[index];
						_visitedExpressions.Add(argument);

						if (!(argument is LambdaExpression) || ((LambdaExpression) argument).Parameters.Count == 0)
						{
							argumentExpressionInfos.Add(
								GetExpressionInfo(Expression.Lambda(argument, _expression.Parameters)));
						}

						argumentValues[index] = getMethodArgumentValueFunc(argument);
					}

					_callPathBuffer.Insert(0,
						new Call
						(
							getMethodValueFunc(methodCallExpression).Compile(), 
							//GetMethodValueExpression = methodValueExpression, 
							methodCallExpression.Method.Name, 
							argumentExpressionInfos,
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
						for (var index = 0; index < count; index++)
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
						for (var index = 0; index < count; index++)
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
					ExpressionInfo expressionInfo = GetExpressionInfo(expressionToWatch);

					memberCallPaths = new List<List<Call>>(8);
					ExpressionCallPaths.Add(new ExpressionCallPaths(expressionInfo, memberCallPaths));
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
				for (var index = 0; index < node.Arguments.Count; index++)
				{
					Expression nodeArgument = node.Arguments[index];
					//argumentNum++;
					ParameterExpression parameterExpression = Expression.Parameter(nodeArgument.Type);
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

			public Call(Delegate methodValue, string name, List<ExpressionInfo> arguments, Delegate[] argumentValues) : this()
			{
				GetMethodValue = methodValue;
				Name = name;
				Arguments = arguments;
				GetArgumentValues = argumentValues;
				Type = CallType.Method;
			}

			public readonly Func<object, object> GetMemberValue;
			public readonly Delegate GetMethodValue;

			public readonly string Name;
			public readonly List<ExpressionInfo> Arguments;
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

			internal CallTreeNodeInfo(Call call, List<CallTreeNodeInfo> children)
			{
				_call = call;
				_children = children;
			}

			internal readonly List<CallTreeNodeInfo> _children;

			internal CallTreeNode getCallTreeNode()
			{
				CallTreeNode[] childNodes = null;
				int childrenCount = _children.Count;
				if (childrenCount > 0)
				{
					childNodes = new CallTreeNode[childrenCount];
				for (var index = 0; index < childrenCount; index++)
				{
					CallTreeNodeInfo callTreeNodeInfo = _children[index];
					childNodes[index] = callTreeNodeInfo.getCallTreeNode();
				}
				}

				return new CallTreeNode(_call, childNodes);
			}
		}

		public sealed class CallTreeNode
		{
			internal PropertyChangedEventHandler _propertyChangedEventHandler;
			internal WeakPropertyChangedEventHandler _weakPropertyChangedEventHandler;

			internal object _holder;
			internal Call _call;

			internal CallTreeNode(Call call, CallTreeNode[] callTreeNodes)
			{
				_call = call;
				_children = callTreeNodes;
			}

			internal readonly CallTreeNode[] _children;

			internal ExpressionWatcher[] _callArguments;
			internal Action<ExpressionWatcher> _callArgumentChangedEventHandler;

			internal EventHandler<NotifyMethodChangedEventArgs> _methodChangedEventHandler;
			internal WeakEventHandler<NotifyMethodChangedEventArgs> _weakMethodChangedEventHandler;
		}

#if DEBUG
		private void raiseValueChanged(Root root)
		{
			LastChangedRoot = root;
			ValueChanged(this);
		}
#else
		private void raiseValueChanged()
		{
			ValueChanged(this);
		}
#endif


		#region Implementation of IDisposable

		public void Dispose()
		{
			_disposed = true;
			if (_constantCallTrees != null)
			{
				int length = _constantCallTrees.Length;
				for (var index = 0; index < length; index++)
				{
					ConstantCallTrees constantCallTree = _constantCallTrees[index];
#if DEBUG
					workWithCallTrees(constantCallTree.CallTrees, null, WorkWithCallTreeNodeType.Dispose);
#else
					workWithCallTrees(constantCallTree.CallTrees, null, WorkWithCallTreeNodeType.Dispose);
#endif
				}
			}

			if (_parameterCallTrees != null)
			{
				int length = _parameterCallTrees.Length;
				for (var index = 0; index < length; index++)
				{
					ParameterCallTrees parameterCallTree = _parameterCallTrees[index];
					if (_parameterValues[parameterCallTree.ParameterIndex] is INotifyPropertyChanged)
					{
#if DEBUG
						workWithCallTrees(parameterCallTree.CallTrees, null, WorkWithCallTreeNodeType.Dispose);
#else
					workWithCallTrees(parameterCallTree.CallTrees, null, WorkWithCallTreeNodeType.Dispose);
#endif
					}
				}
			}

			if (_expressionCallTrees != null)
			{		
				foreach (ExpressionCallTrees expressionCallTree in _expressionCallTrees)
				{
#if DEBUG
					workWithCallTrees(expressionCallTree.CallTrees, null, WorkWithCallTreeNodeType.Dispose);				
#else
					workWithCallTrees(expressionCallTree.CallTrees, null, WorkWithCallTreeNodeType.Dispose);
#endif	
					expressionCallTree.ExpressionWatcher.Dispose();
				}
			}
		}

#endregion

		~ExpressionWatcher ()
		{
			Dispose();
		}
	}
}
