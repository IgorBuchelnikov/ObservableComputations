using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace IBCode.ObservableComputations.Test
{
	[TestFixture]
	public class ContainsComputingTests
	{
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

			public override bool Equals(object obj)
			{
				return IsActive == ((Item)obj).IsActive;
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
		public void ContainsComputing_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			ContainsComputing<Item> anyComputing = items.ContainsComputing(new Item(true));
			anyComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ContainsComputing_Remove(
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

			ContainsComputing<Item> anyComputing = items.ContainsComputing(new Item(true));
			anyComputing.ValidateConsistency();
			items.RemoveAt(index);
			anyComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ContainsComputing_Remove1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			ContainsComputing<Item> anyComputing = items.ContainsComputing(new Item(true));
			anyComputing.ValidateConsistency();
			items.RemoveAt(0);
			anyComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ContainsComputing_Insert(
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

			ContainsComputing<Item> anyComputing = items.ContainsComputing(new Item(true));
			anyComputing.ValidateConsistency();
			items.Insert(index, new Item(newValue));
			anyComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ContainsComputing_Insert1(
			[Values(true, false)] bool newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
			);

			ContainsComputing<Item> anyComputing = items.ContainsComputing(new Item(true));
			anyComputing.ValidateConsistency();
			items.Insert(0, new Item(newValue));
			anyComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ContainsComputing_Move(
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

			ContainsComputing<Item> anyComputing = items.ContainsComputing(new Item(true));
			anyComputing.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			anyComputing.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void ContainsComputing_Set(
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

			ContainsComputing<Item> anyComputing = items.ContainsComputing(new Item(true));
			anyComputing.ValidateConsistency();
			items[index] = new Item(itemNew);
			anyComputing.ValidateConsistency();
		}		
	}
}