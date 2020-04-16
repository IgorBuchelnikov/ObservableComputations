using System;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class ValueProcessingTest
	{
		public class Item
		{
			public bool ProcessedAsNew;
			public object Token = new object();
		}


		[Test]
		public void ValuesProcessing_Test()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);
			object token = null;
			itemScalar.ValuesProcessing<Item, object>((i, current, sender, eventArgs) =>
			{
				Assert.AreEqual(current.Value, token);
				i.ProcessedAsNew = true;
				return i.Token;
			});
			Assert.IsTrue(item.ProcessedAsNew);
			token = item.Token;
			item = new Item();
			itemScalar.Change(item);
			Assert.IsTrue(item.ProcessedAsNew);
		}
	}
}