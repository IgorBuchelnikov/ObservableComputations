using System.ComponentModel;

namespace ObservableComputations.Test
{
		public class Scalar<TValue> : IReadScalar<TValue>
		{
			public Scalar(TValue value)
			{
				Value = value;
			}

			public event PropertyChangedEventHandler PropertyChanged;
			public TValue Value { get; private set;}

			public void Touch()
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}

			public void Change(TValue newValue)
			{
				Value = newValue;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}
}
