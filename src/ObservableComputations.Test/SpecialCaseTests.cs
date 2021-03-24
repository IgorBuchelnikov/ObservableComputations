// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class SpecialCaseTests : TestBase
	{


		OcConsumer consumer = new OcConsumer();

		public class Item : INotifyPropertyChanged
		{
			private bool _isActive;

			public Computing<int> ZeroComputing;
			public Computing<int> OneComputing;

			private Computing<int> _actualNumComputing1;
			public Computing<int> ActualNumComputing1
			{
				get => _actualNumComputing1;
				set
				{
					_actualNumComputing1 = value;
					onPropertyChanged();
				}
			}

			private Computing<int> _actualNumComputing2;
			public Computing<int> ActualNumComputing2
			{
				get => _actualNumComputing2;
				set
				{
					_actualNumComputing2 = value;
					onPropertyChanged();
				}
			}

			public bool IsActive
			{
				get
				{
					if (_specialChangeIsActive)
					{
						_specialChangeIsActive = false;
						IsActive = !_isActive;
					}
					return _isActive;
				}
				set { updatePropertyValue(ref _isActive, value); }
			}

			public Item(bool isActive)
			{
				_isActive = isActive;
				Num = LastNum;
				LastNum++;
				ZeroComputing = new Computing<int>(() => 0);
				OneComputing = new Computing<int>(() => 1);
				_actualNumComputing1 = ZeroComputing;
				_actualNumComputing2 = ZeroComputing;
			}

			public static int LastNum;
			public int Num;

			private bool _specialChangeIsActive;

			public void SpecialChangeIsActive()
			{
				_specialChangeIsActive = true;
				IsActive = !_isActive;
				_specialChangeIsActive = false;
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

			public override string ToString()
			{
				return $"Item(IsActive={_isActive}, Num={Num})";
			}
		}

		[Test]
		public void Test1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			Filtering<Item> filtering = items.Filtering(i => i.IsActive).For(consumer);
			items[2].SpecialChangeIsActive();
			filtering.ValidateInternalConsistency();
			consumer.Dispose();

		}

		[Test]
		public void Test2()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(true),
					new Item(true),
					new Item(true),
					new Item(true)
				}
			);

			bool removed = false;
			Filtering<Item> filtering = items.Filtering(i => i.IsActive).For(consumer);
			filtering.CollectionChanged += (sender, args) =>
			{
				if (!removed)
				{
					removed = true;
					items.RemoveAt(0);
				}
			};
			items[0].IsActive = true;		   
			filtering.ValidateInternalConsistency();
			consumer.Dispose();

		}



		[Test]
		public void Test3()
		{
			Item.LastNum = 3;
			Item item = new Item(false);
			Computing<int> comp = new Computing<int>(() => item.ActualNumComputing1.Value + item.ActualNumComputing2.Value).For(consumer);
			
			Assert.IsTrue(comp.Value == 0);
			Assert.IsTrue(item.ZeroComputing.IsActive);
			Assert.IsTrue(!item.OneComputing.IsActive);

			item.ActualNumComputing1 = item.OneComputing;

			Assert.IsTrue(comp.Value == 1);
			Assert.IsTrue(item.ZeroComputing.IsActive);
			Assert.IsTrue(item.OneComputing.IsActive);

			item.ActualNumComputing2 = item.OneComputing;

			Assert.IsTrue(comp.Value == 2);
			Assert.IsTrue(!item.ZeroComputing.IsActive);
			Assert.IsTrue(item.OneComputing.IsActive);

			consumer.Dispose();

			Assert.IsTrue(!item.ZeroComputing.IsActive);
			Assert.IsTrue(!item.OneComputing.IsActive);
		}

		[Test]
		public void Test4()
		{
			Item.LastNum = 3;
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(false),
				}
			);

			Selecting<Item, int> sel = items.Selecting(i => i.ActualNumComputing1.Value).For(consumer);
			
			Assert.IsTrue(sel[0] == 0);
			Assert.IsTrue(items[0].ZeroComputing.IsActive);
			Assert.IsTrue(!items[0].OneComputing.IsActive);

			items[0].ActualNumComputing1 = items[0].OneComputing;

			Assert.IsTrue(sel[0] == 1);
			Assert.IsTrue(!items[0].ZeroComputing.IsActive);
			Assert.IsTrue(items[0].OneComputing.IsActive);

			items[0].ActualNumComputing1 = items[0].ZeroComputing;

			Assert.IsTrue(sel[0] == 0);
			Assert.IsTrue(items[0].ZeroComputing.IsActive);
			Assert.IsTrue(!items[0].OneComputing.IsActive);

			consumer.Dispose();

			Assert.IsTrue(!items[0].ZeroComputing.IsActive);
			Assert.IsTrue(!items[0].OneComputing.IsActive);

		}

		[ObservableComputationsCall]
		public string SomeStringFunction(string s)
		{
			return null;
		}

		[Test]
		public void TestNonStaticObservableComputationsCall()
		{
			Exception exception = null;

			ObservableCollection<string> items = new ObservableCollection<string>(
				new[]
				{
					"a",
				}
			);

			try
			{
				items.Filtering(i => SomeStringFunction(i) == null).For(consumer);
			}
			catch (Exception e)
			{
				exception = e;
			}

			Assert.IsTrue(exception != null);

			consumer.Dispose();
		}

		[Test]
		public void TestDeferredReset()
		{
			Exception exception = null;

			ObservableCollection<string> items = new ObservableCollection<string>(
				new[]
				{
					"a",
				}
			);

			ObservableCollection<string> items2 = new ObservableCollection<string>(
				new[]
				{
					"b",
				}
			);

			Scalar<ObservableCollection<string>> sourceScalar = new Scalar<ObservableCollection<string>>(items);

			Selecting<string, int> selecting = sourceScalar.Selecting(s => s.Length).For(consumer);

			selecting.CollectionChanged += (sender, args) =>
			{
				if (args.Action != NotifyCollectionChangedAction.Add) return;
				sourceScalar.Change(items2);
			};

			items.Add("q");

			Assert.IsTrue(selecting.SequenceEqual(items2.Select(s => s.Length)));

			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateCastingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			Casting<Item> casting = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					casting = items.Casting<Item>().For(consumer);
					casting.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			casting?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateSelectingOnCountChanged2(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			Selecting<Item, Item> selecting = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					selecting = items.Selecting(i => i).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			selecting?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateSelecting2OnCountChanged3(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			Selecting<Item, Item> testingSelecting = null;

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			((INotifyPropertyChanged) selecting).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					testingSelecting = selecting.Selecting(i => i).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			testingSelecting?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateCollectionDispatchingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcDispatcher dispatcher = new OcDispatcher();
			CollectionDispatching<Item> collectionDispatching = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					collectionDispatching = items.CollectionDispatching(dispatcher).For(consumer);
					collectionDispatching.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			dispatcher.Pass();
			collectionDispatching?.ValidateInternalConsistency();
			dispatcher.Dispose();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateCollectionPausingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			CollectionPausing<Item> collectionPausing = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					collectionPausing = items.CollectionPausing().For(consumer);
					collectionPausing.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			collectionPausing?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateCollectionProcessingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			CollectionProcessing<Item, Item> collectionProcessing = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					collectionProcessing = items.CollectionProcessing((items1, computing) => items1).For(consumer);
				}
			};

				int count = 4;
			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					count = 5;
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					count = 3;
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false),
						new Item(false),
						new Item(false)
					});
					break;
			}

			if (collectionProcessing != null) Assert.AreEqual(collectionProcessing.Count, count);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateCollectionProcessingVoidOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			CollectionProcessingVoid<Item> collectionProcessing = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					collectionProcessing = items.CollectionProcessing((items1, computing) => { }).For(consumer);
				}
			};

			int count = 4;
			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					count = 5;
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					count = 3;
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false),
						new Item(false),
						new Item(false)
					});
					break;
			}

			if (collectionProcessing != null) Assert.AreEqual(collectionProcessing.Count, count);
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateConcatenatingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<MiscTests.MyObservableCollection<Item>> items = new MiscTests.MyObservableCollection<MiscTests.MyObservableCollection<Item>>(
				new[]
				{
					new MiscTests.MyObservableCollection<Item>(
						new[]
						{
							new Item(false),
							new Item(false),
							new Item(false),
							new Item(false)
						}),
					new MiscTests.MyObservableCollection<Item>(
						new[]
						{
							new Item(false),
							new Item(false),
							new Item(false),
							new Item(false)
						})
				}
			);

			Concatenating<Item> concatenating = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					concatenating = items.Concatenating<Item>().For(consumer);
					concatenating.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new MiscTests.MyObservableCollection<Item>(
						new[]
						{
							new Item(false),
							new Item(false),
							new Item(false),
							new Item(false)
						}));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new MiscTests.MyObservableCollection<Item>(
						new[]
						{
							new Item(false),
							new Item(false),
							new Item(false),
							new Item(false)
						});
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(
						new[]
						{
							new MiscTests.MyObservableCollection<Item>(
								new[]
								{
									new Item(false),
									new Item(false),
									new Item(false),
									new Item(false)
								})
						});
					break;
			}

			concatenating?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateConcurrentDictionaringOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			ConcurrentDictionaring<Item, int, Item> concurrentDictionaring = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					concurrentDictionaring = selecting.ConcurrentDictionaring(i => i.Num, i => i).For(consumer);
					concurrentDictionaring.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			concurrentDictionaring?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateDictionaringOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			Dictionaring<Item, int, Item> dictionaring = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					dictionaring = selecting.Dictionaring(i => i.Num, i => i).For(consumer);
					dictionaring.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			dictionaring?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateFilteringOnCountChanged2(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			Filtering<Item> filtering = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					filtering = selecting.Filtering(i => true).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			filtering?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateGroupingOnCountChanged2(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			Grouping<Item, int> grouping = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					grouping = selecting.Grouping(i => i.Num).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			grouping?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateGroupJoiningOnCountChanged2(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			GroupJoining<Item, Item, int> groupJoining = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					groupJoining = selecting.GroupJoining(selecting, i => i.Num, i => i.Num).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			groupJoining?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateHashSettingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			HashSetting<Item, int> hashSetting = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					hashSetting = items.HashSetting(i => i.Num).For(consumer);
					hashSetting.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			hashSetting?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateJoiningOnCountChanged2(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			Joining<Item, Item> joining = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					joining = items.Joining(items, (i1, i2) => i1.Num == i2.Num).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			joining?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateOrderingOnCountChanged2(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			Ordering<Item, int> ordering = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					ordering = selecting.Ordering(i => i.Num).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			ordering?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateThenOrderingOnCountChanged3(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			ThenOrdering<Item, int> thenOrdering = null;

			OcConsumer consumer = new OcConsumer();
			Ordering<Item, int> ordering = items.Ordering(i => i.Num).For(consumer);

			((INotifyPropertyChanged) ordering).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					thenOrdering = ordering.ThenOrdering(i => i.Num).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			thenOrdering?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreatePagingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			Paging<Item> paging = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					paging = items.Paging(2).For(consumer);
					paging.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			paging?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateZippingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			Zipping<Item, Item> zipping = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					zipping = items.Zipping(items).For(consumer);
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			zipping?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateAggregatingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			Summarizing<int> summarizing = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					summarizing = selecting.Selecting(i => i.Num).Summarizing().For(consumer);
					summarizing.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			summarizing?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateAnyComputingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			AnyComputing<Item> anyComputing = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					anyComputing = selecting.AnyComputing<Item>(i => true).For(consumer);
					anyComputing.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			anyComputing?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateItemComputingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			ItemComputing<Item> itemComputing = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					OcConsumer consumer = new OcConsumer();
					itemComputing = items.ItemComputing<Item>(1).For(consumer);
					itemComputing.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			itemComputing?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TestCreateMinimazingOnCountChanged(
			[Values(NotifyCollectionChangedAction.Add, NotifyCollectionChangedAction.Reset, NotifyCollectionChangedAction.Move, NotifyCollectionChangedAction.Remove, NotifyCollectionChangedAction.Replace)]
			NotifyCollectionChangedAction action,
			[Values("Count", "Item[]")] string propertyName)
		{
			MiscTests.MyObservableCollection<Item> items = new MiscTests.MyObservableCollection<Item>(
				new[]
				{
					new Item(false),
					new Item(false),
					new Item(false),
					new Item(false)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Selecting<Item, Item> selecting = items.Selecting(i => i).For(consumer);

			MinimazingOrMaximazing<int> minimazing = null;

			((INotifyPropertyChanged) items).PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == propertyName)
				{
					minimazing = selecting.Selecting(i => i.Num).Minimazing().For(consumer);
					minimazing.ValidateInternalConsistency();
				}
			};

			switch (action)
			{
				case NotifyCollectionChangedAction.Add:
					items.Add(new Item(false));
					break;
				case NotifyCollectionChangedAction.Remove:
					items.RemoveAt(0);
					break;
				case NotifyCollectionChangedAction.Replace:
					items[0] = new Item(false);
					break;
				case NotifyCollectionChangedAction.Move:
					items.Move(0, 1);
					break;
				case NotifyCollectionChangedAction.Reset:
					items.Reset(new[]
					{
						new Item(false),
						new Item(false)
					});
					break;
			}

			minimazing?.ValidateInternalConsistency();
			consumer.Dispose();
		}

		public SpecialCaseTests(bool debug) : base(debug)
		{
		}
	}
}
