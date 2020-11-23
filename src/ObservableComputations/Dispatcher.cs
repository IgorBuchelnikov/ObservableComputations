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

		internal void DoOther()
		{
			if (Configuration.TrackOcDispatcherInvocations)
			{
				DebugInfo._executingOcDispatcherInvocations.TryGetValue(_ocDispatcher._thread, out Stack<Invocation> invocations);
                // ReSharper disable once PossibleNullReferenceException
                invocations.Push(this);

				if (_action != null)
					_action();
				else
					_actionWithState(_state);

				invocations.Pop();
			}
			else
			{
				if (_action != null)
					_action();
				else
					_actionWithState(_state);				
			}	
		}

		internal void Do()
		{
			if (Configuration.TrackOcDispatcherInvocations)
			{
				DebugInfo._executingOcDispatcherInvocations[_ocDispatcher._thread] = _ocDispatcher._invocations;
				_ocDispatcher._invocations.Push(this);

				if (_action != null)
					_action();
				else
					_actionWithState(_state);

				DebugInfo._executingOcDispatcherInvocations.TryRemove(_ocDispatcher._thread, out _);
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
        ConcurrentQueue<Invocation> _invocationQueue = new ConcurrentQueue<Invocation>();
        private ManualResetEventSlim _newInvocationManualResetEvent = new ManualResetEventSlim(false);
        private bool _alive = true;
        internal Thread _thread;
        private int _managedThreadId;
        internal Stack<Invocation> _invocations = new Stack<Invocation>();

        public bool Disposed => !_alive;

        private void queueInvocation(Action action, object context = null)
        {
            Invocation invocation = new Invocation(action, this, context);
            _invocationQueue.Enqueue(invocation);
            _newInvocationManualResetEvent.Set();
        }

        private void queueInvocation(Action<object> action, object state, IComputing computing = null)
        {
            Invocation invocation = new Invocation(action, state, this, computing);
            _invocationQueue.Enqueue(invocation);
            _newInvocationManualResetEvent.Set();
        }

        public OcDispatcher()
        {
            _thread = new Thread(() =>
            {
                while (_alive)
                {
                    _newInvocationManualResetEvent.Wait();
                    _newInvocationManualResetEvent.Reset();

                    while (_invocationQueue.TryDequeue(out Invocation invocation))
                    {
                        invocation.Do();
                    }
                }

            });

            _thread.Name = "ObservableComputations.OcDispatcher";
            _managedThreadId = _thread.ManagedThreadId;
            _thread.Start();
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
            if (_managedThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new ObservableComputationsException(
                    "OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (_invocationQueue.TryDequeue(out Invocation invocation))
            {
                invocation.DoOther();
                if (stopwatch.ElapsedTicks > timeSpan.Ticks) break;
            }

            stopwatch.Stop();
            return timeSpan - TimeSpan.FromTicks(stopwatch.ElapsedTicks);
        }

        public int DoOthers(int count)
        {
            if (_thread != Thread.CurrentThread)
                throw new ObservableComputationsException(
                    "OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");

            int counter = 0;
            while (_invocationQueue.TryDequeue(out Invocation invocation))
            {
                invocation.DoOther();
                counter++;
                if (counter == count) break;
            }

            return count - counter;
        }

        public void DoOthers()
        {
            if (_thread != Thread.CurrentThread)
                throw new ObservableComputationsException(
                    "OcDispatcher.DoOthers method can only be called from the same thread that is associated with this OcDispatcher.");

            while (_invocationQueue.TryDequeue(out Invocation invocation))
            {
                invocation.DoOther();
            }
        }

        public void Dispose()
        {
            _alive = false;
            _newInvocationManualResetEvent.Set();
        }

        void IOcDispatcher.Invoke(Action action, object context)
        {
            if (!_alive) return;

            if (_thread == Thread.CurrentThread)
            {
                action();
            }
            else
            {
                queueInvocation(action, context);                
            }
        }

        public void BeginInvoke(Action action)
        {
            if (!_alive) return;

            if (_thread == Thread.CurrentThread)
            {
                action();
                return;
            }

            queueInvocation(action);
        }

        public void BeginInvoke(Action<object> action, object state)
        {
            if (!_alive) return;

            if (_thread == Thread.CurrentThread)
            {
                action(state);
            }
            else
            {
                queueInvocation(action, state);               
            }
 
        }

        public void Invoke(Action action)
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

            queueInvocation(actionWithManualResetEvent);
            manualResetEvent.WaitOne();
            manualResetEvent.Dispose();

        }

        public void Invoke(Action<object> action, object state)
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

            queueInvocation(actionWithManualResetEvent, state);
            manualResetEvent.WaitOne();
            manualResetEvent.Dispose();
        }

        public TResult Invoke<TResult>(Func<TResult> func)
        {
            TResult result = default;
            Invoke(() => { result = func(); });
            return result;
        }

        public TResult Invoke<TResult>(Func<object, TResult> func, object state)
        {
            TResult result = default;
            Invoke(s => { result = func(s); }, state);

            return result;
        }

        public InvocationResult<TResult> BeginInvoke<TResult>(Func<TResult> func)
        {
            if (!_alive) return default;

            InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
            BeginInvoke(() => { invocationResult.Result = func(); });

            return invocationResult;
        }

        public InvocationResult<TResult> BeginInvoke<TResult>(Func<object, TResult> func, object state)
        {
            if (!_alive) return default;

            InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
           BeginInvoke(s => { invocationResult.Result = func(s); }, state);

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

