using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(true)]
	[TestFixture(false)]
	public class ObservableCollectionExtendedTests : TestBase
	{
		private void test(INotifyCollectionChangedExtended notifyCollectionChangedExtended)
		{
			ObservableCollection<int> collection = (ObservableCollection<int>) notifyCollectionChangedExtended;
			PropertyInfo newItemPropertyInfo = notifyCollectionChangedExtended.GetType().GetProperty("NewItem");

			NotifyCollectionChangedAction? currentChange = null;
			int newItem = 0;
			int oldIndex = -1;
			int newIndex = -1;

			notifyCollectionChangedExtended.PreCollectionChanged += (sender, args) =>
			{
				Assert.AreEqual(notifyCollectionChangedExtended.CurrentChange, currentChange);
				Assert.AreEqual(newItemPropertyInfo.GetValue(notifyCollectionChangedExtended), newItem);
				Assert.AreEqual(notifyCollectionChangedExtended.NewItemObject, newItem);
				Assert.AreEqual(notifyCollectionChangedExtended.OldIndex, oldIndex);
				Assert.AreEqual(notifyCollectionChangedExtended.NewIndex, newIndex);
			};

			notifyCollectionChangedExtended.PostCollectionChanged += (sender, args) =>
			{
				Assert.AreEqual(notifyCollectionChangedExtended.CurrentChange, currentChange);
				Assert.AreEqual(newItemPropertyInfo.GetValue(notifyCollectionChangedExtended), newItem);
				Assert.AreEqual(notifyCollectionChangedExtended.NewItemObject, newItem);
				Assert.AreEqual(notifyCollectionChangedExtended.OldIndex, oldIndex);
				Assert.AreEqual(notifyCollectionChangedExtended.NewIndex, newIndex);
			};

			currentChange = NotifyCollectionChangedAction.Add;
			newItem = 7;
			oldIndex = -1;
			newIndex = 0;
			collection.Insert(0, 7);

			currentChange = NotifyCollectionChangedAction.Remove;
			newItem = default;
			oldIndex = 0;
			newIndex = -1;
			collection.RemoveAt(0);

			currentChange = NotifyCollectionChangedAction.Replace;
			newItem = 10;
			oldIndex = 0;
			newIndex = 0;
			collection[0] = 10;

			currentChange = NotifyCollectionChangedAction.Move;
			newItem = default;
			oldIndex = 0;
			newIndex = 1;
			collection.Move(0, 1);

			currentChange = NotifyCollectionChangedAction.Reset;
			newItem = default;
			oldIndex = -1;
			newIndex = -1;
			collection.Clear();
		}

		[Test]
		public void Test1()
		{
			ObservableCollectionExtended<int> collection = new ObservableCollectionExtended<int>();
			collection.Add(1);
			collection.Add(2);
			collection.Add(3);

			test(collection);
		}

		[Test]
		public void Test2()
		{
			ObservableCollectionExtended<int> collection = new ObservableCollectionExtended<int>(new []{1, 2, 3});

			test(collection);
		}

		[Test]
		public void Test3()
		{
			ObservableCollectionExtended<int> collection = new ObservableCollectionExtended<int>(new []{1, 2, 3}.ToList());

			test(collection);
		}

		[Test]
		public void Test4()
		{
			OcConsumer consumer = new OcConsumer();
			ObservableCollection<int> sourceCollection = (new ObservableCollection<int>(new []{1, 2, 3}));
			Extending<int> collection = sourceCollection.Extending().For(consumer);

			collection.InsertItemRequestHandler = (index, newItem) =>
			{
				sourceCollection.Insert(index, newItem);
			};

			collection.RemoveItemRequestHandler = (index) =>
			{
				sourceCollection.Remove(index);
			};

			collection.SetItemRequestHandler = (index, newItem) =>
			{
				sourceCollection[index] = newItem;
			};

			collection.MoveItemRequestHandler = (oldIndex, newIndex) =>
			{
				sourceCollection.Move(oldIndex, newIndex);
			};

			collection.ClearItemsRequestHandler = () =>
			{
				sourceCollection.Clear();
			};

			test(collection);

			consumer.Dispose();
		}

		public ObservableCollectionExtendedTests(bool debug) : base(debug)
		{
		}
	}
}
