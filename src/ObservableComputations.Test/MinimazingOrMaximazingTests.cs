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
	public class MinimazingOrMaximazingTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\MinimazingOrMaximazing_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\MinimazingOrMaximazing_Deep_Time.log");

#if RunOnlyMinimalTestsToCover
		[Test]
		public void MinimazingOrMaximazing_Deep(
			[Values(MinimazingOrMaximazingMode.Maximazing, MinimazingOrMaximazingMode.Minimazing)] MinimazingOrMaximazingMode mode)
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
			
			test(new int[0], mode);

			int from = -2;
			int to = 2;

			for (int v1 = from; v1 <= to; v1++)
			{
				test(new []{v1}, mode);
				for (int v2 = from; v2 <= to; v2++)
				{
					test(new []{v1, v2}, mode);
					for (int v3 = from; v3 <= to; v3++)
					{
						test(new []{v1, v2, v3}, mode);

						for (int v4 = from; v4 <= to; v4++)
						{
							test(new []{v1, v2, v3, v4}, mode);
							for (int v5 = from; v5 <= to; v5++)
							{
								test(new[] {v1, v2, v3, v4, v5}, mode);
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

		private void test(int[] values, MinimazingOrMaximazingMode mode)
		{
			string testNum = string.Empty;
			int index = 0;
			int value = 0;
			int indexOld = 0;
			int indexNew = 0;

			ObservableCollection<int> items;
			MinimazingOrMaximazing<int> minimazingOrMaximazing;
			try
			{
				trace(testNum = "1", values, index, value, indexOld, indexNew);
				items = getObservableCollection(values);
				minimazingOrMaximazing = getMinimazingOrMaximazing(items, mode);
				minimazingOrMaximazing.ValidateConsistency();				
				consumer.Dispose();
				Assert.AreEqual(minimazingOrMaximazing.Value, getResult(items, mode));

				for (index = 0; index < values.Length; index++)
				{
					trace(testNum = "2", values, index, value, indexOld, indexNew);
					items = getObservableCollection(values);
					MinimazingOrMaximazing<int> minimazingOrMaximazing1 = getMinimazingOrMaximazing(items, mode);
					items.RemoveAt(index);
					minimazingOrMaximazing1.ValidateConsistency();					
					consumer.Dispose();
					Assert.AreEqual(minimazingOrMaximazing1.Value, getResult(items, mode));
				}

				for (index = 0; index <= values.Length; index++)
				{
					for (value = 0; value <= values.Length; value++)
					{
						trace(testNum = "8", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						MinimazingOrMaximazing<int> minimazingOrMaximazing1 = getMinimazingOrMaximazing(items, mode);
						items.Insert(index, value);
						minimazingOrMaximazing1.ValidateConsistency();						
						consumer.Dispose();
						Assert.AreEqual(minimazingOrMaximazing1.Value, getResult(items, mode));
					}
				}

				for (index = 0; index < values.Length; index++)
				{

					for (value = -1; value <= values.Length; value++)
					{
						trace(testNum = "3", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						MinimazingOrMaximazing<int> minimazingOrMaximazing2 = getMinimazingOrMaximazing(items, mode);
						items[index] = value;
						minimazingOrMaximazing2.ValidateConsistency();						
						consumer.Dispose();
						Assert.AreEqual(minimazingOrMaximazing2.Value, getResult(items, mode));

					}
				}

				for (indexOld = 0; indexOld < values.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < values.Length; indexNew++)
					{
						trace(testNum = "7", values, index, value, indexOld, indexNew);
						items = getObservableCollection(values);
						MinimazingOrMaximazing<int> minimazingOrMaximazing2 = getMinimazingOrMaximazing(items, mode);
						items.Move(indexOld, indexNew);
						minimazingOrMaximazing2.ValidateConsistency();						
						consumer.Dispose();
						Assert.AreEqual(minimazingOrMaximazing2.Value, getResult(items, mode));
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

			writeUsefulTest(getTestString(values, mode));
		}

		private void trace(string testNum, int[] values, int index, int value, int indexOld, int indexNew)
		{
			string traceString = getTraceString(testNum, values, index, value, indexOld, indexNew);

			if (traceString == "#3. OrderNums=-2,-2,-2,-2,0  index=4  value=-1   indexOld=0   indexNew=0")
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

		private MinimazingOrMaximazing<int> getMinimazingOrMaximazing(ObservableCollection<int> items, MinimazingOrMaximazingMode mode)
		{
			return mode == MinimazingOrMaximazingMode.Maximazing ? items.Maximazing().For(consumer) : items.Minimazing().For(consumer);
		}

		private int getResult(ObservableCollection<int> items, MinimazingOrMaximazingMode mode)
		{
			if (items.Count == 0) return 0;
			return mode == MinimazingOrMaximazingMode.Maximazing ? items.Max() : items.Min();
		}

		public MinimazingOrMaximazingTests(bool debug) : base(debug)
		{
		}
	}
}