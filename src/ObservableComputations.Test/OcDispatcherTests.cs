using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class OcDispatcherTests
	{
		[Test, Combinatorial]
		public void TestDebugInfo(
			[Values(true, false)] bool debug,
			[Values(true, false)]bool secondAsync)
		{
			Configuration.SaveOcDispatcherInvocationStackTrace = debug;
			Configuration.TrackOcDispatcherInvocations = debug;

			object context = new object();
			OcDispatcher dispatcher = new OcDispatcher(2);
			Action action = null;

			action = () =>
			{
				if (!debug)
				{
					Assert.IsTrue(dispatcher.InvocationStack.Count == 0);
				}
				else
				{
					ReadOnlyCollection<Invocation> invocations = dispatcher.InvocationStack;
					Assert.AreEqual(invocations.Count, 1);
					Invocation invocation = invocations[0];
					Assert.AreEqual(invocation.Action, action);
					Assert.AreEqual(invocation.ActionWithState, null);
					Assert.AreEqual(invocation.State, null);
					Assert.IsTrue(invocation.CallStackTrace != null);
					Assert.AreEqual(invocation.Context, context);
					Assert.AreEqual(invocation.OcDispatcher, dispatcher);
				}

				object context1 = new object();	
				
				Action action1 = null;

				action1 = () =>
				{
					if (!debug)
					{
						Assert.IsTrue(dispatcher.InvocationStack.Count == 0);
					}
					else
					{
						ReadOnlyCollection<Invocation> invocations1 = dispatcher.InvocationStack;
						Assert.AreEqual(invocations1.Count, secondAsync ? 1 : 2);
						Invocation invocation1 = invocations1[0];
						Assert.AreEqual(invocation1.Action, action1);
						Assert.AreEqual(invocation1.ActionWithState, null);
						Assert.AreEqual(invocation1.State, null);
						Assert.IsTrue(invocation1.CallStackTrace != null);
						Assert.AreEqual(invocation1.Context, context1);
						Assert.AreEqual(invocation1.OcDispatcher, dispatcher);
					}
				};

				if (secondAsync) dispatcher.InvokeAsync(action1, 1, context1);
				else dispatcher.Invoke(action1, 1, context1);
			};

			dispatcher.Invoke(action, 1, context);

			dispatcher.Pass();
			dispatcher.Pass();
			Assert.IsTrue(dispatcher.InvocationStack.Count == 0);
			dispatcher.Dispose();
		}

		[Test, Combinatorial]
		public void TestDebugInfoState(
			[Values(true, false)] bool debug, 
			[Values(true, false)]bool secondAsync)
		{
			Configuration.SaveOcDispatcherInvocationStackTrace = debug;
			Configuration.TrackOcDispatcherInvocations = debug;

			object context = new object();
			object state = new object();
			OcDispatcher dispatcher = new OcDispatcher(2);
			Action<object> action = null;

			action = s =>
			{
				Assert.AreEqual(s, state);

				if (!debug)
				{
					Assert.IsTrue(dispatcher.InvocationStack.Count == 0);
				}
				else
				{
					ReadOnlyCollection<Invocation> invocations = dispatcher.InvocationStack;

					Assert.AreEqual(invocations.Count, 1);
					Invocation invocation = invocations[0];
					Assert.AreEqual(invocation.State, state);
					Assert.AreEqual(invocation.Action, null);
					Assert.AreEqual(invocation.ActionWithState, action);
					Assert.IsTrue(invocation.CallStackTrace != null);
					Assert.AreEqual(invocation.Context, context);
					Assert.AreEqual(invocation.OcDispatcher, dispatcher);					
				}

				object context1 = new object();
				object state1 = new object();
				
				Action<object> action1 = null;

				action1 = s1 =>
				{
					if (!debug)
					{
						Assert.IsTrue(dispatcher.InvocationStack.Count == 0);
					}
					else
					{
						ReadOnlyCollection<Invocation> invocations1 = dispatcher.InvocationStack;
						Assert.AreEqual(s1, state1);
						Assert.AreEqual(invocations1.Count, secondAsync ? 1 : 2);
						Invocation invocation1 = invocations1[0];
						Assert.AreEqual(invocation1.State, state1);
						Assert.AreEqual(invocation1.Action, null);
						Assert.AreEqual(invocation1.ActionWithState, action1);
						Assert.IsTrue(invocation1.CallStackTrace != null);
						Assert.AreEqual(invocation1.Context, context1);
						Assert.AreEqual(invocation1.OcDispatcher, dispatcher);						
					}
				};

				if (secondAsync) dispatcher.InvokeAsync(action1, state1, 1, context1);
				else dispatcher.Invoke(action1, state1, 1, context1);
			};

			dispatcher.Invoke(action, state, 1, context);

			dispatcher.Pass();
			dispatcher.Pass();
			Assert.IsTrue(dispatcher.InvocationStack.Count == 0);
			dispatcher.Dispose();
		}

		[Test]
		public void TestFuncSync()
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			object returnObject = new object();
			Assert.AreEqual(dispatcher.Invoke(() => returnObject), returnObject);
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
			Assert.AreEqual(dispatcher.Invoke(s => returnObject, new object()), returnObject);
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
			Assert.AreEqual(invocationResult.Ripe, true);
			dispatcher.Dispose();
		}

		[Test, Combinatorial]
		public void TestDoOthers([Values(1, 2, 3, 4)] int mode)
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
						dispatcher.DoOthers();
						break;
					case 2:
						dispatcher.DoOthers(1);
						break;
					case 3:
						dispatcher.DoOthers(TimeSpan.FromSeconds(1));
						break;
					case 4:
						dispatcher.DoOthers((count, invocation) => invocation.Context == context);
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
						dispatcher.DoOthers();
						break;
					case 2:
						dispatcher.DoOthers(1);
						break;
					case 3:
						dispatcher.DoOthers(TimeSpan.FromSeconds(1));
						break;
					case 4:
						dispatcher.DoOthers((count, invocation) => invocation.Context == context);
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
			Configuration.SaveInstantiatingStackTrace = true;
			OcDispatcher dispatcher = new OcDispatcher(2);
			Assert.AreEqual(dispatcher.GetQueueCount(0), 0);
			Assert.IsTrue(dispatcher.InstantiatingStackTrace != null);
			ApartmentState apartmentState = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?  ApartmentState.Unknown : ApartmentState.MTA;
			Assert.AreEqual(dispatcher.NewInvocationBehaviour, NewInvocationBehaviour.Accept);
			Assert.AreEqual(dispatcher.State, OcDispatcherState.RunOrWait);
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
				if (args.PropertyName == nameof(OcDispatcher.State) 
					&& dispatcher.State == OcDispatcherState.Disposed)
					mreDisposed.Set();
			};

			dispatcher.Dispose();
			Assert.AreEqual(dispatcher.NewInvocationBehaviour, NewInvocationBehaviour.Ignore);

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
			Assert.AreEqual(dispatcher.State, OcDispatcherState.Disposed);
		}

		[Test]
		public void TestInvocationBehaviourIgnore()
		{
			OcDispatcher dispatcher = new OcDispatcher();
			dispatcher.NewInvocationBehaviour = NewInvocationBehaviour.Ignore;
			bool called = false;
			dispatcher.Invoke(() => {called = true;});
			dispatcher.Invoke(s => {called = true;}, new object());
			dispatcher.InvokeAsync(() => {called = true;});
			dispatcher.InvokeAsync(s => {called = true;}, new object());

			dispatcher.Invoke(() => {called = true; return 0;});
			dispatcher.Invoke(s => {called = true; return 0;}, new object());
			dispatcher.InvokeAsync(() => {called = true; return 0;});
			dispatcher.InvokeAsync(s => {called = true; return 0;}, new object());

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

			dispatcher.InvokeAsync(() =>
			{
				dispatcher.InvokeAsync(() =>
				{
					freezed = true;
					mre.Wait();
				});

				dispatcher.Invoke(() => invoked1 = true);

			});

			Thread thread = new Thread(
				() => 
				{
					while (!freezed)
					{
						
					}

					dispatcher.Invoke(() => invoked2 = true);
				});
			thread.Start();

			ManualResetEventSlim mreDisposed = new ManualResetEventSlim(false);
			dispatcher.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(OcDispatcher.State) 
					&& dispatcher.State == OcDispatcherState.Disposed)
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
		}

		[Test]
		public void TestFailture()
		{
			OcDispatcher dispatcher = new OcDispatcher();

			dispatcher.InvokeAsync(() =>
			{
				throw new Exception();
			});

			Thread.Sleep(1000);

			Assert.AreEqual(dispatcher.State, OcDispatcherState.Failture);
			dispatcher.Dispose();
		}

		[Test]
		public void TestInvokeAsyncAwait()
		{
			OcDispatcher dispatcher = new OcDispatcher();
			int count = 0;
			int id = Thread.CurrentThread.ManagedThreadId;
			ManualResetEventSlim mres = new ManualResetEventSlim(false);

			dispatcher.Invoke(async () =>
			{
				count++;

				await Task.Run(() => dispatcher.Invoke(() =>
				{
					Assert.AreEqual(count, 1);
					count++;
				}));

				Assert.AreEqual(count, 2);
				count++;
				Assert.AreEqual(Thread.CurrentThread.ManagedThreadId, dispatcher.ManagedThreadId);
				mres.Set();
			});

			mres.Wait();
			mres.Dispose();
			Assert.AreEqual(count, 3);
			dispatcher.Dispose();
		}
	}
}
