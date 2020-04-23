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
	public class Worker : IDisposable
	{
		private struct WorkItem
		{
			public ManualResetEvent DoneManualResetEvent;
			public Action Action;
			public Action<object> ActionWithState;
			public object State;

			public WorkItem(ManualResetEvent doneManualResetEvent, Action action) : this()
			{
				DoneManualResetEvent = doneManualResetEvent;
				Action = action;
			}

			public WorkItem(ManualResetEvent doneManualResetEvent, Action<object> actionWithState, object state) : this()
			{
				DoneManualResetEvent = doneManualResetEvent;
				ActionWithState = actionWithState;
				State = state;
			}

			public void Do()
			{
				if (Action != null)
					Action();
				else
					ActionWithState(State);

				DoneManualResetEvent.Set();
			}

		}

		ConcurrentQueue<WorkItem> _workItemQueue = new ConcurrentQueue<WorkItem>();
		private ManualResetEvent _newWorkItemManualResetEvent = new ManualResetEvent(false);
		private bool _alive = true;
		private Thread _workerThread;

		public int ManagedThreadId => _workerThread.ManagedThreadId;
		public string ThreadName => _workerThread.Name;


		public WaitHandle Do(Action action)
		{
			ManualResetEvent doneManualResetEvent = new ManualResetEvent(false);
			_workItemQueue.Enqueue(new WorkItem(doneManualResetEvent, action));
			_newWorkItemManualResetEvent.Set();
			return doneManualResetEvent;
		}

		public WaitHandle Do(Action<object> action, object state)
		{
			ManualResetEvent doneManualResetEvent = new ManualResetEvent(false);
			_workItemQueue.Enqueue(new WorkItem(doneManualResetEvent, action, state));
			_newWorkItemManualResetEvent.Set();
			return doneManualResetEvent;
		}


		public Worker(string threadName = null)
		{
			_workerThread = new Thread(() =>
			{
				while (_alive)
				{
					_newWorkItemManualResetEvent.WaitOne();
					_newWorkItemManualResetEvent.Reset();

					DoOthers();
				}

			});

			_workerThread.Name = threadName ?? "ObservableComputations.Worker";

			_workerThread.Start();
		}

		public TimeSpan DoOthers(TimeSpan timeSpan)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (_workItemQueue.TryDequeue(out WorkItem workItem))
			{
				workItem.Do();
				if (stopwatch.ElapsedTicks > timeSpan.Ticks) break;
			}
			stopwatch.Stop();
			return timeSpan - TimeSpan.FromTicks(stopwatch.ElapsedTicks);
		}

		public void DoOthers(int count)
		{
			int counter = 0;
			while (_workItemQueue.TryDequeue(out WorkItem workItem))
			{
				workItem.Do();
				counter++;
				if (counter == count) break;
			}
		}

		public void DoOthers()
		{
			while (_workItemQueue.TryDequeue(out WorkItem workItem))
			{
				workItem.Do();
			}
		}

		public void Dispose()
		{
			_alive = false;
			_newWorkItemManualResetEvent.Set();
		}


		private WorkerDispatcher _workerDispatcher;
		public WorkerDispatcher Dispatcher => _workerDispatcher = _workerDispatcher ?? new WorkerDispatcher(this);

		public AsyncDestinationDispatcher GetNewAsyncDestinationDispatcher() => new AsyncDestinationDispatcher(this);

		public class WorkerDispatcher :
			IDispatcher,
			IDestinationCollectionDispatcher,
			ISourceCollectionDispatcher,
			IDestinationScalarDispatcher,
			ISourceScalarDispatcher
		{
			private Worker _worker;

			internal WorkerDispatcher(Worker worker)
			{
				_worker = worker;
			}

			private void invoke(Action action)
			{
				if (Thread.CurrentThread.ManagedThreadId == _worker.ManagedThreadId) 
					action();
				else
				{
					var waitHandle = _worker.Do(action);
					waitHandle.WaitOne();
					waitHandle.Dispose();					
				}

			}

			public WaitHandle BeginInvoke(Action action)
			{
				return _worker.Do(action);
			}

			public WaitHandle BeginInvoke(Action<object> action, object state)
			{
				return _worker.Do(action, state);
			}

			public void InvokeInitializeFromSourceCollection(Action action, ICollectionComputing collectionDispatching,
				object sourceItem, object sender, EventArgs eventArgs)
			{
				invoke(action);
			}

			public void InvokeCollectionChange(Action action, ICollectionComputing collectionDispatching, object sender,
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
			{
				invoke(action);
			}

			public void InvokeRaiseConsistencyRestored(Action action, ICollectionComputing collectionDispatching)
			{
				invoke(action);
			}

			public void InvokeReadAndSubscribe(Action action, ICollectionComputing collectionDispatching)
			{
				invoke(action);
			}

			public void InvokeSetValue(Action action, IScalarComputing scalarDispatching, object sender,
				PropertyChangedEventArgs propertyChangedEventArgs)
			{
				invoke(action);
			}

			public void InvokeReadAndSubscribe(Action action, IScalarComputing scalarDispatching)
			{
				invoke(action);
			}
		}

		public class AsyncDestinationDispatcher :
			IDestinationCollectionDispatcher,
			IDestinationScalarDispatcher
		{
			private Worker _worker;
			private WaitHandle _lastActionWaitHandle;

			internal AsyncDestinationDispatcher(Worker worker)
			{
				_worker = worker;
			}

			private void invoke(Action action)
			{
				if (_lastActionWaitHandle != null)
				{
					_lastActionWaitHandle.WaitOne();
					_lastActionWaitHandle.Dispose();
				}

				_lastActionWaitHandle = _worker.Do(action);
			}

			public void InvokeInitializeFromSourceCollection(Action action, ICollectionComputing collectionDispatching,
				object sourceItem, object sender, EventArgs eventArgs)
			{
				invoke(action);
			}


			public void InvokeCollectionChange(Action action, ICollectionComputing collectionDispatching, object sender,
				NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
			{
				invoke(action);
			}

			public void InvokeRaiseConsistencyRestored(Action action, ICollectionComputing collectionDispatching)
			{
				invoke(action);
			}


			public void InvokeSetValue(Action action, IScalarComputing scalarDispatching, object sender,
				PropertyChangedEventArgs propertyChangedEventArgs)
			{
				invoke(action);
			}
		}


	}
}
