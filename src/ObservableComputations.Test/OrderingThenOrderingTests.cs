// Copyright (c) 2019-2021 Buchelnikov Igor Vladimirovich. All rights reserved
// Buchelnikov Igor Vladimirovich licenses this file to you under the MIT license.
// The LICENSE file is located at https://github.com/IgorBuchelnikov/ObservableComputations/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture(false)]
	public partial class OrderingThenOrderingTests : TestBase
	{
		OcConsumer consumer = new OcConsumer();

		public class Item : INotifyPropertyChanged, IEquatable<Item>
		{
			private int? _orderNum;

			public int? OrderNum
			{
				get { return _orderNum; }
				set { updatePropertyValue(ref _orderNum, value); }
			}

			private int? _orderNum2;
			public int? OrderNum2
			{
				get { return _orderNum2; }
				set { updatePropertyValue(ref _orderNum2, value); }
			}

			public Item(int? orderNum, int? orderNum2)
			{
				_orderNum = orderNum;
				_orderNum2 = orderNum2;
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
				onPropertyChanged(propertyName);
				return true;
			}

			#endregion

			public bool Equals(Item other)
			{
				return this == other;
			}

			public override string ToString()
			{
				return $"Num={Num} OrderNum={(OrderNum == null ? "null" : OrderNum.Value.ToString())} OrderNum2={(OrderNum2 == null ? "null" : OrderNum2.Value.ToString())}";
			}

			#region Overrides of Object

			public override bool Equals(object obj)
			{
				if (obj == null) return false;
				Item over = (Item)obj;
				return OrderNum == over.OrderNum && OrderNum2 == over.OrderNum2;
			}

			#endregion
		}

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"C:\ThenOrderingOrdering_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"C:\ThenOrderingOrdering_Deep_Time.log");


		[Test]
		public void OrderingThenOrdering_Test()
		{
			int[] orderNums1 = {0,0,0};
			int[] orderNums2 = {1,-1,-1};
			test(orderNums1, orderNums2, ListSortDirection.Ascending);
		}

#if !RunOnlyMinimalTestsToCover
		[Test, Combinatorial]
		public void ThenOrderingOrdering_Deep(
			[Values(ListSortDirection.Ascending, ListSortDirection.Descending)] ListSortDirection listSortDirection)
		{
			Random random =  new Random();

			for (int i1 = 0; i1 < 100; i1++)
			{
				int itemCount = random.Next(4, 7);
				int[] orderNums1 = new int[itemCount];
				int[] orderNums2 = new int[itemCount];

				for (int i = 0; i < itemCount; i++)
				{
					orderNums1[i] = random.Next(-1, itemCount - 1);
					orderNums2[i] = random.Next(-1, itemCount - 1);
				}

				test(orderNums1, orderNums2, listSortDirection);
			}


			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();

			int from = -1;
			int to = 3;

			for (int v1 = from; v1 <= to; v1++)
			{
				test(new []{v1}, listSortDirection);
				for (int v2 = from; v2 <= to; v2++)
				{
					test(new []{v1, v2}, listSortDirection);
					for (int v3 = from; v3 <= to; v3++)
					{
						test(new []{v1, v2, v3}, listSortDirection);

						for (int v4 = from; v4 <= to; v4++)
						{
							test(new []{v1, v2, v3, v4}, listSortDirection);
							counter++;
							_textFileOutputTime.AppentLine($"{stopwatch.Elapsed.TotalMinutes}: {counter}");
						}
					}
				}
			}
		}
#endif

		private void test(int[] orderNums, ListSortDirection listSortDirection)
		{
			//bool rootContinue = false;
			for (int v1 = -1; v1 <= 2; v1++)
			{
				//rootContinue = false;
				if (orderNums.Length == 1)
				{
					test(orderNums, new []{v1}, listSortDirection);
				}
				for (int v2 = -1; v2 <= 2; v2++)
				{
					//if (rootContinue) continue;
					if (orderNums.Length == 2)
					{
						test(orderNums, new []{v1, v2}, listSortDirection);
						//rootContinue = true;
						continue;
					}
					for (int v3 = -1; v3 <= 2; v3++)
					{
						//if (rootContinue) continue;
						if (orderNums.Length == 3)
						{
							test(orderNums, new []{v1, v2, v3}, listSortDirection);
							//rootContinue = true;
						}

					}
				}
			}			
		}

		private void test(int[] orderNums, int[] orderNums2, ListSortDirection listSortDirection)
		{
			int index = 0;
			int orderNum = 0;
			int orderNum2 = 0;
			int indexOld = 0;
			int indexNew = 0;
			string testNum = string.Empty;

			ObservableCollection<Item> items;
			Ordering<Item, int?> ordering;
			try
			{
				//trace(testNum = "1", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew);
				//items = getObservableCollection(orderNums, orderNums2);
				//ordering = items.Ordering(i => i.OrderNum, listSortDirection);
				//ordering.ValidateConsistency();

				for (index = 0; index < orderNums.Length; index++)
				{
					if (!traceThenOrderingOrdering(testNum = "2", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew)) continue;
					items = getObservableCollection(orderNums, orderNums2);
					ThenOrdering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
					items.RemoveAt(index);
					ordering1.ValidateConsistency();					
					consumer.Dispose();
				}

				for (index = 0; index <= orderNums.Length; index++)
				{
					for (orderNum = 0; orderNum <= orderNums.Length; orderNum++)
					{
						for (orderNum2 = 0; orderNum2 <= orderNums.Length; orderNum2++)
						{
							if (!traceThenOrderingOrdering(testNum = "8", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew)) continue;
							items = getObservableCollection(orderNums, orderNums2);
							ThenOrdering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
							items.Insert(index, new Item(orderNum == -1 ? (int?)null : orderNum, orderNum2 == -1 ? (int?)null : orderNum2));
							ordering1.ValidateConsistency();							
							consumer.Dispose();
						}
					}
				}

				for (index = 0; index < orderNums.Length; index++)
				{
					traceThenOrderingOrdering(testNum = "6", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew);
					items = getObservableCollection(orderNums, orderNums2);
					ThenOrdering<Item, int?> ordering3 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
					items[index] = new Item(null, null);
					ordering3.ValidateConsistency();					
					consumer.Dispose();

					for (orderNum = -1; orderNum <= orderNums.Length; orderNum++)
					{
						for (orderNum2 = -1; orderNum2 <= orderNums.Length; orderNum2++)
						{
							if (!traceThenOrderingOrdering(testNum = "3", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew)) continue;
							items = getObservableCollection(orderNums, orderNums2);
							ThenOrdering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
							items[index] = new Item(orderNum == -1 ? (int?)null : orderNum, orderNum2 == -1 ? (int?)null : orderNum2);
							ordering2.ValidateConsistency();							
							consumer.Dispose();
						}
					}
				}

				for (index = 0; index < orderNums.Length; index++)
				{
					traceThenOrderingOrdering(testNum = "4", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew);
					items = getObservableCollection(orderNums, orderNums2);
					ThenOrdering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
					items[index].OrderNum = null;
					ordering1.ValidateConsistency();					
					consumer.Dispose();

					for (orderNum = -1; orderNum <= orderNums.Length; orderNum++)
					{
						for (orderNum2 = -1; orderNum2 <= orderNums.Length; orderNum2++)
						{
							if (!traceThenOrderingOrdering(testNum = "7", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew)) continue;
							items = getObservableCollection(orderNums, orderNums2);
							ThenOrdering<Item, int?> ordering3 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
							items[index].OrderNum = orderNum == -1 ? (int?)null : orderNum;
							items[index].OrderNum2 = orderNum2 == -1 ? (int?)null : orderNum2;
							ordering3.ValidateConsistency();							
							consumer.Dispose();
						}

						traceThenOrderingOrdering(testNum = "5", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew);
						items = getObservableCollection(orderNums, orderNums2);
						ThenOrdering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
						items[index].OrderNum = orderNum == -1 ? (int?)null : orderNum;
						ordering2.ValidateConsistency();						
						consumer.Dispose();

						if (!traceThenOrderingOrdering(testNum = "9", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew)) continue;
						items = getObservableCollection(orderNums, orderNums2);
						ThenOrdering<Item, int?> ordering4 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
						items[index].OrderNum2 = orderNum == -1 ? (int?)null : orderNum;
						ordering4.ValidateConsistency();						
						consumer.Dispose();
					}
				}

				for (indexOld = 0; indexOld < orderNums.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < orderNums.Length; indexNew++)
					{
						if (!traceThenOrderingOrdering(testNum = "6", orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew)) continue;
						items = getObservableCollection(orderNums, orderNums2);
						ThenOrdering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).For(consumer);
						items.Move(indexOld, indexNew);
						ordering2.ValidateConsistency();						
						consumer.Dispose();
					}
				}
			}
			catch (Exception e)
			{
				string traceString = getTraceString(testNum, orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew);
				_textFileOutputLog.AppentLine(traceString);
				_textFileOutputLog.AppentLine(e.Message);
				_textFileOutputLog.AppentLine(e.StackTrace);
				throw new Exception(traceString, e);
			}

			writeUsefulTest(getTestString(orderNums, orderNums2, listSortDirection));

		}

		private bool traceThenOrderingOrdering(string num, int[] orderNums, int[] orderNums2, ListSortDirection listSortDirection, int index, int orderNum, int orderNum2, int indexOld,
			int indexNew)
		{
			string traceString = getTraceString(num, orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew);
			//if (traceString == "#2. OrderNums1=-1,0,0  OrderNums2=-1,-1,-1  index=1  orderNum=0  orderNum2=0  indexOld=0   indexNew=0 listSortDirection=Ascending")
			//{
			//	return true;
			//}

			return true;
		}

		private static string getTraceString(string num, int[] orderNums, int[] orderNums2, ListSortDirection listSortDirection, int index, int orderNum, int orderNum2, int indexOld, int indexNew)
		{
			return string.Format(
				"#{8}. OrderNums1={0}  OrderNums2={1}  index={2}  orderNum={3}  orderNum2={4}  indexOld={5}   indexNew={6} listSortDirection={7}",
				string.Join(",", orderNums),
				string.Join(",", orderNums2),
				index,
				orderNum,
				orderNum2,
				indexOld,
				indexNew,
				listSortDirection,
				num);
		}

		private static ObservableCollection<Item> getObservableCollection(int[] orderNums, int[] orderNums2)
		{
			ObservableCollection<Item> result = new ObservableCollection<Item>();

			for (int i = 0; i < orderNums.Length; i++)
			{
				int? orderNum1 = orderNums[i] >= 0 ? orderNums[i] : (int?)null;
				int? orderNum2 = orderNums2[i] >= 0 ? orderNums2[i] : (int?)null;
				result.Add(new Item(orderNum1, orderNum2));
			}

			return result;
		}

		public OrderingThenOrderingTests(bool debug) : base(debug)
		{
		}
	}
}
