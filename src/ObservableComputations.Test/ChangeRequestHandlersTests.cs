﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
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

			public Computing<int> Computing;

			public static int LastNum;
			public int Num;

			private int _id;

			public int Id
			{
				get
				{
					if (Computing != null && OcConfiguration.TrackComputingsExecutingUserCode)
						Assert.AreEqual(StaticInfo.ComputingsExecutingUserCode[Thread.CurrentThread.ManagedThreadId], Computing);
					return _id;
				}
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

			public PropertyChangedEventArgs _lastPropertyChangedEventArgs;

			protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChangedEventHandler onPropertyChanged = PropertyChanged;
				if (onPropertyChanged != null)
				{
					_lastPropertyChangedEventArgs = new PropertyChangedEventArgs(propertyName);
					onPropertyChanged(this, _lastPropertyChangedEventArgs);
				}
			}

			protected bool updatePropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
			{
				if (EqualityComparer<T>.Default.Equals(field, value)) return false;
				field = value;
				this.onPropertyChanged(propertyName);
				return true;
			}

			#endregion

			#region Overrides of Object

			public override string ToString()
			{
				return _id.ToString();
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

			NotifyCollectionChangedEventArgs lastNotifyCollectionChangedEventArgs = null;
			items.CollectionChanged += (sender, args) =>
			{
				lastNotifyCollectionChangedEventArgs  = args;
			};

			OcConsumer consumer = new OcConsumer();
			Dictionaring<Item, int, string> dictionaring = 
				items.Dictionaring(i => new Computing<int>(() => i.Id).Value, i => new Computing<string>(() => i.Value).Value);

			bool activationInProgress = true;
			bool inActivationInProgress = false;

			dictionaring.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "ActivationInProgress" || args.PropertyName == "InactivationInProgress") return;
				Assert.AreEqual(dictionaring.ActivationInProgress, activationInProgress);
				Assert.AreEqual(dictionaring.InactivationInProgress, inActivationInProgress);
			};

			dictionaring.For(consumer);
			activationInProgress = false;

			if (OcConfiguration.SaveInstantiationStackTrace)
				Assert.IsNotNull(dictionaring.InstantiationStackTrace);

			Assert.IsNotNull(dictionaring.ToString());

			Assert.IsNull(dictionaring.UserCodeIsCalledFrom);

			Assert.IsTrue(((IComputingInternal) dictionaring).Consumers.Contains(consumer));


			dictionaring.DebugTag = "DebugTag";
			Assert.AreEqual(dictionaring.DebugTag,  "DebugTag");

			dictionaring.Tag = "Tag";
			Assert.AreEqual(dictionaring.Tag,  "Tag");

			Assert.IsTrue(dictionaring.IsActive);

			Assert.IsTrue(dictionaring.IsConsistent);

			Assert.IsNotNull(dictionaring.ToString());


			Assert.NotNull(dictionaring.GetEnumerator());
			Assert.NotNull(((IEnumerable) dictionaring).GetEnumerator());
			dictionaring.CopyTo(new KeyValuePair<int, string>[3], 0);
			Assert.NotNull(dictionaring.ToString());

			Assert.IsTrue(dictionaring.Contains(new KeyValuePair<int, string>(0, "0")));
			Assert.IsFalse(dictionaring.IsReadOnly);
			Assert.IsTrue(dictionaring.TryGetValue(0, out string value0));
			Assert.AreEqual(value0, "0");
			Assert.IsTrue(dictionaring.Values.SequenceEqual(items.Select(i => i.Value)));

			int changedKey = 0;
			string changedValue = null;
			bool getValueOrDefaultRaised = false;
			bool itemRaised = false;
			bool containsKeyRaised = false;

			dictionaring.MethodChanged += (sender, args) =>
			{
				if (args.MethodName == "GetValueOrDefault" && args.ArgumentsPredicate(new object[]{changedKey, changedValue})) getValueOrDefaultRaised = true;
				if (args.MethodName == "Item[]" && args.ArgumentsPredicate(new object[]{changedKey, changedValue})) itemRaised = true;
				if (args.MethodName == "ContainsKey" && args.ArgumentsPredicate(new object[]{changedKey, changedValue})) containsKeyRaised = true;

				if (dictionaring.HandledEventArgs is NotifyCollectionChangedEventArgs)
				{
					Assert.AreEqual(dictionaring.HandledEventSender, dictionaring.Source);
					Assert.AreEqual(dictionaring.HandledEventArgs, lastNotifyCollectionChangedEventArgs);
				}
			};

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

			dictionaring.ValidateInternalConsistency();

			changedKey = 5;
			changedValue = "2";
			dictionaring.Add(5, "2");
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			changedKey = 7;
			changedValue = "7";
			dictionaring.Add(new KeyValuePair<int, string>(7, "7"));
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			Assert.AreEqual(dictionaring.GetValueOrDefault(5), "2");
			Assert.AreEqual(dictionaring.GetValueOrDefault(-5), null);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			changedKey = 5;
			changedValue = "2";
			Assert.IsTrue(dictionaring.Remove(5));
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			Assert.IsFalse(dictionaring.Remove(10));
			dictionaring.ValidateInternalConsistency();
			Assert.IsFalse(getValueOrDefaultRaised);
			Assert.IsFalse(itemRaised);
			Assert.IsFalse(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			changedKey = 7;
			changedValue = "7";
			Assert.IsTrue(dictionaring.Remove(new KeyValuePair<int, string>(7, "7")));
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			Assert.IsFalse(dictionaring.Remove(new KeyValuePair<int, string>(8, "8")));
			dictionaring.ValidateInternalConsistency();
			Assert.IsFalse(getValueOrDefaultRaised);
			Assert.IsFalse(itemRaised);
			Assert.IsFalse(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			Assert.IsFalse(dictionaring.Remove(new KeyValuePair<int, string>(7, "88")));
			dictionaring.ValidateInternalConsistency();
			Assert.IsFalse(getValueOrDefaultRaised);
			Assert.IsFalse(itemRaised);
			Assert.IsFalse(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			Assert.IsFalse(dictionaring.Remove(new KeyValuePair<int, string>(2, "3")));
			dictionaring.ValidateInternalConsistency();
			Assert.IsFalse(getValueOrDefaultRaised);
			Assert.IsFalse(itemRaised);
			Assert.IsFalse(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			changedKey = 0;
			changedValue = "0";
			dictionaring[0] = "1";
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);
			dictionaring.Clear();
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			inActivationInProgress = true;
			consumer.Dispose();
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

			NotifyCollectionChangedEventArgs lastNotifyCollectionChangedEventArgs = null;
			items.CollectionChanged += (sender, args) =>
			{
				lastNotifyCollectionChangedEventArgs  = args;
			};

			OcConsumer consumer = new OcConsumer();
			ConcurrentDictionaring<Item, int, string> dictionaring = 
				items.ConcurrentDictionaring(i => new Computing<int>(() => i.Id).Value, i => new Computing<string>(() => i.Value).Value);

			bool activationInProgress = true;
			bool inActivationInProgress = false;

			dictionaring.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "ActivationInProgress" || args.PropertyName == "InactivationInProgress") return;
				Assert.AreEqual(dictionaring.ActivationInProgress, activationInProgress);
				Assert.AreEqual(dictionaring.InactivationInProgress, inActivationInProgress);
			};

			dictionaring.For(consumer);
			activationInProgress = false;

			if (OcConfiguration.SaveInstantiationStackTrace)
				Assert.IsNotNull(dictionaring.InstantiationStackTrace);

			Assert.IsNotNull(dictionaring.ToString());

			Assert.IsNull(dictionaring.UserCodeIsCalledFrom);

			Assert.IsTrue(((IComputingInternal) dictionaring).Consumers.Contains(consumer));

			dictionaring.DebugTag = "DebugTag";
			Assert.AreEqual(dictionaring.DebugTag,  "DebugTag");

			dictionaring.Tag = "Tag";
			Assert.AreEqual(dictionaring.Tag,  "Tag");

			Assert.IsTrue(dictionaring.IsActive);

			Assert.IsTrue(dictionaring.IsConsistent);

			Assert.IsNotNull(dictionaring.ToString());

			Assert.NotNull(dictionaring.GetEnumerator());
			Assert.NotNull(((IEnumerable) dictionaring).GetEnumerator());
			dictionaring.CopyTo(new KeyValuePair<int, string>[3], 0);
			Assert.NotNull(dictionaring.ToString());

			Assert.IsTrue(dictionaring.Contains(new KeyValuePair<int, string>(0, "0")));
			Assert.IsFalse(dictionaring.IsReadOnly);
			Assert.IsTrue(dictionaring.TryGetValue(0, out string value0));
			Assert.AreEqual(value0, "0");
			Assert.IsTrue(dictionaring.Values.SequenceEqual(items.Select(i => i.Value)));

			int changedKey = 0;
			string changedValue = null;
			bool getValueOrDefaultRaised = false;
			bool itemRaised = false;
			bool containsKeyRaised = false;

			dictionaring.MethodChanged += (sender, args) =>
			{
				if (args.MethodName == "GetValueOrDefault" && args.ArgumentsPredicate(new object[]{changedKey, changedValue})) getValueOrDefaultRaised = true;
				if (args.MethodName == "Item[]" && args.ArgumentsPredicate(new object[]{changedKey, changedValue})) itemRaised = true;
				if (args.MethodName == "ContainsKey" && args.ArgumentsPredicate(new object[]{changedKey, changedValue})) containsKeyRaised = true;

				if (dictionaring.HandledEventArgs is NotifyCollectionChangedEventArgs)
				{
					Assert.AreEqual(dictionaring.HandledEventSender, dictionaring.Source);
					Assert.AreEqual(dictionaring.HandledEventArgs, lastNotifyCollectionChangedEventArgs);
				}
			};

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

			changedKey = 5;
			changedValue = "2";
			dictionaring.Add(5, "2");
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			Assert.IsTrue(dictionaring.Contains(new KeyValuePair<int, string>(5, "2")));

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			changedKey = 7;
			changedValue = "7";
			dictionaring.Add(new KeyValuePair<int, string>(7, "7"));
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			Assert.AreEqual(dictionaring.GetValueOrDefault(5), "2");
			Assert.AreEqual(dictionaring.GetValueOrDefault(-5), null);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			changedKey = 5;
			changedValue = "2";
			Assert.IsTrue(dictionaring.Remove(5));
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);


			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			Assert.IsFalse(dictionaring.Remove(10));
			dictionaring.ValidateInternalConsistency();
			Assert.IsFalse(getValueOrDefaultRaised);
			Assert.IsFalse(itemRaised);
			Assert.IsFalse(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			changedKey = 7;
			changedValue = "7";
			Assert.IsTrue(dictionaring.Remove(new KeyValuePair<int, string>(7, "7")));
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			Assert.IsFalse(dictionaring.Remove(new KeyValuePair<int, string>(8, "8")));
			dictionaring.ValidateInternalConsistency();
			Assert.IsFalse(getValueOrDefaultRaised);
			Assert.IsFalse(itemRaised);
			Assert.IsFalse(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			Assert.IsFalse(dictionaring.Remove(new KeyValuePair<int, string>(7, "88")));
			dictionaring.ValidateInternalConsistency();
			Assert.IsFalse(getValueOrDefaultRaised);
			Assert.IsFalse(itemRaised);
			Assert.IsFalse(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			Assert.IsFalse(dictionaring.Remove(new KeyValuePair<int, string>(2, "3")));
			dictionaring.ValidateInternalConsistency();
			Assert.IsFalse(getValueOrDefaultRaised);
			Assert.IsFalse(itemRaised);
			Assert.IsFalse(containsKeyRaised);

			getValueOrDefaultRaised = false;
			itemRaised = false;
			containsKeyRaised = false;
			changedKey = 0;
			changedValue = "0";
			dictionaring[0] = "1";
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);
			dictionaring.Clear();
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(getValueOrDefaultRaised);
			Assert.IsTrue(itemRaised);
			Assert.IsTrue(containsKeyRaised);

			inActivationInProgress = true;
			consumer.Dispose();
		}

		[Test]
		public void TestConcurrentDictionaring1()
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

			Assert.Throws<ObservableComputationsException>(() => items.Add(new Item(0, "2")));

			consumer.Dispose();
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

			NotifyCollectionChangedEventArgs lastNotifyCollectionChangedEventArgs = null;
			items.CollectionChanged += (sender, args) =>
			{
				lastNotifyCollectionChangedEventArgs  = args;
			};

			OcConsumer consumer = new OcConsumer();
			HashSetting<Item, int> dictionaring = 
				items.HashSetting(i => new Computing<int>(() => i.Id).Value);

			bool activationInProgress = true;
			bool inActivationInProgress = false;

			dictionaring.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "ActivationInProgress" || args.PropertyName == "InactivationInProgress") return;
				Assert.AreEqual(dictionaring.ActivationInProgress, activationInProgress);
				Assert.AreEqual(dictionaring.InactivationInProgress, inActivationInProgress);
			};

			dictionaring.For(consumer);
			activationInProgress = false;

			if (OcConfiguration.SaveInstantiationStackTrace)
				Assert.IsNotNull(dictionaring.InstantiationStackTrace);

			Assert.AreEqual(dictionaring.Cast<int>().Count(), 3);

			Assert.IsNotNull(dictionaring.ToString());

			Assert.IsNull(dictionaring.UserCodeIsCalledFrom);

			Assert.IsTrue(((IComputingInternal) dictionaring).Consumers.Contains(consumer));

			dictionaring.DebugTag = "DebugTag";
			Assert.AreEqual(dictionaring.DebugTag,  "DebugTag");

			dictionaring.Tag = "Tag";
			Assert.AreEqual(dictionaring.Tag,  "Tag");

			Assert.IsTrue(dictionaring.IsActive);

			Assert.IsTrue(dictionaring.IsConsistent);

			Assert.IsNotNull(dictionaring.ToString());

			Assert.NotNull(dictionaring.GetEnumerator());
			dictionaring.CopyTo(new int[3], 0);
			Assert.NotNull(dictionaring.ToString());

			Assert.IsTrue(dictionaring.Contains(0));
			Assert.IsFalse(dictionaring.IsReadOnly);

			int changedKey = 0;
			bool containsRaised = false;

			dictionaring.MethodChanged += (sender, args) =>
			{
				if (args.MethodName == "Contains" && args.ArgumentsPredicate(new object[]{changedKey})) containsRaised = true;

				if (dictionaring.HandledEventArgs is NotifyCollectionChangedEventArgs)
				{
					Assert.AreEqual(dictionaring.HandledEventSender, dictionaring.Source);
					Assert.AreEqual(dictionaring.HandledEventArgs, lastNotifyCollectionChangedEventArgs);
				}
			};

			Action<int> dictionaringAddItemRequestHandler = (id) => items.Add(new Item(id, "value"));
			dictionaring.AddItemRequestHandler = dictionaringAddItemRequestHandler;
			Assert.IsTrue(dictionaring.AddItemRequestHandler == dictionaringAddItemRequestHandler);

			Func<int, bool> dictionaringRemoveItemRequestHandler = id => items.Remove(items.SingleOrDefault(i => i.Id == id));
			dictionaring.RemoveItemRequestHandler = dictionaringRemoveItemRequestHandler;
			Assert.IsTrue(dictionaring.RemoveItemRequestHandler == dictionaringRemoveItemRequestHandler);

			Action dictionaringClearItemsRequestHandler = () => items.Clear();
			dictionaring.ClearItemsRequestHandler = dictionaringClearItemsRequestHandler;
			Assert.IsTrue(dictionaring.ClearItemsRequestHandler == dictionaringClearItemsRequestHandler);

			changedKey = 5;
			dictionaring.Add(5);
			dictionaring.ValidateInternalConsistency();
			Assert.IsTrue(containsRaised);

			containsRaised  = false;
			Assert.IsTrue(dictionaring.Remove(5));
			Assert.IsTrue(containsRaised);
			dictionaring.ValidateInternalConsistency();

			containsRaised  = false;
			changedKey = 1;
			Assert.IsFalse(dictionaring.Remove(7));
			Assert.IsFalse(containsRaised);
			dictionaring.ValidateInternalConsistency();

			dictionaring.Clear();
			dictionaring.ValidateInternalConsistency();

			inActivationInProgress = true;
			consumer.Dispose();
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

			Assert.AreEqual(grouping[2].Grouping, grouping);
			Assert.AreEqual(grouping[2].Parent, grouping);
			Assert.AreEqual(grouping.GetGroup("3"), grouping[2]);

			grouping[2].Add(new Item(6, "3"));
			grouping.ValidateInternalConsistency();
			Assert.IsTrue(grouping[2].Remove(grouping[2][1]));
			grouping.ValidateInternalConsistency();
			Assert.IsFalse(grouping[2].Remove(new Item(0, "0")));
			grouping.ValidateInternalConsistency();
			grouping[2][0] = new Item(7, "3");
			grouping.ValidateInternalConsistency();
			grouping[2].Move(0, 1);
			grouping.ValidateInternalConsistency();
			grouping[2].Clear();
			grouping.ValidateInternalConsistency();
			consumer.Dispose();
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

			Assert.AreEqual(groupJoining[2].GroupJoining, groupJoining);
			Assert.AreEqual(groupJoining[2].Parent, groupJoining);

			groupJoining[2].Move(0, 1);
			groupJoining.ValidateInternalConsistency();
			groupJoining[2].Add(new Item(6, "3"));
			groupJoining.ValidateInternalConsistency();
			Assert.IsTrue(groupJoining[2].Remove(groupJoining[2][1]));
			groupJoining.ValidateInternalConsistency();
			Assert.IsFalse(groupJoining[2].Remove(new Item(0, "0")));
			groupJoining.ValidateInternalConsistency();
			groupJoining[2][0] = new Item(7, "3");
			groupJoining.ValidateInternalConsistency();
			groupJoining[2].Clear();
			groupJoining.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test]
		public void TestJoining()
		{
			ObservableCollection<Item> leftItems = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(10, "11"), 
					new Item(10, "12"),
					new Item(20, "21"),
					new Item(30, "31"),
					new Item(30, "32"),
					new Item(30, "33")
				});

			ObservableCollection<Item> rightItems = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(10, "10"),
					new Item(20, "20"),
					new Item(30, "30")
				});

			OcConsumer consumer = new OcConsumer();
			Joining<Item, Item> joining = 
				leftItems.Joining(rightItems, (li, ri) => ri.Id == li.Id).For(consumer);

			Action<JoinPair<Item, Item>, Item> setLeftItemRequestHandler = (pair, item) =>
			{
				leftItems[joining.IndexOf(pair)] = item;

			};
			joining.SetLeftItemRequestHandler = setLeftItemRequestHandler;
			Assert.AreEqual(joining.SetLeftItemRequestHandler, setLeftItemRequestHandler);

			Action<JoinPair<Item, Item>, Item> setRightItemRequestHandler = (pair, item) =>
			{
				leftItems[leftItems.IndexOf(pair.LeftItem)] = item;

			};
			joining.SetRightItemRequestHandler = setRightItemRequestHandler;
			Assert.AreEqual(joining.SetRightItemRequestHandler, setRightItemRequestHandler);

			Assert.AreEqual(joining[2].Joining, joining);
			Assert.NotNull(joining[2].ToString());

			joining[2].LeftItem = new Item(50, "50");
			joining[3].RightItem = new Item(70, "70");
			joining.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test]
		public void TestZipping()
		{
			ObservableCollection<Item> leftItems = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(10, "11"), 
					new Item(10, "12"),
					new Item(20, "21"),
					new Item(30, "31"),
					new Item(30, "32"),
					new Item(30, "33")
				});

			ObservableCollection<Item> rightItems = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(10, "10"),
					new Item(20, "20"),
					new Item(30, "30")
				});

			OcConsumer consumer = new OcConsumer();
			Zipping<Item, Item> zipping = 
				leftItems.Zipping(rightItems).For(consumer);

			Action<ZipPair<Item, Item>, Item> setLeftItemRequestHandler = (pair, item) =>
			{
				leftItems[zipping.IndexOf(pair)] = item;

			};
			zipping.SetLeftItemRequestHandler = setLeftItemRequestHandler;
			Assert.AreEqual(zipping.SetLeftItemRequestHandler, setLeftItemRequestHandler);

			Action<ZipPair<Item, Item>, Item> setRightItemRequestHandler = (pair, item) =>
			{
				rightItems[zipping.IndexOf(pair)] = item;

			};
			zipping.SetRightItemRequestHandler = setRightItemRequestHandler;
			Assert.AreEqual(zipping.SetRightItemRequestHandler, setRightItemRequestHandler);
			Assert.NotNull(zipping[2].ToString());

			zipping[2].LeftItem = new Item(50, "50");
			zipping[1].RightItem = new Item(70, "70");
			zipping.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test]
		public void TestComputing()
		{
			Item item = new Item(0, "0");
			OcConsumer consumer = new OcConsumer();

			Expression<Func<int>> valueExpression = () => item.Id;
			Computing<int> computing = new Computing<int>(valueExpression);
			Assert.AreEqual(computing.GetValueExpression, valueExpression);
			Differing<int> differing = computing.Differing();
			item.Computing = computing;
				
			bool activationInProgress = true;
			bool inActivationInProgress = false;

			computing.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "ActivationInProgress" || args.PropertyName == "InactivationInProgress") return;
				Assert.AreEqual(computing.ActivationInProgress, activationInProgress);
				Assert.AreEqual(computing.InactivationInProgress, inActivationInProgress);
			};

			differing.For(consumer);

			activationInProgress = false;
			Assert.IsTrue(computing.Consumers.Contains(consumer));

			Assert.IsTrue(((IComputingInternal) differing).Consumers.Contains(consumer));
			Assert.IsTrue(computing.UserCodeIsCalledFrom == null);

			Action<int> computingSetValueRequestHandler = i =>
			{
				item.Id = i;
			};
			computing.SetValueRequestHandler += computingSetValueRequestHandler;
			Assert.AreEqual(computing.SetValueRequestHandler, computingSetValueRequestHandler);

			bool disposing = false;

			int value = 1;

			computing.PreValueChanged += (sender, args) =>
			{
				if (disposing) return;
				Assert.AreEqual(computing.NewValue, value);
				Assert.AreEqual(computing.NewValueObject, value);
				Assert.AreEqual(computing.HandledEventSender, item);
				Assert.AreEqual(computing.HandledEventArgs, item._lastPropertyChangedEventArgs);
			};

			computing.PostValueChanged += (sender, args) =>
			{
				if (disposing) return;
				Assert.AreEqual(computing.NewValue, value);
				Assert.AreEqual(computing.NewValueObject, value);
				Assert.AreEqual(computing.HandledEventSender, item);
				Assert.AreEqual(computing.HandledEventArgs, item._lastPropertyChangedEventArgs);
			};

			computing.Value = 1;

			Assert.AreEqual(computing.Value, 1);
			Assert.AreEqual(computing.ValueObject, 1);

			value = 2;
			computing.ValueObject = 2;

			Assert.AreEqual(computing.Value, 2);
			Assert.AreEqual(computing.ValueObject, 2);

			Assert.AreEqual(computing.ValueType, typeof(int));

			Assert.IsNotNull(computing.ToString());

			computing.DebugTag = "DebugTag";
			Assert.AreEqual(computing.DebugTag,  "DebugTag");

			computing.Tag = "Tag";
			Assert.AreEqual(computing.Tag,  "Tag");

			Assert.IsTrue(computing.IsActive);

			Assert.IsTrue(computing.IsConsistent);

			Assert.IsNotNull(computing.ToString());

			if (OcConfiguration.SaveInstantiationStackTrace)
				Assert.IsNotNull(computing.InstantiationStackTrace);


			disposing = true;
			inActivationInProgress = true;
			consumer.Dispose();
		}

		[Test]
		public void CollectionComputingChildTest()
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

			NotifyCollectionChangedEventArgs lastNotifyCollectionChangedEventArgs = null;
			items.CollectionChanged += (sender, args) =>
			{
				lastNotifyCollectionChangedEventArgs  = args;
			};

			OcConsumer consumer = new OcConsumer();
			Grouping<Item, string> grouping = 
				items.Grouping(i => i.Value);

			bool activationInProgress = true;
			bool inActivationInProgress = false;


			grouping.CollectionChanged += (sender, args) =>
			{
				Assert.AreEqual(grouping.ActivationInProgress, activationInProgress);
				Assert.AreEqual(grouping.InactivationInProgress, inActivationInProgress);
				if (grouping.Count >= 3)
				{
					Assert.AreEqual(grouping[2].ActivationInProgress, activationInProgress);
					Assert.AreEqual(grouping[2].InactivationInProgress, inActivationInProgress);
				}
			};

			grouping.For(consumer);

			activationInProgress = false;

			Group<Item, string> group = grouping[2];

			group.DebugTag = "DebugTag";
			Assert.AreEqual(group.DebugTag,  "DebugTag");

			Assert.IsNull(group.UserCodeIsCalledFrom);

			group.Tag = "Tag";
			Assert.AreEqual(group.Tag,  "Tag");

			Assert.IsTrue(group.IsActive);

			Assert.AreEqual(group.ItemType, typeof(Item));

			Assert.IsTrue(group.IsConsistent);

			if (OcConfiguration.SaveInstantiationStackTrace)
				Assert.IsNotNull(group.InstantiationStackTrace);



			PropertyInfo newItemPropertyInfo = group.GetType().GetProperty("NewItem");

			NotifyCollectionChangedAction? currentChange = null;
			Item newItem = null;
			int oldIndex = -1;
			int newIndex = -1;

			group.PreCollectionChanged += (sender, args) =>
			{
				Assert.AreEqual(group.CurrentChange, currentChange);
				Assert.AreEqual(newItemPropertyInfo.GetValue(group), newItem);
				Assert.AreEqual(group.NewItemObject, newItem);
				Assert.AreEqual(group.OldIndex, oldIndex);
				Assert.AreEqual(group.NewIndex, newIndex);

				if (group.HandledEventArgs is NotifyCollectionChangedEventArgs)
				{
					Assert.AreEqual(group.HandledEventSender, items);
					Assert.AreEqual(group.HandledEventArgs, lastNotifyCollectionChangedEventArgs);
				}
			};

			group.PostCollectionChanged += (sender, args) =>
			{
				Assert.AreEqual(group.CurrentChange, currentChange);
				Assert.AreEqual(newItemPropertyInfo.GetValue(group), newItem);
				Assert.AreEqual(group.NewItemObject, newItem);
				Assert.AreEqual(group.OldIndex, oldIndex);
				Assert.AreEqual(group.NewIndex, newIndex);
			};

			currentChange = NotifyCollectionChangedAction.Add;
			newItem = new Item(6, "3");
			oldIndex = -1;
			newIndex = 0;
			items.Insert(0, newItem);

			currentChange = NotifyCollectionChangedAction.Remove;
			newItem = default;
			oldIndex = 0;
			newIndex = -1;
			items.RemoveAt(3);

			currentChange = NotifyCollectionChangedAction.Replace;
			newItem = new Item(7, "3");
			oldIndex = 0;
			newIndex = 0;
			items[0] = newItem;

			currentChange = NotifyCollectionChangedAction.Move;
			newItem = default;
			oldIndex = 1;
			newIndex = 2;
			items.Move(3, 4);

			inActivationInProgress = true;
			consumer.Dispose();
		}

		public ChangeRequestHandlersTests(bool debug) : base(debug)
		{

		}
	}
}
