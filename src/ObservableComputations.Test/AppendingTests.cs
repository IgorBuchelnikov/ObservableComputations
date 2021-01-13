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
	public class AppendingTests : TestBase
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
		public void Appending_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Appending<Item> appending = items.Appending(new Item()).For(consumer);
			appending.ValidateConsistency();			
			consumer.Dispose();
		}


		[Test, Combinatorial]
		public void Appending_Remove(
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

			Appending<Item> appending = items.Appending(new Item()).For(consumer);
			appending.ValidateConsistency();
			items.RemoveAt(index);
			appending.ValidateConsistency();			
			consumer.Dispose();			
		}

		[Test, Combinatorial]
		public void Appending_Remove1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}

			);

			Appending<Item> appending = items.Appending(new Item()).For(consumer);
			appending.ValidateConsistency();
			items.RemoveAt(0);
			appending.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Appending_Insert(
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

			Appending<Item> appending = items.Appending(new Item()).For(consumer);
			appending.ValidateConsistency();
			items.Insert(index, new Item());
			appending.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Appending_Insert1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Appending<Item> appending = items.Appending(new Item()).For(consumer);
			appending.ValidateConsistency();
			items.Insert(0, new Item());
			appending.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Appending_Move(
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

			Appending<Item> appending = items.Appending(new Item()).For(consumer);
			appending.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			appending.ValidateConsistency();		
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Appending_Set(
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

			Appending<Item> appending = items.Appending(new Item()).For(consumer);
			appending.ValidateConsistency();
			items[index] = new Item();
			appending.ValidateConsistency();			
			consumer.Dispose();
		}

		public AppendingTests(bool debug) : base(debug)
		{
		}
	}
}