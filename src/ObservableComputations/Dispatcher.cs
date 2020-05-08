using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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


		internal ManualResetEventSlim _doneManualResetEvent;
		internal Action _action;
		internal Action<object> _actionWithState;
		internal object _state;

		internal Invocation(ManualResetEventSlim doneManualResetEvent, Action action) : this()
		{
			_doneManualResetEvent = doneManualResetEvent;
			_action = action;
		}

		internal Invocation(ManualResetEventSlim doneManualResetEvent, Action<object> actionWithState, object state) : this()
		{
			_doneManualResetEvent = doneManualResetEvent;
			_actionWithState = actionWithState;
			_state = state;
		}

		internal void Do()
		{
			if (_action != null)
				_action();
			else
				_actionWithState(_state);

			_doneManualResetEvent.Set();
			_doneManualResetEvent.Dispose();
		}

		public void Wait()
		{
			if (_doneManualResetEvent != null)
			{
				try
				{
					_doneManualResetEvent.Wait();
				}
				catch (ObjectDisposedException)
				{
				}
			}

		}

		public Task ToTask()
		{
			var doneManualResetEvent = _doneManualResetEvent;
			return Task.Run(() => doneManualResetEvent.Wait());
				
		}
	}

	public class InvocationResult<TResult>
	{
		public Invocation Invocation => _invocation;
		public TResult Result => _result;

		internal Invocation _invocation;
		internal TResult _result;

		public void Wait()
		{
			_invocation.Wait();
		}

		public Task<TResult> ToTask()
		{
			return Task<TResult>.Factory.StartNew(() =>
			{
				_invocation.Wait();
				return _result;
			});
				
		}
	}

	public class Dispatcher : IDisposable, IDispatcher
	{
		ConcurrentQueue<Invocation> _invocationQueue = new ConcurrentQueue<Invocation>();
		private ManualResetEventSlim _newInvocationManualResetEvent = new ManualResetEventSlim(false);
		private bool _alive = true;
		private Thread _thread;
		private int _managedThreadId;

		public int ManagedThreadId => _managedThreadId;
		public string ThreadName => _thread.Name;


		private Invocation queueInvocation(Action action)
		{
			ManualResetEventSlim doneManualResetEvent = new ManualResetEventSlim(false);
			Invocation invocation = new Invocation(doneManualResetEvent, action);
			_invocationQueue.Enqueue(invocation);
			_newInvocationManualResetEvent.Set();
			return invocation;
		}

		private Invocation queueInvocation(Action<object> action, object state)
		{
			ManualResetEventSlim doneManualResetEvent = new ManualResetEventSlim(false);
			Invocation invocation = new Invocation(doneManualResetEvent, action, state);
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

					DoOthers();
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

		public Invocation BeginInvoke(Action action)
		{
			if (_managedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action();
				return new Invocation(null, action);
			}

			return queueInvocation(action);
		}

		public Invocation BeginInvoke(Action<object> action, object state)
		{
			if (_managedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action(state);
				return new Invocation(null, action, state);
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

			Invocation invocation = queueInvocation(action);
			invocation.Wait();
		}

		public void Invoke(Action<object> action, object state)
		{
			if (_managedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action(state);
				return;
			}

			Invocation invocation = queueInvocation(action, state);
			invocation.Wait();
		}

		public TResult Invoke<TResult>(Func<TResult> func)
		{
			TResult result = default;
			Invoke(() => result = func());
			return result;
		}

		public TResult Invoke<TResult>(Func<object, TResult> func, object state)
		{
			TResult result = default;
			Invoke(s => result = func(s), state);
			return result;
		}

		public InvocationResult<TResult> BeginInvoke<TResult>(Func<TResult> func)
		{
			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			invocationResult._invocation = BeginInvoke(() =>
			{
				invocationResult._result = func();
			});

			return invocationResult;
		}

		public  InvocationResult<TResult> BeginInvoke<TResult>(Func<object, TResult> func, object state)
		{
			InvocationResult<TResult> invocationResult = new InvocationResult<TResult>();
			invocationResult._invocation = BeginInvoke(s =>
			{
				invocationResult._result = func(s);
			}, state);

			return invocationResult;
		}
	}
}
