// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public class TakingWhileTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item : INotifyPropertyChanged
		{
			private bool _isActive;

			public bool IsActive
			{
				get { return _isActive; }
				set { updatePropertyValue(ref _isActive, value); }
			}

			public Item(bool isActive)
			{
				_isActive = isActive;
				Num = LastNum;
				LastNum++;
			}

			public static int LastNum;
			public int Num;

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
		public void TakingWhile_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();		
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Change(
			[Values(true, false)] bool item0,
			[Values(true, false)] bool item1,
			[Values(true, false)] bool item2,
			[Values(true, false)] bool item3,
			[Values(true, false)] bool item4,
			[Range(0, 4, 1)] int index,
			[Values(true, false)] bool newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0),
					new Item(item1),
					new Item(item2),
					new Item(item3),
					new Item(item4)
				}

			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();
			items[index].IsActive = newValue;
			takingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Change1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();
			items[0].IsActive = !items[0].IsActive;
			takingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Remove(
			[Values(true, false)] bool item0,
			[Values(true, false)] bool item1,
			[Values(true, false)] bool item2,
			[Values(true, false)] bool item3,
			[Values(true, false)] bool item4,
			[Range(0, 4, 1)] int index)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0),
					new Item(item1),
					new Item(item2),
					new Item(item3),
					new Item(item4)
				}

			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();
			items.RemoveAt(index);
			takingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Remove1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();
			items.RemoveAt(0);
			takingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Insert(
			[Values(true, false)] bool item0,
			[Values(true, false)] bool item1,
			[Values(true, false)] bool item2,
			[Values(true, false)] bool item3,
			[Values(true, false)] bool item4,
			[Range(0, 4, 1)] int index,
			[Values(true, false)] bool newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0),
					new Item(item1),
					new Item(item2),
					new Item(item3),
					new Item(item4)
				}

			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();
			items.Insert(index, new Item(newValue));
			takingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Insert1(
			[Values(true, false)] bool newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(

			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();
			items.Insert(0, new Item(newValue));
			takingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Move(
			[Values(true, false)] bool item0,
			[Values(true, false)] bool item1,
			[Values(true, false)] bool item2,
			[Values(true, false)] bool item3,
			[Values(true, false)] bool item4,
			[Range(0, 4, 1)] int oldIndex,
			[Range(0, 4, 1)] int newIndex)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0),
					new Item(item1),
					new Item(item2),
					new Item(item3),
					new Item(item4)
				}

			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			takingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Set(
			[Values(true, false)] bool item0,
			[Values(true, false)] bool item1,
			[Values(true, false)] bool item2,
			[Values(true, false)] bool item3,
			[Values(true, false)] bool item4,
			[Range(0, 4, 1)] int index,
			[Values(true, false)] bool itemNew)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0),
					new Item(item1),
					new Item(item2),
					new Item(item3),
					new Item(item4)
				}

			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => item.IsActive).For(consumer);
			takingWhile.ValidateConsistency();
			items[index] = new Item(itemNew);
			takingWhile.ValidateConsistency();			
			consumer.Dispose();
		}		

		[Test]
		public void SubExpessing()
		{
			Param param = new Param();
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(true),
					new Item(false),
					new Item(true),
					new Item(false),
					new Item(true)
				}
			);

			TakingWhile<Item> takingWhile = items.TakingWhile(item => 
				Expr.Is(() => param.Value 
					? (ObservableCollection<Item>)items.TakingWhile(item1 => true) 
					: items.TakingWhile(item1 => item1.IsActive == item.IsActive)).Computing().Value.Count == 3).For(consumer);

			TakingWhile<Item> takingWhile2 = items.TakingWhile(item => 
				(param.Value 
					? items.TakingWhile(item1 => true) 
					: items.TakingWhile(item1 => item1.IsActive == item.IsActive)).Count == 3).For(consumer);

			takingWhile.ValidateConsistency();
			takingWhile2.ValidateConsistency();

			param.Value = true;

			takingWhile.ValidateConsistency();
			takingWhile2.ValidateConsistency();
			consumer.Dispose();
		}

		public class Param : INotifyPropertyChanged
		{
			private bool _value;

			public bool Value
			{
				get => _value;
				set
				{
					_value = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

		}

		public TakingWhileTests(bool debug) : base(debug)
		{
		}
	}
}