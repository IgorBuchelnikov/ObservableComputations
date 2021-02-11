using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class OcDispatcherTests
	{
		[Test]
		public void TestDebugInfo()
		{
			Configuration.SaveInstantiatingStackTrace = true;
			Configuration.TrackComputingsExecutingUserCode = true;
			Configuration.SaveOcDispatcherInvocationStackTrace = true;
			Configuration.TrackOcDispatcherInvocations = true;

			object context = new object();
			OcDispatcher dispatcher = new OcDispatcher(2);
			Action action = null;

			action = () =>
			{
				ReadOnlyCollection<Invocation> invocations = DebugInfo.ExecutingOcDispatcherInvocations[dispatcher.ManagedThreadId];
				Assert.AreEqual(invocations.Count, 1);
				Invocation invocation = invocations[0];
				Assert.AreEqual(invocation.Action, action);
				Assert.AreEqual(invocation.ActionWithState, null);
				Assert.AreEqual(invocation.State, null);
				Assert.IsTrue(invocation.CallStackTrace != null);
				Assert.AreEqual(invocation.Context, context);
				Assert.AreEqual(invocation.OcDispatcher, dispatcher);

				object context1 = new object();	
				
				Action action1 = null;

				action1 = () =>
				{
					ReadOnlyCollection<Invocation> invocations1 = DebugInfo.ExecutingOcDispatcherInvocations[dispatcher.ManagedThreadId];
					Assert.AreEqual(invocations1.Count, 2);
					Invocation invocation1 = invocations1[0];
					Assert.AreEqual(invocation1.Action, action1);
					Assert.AreEqual(invocation1.ActionWithState, null);
					Assert.AreEqual(invocation1.State, null);
					Assert.IsTrue(invocation1.CallStackTrace != null);
					Assert.AreEqual(invocation1.Context, context1);
					Assert.AreEqual(invocation1.OcDispatcher, dispatcher);
				};

				dispatcher.BeginInvoke(action1, 1, context1);
			};

			dispatcher.Invoke(action, 1, context);

			dispatcher.Pass();
		}

		[Test]
		public void TestDebugInfoState()
		{
			Configuration.SaveInstantiatingStackTrace = true;
			Configuration.TrackComputingsExecutingUserCode = true;
			Configuration.SaveOcDispatcherInvocationStackTrace = true;
			Configuration.TrackOcDispatcherInvocations = true;

			object context = new object();
			object state = new object();
			OcDispatcher dispatcher = new OcDispatcher(2);
			Action<object> action = null;

			action = s =>
			{
				ReadOnlyCollection<Invocation> invocations = DebugInfo.ExecutingOcDispatcherInvocations[dispatcher.ManagedThreadId];
				Assert.AreEqual(s, state);
				Assert.AreEqual(invocations.Count, 1);
				Invocation invocation = invocations[0];
				Assert.AreEqual(invocation.State, state);
				Assert.AreEqual(invocation.Action, null);
				Assert.AreEqual(invocation.ActionWithState, action);
				Assert.IsTrue(invocation.CallStackTrace != null);
				Assert.AreEqual(invocation.Context, context);
				Assert.AreEqual(invocation.OcDispatcher, dispatcher);

				object context1 = new object();
				object state1 = new object();
				
				Action<object> action1 = null;

				action1 = s1 =>
				{
					ReadOnlyCollection<Invocation> invocations1 = DebugInfo.ExecutingOcDispatcherInvocations[dispatcher.ManagedThreadId];
					Assert.AreEqual(s1, state1);
					Assert.AreEqual(invocations1.Count, 2);
					Invocation invocation1 = invocations1[0];
					Assert.AreEqual(invocation1.State, state1);
					Assert.AreEqual(invocation1.Action, null);
					Assert.AreEqual(invocation1.ActionWithState, action1);
					Assert.IsTrue(invocation1.CallStackTrace != null);
					Assert.AreEqual(invocation1.Context, context1);
					Assert.AreEqual(invocation1.OcDispatcher, dispatcher);
				};

				dispatcher.BeginInvoke(action1, state1, 1, context1);
			};

			dispatcher.Invoke(action, state, 1, context);

			dispatcher.Pass();
		}
	}
}
