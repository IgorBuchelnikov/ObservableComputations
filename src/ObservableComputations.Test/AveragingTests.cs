// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class AveragingTests : TestBase
	{
		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\AverageComputing_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\AverageComputing_Deep_Time.log");

#if !RunOnlyMinimalTestsToCover
		[Test]
		public void AverageComputing_Deep()
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
					
			test(new int[0]);

			int from = -2;
			int to = 2;

			for (int v1 = from; v1 <= to; v1++)
			{
				test(new []{v1});
				for (int v2 = from; v2 <= to; v2++)
				{
					test(new []{v1, v2});
					for (int v3 = from; v3 <= to; v3++)
					{
						test(new []{v1, v2, v3});

						for (int v4 = from; v4 <= to; v4++)
						{
							test(new []{v1, v2, v3, v4});
							for (int v5 = from; v5 <= to; v5++)
							{
								test(new[] {v1, v2, v3, v4, v5});
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
		}
#endif

		OcConsumer consumer = new OcConsumer();

		private void test(int[] values)
		{
			string testNum = string.Empty;
			int index = 0;
			int value = 0;
			int indexOld = 0;
			int indexNew = 0;

			ObservableCollection<int> items;
			Averaging<int, double> averaging;

			try
			{
				trace(testNum = "1", values, index, value, indexOld, indexNew);
				items = getObservableCollection(values);

				averaging = items.Averaging<int, double>().For(consumer);
				//averaging.ValidateConsistency();
				validate(averaging, items);

				for (index = 0; index < values.Length; index++)
				{
					trace(testNum = "2", values, index, value, indexOld, indexNew);
					items = getObservableCollection(values);
					Averaging<int, double> averageComputing1 = items.Averaging<int, double>().For(consumer);
					items.RemoveAt(index);
					//averageComputing1.ValidateConsistency();
					validate(averageComputing1, items);
					consumer.Dispose();
				}

				for (index = 0; index <= values.Length; index++)
				{
					for (value = 0; value <= values.Length; value++)
					{
						trace(testNum = "8", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						Averaging<int, double> averageComputing1 = items.Averaging<int, double>().For(consumer);
						items.Insert(index, value);
						//averageComputing1.ValidateConsistency();
						validate(averageComputing1, items);
						consumer.Dispose();
					}
				}

				for (index = 0; index < values.Length; index++)
				{

					for (value = -1; value <= values.Length; value++)
					{
						trace(testNum = "3", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						Averaging<int, double> averageComputing2 = items.Averaging<int, double>().For(consumer);
						items[index] = value;
						//averageComputing2.ValidateConsistency();
						validate(averageComputing2, items);
						consumer.Dispose();

					}
				}

				for (indexOld = 0; indexOld < values.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < values.Length; indexNew++)
					{
						trace(testNum = "7", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						Averaging<int, double> averageComputing2 = items.Averaging<int, double>().For(consumer);
						items.Move(indexOld, indexNew);
						//averageComputing2.ValidateConsistency();
						validate(averageComputing2, items);
						consumer.Dispose();
					}
				}

				writeUsefulTest(getTestString(values));
			}
			catch (Exception e)
			{
				string traceString = getTraceString(testNum, values, index, value, indexOld, indexNew);
				_textFileOutputLog.AppentLine(traceString);
				_textFileOutputLog.AppentLine(e.Message);
				_textFileOutputLog.AppentLine(e.StackTrace);
				throw new Exception(traceString, e);
			}

		}

		private void validate(Averaging<int, double> averaging, ObservableCollection<int> items)
		{
			Assert.AreEqual(averaging.Value, items.Count > 0 ? items.Average() : double.NaN);
			consumer.Dispose();
		}

		private void trace(string testNum, int[] values, int index, int value, int indexOld, int indexNew)
		{
			string traceString = getTraceString(testNum, values, index, value, indexOld, indexNew);
			
			if (traceString == "#2. OrderNums=-2  index=0  value=0   indexOld=0   indexNew=0")
			{
				//Debugger.Break();
			}
		}

		private string getTraceString(string testNum, int[] values, int index, int value, int indexOld, int indexNew)
		{
			return string.Format(
				"#{5}. OrderNums={0}  index={1}  value={2}   indexOld={3}   indexNew={4}",
				string.Join(",", values),
				index,
				value,
				indexOld,
				indexNew,
				testNum);
		}

		private static ObservableCollection<int> getObservableCollection(int[] values)
		{
			return new ObservableCollection<int>(values);
		}


		//[Test, Combinatorial]
		//public void AverageComputing_Change(
		//	[Range(-3, 2, 1)] int item1,
		//	[Range(-3, 2, 1)] int item2,
		//	[Range(-3, 2, 1)] int item3,
		//	[Range(-3, 2, 1)] int item4,
		//	[Range(-3, 2, 1)] int item5,
		//	[Range(0, 4, 1)] int index,
		//	[Range(-1, 5)] int newValue)
		//{
		//	ObservableCollection<int> items = new ObservableCollection<int>();
		//	if (item1 >= -2)
		//		items.Add(item1);
		//	if (item2 >= -2)
		//		items.Add(item2);
		//	if (item3 >= -2)
		//		items.Add(item3);
		//	if (item4 >= -2)
		//		items.Add(item4);
		//	if (item5 >= -2)
		//		items.Add(item5);

		//	if (index >= items.Count)
		//		return;

		//	Aggregating<int, int> averaging = items.Averaging();
		//	averaging.ValidateConsistency();
		//	Assert.Equals(averaging.Value, items.Sum());

		//	items[index] = newValue;

		//	averaging.ValidateConsistency();
		//	Assert.Equals(averaging.Value, items.Sum());

		//}

		//[Test, Combinatorial]
		//public void AverageComputing_Remove(
		//	[Range(-3, 2, 1)] int item1,
		//	[Range(-3, 2, 1)] int item2,
		//	[Range(-3, 2, 1)] int item3,
		//	[Range(-3, 2, 1)] int item4,
		//	[Range(-3, 2, 1)] int item5,
		//	[Range(0, 4, 1)] int index)
		//{
		//	ObservableCollection<int> items = new ObservableCollection<int>();
		//	if (item1 >= -2) items.Add(item1);
		//	if (item2 >= -2) items.Add(item2);
		//	if (item3 >= -2) items.Add(item3);
		//	if (item4 >= -2) items.Add(item4);
		//	if (item5 >= -2) items.Add(item5);

		//	if (index >= items.Count) return;

		//	Aggregating<int, int> averaging = items.Averaging();
		//	averaging.ValidateConsistency();
		//	Assert.Equals(averaging.Value, items.Sum());

		//	items.RemoveAt(index);

		//	averaging.ValidateConsistency();
		//	Assert.Equals(averaging.Value, items.Sum());
		//}

		//[Test, Combinatorial]
		//public void AverageComputing_Insert(
		//	[Range(-3, 2, 1)] int item1,
		//	[Range(-3, 2, 1)] int item2,
		//	[Range(-3, 2, 1)] int item3,
		//	[Range(-3, 2, 1)] int item4,
		//	[Range(-3, 2, 1)] int item5,
		//	[Range(0, 5, 1)] int index,
		//	[Range(-1, 5)] int newValue)
		//{
		//	ObservableCollection<int> items = new ObservableCollection<int>();
		//	if (item1 >= -2) items.Add(item1);
		//	if (item2 >= -2) items.Add(item2);
		//	if (item3 >= -2) items.Add(item3);
		//	if (item4 >= -2) items.Add(item4);
		//	if (item5 >= -2) items.Add(item5);

		//	if (index > items.Count) return;

		//	Aggregating<int, int> averaging = items.Averaging();

		//	averaging.ValidateConsistency();
		//	Assert.Equals(averaging.Value, items.Sum());

		//	items.Insert(index, newValue);

		//	averaging.ValidateConsistency();
		//	Assert.Equals(averaging.Value, items.Sum());
		//}

		//[Test, Combinatorial]
		//public void AverageComputing_Move(
		//	[Range(-3, 2, 1)] int item1,
		//	[Range(-3, 2, 1)] int item2,
		//	[Range(-3, 2, 1)] int item3,
		//	[Range(-3, 2, 1)] int item4,
		//	[Range(-3, 2, 1)] int item5,
		//	[Range(0, 5, 1)] int oldIndex,
		//	[Range(0, 5, 1)] int newIndex)
		//{
		//	ObservableCollection<int> items = new ObservableCollection<int>();
		//	if (item1 >= -2) items.Add(item1);
		//	if (item2 >= -2) items.Add(item2);
		//	if (item3 >= -2) items.Add(item3);
		//	if (item4 >= -2) items.Add(item4);
		//	if (item5 >= -2) items.Add(item5);

		//	if (oldIndex >= items.Count || newIndex >= items.Count) return;

		//	Aggregating<int, int> averaging = items.Averaging();

		//	averaging.ValidateConsistency();
		//	Assert.Equals(averaging.Value, items.Sum());

		//	items.Move(oldIndex, newIndex);

		//	averaging.ValidateConsistency();
		//	Assert.Equals(averaging.Value, items.Sum());
		//}

		public AveragingTests(bool debug) : base(debug)
		{
		}
	}
}