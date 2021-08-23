// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
		public string InstantiationStackTrace => _instantiationStackTrace;
		public string ExecutionStackTrace => _executionStackTrace;
		public object Context => _context;
		public OcDispatcher OcDispatcher => _ocDispatcher;
		public InvocationStatus Status => _status;

		public Invocation Parent => _parent;
		public Invocation Executor => _executor;
		public bool SetSynchronizationContext => _setSynchronizationContext;

		private readonly Action _action;
		private readonly Action<object> _actionWithState;
		private readonly object _state;
		private readonly string _instantiationStackTrace;
		private string _executionStackTrace;
		internal readonly object _context;
		private readonly OcDispatcher _ocDispatcher;
		internal int _priority;
		internal InvocationStatus _status;
		internal readonly ManualResetEventSlim _doneManualResetEvent;
		private Invocation _parent;
		private Invocation _executor;
		private bool _setSynchronizationContext;

		internal Invocation(Action action, int priority, OcDispatcher ocDispatcher, 
			bool setSynchronizationContext, Invocation parent, string instantiationStackTrace, object context = null, 
			ManualResetEventSlim doneManualResetEvent = null)
		{
			_action = action;
			_priority = priority;
			_ocDispatcher = ocDispatcher;
			_setSynchronizationContext = setSynchronizationContext;
			_context = context;
			_doneManualResetEvent = doneManualResetEvent;
			_instantiationStackTrace = instantiationStackTrace;
			_parent = parent;
		}

		internal Invocation(Action<object> actionWithState, int priority, object state, OcDispatcher ocDispatcher, 
			bool setSynchronizationContext, Invocation parent, string instantiationStackTrace, object context = null,
			ManualResetEventSlim doneManualResetEvent = null)
		{
			_actionWithState = actionWithState;
			_priority = priority;
			_state = state;
			_ocDispatcher = ocDispatcher;
			_setSynchronizationContext = setSynchronizationContext;
			_context = context;
			_doneManualResetEvent = doneManualResetEvent;
			_instantiationStackTrace = instantiationStackTrace;
			_parent = parent;
		}

		internal void Execute()
		{
			Invocation originalCurrentInvocation = _ocDispatcher._currentInvocation;
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

			if (OcConfiguration.SaveOcDispatcherInvocationExecutionStackTrace)
				_executionStackTrace = Environment.StackTrace;

			_executor = originalCurrentInvocation;
			_ocDispatcher._currentInvocation = this;

			_status = InvocationStatus.Executing;

			if (_action != null)
				_action();
			else
				_actionWithState(_state);

			_status = InvocationStatus.Executed;

			if (_setSynchronizationContext)
				SynchronizationContext.SetSynchronizationContext(originalSynchronizationContext);

			_ocDispatcher._currentInvocation = originalCurrentInvocation;

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
		internal readonly Thread _thread;
		internal readonly int _managedThreadId;
		private NewInvocationBehaviour _newInvocationBehaviour;
		private OcDispatcherStatus _status;
		internal Invocation _currentInvocation;

		/// <summary>
		/// The invocation that is being executed now
		/// </summary>
		public Invocation CurrentInvocation => _currentInvocation;

		/// <summary>
		/// OcDispatcher associated with the current thread
		/// </summary>
		public static OcDispatcher Current => 
			StaticInfo._ocDispatchers.TryGetValue(Thread.CurrentThread.ManagedThreadId, out OcDispatcher ocDispatcher) 
				? ocDispatcher : null;

		public int PrioritiesNumber => _highestPriority + 1;

		public OcDispatcherStatus Status => _status;

		private readonly string _instantiationStackTrace;
		public string InstantiationStackTrace => _instantiationStackTrace;
		private int _highestPriority;

		public NewInvocationBehaviour NewInvocationBehaviour
		{
			get => _newInvocationBehaviour;
			set
			{
				if (!_isRunning && value == NewInvocationBehaviour.Accept)
					throw new ObservableComputationsException("The disposal is in progress");
				_newInvocationBehaviour = value;
			}
		}

		public int GetQueueCount(int? priority = null) => 
			priority == null 
				? _invocationQueues.Sum(q => q.Count) 
				: _invocationQueues[priority.Value].Count;

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
			if (OcConfiguration.SaveOcDispatcherInvocationInstantiationStackTrace)
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
			if (OcConfiguration.SaveInstantiationStackTrace)
				_instantiationStackTrace = Environment.StackTrace;

			_highestPriority = prioritiesNumber - 1;


			_invocationQueues = new ConcurrentQueue<Invocation>[prioritiesNumber];

			for (int priority = 0; priority < prioritiesNumber; priority++)
				_invocationQueues[priority] = new ConcurrentQueue<Invocation>();

			_thread = new Thread(() =>
			{
				StaticInfo._ocDispatchers[Thread.CurrentThread.ManagedThreadId] = this;

				while (_isRunning)
				{
					_newInvocationManualResetEvent.Wait();
					_newInvocationManualResetEvent.Reset();

					processQueues(null);
				}

				_status = OcDispatcherStatus.Disposed;
				_newInvocationManualResetEvent.Dispose();
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
				StaticInfo._ocDispatchers.TryRemove(Thread.CurrentThread.ManagedThreadId, out _);
			});

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
						invocation.Execute();
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

		public TimeSpan ExecuteOtherInvocations(TimeSpan timeSpan)
		{
			verifyCurrentThread();

			Stopwatch stopwatch = Stopwatch.StartNew();
			processQueues((count, invocation) => stopwatch.ElapsedTicks > timeSpan.Ticks);
			stopwatch.Stop();
			return timeSpan - TimeSpan.FromTicks(stopwatch.ElapsedTicks);
		}

		public int ExecuteOtherInvocations(int targetCount)
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

		public void ExecuteOtherInvocations(Func<int, Invocation, bool> stop)
		{
			verifyCurrentThread();
			processQueues(stop);
		}

		public void ExecuteOtherInvocations()
		{
			verifyCurrentThread();
			processQueues(null);
		}

		public bool CheckAccess() => Thread.CurrentThread == _thread;

		private void verifyCurrentThread()
		{
			if (_thread != Thread.CurrentThread)
				throw new ObservableComputationsException(
					"OcDispatcher.ExecuteOtherInvocations method can only be called from the thread that is associated with this OcDispatcher.");
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

			queueInvocation(action, priority, context, false, getCurrentInvocation());
		}

		public Invocation Invoke(Action action, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			Invocation currentInvocation = getCurrentInvocation();

			if (shouldCancelNewInvocation())
				return getCanceledInvocation(action, priority, context, setSynchronizationContext, currentInvocation);

			Invocation resultInvocation;

			if (currentInvocation != null && currentInvocation == _currentInvocation)
			{
				resultInvocation = queueInvocation(action, priority, context, setSynchronizationContext, currentInvocation);
				processQueues((count, invocation) => invocation._status == InvocationStatus.Executed);
				return resultInvocation;
			}

			return invokeInOutsideThread(action, priority, context, currentInvocation, setSynchronizationContext);
		}

		private Invocation getCanceledInvocation(Action action, int priority, object context, bool setSynchronizationContext,
			Invocation currentInvocation)
		{
			Invocation canceledInvocation = new Invocation(action, priority, this, setSynchronizationContext,
				currentInvocation, getCallStackTrace(), context);
			canceledInvocation._status = InvocationStatus.Canceled;
			return canceledInvocation;
		}

		internal Invocation invokeInOutsideThread(Action action, int priority, object context, Invocation currentInvocation, bool setSynchronizationContext)
		{
			Invocation resultInvocation;
			ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

			resultInvocation = queueInvocation(action, priority, context, setSynchronizationContext, currentInvocation, manualResetEvent);
			manualResetEvent.Wait();
			manualResetEvent.Dispose();

			return resultInvocation;
		}

		private Invocation getCurrentInvocation() =>
			Thread.CurrentThread == _thread 
				? _currentInvocation 
				: Current?._currentInvocation;

		public Invocation Invoke(Action<object> action, object state, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			Invocation currentInvocation = getCurrentInvocation();

			if (shouldCancelNewInvocation())
				return getCanceledInvocation(action, state, priority, context, setSynchronizationContext,
					currentInvocation);

			Invocation resultInvocation;

			if (currentInvocation != null && currentInvocation == _currentInvocation)
			{
				resultInvocation = queueInvocation(action, priority, state, context, setSynchronizationContext, currentInvocation);
				processQueues((count, invocation) => invocation._status == InvocationStatus.Executed);
				return resultInvocation;
			}

			return invokeInOutsideThread(action, state, priority, context, currentInvocation, setSynchronizationContext);
		}

		private Invocation getCanceledInvocation(Action<object> action, object state, int priority, object context,
			bool setSynchronizationContext, Invocation currentInvocation)
		{
			Invocation canceledInvocation = new Invocation(action, priority, state, this, setSynchronizationContext,
				currentInvocation, getCallStackTrace(), context);
			canceledInvocation._status = InvocationStatus.Canceled;
			return canceledInvocation;
		}

		private Invocation invokeInOutsideThread(Action<object> action, object state, int priority, object context,
			Invocation currentInvocation, bool setSynchronizationContext)
		{
			Invocation resultInvocation;
			ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

			resultInvocation = queueInvocation(action, priority, state, context, setSynchronizationContext, currentInvocation,
				doneManualResetEvent: manualResetEvent);
			manualResetEvent.Wait();
			manualResetEvent.Dispose();

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
			Invocation currentInvocation = getCurrentInvocation();

			if (shouldCancelNewInvocation())
			{
				Invocation canceledInvocation = new Invocation(action, priority, this, setSynchronizationContext, currentInvocation, getCallStackTrace(), context);
				canceledInvocation._status = InvocationStatus.Canceled;
				return  canceledInvocation;
			}

			return queueInvocation(
				action, 
				priority, 
				context, setSynchronizationContext,
				currentInvocation);
		}

		public Invocation InvokeAsync(Action<object> action, object state, int priority = 0, object context = null, bool setSynchronizationContext = false)
		{
			Invocation currentInvocation = getCurrentInvocation();

			if (shouldCancelNewInvocation())
			{
				Invocation canceledInvocation = new Invocation(action, priority, state, this, setSynchronizationContext, currentInvocation, getCallStackTrace(), context);
				canceledInvocation._status = InvocationStatus.Canceled;
				return  canceledInvocation;
			}

			return queueInvocation(
				action, 
				priority,  
				state, 
				context, setSynchronizationContext,
				currentInvocation);						
		}

		public Task InvokeAsyncAwaitable(Action action, int priority = 0, object context = null, bool setSynchronizationContext = false, CancellationToken cancellationToken = new CancellationToken())
		{
			if (shouldCancelNewInvocation()) return Task.FromCanceled(new CancellationToken(true));

			Invocation currentInvocation = getCurrentInvocation();

			return Task.Run(() => invokeInOutsideThread(action, priority, context, currentInvocation, setSynchronizationContext), cancellationToken);
		}

		public Task InvokeAsyncAwaitable(Action<object> action, object state, int priority = 0, object context = null, bool setSynchronizationContext = false, CancellationToken cancellationToken = new CancellationToken())
		{
			if (shouldCancelNewInvocation()) return Task.FromCanceled(new CancellationToken(true));

			Invocation currentInvocation = getCurrentInvocation();

			return Task.Run(() => invokeInOutsideThread(action, state, priority, context, currentInvocation, setSynchronizationContext), cancellationToken);
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
			if (shouldCancelNewInvocation()) return Task.FromCanceled<TResult>(new CancellationToken(true));

			Invocation currentInvocation = getCurrentInvocation();

			return Task.Run(() =>
			{
				TResult result = default;
				invokeInOutsideThread(() => result = func(), priority, context, currentInvocation, setSynchronizationContext);
				return result;
			}, cancellationToken);
		}

		public Task<TResult> InvokeAsyncAwaitable<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null, bool setSynchronizationContext = false, CancellationToken cancellationToken = new CancellationToken())
		{
			if (shouldCancelNewInvocation()) return Task.FromCanceled<TResult>(new CancellationToken(true));

			Invocation executingInvocation = getCurrentInvocation();

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

	public enum NewInvocationBehaviour
	{
		Accept,
		Cancel,
		ThrowException
	}

	public enum OcDispatcherStatus
	{
		/// <summary>
		/// Executing an invocation or wait for new invocations
		/// </summary>
		ExecutingOrWait,

		/// <summary>
		/// Disposal is in progress
		/// </summary>
		Disposing,

		/// <summary>
		/// Disposal is finished
		/// </summary>
		Disposed
	}

	public enum InvocationStatus
	{
		/// <summary>
		/// Execution is queued
		/// </summary>
		Invoked,

		/// <summary>
		/// Execution is in progress
		/// </summary>
		Executing,

		/// <summary>
		/// Execution done
		/// </summary>
		Executed,

		/// <summary>
		/// Execution canceled
		/// </summary>
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

