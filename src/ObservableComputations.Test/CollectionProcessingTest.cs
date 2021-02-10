// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	[TestFixture(true)]
	public class CollectionProcessingTest : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item
		{
			public int ProcessedAsNew;
			public int ProcessedAsOld;
			public object Token = new object();
		}

		Func<Item[], ICollectionComputing, object[]> _newItemProcessor = (newItems, current) =>
		{
			object[] tokens = new object[newItems.Length];
			for (int index = 0; index < newItems.Length; index++)
			{
				Item newItem = newItems[index];
				newItem.ProcessedAsNew++;
				tokens[index] = newItem.Token;
			}

			return tokens; 
		};

		Action<Item[], ICollectionComputing, object[]> _oldItemProcessor = (oldItems, current, returnValues) =>
		{
			for (int index = 0; index < oldItems.Length; index++)
			{
				Item oldItem = oldItems[index];
				oldItem.ProcessedAsOld++;
				Assert.AreEqual(oldItem.Token, returnValues[index]);
			}
		};

		Action<Item, ICollectionComputing, object> _moveItemProcessor = (item, computing, arg3) =>
		{

		};

		private void test(
			CollectionProcessing<Item, object> collectionProcessing,
			ObservableCollection<Item> items,
			Item[] sourceCollection)
		{
			Assert.AreEqual(collectionProcessing.NewItemsProcessor, _newItemProcessor);
			Assert.AreEqual(collectionProcessing.OldItemsProcessor, _oldItemProcessor);
			Assert.AreEqual(collectionProcessing.MoveItemProcessor, _moveItemProcessor);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew == 1));

			items.RemoveAt(0);
			Assert.IsTrue(sourceCollection[0].ProcessedAsNew == 1);
			Assert.IsTrue(sourceCollection[0].ProcessedAsOld == 1);

			Item item1 = new Item();
			items.Insert(0, item1);
			Assert.IsTrue(item1.ProcessedAsNew == 1);
			Assert.IsTrue(item1.ProcessedAsOld == 0);

			items.Move(1, 2);

			consumer.Dispose();
			foreach (Item item in items)
			{
				Assert.IsTrue(item.ProcessedAsNew == 1);
				Assert.IsTrue(item.ProcessedAsOld == 1);
			}
		}

		[Test]
		public void TestCollectionProcessing1()
		{
			Item[] sourceCollection = new[]
			{
				new Item(),
				new Item(),
				new Item(),
				new Item(),
				new Item()
			};

			ObservableCollection<Item> items = new ObservableCollection<Item>(
				sourceCollection);

			CollectionProcessing<Item, object> collectionProcessing;

			collectionProcessing = items.CollectionProcessing(
				_newItemProcessor,
				_oldItemProcessor,
				_moveItemProcessor).For(consumer);

			test(collectionProcessing, items, sourceCollection);
			consumer.Dispose();
		}

		[Test]
		public void CollectionProcessing2()
		{
			Item[] sourceCollection = new[]
			{
				new Item(),
				new Item(),
				new Item(),
				new Item(),
				new Item()
			};

			ObservableCollection<Item> items = new ObservableCollection<Item>(
				sourceCollection);

			Scalar<ObservableCollection<Item>> itemsScalar = new Scalar<ObservableCollection<Item>>(items);

			var collectionProcessing = itemsScalar.CollectionProcessing(
				_newItemProcessor,
				_oldItemProcessor,
				_moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.SourceScalar, itemsScalar);

			test(collectionProcessing ,items, sourceCollection);

			itemsScalar.Touch();
		}

		[Test]
		public void CollectionProcessing3()
		{
			Item[] sourceCollection = new[]
			{
				new Item(),
				new Item(),
				new Item(),
				new Item(),
				new Item()
			};

			ObservableCollection<Item> items = new ObservableCollection<Item>(
				sourceCollection);


			var collectionProcessing = Expr.Is(() => items).CollectionProcessing(
				_newItemProcessor,
				_oldItemProcessor,
				_moveItemProcessor).For(consumer);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew == 1));

			test(collectionProcessing ,items, sourceCollection);
		}

		[Test]
		public void TestCollectionProcessing4()
		{
			Item[] sourceCollection = new[]
			{
				new Item(),
				new Item(),
				new Item(),
				new Item(),
				new Item()
			};

			ObservableCollection<Item> items = new ObservableCollection<Item>(
				sourceCollection);

			CollectionProcessing<Item, object> collectionProcessing;

			collectionProcessing = ((INotifyCollectionChanged)items).CollectionProcessing(
				_newItemProcessor,
				_oldItemProcessor,
				_moveItemProcessor).For(consumer);

			test(collectionProcessing, items, sourceCollection);
			consumer.Dispose();
		}

		[Test]
		public void CollectionProcessing5()
		{
			Item[] sourceCollection = new[]
			{
				new Item(),
				new Item(),
				new Item(),
				new Item(),
				new Item()
			};

			ObservableCollection<Item> items = new ObservableCollection<Item>(
				sourceCollection);

			Scalar<INotifyCollectionChanged> itemsScalar = new Scalar<INotifyCollectionChanged>(items);

			var collectionProcessing = itemsScalar.CollectionProcessing(
				_newItemProcessor,
				_oldItemProcessor,
				_moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.SourceScalar, itemsScalar);

			test(collectionProcessing ,items, sourceCollection);

			itemsScalar.Touch();
		}

		[Test]
		public void CollectionProcessing6()
		{
			Item[] sourceCollection = new[]
			{
				new Item(),
				new Item(),
				new Item(),
				new Item(),
				new Item()
			};

			ObservableCollection<Item> items = new ObservableCollection<Item>(
				sourceCollection);


			var collectionProcessing = Expr.Is(() => (INotifyCollectionChanged)items).CollectionProcessing(
				_newItemProcessor,
				_oldItemProcessor,
				_moveItemProcessor).For(consumer);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew == 1));

			test(collectionProcessing ,items, sourceCollection);
		}

		public CollectionProcessingTest(bool debug) : base(debug)
		{

		}
	}
}