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
		public class Item : INotifyPropertyChanged
		{
            public Consumer Consumer = new Consumer();
			public Item(int num, int num2, OcDispatcher consuminingOcDispatcher, OcDispatcher computingOcDispatcher)
			{
				_num = num;
				_num2 = num2;
				_numCompToConsDispatching = new PropertyDispatching<Item, int>(() => Num, consuminingOcDispatcher, computingOcDispatcher).For(Consumer);
				_num2ConsToCompDispatching = new PropertyDispatching<Item, int>(() => Num2, computingOcDispatcher, consuminingOcDispatcher).For(Consumer);
				_numCompToConsScalarDispatching = new Computing<int>(() => Num).ScalarDispatching(consuminingOcDispatcher, computingOcDispatcher).For(Consumer);				
                _num2ConsToCompScalarDispatching = new Computing<int>(() => Num2).ScalarDispatching(computingOcDispatcher, consuminingOcDispatcher).For(Consumer);
                
                
                _num2ConsToCompDispatching.PropertyChanged += (sender, args) =>
                {
                    if (Thread.CurrentThread != computingOcDispatcher._thread)
                    {
                        throw new Exception("Wrong thread");
                    }
                };
                
                _num2ConsToCompScalarDispatching.PropertyChanged += (sender, args) =>
                {
                    if (Thread.CurrentThread != computingOcDispatcher._thread)
                    {
                        throw new Exception("Wrong thread");
                    }
                };

                _numCompToConsDispatching.PropertyChanged += (sender, args) =>
                {
                    if (Thread.CurrentThread != consuminingOcDispatcher._thread)
                    {
                        throw new Exception("Wrong thread");
                    }
                };

                _numCompToConsScalarDispatching.PropertyChanged += (sender, args) =>
                {
                    if (Thread.CurrentThread != consuminingOcDispatcher._thread)
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

			private PropertyDispatching<Item,int> _numCompToConsDispatching;
			public PropertyDispatching<Item, int> NumCompToConsDispatching => _numCompToConsDispatching;

			private PropertyDispatching<Item,int> _num2ConsToCompDispatching;
			public PropertyDispatching<Item, int> Num2ConsToCompDispatching => _num2ConsToCompDispatching;

            private ScalarDispatching<int> _numCompToConsScalarDispatching;
            public ScalarDispatching<int> NumCompToConsScalarDispatching => _numCompToConsScalarDispatching;

            private ScalarDispatching<int> _num2ConsToCompScalarDispatching;
            public ScalarDispatching<int> Num2ConsToCompScalarDispatching => _num2ConsToCompScalarDispatching;


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
		public void TestCollectionDispatchingTest()
		{
			for (int j = 0; j < 1000000; j++)
            {
                OcDispatcher consuminingOcDispatcher = new OcDispatcher();
                consuminingOcDispatcher.ThreadName = "coNSuminingOcDispatcher";
				OcDispatcher computingOcDispatcher = new OcDispatcher();
                computingOcDispatcher.ThreadName = "coMPutingOcDispatcher";
                Consumer consumer = new Consumer();

				var nums = new ObservableCollection<Item>();
				var filteredNums = nums.Filtering(i => 
                    i.Num % 3 == 0 
                    || i.Num2ConsToCompDispatching.Value % 5 == 0
                    || i.Num2ConsToCompScalarDispatching.Value % 5 == 0);
				var dispatchingfilteredNums = filteredNums.CollectionDispatching(consuminingOcDispatcher, computingOcDispatcher).For(consumer);

                dispatchingfilteredNums.CollectionChanged += (sender, args) =>
                {
                    
                    if (Thread.CurrentThread != consuminingOcDispatcher._thread)
                    {
                        throw new Exception("Wrong thread");
                    }
                };

                ((INotifyPropertyChanged) dispatchingfilteredNums).PropertyChanged += (sender, args) =>
                {
                    if (Thread.CurrentThread != consuminingOcDispatcher._thread)
                    {
                        throw new Exception("Wrong thread");
                    }
                };

                filteredNums.CollectionChanged += (sender, args) =>
                {
                    
                    if (Thread.CurrentThread != computingOcDispatcher._thread)
                    {
                        throw new Exception("Wrong thread");
                    }
                };

                ((INotifyPropertyChanged) filteredNums).PropertyChanged += (sender, args) =>
                {
                    if (Thread.CurrentThread != computingOcDispatcher._thread)
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
					Random random =  new Random();
					while (!stop)
					{
						Thread.Sleep(random.Next(0, 3));

						int nextAction = random.Next(0, 20);
						if (nextAction > 3) nextAction = nextAction == 4 ? 4 : 0;
						NotifyCollectionChangedAction action = (NotifyCollectionChangedAction) nextAction;
						switch (action)
						{
							case NotifyCollectionChangedAction.Add:
                                computingOcDispatcher.Invoke(() =>
                                {
                                    int upperIndex = nums.Count > 0 ? nums.Count - 1 : 0;
                                    int index = random.Next(0, upperIndex);
                                    nums.Insert(index, new Item(random.Next(Int32.MinValue, int.MaxValue), random.Next(Int32.MinValue, int.MaxValue), consuminingOcDispatcher, computingOcDispatcher));
                                }, 0);
                                break;
							case NotifyCollectionChangedAction.Remove:
                                computingOcDispatcher.Invoke(() =>
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
                                computingOcDispatcher.Invoke(() =>
                                {
                                    int upperIndex = nums.Count - 1;
                                    if (upperIndex > 0)
                                    {
                                        int index = random.Next(0, upperIndex);
                                        Item item = nums[index];
                                        nums[index] = new Item(random.Next(Int32.MinValue, int.MaxValue), random.Next(Int32.MinValue, int.MaxValue), consuminingOcDispatcher, computingOcDispatcher);
                                        item.Consumer.Dispose();
                                    }

                                }, 0);
                                break;
							case NotifyCollectionChangedAction.Move:
                                computingOcDispatcher.Invoke(() =>
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
                                computingOcDispatcher.Invoke(() =>
                                {
                                    Item[] items = nums.ToArray();
                                    nums.Clear();
                                    foreach (Item item in items)
                                        item.Consumer.Dispose();
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

                        consuminingOcDispatcher.Invoke(() =>
                        {
                            int dispatchingfilteredNumsCount = dispatchingfilteredNums.Count;
                            if (dispatchingfilteredNumsCount > 0)
                                dispatchingfilteredNums[random.Next(0, dispatchingfilteredNumsCount - 1)].NumCompToConsDispatching.Value =
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

				ThreadStart num2ValueChangerThreadStart = () =>
				{
                    Random random = new Random();
                    while (!stop)
                    {
                        Thread.Sleep(random.Next(0, 3));

                        consuminingOcDispatcher.Invoke(() =>
                        {
                            int dispatchingfilteredNumsCount = dispatchingfilteredNums.Count;
                            if (dispatchingfilteredNumsCount > 0)
                                dispatchingfilteredNums[random.Next(0, dispatchingfilteredNumsCount - 1)].Num2 =
                                    random.Next(Int32.MinValue, int.MaxValue);
                        }, 0);

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
					num2ValueChangerThreads[i].Join();
				}

				//consuminingOcDispatcherInvoker.Join();

				consuminingOcDispatcher.Invoke(() => {}, 0);
				computingOcDispatcher.Invoke(() => {}, 0);
				consuminingOcDispatcher.Invoke(() => {}, 0);

                Assert.IsTrue(nums.Where(i => i.Num % 3 == 0 || i.Num2 % 5 == 0).SequenceEqual(dispatchingfilteredNums));
                Assert.IsTrue(nums.Where(i => 
                    i.NumCompToConsDispatching.Value % 3 == 0 
                    || i.Num2ConsToCompDispatching.Value % 5 == 0
                    || i.Num2ConsToCompScalarDispatching.Value % 5 == 0).SequenceEqual(dispatchingfilteredNums));

                foreach (Item item in nums)
                    item.Consumer.Dispose();

                consuminingOcDispatcher.Invoke(() =>
                {
                    consumer.Dispose();
                }, 0);

                
                Debug.Print("!!!!!");
			}
		}
	}
}