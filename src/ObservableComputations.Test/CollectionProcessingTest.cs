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

		Func<Item[], ICollectionComputing, object[]> _newItemsProcessor = (newItems, current) =>
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

		Action<Item[], ICollectionComputing, object[]> _oldItemsProcessor = (oldItems, current, returnValues) =>
		{
			for (int index = 0; index < oldItems.Length; index++)
			{
				Item oldItem = oldItems[index];
				oldItem.ProcessedAsOld++;
				Assert.AreEqual(oldItem.Token, returnValues[index]);
			}
		};

		Func<Item, ICollectionComputing, object> _newItemProcessor = (newItem, current) =>
		{
			newItem.ProcessedAsNew++;
			return newItem.Token;
		};

		Action<Item, ICollectionComputing, object> _oldItemProcessor = (oldItem, current, returnValue) =>
		{
			oldItem.ProcessedAsOld++;
			Assert.AreEqual(oldItem.Token, returnValue);
		};

		Action<Item, ICollectionComputing, object> _moveItemProcessor = (item, computing, arg3) =>
		{

		};

		private void test(
			CollectionProcessing<Item, object> collectionProcessing,
			ObservableCollection<Item> items,
			Item[] sourceCollection,
			bool batch)
		{
			if (batch)
			{
				Assert.AreEqual(collectionProcessing.NewItemsProcessor, _newItemsProcessor);
				Assert.AreEqual(collectionProcessing.OldItemsProcessor, _oldItemsProcessor);
			}
			else
			{
				Assert.AreEqual(collectionProcessing.NewItemProcessor, _newItemProcessor);
				Assert.AreEqual(collectionProcessing.OldItemProcessor, _oldItemProcessor);
								
			}

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

			Item item2 = new Item();
			items[0] = item2;
			Assert.IsTrue(item1.ProcessedAsNew == 1);
			Assert.IsTrue(item1.ProcessedAsOld == 1);
			Assert.IsTrue(item2.ProcessedAsNew == 1);
			Assert.IsTrue(item2.ProcessedAsOld == 0);

			items.Move(1, 2);

			items.Clear();

			foreach (Item item in items)
			{
				Assert.IsTrue(item.ProcessedAsNew == 1);
				Assert.IsTrue(item.ProcessedAsOld == 1);
			}

			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCollectionProcessing1([Values(true, false)] bool batch)
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

			if (batch)
				collectionProcessing = items.CollectionItemsProcessing(
					_newItemsProcessor,
					_oldItemsProcessor,
					_moveItemProcessor).For(consumer);
			else
				collectionProcessing = items.CollectionItemProcessing(
					_newItemProcessor,
					_oldItemProcessor,
					_moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.Source, items);

			test(collectionProcessing, items, sourceCollection, batch);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing2([Values(true, false)] bool batch)
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

			CollectionProcessing<Item, object> collectionProcessing;

			if (batch)
				collectionProcessing = itemsScalar.CollectionItemsProcessing(
					_newItemsProcessor,
					_oldItemsProcessor,
					_moveItemProcessor).For(consumer);
			else
				collectionProcessing = itemsScalar.CollectionItemProcessing(
					_newItemProcessor,
					_oldItemProcessor,
					_moveItemProcessor).For(consumer);
			Assert.AreEqual(collectionProcessing.SourceScalar, itemsScalar);

			test(collectionProcessing ,items, sourceCollection, batch);

			itemsScalar.Touch();
		}

		[Test, Combinatorial]
		public void CollectionProcessing3([Values(true, false)] bool batch)
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

			if (batch)
				collectionProcessing = Expr.Is(() => items).CollectionItemsProcessing(
					_newItemsProcessor,
					_oldItemsProcessor,
					_moveItemProcessor).For(consumer);
			else
				collectionProcessing = Expr.Is(() => items).CollectionItemProcessing(
					_newItemProcessor,
					_oldItemProcessor,
					_moveItemProcessor).For(consumer);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew == 1));

			test(collectionProcessing ,items, sourceCollection, batch);
		}

		[Test, Combinatorial]
		public void TestCollectionProcessing4([Values(true, false)] bool batch)
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

			if (batch)
				collectionProcessing = ((INotifyCollectionChanged)items).CollectionItemsProcessing(
					_newItemsProcessor,
					_oldItemsProcessor,
					_moveItemProcessor).For(consumer);
			else
				collectionProcessing = ((INotifyCollectionChanged)items).CollectionItemProcessing(
					_newItemProcessor,
					_oldItemProcessor,
					_moveItemProcessor).For(consumer);

			test(collectionProcessing, items, sourceCollection, batch);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing5([Values(true, false)] bool batch)
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

			CollectionProcessing<Item, object> collectionProcessing;

			if (batch)
				collectionProcessing = itemsScalar.CollectionItemsProcessing(
					_newItemsProcessor,
					_oldItemsProcessor,
					_moveItemProcessor).For(consumer);
			else
				collectionProcessing = itemsScalar.CollectionItemProcessing(
					_newItemProcessor,
					_oldItemProcessor,
					_moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.SourceScalar, itemsScalar);

			test(collectionProcessing ,items, sourceCollection, batch);

			itemsScalar.Touch();
		}

		[Test, Combinatorial]
		public void CollectionProcessing6([Values(true, false)] bool batch)
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

			if (batch)
				collectionProcessing = Expr.Is(() => (INotifyCollectionChanged)items).CollectionItemsProcessing(
					_newItemsProcessor,
					_oldItemsProcessor,
					_moveItemProcessor).For(consumer);
			else
				collectionProcessing = Expr.Is(() => (INotifyCollectionChanged)items).CollectionItemProcessing(
					_newItemProcessor,
					_oldItemProcessor,
					_moveItemProcessor).For(consumer);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew == 1));

			test(collectionProcessing ,items, sourceCollection, batch);
		}

		public CollectionProcessingTest(bool debug) : base(debug)
		{

		}
	}
}