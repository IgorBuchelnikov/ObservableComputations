// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ThreadState = System.Threading.ThreadState;

namespace ObservableComputations
{
	public class Invocation
	{
		public Action Action => _action;
		public int Priority => _priority;
		public Action<object> ActionWithState => _actionWithState;
		public object State => _state;
		public string CallStackTrace => _callStackTrace;
		public object Context => _context;
		public OcDispatcher OcDispatcher => _ocDispatcher;
		public InvocationStatus Status => _status;
		public Invocation Parent => _parent;
		public bool SetSynchronizationContext => _setSynchronizationContext;

		private readonly Action _action;
		private readonly Action<object> _actionWithState;
		private readonly object _state;
		private readonly string _callStackTrace;
		internal readonly object _context;
		private readonly OcDispatcher _ocDispatcher;
		internal int _priority;
		internal InvocationStatus _status;
		internal readonly ManualResetEventSlim _doneManualResetEvent;
		private Invocation _parent;
		private bool _setSynchronizationContext;

		internal Invocation(Action action, int priority, OcDispatcher ocDispatcher, 
			bool setSynchronizationContext, Invocation parent, string callStackTrace, object context = null, 
			ManualResetEventSlim doneManualResetEvent = null)
		{
			_action = action;
			_priority = priority;
			_ocDispatcher = ocDispatcher;
			_setSynchronizationContext = setSynchronizationContext;
			_context = context;
			_doneManualResetEvent = doneManualResetEvent;
			_callStackTrace = callStackTrace;
			_parent = parent;
		}

		internal Invocation(Action<object> actionWithState, int priority, object state, OcDispatcher ocDispatcher, 
			bool setSynchronizationContext, Invocation parent, string callStackTrace, object context = null,
			ManualResetEventSlim doneManualResetEvent = null)
		{
			_actionWithState = actionWithState;
			_priority = priority;
			_state = state;
			_ocDispatcher = ocDispatcher;
			_setSynchronizationContext = setSynchronizationContext;
			_context = context;
			_doneManualResetEvent = doneManualResetEvent;
			_callStackTrace = callStackTrace;
			_parent = parent;
		}

		internal void Do()
		{
			Invocation originalExecutingInvocation = _ocDispatcher._executingInvocation;
			SynchronizationContext originalSynchronizationContext = null;

			if (_setSynchronizationContext)
			{
				originalSynchronizationContext = SynchronizationContext.Current;
				SynchronizationContext.SetSynchronizationContext(
					new OcDispatcherSynchronizationContext(
						_ocDispatcher,
						_priority,
						_context,
						_parent));
			}

			_ocDispatcher._executingInvocation = this;

			_status = InvocationStatus.Executing;

			if (_action != null)
				_action();
			else
				_actionWithState(_state);

			_status = InvocationStatus.Done;

			if (_setSynchronizationContext)
				SynchronizationContext.SetSynchronizationContext(originalSynchronizationContext);

			_ocDispatcher._executingInvocation = originalExecutingInvocation;

			_doneManualResetEvent?.Set();
		}
	}

	public class InvocationResult<TValue> : IReadScalar<TValue>
	{
		public TValue Value
		{
			get => _value;
			internal set
			{
				_value = value;
				PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
			}
		}

		public Invocation Invocation => _invocation;

		private TValue _value;
		internal Invocation _invocation;

		public InvocationResult()
		{

		}

		public InvocationResult(Invocation invocation, TValue value)
		{
			_invocation = invocation;
			_value = value;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#region Overrides of Object

		public override string ToString()
		{
			return $"(ObservableComputations.InvocationResult<{typeof(TValue).Name}> (Value = '{_value.ToStringSafe(e => e.Message)}'))";
		}

		#endregion
	}

	public class OcDispatcher : IDisposable, IOcDispatcher, INotifyPropertyChanged
	{
		internal readonly ConcurrentQueue<Invocation>[] _invocationQueues;
		private readonly ManualResetEventSlim _newInvocationManualResetEvent = new ManualResetEventSlim(false);
		private bool _isRunning = true;
		private bool _isDisposed;
		internal readonly Thread _thread;
		readonly Thread _exceptionMonitoringThread;
		internal readonly int _managedThreadId;
		private NewInvocationBehaviour _newInvocationBehaviour;
		private OcDispatcherStatus _status;
		internal Invocation _executingInvocation;
		private Invocation _failedInvocation;

		public event EventHandler InvocationFailed; 

		public Invocation ExecutingInvocation => _executingInvocation;
		public Invocation FailedInvocation => _failedInvocation;

		public static OcDispatcher Current => 
			StaticInfo._ocDispatchers.TryGetValue(Thread.CurrentThread, out OcDispatcher ocDispatcher) 
				? ocDispatcher : null;

		public int PrioritiesNumber => _highestPriority + 1;

		public OcDispatcherStatus Status => _status;

		private readonly string _instantiatingStackTrace;
		public string InstantiatingStackTrace => _instantiatingStackTrace;
		private int _highestPriority;

		public NewInvocationBehaviour NewInvocationBehaviour
		{
			get => _newInvocationBehaviour;
			set
			{
				if (!_isRunning && value == NewInvocationBehaviour.Accept)
					throw new ObservableComputationsException("Disposing is not running");
				_newInvocationBehaviour = value;
			}
		}

		public int GetQueueCount(int priority = 0) => _invocationQueues[priority].Count;

		internal Invocation queueInvocation(Action action, int priority, object context,
			bool setSynchronizationContext, Invocation parent, ManualResetEventSlim doneManualResetEvent = null)
		{
			Invocation invocation = new Invocation(action, priority, this, setSynchronizationContext, parent, getCallStackTrace(), context, doneManualResetEvent);
			_invocationQueues[priority].Enqueue(invocation);
			_newInvocationManualResetEvent.Set();
			return invocation;
		}

		private static string getCallStackTrace()
		{
			string callStackTrace = null;
			if (Configuration.SaveOcDispatcherInvocationStackTrace)
				callStackTrace = Environment.StackTrace;
			return callStackTrace;
		}

		private Invocation queueInvocation(Action<object> action, int priority, object state, object context,
			bool setSynchronizationContext, Invocation parent, ManualResetEventSlim doneManualResetEvent = null)
		{
			Invocation invocation = new Invocation(action, priority, state, this, setSynchronizationContext, parent, getCallStackTrace(), context, doneManualResetEvent);
			_invocationQueues[priority].Enqueue(invocation);
			_newInvocationManualResetEvent.Set();
			return invocation;
		}

		public OcDispatcher(
			int prioritiesNumber = 1,
			ApartmentState threadApartmentState = ApartmentState.Unknown)
		{
			if (Configuration.SaveInstantiatingStackTrace)
				_instantiatingStackTrace = Environment.StackTrace;

			_highestPriority = prioritiesNumber - 1;


			_invocationQueues = new ConcurrentQueue<Invocation>[prioritiesNumber];

			for (int priority = 0; priority < prioritiesNumber; priority++)
				_invocationQueues[priority] = new ConcurrentQueue<Invocation>();

			_thread = new Thread(() =>
			{
				_exceptionMonitoringThread.Start();

				StaticInfo._ocDispatchers[Thread.CurrentThread] = this;

				while (_isRunning)
				{
					_newInvocationManualResetEvent.Wait();
					_newInvocationManualResetEvent.Reset();

					processQueues(null);
				}

				_status = OcDispatcherStatus.Disposed;
				_newInvocationManualResetEvent.Dispose();
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
				StaticInfo._ocDispatchers.TryRemove(Thread.CurrentThread, out _);
			});

			_exceptionMonitoringThread = new Thread(() =>
			{
				_thread.Join();
				if (_status == OcDispatcherStatus.RunOrWait)
				{
					_status = OcDispatcherStatus.InvocationFailed;

					if (_executingInvocation != null)
					{
						_executingInvocation._status = InvocationStatus.Failed;
						_failedInvocation = _executingInvocation;
						_executingInvocation = null;
					}

					_failedInvocation._doneManualResetEvent?.Set();

					InvocationFailed?.Invoke(this, null);
				}
			});

			_exceptionMonitoringThread.Name = "ObservableComputations.OcDispatcher failure monitoring";

			_managedThreadId = _thread.ManagedThreadId;
			_thread.SetApartmentState(threadApartmentState);
			_thread.Start();
		}

		private void processQueues(Func<int, Invocation, bool> stop)
		{
			bool processed = true;
			int count = 0;
			while (processed)
			{
				processed = false;
				int priority;
				for (priority = _highestPriority; priority >= 0; priority--)
					if (_invocationQueues[priority].TryDequeue(out Invocation invocation))
					{
						invocation.Do();
						count++;
						processed = stop == null || !stop(count, invocation);
						break;
					}
			}
		}

		private bool shouldCancelNewInvocation()
		{
			if (_newInvocationBehaviour == NewInvocationBehaviour.ThrowException)
				throw new ObservableComputationsException("New invocations is not allowed");

			return _newInvocationBehaviour == NewInvocationBehaviour.Cancel;
		}


		public string ThreadName
		{
			get => _thread.Name;
			set => _thread.Name = value;
		}

		public CultureInfo ThreadCurrentCulture
		{
			get => _thread.CurrentCulture;
			set => _thread.CurrentCulture = value;
		}

		public CultureInfo ThreadCurrentUICulture
		{
			get => _thread.CurrentUICulture;
			set => _thread.CurrentUICulture = value;
		}

		public bool ThreadIsAlive => _thread.IsAlive;

		public ExecutionContext ThreadExecutionContext => _thread.ExecutionContext;

		public bool ThreadIsBackground
		{
			get => _thread.IsBackground;
			set => _thread.IsBackground = value;
		}

		public int ManagedThreadId => _managedThreadId;

		public ThreadPriority ThreadPriority
		{
			get => _thread.Priority;
			set => _thread.Priority = value;
		}

		public ThreadState ThreadState => _thread.ThreadState;

		public void DisableThreadComObjectEagerCleanup()
		{
			_thread.DisableComObjectEagerCleanup();
		}

		public ApartmentState GetThreadApartmentState()
		{
			return _thread.GetApartmentState();
		}

		public void ClearQueues()
		{
			for (int index = 0; index < _invocationQueues.Length; index++)
			{
				ConcurrentQueue<Invocation> invocationQueue = _invocationQueues[index];
				while (invocationQueue.TryDequeue(out Invocation invocation))
				{
					invocation._status = InvocationStatus.Canceled;
					invocation._doneManualResetEvent?.Set();
				}
			}
		}

		public TimeSpan DoOthers(TimeSpan timeSpan)
		{
			verifyCurrentThread();

			Stopwatch stopwatch = Stopwatch.StartNew();
			processQueues((count, invocation) => stopwatch.ElapsedTicks > timeSpan.Ticks);
			stopwatch.Stop();
			return timeSpan - TimeSpan.FromTicks(stopwatch.ElapsedTicks);
		}

		public int DoOthers(int targetCount)
		{
			verifyCurrentThread();

			int factCount = 0;
			processQueues(
				(count, invocation) =>
				{
					factCount = count;
					return targetCount == count;
				});

			return targetCount - factCount;
		}

		public void DoOthers(Func<int, Invocation, bool> stop)
		{
			verifyCurrentThread();
			processQueues(stop);
		}

		public void DoOthers()
		{
			verifyCurrentThread();
			processQueues(null);
		}

		public bool CheckAccess() => Thread.CurrentThread == _thread;

		private void verifyCurrentThread()
		{
			if (_thread != Thread.CurrentThread)
				throw new ObservableComputationsException(
					"OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");
		}

		public void Dispose(NewInvocationBehaviour newInvocationBehaviour)
		{
			_isRunning = false;
			_status = OcDispatcherStatus.Disposing;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
			_newInvocationBehaviour = newInvocationBehaviour;
			ClearQueues();
			_newInvocationManualResetEvent.Set();
		}

		public void Dispose()
		{
			Dispose(NewInvocationBehaviour.Cancel);
		}

		private static object _passContext = new object();
		public static object PassContext => _passContext;

		public void Pass(int priority = 0)
		{
			Invoke(() => {}, priority, _passContext);
		}

		void IOcDispatcher.Invoke(Action action, int priority, object parameter, object context)
		{
			if (shouldCancelNewInvocation()) return;

			queueInvocation(action, priority, context, false, getExecutingInvocation());
		}

		public Invocation Invoke(Action action, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			Invocation executingInvocation = getExecutingInvocation();

			if (shouldCancelNewInvocation())
				return getCanceledInvocation(action, priority, context, setSynchronizationContext, executingInvocation);

			Invocation resultInvocation;

			if (executingInvocation != null && executingInvocation == _executingInvocation)
			{
				resultInvocation = queueInvocation(action, priority, context, setSynchronizationContext, executingInvocation);
				processQueues((count, invocation) => invocation._status == InvocationStatus.Done);
				return resultInvocation;
			}

			return invokeInOutsideThread(action, priority, context, executingInvocation, setSynchronizationContext);
		}

		private Invocation getCanceledInvocation(Action action, int priority, object context, bool setSynchronizationContext,
			Invocation executingInvocation)
		{
			Invocation canceledInvocation = new Invocation(action, priority, this, setSynchronizationContext,
				executingInvocation, getCallStackTrace(), context);
			canceledInvocation._status = InvocationStatus.Canceled;
			return canceledInvocation;
		}

		internal Invocation invokeInOutsideThread(Action action, int priority, object context, Invocation executingInvocation, bool setSynchronizationContext)
		{
			Invocation resultInvocation;
			ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

			resultInvocation = queueInvocation(action, priority, context, setSynchronizationContext, executingInvocation, manualResetEvent);
			manualResetEvent.Wait();
			manualResetEvent.Dispose();

			if (resultInvocation._status == InvocationStatus.Failed)
				throw new OcDispatcherInvocationFailedException(this, resultInvocation);

			return resultInvocation;
		}

		private Invocation getExecutingInvocation() =>
			Thread.CurrentThread == _thread 
				? _executingInvocation 
				: Current?._executingInvocation;

		public Invocation Invoke(Action<object> action, object state, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			Invocation executingInvocation = getExecutingInvocation();

			if (shouldCancelNewInvocation())
				return getCanceledInvocation(action, state, priority, context, setSynchronizationContext,
					executingInvocation);

			Invocation resultInvocation;

			if (executingInvocation != null && executingInvocation == _executingInvocation)
			{
				resultInvocation = queueInvocation(action, priority, state, context, setSynchronizationContext, executingInvocation);
				processQueues((count, invocation) => invocation._status == InvocationStatus.Done);
				return resultInvocation;
			}

			return invokeInOutsideThread(action, state, priority, context, executingInvocation, setSynchronizationContext);
		}

		private Invocation getCanceledInvocation(Action<object> action, object state, int priority, object context,
			bool setSynchronizationContext, Invocation executingInvocation)
		{
			Invocation canceledInvocation = new Invocation(action, priority, state, this, setSynchronizationContext,
				executingInvocation, getCallStackTrace(), context);
			canceledInvocation._status = InvocationStatus.Canceled;
			return canceledInvocation;
		}

		private Invocation invokeInOutsideThread(Action<object> action, object state, int priority, object context,
			Invocation executingInvocation, bool setSynchronizationContext)
		{
			Invocation resultInvocation;
			ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

			resultInvocation = queueInvocation(action, priority, state, context, setSynchronizationContext, executingInvocation,
				doneManualResetEvent: manualResetEvent);
			manualResetEvent.Wait();
			manualResetEvent.Dispose();

			if (resultInvocation._status == InvocationStatus.Failed)
				throw new OcDispatcherInvocationFailedException(this, resultInvocation);

			return resultInvocation;
		}

		public InvocationResult<TResult> Invoke<TResult>(Func<TResult> func, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			TResult result = default;
			Invocation invocation = Invoke(() => { result = func(); }, priority, context, setSynchronizationContext);
			return invocation != null ? new InvocationResult<TResult>(invocation, result) : null;
		}

		public InvocationResult<TResult> Invoke<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			TResult result = default;
			Invocation invocation = Invoke(s => { result = func(s); }, state, priority, context, setSynchronizationContext);
			return invocation != null ? new InvocationResult<TResult>(invocation, result) : null;
		}

		public Invocation InvokeAsync(Action action, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			Invocation executingInvocation = getExecutingInvocation();

			if (shouldCancelNewInvocation())
			{
				Invocation canceledInvocation = new Invocation(action, priority, this, setSynchronizationContext, executingInvocation, getCallStackTrace(), context);
				canceledInvocation._status = InvocationStatus.Canceled;
				return  canceledInvocation;
			}

			return queueInvocation(
				action, 
				priority, 
				context, setSynchronizationContext,
				executingInvocation);
		}

		public Invocation InvokeAsync(Action<object> action, object state, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			Invocation executingInvocation = getExecutingInvocation();

			if (shouldCancelNewInvocation())
			{
				Invocation canceledInvocation = new Invocation(action, priority, state, this, setSynchronizationContext, executingInvocation, getCallStackTrace(), context);
				canceledInvocation._status = InvocationStatus.Canceled;
				return  canceledInvocation;
			}

			return queueInvocation(
				action, 
				priority,  
				state, 
				context, setSynchronizationContext,
				executingInvocation);						
		}

		public Task InvokeAsyncAwaitable(Action action, int priority = 0, object context = null, bool setSynchronizationContext = false, CancellationToken cancellationToken = new CancellationToken())
		{
			if (shouldCancelNewInvocation()) return Task.FromCanceled(CancellationToken.None);

			Invocation executingInvocation = getExecutingInvocation();

			return Task.Run(() => invokeInOutsideThread(action, priority, context, executingInvocation, setSynchronizationContext), cancellationToken);
		}

		public Task InvokeAsyncAwaitable(Action<object> action, object state, int priority = 0, object context = null, bool setSynchronizationContext = false, CancellationToken cancellationToken = new CancellationToken())
		{
			if (shouldCancelNewInvocation()) return Task.FromCanceled(CancellationToken.None);

			Invocation executingInvocation = getExecutingInvocation();

			return Task.Run(() => invokeInOutsideThread(action, state, priority, context, executingInvocation, setSynchronizationContext), cancellationToken);
		}

		public InvocationResult<TResult> InvokeAsync<TResult>(Func<TResult> func, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			if (shouldCancelNewInvocation()) return null;

			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			invocationResult._invocation = InvokeAsync(() => { invocationResult.Value = func(); }, priority, context, setSynchronizationContext);
			return invocationResult;
		}

		public InvocationResult<TResult> InvokeAsync<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			if (shouldCancelNewInvocation()) return null;

			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			invocationResult._invocation = InvokeAsync(s => { invocationResult.Value = func(s); }, state, priority, context, setSynchronizationContext);
			return invocationResult;
		}

		public Task<TResult> InvokeAsyncAwaitable<TResult>(Func<TResult> func, int priority = 0, object context = null, bool setSynchronizationContext = false, CancellationToken cancellationToken = new CancellationToken())
		{
			if (shouldCancelNewInvocation()) return Task.FromCanceled<TResult>(CancellationToken.None);

			Invocation executingInvocation = getExecutingInvocation();

			return Task.Run(() =>
			{
				TResult result = default;
				invokeInOutsideThread(() => result = func(), priority, context, executingInvocation, setSynchronizationContext);
				return result;
			}, cancellationToken);
		}

		public Task<TResult> InvokeAsyncAwaitable<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null, bool setSynchronizationContext = false, CancellationToken cancellationToken = new CancellationToken())
		{
			if (shouldCancelNewInvocation()) return Task.FromCanceled<TResult>(CancellationToken.None);

			Invocation executingInvocation = getExecutingInvocation();

			return Task.Run(() =>
			{
				TResult result = default;
				invokeInOutsideThread(s => result = func(s), state, priority, context, executingInvocation, setSynchronizationContext);
				return result;
			}, cancellationToken);
		}

		#region Overrides of Object

		public override string ToString()
		{
			return $"(ObservableComputations.OcDispatcher (Thread.Name = '{_thread.Name}'))";
		}

		#endregion

		#region Implementation of INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}

	internal class OcDispatcherInvocationFailedException : ObservableComputationsException
	{
		OcDispatcher _dispatcher;
		Invocation _invocation;

		public OcDispatcher Dispatcher => _dispatcher;

		public Invocation Invocation => _invocation;

		public OcDispatcherInvocationFailedException(OcDispatcher dispatcher, Invocation invocation) : base("Invocation failed")
		{
			_dispatcher = dispatcher;
			_invocation = invocation;
		}
	}

	public enum NewInvocationBehaviour
	{
		Accept,
		Cancel,
		ThrowException
	}

	public enum OcDispatcherStatus
	{
		RunOrWait,
		InvocationFailed,
		Disposing,
		Disposed
	}

	public enum InvocationStatus
	{
		Queued,
		Executing,
		Done,
		Failed,
		Canceled
	}

	public class OcDispatcherSynchronizationContext : SynchronizationContext
	{
		public OcDispatcher OcDispatcher => _ocDispatcher;

		public int Priority => _priority;

		public object Context => _context;

		public Invocation ParentInvocation => _parentInvocation;

		OcDispatcher _ocDispatcher;
		private int _priority;
		private object _context;
		private Invocation _parentInvocation;

		public OcDispatcherSynchronizationContext(OcDispatcher ocDispatcher, int priority, object context, Invocation parentInvocation)
		{
			_ocDispatcher = ocDispatcher;
			_priority = priority;
			_context = context;
			_parentInvocation = parentInvocation;
		}

		public override void Post(SendOrPostCallback postCallback, object state)
		{
			_ocDispatcher.queueInvocation(
				() => postCallback(state), 
				_priority, 
				_context,
				false,
				_parentInvocation);
		}
	}
}

