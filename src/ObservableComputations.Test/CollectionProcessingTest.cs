using System.Collections.ObjectModel;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class CollectionProcessingTest
	{
		OcConsumer consumer = new OcConsumer();

		public class Item
		{
			public bool ProcessedAsNew;
			public bool ProcessedAsOld;
			public object Token = new object();
		}

		private static CollectionProcessing<Item, object> getCollectionProcessing(ObservableCollection<Item> items, OcConsumer consumer)
		{
			return items.CollectionProcessing(
				(newItems, current) =>
				{
					object[] tokens = new object[newItems.Length];
					for (int index = 0; index < newItems.Length; index++)
					{
						Item newItem = newItems[index];
						newItem.ProcessedAsNew = true;
						tokens[index] = newItem.Token;
					}

					return tokens; 
				},
				(oldItems, current, returnValues) =>
				{
					for (int index = 0; index < oldItems.Length; index++)
					{
						Item oldItem = oldItems[index];
						oldItem.ProcessedAsOld = true;
						Assert.AreEqual(oldItem.Token, returnValues[index]);
					}
				}).For(consumer);
		}

		[Test]
		public void CollectionProcessing_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			CollectionProcessing<Item, object> collectionProcessing = getCollectionProcessing(items, consumer);
		}


		[Test, Combinatorial]
		public void CollectionProcessing_Remove(
			[Range(0, 4, 1)] int index)
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

			CollectionProcessing<Item, object> collectionProcessing = getCollectionProcessing(items, consumer);
			items.RemoveAt(index);
			Assert.IsTrue(sourceCollection[index].ProcessedAsNew);
			Assert.IsTrue(sourceCollection[index].ProcessedAsOld);
			consumer.Dispose();

		}

		[Test, Combinatorial]
		public void CollectionProcessing_Remove1()
		{
			Item item = new Item();
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					item
				}
			);

			CollectionProcessing<Item, object> collectionProcessing = getCollectionProcessing(items, consumer);
			items.RemoveAt(0);
			Assert.IsTrue(item.ProcessedAsNew);
			Assert.IsTrue(item.ProcessedAsOld);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Insert(
			[Range(0, 4, 1)] int index)
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

			CollectionProcessing<Item, object> collectionProcessing = getCollectionProcessing(items, consumer);
			Item item = new Item();
			items.Insert(index, item);
			Assert.IsTrue(item.ProcessedAsNew);
			Assert.IsFalse(item.ProcessedAsOld);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Insert1(
			[Range(-1, 5)] int newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			CollectionProcessing<Item, object> collectionProcessing = getCollectionProcessing(items, consumer);
			Item item = new Item();
			items.Insert(0, item);
			Assert.IsTrue(item.ProcessedAsNew);
			Assert.IsFalse(item.ProcessedAsOld);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Move(
			[Range(0, 4, 1)] int oldIndex,
			[Range(0, 4, 1)] int newIndex)
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

			CollectionProcessing<Item, object> collectionProcessing = getCollectionProcessing(items, consumer);
			items.Move(oldIndex, newIndex);
			Assert.IsTrue(sourceCollection[oldIndex].ProcessedAsNew);
			Assert.IsFalse(sourceCollection[oldIndex].ProcessedAsOld);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Set(
			[Range(0, 4, 1)] int index)
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

			CollectionProcessing<Item, object> collectionProcessing = getCollectionProcessing(items, consumer);
			items[index] = new Item();
			Assert.IsTrue(sourceCollection[index].ProcessedAsNew);
			Assert.IsTrue(sourceCollection[index].ProcessedAsOld);
			Assert.IsTrue(items[index].ProcessedAsNew);
			Assert.IsFalse(items[index].ProcessedAsOld);
			consumer.Dispose();
		}	

		[Test, Combinatorial]
		public void CollectionProcessing_Dispose()
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

			CollectionProcessing<Item, object> collectionProcessing = getCollectionProcessing(items, consumer);
			foreach (Item item in sourceCollection)
			{
				Assert.IsTrue(item.ProcessedAsNew);
				Assert.IsFalse(item.ProcessedAsOld);	
			}			
			
			consumer.Dispose();
			foreach (Item item in sourceCollection)
			{
				Assert.IsTrue(item.ProcessedAsNew);
				Assert.IsTrue(item.ProcessedAsOld);				
			}
		}	
	}
}