using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class FirstComputingTests
	{
		OcConsumer consumer = new OcConsumer();

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
		public void First_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			FirstComputing<Item> first = items.FirstComputing().For(consumer);
			first.ValidateConsistency();			
			consumer.Dispose();
		}


		[Test, Combinatorial]
		public void First_Remove(
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

			FirstComputing<Item> first = items.FirstComputing().For(consumer);
			first.ValidateConsistency();
			items.RemoveAt(index);
			first.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void First_Remove1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			FirstComputing<Item> first = items.FirstComputing().For(consumer);
			first.ValidateConsistency();
			items.RemoveAt(0);
			first.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void First_Insert(
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

			FirstComputing<Item> first = items.FirstComputing().For(consumer);
			first.ValidateConsistency();
			items.Insert(index, new Item());
			first.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void First_Insert1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			FirstComputing<Item> first = items.FirstComputing().For(consumer);
			first.ValidateConsistency();
			items.Insert(0, new Item());
			first.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void First_Move(
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

			FirstComputing<Item> first = items.FirstComputing().For(consumer);
			first.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			first.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void First_Set(
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

			FirstComputing<Item> first = items.FirstComputing().For(consumer);
			first.ValidateConsistency();
			items[index] = new Item();
			first.ValidateConsistency();			
			consumer.Dispose();
		}		
	}
}