using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public class CollectionDisposingTest : TestBase
	{
		public CollectionDisposingTest(bool debug) : base(debug)
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

		private void test(CollectionDisposing<Item> collectionDisposing, ObservableCollection<Item> source)
		{
			Assert.IsTrue(source.All(i => !i.Disposed));
			Item item = source[0];
			source.RemoveAt(0);
			Assert.IsTrue(item.Disposed);
		}

		[Test]
		public void Test1()
		{
			ObservableCollection<Item> source = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			OcConsumer consumer = new OcConsumer();
			
			test(source.CollectionDisposing().For(consumer), source);

			consumer.Dispose();
		}

		[Test]
		public void Test2()
		{
			ObservableCollection<Item> source = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			OcConsumer consumer = new OcConsumer();
			
			test(new Scalar<ObservableCollection<Item>>(source).CollectionDisposing().For(consumer), source);

			consumer.Dispose();
		}

		[Test]
		public void Test3()
		{
			ObservableCollection<Item> source = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			OcConsumer consumer = new OcConsumer();
			
			test(Expr.Is(() => source).CollectionDisposing().For(consumer), source);

			consumer.Dispose();
		}

		[Test]
		public void Test4()
		{
			ObservableCollection<Item> source = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			OcConsumer consumer = new OcConsumer();
			
			test(((INotifyCollectionChanged)source).CollectionDisposing<Item>().For(consumer), source);

			consumer.Dispose();
		}

		[Test]
		public void Test5()
		{
			ObservableCollection<Item> source = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			OcConsumer consumer = new OcConsumer();
			
			test(new Scalar<INotifyCollectionChanged>(source).CollectionDisposing<Item>().For(consumer), source);

			consumer.Dispose();
		}

		[Test]
		public void Test6()
		{
			ObservableCollection<Item> source = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}
			);

			OcConsumer consumer = new OcConsumer();
			
			test(Expr.Is(() => (INotifyCollectionChanged)source).CollectionDisposing<Item>().For(consumer), source);

			consumer.Dispose();
		}
	}
}
