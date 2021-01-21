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
	// TODO тестировать смену сепаратора
	[TestFixture(false)]
	public partial class StringsConcatenatingTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\Projects\NevaPolimer\Concatenating_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\Projects\NevaPolimer\Concatenating_Deep_Time.log");

#if !RunOnlyMinimalTestsToCover
		[Test, Combinatorial]
		public void Concatenating_Deep()
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
					
			test(new int[0]);

			int from = -1;
			int to = 4;

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

		private void test(int[] charsCounts)
		{
			string testNum = string.Empty;
			int index = 0;
			int charsCount = 0;
			int indexOld = 0;
			int indexNew = 0;
			int index1 = 0;
			ObservableCollection<string> strings;
			StringsConcatenating stringsConcatenating;
			Scalar<string> separatorScalar ;
			try
			{
				trace(testNum = "1", charsCounts, index, charsCount, indexOld, indexNew);
				strings = getObservableCollections(charsCounts);
				separatorScalar = new Scalar<string>("*");
				stringsConcatenating = strings.StringsConcatenating(separatorScalar).For(consumer);
				test(stringsConcatenating, separatorScalar);

				for (index = 0; index < charsCounts.Length; index++)
				{
					trace(testNum = "2", charsCounts, index, charsCount, indexOld, indexNew);
					strings = getObservableCollections(charsCounts);
					separatorScalar = new Scalar<string>("*");
					StringsConcatenating concatenating1 = strings.StringsConcatenating(separatorScalar).For(consumer);
					strings.RemoveAt(index);
					test(concatenating1, separatorScalar);
				}

				for (index = 0; index <= charsCounts.Length; index++)
				{
					for (charsCount = 0; charsCount <= charsCounts.Length; charsCount++)
					{
						trace(testNum = "11", charsCounts, index, charsCount, indexOld, indexNew);
						strings = getObservableCollections(charsCounts);
						separatorScalar = new Scalar<string>("*");
						StringsConcatenating concatenating2 = strings.StringsConcatenating(separatorScalar).For(consumer);
						strings.Insert(index, getString(charsCount));
						test(concatenating2, separatorScalar);
					}
				}

				for (index = 0; index < charsCounts.Length; index++)
				{
					trace(testNum = "6", charsCounts, index, charsCount, indexOld, indexNew);
					strings = getObservableCollections(charsCounts);
					separatorScalar = new Scalar<string>("*");
					StringsConcatenating concatenating3 = strings.StringsConcatenating(separatorScalar).For(consumer);
					strings[index] = string.Empty;
					test(concatenating3, separatorScalar);

					for (charsCount = 0; charsCount <= charsCounts.Length; charsCount++)
					{
						trace(testNum = "3", charsCounts, index, charsCount, indexOld, indexNew);
						strings = getObservableCollections(charsCounts);
						separatorScalar = new Scalar<string>("*");
						StringsConcatenating concatenating2 = strings.StringsConcatenating(separatorScalar).For(consumer);
						strings[index] = getString(charsCount);
						test(concatenating2, separatorScalar);

					}
				}

				for (index = 0; index < charsCounts.Length; index++)
				{
					trace(testNum = "4", charsCounts, index, charsCount, indexOld, indexNew);
					strings = getObservableCollections(charsCounts);
					separatorScalar = new Scalar<string>("*");
					StringsConcatenating concatenating1 = strings.StringsConcatenating(separatorScalar).For(consumer);
					strings[index] = null;
					test(concatenating1, separatorScalar);

					for (charsCount = 0; charsCount <= charsCounts.Length; charsCount++)
					{
						trace(testNum = "5", charsCounts, index, charsCount, indexOld, indexNew);
						strings = getObservableCollections(charsCounts);
						separatorScalar = new Scalar<string>("*");
						StringsConcatenating concatenating2 = strings.StringsConcatenating(separatorScalar).For(consumer);
						strings[index] = getString(charsCount);
						test(concatenating2, separatorScalar);

					}
				}

				for (indexOld = 0; indexOld < charsCounts.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < charsCounts.Length; indexNew++)
					{
						trace(testNum = "6", charsCounts, index, charsCount, indexOld, indexNew);
						strings = getObservableCollections(charsCounts);
						separatorScalar = new Scalar<string>("*");
						StringsConcatenating concatenating2 = strings.StringsConcatenating(separatorScalar).For(consumer);
						strings.Move(indexOld, indexNew);
						test(concatenating2, separatorScalar);
					}
				}
			}
			catch (Exception e)
			{
				string traceString = getTraceString( 
					testNum,
					charsCounts,
					index,
					charsCount, 
					indexOld,
					indexNew, 
					index1);
				_textFileOutputLog.AppentLine(traceString);
				_textFileOutputLog.AppentLine(e.Message);
				_textFileOutputLog.AppentLine(e.StackTrace);
				throw new Exception(traceString, e);
			}

			writeUsefulTest(getTestString(charsCounts));
		}

		private void test(StringsConcatenating stringsConcatenating, Scalar<string> separatorScalar)
		{
			stringsConcatenating.ValidateConsistency();
			separatorScalar.Change("");
			stringsConcatenating.ValidateConsistency();
			separatorScalar.Change("~!");
			stringsConcatenating.ValidateConsistency();
			separatorScalar.Change("#$%");
			stringsConcatenating.ValidateConsistency();
			separatorScalar.Change("^&");
			stringsConcatenating.ValidateConsistency();
			consumer.Dispose();
		}

		private void trace(string num, int[] charsCounts, int index, int charsCount, int indexOld,
			int indexNew, int index1 = 0)
		{
			string traceString = getTraceString(num, charsCounts, index, charsCount, indexOld, indexNew, index1);
			if (traceString == "#11. CharsCounts=-1,-1  index=0  charsCount=0   indexOld=0   indexNew=0, index1=0")
			{
				
			}
		}

		private static string getTraceString(string num, int[] charsCounts, int index, int charsCount, int indexOld, int indexNew, int index1 = 0)
		{
			return string.Format(
				"#{6}. CharsCounts={0}  index={1}  charsCount={2}   indexOld={3}   indexNew={4}, index1={5}",
				string.Join(",", charsCounts),
				index,
				charsCount,
				indexOld,
				indexNew,
				index1,
				num);
		}

		static int charNum;
		private static ObservableCollection<string> getObservableCollections(int[] charsCounts)
		{
			charNum = 0;
			return new ObservableCollection<string>(charsCounts.Select(charsCount => getString(charsCount)));
		}

		private static string getString(int charsCount)
		{
			return charsCount >= 0 
				? new string(Enumerable.Range(1, charsCount).Select(i => chars[charNum++]).ToArray())
				: null;
		}

		public static string chars = "abcdefghigklmnopqrstuvwxyz";

		public StringsConcatenatingTests(bool debug) : base(debug)
		{
		}
	}
}
