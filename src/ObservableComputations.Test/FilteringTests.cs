﻿// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class FilteringTests : TestBase
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
		}

		[Test]
		public void Filtering_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Filtering_Change(
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

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();
			items[index].IsActive = newValue;
			filtering.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Filtering_Change1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();
			items[0].IsActive = !items[0].IsActive;
			filtering.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Filtering_Remove(
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

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();
			items.RemoveAt(index);
			filtering.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Filtering_Remove1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();
			items.RemoveAt(0);
			filtering.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Filtering_Insert(
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

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();
			items.Insert(index, new Item(newValue));
			filtering.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Filtering_Insert1(
			[Values(true, false)] bool newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(

			);

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();
			items.Insert(0, new Item(newValue));
			filtering.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Filtering_Move(
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

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();
			items.Move(oldIndex, newIndex);
			filtering.ValidateInternalConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Filtering_Set(
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

			Filtering<Item> filtering = items.Filtering(item => item.IsActive).For(consumer);
			filtering.ValidateInternalConsistency();
			items[index] = new Item(itemNew);
			filtering.ValidateInternalConsistency();			
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

			Filtering<Item> filtering = Expr.Is(() => items).Computing().Filtering(item =>
				Expr.Is(() => param.Value
					? (ObservableCollection<Item>)items.Filtering(item1 => true)
					: items.Filtering(item1 => item1.IsActive == item.IsActive)).Computing().Value.Count == 3).For(consumer);

			Filtering<Item> filtering2 = items.Filtering(item =>
				(param.Value
					? items.Filtering(item1 => true)
					: items.Filtering(item1 => item1.IsActive == item.IsActive)).Count == 3).For(consumer);

			Expression<Func<ObservableCollection<Item>>> expression = () => param.Value
				? (ObservableCollection<Item>)items.Filtering(item1 => true)
				: items.Filtering(item1 => item1.IsActive == false);

			Selecting<Item, bool> selecting = expression.Computing().Selecting(item => item.IsActive).For(consumer);

			filtering.ValidateInternalConsistency();
			filtering2.ValidateInternalConsistency();

			param.Value = true;

			filtering.ValidateInternalConsistency();
			filtering2.ValidateInternalConsistency();
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

		public FilteringTests(bool debug) : base(debug)
		{
		}
	}
}