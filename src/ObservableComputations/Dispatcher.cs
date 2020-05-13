using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObservableComputations
{
	public struct Invocation
	{
		public Action Action => _action;
		public Action<object> ActionWithState => _actionWithState;
		public object State => _state;
		public string CallStackTrace => _callStackTrace;
		public IComputing Computing => _computing;
		public Dispatcher Dispatcher => _dispatcher;

		private Action _action;
		private Action<object> _actionWithState;
		private object _state;
		private string _callStackTrace;
		private IComputing _computing;
		private Dispatcher _dispatcher;

		internal Invocation(Action action, Dispatcher dispatcher, IComputing computing = null) : this()
		{
			_action = action;
			_dispatcher = dispatcher;
			_computing = computing;

			if (Configuration.SaveDispatcherInvocationStackTrace)
				_callStackTrace = Environment.StackTrace;
		}

		internal Invocation(Action<object> actionWithState, object state, Dispatcher dispatcher, IComputing computing = null) : this()
		{
			_actionWithState = actionWithState;
			_state = state;
			_dispatcher = dispatcher;
			_computing = computing;

			if (Configuration.SaveDispatcherInvocationStackTrace)
				_callStackTrace = Environment.StackTrace;
		}

		internal void Do()
		{
			if (Configuration.TrackDispatcherInvocations)
				DebugInfo._executingDispatcherInvocations[_dispatcher._thread] = this;

			if (_action != null)
				_action();
			else
				_actionWithState(_state);

			if (Configuration.TrackDispatcherInvocations)
				DebugInfo._executingDispatcherInvocations.TryRemove(_dispatcher._thread, out var _);
		}
	}

	public class InvocationResult<TResult> : INotifyPropertyChanged
	{
		public Invocation Invocation => _invocation;

		public TResult Result
		{
			get => _result;
			internal set
			{
				_result = value;
				PropertyChanged?.Invoke(this, Utils.ResultPropertyChangedEventArgs);
			}
		}

		internal Invocation _invocation;
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

		public int ManagedThreadId => _managedThreadId;
		public string ThreadName => _thread.Name;
		public bool Disposed => !_alive;

		private Invocation queueInvocation(Action action, IComputing computing = null)
		{
			Invocation invocation = new Invocation(action, this, computing);
			_invocationQueue.Enqueue(invocation);
			_newInvocationManualResetEvent.Set();
			return invocation;
		}

		private Invocation queueInvocation(Action<object> action, object state, IComputing computing = null)
		{
			Invocation invocation = new Invocation(action, state, this, computing);
			_invocationQueue.Enqueue(invocation);
			_newInvocationManualResetEvent.Set();
			return invocation;
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
				throw new ObservableComputationsException("Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");

			Stopwatch stopwatch = Stopwatch.StartNew();
			while (_invocationQueue.TryDequeue(out Invocation invocation))
			{
				invocation.Do();
				if (stopwatch.ElapsedTicks > timeSpan.Ticks) break;
			}
			stopwatch.Stop();
			return timeSpan - TimeSpan.FromTicks(stopwatch.ElapsedTicks);
		}

		public int DoOthers(int count)
		{
			if (_managedThreadId != Thread.CurrentThread.ManagedThreadId)
				throw new ObservableComputationsException("Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");

			int counter = 0;
			while (_invocationQueue.TryDequeue(out Invocation invocation))
			{
				invocation.Do();
				counter++;
				if (counter == count) break;
			}

			return count - counter;
		}

		public void DoOthers()
		{
			if (_managedThreadId != Thread.CurrentThread.ManagedThreadId)
				throw new ObservableComputationsException("Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");

			while (_invocationQueue.TryDequeue(out Invocation invocation))
			{
				invocation.Do();
			}
		}

		public void Dispose()
		{
			_alive = false;
			_newInvocationManualResetEvent.Set();
		}

		void IDispatcher.BeginInvoke(Action action, IComputing computing)
		{ 
			if (_managedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action();
			}

			queueInvocation(action);			
		}

		public Invocation BeginInvoke(Action action)
		{
			if (_managedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action();
				return new Invocation(null, action, this);
			}

			return queueInvocation(action);
		}

		public Invocation BeginInvoke(Action<object> action, object state)
		{
			if (_managedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action(state);
			}

			return queueInvocation(action, state);
		}

		public void Invoke(Action action)
		{
			if (_managedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action();
				return;
			}

			ManualResetEvent manualResetEvent = new ManualResetEvent(false);

			Action actionWithManualResetEvent = () =>
			{
				action();
				manualResetEvent.Set();
			};

			queueInvocation(actionWithManualResetEvent);
			manualResetEvent.WaitOne();
			manualResetEvent.Dispose();

		}

		public void Invoke(Action<object> action, object state)
		{
			if (_managedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action(state);
				return;
			}

			ManualResetEvent manualResetEvent = new ManualResetEvent(false);

			Action<object> actionWithManualResetEvent = s =>
			{
				action(s);
				manualResetEvent.Set();
			};

			queueInvocation(actionWithManualResetEvent, state);
			manualResetEvent.WaitOne();
			manualResetEvent.Dispose();
		}

		public TResult Invoke<TResult>(Func<TResult> func)
		{
			TResult result = default;
			Invoke(() =>
			{
				result = func();
			});
			return result;
		}

		public TResult Invoke<TResult>(Func<object, TResult> func, object state)
		{
			TResult result = default;

			Invoke(s =>
			{
				result = func(s);
			}, state);

			return result;
		}

		public InvocationResult<TResult> BeginInvoke<TResult>(Func<TResult> func)
		{
			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			invocationResult._invocation = BeginInvoke(() =>
			{
				invocationResult.Result = func();
			});

			return invocationResult;
		}

		public  InvocationResult<TResult> BeginInvoke<TResult>(Func<object, TResult> func, object state)
		{
			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			invocationResult._invocation = BeginInvoke(s =>
			{
				invocationResult.Result = func(s);
			}, state);

			return invocationResult;
		}
	}
}
