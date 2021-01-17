// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.ObjectModel;
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
			public bool ProcessedAsNew;
			public bool ProcessedAsOld;
			public object Token = new object();
		}

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

			Func<Item[], ICollectionComputing, object[]> newItemProcessor = (newItems, current) =>
			{
				object[] tokens = new object[newItems.Length];
				for (int index = 0; index < newItems.Length; index++)
				{
					Item newItem = newItems[index];
					newItem.ProcessedAsNew = true;
					tokens[index] = newItem.Token;
				}

				return tokens; 
			};

			Action<Item[], ICollectionComputing, object[]> oldItemProcessor = (oldItems, current, returnValues) =>
			{
				for (int index = 0; index < oldItems.Length; index++)
				{
					Item oldItem = oldItems[index];
					oldItem.ProcessedAsOld = true;
					Assert.AreEqual(oldItem.Token, returnValues[index]);
				}
			};

			Action<Item, ICollectionComputing, object> moveItemProcessor = (item, computing, arg3) =>
			{

			};

			var collectionProcessing = items.CollectionProcessing(
				newItemProcessor,
				oldItemProcessor,
				moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.Source, items);
			Assert.AreEqual(collectionProcessing.NewItemsProcessor, newItemProcessor);
			Assert.AreEqual(collectionProcessing.OldItemsProcessor, oldItemProcessor);
			Assert.AreEqual(collectionProcessing.MoveItemProcessor, moveItemProcessor);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew));

			items.RemoveAt(0);
			Assert.IsTrue(sourceCollection[0].ProcessedAsNew);
			Assert.IsTrue(sourceCollection[0].ProcessedAsOld);

			Item item1 = new Item();
			items.Insert(0, item1);
			Assert.IsTrue(item1.ProcessedAsNew);
			Assert.IsFalse(item1.ProcessedAsOld);

			items.Move(1, 2);

			consumer.Dispose();
			foreach (Item item in items)
			{
				Assert.IsTrue(item.ProcessedAsNew);
				Assert.IsTrue(item.ProcessedAsOld);				
			}
		}

		public void CollectionProcessing2()
			//IReadScalar<INotifyCollectionChanged> sourceScalar,
			//Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemProcessor = null,
			//Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemProcessor = null,
			//Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
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

			Func<Item[], ICollectionComputing, object[]> newItemProcessor = (newItems, current) =>
			{
				object[] tokens = new object[newItems.Length];
				for (int index = 0; index < newItems.Length; index++)
				{
					Item newItem = newItems[index];
					newItem.ProcessedAsNew = true;
					tokens[index] = newItem.Token;
				}

				return tokens; 
			};

			Action<Item[], ICollectionComputing, object[]> oldItemProcessor = (oldItems, current, returnValues) =>
			{
				for (int index = 0; index < oldItems.Length; index++)
				{
					Item oldItem = oldItems[index];
					oldItem.ProcessedAsOld = true;
					Assert.AreEqual(oldItem.Token, returnValues[index]);
				}
			};

			Action<Item, ICollectionComputing, object> moveItemProcessor = (item, computing, arg3) =>
			{

			};

			var collectionProcessing = itemsScalar.CollectionProcessing(
				newItemProcessor,
				oldItemProcessor,
				moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.SourceScalar, itemsScalar);
			Assert.AreEqual(collectionProcessing.NewItemsProcessor, newItemProcessor);
			Assert.AreEqual(collectionProcessing.OldItemsProcessor, oldItemProcessor);
			Assert.AreEqual(collectionProcessing.MoveItemProcessor, moveItemProcessor);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew));

			items.RemoveAt(0);
			Assert.IsTrue(sourceCollection[0].ProcessedAsNew);
			Assert.IsTrue(sourceCollection[0].ProcessedAsOld);

			Item item1 = new Item();
			items.Insert(0, item1);
			Assert.IsTrue(item1.ProcessedAsNew);
			Assert.IsFalse(item1.ProcessedAsOld);

			items.Move(1, 2);

			consumer.Dispose();
			foreach (Item item in items)
			{
				Assert.IsTrue(item.ProcessedAsNew);
				Assert.IsTrue(item.ProcessedAsOld);				
			}

			itemsScalar.Touch();
		}

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


			Func<Item[], ICollectionComputing, object[]> newItemProcessor = (newItems, current) =>
			{
				object[] tokens = new object[newItems.Length];
				for (int index = 0; index < newItems.Length; index++)
				{
					Item newItem = newItems[index];
					newItem.ProcessedAsNew = true;
					tokens[index] = newItem.Token;
				}

				return tokens; 
			};

			Action<Item[], ICollectionComputing, object[]> oldItemProcessor = (oldItems, current, returnValues) =>
			{
				for (int index = 0; index < oldItems.Length; index++)
				{
					Item oldItem = oldItems[index];
					oldItem.ProcessedAsOld = true;
					Assert.AreEqual(oldItem.Token, returnValues[index]);
				}
			};

			Action<Item, ICollectionComputing, object> moveItemProcessor = (item, computing, arg3) =>
			{

			};

			var collectionProcessing = Expr.Is(() => items).CollectionProcessing(
				newItemProcessor,
				oldItemProcessor,
				moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.NewItemsProcessor, newItemProcessor);
			Assert.AreEqual(collectionProcessing.OldItemsProcessor, oldItemProcessor);
			Assert.AreEqual(collectionProcessing.MoveItemProcessor, moveItemProcessor);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew));

			items.RemoveAt(0);
			Assert.IsTrue(sourceCollection[0].ProcessedAsNew);
			Assert.IsTrue(sourceCollection[0].ProcessedAsOld);

			Item item1 = new Item();
			items.Insert(0, item1);
			Assert.IsTrue(item1.ProcessedAsNew);
			Assert.IsFalse(item1.ProcessedAsOld);

			items.Move(1, 2);

			consumer.Dispose();
			foreach (Item item in items)
			{
				Assert.IsTrue(item.ProcessedAsNew);
				Assert.IsTrue(item.ProcessedAsOld);				
			}
		}




		public void TestCollectionProcessingVoid1()
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

			Action<Item[], CollectionProcessingVoid<Item>> newItemProcessor = (newItems, current) =>
			{
				for (int index = 0; index < newItems.Length; index++)
				{
					Item newItem = newItems[index];
					newItem.ProcessedAsNew = true;
				} ; 
			};

			Action<Item[], CollectionProcessingVoid<Item>> oldItemProcessor = (oldItems, current) =>
			{
				for (int index = 0; index < oldItems.Length; index++)
				{
					Item oldItem = oldItems[index];
					oldItem.ProcessedAsOld = true;
				}
			};

			Action<Item, CollectionProcessingVoid<Item>> moveItemProcessor = (item, computing) =>
			{

			};

			var collectionProcessing = items.CollectionProcessing(
				newItemProcessor,
				oldItemProcessor,
				moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.Source, items);
			Assert.AreEqual(collectionProcessing.NewItemsProcessor, newItemProcessor);
			Assert.AreEqual(collectionProcessing.OldItemsProcessor, oldItemProcessor);
			Assert.AreEqual(collectionProcessing.MoveItemProcessor, moveItemProcessor);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew));

			items.RemoveAt(0);
			Assert.IsTrue(sourceCollection[0].ProcessedAsNew);
			Assert.IsTrue(sourceCollection[0].ProcessedAsOld);

			Item item1 = new Item();
			items.Insert(0, item1);
			Assert.IsTrue(item1.ProcessedAsNew);
			Assert.IsFalse(item1.ProcessedAsOld);

			items.Move(1, 2);

			consumer.Dispose();
			foreach (Item item in items)
			{
				Assert.IsTrue(item.ProcessedAsNew);
				Assert.IsTrue(item.ProcessedAsOld);				
			}
		}

		public void CollectionProcessingVoid2()
			//IReadScalar<INotifyCollectionChanged> sourceScalar,
			//Func<TSourceItem[], ICollectionComputing, TReturnValue[]> newItemProcessor = null,
			//Action<TSourceItem[], ICollectionComputing, TReturnValue[]> oldItemProcessor = null,
			//Action<TSourceItem, ICollectionComputing, TReturnValue> moveItemProcessor = null)
			
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

			Action<Item[], CollectionProcessingVoid<Item>> newItemProcessor = (newItems, current) =>
			{
				for (int index = 0; index < newItems.Length; index++)
				{
					Item newItem = newItems[index];
					newItem.ProcessedAsNew = true;
				} ; 
			};

			Action<Item[], CollectionProcessingVoid<Item>> oldItemProcessor = (oldItems, current) =>
			{
				for (int index = 0; index < oldItems.Length; index++)
				{
					Item oldItem = oldItems[index];
					oldItem.ProcessedAsOld = true;
				}
			};

			Action<Item, CollectionProcessingVoid<Item>> moveItemProcessor = (item, computing) =>
			{

			};

			var collectionProcessing = itemsScalar.CollectionProcessing(
				newItemProcessor,
				oldItemProcessor,
				moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.SourceScalar, itemsScalar);
			Assert.AreEqual(collectionProcessing.NewItemsProcessor, newItemProcessor);
			Assert.AreEqual(collectionProcessing.OldItemsProcessor, oldItemProcessor);
			Assert.AreEqual(collectionProcessing.MoveItemProcessor, moveItemProcessor);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew));

			items.RemoveAt(0);
			Assert.IsTrue(sourceCollection[0].ProcessedAsNew);
			Assert.IsTrue(sourceCollection[0].ProcessedAsOld);

			Item item1 = new Item();
			items.Insert(0, item1);
			Assert.IsTrue(item1.ProcessedAsNew);
			Assert.IsFalse(item1.ProcessedAsOld);

			items.Move(1, 2);

			consumer.Dispose();
			foreach (Item item in items)
			{
				Assert.IsTrue(item.ProcessedAsNew);
				Assert.IsTrue(item.ProcessedAsOld);				
			}

			itemsScalar.Touch();
		}

		public void CollectionProcessingVoid3()
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


			Action<Item[], CollectionProcessingVoid<Item>> newItemProcessor = (newItems, current) =>
			{
				for (int index = 0; index < newItems.Length; index++)
				{
					Item newItem = newItems[index];
					newItem.ProcessedAsNew = true;
				} ; 
			};

			Action<Item[], CollectionProcessingVoid<Item>> oldItemProcessor = (oldItems, current) =>
			{
				for (int index = 0; index < oldItems.Length; index++)
				{
					Item oldItem = oldItems[index];
					oldItem.ProcessedAsOld = true;
				}
			};

			Action<Item, CollectionProcessingVoid<Item>> moveItemProcessor = (item, computing) =>
			{

			};

			var collectionProcessing = Expr.Is(() => items).CollectionProcessing(
				newItemProcessor,
				oldItemProcessor,
				moveItemProcessor).For(consumer);

			Assert.AreEqual(collectionProcessing.NewItemsProcessor, newItemProcessor);
			Assert.AreEqual(collectionProcessing.OldItemsProcessor, oldItemProcessor);
			Assert.AreEqual(collectionProcessing.MoveItemProcessor, moveItemProcessor);

			Assert.IsTrue(items.All(item => item.ProcessedAsNew));

			items.RemoveAt(0);
			Assert.IsTrue(sourceCollection[0].ProcessedAsNew);
			Assert.IsTrue(sourceCollection[0].ProcessedAsOld);

			Item item1 = new Item();
			items.Insert(0, item1);
			Assert.IsTrue(item1.ProcessedAsNew);
			Assert.IsFalse(item1.ProcessedAsOld);

			items.Move(1, 2);

			consumer.Dispose();
			foreach (Item item in items)
			{
				Assert.IsTrue(item.ProcessedAsNew);
				Assert.IsTrue(item.ProcessedAsOld);				
			}
		}



		public CollectionProcessingTest(bool debug) : base(debug)
		{
		}
	}
}