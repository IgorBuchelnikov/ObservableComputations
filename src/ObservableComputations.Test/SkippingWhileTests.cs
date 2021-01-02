using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class SkippingWhileTests
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
		public void SkipingWhile_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void SkipingWhile_Change(
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

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();
			items[index].IsActive = newValue;
			skippingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void SkipingWhile_Change1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();
			items[0].IsActive = !items[0].IsActive;
			skippingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void SkipingWhile_Remove(
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

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();
			items.RemoveAt(index);
			skippingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void SkipingWhile_Remove1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();
			items.RemoveAt(0);
			skippingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void SkipingWhile_Insert(
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

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();
			items.Insert(index, new Item(newValue));
			skippingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void SkipingWhile_Insert1(
			[Values(true, false)] bool newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(

			);

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();
			items.Insert(0, new Item(newValue));
			skippingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void SkipingWhile_Move(
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

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			skippingWhile.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void SkipingWhile_Set(
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

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => item.IsActive).For(consumer);
			skippingWhile.ValidateConsistency();
			items[index] = new Item(itemNew);
			skippingWhile.ValidateConsistency();			
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

			SkippingWhile<Item> skippingWhile = items.SkippingWhile(item => 
				Expr.Is(() => param.Value 
					? (ObservableCollection<Item>)items.SkippingWhile(item1 => true) 
					: items.SkippingWhile(item1 => item1.IsActive == item.IsActive)).Computing().Value.Count == 3).For(consumer);

			SkippingWhile<Item> skippingWhile2 = items.SkippingWhile(item => 
				(param.Value 
					? items.SkippingWhile(item1 => true) 
					: items.SkippingWhile(item1 => item1.IsActive == item.IsActive)).Count == 3).For(consumer);

			skippingWhile.ValidateConsistency();
			skippingWhile2.ValidateConsistency();

			param.Value = true;

			skippingWhile.ValidateConsistency();
			skippingWhile2.ValidateConsistency();
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

	}
}