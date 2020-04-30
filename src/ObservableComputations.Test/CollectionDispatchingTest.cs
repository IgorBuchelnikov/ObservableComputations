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
	[TestFixture]
	public class CollectionDispatchingTest
	{
		[Test]
		public void TestCollectionDispatchingTest()
		{
			Dispatcher consuminingDispatcher = new Dispatcher();
			Dispatcher computingDispatcher = new Dispatcher();
			var nums = new ObservableCollection<int>();
			var filteredNums = nums.Filtering(n => n % 3 == 0);
			var dispatchingfilteredNums = filteredNums.CollectionDispatching(
				computingDispatcher,
				consuminingDispatcher);
			bool stop = false;
			Random computingWorkerRandom =  new Random();

			ThreadStart numsChangerThreadStart = () =>
			{
				Random random =  new Random();
				while (!stop)
				{
					Thread.Sleep(random.Next(0, 100));

					NotifyCollectionChangedAction action = (NotifyCollectionChangedAction) random.Next(0, 4);
					switch (action)
					{
						case NotifyCollectionChangedAction.Add:
							computingDispatcher.Invoke(() =>
							{
								int upperIndex = nums.Count > 0 ? nums.Count - 1 : 0;
								int index = computingWorkerRandom.Next(0, upperIndex);
								nums.Insert(index, computingWorkerRandom.Next(Int32.MinValue, int.MaxValue));
							});
							break;
						case NotifyCollectionChangedAction.Remove:
							computingDispatcher.Invoke(() =>
							{
								int upperIndex =  nums.Count - 1;
								if (upperIndex > 0)
								{
									int index = computingWorkerRandom.Next(0, upperIndex);
									nums.RemoveAt(index);
								}
							});
							break;
						case NotifyCollectionChangedAction.Replace:
							computingDispatcher.Invoke(() =>
							{
								int upperIndex =  nums.Count - 1;
								if (upperIndex > 0)
								{
									int index = computingWorkerRandom.Next(0, upperIndex);
									nums[index] = computingWorkerRandom.Next(Int32.MinValue, int.MaxValue);
								}

							});
							break;
						case NotifyCollectionChangedAction.Move:
							computingDispatcher.Invoke(() =>
							{
								int upperIndex =  nums.Count - 1;
								if (upperIndex > 0)
								{
									int indexFrom = computingWorkerRandom.Next(0, upperIndex);
									int indexTo = computingWorkerRandom.Next(0, upperIndex);
									nums.Move(indexFrom, indexTo);
								}
							});
							break;
						case NotifyCollectionChangedAction.Reset:
							computingDispatcher.Invoke(() => { nums.Clear(); });
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

				}
			};

			Thread numsChangerThread1 = new Thread(numsChangerThreadStart);
			Thread numsChangerThread2 = new Thread(numsChangerThreadStart);
			Thread numsChangerThread3 = new Thread(numsChangerThreadStart);
			Thread numsChangerThread4 = new Thread(numsChangerThreadStart);
			numsChangerThread1.Start();
			numsChangerThread2.Start();
			numsChangerThread3.Start();
			numsChangerThread4.Start();

			Thread numsChangersStopper = new Thread(() =>
			{
				Thread.Sleep(TimeSpan.FromMinutes(60 * 12));
				stop = true;
			});

			numsChangersStopper.Start();

			Thread consuminingWorkerInvoker = new Thread(() =>
			{
				Random random =  new Random();
				while (!stop)
				{
					Thread.Sleep(random.Next(0, 1000));
					consuminingDispatcher.Invoke(() =>
					{
						computingDispatcher.Invoke(() =>
						{
							Assert.IsTrue(nums.Where(n => n % 3 == 0).SequenceEqual(dispatchingfilteredNums));
							Debug.Print("!!!!!");

						});
					});
				}
			});
			consuminingWorkerInvoker.Start();


			numsChangerThread1.Join();
			numsChangerThread2.Join();
			numsChangerThread3.Join();
			numsChangerThread4.Join();
			//consuminingWorkerInvoker.Join();


		}
	}
}