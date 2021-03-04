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
	public partial class FirstComputingTests : TestBase
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
			first.ValidateInternalConsistency();			
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
			first.ValidateInternalConsistency();
			items.RemoveAt(index);
			first.ValidateInternalConsistency();			
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
			first.ValidateInternalConsistency();
			items.RemoveAt(0);
			first.ValidateInternalConsistency();			
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
			first.ValidateInternalConsistency();
			items.Insert(index, new Item());
			first.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void First_Insert1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			FirstComputing<Item> first = items.FirstComputing().For(consumer);
			first.ValidateInternalConsistency();
			items.Insert(0, new Item());
			first.ValidateInternalConsistency();			
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
			first.ValidateInternalConsistency();
			items.Move(oldIndex, newIndex);
			first.ValidateInternalConsistency();			
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
			first.ValidateInternalConsistency();
			items[index] = new Item();
			first.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		public FirstComputingTests(bool debug) : base(debug)
		{
		}
	}
}