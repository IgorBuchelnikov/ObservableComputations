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
	[TestFixture]
	public class PagingTests
	{
		public class Item 
		{
			public Item()
			{
				Num = LastNum;
				LastNum++;
			}

			public static int LastNum;
			public int Num;


			#region Overrides of Object

			public override string ToString()
			{
				return $" Num = {Num}";
			}

			#endregion
		}

		TextFileOutput _textFileOutputLog = new TextFileOutput(@"D:\Projects\NevaPolimer\Paging_Deep.log");
		TextFileOutput _textFileOutputTime = new TextFileOutput(@"D:\Projects\NevaPolimer\Paging_Deep_Time.log");

		[Test, Combinatorial]
		public void Paging_Deep()
		{
			for (int count = 0; count < 10; count++)
			{
				test(count);
			}		
		}

		private void test(int count)
		{
			int index = 0;
			int indexOld = 0;
			int indexNew = 0;
			int pageSize = 0;
			int currentPage = 1;
			string testNum = string.Empty;

			ObservableCollection<Item> items;
			Paging<Item> paging;
			try
			{
				for (pageSize = 1; pageSize <= count; pageSize++)
				{
					for (currentPage = 1; currentPage < count; currentPage++)
					{
						trace(testNum = "2", count, index, indexOld, indexNew, pageSize, currentPage);
						items = getObservableCollection(count);
						paging = items.Paging(pageSize);
						paging.ValidateConsistency();

						trace(testNum = "3", count, index, indexOld, indexNew, pageSize, currentPage);
						items = getObservableCollection(count);
						paging = items.Paging(pageSize);
						paging.CurrentPage = currentPage;
						paging.ValidateConsistency();

						for (index = 0; index < count; index++)
						{
							trace(testNum = "4", count, index, indexOld, indexNew, pageSize, currentPage);
							items = getObservableCollection(count);
							Paging<Item> paging1 = items.Paging(pageSize, currentPage);
							items.RemoveAt(index);
							paging1.ValidateConsistency();
						}

						for (index = 0; index < count; index++)
						{
							trace(testNum = "5", count, index, indexOld, indexNew, pageSize, currentPage);
							items = getObservableCollection(count);
							Paging<Item> paging1 = items.Paging(pageSize, currentPage);
							items.Insert(index, new Item());
							paging1.ValidateConsistency();
						}

						for (index = 0; index < count; index++)
						{
							trace(testNum = "6", count, index, indexOld, indexNew, pageSize, currentPage);
							items = getObservableCollection(count);
							Paging<Item> paging1 = items.Paging(pageSize, currentPage);
							items[index] = new Item();
							paging1.ValidateConsistency();
						}

						for (indexOld = 0; indexOld < count; indexOld++)
						{
							for (indexNew = 0; indexNew < count; indexNew++)
							{
								trace(testNum = "7", count, index, indexOld, indexNew, pageSize, currentPage);
								items = getObservableCollection(count);
								Paging<Item> paging1 = items.Paging(pageSize, currentPage);
								items.Move(indexOld, indexNew);
								paging1.ValidateConsistency();
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				string traceString = getTraceString(testNum, count, index, indexOld, indexNew, pageSize, currentPage);
				_textFileOutputLog.AppentLine(traceString);
				_textFileOutputLog.AppentLine(e.Message);
				_textFileOutputLog.AppentLine(e.StackTrace);
				throw new Exception(traceString, e);
			}

		}

		private void trace(string num, int count,  int index, int indexOld,
			int indexNew, int pageSize, int currentPage)
		{
			string traceString = getTraceString(num, count, index, indexOld, indexNew, pageSize, currentPage);

			if (traceString == "#3.   count=4    index=4  indexOld=4   indexNew=4    pageSize=3   currentPage=2")
			{
				
			}
		}

		private static string getTraceString(string num, int count, int index, int indexOld, int indexNew, int pageSize, int currentPage)
		{
			return string.Format(
				"#{4}.   count={0}    index={1}  indexOld={2}   indexNew={3}    pageSize={5}   currentPage={6}",
				count,
				index,
				indexOld,
				indexNew,
				num,
				pageSize,
				currentPage);
		}


		private static ObservableCollection<Item> getObservableCollection(int count)
		{
			Item.LastNum = 0;
			return new ObservableCollection<Item>(Enumerable.Range(1, count).Select(orderNum => new Item()));
		}


		[Test]
		public void PagingTest_01()
		{
			ObservableCollection<int> items = new ObservableCollection<int>(Enumerable.Range(1, 100));

			Paging<int> paging = items.Paging(2);
			paging.ValidateConsistency();

			void test()
			{
				paging.ValidateConsistency();

				if (items != null)
				{		
					items.Insert(2, 1);
					paging.ValidateConsistency();
					items[3] = 2;
					paging.ValidateConsistency();
					items.RemoveAt(3);
					paging.ValidateConsistency();
					items.Move(1, 3);
					paging.ValidateConsistency();
					items.RemoveAt(0);
					paging.ValidateConsistency();
					items.RemoveAt(0);
					paging.ValidateConsistency();
					items.RemoveAt(0);
					paging.ValidateConsistency();
					items.RemoveAt(0);
					paging.ValidateConsistency();
					items.Insert(0, 2);
					paging.ValidateConsistency();
					items.Insert(0, 3);
					paging.ValidateConsistency();
				}

				if (items != null)
				{
					items.Clear();
					paging.ValidateConsistency();
				}

				if (items != null)
				{
					foreach (int i in Enumerable.Range(1, 100))
					{
						items.Add(i);
					}
					paging.ValidateConsistency();
				}
			}

			paging.CurrentPage = 2;
			test();

			paging.CurrentPage = 3;
			test();

			paging.CurrentPage = 3;
			test();

			paging.PageSize = 10;

			paging.CurrentPage = 1;
			test();

			paging.CurrentPage = 3;
			test();

			paging.CurrentPage = 3;
			test();
		}

		[Test]
		public void PagingTest_02()
		{
			Scalar<ObservableCollection<int>> items = new Scalar<ObservableCollection<int>>(null);
			Paging<int> paging = items.Paging(2);
			paging.ValidateConsistency();	
						
			items.Change(new ObservableCollection<int>(Enumerable.Range(1, 100)));

			void test()
			{
				paging.ValidateConsistency();
				var sourceScalarValue = items.Value;

				if (sourceScalarValue != null)
				{		
					sourceScalarValue.Insert(2, 1);
					paging.ValidateConsistency();
					sourceScalarValue[3] = 2;
					paging.ValidateConsistency();
					sourceScalarValue.RemoveAt(3);
					paging.ValidateConsistency();
					sourceScalarValue.Move(1, 3);
					paging.ValidateConsistency();
					sourceScalarValue.RemoveAt(0);
					paging.ValidateConsistency();
					sourceScalarValue.RemoveAt(0);
					paging.ValidateConsistency();
					sourceScalarValue.RemoveAt(0);
					paging.ValidateConsistency();
					sourceScalarValue.RemoveAt(0);
					paging.ValidateConsistency();
					sourceScalarValue.Insert(0, 2);
					paging.ValidateConsistency();
					sourceScalarValue.Insert(0, 3);
					paging.ValidateConsistency();
				}

				if (sourceScalarValue != null)
				{
					sourceScalarValue.Clear();
					paging.ValidateConsistency();
				}

				if (sourceScalarValue != null)
				{
					foreach (int i in Enumerable.Range(1, 100))
					{
						sourceScalarValue.Add(i);
					}
					paging.ValidateConsistency();
				}
			}

			paging.CurrentPage = 2;
			test();

			paging.CurrentPage = 3;
			test();

			paging.CurrentPage = 3;
			test();

			paging.PageSize = 10;
			test();

			paging.CurrentPage = 1;
			test();

			paging.CurrentPage = 3;
			test();

			paging.CurrentPage = 4;
			test();
		}
	}
}
