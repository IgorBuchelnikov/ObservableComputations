using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture()]
	[Timeout(1000 * 60 * 60 * 3)]
	public class OcDispatcherTests
	{
		[Test, Combinatorial]
		public void TestDebugInfo(
			[Values(true, false)] bool debug,
			[Values(true, false)]bool secondAsync)
		{
			OcConfiguration.SaveOcDispatcherInvocationInstantiationStackTrace = debug;

			object context = new object();
			OcDispatcher dispatcher = new OcDispatcher(2);
			Assert.NotNull(OcDispatcher.PassContext);

			Action action = null;
			Invocation parentInvocation;

			action = () =>
			{
				Assert.IsTrue(dispatcher.CheckAccess());

				Assert.AreEqual(OcDispatcher.Current, dispatcher);
				parentInvocation = dispatcher.CurrentInvocation;
				Assert.AreEqual(parentInvocation.Parent, null);
				Assert.AreEqual(parentInvocation.Action, action);
				Assert.AreEqual(parentInvocation.ActionWithState, null);
				Assert.AreEqual(parentInvocation.State, null);
				if (debug) Assert.IsTrue(parentInvocation.InstantiationStackTrace != null);
				Assert.AreEqual(parentInvocation.Context, context);
				Assert.AreEqual(parentInvocation.OcDispatcher, dispatcher);
				Assert.AreEqual(parentInvocation.Priority, 1);

				Func<int> func1 = null;

				func1 = () =>
				{
					Assert.IsTrue(dispatcher.CheckAccess());
					Assert.AreEqual(OcDispatcher.Current, dispatcher);
					Invocation invocation1 = dispatcher.CurrentInvocation;
					Assert.AreEqual(invocation1.Parent, parentInvocation);
					Assert.AreEqual(invocation1.ActionWithState, null);
					Assert.AreEqual(invocation1.State, null);
					if (debug) Assert.IsTrue(invocation1.InstantiationStackTrace != null);
					Assert.AreEqual(invocation1.Context, null);
					Assert.AreEqual(invocation1.OcDispatcher, dispatcher);
					Assert.AreEqual(invocation1.Priority, 0);

					return 3;
				};

				if (secondAsync)
				{
					dispatcher.InvokeAsync(func1).PropertyChanged += (sender, args) =>
					{
						if (args.PropertyName == "Value")
							Assert.AreEqual(((InvocationResult<int>)sender).Value, 3);
					};
				}
				else Assert.AreEqual(dispatcher.Invoke(func1).Value, 3);
			};

			Assert.IsFalse(dispatcher.CheckAccess());
			dispatcher.Invoke(action, 1, context);

			dispatcher.Pass();
			dispatcher.Pass();
			Assert.AreEqual(dispatcher, StaticInfo.OcDispatchers[dispatcher.ManagedThreadId]);
			Assert.IsTrue(dispatcher.CurrentInvocation == null);
			Assert.IsTrue(OcDispatcher.Current == null);
			dispatcher.Dispose();
		}

		[Test, Combinatorial]
		public void TestDebugInfoState(
			[Values(true, false)] bool debug, 
			[Values(true, false)] bool secondAsync)
		{
			OcConfiguration.SaveOcDispatcherInvocationInstantiationStackTrace = debug;

			object context = new object();
			object state = new object();
			OcDispatcher dispatcher = new OcDispatcher(2);
			Action<object> action = null;
			Invocation parentInvocation;

			action = s =>
			{
				Assert.AreEqual(s, state);
				Assert.IsTrue(dispatcher.CheckAccess());

				Assert.AreEqual(OcDispatcher.Current, dispatcher);
				parentInvocation = dispatcher.CurrentInvocation;
				Assert.AreEqual(parentInvocation.Parent, null);
				Assert.AreEqual(parentInvocation.State, state);
				Assert.AreEqual(parentInvocation.Action, null);
				Assert.AreEqual(parentInvocation.ActionWithState, action);
				if (debug) Assert.IsTrue(parentInvocation.InstantiationStackTrace != null);
				Assert.AreEqual(parentInvocation.Context, context);
				Assert.AreEqual(parentInvocation.OcDispatcher, dispatcher);
				Assert.AreEqual(parentInvocation.Priority, 1);

				object state1 = new object();
				
				Func<object, int> func1 = null;

				func1 = s1 =>
				{
					Assert.IsTrue(dispatcher.CheckAccess());

					Assert.AreEqual(OcDispatcher.Current, dispatcher);
					Invocation invocation1 = dispatcher.CurrentInvocation;
					Assert.AreEqual(invocation1.Parent, parentInvocation);
					Assert.AreEqual(invocation1.State, state1);
					Assert.AreEqual(invocation1.Action, null);
					if (debug) Assert.IsTrue(invocation1.InstantiationStackTrace != null);
					Assert.AreEqual(invocation1.Context, null);
					Assert.AreEqual(invocation1.OcDispatcher, dispatcher);
					Assert.AreEqual(invocation1.Priority, 0);

					return 3;
				};

				if (secondAsync)
				{
					dispatcher.InvokeAsync(func1, state1).PropertyChanged += (sender, args) =>
					{
						if (args.PropertyName == "Value")
							Assert.AreEqual(((InvocationResult<int>)sender).Value, 3);
					};
				}
				else Assert.AreEqual(dispatcher.Invoke(func1, state1).Value, 3);
			};

			Assert.IsFalse(dispatcher.CheckAccess());
			dispatcher.Invoke(action, state, 1, context);

			dispatcher.Pass();
			dispatcher.Pass();
			Assert.AreEqual(dispatcher, StaticInfo.OcDispatchers[dispatcher.ManagedThreadId]);
			Assert.IsTrue(dispatcher.CurrentInvocation == null);
			Assert.IsTrue(OcDispatcher.Current == null);
			dispatcher.Dispose();
		}

		[Test]
		public void TestFuncSync()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			object returnObject = new object();
			Assert.AreEqual(dispatcher.Invoke(() => returnObject).Value, returnObject);
			dispatcher.Dispose();
		}

		[Test]
		public void TestFuncAsync()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			object returnObject = new object();
			InvocationResult<object> invocationResult = dispatcher.InvokeAsync(() => returnObject);
			dispatcher.Pass();
			Assert.AreEqual(invocationResult.Value, invocationResult.Value);
			Assert.AreEqual(invocationResult.ToString(), "(ObservableComputations.InvocationResult<Object> (Value = '(System.Object)'))");
			dispatcher.Dispose();
		}

		[Test]
		public void TestFuncSyncState()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			object returnObject = new object();
			Assert.AreEqual(dispatcher.Invoke(s => returnObject, new object()).Value, returnObject);
			dispatcher.Dispose();
		}

		public class TestValue
		{
			#region Overrides of Object

			public override string ToString()
			{
				throw new Exception("Mesasage");
			}

			#endregion
		}

		[Test]
		public void TestFuncAsyncState()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			TestValue returnObject = new TestValue();
			InvocationResult<TestValue> invocationResult = dispatcher.InvokeAsync(s => returnObject, new object());
			bool propertyChanged = false;
			invocationResult.PropertyChanged += (sender, args) => propertyChanged = true;
			dispatcher.Pass();
			Assert.AreEqual(invocationResult.Value, returnObject);
			Assert.IsTrue(propertyChanged);
			Assert.AreEqual(invocationResult.ToString(), "(ObservableComputations.InvocationResult<TestValue> (Value = 'Mesasage'))");
			Assert.AreEqual(invocationResult.Invocation.Status, InvocationStatus.Executed);
			dispatcher.Dispose();
		}

		[Test, Combinatorial]
		public void TestExecuteOthers([Values(1, 2, 3, 4)] int mode)
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			ManualResetEventSlim mres = new ManualResetEventSlim();
			bool done = false;
			object context = new object();

			dispatcher.InvokeAsync(() =>
			{
				mres.Wait();
				Assert.AreEqual(done, false);

				switch (mode)
				{
					case 1:
						dispatcher.ExecuteOtherInvocations();
						break;
					case 2:
						dispatcher.ExecuteOtherInvocations(1);
						break;
					case 3:
						dispatcher.ExecuteOtherInvocations(TimeSpan.FromSeconds(1));
						break;
					case 4:
						dispatcher.ExecuteOtherInvocations((count, invocation) => invocation.Context == context);
						break;
				}

				Assert.AreEqual(done, true);
			});


			dispatcher.InvokeAsync(() =>
			{
				done = true;
			}, 0, context);

			mres.Set();

			dispatcher.Pass();

			Exception exception = null;

			try
			{
				switch (mode)
				{
					case 1:
						dispatcher.ExecuteOtherInvocations();
						break;
					case 2:
						dispatcher.ExecuteOtherInvocations(1);
						break;
					case 3:
						dispatcher.ExecuteOtherInvocations(TimeSpan.FromSeconds(1));
						break;
					case 4:
						dispatcher.ExecuteOtherInvocations((count, invocation) => invocation.Context == context);
						break;
				}
			}
			catch (Exception e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);

			dispatcher.Dispose();
		}

		[Test]
		public void TestSetThreadProperites()
		{
			OcConfiguration.SaveInstantiationStackTrace = true;
			OcDispatcher dispatcher = new OcDispatcher(2);
			Assert.AreEqual(dispatcher.GetQueueCount(0), 0);
			Assert.AreEqual(dispatcher.GetQueueCount(), 0);
			Assert.IsTrue(dispatcher.InstantiationStackTrace != null);
			ApartmentState apartmentState = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?  ApartmentState.Unknown : ApartmentState.MTA;
			Assert.AreEqual(dispatcher.NewInvocationBehaviour, NewInvocationBehaviour.Accept);
			Assert.AreEqual(dispatcher.Status, OcDispatcherStatus.ExecutingOrWait);
			Assert.AreEqual(dispatcher.GetThreadApartmentState(), apartmentState);
			Assert.AreEqual(dispatcher.PrioritiesNumber, 2);
			CultureInfo culture = CultureInfo.GetCultureInfo("ru-RU");
			dispatcher.ThreadIsBackground = true;
			dispatcher.ThreadName = "ThreadName";
			Assert.AreEqual(dispatcher.ToString(), "(ObservableComputations.OcDispatcher (Thread.Name = 'ThreadName'))");
			dispatcher.ThreadPriority = ThreadPriority.Highest;
			int managedThreadId = dispatcher.ManagedThreadId;
			Assert.IsTrue(dispatcher.ThreadState == (ThreadState.WaitSleepJoin | ThreadState.Background) || dispatcher.ThreadState == (ThreadState.Running | ThreadState.Background));

			Assert.AreEqual(dispatcher.ThreadIsBackground, true);
			Assert.AreEqual(dispatcher.ThreadName, "ThreadName");
			Assert.AreEqual(dispatcher.ThreadPriority, ThreadPriority.Highest);
			Assert.AreEqual(dispatcher.ThreadIsAlive, true);
			Assert.AreEqual(dispatcher.ManagedThreadId, managedThreadId);
			Assert.AreEqual(dispatcher.GetThreadApartmentState(), apartmentState);


			dispatcher.Invoke(() =>
			{
				dispatcher.ThreadCurrentCulture = culture;
				dispatcher.ThreadCurrentUICulture = culture;
				Assert.AreEqual(Thread.CurrentThread.CurrentCulture, culture);
				Assert.AreEqual(Thread.CurrentThread.CurrentUICulture, culture);
				Assert.AreEqual(dispatcher.ThreadCurrentCulture, culture);
				Assert.AreEqual(dispatcher.ThreadCurrentUICulture, culture);
				Assert.AreEqual(Thread.CurrentThread.IsBackground, true);
				Assert.AreEqual(Thread.CurrentThread.Name, "ThreadName");
				Assert.AreEqual(Thread.CurrentThread.Priority, ThreadPriority.Highest);
				Assert.AreEqual(Thread.CurrentThread.IsAlive, true);
				Assert.AreEqual(dispatcher.ThreadExecutionContext, Thread.CurrentThread.ExecutionContext);
				Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, managedThreadId);
				Assert.AreEqual(Thread.CurrentThread.GetApartmentState(), apartmentState);
			});

			ManualResetEventSlim mreDisposed = new ManualResetEventSlim(false);

			dispatcher.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(OcDispatcher.Status) 
					&& dispatcher.Status == OcDispatcherStatus.Disposed)
					mreDisposed.Set();
			};

			dispatcher.Dispose();
			Assert.AreEqual(dispatcher.NewInvocationBehaviour, NewInvocationBehaviour.Cancel);

			Exception exception = null;
			try
			{
				dispatcher.NewInvocationBehaviour = NewInvocationBehaviour.Accept;
			}
			catch (Exception e)
			{
				exception = e;
			}

			Assert.IsNotNull(exception);

			mreDisposed.Wait();
			Assert.AreEqual(dispatcher.Status, OcDispatcherStatus.Disposed);
		}

		[Test]
		public void TestInvocationBehaviourIgnore()
		{
			OcDispatcher dispatcher = new OcDispatcher();
			dispatcher.NewInvocationBehaviour = NewInvocationBehaviour.Cancel;
			bool called = false;
			dispatcher.Invoke(() => {called = true;});
			dispatcher.Invoke(s => {called = true;}, new object());
			dispatcher.InvokeAsync(() => {called = true;});
			dispatcher.InvokeAsync(s => {called = true;}, new object());

			dispatcher.Invoke(() => {called = true; return 0;});
			dispatcher.Invoke(s => {called = true; return 0;}, new object());
			dispatcher.InvokeAsync(() => {called = true; return 0;});
			dispatcher.InvokeAsync(s => {called = true; return 0;}, new object());

			dispatcher.InvokeAsyncAwaitable(() => {called = true;});
			dispatcher.InvokeAsyncAwaitable(s => {called = true;}, new object());
			dispatcher.InvokeAsyncAwaitable(() => {called = true; return 0;});
			dispatcher.InvokeAsyncAwaitable(s => {called = true; return 0;}, new object());


			dispatcher.NewInvocationBehaviour = NewInvocationBehaviour.Accept;
			dispatcher.Pass();

			Assert.IsFalse(called);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvocationBehaviourThrowException()
		{
			OcDispatcher dispatcher = new OcDispatcher();
			dispatcher.NewInvocationBehaviour = NewInvocationBehaviour.ThrowException;
			bool called = false;
			Exception exception;

			void invoke(Action action)
			{
				exception = null;
				try
				{
					action();
				}
				catch (Exception e)
				{
					exception = e;
				}

				Assert.IsTrue(exception != null);
			}

			invoke(() => dispatcher.Invoke(() => {called = true;}));
			invoke(() => dispatcher.Invoke(s => {called = true;}, new object()));
			invoke(() => dispatcher.InvokeAsync(() => {called = true;}));
			invoke(() => dispatcher.InvokeAsync(s => {called = true;}, new object()));

			invoke(() => dispatcher.Invoke(() => {called = true; return 0;}));
			invoke(() => dispatcher.Invoke(s => {called = true; return 0;}, new object()));
			invoke(() => dispatcher.InvokeAsync(() => {called = true; return 0;}));
			invoke(() => dispatcher.InvokeAsync(s => {called = true; return 0;}, new object()));

			dispatcher.NewInvocationBehaviour = NewInvocationBehaviour.Accept;
			dispatcher.Pass();

			Assert.IsFalse(called);
			dispatcher.Dispose();
		}

		[Test]
		public void DisableThreadComObjectEagerCleanupTest()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return;

			OcDispatcher dispatcher = new OcDispatcher();
			dispatcher.DisableThreadComObjectEagerCleanup();
			dispatcher.Dispose();
		}

		[Test]
		public void DisposeTest()
		{
			OcDispatcher dispatcher = new OcDispatcher();
			ManualResetEventSlim mre = new ManualResetEventSlim(false);
			bool invoked1 = false;
			bool invoked2 = false;
			bool freezed = false;
			InvocationResult<bool> invocationResult1 = null;
			InvocationResult<bool> invocationResult2 = null;

			dispatcher.InvokeAsync(() =>
			{
				dispatcher.InvokeAsync(() =>
				{
					freezed = true;
					mre.Wait();
				});


				invocationResult1 = dispatcher.Invoke(() => 
				{
					mre.Wait();
					invoked1 = true;
					return true;
				});

			});

			Thread thread = new Thread(
				() => 
				{
					while (!freezed)
					{
						
					}

					invocationResult2 = dispatcher.Invoke(() => 
						{
							mre.Wait();
							invoked2 = true;
							return true;
						});
				});
			thread.Start();

			ManualResetEventSlim mreDisposed = new ManualResetEventSlim(false);
			dispatcher.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(OcDispatcher.Status) 
					&& dispatcher.Status == OcDispatcherStatus.Disposed)
					mreDisposed.Set();
			};

			while (dispatcher.GetQueueCount() != 2)
			{
				
			}

			dispatcher.Dispose();
			mre.Set();
			thread.Join();
			mreDisposed.Wait();

			Assert.IsFalse(invoked1);
			Assert.IsFalse(invoked2);
			Assert.AreEqual(invocationResult1.Invocation.Status, InvocationStatus.Canceled);
			Assert.AreEqual(invocationResult2.Invocation.Status, InvocationStatus.Canceled);
		}

		[Test]
		public void DoerTest()
		{
			OcConfiguration.SaveOcDispatcherInvocationExecutionStackTrace = true;
			OcDispatcher dispatcher = new OcDispatcher();
			ManualResetEventSlim mre = new ManualResetEventSlim(false);

			dispatcher.InvokeAsync(() =>
			{
				mre.Wait();
			});

				
			Invocation doer = dispatcher.InvokeAsync(() =>
			{
				dispatcher.Pass();
			});

			Invocation invocation = dispatcher.InvokeAsync(() => {});

			mre.Set();
			dispatcher.Pass();

			Assert.AreEqual(invocation.Executor, doer);
			Assert.IsNotNull(invocation.ExecutionStackTrace);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvokeAsyncAwaitableDispatcherThread()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 0;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();

			dispatcher.Invoke(() =>
			{
				Invocation invocation = dispatcher.CurrentInvocation;

				dispatcher.InvokeAsync(async () =>
				{
					count++;
					Assert.IsTrue(dispatcher.CurrentInvocation.SetSynchronizationContext);
					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, invocation);
					Assert.AreEqual(((OcDispatcherSynchronizationContext) SynchronizationContext.Current).OcDispatcher, dispatcher);

					await dispatcher.InvokeAsyncAwaitable(() =>
					{
						Assert.AreEqual(count, 1);
						count++;
						Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 0);
						Assert.AreEqual(dispatcher.CurrentInvocation.Context, null);
					});

					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, invocation);
					Assert.AreEqual(count, 2);
					count++;
					Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					mres.Set();
				}, 1, context, true);
			});

			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvokeAsyncAwaitableStateDispatcherThread()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 0;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();

			dispatcher.Invoke(() =>
			{
				Invocation invocation = dispatcher.CurrentInvocation;

				dispatcher.InvokeAsync(async () =>
				{
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, invocation);
					Assert.IsTrue(dispatcher.CurrentInvocation.SetSynchronizationContext);

					await dispatcher.InvokeAsyncAwaitable(state =>
					{
						Assert.AreEqual(count, 1);
						count++;
						Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 0);
						Assert.AreEqual(dispatcher.CurrentInvocation.Context, null);
					}, new object());

					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, invocation);
					Assert.AreEqual(count, 2);
					count++;
					Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					mres.Set();
				}, 1, context, true);
			});

			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvokeAsyncAwaitableFuncDispatcherThread()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 0;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();

			dispatcher.Invoke(() =>
			{
				Invocation invocation = dispatcher.CurrentInvocation;

				dispatcher.InvokeAsync(async () =>
				{
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, invocation);
					Assert.IsTrue(dispatcher.CurrentInvocation.SetSynchronizationContext);

					int returnCount = await dispatcher.InvokeAsyncAwaitable(() =>
					{
						Assert.AreEqual(count, 1);
						count++;
						Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 0);
						Assert.AreEqual(dispatcher.CurrentInvocation.Context, null);
						return count;
					});

					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, invocation);
					Assert.AreEqual(returnCount, 2);
					Assert.AreEqual(count, 2);
					count++;
					Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					mres.Set();
				}, 1, context, true);
			});

			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvokeAsyncAwaitableFuncStateDispatcherThread()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 0;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();

			dispatcher.Invoke(() =>
			{
				Invocation invocation = dispatcher.CurrentInvocation;

				dispatcher.InvokeAsync(async () =>
				{
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, invocation);
					Assert.IsTrue(dispatcher.CurrentInvocation.SetSynchronizationContext);

					int returnCount = await dispatcher.InvokeAsyncAwaitable(state =>
					{
						Assert.AreEqual(count, 1);
						count++;
						Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 0);
						Assert.AreEqual(dispatcher.CurrentInvocation.Context, null);
						return count;
					}, new object());

					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, invocation);
					Assert.AreEqual(returnCount, 2);
					Assert.AreEqual(count, 2);
					count++;
					Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					mres.Set();
				}, 1, context, true);
			});


			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}

		internal class TestSynchronizationConext : SynchronizationContext
		{
			public override void Post(SendOrPostCallback d, object state)
			{
				d(state);
			}
		}

		[Test]
		public void TestInvokeAsyncAwaitableNonDispatcherThread()
		{
			TestSynchronizationConext testSynchronizationConext = new TestSynchronizationConext();
			SynchronizationContext.SetSynchronizationContext(testSynchronizationConext);

			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 0;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();

			async void test()
			{
				count++;

				await dispatcher.InvokeAsyncAwaitable(() =>
				{
					Assert.AreEqual(count, 1);
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
				}, 1, context);

				Assert.AreEqual(count, 2);
				count++;
				Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
				Assert.AreEqual(dispatcher.CurrentInvocation, null);
				mres.Set();
			}

			test();
			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvokeAsyncAwaitableStateNonDispatcherThread()
		{
			TestSynchronizationConext testSynchronizationConext = new TestSynchronizationConext();
			SynchronizationContext.SetSynchronizationContext(testSynchronizationConext);

			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 0;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();

			async void test()
			{
				count++;

				await dispatcher.InvokeAsyncAwaitable(state =>
				{
					Assert.AreEqual(count, 1);
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
				}, new object(), 1, context);

				Assert.AreEqual(count, 2);
				count++;
				Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
				Assert.AreEqual(dispatcher.CurrentInvocation, null);
				mres.Set();
			}

			test();
			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvokeAsyncAwaitableFuncNonDispatcherThread()
		{
			TestSynchronizationConext testSynchronizationConext = new TestSynchronizationConext();
			SynchronizationContext.SetSynchronizationContext(testSynchronizationConext);

			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 0;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();

			async void test()
			{
				count++;

				int count1 = await dispatcher.InvokeAsyncAwaitable(() =>
				{
					Assert.AreEqual(count, 1);
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					return count;
				}, 1, context);

				Assert.AreEqual(count1, 2);
				Assert.AreEqual(count, 2);
				count++;
				Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
				Assert.AreEqual(dispatcher.CurrentInvocation, null);
				mres.Set();
			}

			test();
			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvokeAsyncAwaitableFuncStateNonDispatcherThread()
		{
			TestSynchronizationConext testSynchronizationConext = new TestSynchronizationConext();
			SynchronizationContext.SetSynchronizationContext(testSynchronizationConext);

			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 0;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();

			async void test()
			{
				count++;

				int count1 = await dispatcher.InvokeAsyncAwaitable(state =>
				{
					Assert.AreEqual(count, 1);
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					return count;
				}, new object(), 1, context);

				Assert.AreEqual(count1, 2);
				Assert.AreEqual(count, 2);
				count++;
				Assert.AreNotEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
				Assert.AreEqual(dispatcher.CurrentInvocation, null);
				mres.Set();
			}

			test();
			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}

		[Test]
		public void TestCallAwaitable()
		{
			TestSynchronizationConext testSynchronizationConext = new TestSynchronizationConext();
			SynchronizationContext.SetSynchronizationContext(testSynchronizationConext);

			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 1;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();
			Invocation parent;


			dispatcher.InvokeAsync(() =>
			{
				parent = dispatcher.CurrentInvocation;

				dispatcher.Invoke( async () =>
				{
					Assert.AreEqual(count, 1);
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, parent);

					OcDispatcherSynchronizationContext synchronizationContext = (OcDispatcherSynchronizationContext) SynchronizationContext.Current;
					Assert.AreEqual(synchronizationContext.Priority, 1);
					Assert.AreEqual(synchronizationContext.Context, context);
					Assert.AreEqual(synchronizationContext.ParentInvocation, parent);

					await Task.Run(() => count++);

					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, parent);
					count++;
					mres.Set();
					return 1;
				}, 1, context, true);
			});

			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 4);
			dispatcher.Dispose();
		}

		[Test]
		public void TestCallAwaitableState()
		{
			TestSynchronizationConext testSynchronizationConext = new TestSynchronizationConext();
			SynchronizationContext.SetSynchronizationContext(testSynchronizationConext);

			OcDispatcher dispatcher = new OcDispatcher(2);
			int count = 1;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);
			object context = new object();
			Invocation parent;

			dispatcher.InvokeAsync(() =>
			{
				parent = dispatcher.CurrentInvocation;

				dispatcher.Invoke(async s =>
				{
					Assert.AreEqual(count, 1);
					count++;
					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, parent);

					OcDispatcherSynchronizationContext synchronizationContext = (OcDispatcherSynchronizationContext) SynchronizationContext.Current;
					Assert.AreEqual(synchronizationContext.Priority, 1);
					Assert.AreEqual(synchronizationContext.Context, context);
					Assert.AreEqual(synchronizationContext.ParentInvocation, parent);

					await Task.Run(() => count++);

					Assert.AreEqual(dispatcher.CurrentInvocation.Priority, 1);
					Assert.AreEqual(dispatcher.CurrentInvocation.Context, context);
					Assert.AreEqual(dispatcher.CurrentInvocation.Parent, parent);
					count++;
					mres.Set();
				}, new object(), 1, context, true);
			});

			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 4);
			dispatcher.Dispose();
		}


	}
}
