using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class ScalarProcessingVoidTest
	{
		OcConsumer consumer = new OcConsumer();

		public class Item
		{
			public bool ProcessedAsNew;
			public bool ProcessedAsOld;
		}


		[Test]
		public void  ScalarProcessingVoid_Test()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);
			itemScalar.ScalarProcessing(
				(newItem, current) =>
				{
					newItem.ProcessedAsNew = true;
				},
				(oldItem, current) =>
				{
					oldItem.ProcessedAsOld = true;
				}).For(consumer);
			Assert.IsTrue(item.ProcessedAsNew);
			Item newItem1 = new Item();
			itemScalar.Change(newItem1);
			Assert.IsTrue(item.ProcessedAsNew);
			Assert.IsTrue(item.ProcessedAsOld);
			Assert.IsTrue(newItem1.ProcessedAsNew);
			consumer.Dispose();
			Assert.IsTrue(item.ProcessedAsNew);
			Assert.IsTrue(item.ProcessedAsOld);
			Assert.IsTrue(newItem1.ProcessedAsNew);
			Assert.IsTrue(newItem1.ProcessedAsOld);
		}
	}
}