// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using NUnit.Framework;

//namespace ObservableComputations.Test
//{
//	[TestFixture]
//	public class CrossingTests
//	{
//		public class Item : INotifyPropertyChanged
//		{

//			public Item()
//			{
//				Num = LastNum;
//				LastNum++;
//			}

//			public static int LastNum;
//			public int Num;

//			#region INotifyPropertyChanged imlementation

//			public event PropertyChangedEventHandler PropertyChanged;

//			protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
//			{
//				PropertyChangedEventHandler onPropertyChanged = PropertyChanged;
//				if (onPropertyChanged != null) onPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//			}

//			protected bool updatePropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
//			{
//				if (EqualityComparer<T>.Default.Equals(field, value)) return false;
//				field = value;
//				this.onPropertyChanged(propertyName);
//				return true;
//			}

//			#endregion

//			public override string ToString()
//			{
//				return Num.ToString();
//			}
//		}

//		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\Projects\NevaPolimer\Crossing_Deep.log");
//		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\Projects\NevaPolimer\Crossing_Deep_Time.log");

//		[Test, Combinatorial]
//		public void Crossing_Deep()
//		{			
//			for (int v1 = 0; v1 <= 7; v1++)
//			{
//				for (int v2 = 0; v2 <= 7; v2++)
//				{
//					test(v1, v2);
//				}
//			}
//		}

//		private void test(int count1, int count2)
//		{
//			string testNum = string.Empty;
//			int index = 0;
//			int indexOld = 0;
//			int indexNew = 0;
//			int index1 = 0;
//			ObservableCollection<Item> items1;
//			ObservableCollection<Item> items2;
//			Crossing<Item, Item> crossing;
//			try
//			{
//				trace(testNum = "1", count1, count2, index, indexOld, indexNew);
//				items1 = getObservableCollection(count1);
//				items2 = getObservableCollection(count2);
//				crossing = new Crossing<Item, Item>(items1, items2);
//				crossing.ValidateConsistency();

//				for (index = 0; index < count1; index++)
//				{
//					trace(testNum = "2", count1, count2, index, indexOld, indexNew);
//					items1 = getObservableCollection(count1);
//					items2 = getObservableCollection(count2);
//					crossing = new Crossing<Item, Item>(items1, items2);
//					items1.RemoveAt(index);
//					crossing.ValidateConsistency();
//				}

//				for (index = 0; index < count2; index++)
//				{
//					trace(testNum = "3", count1, count2, index, indexOld, indexNew);
//					items1 = getObservableCollection(count1);
//					items2 = getObservableCollection(count2);
//					crossing = new Crossing<Item, Item>(items1, items2);
//					items2.RemoveAt(index);
//					crossing.ValidateConsistency();
//				}

//				for (index = 0; index <= count1; index++)
//				{
//					trace(testNum = "4", count1, count2, index, indexOld, indexNew);
//					items1 = getObservableCollection(count1);
//					items2 = getObservableCollection(count2);
//					crossing = new Crossing<Item, Item>(items1, items2);
//					items1.Insert(index, new Item());
//					crossing.ValidateConsistency();
//				}

//				for (index = 0; index <= count2; index++)
//				{
//					trace(testNum = "5", count1, count2, index, indexOld, indexNew);
//					items1 = getObservableCollection(count1);
//					items2 = getObservableCollection(count2);
//					crossing = new Crossing<Item, Item>(items1, items2);
//					items2.Insert(index, new Item());
//					crossing.ValidateConsistency();
//				}

//				for (index = 0; index < count1; index++)
//				{
//					trace(testNum = "6", count1, count2, index, indexOld, indexNew);
//					items1 = getObservableCollection(count1);
//					items2 = getObservableCollection(count2);
//					crossing = new Crossing<Item, Item>(items1, items2);
//					items1[index] = new Item();
//					crossing.ValidateConsistency();
//				}

//				for (index = 0; index < count2; index++)
//				{
//					trace(testNum = "7", count1, count2, index, indexOld, indexNew);
//					items1 = getObservableCollection(count1);
//					items2 = getObservableCollection(count2);
//					crossing = new Crossing<Item, Item>(items1, items2);
//					items2[index] = new Item();
//					crossing.ValidateConsistency();
//				}

//				for (indexOld = 0; indexOld < count1; indexOld++)
//				{
//					for (indexNew = 0; indexNew < count1; indexNew++)
//					{
//						trace(testNum = "8", count1, count2, index, indexOld, indexNew);
//						items1 = getObservableCollection(count1);
//						items2 = getObservableCollection(count2);
//						crossing = new Crossing<Item, Item>(items1, items2);
//						items1.Move(indexOld, indexNew);
//						crossing.ValidateConsistency();
//					}
//				}

//				for (indexOld = 0; indexOld < count2; indexOld++)
//				{
//					for (indexNew = 0; indexNew < count2; indexNew++)
//					{
//						trace(testNum = "9", count1, count2, index, indexOld, indexNew);
//						items1 = getObservableCollection(count1);
//						items2 = getObservableCollection(count2);
//						crossing = new Crossing<Item, Item>(items1, items2);
//						items2.Move(indexOld, indexNew);
//						crossing.ValidateConsistency();
//					}
//				}

//			}
//			catch (Exception e)
//			{
//				string traceString = getTraceString( 
//					testNum, count1, count2, index, indexOld, indexNew);
//				_textFileOutputLog.AppentLine(traceString);
//				_textFileOutputLog.AppentLine(e.Message);
//				_textFileOutputLog.AppentLine(e.StackTrace);

//				throw new Exception(traceString, e);
//			}

//		}

//		private void trace(string num, int count1, int count2, int index, int indexOld, int indexNew)
//		{
//			string traceString = getTraceString(num, count1, count2, index, indexOld, indexNew);
//			if (traceString == "#3. ItemsCounts1=2   ItemsCounts2=1 index=0  indexOld=0   indexNew=0")
//			{
//				Debugger.Break();
//			}
//		}

//		private static string getTraceString(string num, int count1, int count2, int index, int indexOld, int indexNew)
//		{
//			return string.Format(
//				"#{0}. ItemsCounts1={1}   ItemsCounts2={2} index={3}  indexOld={4}   indexNew={5}",
//				num,
//				count1,
//				count2,
//				index,
//				indexOld,
//				indexNew);
//		}


//		private static ObservableCollection<Item> getObservableCollection(int itemsCounts)
//		{	
//			return new ObservableCollection<Item>(Enumerable.Range(0, itemsCounts).Select(i => new Item()));
//		}
//	}
//}
