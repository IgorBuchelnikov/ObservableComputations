using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class MinimazingOrMaximazingTests
	{
        Consumer consumer = new Consumer();

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\MinimazingOrMaximazing_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\MinimazingOrMaximazing_Deep_Time.log");

		[Test]
		public void MinimazingOrMaximazing_Deep(
			[Values(MinimazingOrMaximazingMode.Maximazing, MinimazingOrMaximazingMode.Minimazing)] MinimazingOrMaximazingMode mode)
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
			
			test(new int[0], mode);

			for (int v1 = -2; v1 <= 2; v1++)
			{
				test(new []{v1}, mode);
				for (int v2 = -2; v2 <= 2; v2++)
				{
					test(new []{v1, v2}, mode);
					for (int v3 = -2; v3 <= 2; v3++)
					{
						test(new []{v1, v2, v3}, mode);
						for (int v4 = -2; v4 <= 2; v4++)
						{
							test(new []{v1, v2, v3, v4}, mode);
							for (int v5 = -2; v5 <= 2; v5++)
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
				Assert.AreEqual(minimazingOrMaximazing.Value, getResult(items, mode));

				for (index = 0; index < values.Length; index++)
				{
					trace(testNum = "2", values, index, value, indexOld, indexNew);
					items = getObservableCollection(values);
					MinimazingOrMaximazing<int> minimazingOrMaximazing1 = getMinimazingOrMaximazing(items, mode);
					items.RemoveAt(index);
					minimazingOrMaximazing1.ValidateConsistency();
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

		}

		private void trace(string testNum, int[] values, int index, int value, int indexOld, int indexNew)
		{
			string traceString = getTraceString(testNum, values, index, value, indexOld, indexNew);

			if (traceString == "#8. OrderNums=-2  index=0  value=0   indexOld=0   indexNew=0")
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
			return mode == MinimazingOrMaximazingMode.Maximazing ? items.Maximazing().IsNeededFor(consumer) : items.Minimazing().IsNeededFor(consumer);
		}

		private int getResult(ObservableCollection<int> items, MinimazingOrMaximazingMode mode)
		{
			if (items.Count == 0) return 0;
			return mode == MinimazingOrMaximazingMode.Maximazing ? items.Max() : items.Min();
		}
	}
}