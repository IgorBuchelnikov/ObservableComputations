using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace IBCode.ObservableCalculations.Test
{
	[TestFixture]
	public class OrderingTests
	{
		public class Item : INotifyPropertyChanged
		{
			private int? _orderNum;

			public int? OrderNum
			{
				get { return _orderNum; }
				set { updatePropertyValue(ref _orderNum, value); }
			}

			public Item(int? orderNum)
			{
				_orderNum = orderNum;
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

			#region Overrides of Object

			public override string ToString()
			{
				return $"OrderNum = {(OrderNum == null ? "null" : OrderNum.Value.ToString())},   Num = {Num}";
			}

			#endregion
		}

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\Projects\NevaPolimer\Ordering_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\Projects\NevaPolimer\Ordering_Deep_Time.log");

		[Test, Combinatorial]
		public void Ordering_Deep(
			[Values(ListSortDirection.Ascending, ListSortDirection.Descending)] ListSortDirection listSortDirection)
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
				
			test(new int[0], listSortDirection);

			for (int v1 = -1; v1 <= 5; v1++)
			{
				test(new []{v1}, listSortDirection);
				for (int v2 = -1; v2 <= 5; v2++)
				{
					test(new []{v1, v2}, listSortDirection);
					for (int v3 = -1; v3 <= 5; v3++)
					{
						test(new []{v1, v2, v3}, listSortDirection);
						for (int v4 = -1; v4 <= 5; v4++)
						{
							test(new []{v1, v2, v3, v4}, listSortDirection);
							for (int v5 = -1; v5 <= 5; v5++)
							{
								test(new[] {v1, v2, v3, v4, v5}, listSortDirection);
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

		private void test(int[] orderNums, ListSortDirection listSortDirection)
		{
			int index = 0;
			int orderNum = 0;
			int indexOld = 0;
			int indexNew = 0;
			string testNum = string.Empty;

			ObservableCollection<Item> items;
			Ordering<Item, int?> ordering;
			try
			{
				trace(testNum = "1", orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
				items = getObservableCollection(orderNums);
				ordering = items.Ordering(i => i.OrderNum, listSortDirection);
				ordering.ValidateConsistency();

				for (index = 0; index < orderNums.Length; index++)
				{
					trace(testNum = "2", orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
					items = getObservableCollection(orderNums);
					Ordering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection);
					items.RemoveAt(index);
					ordering1.ValidateConsistency();
				}

				for (index = 0; index <= orderNums.Length; index++)
				{
					for (orderNum = -1; orderNum <= orderNums.Length; orderNum++)
					{
						trace(testNum = "8", orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
						items = getObservableCollection(orderNums);
						Ordering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection);
						items.Insert(index, new Item(orderNum == -1 ? (int?)null : orderNum));
						ordering1.ValidateConsistency();
					}
				}

				for (index = 0; index < orderNums.Length; index++)
				{
					trace(testNum = "6", orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
					items = getObservableCollection(orderNums);
					Ordering<Item, int?> ordering3 = items.Ordering(i => i.OrderNum, listSortDirection);
					items[index] = new Item(null);
					ordering3.ValidateConsistency();

					for (orderNum = -1; orderNum <= orderNums.Length; orderNum++)
					{
						trace(testNum = "3", orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
						items = getObservableCollection(orderNums);
						Ordering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection);
						items[index] = new Item(orderNum == -1 ? (int?)null : orderNum);
						ordering2.ValidateConsistency();

					}
				}

				for (index = 0; index < orderNums.Length; index++)
				{
					trace(testNum = "4", orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
					items = getObservableCollection(orderNums);
					Ordering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection);
					items[index].OrderNum = null;
					ordering1.ValidateConsistency();

					for (orderNum = -1; orderNum <= orderNums.Length; orderNum++)
					{
						trace(testNum = "5", orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
						items = getObservableCollection(orderNums);
						Ordering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection);
						items[index].OrderNum = orderNum == -1 ? (int?)null : orderNum;
						ordering2.ValidateConsistency();
					}
				}

				for (indexOld = 0; indexOld < orderNums.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < orderNums.Length; indexNew++)
					{
						trace(testNum = "6", orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
						items = getObservableCollection(orderNums);
						Ordering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection);
						items.Move(indexOld, indexNew);
						ordering2.ValidateConsistency();
					}
				}
			}
			catch (Exception e)
			{
				string traceString = getTraceString(testNum, orderNums, listSortDirection, index, orderNum, indexOld, indexNew);
				_textFileOutputLog.AppentLine(traceString);
				_textFileOutputLog.AppentLine(e.Message);
				_textFileOutputLog.AppentLine(e.StackTrace);
				throw new Exception(traceString, e);
			}

		}

		private void trace(string num, int[] orderNums, ListSortDirection listSortDirection, int index, int orderNum, int indexOld,
			int indexNew)
		{
			string traceString = getTraceString(num, orderNums, listSortDirection, index, orderNum, indexOld, indexNew);

			if (traceString == "#3. OrderNums=-1,-1  index=1  orderNum=0   indexOld=0   indexNew=0 listSortDirection=Ascending")
			{
				
			}
		}

		private static string getTraceString(string num, int[] orderNums, ListSortDirection listSortDirection, int index, int orderNum, int indexOld, int indexNew)
		{
			return string.Format(
				"#{6}. OrderNums={0}  index={1}  orderNum={2}   indexOld={3}   indexNew={4} listSortDirection={5}",
				string.Join(",", orderNums),
				index,
				orderNum,
				indexOld,
				indexNew,
				listSortDirection,
				num);
		}


		private static ObservableCollection<Item> getObservableCollection(int[] orderNums)
		{
			return new ObservableCollection<Item>(orderNums.Select(orderNum => new Item(orderNum >= 0 ? orderNum : (int?)null)));
		}

		[Test]
		public void Test()
		{
			ObservableCollection<Item> items = getObservableCollection(new []{-1,-1,-1,-1,-1,0,0});
			Ordering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, ListSortDirection.Ascending);
			items[0] = new Item(-1);
			ordering2.ValidateConsistency();		
		}

		[Test]
		public void Test1()
		{
			Console.WriteLine("!!!!!!!!!!!!!!!!!!Console Console Console Console !!!!!!!!!!!");		
			Console.WriteLine("!!!!!!!!!!!!!!!!!! Console Console Console Console !!!!!!!!!!!!");		
		}

		[Test]
		public void Test2()
		{
			ObservableCollection<Item> items = getObservableCollection(new []{-1, 1});
			Ordering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, ListSortDirection.Ascending);
			items[0] = new Item(0);
			ordering2.ValidateConsistency();		
		}

		//[Test]
		//public void Ordering_Initialization_00()
		//{
		//	Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!jmknmkolnmknknk!!!!!!!!!!!!!!!!!!!!");
		//}

		//[Test, Combinatorial]
		//public void Ordering_Initialization_01(
		//	[Values(0, null)] int item0,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum, listSortDirection);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		//public void Ordering_Initialization_02(
		//	[Values(0, 1, null)] int item0,
		//	[Values(0, 1, null)] int item1,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		//public void Ordering_Initialization_03(
		//	[Values(0, 1, 2, null)] int item0,
		//	[Values(0, 1, 2, null)] int item1,
		//	[Values(0, 1, 2, null)] int item2,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1),
		//			new Item(item2)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		//public void Ordering_Initialization_04(
		//	[Values(0, 1, 2, 3, null)] int item0,
		//	[Values(0, 1, 2, 3, null)] int item1,
		//	[Values(0, 1, 2, 3, null)] int item2,
		//	[Values(0, 1, 2, 3, null)] int item3,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1),
		//			new Item(item2),
		//			new Item(item3)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		//public void Ordering_Initialization_05(
		//	[Values(0, 1, 2, 3, 4, null)] int item0,
		//	[Values(0, 1, 2, 3, 4, null)] int item1,
		//	[Values(0, 1, 2, 3, 4, null)] int item2,
		//	[Values(0, 1, 2, 3, 4, null)] int item3,
		//	[Values(0, 1, 2, 3, 4, null)] int item4,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1),
		//			new Item(item2),
		//			new Item(item3),
		//			new Item(item4)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		////[Timeout(1000)]
		//public void Ordering_Initialization_06(
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item0,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item1,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item2,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item3,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item4,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item5,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1),
		//			new Item(item2),
		//			new Item(item3),
		//			new Item(item4),
		//			new Item(item5)
		//		}
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}




		//[Test, Combinatorial]
		//public void Ordering_Remove_01(
		//	[Values(0, null)] int item0,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum, listSortDirection);
		//	ordering.RemoveAt(0);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		//public void Ordering_Insert_02(
		//	[Values(0, 1, null)] int item0,
		//	[Values(0, 1, null)] int item1,
		//	[Range(0, 1, 1)] int index,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//	ordering.RemoveAt(index);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		//public void Ordering_Insert_03(
		//	[Values(0, 1, 2, null)] int item0,
		//	[Values(0, 1, 2, null)] int item1,
		//	[Values(0, 1, 2, null)] int item2,
		//	[Range(0, 1, 1)] int index,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1),
		//			new Item(item2)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		//public void Ordering_Insert_04(
		//	[Values(0, 1, 2, 3, null)] int item0,
		//	[Values(0, 1, 2, 3, null)] int item1,
		//	[Values(0, 1, 2, 3, null)] int item2,
		//	[Values(0, 1, 2, 3, null)] int item3,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1),
		//			new Item(item2),
		//			new Item(item3)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}

		//[Test, Combinatorial]
		////[Timeout(1000)]
		//public void Ordering_Insert_05(
		//	[Values(0, 1, 2, 3, 4, null)] int item0,
		//	[Values(0, 1, 2, 3, 4, null)] int item1,
		//	[Values(0, 1, 2, 3, 4, null)] int item2,
		//	[Values(0, 1, 2, 3, 4, null)] int item3,
		//	[Values(0, 1, 2, 3, 4, null)] int item4,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1),
		//			new Item(item2),
		//			new Item(item3),
		//			new Item(item4)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}


		//[Test, Combinatorial]
		////[Timeout(1000)]
		//public void Ordering_Insert_06(
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item0,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item1,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item2,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item3,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item4,
		//	[Values(0, 1, 2, 3, 4, 5, null)] int item5,
		//	[Values(ListSortDirection.Ascending, ListSortDirection.Ascending)] ListSortDirection listSortDirection)
		//{
		//	ObservableCollection<Item> items = new ObservableCollection<Item>(
		//		new[]
		//		{
		//			new Item(item0),
		//			new Item(item1),
		//			new Item(item2),
		//			new Item(item3),
		//			new Item(item4),
		//			new Item(item5)
		//		}			
		//	);

		//	Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum);
		//	ordering.ValidateConsistency();
		//}
	}
}
