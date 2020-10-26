using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace ObservableComputations
{
	public struct Invocation
	{
		public Action Action => _action;
		public Action<object> ActionWithState => _actionWithState;
		public object State => _state;
		public string CallStackTrace => _callStackTrace;
		public object Context => _context;
		public Dispatcher Dispatcher => _dispatcher;

		private Action _action;
		private Action<object> _actionWithState;
		private object _state;
		private string _callStackTrace;
		private object _context;
		private Dispatcher _dispatcher;

		internal Invocation(Action action, Dispatcher dispatcher, object context = null) : this()
		{
			_action = action;
			_dispatcher = dispatcher;
			_context = context;

			if (Configuration.SaveDispatcherInvocationStackTrace)
				_callStackTrace = Environment.StackTrace;
		}

		internal Invocation(Action<object> actionWithState, object state, Dispatcher dispatcher, IComputing context = null) : this()
		{
			_actionWithState = actionWithState;
			_state = state;
			_dispatcher = dispatcher;
			_context = context;

			if (Configuration.SaveDispatcherInvocationStackTrace)
				_callStackTrace = Environment.StackTrace;
		}

		internal void DoOther()
		{
			if (Configuration.TrackDispatcherInvocations)
			{
				DebugInfo._executingDispatcherInvocations.TryGetValue(_dispatcher._thread, out Stack<Invocation> invocations);
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
			if (Configuration.TrackDispatcherInvocations)
			{
				DebugInfo._executingDispatcherInvocations[_dispatcher._thread] = _dispatcher._invocations;
				_dispatcher._invocations.Push(this);

				if (_action != null)
					_action();
				else
					_actionWithState(_state);

				DebugInfo._executingDispatcherInvocations.TryRemove(_dispatcher._thread, out _);
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

    public class Dispatcher : IDisposable, IDispatcher
    {
        ConcurrentQueue<Invocation> _invocationQueue = new ConcurrentQueue<Invocation>();
        private ManualResetEventSlim _newInvocationManualResetEvent = new ManualResetEventSlim(false);
        private bool _alive = true;
        internal Thread _thread;
        private int _managedThreadId;
        internal Stack<Invocation> _invocations = new Stack<Invocation>();

        public Thread Thread => _thread;
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

        public Dispatcher(string threadName = null)
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

            _thread.Name = threadName ?? "ObservableComputations.Worker";
            _managedThreadId = _thread.ManagedThreadId;
            _thread.Start();
        }

        public TimeSpan DoOthers(TimeSpan timeSpan)
        {
            if (_managedThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new ObservableComputationsException(
                    "Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");

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
                    "Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");

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
                    "Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");

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

        void IDispatcher.Invoke(Action action, object context)
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
            return $"ObservableComputations.Dispatcher Thread.Name = {Thread.Name}";
        }

        #endregion
    }
}

