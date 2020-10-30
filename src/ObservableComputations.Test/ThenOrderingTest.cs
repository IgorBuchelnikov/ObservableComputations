using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class ThenOrderingOrderingTests
	{
        Consumer consumer = new Consumer();

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

			private int? _orderNum3;
			public int? OrderNum3
			{
				get { return _orderNum3; }
				set { updatePropertyValue(ref _orderNum3, value); }
			}

			public Item(int? orderNum, int? orderNum2)
			{
				_orderNum = orderNum;
				_orderNum2 = orderNum2;
				Num = LastNum;
				LastNum++;
			}

			public Item(int? orderNum, int? orderNum2, int? orderNum3)
			{
				_orderNum = orderNum;
				_orderNum2 = orderNum2;
				_orderNum3 = orderNum3;
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
				return $"Num={Num} OrderNum={(OrderNum == null ? "null" : OrderNum.Value.ToString())} OrderNum2={(OrderNum2 == null ? "null" : OrderNum2.Value.ToString())} OrderNum3={(OrderNum3 == null ? "null" : OrderNum3.Value.ToString())}";
			}

			#region Overrides of Object

			public override bool Equals(object obj)
			{
				if (obj == null) return false;
				Item over = (Item)obj;
				return OrderNum == over.OrderNum && OrderNum2 == over.OrderNum2 && OrderNum3 == over.OrderNum3;
			}

			#endregion
		}

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"C:\ThenOrderingOrdering_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"C:\ThenOrderingOrdering_Deep_Time.log");

		[Test]
		public void ThenOrderingOrdering_Random()
		{
			Random random =  new Random();

			for (int i1 = 0; i1 < 10000; i1++)
			{
				int itemCount = random.Next(4, 7);
				int[] orderNums1 = new int[itemCount];
				int[] orderNums2 = new int[itemCount];

				for (int i = 0; i < itemCount; i++)
				{
					orderNums1[i] = random.Next(-1, itemCount - 1);
					orderNums2[i] = random.Next(-1, itemCount - 1);
				}

				test(orderNums1, orderNums2, ListSortDirection.Ascending);
				test(orderNums1, orderNums2, ListSortDirection.Descending);
			}
		}

		[Test]
		public void ThenOrderingThenOrderingOrdering_Random()
		{
			Random random =  new Random();

			for (int i1 = 0; i1 < 10000; i1++)
			{
				int itemCount = random.Next(4, 7);
				//int itemCount = 3;
				int[] orderNums1 = new int[itemCount];
				int[] orderNums2 = new int[itemCount];
				int[] orderNums3 = new int[itemCount];

				for (int i = 0; i < itemCount; i++)
				{
					orderNums1[i] = random.Next(-1, itemCount);
					orderNums2[i] = random.Next(-1, itemCount);
					orderNums3[i] = random.Next(-1, itemCount);
				}

				test(orderNums1, orderNums2, orderNums3, ListSortDirection.Ascending, ListSortDirection.Ascending);
				test(orderNums1, orderNums2, orderNums3, ListSortDirection.Descending, ListSortDirection.Ascending);
				test(orderNums1, orderNums2, orderNums3, ListSortDirection.Ascending, ListSortDirection.Descending);
				test(orderNums1, orderNums2, orderNums3, ListSortDirection.Descending, ListSortDirection.Descending);
			}
		}

		[Test]
		public void ThenOrderingOrdering_Test()
		{
			int[] orderNums1 = {0,0,0};
			int[] orderNums2 = {1,-1,-1};
			test(orderNums1, orderNums2, ListSortDirection.Ascending);
		}

		[Test]
		public void ThenOrderingThenOrderingOrdering_Test()
		{
			int[] orderNums1 = {0,5,1,4,3,3};
			int[] orderNums2 = {0,2,0,3,4,4};
			int[] orderNums3 = {1,-1,3,5,4,5};
			test(orderNums1, orderNums2, orderNums3, ListSortDirection.Ascending, ListSortDirection.Ascending);
		}

		[Test]
		public void ThenOrderingThenOrderingOrdering1_Test()
		{
			int[] orderNums1 = {4,4,0,4,2};
			int[] orderNums2 = {-1,-1,4,3,4};
			int[] orderNums3 = {4,-1,-1,2,-1};

			ObservableCollection<Item> items = getObservableCollection(orderNums1, orderNums2, orderNums3);
			ThenOrdering<Item, int?> ordering3 = items.Ordering(i => i.OrderNum, ListSortDirection.Ascending).ThenOrdering(i => i.OrderNum2, ListSortDirection.Ascending).ThenOrdering(i => i.OrderNum3, ListSortDirection.Ascending).For(consumer);
			items[3] = new Item(null, null);
			ordering3.ValidateConsistency();			
			consumer.Dispose();
		}

		[Test, Combinatorial]
		public void ThenOrderingOrdering_Deep(
			[Values(ListSortDirection.Ascending, ListSortDirection.Descending)] ListSortDirection listSortDirection)
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
					
			for (int v1 = -1; v1 <= 3; v1++)
			{
				test(new []{v1}, listSortDirection);
				for (int v2 = -1; v2 <= 3; v2++)
				{
					test(new []{v1, v2}, listSortDirection);
					for (int v3 = -1; v3 <= 3; v3++)
					{
						test(new []{v1, v2, v3}, listSortDirection);
						for (int v4 = -1; v4 <= 3; v4++)
						{
							test(new []{v1, v2, v3, v4}, listSortDirection);
							counter++;
							_textFileOutputTime.AppentLine($"{stopwatch.Elapsed.TotalMinutes}: {counter}");
						}
					}
				}
			}
		}

		[Test, Combinatorial]
		public void ThenOrderingThenOrderingOrdering_Deep(
			[Values(ListSortDirection.Ascending, ListSortDirection.Descending)] ListSortDirection listSortDirection,
			[Values(ListSortDirection.Ascending, ListSortDirection.Descending)] ListSortDirection listSortDirection1)
		{
			long counter = 0;
			Stopwatch stopwatch = Stopwatch.StartNew();
					
			for (int v1 = -1; v1 <= 2; v1++)
			{
				for (int v2 = -1; v2 <= 2; v2++)
				{
					for (int v3 = -1; v3 <= 2; v3++)
					{
						test(new []{v1}, new []{v2}, new []{v3}, listSortDirection, listSortDirection1);
						for (int v4 = -1; v4 <= 2; v4++)
						{
							for (int v5 = -1; v5 <= 2; v5++)
							{
								for (int v6 = -1; v6 <= 2; v6++)
								{
									test(new []{v1, v2}, new []{v3, v4}, new []{v5, v6}, listSortDirection, listSortDirection1);
									counter++;

									if (counter % 100 == 0)
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

		}

		private bool traceThenOrderingOrdering(string num, int[] orderNums, int[] orderNums2, ListSortDirection listSortDirection, int index, int orderNum, int orderNum2, int indexOld,
			int indexNew)
		{
			string traceString = getTraceString(num, orderNums, orderNums2, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew);

			//if (traceString == "#2. OrderNums1=-1,0,0  OrderNums2=-1,-1,-1  index=1  orderNum=0  orderNum2=0  indexOld=0   indexNew=0 listSortDirection=Ascending")
			//{
			//	return true;
			//}

			return false;

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


		private static ObservableCollection<Item> getObservableCollection(int[] orderNums, int[] orderNums2, int[] orderNums3)
		{
			ObservableCollection<Item> result = new ObservableCollection<Item>();

			for (int i = 0; i < orderNums.Length; i++)
			{
				int? orderNum1 = orderNums[i] >= 0 ? orderNums[i] : (int?)null;
				int? orderNum2 = orderNums2[i] >= 0 ? orderNums2[i] : (int?)null;
				int? orderNum3 = orderNums3[i] >= 0 ? orderNums3[i] : (int?)null;
				result.Add(new Item(orderNum1, orderNum2, orderNum3));
			}

			return result;
		}



		#region ThenOrderingThenOrdering
		private void test(int[] orderNums, int[] orderNums2, int[] orderNums3, ListSortDirection listSortDirection, ListSortDirection listSortDirection2)
		{
			int index = 0;
			int orderNum = 0;
			int orderNum2 = 0;
			int orderNum3 = 0;
			int indexOld = 0;
			int indexNew = 0;
			string testNum = string.Empty;

			ObservableCollection<Item> items;
			Ordering<Item, int?> ordering;
			try
			{
				//trace(testNum = "1", orderNums, orderNums2, orderNums3, listSortDirection, index, orderNum, orderNum2, indexOld, indexNew);
				//items = getObservableCollection(orderNums, orderNums2, orderNums3);
				//ordering = items.Ordering(i => i.OrderNum, listSortDirection);
				//ordering.ValidateConsistency();

				for (index = 0; index < orderNums.Length; index++)
				{
					if (!traceThenOrderingThenOrderingOrdering(testNum = "2", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew)) continue;
					items = getObservableCollection(orderNums, orderNums2, orderNums3);
					ThenOrdering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
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
							for (orderNum3 = 0; orderNum3 <= orderNums.Length; orderNum3++)
							{
								if (!traceThenOrderingThenOrderingOrdering(testNum = "8", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew)) continue;
								items = getObservableCollection(orderNums, orderNums2, orderNums3);
								ThenOrdering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
								items.Insert(index, new Item(orderNum == -1 ? (int?)null : orderNum, orderNum2 == -1 ? (int?)null : orderNum2, orderNum3 == -1 ? (int?)null : orderNum3));
								ordering1.ValidateConsistency();								
								consumer.Dispose();
							}
						}
					}
				}

				for (index = 0; index < orderNums.Length; index++)
				{
					traceThenOrderingThenOrderingOrdering(testNum = "6", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew);
					items = getObservableCollection(orderNums, orderNums2, orderNums3);
					ThenOrdering<Item, int?> ordering3 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
					items[index] = new Item(null, null);
					ordering3.ValidateConsistency();					
					consumer.Dispose();

					for (orderNum = -1; orderNum <= orderNums.Length; orderNum++)
					{
						for (orderNum2 = -1; orderNum2 <= orderNums.Length; orderNum2++)
						{
							for (orderNum3 = -1; orderNum3 <= orderNums.Length; orderNum3++)
							{
								if (!traceThenOrderingThenOrderingOrdering(testNum = "3", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew)) continue;
								items = getObservableCollection(orderNums, orderNums2, orderNums3);
								ThenOrdering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
								items[index] = new Item(orderNum == -1 ? (int?)null : orderNum, orderNum2 == -1 ? (int?)null : orderNum2, orderNum3 == -1 ? (int?)null : orderNum3);
								ordering2.ValidateConsistency();								
								consumer.Dispose();
							}
						}
					}
				}

				for (index = 0; index < orderNums.Length; index++)
				{
					traceThenOrderingThenOrderingOrdering(testNum = "4", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew);
					items = getObservableCollection(orderNums, orderNums2, orderNums3);
					ThenOrdering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
					items[index].OrderNum = null;
					ordering1.ValidateConsistency();					
					consumer.Dispose();

					for (orderNum = -1; orderNum <= orderNums.Length; orderNum++)
					{
						for (orderNum2 = -1; orderNum2 <= orderNums.Length; orderNum2++)
						{
							for (orderNum3 = -1; orderNum3 <= orderNums.Length; orderNum3++)
							{
								if (!traceThenOrderingThenOrderingOrdering(testNum = "7", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew)) continue;
								items = getObservableCollection(orderNums, orderNums2, orderNums3);
								ThenOrdering<Item, int?> ordering3 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
								items[index].OrderNum = orderNum == -1 ? (int?)null : orderNum;
								items[index].OrderNum2 = orderNum2 == -1 ? (int?)null : orderNum2;
								items[index].OrderNum3 = orderNum2 == -1 ? (int?)null : orderNum3;
								ordering3.ValidateConsistency();								
								consumer.Dispose();
							}
						}

						traceThenOrderingThenOrderingOrdering(testNum = "5", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew);
						items = getObservableCollection(orderNums, orderNums2, orderNums3);
						ThenOrdering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
						items[index].OrderNum = orderNum == -1 ? (int?)null : orderNum;
						ordering2.ValidateConsistency();						
						consumer.Dispose();

						if (!traceThenOrderingThenOrderingOrdering(testNum = "9", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew)) continue;
						items = getObservableCollection(orderNums, orderNums2, orderNums3);
						ThenOrdering<Item, int?> ordering4 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
						items[index].OrderNum2 = orderNum == -1 ? (int?)null : orderNum;
						ordering4.ValidateConsistency();						
						consumer.Dispose();
					}
				}

				for (indexOld = 0; indexOld < orderNums.Length; indexOld++)
				{
					for (indexNew = 0; indexNew < orderNums.Length; indexNew++)
					{
						if (!traceThenOrderingThenOrderingOrdering(testNum = "6", orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew)) continue;
						items = getObservableCollection(orderNums, orderNums2, orderNums3);
						ThenOrdering<Item, int?> ordering2 = items.Ordering(i => i.OrderNum, listSortDirection).ThenOrdering(i => i.OrderNum2, listSortDirection).ThenOrdering(i => i.OrderNum3, listSortDirection2).For(consumer);
						items.Move(indexOld, indexNew);
						ordering2.ValidateConsistency();						
						consumer.Dispose();
					}
				}
			}
			catch (Exception e)
			{
				string traceString = getTraceString(testNum, orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew);
				_textFileOutputLog.AppentLine(traceString);
				_textFileOutputLog.AppentLine(e.Message);
				_textFileOutputLog.AppentLine(e.StackTrace);
				throw new Exception(traceString, e);
			}

		}

		private bool traceThenOrderingThenOrderingOrdering(string num, int[] orderNums, int[] orderNums2, int[] orderNums3, ListSortDirection listSortDirection, ListSortDirection listSortDirection2, int index, int orderNum, int orderNum2, int orderNum3, int indexOld,
			int indexNew)
		{
			string traceString = getTraceString(num, orderNums, orderNums2, orderNums3, listSortDirection, listSortDirection2, index, orderNum, orderNum2, orderNum3, indexOld, indexNew);

            if (traceString == "#8. OrderNums1=-1  OrderNums2=-1  OrderNums3=-1  index=0  orderNum=0  orderNum2=0  orderNum3=0  indexOld=0   indexNew=0 listSortDirection=Ascending   listSortDirection2=Ascending")
            {
                return true;
            }

            return false;

            return true;
		}

		private static string getTraceString(string num, int[] orderNums, int[] orderNums2, int[] orderNums3, ListSortDirection listSortDirection, ListSortDirection listSortDirection2, int index, int orderNum, int orderNum2, int orderNum3, int indexOld, int indexNew)
		{
			return string.Format(
				"#{11}. OrderNums1={0}  OrderNums2={1}  OrderNums3={2}  index={3}  orderNum={4}  orderNum2={5}  orderNum3={6}  indexOld={7}   indexNew={8} listSortDirection={9}   listSortDirection2={10}",
				string.Join(",", orderNums),
				string.Join(",", orderNums2),
				string.Join(",", orderNums3),
				index,
				orderNum,
				orderNum2,
				orderNum3,
				indexOld,
				indexNew,
				listSortDirection,
				listSortDirection2,
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

		[Test]
		public void TestChangeSortDirection()
		{
			ObservableCollection<Item> items = getObservableCollection(new[] { 0, 2, 3, 7, 5, 2, 4, 7, 5, 6, 1, 5 },
																	   new[] { 0, 2, 3, 8, 1, 2, 4, 8, 2, 6, 1, 3 },
																	   new[] { 0, 2, 3, 7, 5, 2, 4, 0, 5, 6, 1, 5 });

			Scalar<ListSortDirection> listSortDirectionScalar = new Scalar<ListSortDirection>(ListSortDirection.Ascending);
			Ordering<Item, int?> ordering = items.Ordering(i => i.OrderNum, listSortDirectionScalar).For(consumer);
			ThenOrdering<Item, int?> ordering1 = items.Ordering(i => i.OrderNum, listSortDirectionScalar).ThenOrdering(i => i.OrderNum2, listSortDirectionScalar).For(consumer);
			listSortDirectionScalar.Change(ListSortDirection.Descending);
			ordering.ValidateConsistency();
			ordering1.ValidateConsistency();
            consumer.Dispose();
		}

		#endregion
	}
}
