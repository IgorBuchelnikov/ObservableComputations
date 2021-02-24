using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(true)]
	[TestFixture(false)]
	public class ChangeRequestHandlersTests : TestBase
	{
		public class Item : INotifyPropertyChanged
		{
			public Item(int id, string value)
			{
				_id = id;
				_value = value;
				Num = LastNum;
				LastNum++;
			}

			public static int LastNum;
			public int Num;

			private int _id;

			public int Id
			{
				get { return _id; }
				set { updatePropertyValue(ref _id, value); }
			}

			private string _value;

			public string Value
			{
				get { return _value; }
				set { updatePropertyValue(ref _value, value); }
			}

			#region INotifyPropertyChanged imlementation

			public event PropertyChangedEventHandler PropertyChanged;

			protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChangedEventHandler onPropertyChanged = PropertyChanged;
				if (onPropertyChanged != null) onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}

			protected bool updatePropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
			{
				if (EqualityComparer<T>.Default.Equals(field, value)) return false;
				field = value;
				this.onPropertyChanged(propertyName);
				return true;
			}

			#endregion
		}

		[Test]
		public void TestDictionaring()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(0, "0"), 
					new Item(1, "1"),
					new Item(2, "2")
				});

			OcConsumer consumer = new OcConsumer();
			Dictionaring<Item, int, string> dictionaring = 
				items.Dictionaring(i => i.Id, i => i.Value).For(consumer);

			Action<int, string> dictionaringAddItemRequestHandler = (id, value) => items.Add(new Item(id, value));
			dictionaring.AddItemRequestHandler = dictionaringAddItemRequestHandler;
			Assert.IsTrue(dictionaring.AddItemRequestHandler == dictionaringAddItemRequestHandler);

			Action<int, string> dictionaringSetItemRequestHandler = (id, value) => items.SingleOrDefault(i => i.Id == id).Value = value;
			dictionaring.SetItemRequestHandler = dictionaringSetItemRequestHandler;
			Assert.IsTrue(dictionaring.SetItemRequestHandler == dictionaringSetItemRequestHandler);

			Func<int, bool> dictionaringRemoveItemRequestHandler = id => items.Remove(items.SingleOrDefault(i => i.Id == id));
			dictionaring.RemoveItemRequestHandler = dictionaringRemoveItemRequestHandler;
			Assert.IsTrue(dictionaring.RemoveItemRequestHandler == dictionaringRemoveItemRequestHandler);

			Action dictionaringClearItemsRequestHandler = () => items.Clear();
			dictionaring.ClearItemsRequestHandler = dictionaringClearItemsRequestHandler;
			Assert.IsTrue(dictionaring.ClearItemsRequestHandler == dictionaringClearItemsRequestHandler);

			dictionaring.Add(5, "2");
			dictionaring.ValidateConsistency();
			Assert.IsTrue(dictionaring.Remove(5));
			dictionaring.ValidateConsistency();
			Assert.IsFalse(dictionaring.Remove(7));
			dictionaring.ValidateConsistency();
			dictionaring[0] = "1";
			dictionaring.ValidateConsistency();
			dictionaring.Clear();
			dictionaring.ValidateConsistency();
		}

		[Test]
		public void TestConcurrentDictionaring()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(0, "0"), 
					new Item(1, "1"),
					new Item(2, "2")
				});

			OcConsumer consumer = new OcConsumer();
			ConcurrentDictionaring<Item, int, string> dictionaring = 
				items.ConcurrentDictionaring(i => i.Id, i => i.Value).For(consumer);

			Action<int, string> dictionaringAddItemRequestHandler = (id, value) => items.Add(new Item(id, value));
			dictionaring.AddItemRequestHandler = dictionaringAddItemRequestHandler;
			Assert.IsTrue(dictionaring.AddItemRequestHandler == dictionaringAddItemRequestHandler);

			Action<int, string> dictionaringSetItemRequestHandler = (id, value) => items.SingleOrDefault(i => i.Id == id).Value = value;
			dictionaring.SetItemRequestHandler = dictionaringSetItemRequestHandler;
			Assert.IsTrue(dictionaring.SetItemRequestHandler == dictionaringSetItemRequestHandler);

			Func<int, bool> dictionaringRemoveItemRequestHandler = id => items.Remove(items.SingleOrDefault(i => i.Id == id));
			dictionaring.RemoveItemRequestHandler = dictionaringRemoveItemRequestHandler;
			Assert.IsTrue(dictionaring.RemoveItemRequestHandler == dictionaringRemoveItemRequestHandler);

			Action dictionaringClearItemsRequestHandler = () => items.Clear();
			dictionaring.ClearItemsRequestHandler = dictionaringClearItemsRequestHandler;
			Assert.IsTrue(dictionaring.ClearItemsRequestHandler == dictionaringClearItemsRequestHandler);

			dictionaring.Add(5, "2");
			dictionaring.ValidateConsistency();
			Assert.IsTrue(dictionaring.Remove(5));
			dictionaring.ValidateConsistency();
			Assert.IsFalse(dictionaring.Remove(7));
			dictionaring.ValidateConsistency();
			dictionaring[0] = "1";
			dictionaring.ValidateConsistency();
			dictionaring.Clear();
			dictionaring.ValidateConsistency();
		}

		[Test]
		public void TestHashSetting()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(0, "0"), 
					new Item(1, "1"),
					new Item(2, "2")
				});

			OcConsumer consumer = new OcConsumer();
			HashSetting<Item, int> dictionaring = 
				items.HashSetting(i => i.Id).For(consumer);

			Action<int> dictionaringAddItemRequestHandler = (id) => items.Add(new Item(id, "value"));
			dictionaring.AddItemRequestHandler = dictionaringAddItemRequestHandler;
			Assert.IsTrue(dictionaring.AddItemRequestHandler == dictionaringAddItemRequestHandler);

			Func<int, bool> dictionaringRemoveItemRequestHandler = id => items.Remove(items.SingleOrDefault(i => i.Id == id));
			dictionaring.RemoveItemRequestHandler = dictionaringRemoveItemRequestHandler;
			Assert.IsTrue(dictionaring.RemoveItemRequestHandler == dictionaringRemoveItemRequestHandler);

			Action dictionaringClearItemsRequestHandler = () => items.Clear();
			dictionaring.ClearItemsRequestHandler = dictionaringClearItemsRequestHandler;
			Assert.IsTrue(dictionaring.ClearItemsRequestHandler == dictionaringClearItemsRequestHandler);

			dictionaring.Add(5);
			dictionaring.ValidateConsistency();
			Assert.IsTrue(dictionaring.Remove(5));
			dictionaring.ValidateConsistency();
			Assert.IsFalse(dictionaring.Remove(7));
			dictionaring.ValidateConsistency();
			dictionaring.Clear();
			dictionaring.ValidateConsistency();
		}

		[Test]
		public void TestGrouping()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(0, "0"), 
					new Item(1, "0"),
					new Item(2, "2"),
					new Item(3, "3"),
					new Item(4, "3"),
					new Item(5, "3")
				});

			OcConsumer consumer = new OcConsumer();
			Grouping<Item, string> grouping = 
				items.Grouping(i => i.Value).For(consumer);

			Action<Group<Item, string>, int, Item> groupingInsertItemIntoGroupRequestHandler = (group, index, item) => 
				items.Insert(index < @group.Count ? @group.SourceItemIndices[index] : @group.SourceItemIndices[@group.Count - 1] + 1, item);
			grouping.InsertItemIntoGroupRequestHandler = groupingInsertItemIntoGroupRequestHandler;
			Assert.IsTrue(grouping.InsertItemIntoGroupRequestHandler == groupingInsertItemIntoGroupRequestHandler);

			Action<Group<Item, string>, int> groupingRemoveItemFromGroupRequestHandler = (group, index) => 
				items.RemoveAt(@group.SourceItemIndices[index]);
			grouping.RemoveItemFromGroupRequestHandler = groupingRemoveItemFromGroupRequestHandler;
			Assert.IsTrue(grouping.RemoveItemFromGroupRequestHandler == groupingRemoveItemFromGroupRequestHandler);

			Action<Group<Item, string>, int, int> groupingMoveItemInGroupRequestHandler = (group, oldIndex, newIndex) =>
			{
				Item item = @group[oldIndex];
				items.RemoveAt(@group.SourceItemIndices[oldIndex]);
				items.Insert(@group.SourceItemIndices[newIndex], item);
			};
			grouping.MoveItemInGroupRequestHandler = groupingMoveItemInGroupRequestHandler;
			Assert.IsTrue(grouping.MoveItemInGroupRequestHandler == groupingMoveItemInGroupRequestHandler);

			Action<Group<Item, string>, int, Item> groupingSetGroupItemRequestHandler = (group, index, item) => 
				items[@group.SourceItemIndices[index]] = item;
			grouping.SetGroupItemRequestHandler = groupingSetGroupItemRequestHandler;
			Assert.IsTrue(grouping.SetGroupItemRequestHandler == groupingSetGroupItemRequestHandler);

			Action<Group<Item, string>> groupingClearGroupItemsRequestHandler = group =>
			{
				ReadOnlyCollection<int> groupSourceItemIndecies = @group.SourceItemIndices;
				for (int i = @group.Count - 1; i >= 0; i--)
				{
					items.RemoveAt(groupSourceItemIndecies[i]);
				}
			};
			grouping.ClearGroupItemsRequestHandler = groupingClearGroupItemsRequestHandler;
			Assert.IsTrue(grouping.ClearGroupItemsRequestHandler == groupingClearGroupItemsRequestHandler);

			grouping[2].Add(new Item(6, "3"));
			grouping.ValidateConsistency();
			Assert.IsTrue(grouping[2].Remove(grouping[2][1]));
			grouping.ValidateConsistency();
			Assert.IsFalse(grouping[2].Remove(new Item(0, "0")));
			grouping.ValidateConsistency();
			grouping[2][0] = new Item(7, "3");
			grouping.ValidateConsistency();
			grouping[2].Clear();
			grouping.ValidateConsistency();
		}

		[Test]
		public void TestGroupJoining()
		{
			ObservableCollection<Item> innerItems = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(10, "11"), 
					new Item(10, "12"),
					new Item(20, "21"),
					new Item(30, "31"),
					new Item(30, "32"),
					new Item(30, "33")
				});

			ObservableCollection<Item> outerItems = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(10, "10"),
					new Item(20, "20"),
					new Item(30, "30")
				});

			OcConsumer consumer = new OcConsumer();
			GroupJoining<Item, Item, int> groupJoining = 
				outerItems.GroupJoining(innerItems, i => i.Id, i => i.Id).For(consumer);

			Action<JoinGroup<Item, Item, int>, int, Item> groupJoiningInsertItemIntoGroupRequestHandler = (group, index, item) =>
				innerItems.Insert(index < @group.Count ? @group.InnerSourceItemIndices[index] : @group.InnerSourceItemIndices[@group.Count - 1] + 1, item);
			groupJoining.InsertItemIntoGroupRequestHandler = groupJoiningInsertItemIntoGroupRequestHandler;
			Assert.IsTrue(groupJoining.InsertItemIntoGroupRequestHandler == groupJoiningInsertItemIntoGroupRequestHandler);

			Action<JoinGroup<Item, Item, int>, int> groupJoiningRemoveItemFromGroupRequestHandler = (group, index) =>
				innerItems.RemoveAt(@group.InnerSourceItemIndices[index]);
			groupJoining.RemoveItemFromGroupRequestHandler = groupJoiningRemoveItemFromGroupRequestHandler;
			Assert.IsTrue(groupJoining.RemoveItemFromGroupRequestHandler == groupJoiningRemoveItemFromGroupRequestHandler);

			Action<JoinGroup<Item, Item, int>, int, int> groupJoiningMoveItemInGroupRequestHandler = (group, oldIndex, newIndex) => {
				Item item = @group[oldIndex];
				innerItems.RemoveAt(@group.InnerSourceItemIndices[oldIndex]);
				innerItems.Insert(@group.InnerSourceItemIndices[newIndex], item);
			};
			groupJoining.MoveItemInGroupRequestHandler = groupJoiningMoveItemInGroupRequestHandler;
			Assert.IsTrue(groupJoining.MoveItemInGroupRequestHandler == groupJoiningMoveItemInGroupRequestHandler);

			Action<JoinGroup<Item, Item, int>, int, Item> groupJoiningSetGroupItemRequestHandler = (group, index, item) =>
				innerItems[@group.InnerSourceItemIndices[index]] = item;
			groupJoining.SetGroupItemRequestHandler = groupJoiningSetGroupItemRequestHandler;
			Assert.IsTrue(groupJoining.SetGroupItemRequestHandler == groupJoiningSetGroupItemRequestHandler);

			Action<JoinGroup<Item, Item, int>> groupJoiningClearGroupItemsRequestHandler = group => {
				ReadOnlyCollection<int> groupSourceItemIndecies = @group.InnerSourceItemIndices;
				for (int i = @group.Count - 1; i >= 0; i--)
				{
					innerItems.RemoveAt(groupSourceItemIndecies[i]);
				}
			};
			groupJoining.ClearGroupItemsRequestHandler = groupJoiningClearGroupItemsRequestHandler;
			Assert.IsTrue(groupJoining.ClearGroupItemsRequestHandler == groupJoiningClearGroupItemsRequestHandler);

			groupJoining[2].Add(new Item(6, "3"));
			groupJoining.ValidateConsistency();
			Assert.IsTrue(groupJoining[2].Remove(groupJoining[2][1]));
			groupJoining.ValidateConsistency();
			Assert.IsFalse(groupJoining[2].Remove(new Item(0, "0")));
			groupJoining.ValidateConsistency();
			groupJoining[2][0] = new Item(7, "3");
			groupJoining.ValidateConsistency();
			groupJoining[2].Clear();
			groupJoining.ValidateConsistency();
		}

		public ChangeRequestHandlersTests(bool debug) : base(debug)
		{
		}
	}
}
