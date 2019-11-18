using System.Collections.ObjectModel;
using NUnit.Framework;

namespace IBCode.ObservableComputations.Test
{
	[TestFixture]
	public class ItemComputingTests
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
		public void ItemComputing_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			ItemComputing<Item> itemComputing = items.ItemComputing(0);
			itemComputing.ValidateConsistency();
		}


		[Test, Combinatorial]
		public void ItemComputing_Remove(
			[Range(0, 4, 1)] int computingIndex,
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

			ItemComputing<Item> itemComputing = items.ItemComputing(computingIndex);
			itemComputing.ValidateConsistency();
			items.RemoveAt(index);
			itemComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemComputing_Remove1(
			[Range(0, 2, 1)] int computingIndex)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			ItemComputing<Item> itemComputing = items.ItemComputing(computingIndex);
			itemComputing.ValidateConsistency();
			items.RemoveAt(0);
			itemComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemComputing_Insert(
			[Range(0, 4, 1)] int index,
			[Range(0, 4, 1)] int computingIndex)
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

			ItemComputing<Item> itemComputing = items.ItemComputing(computingIndex);
			itemComputing.ValidateConsistency();
			items.Insert(index, new Item());
			itemComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemComputing_Insert1(
			[Range(0, 2, 1)] int computingIndex)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			ItemComputing<Item> itemComputing = items.ItemComputing(computingIndex);
			itemComputing.ValidateConsistency();
			items.Insert(0, new Item());
			itemComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemComputing_Move(
			[Range(0, 4, 1)] int computingIndex,
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

			ItemComputing<Item> itemComputing = items.ItemComputing(computingIndex);
			itemComputing.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			itemComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ItemComputing_Set(
			[Range(0, 4, 1)] int computingIndex,
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

			ItemComputing<Item> itemComputing = items.ItemComputing(computingIndex);
			itemComputing.ValidateConsistency();
			items[index] = new Item();
			itemComputing.ValidateConsistency();
		}		
	}
}