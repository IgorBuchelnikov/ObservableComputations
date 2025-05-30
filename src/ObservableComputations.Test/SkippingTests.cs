﻿// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.ObjectModel;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class SkippingTests : TestBase
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
		public void Skipping_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Skipping<Item> itemComputing = items.Skipping(0).For(consumer);
			itemComputing.ValidateInternalConsistency();
			consumer.Dispose();
		}


		[Test, Combinatorial]
		public void Skipping_Remove(
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

			Skipping<Item> itemComputing = items.Skipping(count).For(consumer);
			itemComputing.ValidateInternalConsistency();
			items.RemoveAt(index);
			itemComputing.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Skipping_Remove1(
			[Range(0, 4, 1)] int count)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			Skipping<Item> itemComputing = items.Skipping(count).For(consumer);
			itemComputing.ValidateInternalConsistency();
			items.RemoveAt(0);
			itemComputing.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Skipping_Insert(
			[Range(0, 4, 1)] int index,
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

			Skipping<Item> itemComputing = items.Skipping( count).For(consumer);
			itemComputing.ValidateInternalConsistency();
			items.Insert(index, new Item());
			itemComputing.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Skipping_Insert1(
			[Range(0, 4, 1)] int count)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Skipping<Item> itemComputing = items.Skipping(count).For(consumer);
			itemComputing.ValidateInternalConsistency();
			items.Insert(0, new Item());
			itemComputing.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Skipping_Move(
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

			Skipping<Item> itemComputing = items.Skipping(count).For(consumer);
			itemComputing.ValidateInternalConsistency();
			items.Move(oldIndex, newIndex);
			itemComputing.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Skipping_Set(
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

			Skipping<Item> itemComputing = items.Skipping(count).For(consumer);
			itemComputing.ValidateInternalConsistency();
			items[index] = new Item();
			itemComputing.ValidateInternalConsistency();
			consumer.Dispose();
		}

		public SkippingTests(bool debug) : base(debug)
		{
		}
	}
}