using System.Collections.ObjectModel;
using NUnit.Framework;

namespace IBCode.ObservableCalculations.Test
{
	[TestFixture]
	public class ItemCalculatingTests
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
		public void ItemCalculating_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			ItemCalculating<Item> itemCalculating = items.ItemCalculating(0);
			itemCalculating.ValidateConsistency();
		}


		[Test, Combinatorial]
		public void ItemCalculating_Remove(
			[Range(0, 4, 1)] int calculatingIndex,
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

			ItemCalculating<Item> itemCalculating = items.ItemCalculating(calculatingIndex);
			itemCalculating.ValidateConsistency();
			items.RemoveAt(index);
			itemCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemCalculating_Remove1(
			[Range(0, 2, 1)] int calculatingIndex)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			ItemCalculating<Item> itemCalculating = items.ItemCalculating(calculatingIndex);
			itemCalculating.ValidateConsistency();
			items.RemoveAt(0);
			itemCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemCalculating_Insert(
			[Range(0, 4, 1)] int index,
			[Range(0, 4, 1)] int calculatingIndex)
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

			ItemCalculating<Item> itemCalculating = items.ItemCalculating(calculatingIndex);
			itemCalculating.ValidateConsistency();
			items.Insert(index, new Item());
			itemCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemCalculating_Insert1(
			[Range(0, 2, 1)] int calculatingIndex)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			ItemCalculating<Item> itemCalculating = items.ItemCalculating(calculatingIndex);
			itemCalculating.ValidateConsistency();
			items.Insert(0, new Item());
			itemCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemCalculating_Move(
			[Range(0, 4, 1)] int calculatingIndex,
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

			ItemCalculating<Item> itemCalculating = items.ItemCalculating(calculatingIndex);
			itemCalculating.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			itemCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemCalculating_Set(
			[Range(0, 4, 1)] int calculatingIndex,
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

			ItemCalculating<Item> itemCalculating = items.ItemCalculating(calculatingIndex);
			itemCalculating.ValidateConsistency();
			items[index] = new Item();
			itemCalculating.ValidateConsistency();
		}		
	}
}