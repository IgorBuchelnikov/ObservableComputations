using System.Collections.ObjectModel;
using NUnit.Framework;

namespace IBCode.ObservableCalculations.Test
{
	[TestFixture]
	public class SkippingTests
	{
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

			Skipping<Item> itemCalculating = items.Skipping(0);
			itemCalculating.ValidateConsistency();
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

			Skipping<Item> itemCalculating = items.Skipping(count);
			itemCalculating.ValidateConsistency();
			items.RemoveAt(index);
			itemCalculating.ValidateConsistency();
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

			Skipping<Item> itemCalculating = items.Skipping(count);
			itemCalculating.ValidateConsistency();
			items.RemoveAt(0);
			itemCalculating.ValidateConsistency();
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

			Skipping<Item> itemCalculating = items.Skipping( count);
			itemCalculating.ValidateConsistency();
			items.Insert(index, new Item());
			itemCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Skipping_Insert1(
			[Range(0, 4, 1)] int count)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Skipping<Item> itemCalculating = items.Skipping(count);
			itemCalculating.ValidateConsistency();
			items.Insert(0, new Item());
			itemCalculating.ValidateConsistency();
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

			Skipping<Item> itemCalculating = items.Skipping(count);
			itemCalculating.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			itemCalculating.ValidateConsistency();
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

			Skipping<Item> itemCalculating = items.Skipping(count);
			itemCalculating.ValidateConsistency();
			items[index] = new Item();
			itemCalculating.ValidateConsistency();
		}		
	}
}