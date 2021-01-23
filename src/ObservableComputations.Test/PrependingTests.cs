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
	public partial class PrependingTests : TestBase
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
		public void Prepending_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Prepending<Item> prepending = items.Prepending(new Item()).For(consumer);
			prepending.ValidateConsistency();			
			consumer.Dispose();
		}


		[Test, Combinatorial]
		public void Prepending_Remove(
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

			Prepending<Item> prepending = items.Prepending(new Item()).For(consumer);
			prepending.ValidateConsistency();
			items.RemoveAt(index);
			prepending.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Prepending_Remove1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}

			);

			Prepending<Item> prepending = items.Prepending(new Item()).For(consumer);
			prepending.ValidateConsistency();
			items.RemoveAt(0);
			prepending.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Prepending_Insert(
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

			Prepending<Item> prepending = items.Prepending(new Item()).For(consumer);
			prepending.ValidateConsistency();
			items.Insert(index, new Item());
			prepending.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Prepending_Insert1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Prepending<Item> prepending = items.Prepending(new Item()).For(consumer);
			prepending.ValidateConsistency();
			items.Insert(0, new Item());
			prepending.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Prepending_Move(
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

			Prepending<Item> prepending = items.Prepending(new Item()).For(consumer);
			prepending.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			prepending.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Prepending_Set(
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

			Prepending<Item> prepending = items.Prepending(new Item()).For(consumer);
			prepending.ValidateConsistency();
			items[index] = new Item();
			prepending.ValidateConsistency();			
			consumer.Dispose();
		}

		public PrependingTests(bool debug) : base(debug)
		{
		}
	}
}