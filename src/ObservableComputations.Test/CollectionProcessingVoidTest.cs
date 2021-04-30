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
	[TestFixture(false, SourceCollectionType.INotifyPropertyChanged)]
	[TestFixture(true, SourceCollectionType.INotifyPropertyChanged)]
	[TestFixture(false, SourceCollectionType.ObservableCollection)]
	[TestFixture(true, SourceCollectionType.ObservableCollection)]
	[TestFixture(false, SourceCollectionType.ScalarINotifyPropertyChanged)]
	[TestFixture(false, SourceCollectionType.ScalarObservableCollection)]
	[TestFixture(false, SourceCollectionType.ExpressionINotifyPropertyChanged)]
	[TestFixture(false, SourceCollectionType.ExpressionObservableCollection)]
	public partial class CollectionProcessingVoidTest : TestBase
	{
		OcConsumer consumer = new OcConsumer();
		SourceCollectionType _sourceCollectionType;

		public class Item
		{
			public int ProcessedAsNew;
			public int ProcessedAsOld;
		}

		private CollectionProcessingVoid<Item> getCollectionProcessing(ObservableCollection<Item> items, OcConsumer consumer, bool batch)
		{
			Action<Item[], CollectionProcessingVoid<Item>> newItemsProcessor = (newItems, current) =>
			{
				foreach (Item newItem in newItems)
				{
					newItem.ProcessedAsNew++;
				}
			};

			Action<Item[], CollectionProcessingVoid<Item>> oldItemsProcessor = (oldItems, current) =>
			{
				foreach (Item oldItem in oldItems)
				{
					oldItem.ProcessedAsOld++;
				}
			};

			Action<Item, CollectionProcessingVoid<Item>> newItemProcessor = (newItem, current) =>
			{
				newItem.ProcessedAsNew++;
				
			};

			Action<Item, CollectionProcessingVoid<Item>> oldItemProcessor = (oldItem, current) =>
			{
				oldItem.ProcessedAsOld++;
			};

			Action<Item, CollectionProcessingVoid<Item>> moveItemProcessor = (item, current) =>
			{

			};

			CollectionProcessingVoid<Item> collectionProcessingVoid = null;

			switch (_sourceCollectionType)
			{
				case SourceCollectionType.INotifyPropertyChanged:
					collectionProcessingVoid = batch ? 
						((INotifyCollectionChanged) items).CollectionItemsProcessing(
							newItemsProcessor,
							oldItemsProcessor,
							moveItemProcessor).For(consumer)
						: ((INotifyCollectionChanged) items).CollectionItemProcessing(
							newItemProcessor,
							oldItemProcessor,
							moveItemProcessor).For(consumer);
					Assert.AreEqual(collectionProcessingVoid.Source, items);
					Assert.IsTrue(collectionProcessingVoid.Sources.Contains(items));
					break;
				case SourceCollectionType.ObservableCollection:
					collectionProcessingVoid = batch ? 
						collectionProcessingVoid = items.CollectionItemsProcessing(
							newItemsProcessor,
							oldItemsProcessor,
							moveItemProcessor).For(consumer)
						: collectionProcessingVoid = items.CollectionItemProcessing(
							newItemProcessor,
							oldItemProcessor,
							moveItemProcessor).For(consumer);
					Assert.AreEqual(collectionProcessingVoid.Source, items);
					Assert.IsTrue(collectionProcessingVoid.Sources.Contains(items));
					break;
				case SourceCollectionType.ScalarINotifyPropertyChanged:
					Scalar<INotifyCollectionChanged> scalar = new Scalar<INotifyCollectionChanged>(items);
					collectionProcessingVoid = batch ?
						scalar.CollectionItemsProcessing(
							newItemsProcessor,
							oldItemsProcessor,
							moveItemProcessor).For(consumer)
						: scalar.CollectionItemProcessing(
							newItemProcessor,
							oldItemProcessor,
							moveItemProcessor).For(consumer);
					Assert.AreEqual(collectionProcessingVoid.SourceScalar, scalar);
					Assert.IsTrue(collectionProcessingVoid.Sources.Contains(scalar));
					break;
				case SourceCollectionType.ScalarObservableCollection:
					collectionProcessingVoid = batch ?
						new Scalar<ObservableCollection<Item>>(items).CollectionItemsProcessing(
							newItemsProcessor,
							oldItemsProcessor,
							moveItemProcessor).For(consumer)
						: new Scalar<ObservableCollection<Item>>(items).CollectionItemProcessing(
							newItemProcessor,
							oldItemProcessor,
							moveItemProcessor).For(consumer);
					break;
				case SourceCollectionType.ExpressionINotifyPropertyChanged:
					collectionProcessingVoid = batch ?
						Expr.Is(() => items).CollectionItemsProcessing(
							newItemsProcessor,
							oldItemsProcessor,
							moveItemProcessor).For(consumer)
						: Expr.Is(() => items).CollectionItemProcessing(
							newItemProcessor,
							oldItemProcessor,
							moveItemProcessor).For(consumer);
					break;
				case SourceCollectionType.ExpressionObservableCollection:
					collectionProcessingVoid = batch ?
						Expr.Is(() => (INotifyCollectionChanged)items).CollectionItemsProcessing(
							newItemsProcessor,
							oldItemsProcessor,
							moveItemProcessor).For(consumer)
						: Expr.Is(() => (INotifyCollectionChanged)items).CollectionItemProcessing(
							newItemProcessor,
							oldItemProcessor,
							moveItemProcessor).For(consumer);
					break;
			}

			if (batch)
			{
				Assert.AreEqual(collectionProcessingVoid.NewItemsProcessor, newItemsProcessor);
				Assert.AreEqual(collectionProcessingVoid.OldItemsProcessor, oldItemsProcessor);
			}
			else
			{
				Assert.AreEqual(collectionProcessingVoid.NewItemProcessor, newItemProcessor);
				Assert.AreEqual(collectionProcessingVoid.OldItemProcessor, oldItemProcessor);
			}

			Assert.AreEqual(collectionProcessingVoid.MoveItemProcessor, moveItemProcessor);

			return collectionProcessingVoid;
		}


		[Test, Combinatorial]
		public void CollectionProcessing_Initialization_01([Values(true, false)] bool batch)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
		}


		[Test, Combinatorial]
		public void CollectionProcessing_Remove(
			[Range(0, 4, 1)] int index,
			[Values(true, false)] bool batch)
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
			items.RemoveAt(index);
			Assert.IsTrue(sourceCollection[index].ProcessedAsNew == 1);
			Assert.IsTrue(sourceCollection[index].ProcessedAsOld == 1);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Remove1([Values(true, false)] bool batch)
		{
			Item item = new Item();
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					item
				}
			);

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
			items.RemoveAt(0);
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 1);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Insert(
			[Range(0, 4, 1)] int index, [Values(true, false)] bool batch)
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
			Item item = new Item();
			items.Insert(index, item);
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 0);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Insert1(
			[Range(-1, 5)] int newValue, [Values(true, false)] bool batch)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
			Item item = new Item();
			items.Insert(0, item);
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 0);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Move(
			[Range(0, 4, 1)] int oldIndex,
			[Range(0, 4, 1)] int newIndex,
			[Values(true, false)] bool batch)
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
			items.Move(oldIndex, newIndex);
			Assert.IsTrue(sourceCollection[oldIndex].ProcessedAsNew == 1);
			Assert.IsTrue(sourceCollection[oldIndex].ProcessedAsOld == 0);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Set(
			[Range(0, 4, 1)] int index,
			[Values(true, false)] bool batch)
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
			items[index] = new Item();
			Assert.IsTrue(sourceCollection[index].ProcessedAsNew == 1);
			Assert.IsTrue(sourceCollection[index].ProcessedAsOld == 1);
			Assert.IsTrue(items[index].ProcessedAsNew == 1);
			Assert.IsTrue(items[index].ProcessedAsOld == 0);
			consumer.Dispose();
		}	

		[Test, Combinatorial]
		public void CollectionProcessing_Reset([Values(true, false)] bool batch)
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
			items.Clear();
			Assert.IsTrue(sourceCollection.All(i => i.ProcessedAsNew == 1));
			Assert.IsTrue(sourceCollection.All(i => i.ProcessedAsOld == 1));
			Assert.IsTrue(items.All(i => i.ProcessedAsNew == 1));
			Assert.IsTrue(items.All(i => i.ProcessedAsOld == 1));
			consumer.Dispose();
		}	

		[Test, Combinatorial]
		public void CollectionProcessing_InitDispose([Values(true, false)] bool batch)
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer, batch);
			foreach (Item item in sourceCollection)
			{
				Assert.IsTrue(item.ProcessedAsNew == 1);
				Assert.IsTrue(item.ProcessedAsOld == 0);
			}			
			
			consumer.Dispose();
			foreach (Item item in sourceCollection)
			{
				Assert.IsTrue(item.ProcessedAsNew == 1);
				Assert.IsTrue(item.ProcessedAsOld == 1);				
			}
		}

		public CollectionProcessingVoidTest(bool debug, SourceCollectionType sourceCollectionType) : base(debug)
		{
			_sourceCollectionType = sourceCollectionType;
		}
	}
}