using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class ValuesProcessingVoidTest
	{
		public class Item
		{
			public bool ProcessedAsNew;
		}


		[Test]
		public void  ValuesProcessingVoid_Test()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);
			itemScalar.ValuesProcessing((i, current, sender, eventArgs) =>
			{
				i.ProcessedAsNew = true;
			});
			Assert.IsTrue(item.ProcessedAsNew);
			item = new Item();
			itemScalar.Change(item);
			itemScalar.Change(item);

		}
	}
}