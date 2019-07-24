using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace IBCode.ObservableCalculations.Test
{
	[TestFixture]
	public class LastCalculatingTests
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
		public void Last_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			LastCalculating<Item> last = items.LastCalculating();
			last.ValidateConsistency();
		}


		[Test, Combinatorial]
		public void Last_Remove(
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

			LastCalculating<Item> last = items.LastCalculating();
			last.ValidateConsistency();
			items.RemoveAt(index);
			last.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Last_Remove1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			LastCalculating<Item> last = items.LastCalculating();
			last.ValidateConsistency();
			items.RemoveAt(0);
			last.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Last_Insert(
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

			LastCalculating<Item> last = items.LastCalculating();
			last.ValidateConsistency();
			items.Insert(index, new Item());
			last.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Last_Insert1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			LastCalculating<Item> last = items.LastCalculating();
			last.ValidateConsistency();
			items.Insert(0, new Item());
			last.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Last_Move(
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

			LastCalculating<Item> last = items.LastCalculating();
			last.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			last.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void Last_Set(
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

			LastCalculating<Item> last = items.LastCalculating();
			last.ValidateConsistency();
			items[index] = new Item();
			last.ValidateConsistency();
		}		
	}
}