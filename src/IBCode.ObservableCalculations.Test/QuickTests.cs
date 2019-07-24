using System.Collections.ObjectModel;
using System.ComponentModel;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace IBCode.ObservableCalculations.Test
{
	public partial class QuickTests
	{
		public class Item : INotifyPropertyChanged
		{
			private bool _active;
			private int _num;
			private ObservableCollection<Item> _items;

			public Item(int num, bool active)
			{
				_active = active;
				_num = num;
				Id = ++_lastId;
			}

			public bool Active
			{
				get => _active;
				set
				{
					_active = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Active)));
				}
			}

			public int Num
			{
				get => _num;
				set
				{
					_num = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Num)));
				}
			}

			public int Id {get;}
			private static int _lastId;

			public ObservableCollection<Item> Items
			{
				get
				{
					if (_items == null)
					{
						_items = new ObservableCollection<Item>{_item1, _item3, _item5, _item7};
					}
					return _items;
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			#region Overrides of Object

			public override string ToString()
			{
				return Num.ToString();
			}

			#endregion
		}

		private static Item _item1 = new Item(1, true);
		private static Item _item2 = new Item(2, true);
		private static Item _item3 = new Item(3, true);
		private static Item _item4 = new Item(4, true);
		private static Item _item5 = new Item(5, true);
		private static Item _item6 = new Item(6, true);
		private static Item _item7 = new Item(7, true);

		private static bool _mode;

		public ObservableCollection<Item> getItems()
		{
			if (_mode) return new ObservableCollection<Item>(new []{_item1, _item2, _item3, _item4, _item5});
			else  return new ObservableCollection<Item>(new []{_item3, _item4, _item5, _item6, _item7});
			_mode = !_mode;
		}

		public ObservableCollection<ObservableCollection<Item>> getCollectionsOfItems()
		{
			return new ObservableCollection<ObservableCollection<Item>>(new []{
				new ObservableCollection<Item>(new []{_item1, _item2, _item3, _item4, _item5}),
				new ObservableCollection<Item>(new []{_item3, _item4, _item5, _item6, _item7})});
;
		}

		public Scalar<TValue> getScalar<TValue>(TValue value)
		{
			return new Scalar<TValue>(value);
		}
	}
}
