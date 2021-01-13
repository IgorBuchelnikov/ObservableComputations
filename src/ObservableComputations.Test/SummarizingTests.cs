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
	public class SummarizingTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\Summarizing_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\Summarizing_Deep_Time.log");

		[Test]
		public void Summarizing_Deep()
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
					
			test(new int[0]);

#if !TestCoverageAnalisis
			int from = -2;
			int to = 2;
#else
			int from = -2;
			int to = 0;
#endif

			for (int v1 = from; v1 <= to; v1++)
			{
				test(new []{v1});
				for (int v2 = from; v2 <= to; v2++)
				{
					test(new []{v1, v2});
					for (int v3 = from; v3 <= to; v3++)
					{
						test(new []{v1, v2, v3});
#if !TestCoverageAnalisis
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
#endif
					}
				}
			}
		}

		private void test(int[] values)
		{
			string testNum = string.Empty;
			int index = 0;
			int value = 0;
			int indexOld = 0;
			int indexNew = 0;

			ObservableCollection<int> items;
			Summarizing<int> summarizing;
			try
			{
				trace(testNum = "1", values, index, value, indexOld, indexNew);
				items = getObservableCollection(values);
				summarizing = items.Summarizing().For(consumer);
				validateSummarizingConsistency(summarizing, items);
				Assert.AreEqual(summarizing.Value, items.Sum());
				consumer.Dispose();

				for (index = 0; index < values.Length; index++)
				{
					trace(testNum = "2", values, index, value, indexOld, indexNew);
					items = getObservableCollection(values);
					Summarizing<int> summarizing1 = items.Summarizing().For(consumer);
					items.RemoveAt(index);
					validateSummarizingConsistency(summarizing1, items);
					Assert.AreEqual(summarizing1.Value, items.Sum());
					consumer.Dispose();
				}

				for (index = 0; index <= values.Length; index++)
				{
					for (value = 0; value <= values.Length; value++)
					{
						trace(testNum = "8", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						Summarizing<int> summarizing1 = items.Summarizing().For(consumer);
						items.Insert(index, value);
						validateSummarizingConsistency(summarizing1, items);
						Assert.AreEqual(summarizing1.Value, items.Sum());
						consumer.Dispose();
					}
				}

				for (index = 0; index < values.Length; index++)
				{

					for (value = -1; value <= values.Length; value++)
					{
						trace(testNum = "3", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						Summarizing<int> summarizing2 = items.Summarizing().For(consumer);
						items[index] = value;
						validateSummarizingConsistency(summarizing2, items);
						Assert.AreEqual(summarizing2.Value, items.Sum());
						consumer.Dispose();

					}
				}

				for (indexOld = 0; indexOld < values.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < values.Length; indexNew++)
					{
						trace(testNum = "7", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						Summarizing<int> summarizing2 = items.Summarizing().For(consumer);
						items.Move(indexOld, indexNew);
						validateSummarizingConsistency(summarizing2, items);
						Assert.AreEqual(summarizing2.Value, items.Sum());
						consumer.Dispose();
					}
				}
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

		private void validateSummarizingConsistency(Summarizing<int> summarizing, ObservableCollection<int> items)
		{
			Assert.AreEqual(summarizing.Value, items.Sum());
		}

		private void trace(string testNum, int[] values, int index, int value, int indexOld, int indexNew)
		{
			string traceString = getTraceString(testNum, values, index, value, indexOld, indexNew);
			
			if (traceString == "#3. OrderNums=-1  index=0  value=-1   indexOld=0   indexNew=0")
			{
				Debugger.Break();
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
		//public void Summarizing_Change(
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

		//	Aggregating<int, int> summarizing = items.Summarizing().For(consumer);
		//	summarizing.ValidateConsistency();
		//	Assert.Equals(summarizing.Value, items.Sum());

		//	items[index] = newValue;

		//	summarizing.ValidateConsistency();
		//	Assert.Equals(summarizing.Value, items.Sum());

		//}

		//[Test, Combinatorial]
		//public void Summarizing_Remove(
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

		//	Aggregating<int, int> summarizing = items.Summarizing().For(consumer);
		//	summarizing.ValidateConsistency();
		//	Assert.Equals(summarizing.Value, items.Sum());

		//	items.RemoveAt(index);

		//	summarizing.ValidateConsistency();
		//	Assert.Equals(summarizing.Value, items.Sum());
		//}

		//[Test, Combinatorial]
		//public void Summarizing_Insert(
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

		//	Aggregating<int, int> summarizing = items.Summarizing().For(consumer);

		//	summarizing.ValidateConsistency();
		//	Assert.Equals(summarizing.Value, items.Sum());

		//	items.Insert(index, newValue);

		//	summarizing.ValidateConsistency();
		//	Assert.Equals(summarizing.Value, items.Sum());
		//}

		//[Test, Combinatorial]
		//public void Summarizing_Move(
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

		//	Aggregating<int, int> summarizing = items.Summarizing().For(consumer);

		//	summarizing.ValidateConsistency();
		//	Assert.Equals(summarizing.Value, items.Sum());

		//	items.Move(oldIndex, newIndex);

		//	summarizing.ValidateConsistency();
		//	Assert.Equals(summarizing.Value, items.Sum());
		//}

		public SummarizingTests(bool debug) : base(debug)
		{
		}
	}
}