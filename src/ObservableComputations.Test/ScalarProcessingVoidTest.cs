// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(true)]
	[TestFixture(false)]
	public partial class ScalarProcessingVoidTest : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item
		{
			public int ProcessedAsNew;
			public int ProcessedAsOld;
		}

		Action<Item, ScalarProcessingVoid<Item>> _newValueProcessor = (newItem, current) =>
		{
			newItem.ProcessedAsNew++;
		};
		Action<Item, ScalarProcessingVoid<Item>> _oldValueProcessor = (oldItem, current) =>
		{
			oldItem.ProcessedAsOld++;
		};

		[Test]
		public void  ScalarProcessingVoid_Test()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);

			ScalarProcessingVoid<Item> scalarProcessingVoid = itemScalar.ScalarProcessing(
				_newValueProcessor,
				_oldValueProcessor).For(consumer);

			Assert.AreEqual(scalarProcessingVoid.Source, itemScalar);
			Assert.AreEqual(scalarProcessingVoid.NewValueProcessor, _newValueProcessor);
			Assert.AreEqual(scalarProcessingVoid.OldValueProcessor, _oldValueProcessor);

			test(item, itemScalar);
			consumer.Dispose();
		}

		[Test]
		public void  ScalarProcessingVoid_Test1()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);

			ScalarProcessingVoid<Item> scalarProcessingVoid = Expr.Is(() => itemScalar.Value).ScalarProcessing(
				_newValueProcessor,
				_oldValueProcessor).For(consumer);

			Assert.AreEqual(scalarProcessingVoid.NewValueProcessor, _newValueProcessor);
			Assert.AreEqual(scalarProcessingVoid.OldValueProcessor, _oldValueProcessor);

			test(item, itemScalar);
			consumer.Dispose();
		}

		private void test(Item item, Scalar<Item> itemScalar)
		{
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Item newItem1 = new Item();
			itemScalar.Change(newItem1);
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 1);
			Assert.IsTrue(newItem1.ProcessedAsNew == 1);
			consumer.Dispose();
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 1);
			Assert.IsTrue(newItem1.ProcessedAsNew == 1);
			Assert.IsTrue(newItem1.ProcessedAsOld == 1);
		}

		public ScalarProcessingVoidTest(bool debug) : base(debug)
		{
		}
	}
}