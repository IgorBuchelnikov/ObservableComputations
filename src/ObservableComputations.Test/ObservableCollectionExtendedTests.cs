using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(true)]
	[TestFixture(false)]
	public class ObservableCollectionExtendedTests : TestBase
	{
		NotifyCollectionChangedEventArgs _lastNotifyCollectionChangedEventArgs = null;
		private void test(INotifyCollectionChangedExtended notifyCollectionChangedExtended)
		{
			ObservableCollection<int> collection = (ObservableCollection<int>) notifyCollectionChangedExtended;
			PropertyInfo newItemPropertyInfo = notifyCollectionChangedExtended.GetType().GetProperty("NewItem");

			NotifyCollectionChangedAction? currentChange = null;
			int newItem = 0;
			int oldIndex = -1;
			int newIndex = -1;

			Extending<int> extending = notifyCollectionChangedExtended as Extending<int>;

			notifyCollectionChangedExtended.PreCollectionChanged += (sender, args) =>
			{
				Assert.AreEqual(notifyCollectionChangedExtended.CurrentChange, currentChange);
				Assert.AreEqual(newItemPropertyInfo.GetValue(notifyCollectionChangedExtended), newItem);
				Assert.AreEqual(notifyCollectionChangedExtended.NewItemObject, newItem);
				Assert.AreEqual(notifyCollectionChangedExtended.OldIndex, oldIndex);
				Assert.AreEqual(notifyCollectionChangedExtended.NewIndex, newIndex);
				if (extending != null)
				{
					Assert.AreEqual(extending.HandledEventSender, extending.Source);
					Assert.AreEqual(extending.HandledEventArgs, _lastNotifyCollectionChangedEventArgs);
				}
			};

			notifyCollectionChangedExtended.PostCollectionChanged += (sender, args) =>
			{
				Assert.AreEqual(notifyCollectionChangedExtended.CurrentChange, currentChange);
				Assert.AreEqual(newItemPropertyInfo.GetValue(notifyCollectionChangedExtended), newItem);
				Assert.AreEqual(notifyCollectionChangedExtended.NewItemObject, newItem);
				Assert.AreEqual(notifyCollectionChangedExtended.OldIndex, oldIndex);
				Assert.AreEqual(notifyCollectionChangedExtended.NewIndex, newIndex);
				if (extending != null)
				{
					Assert.AreEqual(extending.HandledEventSender, extending.Source);
					Assert.AreEqual(extending.HandledEventArgs, _lastNotifyCollectionChangedEventArgs);
				}
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
			Assert.AreEqual(collection.ItemType, typeof(int));

			test(collection);
		}

		[Test]
		public void Test3()
		{
			ObservableCollectionExtended<int> collection = new ObservableCollectionExtended<int>(new []{1, 2, 3}.ToList());
			Assert.AreEqual(collection.ItemType, typeof(int));

			test(collection);
		}

		[Test]
		public void Test4()
		{
			OcConsumer consumer = new OcConsumer();
			ObservableCollection<int> sourceCollection = (new ObservableCollection<int>(new []{1, 2, 3}));
			sourceCollection.CollectionChanged += (sender, args) =>
			{
				_lastNotifyCollectionChangedEventArgs  = args;
			};

			Extending<int> collection = sourceCollection.Extending().For(consumer);


			Action<int, int> collectionInsertItemRequestHandler = (index, newItem) =>
			{
				sourceCollection.Insert(index, newItem);
			};
			collection.InsertItemRequestHandler = collectionInsertItemRequestHandler;
			Assert.IsTrue(collection.InsertItemRequestHandler == collectionInsertItemRequestHandler);

			Action<int> collectionRemoveItemRequestHandler = (index) =>
			{
				sourceCollection.Remove(index);
			};
			collection.RemoveItemRequestHandler = collectionRemoveItemRequestHandler;
			Assert.IsTrue(collection.RemoveItemRequestHandler == collectionRemoveItemRequestHandler);

			Action<int, int> collectionSetItemRequestHandler = (index, newItem) =>
			{
				sourceCollection[index] = newItem;
			};
			collection.SetItemRequestHandler = collectionSetItemRequestHandler;
			Assert.IsTrue(collection.SetItemRequestHandler == collectionSetItemRequestHandler);

			Action<int, int> collectionMoveItemRequestHandler = (oldIndex, newIndex) =>
			{
				sourceCollection.Move(oldIndex, newIndex);
			};
			collection.MoveItemRequestHandler = collectionMoveItemRequestHandler;
			Assert.IsTrue(collection.MoveItemRequestHandler == collectionMoveItemRequestHandler);

			Action collectionClearItemsRequestHandler = () =>
			{
				sourceCollection.Clear();
			};
			collection.ClearItemsRequestHandler = collectionClearItemsRequestHandler;
			Assert.IsTrue(collection.ClearItemsRequestHandler == collectionClearItemsRequestHandler);

			test(collection);

			consumer.Dispose();
		}

		public ObservableCollectionExtendedTests(bool debug) : base(debug)
		{
		}
	}
}
