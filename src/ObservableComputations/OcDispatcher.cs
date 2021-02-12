// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace ObservableComputations
{
	public struct Invocation
	{
		public Action Action => _action;
		public Action<object> ActionWithState => _actionWithState;
		public object State => _state;
		public string CallStackTrace => _callStackTrace;
		public object Context => _context;
		public OcDispatcher OcDispatcher => _ocDispatcher;

		private readonly Action _action;
		private readonly Action<object> _actionWithState;
		private readonly object _state;
		private readonly string _callStackTrace;
		private readonly object _context;
		private readonly OcDispatcher _ocDispatcher;
		internal readonly InvocationStatus InvocationStatus;
		internal readonly ManualResetEventSlim _doneManualResetEvent;

		internal Invocation(Action action, OcDispatcher ocDispatcher, string callStackTrace, InvocationStatus invocationStatus, object context = null, ManualResetEventSlim doneManualResetEvent = null) : this()
		{
			_action = action;
			_ocDispatcher = ocDispatcher;
			_context = context;
			InvocationStatus = invocationStatus;
			_doneManualResetEvent = doneManualResetEvent;
			_callStackTrace = callStackTrace;
		}

		internal Invocation(Action<object> actionWithState, object state, OcDispatcher ocDispatcher, string callStackTrace, InvocationStatus invocationStatus, object context = null, ManualResetEventSlim doneManualResetEvent = null) : this()
		{
			_actionWithState = actionWithState;
			_state = state;
			_ocDispatcher = ocDispatcher;
			_context = context;
			InvocationStatus = invocationStatus;
			_doneManualResetEvent = doneManualResetEvent;
			_callStackTrace = callStackTrace;
		}

		internal void Do()
		{
			if (Configuration.TrackOcDispatcherInvocations)
			{
				bool nestedInvocation = true;
				if (!DebugInfo._executingOcDispatcherInvocations.TryGetValue(_ocDispatcher._managedThreadId, out Stack<Invocation> invocations))
				{
					nestedInvocation = false;
					invocations = new Stack<Invocation>();
					DebugInfo._executingOcDispatcherInvocations[_ocDispatcher._managedThreadId] = invocations;
				}

				// ReSharper disable once PossibleNullReferenceException
				invocations.Push(this);

				if (_action != null)
					_action();
				else
					_actionWithState(_state);

				if (nestedInvocation) invocations.Pop();
				else DebugInfo._executingOcDispatcherInvocations.TryRemove(_ocDispatcher._managedThreadId, out _);
			}
			else
			{
				if (_action != null)
					_action();
				else
					_actionWithState(_state);				
			}  

			if (InvocationStatus != null) 
				InvocationStatus.Done = true; 
			
			_doneManualResetEvent?.Set();
		}
	}

	internal class InvocationStatus
	{
		internal bool Done;
	}

	public class InvocationResult<TResult> : IReadScalar<TResult>
	{
		public TResult Value
		{
			get => _value;
			internal set
			{
				_value = value;
				PropertyChanged?.Invoke(this, Utils.ResultPropertyChangedEventArgs);
			}
		}

		private TResult _value;

		public event PropertyChangedEventHandler PropertyChanged;

		#region Overrides of Object

		public override string ToString()
		{
			return $"(ObservableComputations.InvocationResult<{typeof(TResult).Name}> (Value = '{_value.ToStringSafe(e => e.Message)}'))";
		}

		#endregion
	}

	public class OcDispatcher : IDisposable, IOcDispatcher
	{
		readonly ConcurrentQueue<Invocation>[] _invocationQueues;
		private readonly ManualResetEventSlim _newInvocationManualResetEvent = new ManualResetEventSlim(false);
		private bool _isAlive = true;
		private bool _isDisposed ;
		internal readonly Thread _thread;
		internal readonly int _managedThreadId;
		private NewInvocationBehaviour _newInvocationBehaviour;
		public event EventHandler DisposeFinished;

		public bool IsAlive => _isAlive;
		public bool IsDisposed => _isDisposed;

		private readonly string _instantiatingStackTrace;
		public string InstantiatingStackTrace => _instantiatingStackTrace;

		public NewInvocationBehaviour NewInvocationBehaviour
		{
			get => _newInvocationBehaviour;
			set
			{
				if (!_isAlive && value != NewInvocationBehaviour.Ignore)
					throw new ObservableComputationsException("Disposing is in progress");
				_newInvocationBehaviour = value;
			}
		}

		private void queueInvocation(Action action, int priority, object context = null,
			InvocationStatus invocationStatus = null, ManualResetEventSlim doneManualResetEvent = null)
		{
			Invocation invocation = new Invocation(action, this, getCallStackTrace(), invocationStatus, context, doneManualResetEvent);
			_invocationQueues[priority].Enqueue(invocation);
			_newInvocationManualResetEvent.Set();
		}

		private static string getCallStackTrace()
		{
			string callStackTrace = null;
			if (Configuration.SaveOcDispatcherInvocationStackTrace)
				callStackTrace = Environment.StackTrace;
			return callStackTrace;
		}

		private void queueInvocation(Action<object> action, int priority, object state, object context = null,
			InvocationStatus invocationStatus = null, ManualResetEventSlim doneManualResetEvent = null)
		{
			Invocation invocation = new Invocation(action, state, this, getCallStackTrace(), invocationStatus, context, doneManualResetEvent);
			_invocationQueues[priority].Enqueue(invocation);
			_newInvocationManualResetEvent.Set();
		}

		public OcDispatcher(
			int prioritiesNumber = 1,
			ApartmentState threadApartmentState = ApartmentState.Unknown)
		{
			if (Configuration.SaveInstantiatingStackTrace)
				_instantiatingStackTrace = Environment.StackTrace;

			_invocationQueues = new ConcurrentQueue<Invocation>[prioritiesNumber];

			for (int priority = 0; priority < prioritiesNumber; priority++)
				_invocationQueues[priority] = new ConcurrentQueue<Invocation>();

			_thread = new Thread(() =>
			{
				int highestPriority = prioritiesNumber - 1;
				while (_isAlive)
				{
					_newInvocationManualResetEvent.Wait();
					_newInvocationManualResetEvent.Reset();

					processQueues(highestPriority, null);
				}

				_isDisposed = true;
				_newInvocationManualResetEvent.Dispose();
				DisposeFinished?.Invoke(this, new EventArgs());
			});

			_managedThreadId = _thread.ManagedThreadId;
			_thread.SetApartmentState(threadApartmentState);
			_thread.Start();
		}

		private void processQueues(int highestPriority, Func<int, bool> stop)
		{
			bool processed = true;
			int count = 0;
			while (processed)
			{
				processed = false;
				int priority;
				for (priority = highestPriority; priority >= 0; priority--)
					if (_invocationQueues[priority].TryDequeue(out Invocation invocation))
					{
						invocation.Do();
						count++;
						processed = stop == null || !stop(count);
						break;
					}
			}
		}

		private bool checkNewInvocationBehaviour()
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

		public void SetThreadApartmentState(ApartmentState state)
		{
			_thread.SetApartmentState(state);
		}

		public bool TrySetThreadApartmentState(ApartmentState state)
		{
			return _thread.TrySetApartmentState(state);
		}

		public void ClearQueues()
		{
			for (int index = 0; index < _invocationQueues.Length; index++)
			{
				ConcurrentQueue<Invocation> invocationQueue = _invocationQueues[index];
				while (invocationQueue.TryDequeue(out Invocation invocation))
				{
					if (invocation.InvocationStatus != null) 
						invocation.InvocationStatus.Done = true;

					invocation._doneManualResetEvent?.Set();
				}
			}
		}

		public TimeSpan DoOthers(TimeSpan timeSpan)
		{
			if (_thread != Thread.CurrentThread)
				throw new ObservableComputationsException(
					"OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");

			Stopwatch stopwatch = Stopwatch.StartNew();
			processQueues(_invocationQueues.Length - 1, count => stopwatch.ElapsedTicks > timeSpan.Ticks);
			stopwatch.Stop();
			return timeSpan - TimeSpan.FromTicks(stopwatch.ElapsedTicks);
		}

		public int DoOthers(int targetCount)
		{
			if (_thread != Thread.CurrentThread)
				throw new ObservableComputationsException(
					"OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");

			int factCount = 0;
			processQueues(_invocationQueues.Length - 1, 
				count =>
				{
					factCount = count;
					return targetCount == count;
				});
			return targetCount - factCount;
		}

		public void DoOthers()
		{
			if (_thread != Thread.CurrentThread)
				throw new ObservableComputationsException(
					"OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");

			processQueues(_invocationQueues.Length - 1, null);
		}

		public void Dispose()
		{
			_isAlive = false;
			_newInvocationBehaviour = NewInvocationBehaviour.Ignore;
			ClearQueues();
			_newInvocationManualResetEvent.Set();
		}

		public void Pass(int priority = 0)
		{
			Invoke(() => {}, priority);
		}

		void IOcDispatcher.Invoke(Action action, int priority, object parameter, object context)
		{
			if (checkNewInvocationBehaviour()) return;

			queueInvocation(action, priority, context, null);
		}

		public void BeginInvoke(Action action, int priority = 0, object context = null)
		{
			if (checkNewInvocationBehaviour()) return;

			queueInvocation(action, priority, context);
		}

		public void BeginInvoke(Action<object> action, object state, int priority = 0, object context = null)
		{
			if (checkNewInvocationBehaviour()) return;

			queueInvocation(action, priority, state, context);						
		}

		public void Invoke(Action action, int priority = 0, object context = null)
		{
			if (checkNewInvocationBehaviour()) return;

			if (_thread == Thread.CurrentThread)
			{
				InvocationStatus invocationStatus = new InvocationStatus();
				queueInvocation(action, priority, context, invocationStatus);
				processQueues(_invocationQueues.Length - 1, count => invocationStatus.Done);
				return;
			}

			ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

			queueInvocation(action, priority, context, doneManualResetEvent: manualResetEvent);
			manualResetEvent.Wait();
			manualResetEvent.Dispose();
		}

		public void Invoke(Action<object> action, object state, int priority = 0, object context = null)
		{
			if (checkNewInvocationBehaviour()) return;

			if (_thread == Thread.CurrentThread)
			{
				InvocationStatus invocationStatus = new InvocationStatus();
				queueInvocation(action, priority, state, context, invocationStatus);
				processQueues(_invocationQueues.Length - 1, count => invocationStatus.Done);
				return;
			}

			ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

			queueInvocation(action, priority, state, context, doneManualResetEvent: manualResetEvent);
			manualResetEvent.Wait();
			manualResetEvent.Dispose();
		}

		public TResult Invoke<TResult>(Func<TResult> func, int priority = 0, object context = null)
		{
			TResult result = default;
			Invoke(() => { result = func(); }, priority, context);
			return result;
		}

		public TResult Invoke<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null)
		{
			TResult result = default;
			Invoke(s => { result = func(s); }, state, priority, context);

			return result;
		}

		public InvocationResult<TResult> BeginInvoke<TResult>(Func<TResult> func, int priority = 0, object context = null)
		{
			if (checkNewInvocationBehaviour()) return default;

			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			BeginInvoke(() => { invocationResult.Value = func(); }, priority, context);

			return invocationResult;
		}

		public InvocationResult<TResult> BeginInvoke<TResult>(Func<object, TResult> func, object state, int priority = 0, object context = null)
		{
			if (checkNewInvocationBehaviour()) return default;

			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			BeginInvoke(s => { invocationResult.Value = func(s); }, state, priority, context);

			return invocationResult;
		}

		#region Overrides of Object

		public override string ToString()
		{
			return $"(ObservableComputations.OcDispatcher (Thread.Name = '{_thread.Name}'))";
		}

		#endregion
	}

	public enum NewInvocationBehaviour
	{
		Accept,
		Ignore,
		ThrowException
	}
}

