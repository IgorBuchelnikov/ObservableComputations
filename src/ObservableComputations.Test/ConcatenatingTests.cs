// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class ConcatenatingTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item : INotifyPropertyChanged
		{

			public Item()
			{
				Num = LastNum;
				LastNum++;
			}

			public static int LastNum;
			public int Num;

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

			public override string ToString()
			{
				return $"Num={Num}";
			}
		}

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\Projects\NevaPolimer\Concatenating_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\Projects\NevaPolimer\Concatenating_Deep_Time.log");

#if !RunOnlyMinimalTestsToCover
		[Test, Combinatorial]
		public void Concating_Deep()
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();

			//test(new int[0]);

			int from = -1;
			int to = 4;


			for (int v1 = from; v1 <= to; v1++)
			{
				test(new[] { v1 });
				for (int v2 = from; v2 <= to; v2++)
				{
					test(new[] { v1, v2 });
					for (int v3 = from; v3 <= to; v3++)
					{
						test(new[] { v1, v2, v3 });
						for (int v4 = from; v4 <= to; v4++)
						{
							test(new[] { v1, v2, v3, v4 });
							counter++;
							if (counter % 100 == 0)
							{
								_textFileOutputTime.AppentLine($"{stopwatch.Elapsed.TotalMinutes}: {counter}");
							}
						}
					}
				}
			}
		}
#endif

		private void test(int[] itemsCounts)
		{
			string testNum = string.Empty;
			int index = 0;
			int itemsCount = 0;
			int indexOld = 0;
			int indexNew = 0;
			int index1 = 0;
			ObservableCollection<ObservableCollection<Item>> items;
			Concatenating<Item> concatenating;
			try
			{
				trace(testNum = "1", itemsCounts, index, itemsCount, indexOld, indexNew);
				items = getObservableCollections(itemsCounts);
				concatenating = items.Concatenating().For(consumer);
				concatenating.ValidateInternalConsistency();				
				consumer.Dispose();

				for (index = 0; index < itemsCounts.Length; index++)
				{
					trace(testNum = "2", itemsCounts, index, itemsCount, indexOld, indexNew);
					items = getObservableCollections(itemsCounts);
					Concatenating<Item> concating1 = items.Concatenating().For(consumer);
					items.RemoveAt(index);
					concating1.ValidateInternalConsistency();					
					consumer.Dispose();
				}

				for (index = 0; index <= itemsCounts.Length; index++)
				{
					for (itemsCount = -1; itemsCount <= itemsCounts.Length; itemsCount++)
					{
						trace(testNum = "11", itemsCounts, index, itemsCount, indexOld, indexNew);
						items = getObservableCollections(itemsCounts);
						Concatenating<Item> concating2 = items.Concatenating().For(consumer);
						items.Insert(index, getObservableCollection(itemsCount));
						concating2.ValidateInternalConsistency();						
						consumer.Dispose();
					}
				}

				for (index = 0; index < itemsCounts.Length; index++)
				{
					trace(testNum = "6", itemsCounts, index, itemsCount, indexOld, indexNew);
					items = getObservableCollections(itemsCounts);
					Concatenating<Item> concating3 = items.Concatenating().For(consumer);
					items[index] = new ObservableCollection<Item>();
					concating3.ValidateInternalConsistency();					
					consumer.Dispose();

					for (itemsCount = -1; itemsCount <= itemsCounts.Length; itemsCount++)
					{
						trace(testNum = "3", itemsCounts, index, itemsCount, indexOld, indexNew);
						items = getObservableCollections(itemsCounts);
						Concatenating<Item> concating2 = items.Concatenating().For(consumer);
						items[index] = getObservableCollection(itemsCount);
						concating2.ValidateInternalConsistency();						
						consumer.Dispose();

					}
				}

				for (index = 0; index < itemsCounts.Length; index++)
				{
					trace(testNum = "4", itemsCounts, index, itemsCount, indexOld, indexNew);
					items = getObservableCollections(itemsCounts);
					Concatenating<Item> concating1 = items.Concatenating().For(consumer);
					items[index] = null;
					concating1.ValidateInternalConsistency();					
					consumer.Dispose();

					for (itemsCount = -1; itemsCount <= itemsCounts.Length; itemsCount++)
					{
						trace(testNum = "5", itemsCounts, index, itemsCount, indexOld, indexNew);
						items = getObservableCollections(itemsCounts);
						Concatenating<Item> concating2 = items.Concatenating().For(consumer);
						items[index] = getObservableCollection(itemsCount);
						concating2.ValidateInternalConsistency();						
						consumer.Dispose();

					}
				}

				for (indexOld = 0; indexOld < itemsCounts.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < itemsCounts.Length; indexNew++)
					{
						trace(testNum = "6", itemsCounts, index, itemsCount, indexOld, indexNew);
						items = getObservableCollections(itemsCounts);
						Concatenating<Item> concating2 = items.Concatenating().For(consumer);
						items.Move(indexOld, indexNew);
						concating2.ValidateInternalConsistency();						
						consumer.Dispose();
					}
				}



				for (index1 = 0; index1 < itemsCounts.Length; index1++)
				{
					int itemsCount1 = itemsCounts[index1];
					for (index = 0; index < itemsCount1; index++)
					{
						trace(testNum = "7", itemsCounts, index, itemsCount, indexOld, indexNew);
						items = getObservableCollections(itemsCounts);
						Concatenating<Item> concating1 = items.Concatenating().For(consumer);
						items[index1].RemoveAt(index);
						concating1.ValidateInternalConsistency();						
						consumer.Dispose();
					}

					for (index = 0; index <= itemsCount1; index++)
					{
						trace(testNum = "12", itemsCounts, index, itemsCount, indexOld, indexNew);
						items = getObservableCollections(itemsCounts);
						Concatenating<Item> concating1 = items.Concatenating().For(consumer);
						items[index1].Insert(index, new Item());
						concating1.ValidateInternalConsistency();						
						consumer.Dispose();
					}

					for (index = 0; index < itemsCount1; index++)
					{
						trace(testNum = "4", itemsCounts, index, itemsCount, indexOld, indexNew);
						items = getObservableCollections(itemsCounts);
						Concatenating<Item> concating3 = items.Concatenating().For(consumer);
						items[index1][index] = null;
						concating3.ValidateInternalConsistency();						
						consumer.Dispose();

						trace(testNum = "9", itemsCounts, index, itemsCount, indexOld, indexNew);
						items = getObservableCollections(itemsCounts);
						Concatenating<Item> concating2 = items.Concatenating().For(consumer);
						items[index1][index] = new Item();
						concating2.ValidateInternalConsistency();						
						consumer.Dispose();
					}

					for (indexOld = 0; indexOld < itemsCount1; indexOld++)
					{
						for (indexNew = 0; indexNew < itemsCount1; indexNew++)
						{
							trace(testNum = "10", itemsCounts, index, itemsCount, indexOld, indexNew);
							items = getObservableCollections(itemsCounts);
							Concatenating<Item> concating2 = items.Concatenating().For(consumer);
							items[index1].Move(indexOld, indexNew);
							concating2.ValidateInternalConsistency();							
							consumer.Dispose();
						}
					}
				}
			}
			catch (Exception e)
			{
				string traceString = getTraceString( 
					testNum,
					itemsCounts,
					index,
					itemsCount, 
					indexOld,
					indexNew, 
					index1);

				_textFileOutputLog.AppentLine(traceString);
				_textFileOutputLog.AppentLine(e.Message);
				_textFileOutputLog.AppentLine(e.StackTrace);

				throw new Exception(traceString, e);
			}

			writeUsefulTest(getTestString(itemsCounts));
		}

		private void trace(string num, int[] itemsCounts, int index, int itemsCount, int indexOld,
			int indexNew, int index1 = 0)
		{
			string traceString = getTraceString(num, itemsCounts, index, itemsCount, indexOld, indexNew, index1);
			if (traceString == "#3. ItemsCounts=-1,-1,-1,1  index=3  itemsCount=-1   indexOld=0   indexNew=0, index1=0")
			{
				
			}
		}

		private static string getTraceString(string num, int[] itemsCounts, int index, int itemsCount, int indexOld, int indexNew, int index1 = 0)
		{
			return string.Format(
				"#{6}. ItemsCounts={0}  index={1}  itemsCount={2}   indexOld={3}   indexNew={4}, index1={5}",
				string.Join(",", itemsCounts),
				index,
				itemsCount,
				indexOld,
				indexNew,
				index1,
				num);
		}


		private static ObservableCollection<ObservableCollection<Item>> getObservableCollections(int[] itemsCounts)
		{
			return new ObservableCollection<ObservableCollection<Item>>(itemsCounts.Select(itemsCount => getObservableCollection(itemsCount)));
		}

		private static ObservableCollection<Item> getObservableCollection(int itemsCount)
		{
			return itemsCount >= 0 
				? new ObservableCollection<Item>(Enumerable.Range(1, itemsCount).Select(i => new Item()))
				: null;
		}

		[Test]
		public void TestConcatenatingQueuedItemCollectionChange()
		{
			ObservableCollection<ObservableCollection<Item>> items = new ObservableCollection<ObservableCollection<Item>>(
				new[]
				{
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					),
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Concatenating<Item> concatenating = items.Concatenating().For(consumer);
			bool handled = false;

			concatenating.CollectionChanged += (sender, args) =>
			{
				if (!handled)
				{
					items[1].Add(new Item());
					handled = true;
				}
			};

			items[0].Add(new Item());

			concatenating.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test]
		public void TestConcatenatingQueuedItemCollectionReset()
		{
			ObservableCollection<ObservableCollection<Item>> items = new ObservableCollection<ObservableCollection<Item>>(
				new[]
				{
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					),
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Concatenating<Item> concatenating = items.Concatenating().For(consumer);
			bool handled = false;

			concatenating.CollectionChanged += (sender, args) =>
			{
				if (!handled)
				{
					items[1].Clear();
					handled = true;
				}
			};

			items[0].Add(new Item());

			concatenating.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test]
		public void TestConcatenating1()
		{
			ObservableCollection<object> items = new ObservableCollection<object>(
				new object[]
				{
					new Scalar<ObservableCollection<Item>>(
						new ObservableCollection<Item>(
							new[]
							{
								new Item(),
								new Item(),
								new Item(),
								new Item()
							})
					), 
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Concatenating<Item> concatenating = items.Concatenating<Item>().For(consumer);

			items.RemoveAt(0);

			concatenating.ValidateInternalConsistency();
		}

		[Test]
		public void TestConcatenating2()
		{
			ObservableCollection<ObservableCollection<Item>> items = new ObservableCollection<ObservableCollection<Item>>(
				new[]
				{
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					),
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					)
				}
			);

			OcConsumer consumer = new OcConsumer();
			Concatenating<Item> concatenating1 = items.Concatenating();
			Selecting<Item, Item> concatenating = concatenating1.Selecting(i => i).For(consumer);
			concatenating1.For(consumer);

			concatenating.ValidateInternalConsistency();
			consumer.Dispose();
		}

		[Test]
		public void TestConcatenating3()
		{
			OcConsumer consumer = new OcConsumer();

			ObservableCollection<ObservableCollection<Item>> items = new ObservableCollection<ObservableCollection<Item>>(
				new[]
				{
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					),
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					).Selecting(i => i).For(consumer)
				}
			);


			Concatenating<Item> concatenating1 = items.Concatenating();
			Selecting<Item, Item> concatenating = concatenating1.Selecting(i => i).For(consumer);
			concatenating1.For(consumer);

			concatenating.ValidateInternalConsistency();
			consumer.Dispose();
		}


		[Test]
		public void TestConcatenating4()
		{
			OcConsumer consumer = new OcConsumer();

			ObservableCollection<ObservableCollection<Item>> items = new ObservableCollection<ObservableCollection<Item>>(
				new[]
				{
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					),
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					).Selecting(i => i).For(consumer)
				}
			);


			Concatenating<Item> concatenating1 = items.Concatenating();
			Selecting<Item, Item> concatenating = concatenating1.Selecting(i => i).SetDebugTag("1").For(consumer);
			Selecting<Item, Item> concatenating2 = concatenating1.Selecting(i => i).SetDebugTag("2").For(consumer);
			concatenating1.For(consumer);
			consumer.Dispose();

			Assert.AreEqual(concatenating1.IsActive, false);
		}

		[Test]
		public void TestConcatenating5()
		{
			OcConsumer consumer = new OcConsumer();

			ObservableCollection<ObservableCollection<Item>> items = new ObservableCollection<ObservableCollection<Item>>(
				new[]
				{
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					),
					new ObservableCollection<Item>(
						new[]
						{
							new Item(),
							new Item(),
							new Item(),
							new Item()
						}
					).Selecting(i => i)
				}
			);


			Concatenating<Item> concatenating1 = items.Concatenating().For(consumer);
			Assert.AreEqual(((CollectionComputing<Item>) items[1]).IsActive, true);
			concatenating1.ValidateInternalConsistency();

			consumer.Dispose();

			Assert.AreEqual(((CollectionComputing<Item>) items[1]).IsActive, false);
		}

		public ConcatenatingTests(bool debug) : base(debug)
		{
		}
	}
}
