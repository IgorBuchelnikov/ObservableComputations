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
	public partial class LastComputingTests : TestBase
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
		public void Last_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			LastComputing<Item> last = items.LastComputing().For(consumer);
			last.ValidateConsistency();			
			consumer.Dispose();
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

			LastComputing<Item> last = items.LastComputing().For(consumer);
			last.ValidateConsistency();
			items.RemoveAt(index);
			last.ValidateConsistency();			
			consumer.Dispose();
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

			LastComputing<Item> last = items.LastComputing().For(consumer);
			last.ValidateConsistency();
			items.RemoveAt(0);
			last.ValidateConsistency();			
			consumer.Dispose();
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

			LastComputing<Item> last = items.LastComputing().For(consumer);
			last.ValidateConsistency();
			items.Insert(index, new Item());
			last.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Last_Insert1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			LastComputing<Item> last = items.LastComputing().For(consumer);
			last.ValidateConsistency();
			items.Insert(0, new Item());
			last.ValidateConsistency();			
			consumer.Dispose();
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

			LastComputing<Item> last = items.LastComputing().For(consumer);
			last.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			last.ValidateConsistency();			
			consumer.Dispose();
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

			LastComputing<Item> last = items.LastComputing().For(consumer);
			last.ValidateConsistency();
			items[index] = new Item();
			last.ValidateConsistency();			
			consumer.Dispose();
		}

		public LastComputingTests(bool debug) : base(debug)
		{
		}
	}
}