using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
					Assert.IsFalse(DebugInfo.ExecutingOcDispatcherInvocationStacks.ContainsKey(dispatcher.ManagedThreadId));
				}
				else
				{
					ReadOnlyCollection<Invocation> invocations = DebugInfo.ExecutingOcDispatcherInvocationStacks[dispatcher.ManagedThreadId];
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
						Assert.IsFalse(DebugInfo.ExecutingOcDispatcherInvocationStacks.ContainsKey(dispatcher.ManagedThreadId));
					}
					else
					{
						ReadOnlyCollection<Invocation> invocations1 = DebugInfo.ExecutingOcDispatcherInvocationStacks[dispatcher.ManagedThreadId];
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

				if (secondAsync) dispatcher.BeginInvoke(action1, 1, context1);
				else dispatcher.Invoke(action1, 1, context1);
			};

			dispatcher.Invoke(action, 1, context);

			dispatcher.Pass();
			dispatcher.Pass();
			Assert.IsFalse(DebugInfo.ExecutingOcDispatcherInvocationStacks.ContainsKey(dispatcher.ManagedThreadId));
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
					Assert.IsFalse(DebugInfo.ExecutingOcDispatcherInvocationStacks.ContainsKey(dispatcher.ManagedThreadId));
				}
				else
				{
					ReadOnlyCollection<Invocation> invocations = DebugInfo.ExecutingOcDispatcherInvocationStacks[dispatcher.ManagedThreadId];

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
						Assert.IsFalse(DebugInfo.ExecutingOcDispatcherInvocationStacks.ContainsKey(dispatcher.ManagedThreadId));
					}
					else
					{
						ReadOnlyCollection<Invocation> invocations1 = DebugInfo.ExecutingOcDispatcherInvocationStacks[dispatcher.ManagedThreadId];
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

				if (secondAsync) dispatcher.BeginInvoke(action1, state1, 1, context1);
				else dispatcher.Invoke(action1, state1, 1, context1);
			};

			dispatcher.Invoke(action, state, 1, context);

			dispatcher.Pass();
			dispatcher.Pass();
			Assert.IsFalse(DebugInfo.ExecutingOcDispatcherInvocationStacks.ContainsKey(dispatcher.ManagedThreadId));
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
			InvocationResult<object> invocationResult = dispatcher.BeginInvoke(() => returnObject);
			dispatcher.Pass();
			Assert.AreEqual(invocationResult.Value, invocationResult.Value);
			Assert.AreEqual(invocationResult.ToString(), "(ObservableComputations.InvocationResult<Object> (Value = '(System.Object)'))");
			dispatcher.Dispose();
		}

		[Test, Combinatorial]
		public void TestDoOthers([Values(1, 2, 3)] int mode)
		{
			OcDispatcher dispatcher = new OcDispatcher(2);
			ManualResetEventSlim mres = new ManualResetEventSlim();
			bool done = false;
			dispatcher.BeginInvoke(() =>
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
				}

				Assert.AreEqual(done, true);
			});

			dispatcher.BeginInvoke(() =>
			{
				done = true;
			});

			mres.Set();

			dispatcher.Pass();
			dispatcher.Dispose();
		}


	}
}
