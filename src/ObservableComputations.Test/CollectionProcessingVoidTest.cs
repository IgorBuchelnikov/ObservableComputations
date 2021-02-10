// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

		private CollectionProcessingVoid<Item> getCollectionProcessing(ObservableCollection<Item> items, OcConsumer consumer)
		{
			Action<Item[], CollectionProcessingVoid<Item>> newItemProcessor = (newItems, current) =>
			{
				foreach (Item newItem in newItems)
				{
					newItem.ProcessedAsNew++;
				}
			};
			Action<Item[], CollectionProcessingVoid<Item>> oldItemProcessor = (oldItems, current) =>
			{
				foreach (Item oldItem in oldItems)
				{
					oldItem.ProcessedAsOld++;
				}
			};
			Action<Item, CollectionProcessingVoid<Item>> moveItemProcessor = (item, current) =>
			{

			};

			switch (_sourceCollectionType)
			{
				case SourceCollectionType.INotifyPropertyChanged:
					return ((INotifyCollectionChanged) items).CollectionProcessing(
						newItemProcessor,
						oldItemProcessor,
						moveItemProcessor).For(consumer);
				case SourceCollectionType.ObservableCollection:
					return items.CollectionProcessing(
						newItemProcessor,
						oldItemProcessor,
						moveItemProcessor).For(consumer);
				case SourceCollectionType.ScalarINotifyPropertyChanged:
					return new Scalar<INotifyCollectionChanged>(items).CollectionProcessing(
						newItemProcessor,
						oldItemProcessor,
						moveItemProcessor).For(consumer);
				case SourceCollectionType.ScalarObservableCollection:
					return new Scalar<ObservableCollection<Item>>(items).CollectionProcessing(
						newItemProcessor,
						oldItemProcessor,
						moveItemProcessor).For(consumer);
				case SourceCollectionType.ExpressionINotifyPropertyChanged:
					return Expr.Is(() => items).CollectionProcessing(
						newItemProcessor,
						oldItemProcessor,
						moveItemProcessor).For(consumer);
				case SourceCollectionType.ExpressionObservableCollection:
					return Expr.Is(() => (INotifyCollectionChanged)items).CollectionProcessing(
						newItemProcessor,
						oldItemProcessor,
						moveItemProcessor).For(consumer);
			}

			return null;
		}


		[Test]
		public void CollectionProcessing_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer);
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer);
			items.RemoveAt(index);
			Assert.IsTrue(sourceCollection[index].ProcessedAsNew == 1);
			Assert.IsTrue(sourceCollection[index].ProcessedAsOld == 1);
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer);
			items.RemoveAt(0);
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 1);
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer);
			Item item = new Item();
			items.Insert(index, item);
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 0);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void CollectionProcessing_Insert1(
			[Range(-1, 5)] int newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer);
			Item item = new Item();
			items.Insert(0, item);
			Assert.IsTrue(item.ProcessedAsNew == 1);
			Assert.IsTrue(item.ProcessedAsOld == 0);
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer);
			items.Move(oldIndex, newIndex);
			Assert.IsTrue(sourceCollection[oldIndex].ProcessedAsNew == 1);
			Assert.IsTrue(sourceCollection[oldIndex].ProcessedAsOld == 0);
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer);
			items[index] = new Item();
			Assert.IsTrue(sourceCollection[index].ProcessedAsNew == 1);
			Assert.IsTrue(sourceCollection[index].ProcessedAsOld == 1);
			Assert.IsTrue(items[index].ProcessedAsNew == 1);
			Assert.IsTrue(items[index].ProcessedAsOld == 0);
			consumer.Dispose();
		}	

		[Test, Combinatorial]
		public void CollectionProcessing_InitDispose()
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

			CollectionProcessingVoid<Item> collectionProcessing = getCollectionProcessing(items, consumer);
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