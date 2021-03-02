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
	}
}
