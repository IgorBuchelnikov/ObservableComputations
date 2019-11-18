using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace IBCode.ObservableComputations.Test
{
	[TestFixture]
	public class ToStringTests
	{
		public class Item : INotifyPropertyChanged
		{
			public Item()
			{
				Num = LastNum;
				LastNum++;
			}

			public Item(int num)
			{
				_num = num;
			}

			public static int LastNum;
			private int _num;
			public int Num
			{
				get => _num;
				set => updatePropertyValue(ref _num, value);
			}

			#region INotifyPropertyChanged imlementation

			public event PropertyChangedEventHandler PropertyChanged;

			protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChangedEventHandler onPropertyChanged = PropertyChanged;
				if (onPropertyChanged != null)
					onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}

			protected bool updatePropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
			{
				if (EqualityComparer<T>.Default.Equals(field, value))
					return false;
				field = value;
				this.onPropertyChanged(propertyName);
				return true;
			}

			#endregion
		}

		[Test]
		public void Selecting_Change()
		{
			ObservableCollection<Item> items1 = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}
			);

			ReadOnlyObservableCollection<Item> items = new ReadOnlyObservableCollection<Item>(new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}
			));

			//var calc = items.Using(
			//	itemsUsing => items1.Selecting(i => i.Num + items1[i.Num].Num));
			//var calc = items.Zipping(Expr.Is(() => items.Count).Computing().SequenceComputing()).Using(
			//	z => z.Selecting(zp => zp.ItemLeft));

			//var calc = items.Zipping(Expr.Is(() => items.Count).Computing().SequenceComputing());
			//var calc = items.Selecting(i => i.Num);

			//string test = calc.ToString();

			var using1 =
				items1.Zipping(
					Expr.Is(() => items.Count).Computing().SequenceComputing()
				).Using(
					z =>
						z.Filtering(
							zp => zp.ItemRight >
								  z.Filtering(zp1 => zp1.ItemLeft.Num > 1)
								  .Selecting(zp3 => zp3.ItemRight).Maximazing().Value
						).Selecting(zp4 => zp4.ItemLeft)
				);

			string test = using1.ToString();
		}
	}
}