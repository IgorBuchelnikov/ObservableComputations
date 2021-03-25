// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(true)]
	[TestFixture(false)]
	public partial class ScalarProcessingTest : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item
		{
			public int ProcessedAsNew;
			public int ProcessedAsOld;
			public object Token = new object();
		}

		object _token;

		Func<Item, IScalarComputing, object> _newValueProcessor;
		Action<Item, IScalarComputing, object> _oldValueProcessor;

		public ScalarProcessingTest(bool debug) : base(debug)
		{
			_newValueProcessor = (newItem, current) =>
			{
				newItem.ProcessedAsNew++;
				return newItem.Token;
			};

			_oldValueProcessor = (oldItem, current, retVal) =>
			{
				Assert.AreEqual(retVal, _token);
				oldItem.ProcessedAsOld++;
			};
		}

		[Test]
		public void ScalarProcessing_Test()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);
			ScalarProcessing<Item, object> scalarProcessing = itemScalar.ScalarProcessing<Item, object>(
				_newValueProcessor,
				_oldValueProcessor).For(consumer);

			Assert.AreEqual(scalarProcessing.Source, itemScalar);
			Assert.AreEqual(scalarProcessing.NewValueProcessor, _newValueProcessor);
			Assert.AreEqual(scalarProcessing.OldValueProcessor, _oldValueProcessor);
			Assert.IsTrue(scalarProcessing.Sources.Contains(itemScalar));

			test(item, itemScalar);

			consumer.Dispose();
		}

		[Test]
		public void ScalarProcessing_Test2()
		{
			Item item = new Item();
			Scalar<Item> itemScalar = new Scalar<Item>(item);

			Expr.Is(() => itemScalar.Value).ScalarProcessing<Item, object>(
				_newValueProcessor,
				_oldValueProcessor).For(consumer);

			test(item, itemScalar);

			consumer.Dispose();
		}

		private void test(Item item, Scalar<Item> itemScalar)
		{
			Assert.IsTrue(item.ProcessedAsNew == 1);
			_token = item.Token;
			Item newItem1 = new Item();
			itemScalar.Change(newItem1);
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 1);
			Assert.IsTrue(newItem1.ProcessedAsNew == 1);
			_token = newItem1.Token;
			consumer.Dispose();
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 1);
			Assert.IsTrue(newItem1.ProcessedAsNew == 1);
			Assert.IsTrue(newItem1.ProcessedAsOld == 1);
		}

	}
}
