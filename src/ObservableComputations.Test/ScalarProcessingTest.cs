using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class ScalarProcessingTest
	{
		OcConsumer consumer = new OcConsumer();

		public class Item
		{
			public bool ProcessedAsNew;
			public object Token = new object();
		}


		[Test]
		public void ScalarProcessing_Test()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);
			object token = null;
			itemScalar.ScalarProcessing<Item, object>((i, current, val) =>
			{
				Assert.AreEqual(current.ValueObject, token);
				i.ProcessedAsNew = true;
				return i.Token;
			}).For(consumer);
			Assert.IsTrue(item.ProcessedAsNew);
			token = item.Token;
			item = new Item();
			itemScalar.Change(item);
			Assert.IsTrue(item.ProcessedAsNew);
			consumer.Dispose();
		}
	}
}