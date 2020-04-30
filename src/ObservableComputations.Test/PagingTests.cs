using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	public class PagingTests
	{
		[Test]
		public void PagingTest_01()
		{
			ObservableCollection<int> items = new ObservableCollection<int>(Enumerable.Range(1, 100));

			Paging<int> paging = items.Paging();
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

			paging.CurrentPageNum = 2;
			test();

			paging.CurrentPageNum = 3;
			test();

			paging.CurrentPageNum = 3;
			test();

			paging.PageSize = 10;

			paging.CurrentPageNum = 1;
			test();

			paging.CurrentPageNum = 3;
			test();

			paging.CurrentPageNum = 3;
			test();
		}

		[Test]
		public void PagingTest_02()
		{
			Scalar<ObservableCollection<int>> items = new Scalar<ObservableCollection<int>>(null);
			Paging<int> paging = items.Paging();
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

			paging.CurrentPageNum = 2;
			test();

			paging.CurrentPageNum = 3;
			test();

			paging.CurrentPageNum = 3;
			test();

			paging.PageSize = 10;
			test();

			paging.CurrentPageNum = 1;
			test();

			paging.CurrentPageNum = 3;
			test();

			paging.CurrentPageNum = 4;
			test();
		}

	}
}
