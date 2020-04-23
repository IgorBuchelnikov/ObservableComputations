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
			Worker consuminingWorker = new Worker();
			Worker computingWorker = new Worker();
			var nums = new ObservableCollection<int>();
			var filteredNums = nums.Filtering(n => n % 3 == 0);
			var dispatchingfilteredNums = filteredNums.CollectionDispatching(
				computingWorker.Dispatcher,
				consuminingWorker.Dispatcher);
			bool stop = false;
			Random computingWorkerRandom =  new Random();

			ThreadStart numsChangerThreadStart = () =>
			{
				Random numsChangerRandom =  new Random();
				while (!stop)
				{
					Thread.Sleep(numsChangerRandom.Next(0, 100));

					NotifyCollectionChangedAction action = (NotifyCollectionChangedAction) numsChangerRandom.Next(0, 4);
					switch (action)
					{
						case NotifyCollectionChangedAction.Add:
							computingWorker.Dispatcher.BeginInvoke(() =>
							{
								int upperIndex = nums.Count > 0 ? nums.Count - 1 : 0;
								int index = computingWorkerRandom.Next(0, upperIndex);
								nums.Insert(index, computingWorkerRandom.Next(Int32.MinValue, int.MaxValue));
							}).WaitOneAndDispose();
							break;
						case NotifyCollectionChangedAction.Remove:
							computingWorker.Dispatcher.BeginInvoke(() =>
							{
								int upperIndex =  nums.Count - 1;
								if (upperIndex > 0)
								{
									int index = computingWorkerRandom.Next(0, upperIndex);
									nums.RemoveAt(index);
								}
							}).WaitOneAndDispose();
							break;
						case NotifyCollectionChangedAction.Replace:
							computingWorker.Dispatcher.BeginInvoke(() =>
							{
								int upperIndex =  nums.Count - 1;
								if (upperIndex > 0)
								{
									int index = computingWorkerRandom.Next(0, upperIndex);
									nums[index] = computingWorkerRandom.Next(Int32.MinValue, int.MaxValue);
								}

							}).WaitOneAndDispose();
							break;
						case NotifyCollectionChangedAction.Move:
							computingWorker.Dispatcher.BeginInvoke(() =>
							{
								int upperIndex =  nums.Count - 1;
								if (upperIndex > 0)
								{
									int indexFrom = computingWorkerRandom.Next(0, upperIndex);
									int indexTo = computingWorkerRandom.Next(0, upperIndex);
									nums.Move(indexFrom, indexTo);
								}
							}).WaitOneAndDispose();
							break;
						case NotifyCollectionChangedAction.Reset:
							computingWorker.Dispatcher.BeginInvoke(() => { nums.Clear(); }).WaitOneAndDispose();
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
				Thread.Sleep(TimeSpan.FromMinutes(20));
				stop = true;
			});

			numsChangersStopper.Start();

			Thread consuminingWorkerInvoker = new Thread(() =>
			{
				while (!stop)
				{
					Thread.Sleep(TimeSpan.FromMilliseconds(1000));
					consuminingWorker.Dispatcher.BeginInvoke(() =>
					{
						computingWorker.Dispatcher.BeginInvoke(() =>
						{
							Assert.IsTrue(nums.Where(n => n % 3 == 0).SequenceEqual(dispatchingfilteredNums));
							Debug.Print("!!!!!");

						});
					}).WaitOneAndDispose();
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