using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(true)]
	[TestFixture(false)]
	public class ChangeRequestHandlersTests : TestBase
	{
		public class Item : INotifyPropertyChanged
		{
			public Item(int id, string value)
			{
				_id = id;
				_value = value;
				Num = LastNum;
				LastNum++;
			}

			public static int LastNum;
			public int Num;

			private int _id;

			public int Id
			{
				get { return _id; }
				set { updatePropertyValue(ref _id, value); }
			}

			private string _value;

			public string Value
			{
				get { return _value; }
				set { updatePropertyValue(ref _value, value); }
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
		public void TestDictionaring()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(0, "0"), 
					new Item(1, "1"),
					new Item(2, "2")
				});

			OcConsumer consumer = new OcConsumer();
			Dictionaring<Item, int, string> dictionaring = 
				items.Dictionaring(i => i.Id, i => i.Value).For(consumer);

			dictionaring.AddItemRequestHandler = (id, value) => items.Add(new Item(id, value));
			dictionaring.SetItemRequestHandler = (id, value) => items.SingleOrDefault(i => i.Id == id).Value = value;
			dictionaring.RemoveItemRequestHandler = id => items.Remove(items.SingleOrDefault(i => i.Id == id));
			dictionaring.ClearItemsRequestHandler = () => items.Clear();

			dictionaring.Add(5, "2");
			dictionaring.ValidateConsistency();
			Assert.IsTrue(dictionaring.Remove(5));
			dictionaring.ValidateConsistency();
			Assert.IsFalse(dictionaring.Remove(7));
			dictionaring.ValidateConsistency();
			dictionaring[0] = "1";
			dictionaring.ValidateConsistency();
			dictionaring.Clear();
			dictionaring.ValidateConsistency();
		}

		[Test]
		public void TestConcurrentDictionaring()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(0, "0"), 
					new Item(1, "1"),
					new Item(2, "2")
				});

			OcConsumer consumer = new OcConsumer();
			ConcurrentDictionaring<Item, int, string> dictionaring = 
				items.ConcurrentDictionaring(i => i.Id, i => i.Value).For(consumer);

			dictionaring.AddItemRequestHandler = (id, value) => items.Add(new Item(id, value));
			dictionaring.SetItemRequestHandler = (id, value) => items.SingleOrDefault(i => i.Id == id).Value = value;
			dictionaring.RemoveItemRequestHandler = id => items.Remove(items.SingleOrDefault(i => i.Id == id));
			dictionaring.ClearItemsRequestHandler = () => items.Clear();

			dictionaring.Add(5, "2");
			dictionaring.ValidateConsistency();
			Assert.IsTrue(dictionaring.Remove(5));
			dictionaring.ValidateConsistency();
			Assert.IsFalse(dictionaring.Remove(7));
			dictionaring.ValidateConsistency();
			dictionaring[0] = "1";
			dictionaring.ValidateConsistency();
			dictionaring.Clear();
			dictionaring.ValidateConsistency();
		}

		[Test]
		public void TestHashSetting()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new Item[]
				{
					new Item(0, "0"), 
					new Item(1, "1"),
					new Item(2, "2")
				});

			OcConsumer consumer = new OcConsumer();
			HashSetting<Item, int> dictionaring = 
				items.HashSetting(i => i.Id).For(consumer);

			dictionaring.AddItemRequestHandler = (id) => items.Add(new Item(id, "value"));
			dictionaring.RemoveItemRequestHandler = id => items.Remove(items.SingleOrDefault(i => i.Id == id));
			dictionaring.ClearItemsRequestHandler = () => items.Clear();

			dictionaring.Add(5);
			dictionaring.ValidateConsistency();
			Assert.IsTrue(dictionaring.Remove(5));
			dictionaring.ValidateConsistency();
			Assert.IsFalse(dictionaring.Remove(7));
			dictionaring.ValidateConsistency();
			dictionaring.Clear();
			dictionaring.ValidateConsistency();
		}

		public ChangeRequestHandlersTests(bool debug) : base(debug)
		{
		}
	}
}
