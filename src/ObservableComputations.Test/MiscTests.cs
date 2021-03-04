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
			Configuration.TrackComputingsExecutingUserCode = true;
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
	}
}
