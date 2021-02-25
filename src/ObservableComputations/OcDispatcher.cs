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
		public bool Done => _done;
		public Invocation Parent => _parent;

		private readonly Action _action;
		private readonly Action<object> _actionWithState;
		private readonly object _state;
		private readonly string _callStackTrace;
		internal readonly object _context;
		private readonly OcDispatcher _ocDispatcher;
		internal int _priority;
		internal bool _done;
		internal readonly ManualResetEventSlim _doneManualResetEvent;
		private Invocation _parent;

		internal Invocation(Action action, int priority, OcDispatcher ocDispatcher, Invocation parent, string callStackTrace, object context = null, ManualResetEventSlim doneManualResetEvent = null)
		{
			_action = action;
			_priority = priority;
			_ocDispatcher = ocDispatcher;
			_context = context;
			_doneManualResetEvent = doneManualResetEvent;
			_callStackTrace = callStackTrace;
			_parent = parent;
		}

		internal Invocation(Action<object> actionWithState, int priority, object state, OcDispatcher ocDispatcher,
			Invocation parent, string callStackTrace, object context = null,
			ManualResetEventSlim doneManualResetEvent = null)
		{
			_actionWithState = actionWithState;
			_priority = priority;
			_state = state;
			_ocDispatcher = ocDispatcher;
			_context = context;
			_doneManualResetEvent = doneManualResetEvent;
			_callStackTrace = callStackTrace;
			_parent = parent;
		}

		internal void Do()
		{
			Invocation originalExecutingInvocation = _ocDispatcher._executingInvocation;
			_ocDispatcher._executingInvocation = this;
			Console.WriteLine($" --> executingInvocation = {this.GetHashCode()}");

			if (_action != null)
				_action();
			else
				_actionWithState(_state);

			_ocDispatcher._executingInvocation = originalExecutingInvocation;
			Console.WriteLine($"<-- _executingInvocation = {(originalExecutingInvocation == null ? "null" : originalExecutingInvocation.GetHashCode().ToString())}");

			_done = true; 
			
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
				_ripe = true;
				PropertyChanged?.Invoke(this, Utils.ValuePropertyChangedEventArgs);
				PropertyChanged?.Invoke(this, Utils.RipePropertyChangedEventArgs);
			}
		}

		public bool Ripe => _ripe;

		private TValue _value;
		private bool _ripe;
		internal Invocation _invocation;

		public InvocationResult()
		{

		}

		public InvocationResult(Invocation invocation)
		{
			_invocation = invocation;
		}

		public InvocationResult(Invocation invocation, TValue value)
		{
			_invocation = invocation;
			_value = value;
			_ripe = true;
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
		internal readonly int _managedThreadId;
		private NewInvocationBehaviour _newInvocationBehaviour;
		private OcDispatcherState _state;
		private OcDispatcherSynchronizationContext _synchronizationContext;
		internal Invocation _executingInvocation;

		[ThreadStatic]
		internal static int _ContinuationPriority;

		[ThreadStatic]
		internal static object _ContinuationContext;

		[ThreadStatic]
		internal static Invocation _ContinuationExecutingInvocation;

		public Invocation ExecutingInvocation => _executingInvocation;

		public static OcDispatcher Current => 
			StaticInfo._ocDispatchers.TryGetValue(Thread.CurrentThread, out OcDispatcher ocDispatcher) 
				? ocDispatcher : null;

		public int PrioritiesNumber => _highestPriority + 1;

		public OcDispatcherState State =>
			_state == OcDispatcherState.RunOrWait
			&& _thread.ThreadState != ThreadState.Running
			&& _thread.ThreadState != ThreadState.WaitSleepJoin
				? OcDispatcherState.Failure
				: _state;

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

		private Invocation queueInvocation(Action action, int priority, object context,
			Invocation parent, ManualResetEventSlim doneManualResetEvent = null)
		{
			Invocation invocation = new Invocation(action, priority, this, parent, getCallStackTrace(), context, doneManualResetEvent);
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
			Invocation parent, ManualResetEventSlim doneManualResetEvent = null)
		{
			Invocation invocation = new Invocation(action, priority, state, this, parent, getCallStackTrace(), context, doneManualResetEvent);
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

			_synchronizationContext = new OcDispatcherSynchronizationContext(this);

			_highestPriority = prioritiesNumber - 1;


			_invocationQueues = new ConcurrentQueue<Invocation>[prioritiesNumber];

			for (int priority = 0; priority < prioritiesNumber; priority++)
				_invocationQueues[priority] = new ConcurrentQueue<Invocation>();

			_thread = new Thread(() =>
			{
				SynchronizationContext.SetSynchronizationContext(_synchronizationContext);
				StaticInfo._ocDispatchers[Thread.CurrentThread] = this;

				while (_isRunning)
				{
					_newInvocationManualResetEvent.Wait();
					_newInvocationManualResetEvent.Reset();

					processQueues(null);
				}

				_state = OcDispatcherState.Disposed;
				_newInvocationManualResetEvent.Dispose();
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
				StaticInfo._ocDispatchers.TryRemove(Thread.CurrentThread, out _);
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
						int originalPrioity = _synchronizationContext._priority;
						_synchronizationContext._priority = priority;
						invocation.Do();
						_synchronizationContext._priority = originalPrioity;
						count++;
						processed = stop == null || !stop(count, invocation);
						break;
					}
			}
		}

		private bool cannotAcceptNewInvocation()
		{
			if (_newInvocationBehaviour == NewInvocationBehaviour.ThrowException)
				throw new ObservableComputationsException("New invocations is not allowed");

			return _newInvocationBehaviour == NewInvocationBehaviour.Ignore;
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
					invocation._done = true;
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
			_state = OcDispatcherState.Disposing;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
			_newInvocationBehaviour = newInvocationBehaviour;
			ClearQueues();
			_newInvocationManualResetEvent.Set();
		}

		public void Dispose()
		{
			Dispose(NewInvocationBehaviour.Ignore);
		}

		private static object _passContext = new object();
		public static object PassContext => _passContext;

		public void Pass(int priority = 0)
		{
			Invoke(() => {}, priority, _passContext);
		}

		private static void verifyContinuationParameters(int? continuationPriority, object continuationContext)
		{
			if (continuationPriority != null || continuationContext != null)
				throw new ObservableComputationsException(
					$"The parameters {nameof(continuationPriority)} and {nameof(continuationContext)} only make sense  when you call InvokeAsyncAwaitable in this dispatcher thread");
		}

		void IOcDispatcher.Invoke(Action action, int priority, object parameter, object context)
		{
			if (cannotAcceptNewInvocation()) return;

			queueInvocation(action, priority, context, null);
		}

		public Invocation Invoke(Action action, int priority = 0, object context = null)
		{
			if (cannotAcceptNewInvocation()) return null;
			Invocation executingInvocation = getExecutingInvocation();
			return invoke(action, priority, context, executingInvocation);
		}

		internal Invocation invoke(Action action, int priority, object context, Invocation executingInvocation)
		{
			Invocation resultInvocation;

			if (executingInvocation != null && executingInvocation == _executingInvocation)
			{
				resultInvocation = queueInvocation(action, priority, context, executingInvocation);
				processQueues((count, invocation) => invocation._done);
				return resultInvocation;
			}

			ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

			resultInvocation = queueInvocation(action, priority, context, executingInvocation, manualResetEvent);
			manualResetEvent.Wait();
			manualResetEvent.Dispose();
			return resultInvocation;
		}

		private Invocation getExecutingInvocation() =>
			Thread.CurrentThread == _thread 
				? _executingInvocation 
				: Current?._executingInvocation;

		public Invocation Invoke(Action<object> action, object state, int priority = 0, object context = null)
		{
			if (cannotAcceptNewInvocation()) return null;
			Invocation executingInvocation = getExecutingInvocation();
			return invoke(action, state, priority, context, executingInvocation);
		}

		private Invocation invoke(Action<object> action, object state, int priority, object context, Invocation executingInvocation)
		{
			Invocation resultInvocation;

			if (executingInvocation != null && executingInvocation == _executingInvocation)
			{
				resultInvocation = queueInvocation(action, priority, state, context, executingInvocation);
				processQueues((count, invocation) => invocation._done);
				return resultInvocation;
			}

			ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

			resultInvocation = queueInvocation(action, priority, state, context, executingInvocation,
				doneManualResetEvent: manualResetEvent);
			manualResetEvent.Wait();
			manualResetEvent.Dispose();

			return resultInvocation;
		}

		public InvocationResult<TResult> Invoke<TResult>(Func<TResult> func, int priority = 0, object context = null)
		{
			TResult result = default;
			Invocation invocation = Invoke(() => { result = func(); }, priority, context);
			return invocation != null ? new InvocationResult<TResult>(invocation, result) : null;
		}

		public InvocationResult<TResult> Invoke<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null)
		{
			TResult result = default;
			Invocation invocation = Invoke(s => { result = func(s); }, state, priority, context);
			return invocation != null ? new InvocationResult<TResult>(invocation, result) : null;
		}

		public Invocation InvokeAsync(Action action, int priority = 0, object context = null)
		{
			if (cannotAcceptNewInvocation()) return null;

			return queueInvocation(
				action, 
				priority, 
				context,
				getExecutingInvocation());
		}

		public Invocation InvokeAsync(Action<object> action, object state, int priority = 0, object context = null)
		{
			if (cannotAcceptNewInvocation()) return null;

			return queueInvocation(
				action, 
				priority,  
				state, 
				context,
				getExecutingInvocation());						
		}

		public Task InvokeAsyncAwaitable(Action action, int priority = 0, object context = null, int? continuationPriority = null, object continuationContext = null, CancellationToken cancellationToken = new CancellationToken())
		{
			if (cannotAcceptNewInvocation()) return Task.CompletedTask;

			Invocation executingInvocation = getExecutingInvocation();

			if (executingInvocation != null && executingInvocation == _executingInvocation)
			{
				return Task.Run(() =>
				{
					_ContinuationPriority = continuationPriority ?? executingInvocation._priority;
					_ContinuationContext = continuationContext ?? executingInvocation._context;
					_ContinuationExecutingInvocation = executingInvocation.Parent;

					invoke(action, priority, context, executingInvocation);
				}, cancellationToken);
			}

			verifyContinuationParameters(continuationPriority, continuationContext);
			return Task.Run(() => invoke(action, priority, context, executingInvocation), cancellationToken);
		}

		public Task InvokeAsyncAwaitable(Action<object> action, object state, int priority = 0, object context = null, int? continuationPriority = null, object continuationContext = null, CancellationToken cancellationToken = new CancellationToken())
		{
			if (cannotAcceptNewInvocation()) return Task.CompletedTask;

			Invocation executingInvocation = getExecutingInvocation();

			if (executingInvocation != null && executingInvocation == _executingInvocation)
			{
				return Task.Run(() =>
				{
					_ContinuationPriority = continuationPriority ?? executingInvocation._priority;
					_ContinuationContext = continuationContext ?? executingInvocation._context;
					_ContinuationExecutingInvocation = executingInvocation.Parent;

					Invoke(action, state, priority, context);
				}, cancellationToken);
			}

			verifyContinuationParameters(continuationPriority, continuationContext);
			return Task.Run(() => Invoke(action, state, priority, context), cancellationToken);
		}

		public InvocationResult<TResult> InvokeAsync<TResult>(Func<TResult> func, int priority = 0, object context = null)
		{
			if (cannotAcceptNewInvocation()) return null;

			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			invocationResult._invocation = InvokeAsync(() => { invocationResult.Value = func(); }, priority, context);
			return invocationResult;
		}

		public InvocationResult<TResult> InvokeAsync<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null)
		{
			if (cannotAcceptNewInvocation()) return null;

			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			invocationResult._invocation = InvokeAsync(s => { invocationResult.Value = func(s); }, state, priority, context);
			return invocationResult;
		}

		public Task<TResult> InvokeAsyncAwaitable<TResult>(Func<TResult> func, int priority = 0, object context = null, int? continuationPriority = null, object continuationContext = null, CancellationToken cancellationToken = new CancellationToken())
		{
			if (cannotAcceptNewInvocation()) return Task.FromResult(default(TResult));

			Invocation executingInvocation = getExecutingInvocation();

			if (executingInvocation != null && executingInvocation == _executingInvocation)
			{
				return Task.Run(() =>
				{
					_ContinuationPriority = continuationPriority ?? executingInvocation._priority;
					_ContinuationContext = continuationContext ?? executingInvocation._context;
					_ContinuationExecutingInvocation = executingInvocation.Parent;

					TResult result = default;
					invoke(() => result = func(), priority, context, executingInvocation);
					return result;
				}, cancellationToken);
			}

			verifyContinuationParameters(continuationPriority, continuationContext);
			return Task.Run(() =>
			{
				TResult result = default;
				invoke(() => result = func(), priority, context, executingInvocation);
				return result;
			}, cancellationToken);
		}

		public Task<TResult> InvokeAsyncAwaitable<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null, int? continuationPriority = null, object continuationContext = null, CancellationToken cancellationToken = new CancellationToken())
		{
			if (cannotAcceptNewInvocation()) return Task.FromResult(default(TResult));

			Invocation executingInvocation = getExecutingInvocation();

			if (executingInvocation != null && executingInvocation == _executingInvocation)
			{
				return Task.Run(() =>
				{
					_ContinuationPriority = continuationPriority ?? executingInvocation._priority;
					_ContinuationContext = continuationContext ?? executingInvocation._context;
					_ContinuationExecutingInvocation = executingInvocation.Parent;

					TResult result = default;
					invoke(s => result = func(s), state, priority, context, executingInvocation);
					return result;
				}, cancellationToken);
			}

			verifyContinuationParameters(continuationPriority, continuationContext);
			return Task.Run(() =>
			{
				TResult result = default;
				invoke(s => result = func(s), state, priority, context, executingInvocation);
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
		Ignore,
		ThrowException
	}

	public enum OcDispatcherState
	{
		RunOrWait,
		Failure,
		Disposing,
		Disposed
	}

	internal class OcDispatcherSynchronizationContext : SynchronizationContext
	{
		OcDispatcher _ocDispatcher;
		internal int _priority;

		public OcDispatcherSynchronizationContext(OcDispatcher ocDispatcher)
		{
			_ocDispatcher = ocDispatcher;
		}

		#region Overrides of SynchronizationContext
		public override void Post(SendOrPostCallback postCallback, object state)
		{
			_ocDispatcher.invoke(
				() => postCallback(state), 
				OcDispatcher._ContinuationPriority, 
				OcDispatcher._ContinuationContext,
				OcDispatcher._ContinuationExecutingInvocation);
		}
		#endregion
	}
}

