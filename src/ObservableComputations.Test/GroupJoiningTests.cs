// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public class GroupJoiningTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item : INotifyPropertyChanged
		{

			public Item(int? id)
			{
				_id = id;
				Num = LastNum;
				LastNum++;
			}

			public static int LastNum;
			public int Num;

			private int? _id;

			public int? Id
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

			public override string ToString()
			{
				return $"Num={Num}  Id={Id}";
			}
		}

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\Projects\NevaPolimer\GroupJoining_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\Projects\NevaPolimer\GroupJoining_Deep_Time.log");

		[Test]
		public void GroupJoining_Deep()
		{		
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			test(new int[0], new int[0]);

			for (int v1 = -1; v1 <= 3; v1++)
			{
				for (int v2 = -1; v2 <= 3; v2++)
				{
#if !TestCoverageAnalisis
					test(new []{v1, v2}, new int[0]);
					test(new int[0], new []{v1, v2});
#endif
					for (int v3 = -1; v3 <= 3; v3++)
					{
						for (int v4 = -1; v4 <= 3; v4++)
						{
							test(new []{v1, v2, v3, v4}, new []{v1, v2, v3, v4});
#if !TestCoverageAnalisis
							test(new []{v1, v2, v3, v4}, new []{v1, v2});
							test(new []{v1, v2}, new []{v1, v2, v3, v4});
#endif
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
			GroupJoining<Item, Item, int?> groupJoining;
			try
			{
				trace(testNum = "1", ids1, ids2, newItemId, index, indexOld, indexNew);
				items1 = getObservableCollection(ids1);
				items2 = getObservableCollection(ids2);
				groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
				groupJoining.ValidateConsistency();				
				consumer.Dispose();

				for (index = 0; index < ids1.Length; index++)
				{
					trace(testNum = "2", ids1, ids2, newItemId, index, indexOld, indexNew);
					items1 = getObservableCollection(ids1);
					items2 = getObservableCollection(ids2);
					groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
					items1.RemoveAt(index);
					groupJoining.ValidateConsistency();					
					consumer.Dispose();
				}

				for (index = 0; index < ids2.Length; index++)
				{
					trace(testNum = "3", ids1, ids2, newItemId, index, indexOld, indexNew);
					items1 = getObservableCollection(ids1);
					items2 = getObservableCollection(ids2);
					groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
					items2.RemoveAt(index);
					groupJoining.ValidateConsistency();					
					consumer.Dispose();
				}

				for (index = 0; index <= ids1.Length; index++)
				{
					for (newItemId = 0; newItemId <= ids1.Length; newItemId++)
					{
						trace(testNum = "4", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						items2 = getObservableCollection(ids2);
						groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
						items1.Insert(index, new Item(newItemId));
						groupJoining.ValidateConsistency();						
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
						groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
						items2.Insert(index, new Item(newItemId));
						groupJoining.ValidateConsistency();						
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
						groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
						items1[index] = new Item(newItemId);
						groupJoining.ValidateConsistency();						
						consumer.Dispose();
					}

					for (newItemId = 0; newItemId <= ids1.Length; newItemId++)
					{
						trace(testNum = "10", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						if (items1[index] == null) continue;   
						items2 = getObservableCollection(ids2);
						groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
						items1[index].Id = newItemId;
						groupJoining.ValidateConsistency();						
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
						groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
						items2[index] = new Item(newItemId);
						groupJoining.ValidateConsistency();						
						consumer.Dispose();
					}

					for (newItemId = 0; newItemId <= ids2.Length; newItemId++)
					{
						trace(testNum = "11", ids1, ids2, newItemId, index, indexOld, indexNew);
						items1 = getObservableCollection(ids1);
						items2 = getObservableCollection(ids2);
						if (items2[index] == null) continue; 
						groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
						items2[index].Id = newItemId;
						groupJoining.ValidateConsistency();						
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
						groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
						items1.Move(indexOld, indexNew);
						groupJoining.ValidateConsistency();						
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
						groupJoining = items1.GroupJoining(items2, item1 => item1.Id, item2 => item2.Id).For(consumer);
						items2.Move(indexOld, indexNew);
						groupJoining.ValidateConsistency();						
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
		}

		private void trace(string num, int[] ids1, int[] ids2, int newId, int index, int indexOld, int indexNew)
		{
			string traceString = getTraceString(num, ids1, ids2, newId, index, indexOld, indexNew);
			if (traceString == "#10. ids1=-1,-1  ids2=  index=0  newId=0  indexOld=0  indexNew=0")
			{
				Debugger.Break();
			}
		}

		private static string getTraceString(string num, int[] ids1, int[] ids2, int newId, int index, int indexOld, int indexNew)
		{
			return string.Format(
				"#{0}. ids1={1}  ids2={2}  index={3}  newId={6}  indexOld={4}  indexNew={5}",
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
			return new ObservableCollection<Item>(Enumerable.Range(0, ids.Length).Select(i => ids[i] >= 0 ? new Item(ids[i]) : new Item(null)));
		}


		public GroupJoiningTests(bool debug) : base(debug)
		{
		}
	}
}