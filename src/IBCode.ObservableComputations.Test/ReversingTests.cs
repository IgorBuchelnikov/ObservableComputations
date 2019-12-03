using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class ReversingTests
	{
		public class Item : INotifyPropertyChanged
		{
			public Item()
			{
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
		}

		[Test]
		public void Reversing_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Reversing<Item> reversing = items.Reversing();
			reversing.ValidateConsistency();
		}


		[Test, Combinatorial]
		public void Reversing_Remove(
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

			Reversing<Item> reversing = items.Reversing();
			reversing.ValidateConsistency();
			items.RemoveAt(index);
			reversing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Reversing_Remove1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			Reversing<Item> reversing = items.Reversing();
			reversing.ValidateConsistency();
			items.RemoveAt(0);
			reversing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Reversing_Insert(
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
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}
			);

			Reversing<Item> reversing = items.Reversing();
			reversing.ValidateConsistency();
			items.Insert(index, new Item());
			reversing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Reversing_Insert1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Reversing<Item> reversing = items.Reversing();
			reversing.ValidateConsistency();
			items.Insert(0, new Item());
			reversing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Reversing_Move(
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

			Reversing<Item> reversing = items.Reversing();
			reversing.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			reversing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Reversing_Set(
			[Range(0, 4, 1)] int index,
			[Values(true, false)] bool itemNew)
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

			Reversing<Item> reversing = items.Reversing();
			reversing.ValidateConsistency();
			items[index] = new Item();
			reversing.ValidateConsistency();
		}		



	}
}