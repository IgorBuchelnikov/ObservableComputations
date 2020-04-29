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
		internal ManualResetEventSlim DoneManualResetEvent;
		internal Action Action;
		internal Action<object> ActionWithState;
		internal object State;

		internal Invocation(ManualResetEventSlim doneManualResetEvent, Action action) : this()
		{
			DoneManualResetEvent = doneManualResetEvent;
			Action = action;
		}

		internal Invocation(ManualResetEventSlim doneManualResetEvent, Action<object> actionWithState, object state) : this()
		{
			DoneManualResetEvent = doneManualResetEvent;
			ActionWithState = actionWithState;
			State = state;
		}

		internal void Do()
		{
			if (Action != null)
				Action();
			else
				ActionWithState(State);

			DoneManualResetEvent.Set();
			DoneManualResetEvent.Dispose();
		}

		public void Wait()
		{
			DoneManualResetEvent.Wait();
		}

		public Task ToTask()
		{
			var doneManualResetEvent = DoneManualResetEvent;
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
		ConcurrentQueue<Invocation> _workItemQueue = new ConcurrentQueue<Invocation>();
		private ManualResetEventSlim _newWorkItemManualResetEvent = new ManualResetEventSlim(false);
		private bool _alive = true;
		private Thread _workerThread;

		public int ManagedThreadId => _workerThread.ManagedThreadId;
		public string ThreadName => _workerThread.Name;


		private Invocation queueWorkItem(Action action)
		{
			ManualResetEventSlim doneManualResetEvent = new ManualResetEventSlim(false);
			Invocation invocation = new Invocation(doneManualResetEvent, action);
			_workItemQueue.Enqueue(invocation);
			_newWorkItemManualResetEvent.Set();
			return invocation;
		}

		private Invocation queueWorkItem(Action<object> action, object state)
		{
			ManualResetEventSlim doneManualResetEvent = new ManualResetEventSlim(false);
			Invocation invocation = new Invocation(doneManualResetEvent, action, state);
			_workItemQueue.Enqueue(invocation);
			_newWorkItemManualResetEvent.Set();
			return invocation;
		}


		public Dispatcher(string threadName = null)
		{
			_workerThread = new Thread(() =>
			{
				while (_alive)
				{
					_newWorkItemManualResetEvent.Wait();
					_newWorkItemManualResetEvent.Reset();

					DoOthers();
				}

			});

			_workerThread.Name = threadName ?? "ObservableComputations.Worker";

			_workerThread.Start();
		}

		public TimeSpan DoOthers(TimeSpan timeSpan)
		{
			if (_workerThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
				throw new ObservableComputationsException("Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");

			Stopwatch stopwatch = Stopwatch.StartNew();
			while (_workItemQueue.TryDequeue(out Invocation workItem))
			{
				workItem.Do();
				if (stopwatch.ElapsedTicks > timeSpan.Ticks) break;
			}
			stopwatch.Stop();
			return timeSpan - TimeSpan.FromTicks(stopwatch.ElapsedTicks);
		}

		public int DoOthers(int count)
		{
			if (_workerThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
				throw new ObservableComputationsException("Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");


			int counter = 0;
			while (_workItemQueue.TryDequeue(out Invocation workItem))
			{
				workItem.Do();
				counter++;
				if (counter == count) break;
			}

			return count - counter;
		}

		public void DoOthers()
		{
			if (_workerThread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
				throw new ObservableComputationsException("Dispatcher.DoOthers method can only be called from the same thread that is associated with this Dispatcher.");


			while (_workItemQueue.TryDequeue(out Invocation workItem))
			{
				workItem.Do();
			}
		}

		public void Dispose()
		{
			_alive = false;
			_newWorkItemManualResetEvent.Set();
		}

		public Invocation BeginInvoke(Action action)
		{
			if (_workerThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
				throw new ObservableComputationsException("Dispatcher.BeginInvoke can only be called from a thread other than the thread associated with this Dispatcher");
			return queueWorkItem(action);
		}

		public Invocation BeginInvoke(Action<object> action, object state)
		{
			if (_workerThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
				throw new ObservableComputationsException("Dispatcher.BeginInvoke can only be called from a thread other than the thread associated with this Dispatcher");

			return queueWorkItem(action, state);
		}

		public void Invoke(Action action)
		{
			if (_workerThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action();
				return;
			}

			Invocation invocation = queueWorkItem(action);
			invocation.Wait();
		}

		public void Invoke(Action<object> action, object state)
		{
			if (_workerThread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				action(state);
				return;
			}

			Invocation invocation = queueWorkItem(action, state);
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
