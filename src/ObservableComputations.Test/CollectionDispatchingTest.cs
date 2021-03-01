// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class CollectionDispatchingTest : TestBase
	{
		public class Item : INotifyPropertyChanged
		{
			public OcConsumer Consumer = new OcConsumer();
			public Item(int num, int num2, OcDispatcher mainOcDispatcher, OcDispatcher backgroundOcDispatcher)
			{
				_num = num;
				_num2 = num2;
				_numBackgroundToMainDispatching = new PropertyDispatching<Item, int>(() => Num, mainOcDispatcher, backgroundOcDispatcher).For(Consumer);
				_num2MainToBackgroundDispatching = new PropertyDispatching<Item, int>(() => Num2, backgroundOcDispatcher, mainOcDispatcher).For(Consumer);
				_numBackgroundToMainScalarDispatching = new Computing<int>(() => Num).ScalarDispatching(mainOcDispatcher, backgroundOcDispatcher).For(Consumer);				
				_num2MainToBackgroundScalarDispatching = new Computing<int>(() => Num2).ScalarDispatching(backgroundOcDispatcher, mainOcDispatcher).For(Consumer);
				
				_numBackgroundToMainScalarDispatching.SetValueRequestHandler = i =>
				{
					backgroundOcDispatcher.Invoke(() => Num = i);
				};

				_num2MainToBackgroundDispatching.PropertyChanged += (sender, args) =>
				{
					if (Thread.CurrentThread != backgroundOcDispatcher._thread)
					{
						throw new Exception("Wrong thread");
					}
				};
				
				_num2MainToBackgroundScalarDispatching.PropertyChanged += (sender, args) =>
				{
					if (Thread.CurrentThread != backgroundOcDispatcher._thread)
					{
						throw new Exception("Wrong thread");
					}
				};

				_numBackgroundToMainDispatching.PropertyChanged += (sender, args) =>
				{
					if (Thread.CurrentThread != mainOcDispatcher._thread)
					{
						throw new Exception("Wrong thread");
					}
				};

				_numBackgroundToMainScalarDispatching.PropertyChanged += (sender, args) =>
				{
					if (Thread.CurrentThread != mainOcDispatcher._thread)
					{
						throw new Exception("Wrong thread");
					}
				};
			}

			private int _num;
			public int Num
			{
				get => _num;
				set => updatePropertyValue(ref _num, value);
			}

			private int _num2;
			public int Num2
			{
				get => _num2;
				set => updatePropertyValue(ref _num2, value);
			}

			private PropertyDispatching<Item,int> _numBackgroundToMainDispatching;
			public PropertyDispatching<Item, int> NumBackgroundToMainDispatching => _numBackgroundToMainDispatching;

			private PropertyDispatching<Item,int> _num2MainToBackgroundDispatching;
			public PropertyDispatching<Item, int> Num2MainToBackgroundDispatching => _num2MainToBackgroundDispatching;

			private ScalarDispatching<int> _numBackgroundToMainScalarDispatching;
			public ScalarDispatching<int> NumBackgroundToMainScalarDispatching => _numBackgroundToMainScalarDispatching;

			private ScalarDispatching<int> _num2MainToBackgroundScalarDispatching;
			public ScalarDispatching<int> Num2MainToBackgroundScalarDispatching => _num2MainToBackgroundScalarDispatching;


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
		[Repeat(5)]
		[Timeout(1000 * 60 * 60 * 5)]
		public void TestCollectionDispatchingTest()
		{
			OcDispatcher mainOcDispatcher = new OcDispatcher();
			mainOcDispatcher.ThreadName = "mainOcDispatcher";
			OcDispatcher backgroundOcDispatcher = new OcDispatcher();
			backgroundOcDispatcher.ThreadName = "backgroundOcDispatcher";
			OcConsumer consumer = new OcConsumer();

			ObservableCollection<Item> nums = new ObservableCollection<Item>();
			Filtering<Item> filteredNums = nums.Filtering(i =>
				i.Num % 3 == 0
				|| i.Num2MainToBackgroundDispatching.Value % 5 == 0
				|| i.Num2MainToBackgroundScalarDispatching.Value % 5 == 0);
			CollectionDispatching<Item> dispatchingfilteredNums = filteredNums
				.CollectionDispatching(mainOcDispatcher, backgroundOcDispatcher).For(consumer);

			dispatchingfilteredNums.CollectionChanged += (sender, args) =>
			{
				if (Thread.CurrentThread != mainOcDispatcher._thread)
				{
					throw new Exception("Wrong thread");
				}
			};

			((INotifyPropertyChanged)dispatchingfilteredNums).PropertyChanged += (sender, args) =>
			{
				if (Thread.CurrentThread != mainOcDispatcher._thread)
				{
					throw new Exception("Wrong thread");
				}
			};

			filteredNums.CollectionChanged += (sender, args) =>
			{

				if (Thread.CurrentThread != backgroundOcDispatcher._thread)
				{
					throw new Exception("Wrong thread");
				}
			};

			((INotifyPropertyChanged)filteredNums).PropertyChanged += (sender, args) =>
			{
				if (Thread.CurrentThread != backgroundOcDispatcher._thread)
				{
					throw new Exception("Wrong thread");
				}
			};

			bool stop = false;

			Random stopperRandom = new Random();
			Thread stopper = new Thread(() =>
			{
				Thread.Sleep(TimeSpan.FromSeconds(stopperRandom.Next(2, 20)));
				stop = true;
			});

			stopper.Start();

			ThreadStart numsChangerThreadStart = () =>
			{
				Random random = new Random();
				while (!stop)
				{
					Thread.Sleep(random.Next(0, 3));

					int nextAction = random.Next(0, 20);
					if (nextAction > 3) nextAction = nextAction == 4 ? 4 : 0;
					NotifyCollectionChangedAction action = (NotifyCollectionChangedAction)nextAction;
					switch (action)
					{
						case NotifyCollectionChangedAction.Add:
							backgroundOcDispatcher.Invoke(() =>
							{
								int upperIndex = nums.Count > 0 ? nums.Count - 1 : 0;
								int index = random.Next(0, upperIndex);
								nums.Insert(index,
									new Item(random.Next(Int32.MinValue, int.MaxValue),
										random.Next(Int32.MinValue, int.MaxValue), mainOcDispatcher,
										backgroundOcDispatcher));
							}, 0);
							break;
						case NotifyCollectionChangedAction.Remove:
							backgroundOcDispatcher.Invoke(() =>
							{
								int upperIndex = nums.Count - 1;
								if (upperIndex > 0)
								{
									int index = random.Next(0, upperIndex);
									Item item = nums[index];
									nums.RemoveAt(index);
									item.Consumer.Dispose();
								}
							}, 0);
							break;
						case NotifyCollectionChangedAction.Replace:
							backgroundOcDispatcher.Invoke(() =>
							{
								int upperIndex = nums.Count - 1;
								if (upperIndex > 0)
								{
									int index = random.Next(0, upperIndex);
									Item item = nums[index];
									nums[index] = new Item(random.Next(Int32.MinValue, int.MaxValue),
										random.Next(Int32.MinValue, int.MaxValue), mainOcDispatcher,
										backgroundOcDispatcher);
									item.Consumer.Dispose();
								}

							}, 0);
							break;
						case NotifyCollectionChangedAction.Move:
							backgroundOcDispatcher.Invoke(() =>
							{
								int upperIndex = nums.Count - 1;
								if (upperIndex > 0)
								{
									int indexFrom = random.Next(0, upperIndex);
									int indexTo = random.Next(0, upperIndex);
									nums.Move(indexFrom, indexTo);
								}
							}, 0);
							break;
						case NotifyCollectionChangedAction.Reset:
							backgroundOcDispatcher.Invoke(() =>
							{
								Item[] items = nums.ToArray();
								nums.Clear();
								foreach (Item item in items)
								{
									item.Consumer.Dispose();
								}
							}, 0);

							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

				}
			};


			int threadsCount = 10;
			Thread[] numsChangerThreads = new Thread[threadsCount];
			for (int i = 0; i < threadsCount; i++)
			{
				numsChangerThreads[i] = new Thread(numsChangerThreadStart);
				numsChangerThreads[i].Start();
			}

			ThreadStart numValueChangerThreadStart = () =>
			{
				Random random = new Random();
				while (!stop)
				{
					Thread.Sleep(random.Next(0, 3));

					mainOcDispatcher.Invoke(() =>
					{
						int dispatchingfilteredNumsCount = dispatchingfilteredNums.Count;
						if (dispatchingfilteredNumsCount > 0)
							dispatchingfilteredNums[random.Next(0, dispatchingfilteredNumsCount - 1)]
									.NumBackgroundToMainDispatching.Value =
								random.Next(Int32.MinValue, int.MaxValue);
					}, 0);

				}
			};

			Thread[] numValueChangerThreads = new Thread[threadsCount];
			for (int i = 0; i < threadsCount; i++)
			{
				numValueChangerThreads[i] = new Thread(numValueChangerThreadStart);
				numValueChangerThreads[i].Start();
			}

			ThreadStart numValueChanger2ThreadStart = () =>
			{
				Random random = new Random();
				while (!stop)
				{
					Thread.Sleep(random.Next(0, 3));

					mainOcDispatcher.Invoke(() =>
					{
						int dispatchingfilteredNumsCount = dispatchingfilteredNums.Count;
						if (dispatchingfilteredNumsCount > 0)
							dispatchingfilteredNums[random.Next(0, dispatchingfilteredNumsCount - 1)]
									.NumBackgroundToMainScalarDispatching.Value =
								random.Next(Int32.MinValue, int.MaxValue);
					});

				}
			};

			Thread[] numValueChanger2Threads = new Thread[threadsCount];
			for (int i = 0; i < threadsCount; i++)
			{
				numValueChanger2Threads[i] = new Thread(numValueChanger2ThreadStart);
				numValueChanger2Threads[i].Start();
			}


			ThreadStart num2ValueChangerThreadStart = () =>
			{
				Random random = new Random();
				while (!stop)
				{
					Thread.Sleep(random.Next(0, 3));

					mainOcDispatcher.Invoke(() =>
					{
						int dispatchingfilteredNumsCount = dispatchingfilteredNums.Count;
						if (dispatchingfilteredNumsCount > 0)
							dispatchingfilteredNums[random.Next(0, dispatchingfilteredNumsCount - 1)].Num2 =
								random.Next(Int32.MinValue, int.MaxValue);
					});

				}
			};

			Thread[] num2ValueChangerThreads = new Thread[threadsCount];
			for (int i = 0; i < threadsCount; i++)
			{
				num2ValueChangerThreads[i] = new Thread(num2ValueChangerThreadStart);
				num2ValueChangerThreads[i].Start();
			}

			for (int i = 0; i < threadsCount; i++)
			{
				numsChangerThreads[i].Join();
			}

			for (int i = 0; i < threadsCount; i++)
			{
				numValueChangerThreads[i].Join();
			}

			for (int i = 0; i < threadsCount; i++)
			{
				numValueChanger2Threads[i].Join();
			}

			for (int i = 0; i < threadsCount; i++)
			{
				num2ValueChangerThreads[i].Join();
			}

			//consuminingOcDispatcherInvoker.Join();

			mainOcDispatcher.Invoke(() => { });
			backgroundOcDispatcher.Invoke(() => { });
			mainOcDispatcher.Invoke(() => { });

			Assert.IsTrue(nums.Where(i => i.Num % 3 == 0 || i.Num2 % 5 == 0).SequenceEqual(dispatchingfilteredNums));
			Assert.IsTrue(nums.Where(i =>
				i.Num % 3 == 0
				|| i.NumBackgroundToMainDispatching.Value % 3 == 0
				|| i.NumBackgroundToMainScalarDispatching.Value % 3 == 0
				|| i.Num2 % 5 == 0
				|| i.Num2MainToBackgroundDispatching.Value % 5 == 0
				|| i.Num2MainToBackgroundScalarDispatching.Value % 5 == 0).SequenceEqual(dispatchingfilteredNums));

			foreach (Item item in nums)
				item.Consumer.Dispose();

			mainOcDispatcher.Invoke(() => consumer.Dispose());
			
			ManualResetEventSlim backgroundOcDispatcherDisposedMru = new ManualResetEventSlim(false);
			ManualResetEventSlim mainOcDispatcherDisposedMru = new ManualResetEventSlim(false);

			backgroundOcDispatcher.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(OcDispatcher.Status) 
					&& backgroundOcDispatcher.Status == OcDispatcherStatus.Disposed)
					backgroundOcDispatcherDisposedMru.Set();
			};

			mainOcDispatcher.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(OcDispatcher.Status) 
					&& mainOcDispatcher.Status == OcDispatcherStatus.Disposed)
					mainOcDispatcherDisposedMru.Set();
			};


			backgroundOcDispatcher.Dispose();
			mainOcDispatcher.Dispose();

			backgroundOcDispatcherDisposedMru.Wait(30000);
			mainOcDispatcherDisposedMru.Wait(30000);

			if (backgroundOcDispatcher.Status != OcDispatcherStatus.Disposed 
				|| mainOcDispatcher.Status != OcDispatcherStatus.Disposed)
			{
				backgroundOcDispatcherDisposedMru.Dispose();
				mainOcDispatcherDisposedMru.Dispose();
				throw new Exception("dispose failed");
			}

			backgroundOcDispatcherDisposedMru.Dispose();
			mainOcDispatcherDisposedMru.Dispose();
		}

		public CollectionDispatchingTest(bool debug) : base(debug)
		{
		}
	}
}
