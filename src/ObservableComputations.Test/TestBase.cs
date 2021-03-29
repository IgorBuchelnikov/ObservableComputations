using System.ComponentModel;
using System.Linq;

namespace ObservableComputations.Test
{
	public class TestBase
	{
		public const string MinimalTestsToCoverFileName = "MinimalTestsToCover.generated.cs";

#if GeneratingMinimalTestsToCover
		public static bool _firstTestClass = true;
#endif	

		bool _debug;
		public TestBase(bool debug)
		{
			_debug = debug;

			OcConfiguration.SaveInstantiationStackTrace = _debug;
			OcConfiguration.TrackComputingsExecutingUserCode = _debug;
			OcConfiguration.SaveOcDispatcherInvocationInstantiationStackTrace = _debug;

			if (!debug)
			{
#if GeneratingMinimalTestsToCover
				if (GetType().Name != nameof(QuickTests))
				{
					if (!_firstTestClass)
					{
						File.AppendAllText(TestBase.MinimalTestsToCoverFileName,
$@"
		}}
	}}	
"						);
					}
					else
					{
						_firstTestClass = false;
					}

					File.AppendAllText(TestBase.MinimalTestsToCoverFileName,
$@"
	public partial class {GetType().Name}
	{{
		[Test]
		public void MinimalTestToCover()
		{{
"					);
				}
#endif				
			}


		}

		int _lastVisitsTotal= 0;
		protected void writeUsefulTest(string test)
		{
#if GeneratingMinimalTestsToCover
			if (AltCover.Monitor.TryGetVisitTotals(out PointCount pointCount))
			{
				if (_lastVisitsTotal < pointCount.Code)
				{
					_lastVisitsTotal = pointCount.Code;
					if (!_debug) File.AppendAllLines(TestBase.MinimalTestsToCoverFileName, new []{test});
				}
			}
#endif
		}

		protected string getTestString(int[] values)
		{
			return $"			test(new int[]{{{string.Join(", ", values.Select(v => v.ToString()))}}});";
		}

		protected string getTestString(int[] ids1, int[] ids2)
		{
			return $"			test(new int[]{{{string.Join(", ", ids1.Select(v => v.ToString()))}}}, new int[]{{{string.Join(", ", ids2.Select(v => v.ToString()))}}});";
		}

		protected string getTestString(int[] ids1, MinimazingOrMaximazingMode mode)
		{
			return $"			test(new int[]{{{string.Join(", ", ids1.Select(v => v.ToString()))}}}, MinimazingOrMaximazingMode.{mode});";
		}


		protected string getTestString(int[] ids1, ListSortDirection mode)
		{
			return $"			test(new int[]{{{string.Join(", ", ids1.Select(v => v.ToString()))}}}, ListSortDirection.{mode});";
		}

		protected string getTestString(int[] orderNums, int[] orderNums2, ListSortDirection listSortDirection)
		{
			return $"			test(new int[]{{{string.Join(", ", orderNums.Select(v => v.ToString()))}}}, new int[]{{{string.Join(", ", orderNums2.Select(v => v.ToString()))}}}, ListSortDirection.{listSortDirection});";
		}

		protected string getTestString(int[] orderNums, int[] orderNums2, int[] orderNums3, ListSortDirection listSortDirection, ListSortDirection listSortDirection2)
		{
			return $"			test(new int[]{{{string.Join(", ", orderNums.Select(v => v.ToString()))}}}, new int[]{{{string.Join(", ", orderNums2.Select(v => v.ToString()))}}}, new int[]{{{string.Join(", ", orderNums3.Select(v => v.ToString()))}}}, ListSortDirection.{listSortDirection}, ListSortDirection.{listSortDirection2});";
		}

		protected string getTestString(int count)
		{
			return $"			test({count});";
		}

		protected string getTestString(int count1, int count2)
		{
			return $"			test({count1}, {count2});";
		}

	}
}
