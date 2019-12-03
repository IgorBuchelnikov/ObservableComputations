using System.ComponentModel;
using NUnit.Framework;

namespace ObservableComputations.Test
{
	[TestFixture]
	class SequenceTests
	{
		class Count : INotifyPropertyChanged
		{
			private int _countValue;

			public int CountValue
			{
				get => _countValue;
				set
				{
					_countValue = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CountValue)));
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;
		}

		[Test, Combinatorial]
		public void SequenceTest(
			[Range(0, 5, 1)] int initialCount,
			[Values(-1, 1)] int increment)
		{
			Count countInstance = new Count();
			int count = initialCount;
			countInstance.CountValue = count;
			SequenceComputing sequenceComputing = Expr.Is(() => countInstance.CountValue).Computing().SequenceComputing();
			sequenceComputing.ValidateConsistency();
					
			void test()
			{
				do
				{
					countInstance.CountValue = count;
					sequenceComputing.ValidateConsistency();
					count = count + increment;
				} while (count >= 0 && count <= 4);
			}

			test();

			increment = increment * -1;
			count = count + increment;

			test();

		}
	}
}
