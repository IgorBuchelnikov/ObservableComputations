using System;
using System.Linq.Expressions;
using System.Threading;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class MiscTests
	{
		[Test]
		public void DoSetDebugTagTest()
		{
			Computing<string> computing = new Computing<string>(() => "").Do(c => c.SetDebugTag("DebugTag"));
			Assert.AreEqual(computing.DebugTag, "DebugTag");
		}

		private Computing<int> _computing;

		[Test]
		public void TestComputingsExecutingUserCode()
		{
			OcConfiguration.TrackComputingsExecutingUserCode = true;
			_computing = new Computing<int>(() => test());
			OcConsumer consumer = new OcConsumer();
			_computing.For(consumer);
			consumer.Dispose();
		}

		int  test()
		{
			Assert.AreEqual(StaticInfo.ComputingsExecutingUserCode[Thread.CurrentThread.ManagedThreadId], _computing);
			return 0;
		}

		[Test]
		public void TestSameAs()
		{
			object nullO = null; 
			object o1 = new object();
			object o2 = new object();

			Assert.IsTrue(o1.IsSameAs(o1));
			Assert.IsFalse(o1.IsSameAs(o2));
			Assert.IsTrue(nullO.IsSameAs(nullO));
			Assert.IsFalse(nullO.IsSameAs(o1));
			Assert.IsFalse(o1.IsSameAs(nullO));

		}

		[Test]
		public void TestUsing()
		{
			Expression<Func<int, int>> expression = v => 1;
			var @using = 0.Using(expression);
			Assert.AreEqual(@using.Argument, 0);
			Assert.AreEqual(@using.GetValueExpressionUsing, expression);
		}
	}
}
