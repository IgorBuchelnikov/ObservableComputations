using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class SelectingTests
	{
        Consumer consumer = new Consumer();

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
		public void Selecting_Initialization_01()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>();

            Consumer consumer = new Consumer();
			Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
			selecting.ValidateConsistency();
            consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Selecting_Change(
			[Range(0, 4, 1)] int index,
			[Range(-1, 5)] int newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}

			);

            Consumer consumer = new Consumer();
			Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
			selecting.ValidateConsistency();
			items[index].Num = newValue;
			selecting.ValidateConsistency();
            consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Selecting_Remove(
			[Range(0, 4, 1)] int index)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}

			);

            Consumer consumer = new Consumer();
			Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
			selecting.ValidateConsistency();
			items.RemoveAt(index);
			selecting.ValidateConsistency();
            consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Selecting_Remove1()
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item()
				}

			);

            Consumer consumer = new Consumer();
			Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
			selecting.ValidateConsistency();
			items.RemoveAt(0);
			selecting.ValidateConsistency();
            consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Selecting_Insert(
			[Range(0, 4, 1)] int index,
			[Range(-1, 5)] int newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}

			);

            Consumer consumer = new Consumer();
			Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
			selecting.ValidateConsistency();
			items.Insert(index, new Item(newValue));
			selecting.ValidateConsistency();
            consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Selecting_Insert1(
			[Range(-1, 5)] int newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
			);

            Consumer consumer = new Consumer();
			Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
			selecting.ValidateConsistency();
			items.Insert(0, new Item(newValue));
			selecting.ValidateConsistency();
            consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Selecting_Move(
			[Range(0, 4, 1)] int oldIndex,
			[Range(0, 4, 1)] int newIndex)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}

			);

            Consumer consumer = new Consumer();
			Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
			selecting.ValidateConsistency();
			items.Move(oldIndex, newIndex);
			selecting.ValidateConsistency();
            consumer.Dispose();
		}

		[Test, Combinatorial]
		public void Selecting_Set(
			[Range(0, 4, 1)] int index,
			[Range(-1, 5)] int newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}

			);

            Consumer consumer = new Consumer();
			Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
			selecting.ValidateConsistency();
			items[index] = new Item(newValue);
			selecting.ValidateConsistency();
            consumer.Dispose();
		}	

        [Test]
        public void Selecting_Nested()
        {
            Item.LastNum = 0;
            ObservableCollection<Item> items = new ObservableCollection<Item>(
                new[]
                {
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item()
                }

            );

            Consumer consumer = new Consumer();
            Selecting<Item, int> selecting = items.Selecting(item => items.Selecting(i => i).Count + item.Num).IsNeededFor(consumer);

            Assert.IsTrue(new int[]{5, 6, 7, 8, 9}.SequenceEqual(selecting));
            consumer.Dispose();

        }

        [Test]
        public void Selecting_Nested3()
        {
            Item.LastNum = 0;
            ObservableCollection<Item> items = new ObservableCollection<Item>(
                new[]
                {
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item()
                }

            );

            Consumer consumer = new Consumer();
            var selecting = items.Selecting(item => items.Selecting(i => i)).IsNeededFor(consumer);

            consumer.Dispose();

        }

        private int Dummy(object o1, object o2)
        {
            return 0;
        }

        [Test]
        public void Selecting_Nested2()
        {
            Item.LastNum = 0;

            ObservableCollection<Item> items = new ObservableCollection<Item>(
                new[]
                {
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item()
                }

            );

            Consumer consumer = new Consumer();
            Selecting<Item, int> selecting = items.Selecting(item => items.Selecting(i => item).Count + item.Num).IsNeededFor(consumer);

            Assert.IsTrue(new int[]{5, 6, 7, 8, 9}.SequenceEqual(selecting));
            consumer.Dispose();
        }
		
		[Test, Combinatorial]
		public void Selecting_Dispose(
			[Range(0, 4, 1)] int index,
			[Range(-1, 5)] int newValue)
		{
			ObservableCollection<Item> items = new ObservableCollection<Item>(
				new[]
				{
					new Item(),
					new Item(),
					new Item(),
					new Item(),
					new Item()
				}

			);

			WeakReference<Selecting<Item, int>> selectingWeakReference = null;

			Action action = () =>
			{
                Consumer consumer = new Consumer();
				Selecting<Item, int> selecting = items.Selecting(item => item.Num).IsNeededFor(consumer);
                consumer.Dispose();
				selectingWeakReference = new WeakReference<Selecting<Item, int>>(selecting);
			};

			action();

            GC.AddMemoryPressure(1024 * 1024 * 1024);
            Thread.Sleep(10);
            GC.Collect();
            Thread.Sleep(50);
            GC.Collect();
            Thread.Sleep(10);
            GC.Collect();
            GC.RemoveMemoryPressure(1024 * 1024 * 1024);

			Assert.IsFalse(selectingWeakReference.TryGetTarget(out Selecting<Item, int> s));
		}

        [Test, Combinatorial]
        public void Selecting_Dispose2(
            [Range(0, 4, 1)] int index,
            [Range(-1, 5)] int newValue)
        {
            ObservableCollection<Item> items = new ObservableCollection<Item>(
                new[]
                {
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item()
                }

            );

            WeakReference<Selecting<Item, int>> selectingWeakReference = null;

            Action action = () =>
            {
                Consumer consumer = new Consumer();
                Selecting<Item, int> selecting = items.Selecting(item => items.Selecting(i => i).Count + item.Num).IsNeededFor(consumer);
                consumer.Dispose();
                selectingWeakReference = new WeakReference<Selecting<Item, int>>(selecting);
            };

            action();

            GC.AddMemoryPressure(1024 * 1024 * 1024);
            Thread.Sleep(10);
            GC.Collect();
            Thread.Sleep(50);
            GC.Collect();
            Thread.Sleep(10);
            GC.Collect();
            GC.RemoveMemoryPressure(1024 * 1024 * 1024);

            Assert.IsFalse(selectingWeakReference.TryGetTarget(out Selecting<Item, int> s));
        }

        [Test, Combinatorial]
        public void Selecting_Dispose3(
            [Range(0, 4, 1)] int index,
            [Range(-1, 5)] int newValue)
        {
            ObservableCollection<Item> items = new ObservableCollection<Item>(
                new[]
                {
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item()
                }

            );

            WeakReference<Selecting<Item, int>> selectingWeakReference = null;

            Action action = () =>
            {
                Consumer consumer = new Consumer();
                Selecting<Item, int> selecting = items.Selecting(item => items.Selecting(i => item).Count + item.Num).IsNeededFor(consumer);
                consumer.Dispose();
                selectingWeakReference = new WeakReference<Selecting<Item, int>>(selecting);
            };

            action();

            GC.AddMemoryPressure(1024 * 1024 * 1024);
            Thread.Sleep(10);
            GC.Collect();
            Thread.Sleep(50);
            GC.Collect();
            Thread.Sleep(10);
            GC.Collect();
            GC.RemoveMemoryPressure(1024 * 1024 * 1024);

            Assert.IsFalse(selectingWeakReference.TryGetTarget(out Selecting<Item, int> s));
        }

        [Test, Combinatorial]
        public void Selecting_InitAndDispose4(
            [Range(0, 4, 1)] int index,
            [Range(-1, 5)] int newValue)
        {
            Item.LastNum = 0;
            ObservableCollection<Item> items = new ObservableCollection<Item>(
                new[]
                {
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item()
                }
            );

            WeakReference<Selecting<Item, int>> selectingWeakReference = null;

            Action action = () =>
            {
                Consumer consumer = new Consumer();
                Selecting<Item, int> selecting = items.Selecting(item => item).Selecting(item => item).Selecting(item => item.Num).IsNeededFor(consumer);
                Assert.IsTrue(new int[]{0, 1, 2, 3, 4}.SequenceEqual(selecting));
                consumer.Dispose();
                selectingWeakReference = new WeakReference<Selecting<Item, int>>(selecting);
            };

            action();

            GC.AddMemoryPressure(1024 * 1024 * 1024);
            Thread.Sleep(10);
            GC.Collect();
            Thread.Sleep(50);
            GC.Collect();
            Thread.Sleep(10);
            GC.Collect();
            GC.RemoveMemoryPressure(1024 * 1024 * 1024);

            Assert.IsFalse(selectingWeakReference.TryGetTarget(out Selecting<Item, int> s));
        }

        [Test, Combinatorial]
        public void Selecting_InitAndDispose5(
            [Range(0, 4, 1)] int index,
            [Range(-1, 5)] int newValue)
        {
            Item.LastNum = 0;
            ObservableCollection<Item> items = new ObservableCollection<Item>(
                new[]
                {
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item()
                }
            );

            WeakReference<Selecting<Item, int>> selectingWeakReference = null;

            Action action = () =>
            {
                Consumer consumer = new Consumer();
                Selecting<Item, Item> selecting1 = items.Selecting(item => item);
                Selecting<Item, Item> selecting2 = items.Selecting(item => item);
                Scalar<INotifyCollectionChanged> sourceScalar = new Scalar<INotifyCollectionChanged>(selecting1); 
                Selecting<Item, int> selecting = sourceScalar.Selecting<Item, int>(item => item.Num).IsNeededFor(consumer);
                sourceScalar.Change(selecting2);
                Assert.IsTrue(new int[]{0, 1, 2, 3, 4}.SequenceEqual(selecting));
                consumer.Dispose();
                selectingWeakReference = new WeakReference<Selecting<Item, int>>(selecting);
            };

            action();

            GC.AddMemoryPressure(1024 * 1024 * 1024);
            Thread.Sleep(10);
            GC.Collect();
            Thread.Sleep(50);
            GC.Collect();
            Thread.Sleep(10);
            GC.Collect();
            GC.RemoveMemoryPressure(1024 * 1024 * 1024);

            Assert.IsFalse(selectingWeakReference.TryGetTarget(out Selecting<Item, int> s));
        }

        [Test, Combinatorial]
        public void Selecting_InitAndDispose6(
            [Range(0, 4, 1)] int index,
            [Range(-1, 5)] int newValue)
        {
            Item.LastNum = 0;
            ObservableCollection<Item> items = new ObservableCollection<Item>(
                new[]
                {
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item(),
                    new Item()
                }
            );

            WeakReference<Selecting<Item, int>> selectingWeakReference = null;

            Action action = () =>
            {
                Consumer consumer = new Consumer();
                Selecting<Item, Item> selecting1 = items.Selecting(item => item);
                Selecting<Item, Item> selecting2 = items.Selecting(item => item);
                Scalar<INotifyCollectionChanged> sourceScalar = new Scalar<INotifyCollectionChanged>(null); 
                Selecting<Item, int> selecting = sourceScalar.Selecting<Item, Item>(item => item).Selecting(item => item.Num).IsNeededFor(consumer);
                sourceScalar.Change(selecting1);
                sourceScalar.Change(selecting2);
                Assert.IsTrue(new int[]{0, 1, 2, 3, 4}.SequenceEqual(selecting));
                consumer.Dispose();
                selectingWeakReference = new WeakReference<Selecting<Item, int>>(selecting);
            };

            action();

            GC.AddMemoryPressure(1024 * 1024 * 1024);
            Thread.Sleep(10);
            GC.Collect();
            Thread.Sleep(50);
            GC.Collect();
            Thread.Sleep(10);
            GC.Collect();
            GC.RemoveMemoryPressure(1024 * 1024 * 1024);

            Assert.IsFalse(selectingWeakReference.TryGetTarget(out Selecting<Item, int> s));
        }
	}
}