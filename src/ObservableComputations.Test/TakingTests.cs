// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.ObjectModel;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public class TakingTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item
		{
			public Item()
			{
				Num = LastNum;
				LastNum++;
			}

			public static int LastNum;
			public int Num;
		}

		[Test]
		public void Taking_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Taking<Item> itemComputing = items.Taking(0, 0).For(consumer);;
			itemComputing.ValidateConsistency();			
			consumer.Dispose();
		}


		[Test, Combinatorial]
		public void Taking_Remove(
			[Range(0, 4, 1)] int startIndex,
			[Range(0, 4, 1)] int count,
			[Range(0, 4, 1)] int index)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}
			);

			Taking<Item> itemComputing = items.Taking(startIndex, count).For(consumer);
			itemComputing.ValidateConsistency();
			items.RemoveAt(index);
			itemComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Taking_Remove1(
			[Range(0, 2, 1)] int startIndex,
			[Range(0, 4, 1)] int count)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			Taking<Item> itemComputing = items.Taking(startIndex, count).For(consumer);
			itemComputing.ValidateConsistency();
			items.RemoveAt(0);
			itemComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Taking_Insert(
			[Range(0, 4, 1)] int index,
			[Range(0, 4, 1)] int startIndex,
			[Range(0, 4, 1)] int count)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}
			);

			Taking<Item> itemComputing = items.Taking(startIndex, count).For(consumer);
			itemComputing.ValidateConsistency();
			items.Insert(index, new Item());
			itemComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Taking_Insert1(
			[Range(0, 2, 1)] int startIndex,
			[Range(0, 4, 1)] int count)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Taking<Item> itemComputing = items.Taking(startIndex, count).For(consumer);
			itemComputing.ValidateConsistency();
			items.Insert(0, new Item());
			itemComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Taking_Move(
			[Range(0, 4, 1)] int startIndex,
			[Range(0, 4, 1)] int count,
			[Range(0, 4, 1)] int oldIndex,
			[Range(0, 4, 1)] int newIndex)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}
			);

			Taking<Item> itemComputing = items.Taking(startIndex, count).For(consumer);
			itemComputing.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			itemComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Taking_Set(
			[Range(0, 4, 1)] int startIndex,
			[Range(0, 4, 1)] int count,
			[Range(0, 4, 1)] int index)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}
			);

			Taking<Item> itemComputing = items.Taking(startIndex, count).For(consumer);
			itemComputing.ValidateConsistency();
			items[index] = new Item();
			itemComputing.ValidateConsistency();			
			consumer.Dispose();
		}

		public TakingTests(bool debug) : base(debug)
		{
		}
	}
}