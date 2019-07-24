using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace IBCode.ObservableCalculations.Test
{
	[TestFixture]
	public class IndicesCalculatingTests
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
		public void IndicesCalculating_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

			IndicesCalculating<Item> indicesCalculating = items.IndicesCalculating(item => item.IsActive);
			indicesCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void IndicesCalculating_Change(
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

			IndicesCalculating<Item> indicesCalculating = items.IndicesCalculating(item => item.IsActive);
			indicesCalculating.ValidateConsistency();
			items[index].IsActive = newValue;
			indicesCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void IndicesCalculating_Remove(
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

			IndicesCalculating<Item> indicesCalculating = items.IndicesCalculating(item => item.IsActive);
			indicesCalculating.ValidateConsistency();
			items.RemoveAt(index);
			indicesCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void IndicesCalculating_Remove1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			IndicesCalculating<Item> indicesCalculating = items.IndicesCalculating(item => item.IsActive);
			indicesCalculating.ValidateConsistency();
			items.RemoveAt(0);
			indicesCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void IndicesCalculating_Insert(
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

			IndicesCalculating<Item> indicesCalculating = items.IndicesCalculating(item => item.IsActive);
			indicesCalculating.ValidateConsistency();
			items.Insert(index, new Item(newValue));
			indicesCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void IndicesCalculating_Insert1(
			[Values(true, false)] bool newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(

			);

			IndicesCalculating<Item> indicesCalculating = items.IndicesCalculating(item => item.IsActive);
			indicesCalculating.ValidateConsistency();
			items.Insert(0, new Item(newValue));
			indicesCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void IndicesCalculating_Move(
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

			IndicesCalculating<Item> indicesCalculating = items.IndicesCalculating(item => item.IsActive);
			indicesCalculating.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			indicesCalculating.ValidateConsistency();
		}

		[Test, Combinatorial]
		public void IndicesCalculating_Set(
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

			IndicesCalculating<Item> indicesCalculating = items.IndicesCalculating(item => item.IsActive);
			indicesCalculating.ValidateConsistency();
			items[index] = new Item(itemNew);
			indicesCalculating.ValidateConsistency();
		}		
	}
}