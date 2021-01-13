// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public class ScalarProcessingTest : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item
		{
			public bool ProcessedAsNew;
			public bool ProcessedAsOld;
			public object Token = new object();
		}


		[Test]
		public void ScalarProcessing_Test()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);
			object token = null;
			itemScalar.ScalarProcessing<Item, object>(
				(newItem, current) =>
				{
					newItem.ProcessedAsNew = true;
					return newItem.Token;
				},
				(oldItem, current, retVal) =>
				{
					Assert.AreEqual(retVal, token);
					oldItem.ProcessedAsOld = true;
				}).For(consumer);
			Assert.IsTrue(item.ProcessedAsNew);
			token = item.Token;
			Item newItem1 = new Item();
			itemScalar.Change(newItem1);
			Assert.IsTrue(item.ProcessedAsNew);
			Assert.IsTrue(item.ProcessedAsOld);
			Assert.IsTrue(newItem1.ProcessedAsNew);
			token = newItem1.Token;
			consumer.Dispose();
			Assert.IsTrue(item.ProcessedAsNew);
			Assert.IsTrue(item.ProcessedAsOld);
			Assert.IsTrue(newItem1.ProcessedAsNew);
			Assert.IsTrue(newItem1.ProcessedAsOld);
		}

		public ScalarProcessingTest(bool debug) : base(debug)
		{
		}
	}
}
