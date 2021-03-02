using System;
using System.ComponentModel;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public class ScalarDisposingTest : TestBase
	{
		public ScalarDisposingTest(bool debug) : base(debug)
		{
		}

		private class Item : IDisposable
		{
			public bool Disposed;

			public void Dispose()
			{
				Disposed = true;
			}
		}

		private class ItemHolder : INotifyPropertyChanged
		{
			private Item _item;

			public Item Item
			{
				get => _item;
				set
				{
					_item = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item)));
				}
			}

			#region Implementation of INotifyPropertyChanged

			public event PropertyChangedEventHandler PropertyChanged;

			#endregion
		}

		private void test(ScalarDisposing<Item> scalarDisposing, Action changeValue)
		{
			Item item = scalarDisposing.Value;
			Assert.IsTrue(!item.Disposed);
			changeValue();
			Assert.IsTrue(item.Disposed);
		}

		[Test]
		public void Test1()
		{
			Scalar<Item> scalar = new Scalar<Item>(new Item());

			OcConsumer consumer = new OcConsumer();
			
			test(scalar.ScalarDisposing().For(consumer), () => scalar.Change(new Item()));

			consumer.Dispose();
		}

		[Test]
		public void Test2()
		{
			OcConsumer consumer = new OcConsumer();
			
			ItemHolder itemHolder = new ItemHolder();
			itemHolder.Item = new Item();
			test(Expr.Is(() => itemHolder.Item).ScalarDisposing().For(consumer), () => itemHolder.Item = new Item());

			consumer.Dispose();
		}
	}
}
