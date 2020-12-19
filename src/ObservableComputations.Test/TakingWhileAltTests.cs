using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class TakingWhileAltTests
	{
		Consumer consumer = new Consumer();

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
		public void TakingWhile_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();
			INotifyCollectionChanged takingWhile = getTakingWhile(items);

			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);	
			consumer.Dispose();
		}

		private INotifyCollectionChanged getTakingWhile(ObservableCollection<Item> items)
		{
			return (INotifyCollectionChanged)Expr.Is(
					() => (INotifyCollectionChanged)items.Using(
						sc => Expr.Is(() => ((IList)sc).Count)
							.SequenceComputing().SetDebugTag("SequenceComputing")
							.Zipping<int, Item>(() => sc).SetDebugTag("Zipping")).Value)   
				.Computing().SetDebugTag("ZippingComputing")
				.Using(zipping => 
					zipping.Filtering<ZipPair<int, Item>>(
						zp => zp.LeftItem < 
							  zipping.Filtering<ZipPair<int, Item>>(i => 
									  !i.RightItem.IsActive).SetDebugTag("FilteringInner")
								  .Selecting(zp1 => zp1.LeftItem).SetDebugTag("SelectingInner").Using(ii => ii.Count > 0 ? ii.Minimazing().SetDebugTag("Minimazing").Value : items.Count).SetDebugTag("UsingMinimazing").Value).SetDebugTag("FilteringOuter")
				).SetDebugTag("ZippingUsing").For(consumer).Value
				.Selecting(zp => zp.RightItem).SetDebugTag("SelectingOuter").For(consumer);
		}

		[Test, Combinatorial]
		public void TakingWhile_Change(
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

			INotifyCollectionChanged takingWhile = getTakingWhile(items);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);
			items[index].IsActive = newValue;
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Change1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			INotifyCollectionChanged takingWhile = getTakingWhile(items);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);
			items[0].IsActive = !items[0].IsActive;
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);		
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Remove(
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

			INotifyCollectionChanged takingWhile = getTakingWhile(items);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);
			items.RemoveAt(index);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Remove1(
			[Values(true, false)] bool item0)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(item0)
				}

			);

			INotifyCollectionChanged takingWhile = getTakingWhile(items);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);
			items.RemoveAt(0);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);		
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Insert(
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

			INotifyCollectionChanged takingWhile = getTakingWhile(items);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);
			items.Insert(index, new Item(newValue));
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);		
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Insert1(
			[Values(true, false)] bool newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(

			);

			INotifyCollectionChanged takingWhile = getTakingWhile(items);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);
			items.Insert(0, new Item(newValue));
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);		
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Move(
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

			INotifyCollectionChanged takingWhile = getTakingWhile(items);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);
			items.Move(oldIndex, newIndex);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void TakingWhile_Set(
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

			INotifyCollectionChanged takingWhile = getTakingWhile(items);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);
			items[index] = new Item(itemNew);
			ValidateConsistency(items, (ObservableCollection<Item>) takingWhile);			
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

		public new void ValidateConsistency(ObservableCollection<Item> items, ObservableCollection<Item> takingWhile)
		{
			// ReSharper disable once AssignNullToNotNullAttribute
			if (!takingWhile.SequenceEqual(items.TakeWhile((si, i) => si.IsActive)))
			{
				throw new Exception("Consistency violation: TakingWhileAlt.1");
			}
		}

	}
}