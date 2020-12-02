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

		private Action _action;
		private Action<object> _actionWithState;
		private object _state;
		private string _callStackTrace;
		private object _context;
		private OcDispatcher _ocDispatcher;

		internal Invocation(Action action, OcDispatcher ocDispatcher, object context = null) : this()
		{
			_action = action;
			_ocDispatcher = ocDispatcher;
			_context = context;

			if (Configuration.SaveOcDispatcherInvocationStackTrace)
				_callStackTrace = Environment.StackTrace;
		}

		internal Invocation(Action<object> actionWithState, object state, OcDispatcher ocDispatcher, IComputing context = null) : this()
		{
			_actionWithState = actionWithState;
			_state = state;
			_ocDispatcher = ocDispatcher;
			_context = context;

			if (Configuration.SaveOcDispatcherInvocationStackTrace)
				_callStackTrace = Environment.StackTrace;
		}

		internal void Do(bool others)
		{
			if (Configuration.TrackOcDispatcherInvocations)
            {
                Stack<Invocation> invocations;
                if (others)
                {
                    DebugInfo._executingOcDispatcherInvocations.TryGetValue(_ocDispatcher._thread, out invocations);
                }
                else
                {
                    invocations = _ocDispatcher._invocations;
                    DebugInfo._executingOcDispatcherInvocations[_ocDispatcher._thread] = invocations;
                }

                // ReSharper disable once PossibleNullReferenceException
                invocations.Push(this);

				if (_action != null)
					_action();
				else
					_actionWithState(_state);

                if (others) invocations.Pop();
				else DebugInfo._executingOcDispatcherInvocations.TryRemove(_ocDispatcher._thread, out _);
			}
			else
			{
				if (_action != null)
					_action();
				else
					_actionWithState(_state);				
			}          
		}
	}

	public class InvocationResult<TResult> : INotifyPropertyChanged
	{
		public TResult Result
		{
			get => _result;
			internal set
			{
				_result = value;
				PropertyChanged?.Invoke(this, Utils.ResultPropertyChangedEventArgs);
			}
		}

		private TResult _result;

		public event PropertyChangedEventHandler PropertyChanged;
	}

    public class OcDispatcher : IDisposable, IOcDispatcher
    {
        ConcurrentQueue<Invocation>[] _invocationQueues;
        private ManualResetEventSlim _newInvocationManualResetEvent = new ManualResetEventSlim(false);
        private ManualResetEventSlim _disposeManualResetEvent = new ManualResetEventSlim(false);
        private bool _alive = true;
        internal Thread _thread;
        internal Stack<Invocation> _invocations = new Stack<Invocation>();

        public bool Disposed => !_alive;

        private void queueInvocation(Action action, int priority, object context = null)
        {
            Invocation invocation = new Invocation(action, this, context);
            _invocationQueues[priority].Enqueue(invocation);
            _newInvocationManualResetEvent.Set();
        }

        private void queueInvocation(Action<object> action, int priority, object state, IComputing computing = null)
        {
            Invocation invocation = new Invocation(action, state, this, computing);
            _invocationQueues[priority].Enqueue(invocation);
            _newInvocationManualResetEvent.Set();
        }

        public OcDispatcher(
            int prioritiesNumber = 1,
            ApartmentState threadApartmentState = ApartmentState.Unknown)
        {
            _invocationQueues = new ConcurrentQueue<Invocation>[prioritiesNumber];

            for (int priority = 0; priority < prioritiesNumber; priority++)
                _invocationQueues[priority] = new ConcurrentQueue<Invocation>();

            _thread = new Thread(() =>
            {

                int highestPriority = prioritiesNumber - 1;
                while (_alive)
                {
                    _newInvocationManualResetEvent.Wait();
                    _newInvocationManualResetEvent.Reset();

                    processQueues(highestPriority, false, null);
                }

                _disposeManualResetEvent.Set();
            });

            _thread.SetApartmentState(threadApartmentState);
            _thread.Start();
        }

        private void processQueues(int highestPriority, bool doOthers, Func<int, bool> stop)
        {
            int priority;
            bool processed = true;
            int count = 0;
            while (processed)
            {
                processed = false;
                for (priority = highestPriority; priority >= 0; priority--)
                {
                    if (priority == highestPriority)
                    {
                        ConcurrentQueue<Invocation> invocationQueue = _invocationQueues[priority];
                        while (invocationQueue.TryDequeue(out Invocation invocation))
                        {
                            invocation.Do(doOthers);
                            count++;
                            if (stop != null && stop(count))
                            {
                                processed = false;
                                break;
                            }
                            else
                            {
                                processed = true;                               
                            }

                        }
                    }
                    else
                    {
                        if (_invocationQueues[priority].TryDequeue(out Invocation invocation))
                        {
                            invocation.Do(doOthers);
                            count++;
                            processed = stop == null || !stop(count);
                            break;
                        }
                    }
                }
            }
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

        public bool ThreadIsAlive
        {
            get => _thread.IsAlive;
        }

        public ExecutionContext ThreadExecutionContext
        {
            get => _thread.ExecutionContext;
        }

        public bool ThreadIsBackground
        {
            get => _thread.IsBackground;
            set => _thread.IsBackground = value;
        }

        public int ManagedThreadId
        {
            get => _thread.ManagedThreadId;
        }

        public ThreadPriority ThreadPriority
        {
            get => _thread.Priority;
            set => _thread.Priority = value;
        }

        public ThreadState ThreadState
        {
            get => _thread.ThreadState;
        }

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


        public TimeSpan DoOthers(TimeSpan timeSpan)
        {
            if (_thread != Thread.CurrentThread)
                throw new ObservableComputationsException(
                    "OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");

            Stopwatch stopwatch = Stopwatch.StartNew();
            processQueues(_invocationQueues.Length - 1, true, count => stopwatch.ElapsedTicks > timeSpan.Ticks);
            stopwatch.Stop();
            return timeSpan - TimeSpan.FromTicks(stopwatch.ElapsedTicks);
        }

        public int DoOthers(int targetCount)
        {
            if (_thread != Thread.CurrentThread)
                throw new ObservableComputationsException(
                    "OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");

            int factCount = 0;
            processQueues(_invocationQueues.Length - 1, true, 
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

            processQueues(_invocationQueues.Length - 1, true, null);
        }

        public void Dispose()
        {
            _alive = false;
            _newInvocationManualResetEvent.Set();
            _disposeManualResetEvent.Wait();
        }

        void IOcDispatcher.Invoke(Action action, int priority, object context)
        {
            if (!_alive) return;

            if (_thread == Thread.CurrentThread)
            {
                action();
                return;
            }

            queueInvocation(action, priority, context);
        }

        public void BeginInvoke(Action action, int priority)
        {
            if (!_alive) return;

            if (_thread == Thread.CurrentThread)
            {
                action();
                return;
            }

            queueInvocation(action, priority);
        }

        public void BeginInvoke(Action<object> action, int priority, object state)
        {
            if (!_alive) return;

            if (_thread == Thread.CurrentThread)
            {
                action(state);
                return;
            }

            queueInvocation(action, priority, state);                        
        }

        public void Invoke(Action action, int priority)
        {
            if (!_alive) return;

            if (_thread == Thread.CurrentThread)
            {
                action();
                return;
            }

            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            Action actionWithManualResetEvent = () =>
            {
                action();
                // ReSharper disable once AccessToDisposedClosure
                manualResetEvent.Set();
            };

            queueInvocation(actionWithManualResetEvent, priority);
            manualResetEvent.WaitOne();
            manualResetEvent.Dispose();

        }

        public void Invoke(Action<object> action, int priority, object state)
        {
            if (!_alive) return;

            if (_thread == Thread.CurrentThread)
            {
                action(state);
                return;
            }

            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            Action<object> actionWithManualResetEvent = s =>
            {
                action(s);
                // ReSharper disable once AccessToDisposedClosure
                manualResetEvent.Set();
            };

            queueInvocation(actionWithManualResetEvent, priority, state);
            manualResetEvent.WaitOne();
            manualResetEvent.Dispose();
        }

        public TResult Invoke<TResult>(Func<TResult> func, int priority)
        {
            TResult result = default;
            Invoke(() => { result = func(); }, priority);
            return result;
        }

        public TResult Invoke<TResult>(Func<object, TResult> func, int priority, object state)
        {
            TResult result = default;
            Invoke(s => { result = func(s); }, priority, state);

            return result;
        }

        public InvocationResult<TResult> BeginInvoke<TResult>(Func<TResult> func, int priority)
        {
            if (!_alive) return default;

            InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
            BeginInvoke(() => { invocationResult.Result = func(); }, priority);

            return invocationResult;
        }

        public InvocationResult<TResult> BeginInvoke<TResult>(Func<object, TResult> func, int priority, object state)
        {
            if (!_alive) return default;

            InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
           BeginInvoke(s => { invocationResult.Result = func(s); }, priority, state);

            return invocationResult;
        }

        #region Overrides of Object

        public override string ToString()
        {
            return $"ObservableComputations.OcDispatcher Thread.Name = {_thread.Name}";
        }

        #endregion
    }
}

