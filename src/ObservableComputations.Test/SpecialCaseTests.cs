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

			public Computing<int> NumComputing;
			public Computing<int> NumPlus1Computing;

			public Computing<int> ActualNumComputing
			{
				get => _actualNumComputing;
				set
				{
					_actualNumComputing = value;
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
				NumComputing = new Computing<int>(() => Num);
				NumPlus1Computing = new Computing<int>(() => Num + 1);
				_actualNumComputing = NumComputing;
			}

			public static int LastNum;
			public int Num;

			private bool _specialChangeIsActive;
			private Computing<int> _actualNumComputing;

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
			filtering.ValidateConsistency();
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
			filtering.ValidateConsistency();
			consumer.Dispose();

		}

		[Test]
		public void Test3()
		{
			Item.LastNum = 3;
			Item item = new Item(false);
			Computing<int> comp = new Computing<int>(() => item.ActualNumComputing.Value).For(consumer);
			
			Assert.IsTrue(comp.Value == 3);
			Assert.IsTrue(item.NumComputing.IsActive);
			Assert.IsTrue(!item.NumPlus1Computing.IsActive);

			item.ActualNumComputing = item.NumPlus1Computing;

			Assert.IsTrue(comp.Value == 4);
			Assert.IsTrue(!item.NumComputing.IsActive);
			Assert.IsTrue(item.NumPlus1Computing.IsActive);

			consumer.Dispose();

			Assert.IsTrue(!item.NumComputing.IsActive);
			Assert.IsTrue(!item.NumPlus1Computing.IsActive);
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

			Selecting<Item, int> sel = items.Selecting(i => i.ActualNumComputing.Value).For(consumer);
			
			Assert.IsTrue(sel[0] == 3);
			Assert.IsTrue(items[0].NumComputing.IsActive);
			Assert.IsTrue(!items[0].NumPlus1Computing.IsActive);

			items[0].ActualNumComputing = items[0].NumPlus1Computing;

			Assert.IsTrue(sel[0] == 4);
			Assert.IsTrue(!items[0].NumComputing.IsActive);
			Assert.IsTrue(items[0].NumPlus1Computing.IsActive);

			consumer.Dispose();

			Assert.IsTrue(!items[0].NumComputing.IsActive);
			Assert.IsTrue(!items[0].NumPlus1Computing.IsActive);

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

		public SpecialCaseTests(bool debug) : base(debug)
		{
		}
	}
}
