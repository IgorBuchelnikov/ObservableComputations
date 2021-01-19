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
	public class ExceptingTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item : INotifyPropertyChanged
		{

			public Item(int id)
			{
				_id = id;
				Num = LastNum;
				LastNum++;
			}

			public static int LastNum;
			public int Num;

			private int _id;

			public int Id
			{
				get { return _id; }
				set { updatePropertyValue(ref _id, value); }
			}

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

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\Projects\NevaPolimer\Excepting_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\Projects\NevaPolimer\Excepting_Deep_Time.log");

		[Test]
		public void Excepting_Deep()
		{			
			test(new int[0], new int[0]);

			int from = -1;
			int to = 3;

			for (int v1 = from; v1 <= to; v1++)
			{
				test(new []{v1}, new int[0]);
				for (int v2 = from; v2 <= to; v2++)
				{
					test(new []{v1}, new []{v2});
					for (int v3 = from; v3 <= to; v3++)
					{
						test(new []{v1, v3}, new []{v2});
						for (int v4 = from; v4 <= to; v4++)
						{
							test(new []{v1, v3}, new []{v2, v4});
						}
					}
				}
			}
		}

		private void test(int[] ids1, int[] ids2)
		{
			string testNum = string.Empty;
			int index = 0;
			int indexOld = 0;
			int indexNew = 0;
			int index1 = 0;
			int newItemId = 0;
			ObservableCollection<Item> items1;
			ObservableCollection<Item> items2;
			Excepting<Item> excepting;
			try
			{
				trace(testNum = "1", ids1, ids2, newItemId, index, indexOld, indexNew);
				items1 = getObservableCollection(ids1);
				items2 = getObservableCollection(ids2);
				excepting = items1.Excepting(items2).For(consumer);
				excepting.ValidateConsistency();				
				consumer.Dispose();

				for (index = 0; index < ids1.Length; index++)
				{
					trace(testNum = "2", ids1, ids2, newItemId, index, indexOld, indexNew);
					items1 = getObservableCollection(ids1);
					items2 = getObservableCollection(ids2);
					excepting = items1.Excepting(items2).For(consumer);
					items1.RemoveAt(index);
					excepting.ValidateConsistency();					
					consumer.Dispose();
				}

				for (index = 0; index < ids2.Length; index++)
				{
					trace(testNum = "3", ids1, ids2, newItemId, index, indexOld, indexNew);
					items1 = getObservableCollection(ids1);
					items2 = getObservableCollection(ids2);
					excepting = items1.Excepting(items2).For(consumer);
					items2.RemoveAt(index);
					excepting.ValidateConsistency();					
					consumer.Dispose();
				}

				for (index = 0; index <= ids1.Length; index++)
				{
					for (newItemId = 0; newItemId <= ids1.Length; newItemId++)
					{
						trace(testNum = "4", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						items2 = getObservableCollection(ids2);
						excepting = items1.Excepting(items2).For(consumer);
						items1.Insert(index, new Item(newItemId));
						excepting.ValidateConsistency();						
						consumer.Dispose();
					}

				}

				for (index = 0; index <= ids2.Length; index++)
				{
					for (newItemId = 0; newItemId <= ids2.Length; newItemId++)
					{
						trace(testNum = "5", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						items2 = getObservableCollection(ids2);
						excepting = items1.Excepting(items2).For(consumer);
						items2.Insert(index, new Item(newItemId));
						excepting.ValidateConsistency();						
						consumer.Dispose();
					}
				}

				for (index = 0; index < ids1.Length; index++)
				{
					for (newItemId = 0; newItemId <= ids1.Length; newItemId++)
					{
						trace(testNum = "6", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						items2 = getObservableCollection(ids2);
						excepting = items1.Excepting(items2).For(consumer);
						items1[index] = new Item(newItemId);
						excepting.ValidateConsistency();						
						consumer.Dispose();
					}
				}

				for (index = 0; index < ids2.Length; index++)
				{
					for (newItemId = 0; newItemId <= ids2.Length; newItemId++)
					{
						trace(testNum = "7", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						items2 = getObservableCollection(ids2);
						excepting = items1.Excepting(items2).For(consumer);
						items2[index] = new Item(newItemId);
						excepting.ValidateConsistency();						
						consumer.Dispose();
					}
				}

				for (indexOld = 0; indexOld < ids1.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < ids1.Length; indexNew++)
					{
						trace(testNum = "8", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						items2 = getObservableCollection(ids2);
						excepting = items1.Excepting(items2).For(consumer);
						items1.Move(indexOld, indexNew);
						excepting.ValidateConsistency();						
						consumer.Dispose();
					}
				}

				for (indexOld = 0; indexOld < ids2.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < ids2.Length; indexNew++)
					{
						trace(testNum = "9", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						items2 = getObservableCollection(ids2);
						excepting = items1.Excepting(items2).For(consumer);
						items2.Move(indexOld, indexNew);
						excepting.ValidateConsistency();						
						consumer.Dispose();
					}
				}

			}
			catch (Exception e)
			{
				string traceString = getTraceString( 
					testNum, ids1, ids2, newItemId, index, indexOld, indexNew);
				_textFileOutputLog.AppentLine(traceString);
				_textFileOutputLog.AppentLine(e.Message);
				_textFileOutputLog.AppentLine(e.StackTrace);
				throw new Exception(traceString, e);
			}

			writeUsefulTest(getTestString(ids1, ids2));

		}

		private void trace(string num, int[] ids1, int[] ids2, int newId, int index, int indexOld, int indexNew)
		{
			string traceString = getTraceString(num, ids1, ids2, newId, index, indexOld, indexNew);
			if (traceString == "#9. ItemsCounts1=1   ItemsCounts2=2 " +
				"index=2  indexOld=0   indexNew=1")
			{
				Debugger.Break();
			}
		}

		private static string getTraceString(string num, int[] ids1, int[] ids2, int newId, int index, int indexOld, int indexNew)
		{
			return string.Format(
				"#{0}. ids1={1}  ids2={2} index={3} newId= {6} indexOld={4}   indexNew={5}",
				num,
				string.Join(",", ids1),
				string.Join(",", ids2),
				index,
				indexOld,
				indexNew,
				newId);
		}


		private static ObservableCollection<Item> getObservableCollection(int[] ids)
		{	
			return new ObservableCollection<Item>(Enumerable.Range(0, ids.Length).Select(i => ids[i] >= 0 ? new Item(ids[i]) : null));
		}


		public ExceptingTests(bool debug) : base(debug)
		{
		}
	}
}
