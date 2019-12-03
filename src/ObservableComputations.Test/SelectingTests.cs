using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class SelectingTests
	{
		public class Item : INotifyPropertyChanged
		{
			public Item()
			{
				Num = LastNum;
				LastNum++;
			}

			public Item(int num)
			{
				_num = num;
			}

			public static int LastNum;
			private int _num;
			public int Num
			{
				get => _num;
				set => updatePropertyValue(ref _num, value);
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
		public void Selecting_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Selecting<Item, int> selecting = items.Selecting(item => item.Num);
			selecting.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Selecting_Change(
			[Range(0, 4, 1)] int index,
			[Range(-1, 5)] int newValue)
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

			Selecting<Item, int> selecting = items.Selecting(item => item.Num);
			selecting.ValidateConsistency();
			items[index].Num = newValue;
			selecting.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Selecting_Remove(
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

			Selecting<Item, int> selecting = items.Selecting(item => item.Num);
			selecting.ValidateConsistency();
			items.RemoveAt(index);
			selecting.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Selecting_Remove1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}

			);

			Selecting<Item, int> selecting = items.Selecting(item => item.Num);
			selecting.ValidateConsistency();
			items.RemoveAt(0);
			selecting.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Selecting_Insert(
			[Range(0, 4, 1)] int index,
			[Range(-1, 5)] int newValue)
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

			Selecting<Item, int> selecting = items.Selecting(item => item.Num);
			selecting.ValidateConsistency();
			items.Insert(index, new Item(newValue));
			selecting.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Selecting_Insert1(
			[Range(-1, 5)] int newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
			);

			Selecting<Item, int> selecting = items.Selecting(item => item.Num);
			selecting.ValidateConsistency();
			items.Insert(0, new Item(newValue));
			selecting.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Selecting_Move(
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

			Selecting<Item, int> selecting = items.Selecting(item => item.Num);
			selecting.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			selecting.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Selecting_Set(
			[Range(0, 4, 1)] int index,
			[Range(-1, 5)] int newValue)
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

			Selecting<Item, int> selecting = items.Selecting(item => item.Num);
			selecting.ValidateConsistency();
			items[index] = new Item(newValue);
			selecting.ValidateConsistency();


		}	
		
		[Test, Combinatorial]
		public void Selecting_Dispose(
			[Range(0, 4, 1)] int index,
			[Range(-1, 5)] int newValue)
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

			WeakReference<Selecting<Item, int>> selectingWeakReference = null;

			Action action = () =>
			{
				Selecting<Item, int> selecting = items.Selecting(item => item.Num);
				selectingWeakReference = new WeakReference<Selecting<Item, int>>(selecting);
			};

			action();
			GC.Collect();
			Assert.IsFalse(selectingWeakReference.TryGetTarget(out Selecting<Item, int> s));
		}
	}
}